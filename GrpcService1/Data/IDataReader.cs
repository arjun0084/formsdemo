namespace GrpcService1.Data
{
    public interface IDataReader : IDisposable
    {
        Task<bool> ReadAsync();
        string GetString(string columnName);
    }

}
