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

namespace WindowsFormsClient
{
    public partial class Form2 : Form
    {
        private readonly static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7105");
        private readonly static CustomerData.CustomerDataClient client = new CustomerData.CustomerDataClient(channel);
        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void AddUserButton_Click(object sender, EventArgs e)
        {
            string fname = FirstNameTextBox.Text;
            string fname2 = LastNameTextBox.Text;
            string dt = DateOfBirthPicker.Text;

            Customer c = new Customer()
            {
                FirstName = fname,
                LastName = fname2,
                Dateofbirth = dt,
            };

            var response = await client.AddCustomerAsync(c);

            if (response.Isfailed)
            {
                MessageBox.Show(response.Errortxt, "Failed");
            }
            else if (response == null)
            {
                MessageBox.Show("Null responce", "Failed");

            }
            else
            {
                MessageBox.Show("User added sucessfully", "Successful");

                
               
            }
        }
    }
}
