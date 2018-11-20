using EAN.GPD.Infrastructure.Database;
using EAN.GPD.Infrastructure.Database.SqlClient;
using System;
using System.Linq;
using System.Reflection;

namespace EAN.GPD.Domain.Entities
{
    public class BaseEntity
    {
        private long? id;
        private readonly string nameTable;

        public BaseEntity(string nameTable, long? id = null)
        {
            this.nameTable = nameTable;
            this.id = id;

            if (id.HasValue)
            {
                LoadObject();
            }
        }

        public string GetNameTable() => nameTable;
        public string GetNameSequence() => $"Seq{nameTable}";
        public string GetNamePrimaryKey() => $"Id{nameTable}";

        private void LoadObject()
        {
            var data = DatabaseProvider.GetAllById(nameTable, GetNamePrimaryKey(), id.Value).GetAll().FirstOrDefault();

            if (data is null || !data.Any())
            {
                throw new Exception($"No result for query by id in '{nameTable}' value id '{id.Value}'.");
            }

            var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var column = prop.GetCustomAttribute<Column>();
                if (column != null)
                {
                    string name = (column.Name ?? prop.Name).Trim().ToUpper();
                    if (data.ContainsKey(name))
                    {
                        object value = data[name];

                        if (prop.PropertyType == typeof(bool))
                        {
                            prop.SetValue(this, Convert.ToChar(value) == 'S');
                        }
                        else
                        {
                            prop.SetValue(this, value == DBNull.Value ? null : value);
                        }
                    }
                }
            }

            foreach (var prop in props)
            {
                var joinColumn = prop.GetCustomAttribute<JoinColumn>();
                if (joinColumn != null)
                {
                    string name = joinColumn.NameColumnReference.Trim().ToUpper();
                    if (data.ContainsKey(name))
                    {
                        object value = data[name];
                        if (value != DBNull.Value)
                        {
                            var newObjectJoin = Activator.CreateInstance(prop.PropertyType, Convert.ToInt64(value));
                            prop.SetValue(this, newObjectJoin);
                        }
                    }
                }
            }
        }

        private void SetPersistenceEntity(IPersistenceEntity persistenceEntity)
        {
            var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var column = prop.GetCustomAttribute<Column>();
                if (column != null)
                {
                    object value = prop.GetValue(this);
                    string name = column.Name ?? prop.Name;
                    if (prop.PropertyType == typeof(string))
                    {
                        if (value is null)
                        {
                            if (column.StringNotNullable)
                            {
                                throw new Exception($"String nullable in property column: '{prop.Name}'.");
                            }
                            persistenceEntity.AddColumn(name, DBNull.Value);
                        }
                        else
                        {
                            if (column.StringMaxLenght > 0)
                            {
                                int maxLenghtString = value.ToString().Length;
                                if (maxLenghtString > column.StringMaxLenght)
                                {
                                    throw new Exception($"String max capacity lenght in property column: '{prop.Name}'.");
                                }
                            }
                            persistenceEntity.AddColumn(name, value);
                        }
                    }
                    else
                    {
                        if (value is null)
                        {
                            persistenceEntity.AddColumn(name, DBNull.Value);
                        }
                        else
                        {
                            if (column.GetType() == typeof(bool))
                            {
                                persistenceEntity.AddColumn(name, Convert.ToBoolean(value) ? 'S' : 'N');
                            }
                            else
                            {
                                persistenceEntity.AddColumn(name, value);
                            }
                        }
                    }
                }
            }
        }

        public long Save()
        {
            long result = id ?? GetSequence();
            
            if (id.HasValue)
            {
                var persistence = DatabaseProvider.NewPersistenceEntity(TypePersistence.Update, nameTable, GetNamePrimaryKey(), result);
                SetPersistenceEntity(persistence);
                persistence.Execute();
            }
            else
            {
                var persistence = DatabaseProvider.NewPersistenceEntity(TypePersistence.Insert, nameTable, GetNamePrimaryKey(), result);
                SetPersistenceEntity(persistence);
                persistence.Execute();
                id = result;
            }

            return result;
        }

        private long GetSequence() => DatabaseProvider.NewSequence(GetNameSequence());

        public static void Delete<TEntity>(long id) where TEntity : BaseEntity
        {
            var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            string nameTable = entity.GetNameTable();
            string namePrimaryKey = entity.GetNamePrimaryKey();
            var persistence = DatabaseProvider.NewPersistenceEntity(TypePersistence.Delete, nameTable, namePrimaryKey, id);
            persistence.Execute();
        }

        public static long GetSequence<TEntity>() where TEntity : BaseEntity
        {
            var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
            return DatabaseProvider.NewSequence(entity.GetNameSequence());
        }
    }
}