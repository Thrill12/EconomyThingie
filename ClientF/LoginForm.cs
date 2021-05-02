using RequestLibrary;
using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientF
{
    public partial class LoginForm : Form
    {
        public User currUser;
        RequestClient client = new RequestClient(IPAddress.Loopback, 57253);

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Text;

            LoginRequest logReq = new LoginRequest(username, password);
            User chk = client.SendRequest<User>(logReq);
            if (chk != null)
            {
                currUser = chk;
                var f = new Form1(currUser, client);
                this.Hide();
                MessageBox.Show("Login successful: " + currUser.username);
                f.Show();
            }
            else
            {
                MessageBox.Show("Login failed. Please retry...");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Text;

            CreateAccountRequest createReq = new CreateAccountRequest(username, password);
            User chk = client.SendRequest<User>(createReq);
            if (chk != null)
            {
                currUser = chk;
                var f = new Form1(currUser, client);
                this.Hide();
                MessageBox.Show("Account created successfully: " + currUser.username);
                f.Show();
            }
            else
            {
                MessageBox.Show("Account not created. There may already be a user with your username...");
            }
        }

        private void UsernameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
