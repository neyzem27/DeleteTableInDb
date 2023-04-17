using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;

namespace HataDuzeltme
{
    public partial class Form1 : Form
    {
        // Getting connection string from App.config file
        string StrCon = ConfigurationManager.ConnectionStrings["hedefDb"].ToString();//silinecek db
        string anaDB = ConfigurationManager.ConnectionStrings["anaDB"].ToString();//ana db
        /*gercek ortam db tanımları
             <add name="hedefDb" connectionString="Data Source=192.168.1.219\SQLEXPJUMP;Initial Catalog=MikroDB_V16_2022;User ID =sa;Password =Sa1234;Integrated Security=True"
                providerName="System.Data.SqlClient" />
            <add name="anaDB" connectionString="MikroDB_V16_001"
                providerName="System.Data.SqlClient" />
          </connectionStrings>
         * */
        public Form1()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // string XMlFile = txtFilePath.Text;
            try
            {
                if (!txtSeri.Text.Equals("") && !txtSira.Text.Equals(""))
                {
                    /* // Conversion Xml file to DataTable
                     DataTable dt = CreateDataTableXML(XMlFile);
                     if (dt.Columns.Count == 0)
                         dt.ReadXml(XMlFile);
                    */
                    // Creating Query for Table Creation
                    //string Query = CreateTableQuery(dt);  ----@"Data Source=192.168.1.219\SQLEXPJUMP;Initial Catalog=MikroDB_V16_004;User ID =sa;Password =Sa1234;Integrated Security=True"
                    SqlConnection con = new SqlConnection(StrCon);//
                    con.Open();

                    // Deletion of Table 
                    SqlCommand cmd = new SqlCommand("delete from  [dbo].STOK_HAREKETLERI where concat(sth_evrakno_seri,sth_evrakno_sira) in(SELECT concat(sth_evrakno_seri,sth_evrakno_sira) FROM "+ anaDB + ".[dbo].STOK_HAREKETLERI WHERE sth_fat_uid in(select cha_Guid  from "+ anaDB + ".[dbo].[CARI_HESAP_HAREKETLERI] where cha_evrakno_seri='"+ txtSeri.Text.ToUpper() + "' and cha_evrakno_sira in ("+txtSira.Text.ToUpper()+")));", con);
                    int check = cmd.ExecuteNonQuery();
                    SqlCommand cmd2 = new SqlCommand("DELETE from [dbo].CARI_HESAP_HAREKETLERI WHERE cha_evrakno_seri='"+ txtSeri.Text.ToUpper() + "' and cha_evrakno_sira in ("+txtSira.Text.ToUpper()+");", con);
                    int check2 = cmd2.ExecuteNonQuery();
                    SqlCommand cmd3 = new SqlCommand("delete from [dbo].MUHASEBE_FISLERI where fis_tic_evrak_seri='"+ txtSeri.Text.ToUpper() + "' and fis_tic_evrak_sira in("+ txtSira.Text.ToUpper() + ");", con);
                    int check3 = cmd3.ExecuteNonQuery();

                    if (check != 0 && check2 != 0)
                    {
                        // Copy Data from DataTable to Sql Table

                        txtSeri.Text = "";
                        txtSira.Text = "";
                        MessageBox.Show("işlem başarıyla yapılmıştır.");
                    }
                    else
                    {
                        MessageBox.Show("Data düzeltmesi yapılmadı.2 sebepten olur.1)db ayarları 2)yanlış kod girilmesi");
                    }
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Lütfen seri ve sıra alanlarını doldurun.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İşlem başarısız.Hata: " + ex);
            }

        }
    }
}
