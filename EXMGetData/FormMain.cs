using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ivi.Visa.Interop; //Equipment
using System.Threading;
using System.IO;

namespace EXMGetData
{
    public partial class FormMain : Form
    {
        private Ivi.Visa.Interop.FormattedIO488 ioTestSet;
        public FormMain()
        {
            InitializeComponent();
        }
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void buttonGetInfo_Click(object sender, EventArgs e)
        {
            getEXMInfos();
        }
        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxAllInfo.Clear();
        }

        ////////////////////////////////////Functions/////////////////////////////////

        private bool sendCmd(string cmd, string msg)
        {
            try
            {
                ioTestSet.WriteString(cmd, true);
                textBoxAllInfo.Text += msg + ioTestSet.ReadString() + "\r\n\r\n";
                Thread.Sleep(200);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void getEXMInfos()
        {
            string dir = string.Empty;
            string fileName = string.Empty;
            ioTestSet = new FormattedIO488();
            ResourceManager grm = new ResourceManager();
            bool status = false;
            string errorMessage = "Erro ao conectar com equipamento EXM!!! ";
            try
            {
                this.textBoxAllInfo.Text = "Starting connection.... \r\n";
                ioTestSet.IO = (IMessage)grm.Open("AGILENT_EXT", AccessMode.NO_LOCK, 2000, "");
            }
            catch (Exception ex)
            {
                ioTestSet = null;
                MessageBox.Show(errorMessage + ex.Message);
            }

            if (ioTestSet != null)
            {
                buttonLed.BackColor = Color.Green;
                labelStatus.Text = "connected!!";

                status = sendCmd("*IDN?", "-->EXM SERIAL: ");
                if (status)
                    status = sendCmd(":SYSTem:MODule:NAME?", "-->MODULE Nº: ");

                if (status)
                    status = sendCmd(":SYSTem:MODule:SERial?", "-->MODULE SERIAL: ");

                if (status)
                    status = sendCmd("*OPT?", "-->LICENSE: ");

                if (status)
                    status = sendCmd(":SYSTem:LICense:MODule:HID?", "-->");

                if (status)
                    status = sendCmd("INST:CAT?", "-->OPTIONS (TDD/FDD): ");

                if (status)
                    status = sendCmd(":SYSTem:CONFigure:HARDware?", "");
                if (status)
                {
                    dir = textBoxDir.Text;
                    fileName = textBoxFile.Text;

                    if (fileName == "")
                        MessageBox.Show("!!!!Preencha o campo File Name!!!");

                    else
                    {
                        if (!System.IO.Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        File.WriteAllText(dir + "\\" + fileName, textBoxAllInfo.Text);
                    }
                }
                else
                    MessageBox.Show(errorMessage);
            }
        }
    }
}
