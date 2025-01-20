namespace WindowsFormsClient
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.searchbox = new System.Windows.Forms.TextBox();
            this.label_search = new System.Windows.Forms.Label();
            this.label_Users = new System.Windows.Forms.Label();
            this.label_Addresses = new System.Windows.Forms.Label();
            this.DeleteCustomer = new System.Windows.Forms.Button();
            this.delete_address = new System.Windows.Forms.Button();
            this.adduserbutton = new System.Windows.Forms.Button();
            this.addaddressbutton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(594, 221);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(67, 313);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(929, 162);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(67, 53);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersWidth = 62;
            this.dataGridView2.RowTemplate.Height = 28;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(401, 213);
            this.dataGridView2.TabIndex = 2;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(594, 151);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 38);
            this.button2.TabIndex = 3;
            this.button2.Text = "Fetch Address";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // searchbox
            // 
            this.searchbox.Location = new System.Drawing.Point(594, 85);
            this.searchbox.Name = "searchbox";
            this.searchbox.Size = new System.Drawing.Size(193, 26);
            this.searchbox.TabIndex = 6;
            this.searchbox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label_search
            // 
            this.label_search.AutoSize = true;
            this.label_search.Location = new System.Drawing.Point(501, 91);
            this.label_search.Name = "label_search";
            this.label_search.Size = new System.Drawing.Size(64, 20);
            this.label_search.TabIndex = 7;
            this.label_search.Text = "Search:";
            this.label_search.Click += new System.EventHandler(this.label1_Click);
            // 
            // label_Users
            // 
            this.label_Users.AutoSize = true;
            this.label_Users.Location = new System.Drawing.Point(63, 30);
            this.label_Users.Name = "label_Users";
            this.label_Users.Size = new System.Drawing.Size(55, 20);
            this.label_Users.TabIndex = 8;
            this.label_Users.Text = "Users:";
            this.label_Users.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // label_Addresses
            // 
            this.label_Addresses.AutoSize = true;
            this.label_Addresses.Location = new System.Drawing.Point(67, 287);
            this.label_Addresses.Name = "label_Addresses";
            this.label_Addresses.Size = new System.Drawing.Size(89, 20);
            this.label_Addresses.TabIndex = 9;
            this.label_Addresses.Text = "Addresses:";
            // 
            // DeleteCustomer
            // 
            this.DeleteCustomer.Location = new System.Drawing.Point(764, 151);
            this.DeleteCustomer.Name = "DeleteCustomer";
            this.DeleteCustomer.Size = new System.Drawing.Size(117, 38);
            this.DeleteCustomer.TabIndex = 10;
            this.DeleteCustomer.Text = "Delete ";
            this.DeleteCustomer.UseVisualStyleBackColor = true;
            this.DeleteCustomer.Click += new System.EventHandler(this.DeleteCustomer_Click);
            // 
            // delete_address
            // 
            this.delete_address.Location = new System.Drawing.Point(764, 221);
            this.delete_address.Name = "delete_address";
            this.delete_address.Size = new System.Drawing.Size(131, 34);
            this.delete_address.TabIndex = 11;
            this.delete_address.Text = "Delete Address";
            this.delete_address.UseVisualStyleBackColor = true;
            this.delete_address.Click += new System.EventHandler(this.delete_address_Click);
            // 
            // adduserbutton
            // 
            this.adduserbutton.Location = new System.Drawing.Point(921, 151);
            this.adduserbutton.Name = "adduserbutton";
            this.adduserbutton.Size = new System.Drawing.Size(118, 38);
            this.adduserbutton.TabIndex = 12;
            this.adduserbutton.Text = "adduser";
            this.adduserbutton.UseVisualStyleBackColor = true;
            this.adduserbutton.Click += new System.EventHandler(this.adduserbutton_Click);
            // 
            // addaddressbutton
            // 
            this.addaddressbutton.Location = new System.Drawing.Point(921, 221);
            this.addaddressbutton.Name = "addaddressbutton";
            this.addaddressbutton.Size = new System.Drawing.Size(118, 34);
            this.addaddressbutton.TabIndex = 13;
            this.addaddressbutton.Text = "Add Address";
            this.addaddressbutton.UseVisualStyleBackColor = true;
            this.addaddressbutton.Click += new System.EventHandler(this.addaddressbutton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(1129, 551);
            this.Controls.Add(this.addaddressbutton);
            this.Controls.Add(this.adduserbutton);
            this.Controls.Add(this.delete_address);
            this.Controls.Add(this.DeleteCustomer);
            this.Controls.Add(this.label_Addresses);
            this.Controls.Add(this.label_Users);
            this.Controls.Add(this.label_search);
            this.Controls.Add(this.searchbox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Customer Address";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox searchbox;
        private System.Windows.Forms.Label label_search;
        private System.Windows.Forms.Label label_Users;
        private System.Windows.Forms.Label label_Addresses;
        private System.Windows.Forms.Button DeleteCustomer;
        private System.Windows.Forms.Button delete_address;
        private System.Windows.Forms.Button adduserbutton;
        private System.Windows.Forms.Button addaddressbutton;
    }
}

