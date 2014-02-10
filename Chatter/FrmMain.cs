using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chatter
{
    public partial class FrmMain : Form
    {
        #region Interface details
        public FrmMain()
        {
            InitializeComponent();
        }
        private void input_KeyPress(object sender, KeyPressEventArgs e)
        {
            ((TextBox)sender).BackColor = Color.White;
        }
        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            SendMessage(txtMessage.Text.Trim());
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendMessage(txtMessage.Text.Trim());
                e.Handled = true;
            }
        }

        delegate void DisplayMessageCallback(string message);
        private void DisplayMessage(string message)
        {
            if (this.lbMessages.InvokeRequired)
            {
                DisplayMessageCallback d = new DisplayMessageCallback(DisplayMessage);
                this.Invoke(d, new object[] { message });
            }
            else
            {
                this.lbMessages.Items.Add(message);
            }
        }
        #endregion

        private void SendMessage(string message)
        {

        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            gbChannel.Enabled = false;
            //setup connections
        }

        private void btnJoinChannel_Click(object sender, EventArgs e)
        {
            bool valid = true;
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                valid = false;
                txtName.BackColor = Color.OrangeRed;
            }
            if (String.IsNullOrWhiteSpace(txtChannelName.Text))
            {
                valid = false;
                txtChannelName.BackColor = Color.OrangeRed;
            }
            if (!valid) return;
            string channelName = txtChannelName.Text.Trim().ToUpperInvariant();

            //setup our consumer



            gbChannel.Enabled = true;
            //announce our entrance to the channel
            SendMessage(String.Format("{0} has joined {1}", txtName.Text, channelName));
        }


    }
}
