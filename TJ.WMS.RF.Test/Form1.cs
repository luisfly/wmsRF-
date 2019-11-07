using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TJ.WMS.RF.Service;
using System.Runtime.InteropServices;

namespace TJ.WMS.RF.Test
{
    public partial class Form1 : Form
    {
        string token = "";
        string user_name = "";
        string user_id = "10000";
        DcAccept acpt;
        public Form1()
        {
            InitializeComponent();

            UserLogin login = new UserLogin();

            if (login.Login(user_id, "111111", ref user_name, ref token))
            {
                MessageBox.Show("登录成功!");
            }
            DataTable data = new MainPage(user_id, user_name, token).GetModuleList();
            acpt = new DcAccept(user_id, user_name, token);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    acpt.SetParameter("OrderNO", textBox1.Text.Trim());
                    acpt.ValidateOrderCode();
                    textBox2.Focus();
                }
                catch (RFException rfex)
                {
                    MessageBox.Show(rfex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    acpt.SetParameter("Barcode", textBox2.Text.Trim());
                    acpt.ValidateBarcode();
                    textBox3.Focus();
                }
                catch (RFException rfex)
                {
                    MessageBox.Show(rfex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    acpt.SetParameter("TrayNO", textBox3.Text.Trim());
                    acpt.ValidateTrayNO();
                    textBox4.Focus();
                }
                catch (RFException rfex)
                {
                    MessageBox.Show(rfex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    acpt.SetParameter("Qty", textBox4.Text.Trim());
                    acpt.ValidateQty();
                    textBox5.Focus();
                }
                catch (RFException rfex)
                {
                    MessageBox.Show(rfex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    acpt.SetParameter("ProductDate", textBox5.Text.Trim());
                    acpt.ValidateProductDate();
                    textBox6.Focus();
                }
                catch (RFException rfex)
                {
                    MessageBox.Show(rfex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    acpt.SetParameter("EffectiveDate", textBox6.Text.Trim());
                    acpt.ValidateEffectiveDate();
                    button1.Focus();
                }
                catch (RFException rfex)
                {
                    MessageBox.Show(rfex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                acpt.Accept();
            }
            catch (Exception rfex)
            {
                MessageBox.Show(rfex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
