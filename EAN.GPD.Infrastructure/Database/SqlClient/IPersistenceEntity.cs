namespace EAN.GPD.Infrastructure.Database.SqlClient
{
    public enum TypePersistence
    {
        Update,
        Insert,
        Delete
    }

    public interface IPersistenceEntity
    {
        void AddColumn(string name, object value);
        void Execute();
    }
}
