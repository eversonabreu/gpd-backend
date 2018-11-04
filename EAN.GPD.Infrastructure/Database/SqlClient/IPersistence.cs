namespace EAN.GPD.Infrastructure.Database.SqlClient
{
    public interface IPersistence
    {
        void AddParameter(string name, object value);
        void Execute();
    }
}
