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
        string aid;
        bool isupdate;
        public AddAddressForm(string userid)
        {
            InitializeComponent();
            uid = userid;
        }

        public AddAddressForm(string adressid,bool _isupdate)
        {
            InitializeComponent();
            aid = adressid;
            isupdate = _isupdate;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7105");
            var client = new CustomerData.CustomerDataClient(channel);
            Status response;

            if (isupdate)
            {
                Address ad = new Address()
                {
                    Building = textBox1.Text,
                    Area = textBox2.Text,
                    City = textBox3.Text,
                    State = textBox4.Text,
                    Pincode = textBox5.Text,
                    Id=aid
                };
                 response = await client.UpdateAddressAsync(ad);

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
                    var result =MessageBox.Show("Address Updated sucessfully", "Successful",MessageBoxButtons.OK);
                    if (result == DialogResult.OK)
                    {
                        this.Close();
                       
                    }
                }


            }
            else
            {
                Address ad = new Address()
                {
                    Building = textBox1.Text,
                    Area = textBox2.Text,
                    City = textBox3.Text,
                    State = textBox4.Text,
                    Pincode = textBox5.Text,
                    UserId = uid
                };
                response = await client.AddAddressAsync(ad);

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
                    var result=MessageBox.Show("Address added sucessfully", "Successful");
                    if (result == DialogResult.OK)
                    { 
                        this.Close(); 
                    
                    
                    }
                }

            }
        }
    }
}
