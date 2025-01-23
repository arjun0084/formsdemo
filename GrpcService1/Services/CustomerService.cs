using Grpc.Core;
using GrpcService1;
using GrpcService1.Data;
using Microsoft.Data.SqlClient;

namespace GrpcService1.Services
{
    public class CustomerDataService : CustomerData.CustomerDataBase
    {
        public readonly ISqlService _sqlService;
        public CustomerDataService(ISqlService sqlservice)
        {
            _sqlService = sqlservice;
        }

        private readonly string connectionstring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Customer;Integrated Security=True;"; // connection string for sql connection


        //service defination for  rpc function GetCustomers
        public override async Task<CustomerList> GetCustomers(Empty request, ServerCallContext context)
        {
            CustomerList customerList = new(); //Initialing return list
            try
            {
                

                using IDataReader reader=await _sqlService.ExecuteTheReader("get", null);

                    while (await reader.ReadAsync())
                    {
                        var customer = new Customer()
                        {
                            FirstName = reader.GetString("First_Name"),
                            LastName = reader.GetString("Last_Name"),
                            Dateofbirth = reader.GetString("Date_of_Birth"),
                            Id = reader.GetString("Id"),

                        };
                        customerList.Custometrs.Add(customer);  // maping customer data customer object and adding it to list

                    }
                //}
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
            AddressList addresslist = new (); // initializing data which we have to return
            try
            {
                var custid = request.UserId;
                using (SqlConnection con = new("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Customer;Integrated Security=True;"))
                {
                    await con.OpenAsync();

                    using SqlCommand cmd = new ($"exec getaddress @_id={custid}", con); // passing the id to stored procedure named getaddress.

                    using SqlDataReader reader = cmd.ExecuteReader();

                    while (await reader.ReadAsync())
                    {
                        var address = new Address()
                        {
                            Building = reader["Building"].ToString(),
                            Area = reader["Area"].ToString(),
                            City = reader["City"].ToString(),
                            State = reader["State"].ToString(),
                            Pincode = reader["Pincode"].ToString(),
                            Id = reader["Id"].ToString()
                        };
                        addresslist.Addresses.Add(address);// mapping the data and adding to the list.

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


        //service defination for rpc function deleteuser
        public async override Task<Status> DeleteUser(Id request, ServerCallContext context)
        {
            try
            {
                var custid = request.UserId.ToString();//to delete user by user id
                using SqlConnection conn = new(connectionstring);
                await conn.OpenAsync();


                using SqlCommand cmd = new("deletecustomer",conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@_id", custid));

                var rowsaffected = cmd.ExecuteNonQuery();
                if (rowsaffected >= 0) //sucess
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


        //service defination for rpc function DeleteAddress
        //it deletes the address by addressid
        public async override  Task<Status> DeleteAddress(AddressId request, ServerCallContext context)
        {
            try
            {
                var id = request.AddressId_; 
                using SqlConnection conn = new(connectionstring);
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



        //service defination for adding the customer
        public async override Task<Status> AddCustomer(Customer request, ServerCallContext context)
        {
            try
            {
                //defining the values for creating a new customer
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



        //service defination for rpc defination addaddress
        //adds address for specified customer 
        public async override Task<Status> AddAddress(Address request, ServerCallContext context)
        {

            try
            {
                //assigning data required to add address
                int userid = int.Parse(request.UserId);
                string building = request.Building;
                string area = request.Area;
                string city = request.City;
                string state = request.State;
                int pincode = int.Parse(request.Pincode);


                using SqlConnection conn = new (connectionstring);
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



        //service defination for rpc function updateaddress
        //updates selected address
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
