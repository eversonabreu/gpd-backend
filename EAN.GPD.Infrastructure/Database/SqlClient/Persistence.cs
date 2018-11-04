using Npgsql;
using System;
using System.Collections.Generic;

namespace EAN.GPD.Infrastructure.Database.SqlClient
{
    internal class Persistence : IPersistence
    {
        protected string Command { get; set; }
        private readonly Dictionary<string, object> parameters;

        public Persistence(string command)
        {
            parameters = new Dictionary<string, object>();
            Command = command;
        }

        public void AddParameter(string name, object value) => parameters.Add(name, value);

        public virtual void Execute()
        {
            Exception error = null;
            using (Connection connection = new Connection())
            {
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand(Command, connection.GetConnection(true), connection.GetTransaction()))
                {
                    foreach (var param in parameters.Keys)
                    {
                        sqlCommand.Parameters.AddWithValue(param, parameters[param]);
                    }

                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                        connection.Commit();
                    }
                    catch (Exception exc)
                    {
                        error = exc;
                    }
                }
            }

            if (error != null)
            {
                throw error;
            }
        }
    }
}
