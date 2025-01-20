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

        string connectionstring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Customer;Integrated Security=True;";


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
                                    FirstName = reader["First_Name"].ToString(),
                                    LastName = reader["Last_Name"].ToString(),
                                    Dateofbirth = reader["Date_of_Birth"].ToString(),
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
                                    Id=reader["Id"].ToString()
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



        public async override Task<Status> DeleteUser(Id request, ServerCallContext context)
        {
            try
            {
                var custid = request.UserId.ToString();
                using SqlConnection conn = new("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Customer;Integrated Security=True;");
                await conn.OpenAsync();


                using SqlCommand cmd = new("deletecustomer",conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_id", custid));

                var rowsaffected = cmd.ExecuteNonQuery();
                if (rowsaffected >= 0)
                {
                    Status status = new();
                    return status;
                }
                else
                {
                    Status status = new()
                    {
                        Isfailed = true,
                        Errortxt = rowsaffected.ToString()
                    };
                    return status;
                }
            }
            catch (Exception e)
            {
                Status status = new()
                {
                    Isfailed = true,
                    Errortxt = e.Message
                };
                return status;
            }
        }



        public async override  Task<Status> DeleteAddress(AddressId request, ServerCallContext context)
        {
            try
            {
                var id = request.AddressId_;
                using SqlConnection conn = new("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Customer;Integrated Security=True;");
                await conn.OpenAsync();
                using SqlCommand cmd = new("deleteaddress", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@address_id", id));

                var rowsaffected = cmd.ExecuteNonQuery();
                if (rowsaffected >= 0)
                {
                    return new Status();
                }
                else
                {
                    Status status = new()
                    {
                        Isfailed = true,
                        Errortxt = rowsaffected.ToString()
                    };
                    return status;
                }
            }
            catch (Exception ex)
            {

                return new Status() { Isfailed = true, Errortxt = ex.Message };
            }


        }


        public async override Task<Status> AddCustomer(Customer request, ServerCallContext context)
        {
            try
            {
                string f_name = request.FirstName;
                string s_name = request.LastName;
                DateOnly dob = DateOnly.ParseExact(request.Dateofbirth,"dd/MM/yyyy");


                using SqlConnection conn = new(connectionstring);
                await conn.OpenAsync();
                using SqlCommand cmd = new("adduser", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@firstname", f_name));
                cmd.Parameters.Add(new SqlParameter("@lastname", s_name));
                cmd.Parameters.Add(new SqlParameter("@dateofbirth", dob));

                int rowsafected = await cmd.ExecuteNonQueryAsync();
                if (rowsafected > 0)
                {
                    return new Status();
                }
                else
                {
                    return new Status()
                    {
                        Isfailed = true,
                    };
                }
            }
            catch (Exception ex)
            {

                return new Status { Isfailed = true, Errortxt = ex.Message };
            }


        }


        public async override Task<Status> AddAddress(Address request, ServerCallContext context)
        {

            try
            {
                int userid = int.Parse(request.UserId);
                string building = request.Building;
                string area = request.Area;
                string city = request.City;
                string state = request.State;
                int pincode = int.Parse(request.Pincode);


                using SqlConnection conn = new SqlConnection(connectionstring);
                using SqlCommand cmd = new("addaddress", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@userid", userid));
                cmd.Parameters.Add(new SqlParameter("@building", building));
                cmd.Parameters.Add(new SqlParameter("@area", area));
                cmd.Parameters.Add(new SqlParameter("@city", city));
                cmd.Parameters.Add(new SqlParameter("@state", state));
                cmd.Parameters.Add(new SqlParameter("@pincode", pincode));

                await conn.OpenAsync();
                int noofrowsaffected = cmd.ExecuteNonQuery();

                if (noofrowsaffected >= 0)
                {
                    Status status = new ();
                    return status;
                }
                else
                {
                    return new Status() { Isfailed = true };
                }

            }
            catch (Exception ex)
            {

                return new Status { Isfailed = true, Errortxt = ex.Message };
            }

        }


        public async override Task<Status> UpdateAddress(Address request, ServerCallContext context)
        {
            try
            {
                int id = int.Parse(request.Id);
                string building = request.Building;
                string area = request.Area;
                string city = request.City;
                string state = request.State;
                int pincode = int.Parse(request.Pincode);


                using SqlConnection conn = new SqlConnection(connectionstring);
                using SqlCommand cmd = new("updateaddress", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.Parameters.Add(new SqlParameter("@building", building));
                cmd.Parameters.Add(new SqlParameter("@area", area));
                cmd.Parameters.Add(new SqlParameter("@city", city));
                cmd.Parameters.Add(new SqlParameter("@state", state));
                cmd.Parameters.Add(new SqlParameter("@pincode", pincode));

                await conn.OpenAsync();
                int noofrowsaffected = cmd.ExecuteNonQuery();

                if (noofrowsaffected >= 0)
                {
                    Status status = new();
                    return status;
                }
                else
                {
                    return new Status() { Isfailed = true };
                }

            }
            catch (Exception ex)
            {

                return new Status { Isfailed = true, Errortxt = ex.Message };
            }
        }


    }
}
