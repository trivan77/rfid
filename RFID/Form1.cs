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
using System.Data.SqlClient;

namespace RFID
{
     public partial class Form1 : Form
     {
          string strCon = "Data Source=.;Initial Catalog=RFID;Integrated Security=True";
          Image File;
          string source_Pic;
          public Form1()
          {
               InitializeComponent();
               Control.CheckForIllegalCrossThreadCalls = false;
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
               myPort.PortName = "COM3";
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
               scanCard.IsBackground = true;
               scanCard.Start();
          }

          private void txtMaThe_TextChanged(object sender, EventArgs e)
          {

               txtMaND.Text = "";
               txtHoTen.Text = "";
               txtNgayS.Text = "";
               txtThangS.Text = "";
               txtNamS.Text = "";
               txtSDT.Text = "";
               txtDiaChi.Text = "";
               txtQueQuan.Text = "";
               txtNgayC.Text = "";
               txtThangC.Text = "";
               txtNamC.Text = "";

               pbAnh.Image = null;

               SqlConnection con = new SqlConnection(strCon);

               con.Open();

               string qry_Infor_IDCard = "select cu.idCard,u.idUser,u.hoTen,u.ngaySinh,u.sdt,u.diaChi,u.queQuan,c.ngayCap,u.linkAnh" +
                    " from _CARD as c, CARD_USER as cu, _USER as u " +
                    "where cu.idUser = u.idUser and c.idCard = cu.idCard and c.idCard='" + txtMaThe.Text + "'";
               SqlCommand cmd_Infor_IDCard = new SqlCommand(qry_Infor_IDCard, con);
               SqlDataAdapter a_Infor_IDCard = new SqlDataAdapter(cmd_Infor_IDCard);

               DataTable t_Infor_IDCard = new DataTable();
               a_Infor_IDCard.Fill(t_Infor_IDCard);

               foreach (DataRow row in t_Infor_IDCard.Rows)
               {
                    txtMaND.Text = row["idUser"].ToString();
                    txtHoTen.Text = row["hoTen"].ToString();
                    txtSDT.Text = row["sdt"].ToString();
                    txtDiaChi.Text = row["diaChi"].ToString();
                    txtQueQuan.Text = row["queQuan"].ToString();

                    //string thangCap = row["ngayCap"].ToString().Substring(0,2);
                    //string ngayCap = row["ngayCap"].ToString().Substring(3, 2);
                    //string namCap = row["ngayCap"].ToString().Substring(6, 4);

                    string[] Cap = row["ngayCap"].ToString().Split('/');
                    string ngayCap = Cap[1];
                    string thangCap = Cap[0];
                    string namCap = Cap[2].Substring(0, 4);

                    string[] Sinh = row["ngaySinh"].ToString().Split('/');
                    string thangSinh = Sinh[0];
                    string ngaySinh = Sinh[1];
                    string namSinh = Sinh[2].Substring(0, 4);

                    txtNgayS.Text = ngaySinh;
                    txtThangS.Text = thangSinh;
                    txtNamS.Text = namSinh;

                    txtNgayC.Text = ngayCap;
                    txtThangC.Text = thangCap;
                    txtNamC.Text = namCap;

                    pbAnh.Image = Image.FromFile(row["linkAnh"].ToString());
               }

               if (txtHoTen.Text == "")
               {
                    btnDangKy.Enabled = true;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
               }
               else
               {
                    btnDangKy.Enabled = false;
                    btnSua.Enabled = true;
                    btnXoa.Enabled = true;
               }

               con.Close();

          }

          #region button

          private void btnSua_Click(object sender, EventArgs e)
          {
               SqlConnection con = new SqlConnection(strCon);
               con.Open();

               string ngaySinh = txtThangS.Text + "-" + txtNgayS.Text + "-" + txtNamS.Text;

               string qry_update = "update _USER " +
                    "set hoTen = '" + txtHoTen.Text + "', ngaySinh = '" + ngaySinh + "', sdt = '" + txtSDT.Text + "', diaChi = '" + txtDiaChi.Text + "', queQuan = '" + txtQueQuan.Text + "' " +
                    "where idUser = '" + txtMaND.Text + "'";
               SqlCommand cmd_update = new SqlCommand(qry_update, con);
               cmd_update.ExecuteNonQuery();

               con.Close();
          }

          private void btnXoa_Click(object sender, EventArgs e)
          {
               SqlConnection con = new SqlConnection(strCon);
               con.Open();

               string qry_idCard = "select idCard from CARD_USER where idUser='" + txtMaND.Text + "'";
               SqlCommand cmd_idCard = new SqlCommand(qry_idCard, con);
               string idCard = cmd_idCard.ExecuteScalar().ToString();

               string qry_delete = "delete from CARD_USER where idUser='" + txtMaND.Text + "'";
               SqlCommand cmd_delete = new SqlCommand(qry_delete, con);
               cmd_delete.ExecuteNonQuery();

               string qry_deleteUser = "delete from _USER where idUser='" + txtMaND.Text + "'";
               SqlCommand cmd_deleteUser = new SqlCommand(qry_deleteUser, con);
               cmd_deleteUser.ExecuteNonQuery();

               //string qry_idCard = "select idCard from CARD_USER where idUser='" + txtMaND.Text + "'";
               //SqlCommand cmd_idCard = new SqlCommand(qry_idCard, con);
               //string idCard = cmd_idCard.ExecuteScalar().ToString();

               string qry_deleteCard = "delete from _CARD where idCard='" + idCard + "'";
               SqlCommand cmd_deleteCard = new SqlCommand(qry_deleteCard, con);
               cmd_deleteCard.ExecuteNonQuery();

               con.Close();
          }

          private void btnDangKy_Click(object sender, EventArgs e)
          {
               SqlConnection con = new SqlConnection(strCon);
               con.Open();

               string ngaySinh = txtThangS.Text + "-" + txtNgayS.Text + "-" + txtNamS.Text;
               string ngayCap = txtThangC.Text + "-" + txtNgayC.Text + "-" + txtNamC.Text;
               
               string qry_insertUser = "INSERT INTO [dbo].[_USER] ([hoTen],[ngaySinh],[sdt],[queQuan],[diaChi],[linkAnh]) " +
                    "VALUES('"+txtHoTen.Text+"','"+ngaySinh+"','"+txtSDT.Text+"','"+txtQueQuan.Text+"','"+txtDiaChi.Text+"','"+source_Pic+"')";
               string qry_insertCard = "INSERT INTO [dbo].[_CARD] ([idCard],[ngayCap],[idNV]) VALUES('"+txtMaThe.Text+"','"+ngayCap+"',1)";
               string qry_insert = "INSERT INTO[dbo].[CARD_USER] ([idCard],[idUser]) VALUES(,)";
               
               SqlCommand cmd_update = new SqlCommand(qry_update, con);
               cmd_update.ExecuteNonQuery();

               con.Close();
          }

          #endregion 
     }
}
