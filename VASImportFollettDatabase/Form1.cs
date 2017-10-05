using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VASImportFollettDatabase
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnImportXls_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            DataTable sourceData = new DataTable();

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            using (OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + dlg.FileName + "'; Extended Properties='Excel 8.0;HDR=Yes'"))
            using (OleDbCommand command = new OleDbCommand(@"SELECT * FROM [Sheet1$]", connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                sourceData.Locale = System.Globalization.CultureInfo.CurrentCulture;
                adapter.Fill(sourceData);
            }

            int rowsInserted = 0;

            using (SqlConnection con = new SqlConnection(@"Data Source=prd-sqllis-01;Initial Catalog=PackingSlips;Integrated Security=True"))
            {
                SqlCommand insertCommand = new SqlCommand("INSERT INTO [PackingSlips].[dbo].[FollettOrderData] ([PT] ,[Ref] ,[Style] ,[NewStyle] ,[Logo] ,[Color] ,[Size] ,[SKU] ,[Price] ,[Qty]) VALUES (@PT ,@Ref ,@Style ,@NewStyle ,@Logo ,@Color ,@Size ,@SKU ,@Price ,@Qty)", con);
                con.Open();
                foreach (DataRow dr in sourceData.Rows)
                {
                    try
                    {
                        if (dr["PT#"].ToString().Trim().Length == 0)
                            continue;

                        insertCommand.Parameters.Clear();
                        insertCommand.Parameters.AddWithValue("@PT", dr["PT#"].ToString().Trim());
                        insertCommand.Parameters.AddWithValue("@Ref", dr["Ref"].ToString().Trim());
                        insertCommand.Parameters.AddWithValue("@Style", dr["Style"].ToString().Trim());
                        insertCommand.Parameters.AddWithValue("@NewStyle", dr["New Style"].ToString().Trim());
                        insertCommand.Parameters.AddWithValue("@Logo", dr["Logo"].ToString().Trim());
                        insertCommand.Parameters.AddWithValue("@Color", dr["Color"].ToString().Trim());
                        insertCommand.Parameters.AddWithValue("@Size", dr["Size"].ToString().Trim());
                        insertCommand.Parameters.AddWithValue("@SKU", dr["SKU"].ToString().Trim());
                        insertCommand.Parameters.AddWithValue("@Price", dr["Price"].ToString().Trim());
                        insertCommand.Parameters.AddWithValue("@Qty", dr["Qty"].ToString().Trim());
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Error parsing XLS document: " + ex.Message);
                        return;
                    }

                    try
                    {
                        if (insertCommand.ExecuteNonQuery() == 0)
                        {
                            MessageBox.Show("Unable to insert row");
                            continue;
                        }
                        rowsInserted++;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Exception inserting row: " + ex.Message);
                        continue;
                    }
                }
                con.Close();

                MessageBox.Show("Successfully inserted " + rowsInserted.ToString() + " rows");
            }
        }
    }
}
