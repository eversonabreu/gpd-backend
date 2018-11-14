using EAN.GPD.Infrastructure.Database.SqlClient;
using System;

namespace EAN.GPD.Infrastructure.Database
{
    public static class DatabaseProvider
    {
        public static string ConnectionString;

        public static IQuery NewQuery(string command) => new Query(command);

        public static IPersistence NewPersistence(string command) => new Persistence(command);

        public static IPersistenceEntity NewPersistenceEntity(TypePersistence typePersistence, string nameTable, string namePrimaryKey, long valuePrimaryKey)
        {
            return new PersistenceEntity(typePersistence, nameTable, namePrimaryKey, valuePrimaryKey);
        }

        public static long NewSequence(string nameSequence)
        {
            var query = NewQuery($"SELECT NEXTVAL('{nameSequence}') AS \"SEQ\"");
            query.ExecuteQuery();
            return query.GetLong("SEQ");
        }

        public static IQuery GetAllById(string nameTable, string namePrimaryKey, long valuePrimaryKey)
        {
            var query = NewQuery($"SELECT * FROM {nameTable} WHERE {namePrimaryKey} = {valuePrimaryKey}");
            query.ExecuteQuery();

            if (query.IsNotEmpty)
            {
                return query;
            }

            throw new Exception("Nenhum resultado foi obtido para o 'ID' informado.");
        }

        public static bool ExistsTable(string nameTable)
        {
            try
            {
                var query = NewQuery($"SELECT 1 FROM {nameTable} WHERE 1 = 2");
                query.ExecuteQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
