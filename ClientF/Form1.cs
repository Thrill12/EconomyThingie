using RequestLibrary;
using RequestLibrary.Alerts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientF
{
    public partial class Form1 : Form
    {
        User currUser;
        RequestClient client;
        List<StarSystem> nearbySystems;

        public Form1(User user, RequestClient client)
        {
            currUser = user;
            this.client = client;

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {           
            StartTimer();
            RefreshForm();
            FindAndDisplayNearbyStars();           

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += backgroundWorker1_DoWork;
            bw.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;

            client.SubscribeTo<TestAlert>(this, OnTestAlert);
        }

        private void OnTestAlert(TestAlert alert)
        {
            ChatBox.AppendText(alert.MessageFromServer);
        }

        private void FindAndDisplayNearbyStars()
        {
            listView1.Items.Clear();

            FindJumpableSystemsRequest createReq = new FindJumpableSystemsRequest(currUser.position);
            lock (client)
            {
                nearbySystems = client.SendRequest<StarSystemListWrapper>(createReq).systems;
            }

            foreach(StarSystem sys in nearbySystems)
            {
                ListViewItem item = new ListViewItem();
                item.Text = sys.name + " - " + sys.starClass;
                item.Tag = sys;
                listView1.Items.Add(item);
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            lock (client)
            {
                currUser.position = (StarSystem)e.Item.Tag;
                RefreshForm();
                SendUpdateToServer();
            }        
        }

        private void RefreshForm()
        {
            currUser.positionID = currUser.position.ID;
            FindAndDisplayNearbyStars();

            lock (this)
            {
                this.Text = $"{currUser.username}--{currUser.position.name}--{currUser.seshID}";
                UsernameBox.Text = currUser.username;
                PositionBox.Text = currUser.position.name;
                GCBox.Text = currUser.galacticCredits.ToString();
                DWBox.Text = currUser.diplomaticWeight.ToString();
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReadRequest req = new ReadRequest("got back from server woo");

            string temp = client.SendRequest<string>(req);

            UsernameBox.AppendText(Environment.NewLine + temp);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            currUser.galacticCredits += 500;
            RefreshForm();
            SendUpdateToServer();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            SendUpdateToServer();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            currUser.diplomaticWeight += 100;
            RefreshForm();
            SendUpdateToServer();
        }

        #region ServerStuff

        System.Threading.Timer tim;

        public void StartTimer()
        {
            tim = new System.Threading.Timer(SendUpdateToServerTimer, null, 1000, 1000);
        }

        private void SendUpdateToServerTimer(object state)
        {
            SendUpdateToServer();
        }

        private void SendUpdateToServer()
        {
            lock (client)
            {
                UpdateClientOnServerRequest createReq = new UpdateClientOnServerRequest(currUser);
                currUser = client.SendRequest<User>(createReq);
            }                     
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SendUpdateToServer();
            System.Threading.Thread.Sleep(1000);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshForm();
        }

        #endregion

        private void ChatBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ChatBox.AppendText(ChatBoxInput.Text);
                ChatBoxInput.Clear();
            }
        }
    }
}
