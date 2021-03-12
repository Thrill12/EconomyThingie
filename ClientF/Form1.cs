using RequestLibrary;
using RequestLibrary.Alerts;
using RequestLibrary.Form;
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
        List<User> localPlayers;

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
            FindAndDisplayLocalPlayers();

            currUser.equippedShip.UpdateShipStats();

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += backgroundWorker1_DoWork;
            bw.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;

            client.SubscribeTo<ChatAlert>(this, OnAlert);
        }

        private void OnAlert(ChatAlert alert)
        {
            ChatBox.AppendText(alert.messageToSend + "\n");
            ChatBox.SelectionStart = ChatBox.Text.Length;
            ChatBox.ScrollToCaret();
        }

        private void FindAndDisplayLocalPlayers()
        {
            LocalPlayers.Items.Clear();

            FindLocalPlayersRequest createReq = new FindLocalPlayersRequest(currUser.position);
            lock (client)
            {
                localPlayers = client.SendRequest<LocalPlayersListWrapper>(createReq).users;
            }

            foreach(User u in localPlayers)
            {
                ListViewItem item = new ListViewItem();
                item.Text = u.username;
                item.Tag = u;
                LocalPlayers.Items.Add(item);
            }
        }

        private void FindAndDisplayNearbyStars()
        {
            JumpableSystems.Items.Clear();

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
                JumpableSystems.Items.Add(item);
            }
        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            lock (client)
            {
                currUser.position = (StarSystem)e.Item.Tag;
                ChatBox.AppendText("Entered system " + currUser.position.name + "\n");
                SendUpdateToServer();
                RefreshForm();      
            }        
        }

        private void RefreshForm()
        {
            currUser.positionID = currUser.position.ID;
            lock (this)
            {
                FindAndDisplayNearbyStars();
            }

            lock (this)
            {
                FindAndDisplayLocalPlayers();
            }    

            lock (this)
            {
                this.Text = $"{currUser.username}--{currUser.position.name}--{currUser.seshID}";
                UsernameBox.Text = currUser.username;
                PositionBox.Text = currUser.position.name;
                GCBox.Text = currUser.galacticCredits.ToString();
                DWBox.Text = currUser.diplomaticWeight.ToString();

                ShipTestingDisplay.Clear();
                ShipTestingDisplay.AppendText((currUser.equippedShip.id + "\n").ToString());
                ShipTestingDisplay.AppendText((currUser.equippedShip.cargoLimit + "\n").ToString());
                ShipTestingDisplay.AppendText((currUser.equippedShip.crewCap + "\n").ToString());
                ShipTestingDisplay.AppendText((currUser.equippedShip.health + "\n").ToString());
                ShipTestingDisplay.AppendText((currUser.equippedShip.QELimit + "\n").ToString());
                ShipTestingDisplay.AppendText((currUser.equippedShip.passiveSlots.Count().ToString() + " slots in the ship." + "\n"));
            }            
        }

        private void LocalPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void ChatBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendChatRequest chatReq = new SendChatRequest(ChatBoxInput.Text);               
                client.SendRequest(chatReq, currUser);
                ChatBoxInput.Clear();
                ChatBox.SelectionStart = ChatBox.Text.Length;
                ChatBox.ScrollToCaret();
            }
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
            System.Threading.Thread.Sleep(500);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshForm();
        }
        #endregion    
    }
}
