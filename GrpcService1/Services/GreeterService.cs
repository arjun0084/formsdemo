using Grpc.Core;
using GrpcService1;
using Microsoft.Data.SqlClient;

namespace GrpcService1.Services
{
    public class CustomerDataService : CustomerData.CustomerDataBase
    {
        private readonly ILogger<CustomerDataService> _logger;
        public CustomerDataService(ILogger<CustomerDataService> logger)
        {
            _logger = logger;
        }


        //service defination for  rpc function GetCustomers
        public override async Task<CustomerList> GetCustomers(Empty request, ServerCallContext context)
        {
            CustomerList customerList = new CustomerList(); //Initialing return list
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Customer;Integrated Security=True;"))
                {
                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("get", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; // calling stored procedure named 'get'.
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {


                            while (await reader.ReadAsync())
                            {
                                var customer = new Customer()
                                {
                                    Name = reader["Name"].ToString(),
                                    Id = reader["Id"].ToString(),

                                };
                                customerList.Custometrs.Add(customer);  // maping customer data customer object and adding it to list

                            }
                        }
                    }
                }
                return customerList;
            }
            catch (Exception e)
            {

                customerList.Isfailed = true; // if some error occurs
                customerList.Errortxt = e.Message;
                return customerList;

            }
        
        }




        //service defination for  rpc function GetCustomer
        public async override Task<AddressList> GetCustomersById(Id request, ServerCallContext context)
        {
            AddressList addresslist = new AddressList(); // initializing data which we have to return
            try
            {
                var custid = request.UserId;
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Customer;Integrated Security=True;"))
                {
                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand($"exec getaddress @_id={custid}", con)) // passing the id to stored procedure named getaddress.
                    {
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {


                            while (await reader.ReadAsync())
                            {
                                var address = new Address()
                                {
                                    Building = reader["Building"].ToString(),
                                    Area = reader["Area"].ToString(),
                                    City = reader["City"].ToString(),
                                    State = reader["State"].ToString(),
                                    Pincode = reader["Pincode"].ToString(),
                                };
                                addresslist.Addresses.Add(address);// mapping the data and adding to the list.

                            }
                        }
                    }
                }
                return addresslist;
            }
            catch (Exception e)
            {
                addresslist.Isfailed = true;
                addresslist.Errortxt=e.Message;
                return addresslist;
                
            }
        }


    }
}
