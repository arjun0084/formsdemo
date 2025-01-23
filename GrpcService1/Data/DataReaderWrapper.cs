using Microsoft.Data.SqlClient;

namespace GrpcService1.Data
{
    public class DataReaderWrapper : IDataReader
    {
        private readonly SqlDataReader _reader;

        public DataReaderWrapper(SqlDataReader reader)
        {
            _reader = reader;
        }

        public async Task<bool> ReadAsync()
        {
            return await _reader.ReadAsync();
        }

        public string GetString(string columnName)
        {
            return _reader[columnName]?.ToString();
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}
