using System;
using System.Collections.Generic;
using System.Data;

namespace EAN.GPD.Infrastructure.Database.SqlClient
{
    public interface IQuery
    {
        int Count { get; }
        bool IsNotEmpty { get; }
        string GetString(string nameColumn);
        string GetString(string nameColumn, string defaultValue);
        string GetStringOrNull(string nameColumn);
        char GetChar(string nameColumn);
        char GetChar(string nameColumn, char defaultValue);
        char? GetCharOrNull(string nameColumn);
        byte GetByte(string nameColumn);
        byte GetByte(string nameColumn, byte defaultValue);
        byte? GetByteOrNull(string nameColumn);
        short GetShort(string nameColumn);
        short GetShort(string nameColumn, short defaultValue);
        short? GetShortOrNull(string nameColumn);
        int GetInt(string nameColumn);
        int GetInt(string nameColumn, int defaultValue);
        int? GetIntOrNull(string nameColumn);
        long GetLong(string nameColumn);
        long GetLong(string nameColumn, long defaultValue);
        long? GetLongOrNull(string nameColumn);
        float GetFloat(string nameColumn);
        float GetFloat(string nameColumn, float defaultValue);
        float? GetFloatOrNull(string nameColumn);
        double GetDouble(string nameColumn);
        double GetDouble(string nameColumn, double defaultValue);
        double? GetDoubleOrNull(string nameColumn);
        bool GetBoolean(string nameColumn);
        decimal GetDecimal(string nameColumn);
        decimal GetDecimal(string nameColumn, decimal defaultValue);
        decimal? GetDecimalOrNull(string nameColumn);
        DateTime GetDateTime(string nameColumn);
        DateTime GetDateTime(string nameColumn, DateTime defaultValue);
        DateTime? GetDateTimeOrNull(string nameColumn);
        TimeSpan GetTime(string nameColumn);
        TimeSpan GetTime(string nameColumn, TimeSpan defaultValue);
        TimeSpan? GetTimeOrNull(string nameColumn);
        void AddParameter(string nameParam, object valueParam);
        void ExecuteQuery();
        bool IsNull(string nameColumn);
        object Get(string nameColumn);
        List<Dictionary<string, object>> GetAll();
        void ForEach(Action action);
    }
}
