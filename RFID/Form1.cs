using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;

namespace RFID
{
     public partial class Form1 : Form
     {
          Image File;
          string source_Pic;
          public Form1()
          {
               InitializeComponent();

          }

          private void linkAnh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
          {
               OpenFileDialog f = new OpenFileDialog();
               f.Filter = "JPG(*.JPG)|*.jpg";

               if (f.ShowDialog() == DialogResult.OK)
               {
                    source_Pic = f.FileName;
                    File = Image.FromFile(f.FileName);
                    pbAnh.Image = File;
               }
          }

          private void getID()
          {
               SerialPort myPort = new SerialPort();
               myPort.BaudRate = 9600;
               myPort.PortName = "COM4";
               myPort.Open();

               while (true)
               {

                    string data = myPort.ReadLine();


                    Console.WriteLine(data.LastIndexOf("UID:"));
                    if (data.Contains("UID") == true)
                    {
                         txtMaThe.Text = data.Substring(10, 11);
                         //id = data.Substring(10, 11);
                    }


               //     if (id == "53 04 5E 1D")
               //     {
               //          pictureBox1.Image = new Bitmap("C:\\image\\Binh.jpg");
               //          id = "";
               //     }

               //     else if (id == "87 BF A8 4B")
               //     {
               //          pictureBox1.Image = new Bitmap("C:\\image\\Hiep.jpg");
               //          id = "";
               //     }

               //     else if (id == "0A BC 62 1A")
               //     {
               //          pictureBox1.Image = new Bitmap("C:\\image\\Hoang.jpg");
               //          id = "";
               //     }

               //     else if (id == "24 0A B2 24")
               //     {
               //          pictureBox1.Image = new Bitmap("C:\\image\\Dat.jpg");

               //     }

               }


          }

          private void Form1_Load(object sender, EventArgs e)
          {
               Thread scanCard = new Thread(getID);
          }
     }
}
