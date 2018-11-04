using System.Collections.Generic;

namespace EAN.GPD.Infrastructure.Database.SqlClient
{
    internal class PersistenceEntity : Persistence, IPersistenceEntity
    {
        private readonly TypePersistence typePersistence;
        private readonly string nameTable;
        private readonly string namePrimaryKey;
        private readonly long valuePrimaryKey;
        private readonly Dictionary<string, object> columns;

        public PersistenceEntity(TypePersistence typePersistence, string nameTable, string namePrimaryKey, long valuePrimaryKey) : base(string.Empty)
        {
            this.typePersistence = typePersistence;
            this.nameTable = nameTable;
            this.namePrimaryKey = namePrimaryKey;
            this.valuePrimaryKey = valuePrimaryKey;
            columns = new Dictionary<string, object>();
        }

        public void AddColumn(string name, object value) => columns.Add(name, value);

        private string GetCommand()
        {
            var result = string.Empty;

            switch (typePersistence)
            {
                case TypePersistence.Update:
                    {
                        result = $"UPDATE {nameTable} SET ";
                        foreach (var pars in columns.Keys)
                        {
                            result += pars + " = :P_" + pars + ",";
                            base.AddParameter("P_" + pars, columns[pars]);
                        }
                        result = result.Remove(result.Length - 1);
                        result += $" WHERE {namePrimaryKey} = {valuePrimaryKey}";

                        return result;
                    }

                case TypePersistence.Insert:
                    {
                        result = $"INSERT INTO {nameTable} ({namePrimaryKey}, ";
                        foreach (var parametro in columns.Keys)
                        {
                            result += parametro + ",";
                            base.AddParameter("P_" + parametro, columns[parametro]);
                        }
                        result = result.Remove(result.Length - 1);
                        result += $") VALUES ({valuePrimaryKey},";
                        foreach (var parametro in columns.Keys)
                        {
                            result += ":P_" + parametro + ",";
                        }
                        result = result.Remove(result.Length - 1);
                        result += ")";

                        return result;
                    }

                default: return $"DELETE FROM {nameTable} WHERE {namePrimaryKey} = {valuePrimaryKey}";
            }
        }

        public override void Execute()
        {
            Command = GetCommand();
            base.Execute();
        }
    }
}
