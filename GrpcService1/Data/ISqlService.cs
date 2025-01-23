using Microsoft.Data.SqlClient;

namespace GrpcService1.Data
{
    public interface ISqlService
    {
        Task<IDataReader> ExecuteTheReader(string Procedure_name, SqlParameter[] parameters);
    }
}
