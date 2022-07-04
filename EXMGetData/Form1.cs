using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ivi.Visa.Interop; //EXM commands
using System.Threading;

namespace EXMGetData
{
    public partial class Form1 : Form
    {
        private Ivi.Visa.Interop.FormattedIO488 ioTestSet;
 

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ioTestSet = new FormattedIO488();
                this.textBoxAllInfo.Text = "Starting connection.... \r\n";
                ResourceManager grm = new ResourceManager();
                ioTestSet.IO = (IMessage)grm.Open("AGILENT_EXT", AccessMode.NO_LOCK, 2000, "");            
                
            }
            catch           
            {
                ioTestSet = null;
                MessageBox.Show("Falha ao conectar com equipamento EXM!!!");
            }


            if (ioTestSet != null) {

                this.textBoxAllInfo.Text += "!!!connected!!! \r\n";

                button4.BackColor = Color.Green;

                //textBoxAllInfo.Text += "Command: *IDN? \r\n";
                ioTestSet.WriteString("*IDN?", true);
                textBoxAllInfo.Text += "EXM SERIAL: " + ioTestSet.ReadString() + "\r\n";

                Thread.Sleep(200);

                //textBoxAllInfo.Text += "Command: :SYSTem:MODule:NAME? \r\n";
                ioTestSet.WriteString(":SYSTem:MODule:NAME?", true);
                textBoxAllInfo.Text += "MODULE Nº: " + ioTestSet.ReadString() + "\r\n";

                Thread.Sleep(200);

                //textBoxAllInfo.Text += "Command: *:SYSTem:MODule:SERial? \r\n";
                ioTestSet.WriteString(":SYSTem:MODule:SERial?", true);
                textBoxAllInfo.Text += "MODULE SERIAL: " + ioTestSet.ReadString() + "\r\n";

                Thread.Sleep(200);

                //textBoxAllInfo.Text += "Command: *OPT? \r\n";
                ioTestSet.WriteString("*OPT?", true);
                textBoxAllInfo.Text += "LICENSE: " + ioTestSet.ReadString() + "\r\n";

                Thread.Sleep(200);

                //textBoxAllInfo.Text += "Command: *INST:CAT? \r\n";
                ioTestSet.WriteString("INST:CAT?", true);
                textBoxAllInfo.Text += "OPTIONS (TDD/FDD): " + ioTestSet.ReadString() + "\r\n";
             

                Thread.Sleep(200);

                //textBoxAllInfo.Text += "Command: :SYSTem:HID? \r\n";
                ioTestSet.WriteString(":SYSTem:HID?", true);
                textBoxAllInfo.Text += ioTestSet.ReadString() + "\r\n";
                

           
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxAllInfo.Clear();
        }

    }
}
