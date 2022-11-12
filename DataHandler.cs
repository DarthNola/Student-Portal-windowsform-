﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRG_282_Project
{
    internal class DataHandler
    {
        SqlConnection con = new SqlConnection("Data Source = MSI\\SQLEXPRESS; Initial Catalog = Student_Details; Integrated Security = true;");
        public BindingSource readData(string query)
        {
            string qry = query;
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            BindingSource source = new BindingSource();
            source.DataSource = reader;
            //dataGridView1.DataSource = source;
            con.Close();
            return source;
        }

        public void CloseConnection()
        {
            con.Close();
        }

        public DataSet searchStudents(string query)
        {
            DataSet set = new DataSet();
            try
            {
                string qry = query;
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return set;

        }

        public void DML_procedures(string query)
        {
            try
            {
                string qry = query;
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }


        }

        public void Load_Image(string Image_Path, string ID, DataGridView dgv)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "PNG Files(*.png)|*.png|All Files(*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string picPath = dlg.FileName.ToString();
                Image_Path = picPath;
            }

            byte[] imageBt = null;
            FileStream fstream = new FileStream(Image_Path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fstream);
            imageBt = br.ReadBytes((int)fstream.Length);
            string qry = @"UPDATE STUDENTS SET ST_Image =" + "@IMG \n WHERE StudentID= " + ID;
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataReader reader;
            if (ID != "")
            {
                try
                {
                    con.Open();
                    cmd.Parameters.Add(new SqlParameter("@IMG", imageBt));
                    reader = cmd.ExecuteReader();
                    MessageBox.Show("Inserted image");
                    con.Close();
                    Image_Path = "";
                    dgv.DataSource = readData("SELECT * FROM Students");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please select a item in Data Grid");
            }
            dgv.AutoResizeRows();
        }

        public DataTable getLecutres()
        {
            string query = @"SELECT Name,  Surname , Module_Name FROM Lectures FULL OUTER JOIN Codes ON Lectures.Module_Code = Codes.Module_Code;";
            SqlDataAdapter reader = new SqlDataAdapter(query, con);


            DataTable datatable = new DataTable();

            reader.Fill(datatable);
            return datatable;






        }
    }
}