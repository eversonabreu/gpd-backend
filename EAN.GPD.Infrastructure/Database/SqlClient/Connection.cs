using Npgsql;
using System;
using System.Data;

namespace EAN.GPD.Infrastructure.Database.SqlClient
{
    internal class Connection : IDisposable
    {
        private NpgsqlTransaction transaction;
        private NpgsqlConnection connection;

        public NpgsqlConnection GetConnection(bool startTransaction = false)
        {
            connection = new NpgsqlConnection(DatabaseProvider.ConnectionString);
            connection.Open();

            if (startTransaction)
            {
                transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            }

            return connection;
        }

        public NpgsqlTransaction GetTransaction()
        {
            if (transaction is null)
            {
                throw new Exception("Transaction is not started.");
            }

            return transaction;
        }

        public void Commit()
        {
            if (transaction is null)
            {
                throw new Exception("Transaction is not started.");
            }

            transaction.Commit();
            while (!transaction.IsCompleted) continue;
            transaction.Dispose();
            transaction = null;
        }

        public void RollBack()
        {
            transaction?.Rollback();
            transaction?.Dispose();
            transaction = null;
        }

        public void Dispose()
        {
            try
            {
                if (connection != null)
                {
                    RollBack();
                    connection.Close();
                    NpgsqlConnection.ClearPool(connection);
                    connection.Dispose();
                    connection = null;
                }
            }
            catch (Exception exc)
            {
                Console.Out.WriteLine(exc);
            }
        }
    }
}
