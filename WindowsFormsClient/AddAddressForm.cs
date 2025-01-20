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
    public partial class AddAddressForm : Form
    {
        string uid;
        public AddAddressForm(string userid)
        {
            InitializeComponent();
            uid = userid;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7105");
            var client = new CustomerData.CustomerDataClient(channel);

            Address ad = new Address()
            {
                Building = textBox1.Text,
                Area = textBox2.Text,
                City = textBox3.Text,
                State = textBox4.Text,
                Pincode = textBox5.Text,
                UserId = uid
            };

            var response =await client.AddAddressAsync(ad);

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
                MessageBox.Show("Address added sucessfully", "Successful");

               


            }
        }
    }
}
