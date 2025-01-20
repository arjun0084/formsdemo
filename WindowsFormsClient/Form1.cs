            
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Grpc.Net.Client;
using GrpcClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace WindowsFormsClient
{
    public partial class Form1 : Form
    {

        private readonly DataTable dt_user = new DataTable(); // Data table for user grid
        private readonly DataTable dt_address = new DataTable(); // Datatable for addresses gird
        private readonly static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7105"); // URL of the grpservice application
        private readonly CustomerData.CustomerDataClient client = new CustomerData.CustomerDataClient(channel);//client to access grp functions



        public Form1()
        {
            InitializeComponent(); 

            //columns for user table
            dt_user.Columns.Add("Id");
            dt_user.Columns.Add("First_Name");
            dt_user.Columns.Add("Last_Name");
            dt_user.Columns.Add("Date Of Birth");
     


            //columns for address table
            dt_address.Columns.Add("Id");
            dt_address.Columns.Add("Building");
            dt_address.Columns.Add("Area");
            dt_address.Columns.Add("City");
            dt_address.Columns.Add("State");
            dt_address.Columns.Add("Pincode");

            

            //adding datasources
            dataGridView2.DataSource = dt_user; 
            dataGridView1.DataSource = dt_address;
            dataGridView2.Columns["Id"].Visible = false;//hiding unnecessary id column of the Users
            dataGridView1.Columns["Id"].Visible = false;//hiding unnecessary id column of the address

            //calling the function to load data received form grpc service in the datatable
            Loaddata();

        }

        public async  void Loaddata()
        {
            try
            {
                var response = await client.GetCustomersAsync(new Empty()); //passing empty msg and getting all users

                //for error response from the grpcservice
                if (response.Isfailed == true)  
                {
                    MessageBox.Show(response.Errortxt);
                }
                else
                {
                    var customers = response.Custometrs;
                    dt_user.Rows.Clear();

                    foreach (var customer in customers)
                    {
                        dt_user.Rows.Add(customer.Id, customer.FirstName,customer.LastName,customer.Dateofbirth); // adding the datatable row with the data received
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button1_Click(object sender, EventArgs e) // refresh/fetch button
        {
            Loaddata();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e) // Button for fetching address
        {
            try
            {
                    var row = dataGridView2.SelectedRows[0]; // to get the selected row in user datatable , 0 represents the 1st selected row
                    var id = row.Cells[0].Value; //extracting the value of the first column of the selected row
                if (id!=null)
                {

                    Console.WriteLine(id); //preparing request message for grpc service
                    Id idd = new Id()
                    {
                        UserId = id.ToString(),
                    };


                    var response = await client.GetCustomersByIdAsync(idd);
                    var address = response.Addresses;
                    dt_address.Rows.Clear();

                    foreach (var ad in address)
                    {
                        dt_address.Rows.Add(ad.Id,ad.Building, ad.Area, ad.City, ad.State, ad.Pincode);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

      

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchValue = searchbox.Text.Trim();

            if (!string.IsNullOrEmpty(searchValue))
            {
                // Apply a filter to the DataTable's DefaultView
                dt_user.DefaultView.RowFilter = $"First_Name LIKE '%{searchValue}%' OR Last_Name LIKE '%{searchValue}%'";

            }
            else
            {
                // Clear the filter when the search box is empty
                dt_user.DefaultView.RowFilter = string.Empty;
            }


            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[1].Value.ToString().ToLower().Equals(searchValue.ToLower()))
                    {
                        row.Selected = true;
                        break;
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private async void DeleteCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                var row = dataGridView2.SelectedRows[0];
                var id = row.Cells[0].Value.ToString();

                Id idd = new Id()
                {
                    UserId = id,
                };

                var response = await client.DeleteUserAsync(idd);
                if (response.Isfailed)
                {
                    MessageBox.Show( response.Errortxt,("Failed"));
                }
                else
                {
                    MessageBox.Show("User Deleted Successfully");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }



        }

        private async void delete_address_Click(object sender, EventArgs e) // check for deleteall or nothing :|
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var rows = dataGridView1.SelectedRows;

                    foreach (DataGridViewRow row in rows)
                    {
                        string adr_id = row.Cells[0].Value.ToString();
                        AddressId adressid = new AddressId() { AddressId_ = adr_id };
                        var response = await client.DeleteAddressAsync(adressid);

                        if (response.Isfailed)
                        {
                            MessageBox.Show(response.Errortxt + "for address: " + row.Cells["Building"], "Failed");
                            break;
                        }
                    }
                  
                    var result =MessageBox.Show("Address Deleted Sucessfully","Successful",MessageBoxButtons.OK);
                    if (result == DialogResult.OK)
                    {
                        button2_Click(sender, e);
                    }
                    

                }
                else
                {
                    MessageBox.Show("Select a row");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
          

        }

        private void adduserbutton_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }

        private void addaddressbutton_Click(object sender, EventArgs e)
        {
            int count = dataGridView2.SelectedRows.Count;
            if (count > 0)
            {
                var row = dataGridView2.SelectedRows[0];
                string user_id = row.Cells[0].Value.ToString();

                AddAddressForm form = new AddAddressForm(user_id);
                form.Show();
            }
        }

        private void UpdateAddressbutton_Click(object sender, EventArgs e)
        {
            int count = dataGridView1.SelectedRows.Count;
            if (count>0)
            {
                
                var row = dataGridView1.SelectedRows[0];
                string address_id = row.Cells[0].Value.ToString();

                AddAddressForm form = new AddAddressForm(address_id,true);
                form.textBox1.Text = row.Cells[1].Value.ToString();
                form.textBox2.Text = row.Cells[2].Value.ToString();
                form.textBox3.Text = row.Cells[3].Value.ToString();
                form.textBox4.Text = row.Cells[4].Value.ToString();
                form.textBox5.Text = row.Cells[5].Value.ToString();
                form.button1.Text = "Update Address";
                form.Show(); 
            }
        }
    }
}
