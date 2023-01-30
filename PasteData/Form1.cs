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

namespace PasteData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.Control && e.KeyValue == 86)
            //{
            //    e.Handled = true;
            //    e.SuppressKeyPress = true;

            //}
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyValue == 86)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                //object obj = Clipboard.();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //object obj = Clipboard.GetDataObject();
            //PasteFromExcel();
            string message= ExecuteSendRequest();
            richTextBox1.Text = message;
        }

        private void PasteFromExcel()
        {
            DataTable tbl = new DataTable();
            tbl.TableName = "ImportedTable";
            tbl.Columns.Add("Code");
            tbl.Columns.Add("Name");
            tbl.Columns.Add("Check");
            tbl.Columns.Add("Amount");
            tbl.Columns.Add("Date");
            List<string> data = new List<string>(Clipboard.GetText().Split('\n'));
            bool firstRow = true;

            if (data.Count > 0 && string.IsNullOrWhiteSpace(data[data.Count - 1]))
            {
                data.RemoveAt(data.Count - 1);
            }

            foreach (string iterationRow in data)
            {
                string row = iterationRow;
                if (row.EndsWith("\r"))
                {
                    row = row.Substring(0, row.Length - "\r".Length);
                }

                string[] rowData = row.Split(new char[] { '\r', '\x09' });
                DataRow newRow = tbl.NewRow();
                for (int i = 0; i < rowData.Length; i++)
                {
                    if (i >= tbl.Columns.Count) break;
                    newRow[i] = rowData[i];
                }
                tbl.Rows.Add(newRow);
            }

            dataGridView1.DataSource = tbl;
        }


        public string ExecuteSendRequest()
        {
            try
            {
                WebClient wb = new WebClient();
                
                wb.Headers.Add("content-type", "application/x-www-form-urlencoded");
                wb.Headers.Add("gstin", "27GSPMH2102G1Z1");
                wb.Headers.Add("Authorization", "Basic dGVzdGVycGNsaWVudDpBZG1pbkAxMjM=");
              

                string resultRes = wb.UploadString("https://qa.gsthero.com/auth-server/oauth/token", "POST", "grant_type=password&username=gstinapi.sandbox@gsthero.com&password=12345678&client_id=testerpclient");

                return resultRes;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
