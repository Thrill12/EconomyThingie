﻿namespace ClientF
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
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.GCLabel = new System.Windows.Forms.Label();
            this.GCBox = new System.Windows.Forms.TextBox();
            this.DWLabel = new System.Windows.Forms.Label();
            this.DWBox = new System.Windows.Forms.TextBox();
            this.PositionBox = new System.Windows.Forms.TextBox();
            this.ADDGC = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ADDDW = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Star_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChatBox = new System.Windows.Forms.RichTextBox();
            this.ChatBoxInput = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // UsernameBox
            // 
            this.UsernameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UsernameBox.BackColor = System.Drawing.SystemColors.Window;
            this.UsernameBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.UsernameBox.Enabled = false;
            this.UsernameBox.Location = new System.Drawing.Point(890, 9);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.ReadOnly = true;
            this.UsernameBox.Size = new System.Drawing.Size(132, 20);
            this.UsernameBox.TabIndex = 1;
            this.UsernameBox.TabStop = false;
            this.UsernameBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // GCLabel
            // 
            this.GCLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GCLabel.AutoSize = true;
            this.GCLabel.Location = new System.Drawing.Point(895, 38);
            this.GCLabel.Name = "GCLabel";
            this.GCLabel.Size = new System.Drawing.Size(25, 13);
            this.GCLabel.TabIndex = 0;
            this.GCLabel.Text = "GC:";
            // 
            // GCBox
            // 
            this.GCBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GCBox.BackColor = System.Drawing.SystemColors.Window;
            this.GCBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.GCBox.Enabled = false;
            this.GCBox.ForeColor = System.Drawing.SystemColors.MenuText;
            this.GCBox.Location = new System.Drawing.Point(922, 35);
            this.GCBox.Name = "GCBox";
            this.GCBox.ReadOnly = true;
            this.GCBox.Size = new System.Drawing.Size(72, 20);
            this.GCBox.TabIndex = 2;
            this.GCBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // DWLabel
            // 
            this.DWLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DWLabel.AutoSize = true;
            this.DWLabel.Location = new System.Drawing.Point(891, 64);
            this.DWLabel.Name = "DWLabel";
            this.DWLabel.Size = new System.Drawing.Size(29, 13);
            this.DWLabel.TabIndex = 3;
            this.DWLabel.Text = "DW:";
            // 
            // DWBox
            // 
            this.DWBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DWBox.BackColor = System.Drawing.SystemColors.Window;
            this.DWBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.DWBox.Enabled = false;
            this.DWBox.ForeColor = System.Drawing.SystemColors.MenuText;
            this.DWBox.Location = new System.Drawing.Point(922, 61);
            this.DWBox.Name = "DWBox";
            this.DWBox.ReadOnly = true;
            this.DWBox.Size = new System.Drawing.Size(72, 20);
            this.DWBox.TabIndex = 4;
            this.DWBox.TabStop = false;
            this.DWBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // PositionBox
            // 
            this.PositionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionBox.BackColor = System.Drawing.SystemColors.Window;
            this.PositionBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.PositionBox.Enabled = false;
            this.PositionBox.Location = new System.Drawing.Point(890, 303);
            this.PositionBox.Name = "PositionBox";
            this.PositionBox.ReadOnly = true;
            this.PositionBox.Size = new System.Drawing.Size(132, 20);
            this.PositionBox.TabIndex = 5;
            this.PositionBox.TabStop = false;
            this.PositionBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ADDGC
            // 
            this.ADDGC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ADDGC.Location = new System.Drawing.Point(996, 35);
            this.ADDGC.Name = "ADDGC";
            this.ADDGC.Size = new System.Drawing.Size(26, 20);
            this.ADDGC.TabIndex = 6;
            this.ADDGC.Text = "button1";
            this.ADDGC.UseVisualStyleBackColor = true;
            this.ADDGC.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // ADDDW
            // 
            this.ADDDW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ADDDW.Location = new System.Drawing.Point(996, 61);
            this.ADDDW.Name = "ADDDW";
            this.ADDDW.Size = new System.Drawing.Size(26, 20);
            this.ADDDW.TabIndex = 8;
            this.ADDDW.Text = "button3";
            this.ADDDW.UseVisualStyleBackColor = true;
            this.ADDDW.Click += new System.EventHandler(this.button3_Click);
            // 
            // listView1
            // 
            this.listView1.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.AutoArrange = false;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Star_Name});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(755, 303);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(129, 246);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 10;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            // 
            // Star_Name
            // 
            this.Star_Name.Text = "Name";
            this.Star_Name.Width = 100;
            // 
            // ChatBox
            // 
            this.ChatBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatBox.Location = new System.Drawing.Point(12, 303);
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.ReadOnly = true;
            this.ChatBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ChatBox.Size = new System.Drawing.Size(737, 210);
            this.ChatBox.TabIndex = 11;
            this.ChatBox.Text = "";
            // 
            // ChatBoxInput
            // 
            this.ChatBoxInput.Location = new System.Drawing.Point(12, 521);
            this.ChatBoxInput.Name = "ChatBoxInput";
            this.ChatBoxInput.Size = new System.Drawing.Size(737, 28);
            this.ChatBoxInput.TabIndex = 12;
            this.ChatBoxInput.Text = "";
            this.ChatBoxInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatBoxInput_KeyDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 561);
            this.Controls.Add(this.ChatBoxInput);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.ADDDW);
            this.Controls.Add(this.ADDGC);
            this.Controls.Add(this.PositionBox);
            this.Controls.Add(this.DWBox);
            this.Controls.Add(this.DWLabel);
            this.Controls.Add(this.GCBox);
            this.Controls.Add(this.GCLabel);
            this.Controls.Add(this.UsernameBox);
            this.MaximumSize = new System.Drawing.Size(1050, 600);
            this.MinimumSize = new System.Drawing.Size(640, 390);
            this.Name = "Form1";
            this.Text = "SEND";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.Label GCLabel;
        private System.Windows.Forms.TextBox GCBox;
        private System.Windows.Forms.Label DWLabel;
        private System.Windows.Forms.TextBox DWBox;
        private System.Windows.Forms.TextBox PositionBox;
        private System.Windows.Forms.Button ADDGC;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button ADDDW;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Star_Name;
        private System.Windows.Forms.RichTextBox ChatBox;
        private System.Windows.Forms.RichTextBox ChatBoxInput;
    }
}
