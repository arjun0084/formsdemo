using Microsoft.Data.SqlClient;

namespace GrpcService1.Data
{
    public class SqlService : ISqlService
    {
        public async Task<IDataReader> ExecuteTheReader(string Procedure_name, SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Customer;Integrated Security=True;");
            await conn.OpenAsync();
            using SqlCommand cmd = new SqlCommand(Procedure_name, conn);
            cmd.CommandType= System.Data.CommandType.StoredProcedure;
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            var reader = await cmd.ExecuteReaderAsync();
            return new DataReaderWrapper(reader);



        }
    }
}
