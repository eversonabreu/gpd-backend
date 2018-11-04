using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EAN.GPD.Infrastructure.Database.SqlClient
{
    internal class Query : IQuery
    {
        private readonly string command;
        private readonly Dictionary<string, object> parameters;
        private readonly List<Dictionary<string, object>> records;
        private int record;

        public Query(string command)
        {
            parameters = new Dictionary<string, object>();
            records = new List<Dictionary<string, object>>();
            this.command = command;
        }

        public void AddParameter(string name, object value) => parameters.Add(name, value);

        public void ExecuteQuery()
        {
            record = -1;
            using (Connection connection = new Connection())
            {
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand(command, connection.GetConnection()))
                {
                    foreach (var param in parameters.Keys)
                    {
                        sqlCommand.Parameters.AddWithValue(param, parameters[param]);
                    }

                    using (NpgsqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int it = 0; it < sqlDataReader.FieldCount; it++)
                            {
                                string name = (sqlDataReader.GetName(it) ?? string.Empty).Trim().ToUpper();
                                if (row.ContainsKey(name) || name == string.Empty)
                                {
                                    name = "COLUNA_" + it.ToString();
                                }
                                row.Add(name, sqlDataReader.GetValue(it));
                            }

                            records.Add(row);
                        }

                        if (records.Any())
                        {
                            record = 0;
                        }
                    }
                }
            }
        }

        public List<Dictionary<string, object>> GetAll() => records;

        private void QueryIsOpen()
        {
            if (!records.Any())
            {
                throw new Exception("Query is closed or empty.");
            }
        }

        private void ColumnExists(string nameColumn)
        {
            if (!records[record].ContainsKey(nameColumn))
            {
                throw new Exception($"Column '{nameColumn}' is not exists.");
            }
        }

        public bool IsNull(string nameColumn)
        {
            QueryIsOpen();
            string column = nameColumn.Trim().ToUpper();
            ColumnExists(column);
            return records[record][column] == DBNull.Value;
        }

        public object Get(string nameColumn)
        {
            if (IsNull(nameColumn))
            {
                throw new Exception($"Column '{nameColumn}' is state null");
            }

            string column = nameColumn.Trim().ToUpper();
            return records[record][column];
        }

        private object GetValue(string nameColumn)
        {
            string column = nameColumn.Trim().ToUpper();
            return records[record][column];
        }

        public string GetString(string nameColumn) => Get(nameColumn).ToString();
        public string GetString(string nameColumn, string defaultValue) => IsNull(nameColumn) ? defaultValue : GetValue(nameColumn).ToString();
        public string GetStringOrNull(string nameColumn) => IsNull(nameColumn) ? null : GetValue(nameColumn).ToString();

        public char GetChar(string nameColumn) => Convert.ToChar(Get(nameColumn));
        public char GetChar(string nameColumn, char defaultValue) => IsNull(nameColumn) ? defaultValue : Convert.ToChar(GetValue(nameColumn));
        public char? GetCharOrNull(string nameColumn) => IsNull(nameColumn) ? null : (char?)Convert.ToChar(GetValue(nameColumn));

        public byte GetByte(string nameColumn) => Convert.ToByte(Get(nameColumn));
        public byte GetByte(string nameColumn, byte defaultValue) => IsNull(nameColumn) ? defaultValue : Convert.ToByte(GetValue(nameColumn));
        public byte? GetByteOrNull(string nameColumn) => IsNull(nameColumn) ? null : (byte?)Convert.ToByte(GetValue(nameColumn));

        public short GetShort(string nameColumn) => Convert.ToInt16(Get(nameColumn));
        public short GetShort(string nameColumn, short defaultValue) => IsNull(nameColumn) ? defaultValue : Convert.ToInt16(GetValue(nameColumn));
        public short? GetShortOrNull(string nameColumn) => IsNull(nameColumn) ? null : (byte?)Convert.ToInt16(GetValue(nameColumn));

        public int GetInt(string nameColumn) => Convert.ToInt32(Get(nameColumn));
        public int GetInt(string nameColumn, int defaultValue) => IsNull(nameColumn) ? defaultValue : Convert.ToInt32(GetValue(nameColumn));
        public int? GetIntOrNull(string nameColumn) => IsNull(nameColumn) ? null : (int?)Convert.ToInt32(GetValue(nameColumn));

        public long GetLong(string nameColumn) => Convert.ToInt64(Get(nameColumn));
        public long GetLong(string nameColumn, long defaultValue) => IsNull(nameColumn) ? defaultValue : Convert.ToInt64(GetValue(nameColumn));
        public long? GetLongOrNull(string nameColumn) => IsNull(nameColumn) ? null : (long?)Convert.ToInt64(GetValue(nameColumn));

        public float GetFloat(string nameColumn) => Convert.ToSingle(Get(nameColumn));
        public float GetFloat(string nameColumn, float defaultValue) => IsNull(nameColumn) ? defaultValue : Convert.ToSingle(GetValue(nameColumn));
        public float? GetFloatOrNull(string nameColumn) => IsNull(nameColumn) ? null : (float?)Convert.ToSingle(GetValue(nameColumn));

        public double GetDouble(string nameColumn) => Convert.ToDouble(Get(nameColumn));
        public double GetDouble(string nameColumn, double defaultValue) => IsNull(nameColumn) ? defaultValue : Convert.ToDouble(GetValue(nameColumn));
        public double? GetDoubleOrNull(string nameColumn) => IsNull(nameColumn) ? null : (double?)Convert.ToDouble(GetValue(nameColumn));

        public bool GetBoolean(string nameColumn)
        {
            if (IsNull(nameColumn))
            {
                return false;
            }

            return GetChar(nameColumn) == 'S';
        }

        public decimal GetDecimal(string nameColumn) => Convert.ToDecimal(Get(nameColumn));
        public decimal GetDecimal(string nameColumn, decimal defaultValue) => IsNull(nameColumn) ? defaultValue : Convert.ToDecimal(GetValue(nameColumn));
        public decimal? GetDecimalOrNull(string nameColumn) => IsNull(nameColumn) ? null : (decimal?)Convert.ToDecimal(GetValue(nameColumn));

        public DateTime GetDateTime(string nameColumn) => Convert.ToDateTime(Get(nameColumn));
        public DateTime GetDateTime(string nameColumn, DateTime defaultValue) => IsNull(nameColumn) ? defaultValue : Convert.ToDateTime(GetValue(nameColumn));
        public DateTime? GetDateTimeOrNull(string nameColumn) => IsNull(nameColumn) ? null : (DateTime?)Convert.ToDateTime(GetValue(nameColumn));

        public TimeSpan GetTime(string nameColumn) => GetDateTime(nameColumn).TimeOfDay;
        public TimeSpan GetTime(string nameColumn, TimeSpan defaultValue) => IsNull(nameColumn) ? defaultValue : Convert.ToDateTime(GetValue(nameColumn)).TimeOfDay;
        public TimeSpan? GetTimeOrNull(string nameColumn) => IsNull(nameColumn) ? null : (TimeSpan?)Convert.ToDateTime(GetValue(nameColumn)).TimeOfDay;

        public void ForEach(Action action)
        {
            if (action != null && records.Any())
            {
                try
                {
                    for (var it = 0; it < records.Count; it++)
                    {
                        record = it;
                        action();
                    }
                }
                finally
                {
                    record = 0;
                }
            }
        }

        public int Count { get => records.Count; }

        public bool IsNotEmpty { get => records.Any(); }
    }
}
