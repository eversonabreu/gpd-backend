using EAN.GPD.Domain.Entities;
using EAN.GPD.Domain.Utils;
using EAN.GPD.Infrastructure.Database;
using EAN.GPD.Infrastructure.Database.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAN.GPD.Domain.Repositories
{
    public enum TypeAudit
    {
        Insert = 1,
        Update = 2,
        Delete = 3
    }

    internal class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        public IEnumerable<TEntity> Filter(string whereExpression = null)
        {
            var objData = (TEntity)Activator.CreateInstance(typeof(TEntity));
            string where = string.IsNullOrWhiteSpace(whereExpression) ? "1 = 1" : whereExpression;
            var query = DatabaseProvider.NewQuery($"select {objData.GetNamePrimaryKey()} id from {objData.GetNameTable()} where {where}");
            query.ExecuteQuery();
            var result = new List<TEntity>();

            query.ForEach(() =>
            {
                var obj = Activator.CreateInstance(typeof(TEntity), query.GetLong("ID"));
                result.Add((TEntity)obj);
            });

            return result;
        }

        private int CountRecords(string whereExpression = null)
        {
            var objData = (TEntity)Activator.CreateInstance(typeof(TEntity));
            string where = string.IsNullOrWhiteSpace(whereExpression) ? "1 = 1" : whereExpression;
            var query = DatabaseProvider.NewQuery($"select count(1) qtd from {objData.GetNameTable()} where {where}");
            query.ExecuteQuery();
            return query.GetInt("QTD");
        }

        public (IEnumerable<TEntity>, int) Filter(uint page = 0, uint count = 10, string whereExpression = null)
        {
            int countRecords = CountRecords(whereExpression);
            string paginate = $"limit {count} offset ({page} * {count})";
            string where = string.IsNullOrWhiteSpace(whereExpression) ? $"1 = 1 {paginate}" : $"{whereExpression} {paginate}";
            return (Filter(where), countRecords);
        }

        public TEntity Find(string whereExpression)
        {
            if (string.IsNullOrWhiteSpace(whereExpression))
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            var objData = (TEntity)Activator.CreateInstance(typeof(TEntity));
            var query = DatabaseProvider.NewQuery($"select {objData.GetNamePrimaryKey()} id from {objData.GetNameTable()} where {whereExpression} limit 1");
            query.ExecuteQuery();
            if (query.IsNotEmpty)
            {
                return (TEntity)Activator.CreateInstance(typeof(TEntity), query.GetLong("ID"));
            }

            return null;
        }

        public TEntity GetOne(long id) => (TEntity)Activator.CreateInstance(typeof(TEntity), id);

        public async Task GenerateAuditAsync(TypeAudit typeAudit, string nameTable, long idTabela, UserLogged userLogged, string entity = null)
        {
            long sequence = DatabaseProvider.NewSequence("SEQ_AUDITORIA");
            switch (typeAudit)
            {
                case TypeAudit.Insert: await CreateAuditAsync(sequence, nameTable, idTabela, userLogged); break;
                case TypeAudit.Update: await UpdateAuditAsync(sequence, nameTable, idTabela, userLogged, entity); break;
                case TypeAudit.Delete: await DeleteAuditAsync(sequence, nameTable, idTabela, userLogged, entity); break;
                default: throw new Exception("Tipo de auditoria inválido.");
            }
        }

        private async Task CreateAuditAsync(long sequence, string nameTable, long idTabela, UserLogged userLogged)
        {
            await Task.Run(() =>
            {
                var insert = DatabaseProvider.NewPersistenceEntity(TypePersistence.Insert, "Auditoria", "IdAuditoria", sequence);
                insert.AddColumn("Tabela", nameTable);
                insert.AddColumn("IdTabela", idTabela);
                insert.AddColumn("TipoAuditoria", 1);
                insert.AddColumn("DataCriacao", DateTime.Now);
                insert.AddColumn("Cpf", userLogged.Cpf);
                if (!string.IsNullOrWhiteSpace(userLogged.Matricula))
                {
                    insert.AddColumn("Matricula", userLogged.Matricula);
                }
                insert.Execute();
            });
        }

        private async Task UpdateAuditAsync(long sequence, string nameTable, long idTabela, UserLogged userLogged, string entity)
        {
            var update = DatabaseProvider.NewPersistenceEntity(TypePersistence.Insert, "Auditoria", "IdAuditoria", sequence);
            update.AddColumn("Tabela", nameTable);
            update.AddColumn("IdTabela", idTabela);
            update.AddColumn("TipoAuditoria", 2);
            update.AddColumn("DataCriacao", DateTime.Now);
            update.AddColumn("Cpf", userLogged.Cpf);
            if (!string.IsNullOrWhiteSpace(userLogged.Matricula))
            {
                update.AddColumn("Matricula", userLogged.Matricula);
            }
            update.AddColumn("Objeto", entity);
            update.Execute();
            await RemoveUpdatesAuditAsync(nameTable, idTabela);
        }

        private async Task DeleteAuditAsync(long sequence, string nameTable, long idTabela, UserLogged userLogged, string entity)
        {
            var delete = DatabaseProvider.NewPersistenceEntity(TypePersistence.Insert, "Auditoria", "IdAuditoria", sequence);
            delete.AddColumn("Tabela", nameTable);
            delete.AddColumn("IdTabela", idTabela);
            delete.AddColumn("TipoAuditoria", 3);
            delete.AddColumn("DataCriacao", DateTime.Now);
            delete.AddColumn("Cpf", userLogged.Cpf);
            if (!string.IsNullOrWhiteSpace(userLogged.Matricula))
            {
                delete.AddColumn("Matricula", userLogged.Matricula);
            }
            delete.AddColumn("Objeto", entity);
            delete.Execute();
            await RemoveAllUpdatesAuditAsync(nameTable, idTabela);
        }

        private async Task RemoveAllUpdatesAuditAsync(string nameTable, long idTabela)
        {
            await Task.Run(() =>
            {
                DatabaseProvider.NewPersistence($"delete from auditoria where idtabela = {idTabela} and tabela = '{nameTable}' and tipoauditoria = 2").Execute();
            });
        }

        private async Task RemoveUpdatesAuditAsync(string nameTable, long idTabela)
        {
            await Task.Run(() =>
            {
                string sql = $"select idauditoria from auditoria where idtabela = {idTabela} and tabela = '{nameTable}' and tipoauditoria = 2 order by datacriacao desc";
                var select = DatabaseProvider.NewQuery(sql);
                select.ExecuteQuery();
                if (select.Count >= 5)
                {
                    for (int index = 3; index < select.Count; index++)
                    {
                        long idAuditoria = Convert.ToInt64(select.GetAll()[index]["IDAUDITORIA"]);
                        DatabaseProvider.NewPersistence($"delete from auditoria where idAuditoria = {idAuditoria}").Execute();
                    }
                }
            });
        }
    }
}
