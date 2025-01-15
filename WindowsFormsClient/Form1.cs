            
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
        string nametosearch = "";
        
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7105");
        CustomerData.CustomerDataClient client = new CustomerData.CustomerDataClient(channel);



        public Form1()
        {
            InitializeComponent();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt2.Columns.Add("Building");
            dt2.Columns.Add("Area");
            dt2.Columns.Add("City");
            dt2.Columns.Add("State");
            dt2.Columns.Add("Pincode");
            dataGridView2.DataSource = dt;
            dataGridView1.DataSource = dt2;
            loaddata();

        }

        public async  void loaddata()
        {
            

            try
            {
                var response = await client.GetCustomersAsync(new Empty());
                if (response == null)
                {
                    MessageBox.Show("Null response form grpc service");
                }
                if (response.Isfailed == true)
                {
                    MessageBox.Show(response.Errortxt);

                }

                else
                {
                    var customers = response.Custometrs;


                    dt.Rows.Clear();


                    foreach (var customer in customers)
                    {
                        dt.Rows.Add(customer.Id, customer.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }

        private async void button1_Click(object sender, EventArgs e)
        {

            loaddata();
            



        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 selectedRowCount = dataGridView2.Rows.GetRowCount(DataGridViewElementStates.Selected);
                    var row = dataGridView2.SelectedRows[0];
                    var id = row.Cells[0].Value;
                if (selectedRowCount > 0 && id!=null)
                {

                    Console.WriteLine(id);
                    Id idd = new Id()
                    {
                        UserId = id.ToString(),
                    };


                    var response = await client.GetCustomersByIdAsync(idd);
                    var address = response.Addresses;
                    dt2.Rows.Clear();

                    foreach (var ad in address)
                    {
                        dt2.Rows.Add(ad.Building, ad.Area, ad.City, ad.State, ad.Pincode);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

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
                dt.DefaultView.RowFilter = $"Name LIKE '{searchValue}%'";
            }
            else
            {
                // Clear the filter when the search box is empty
                dt.DefaultView.RowFilter = string.Empty;
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

       
       
    }
}
