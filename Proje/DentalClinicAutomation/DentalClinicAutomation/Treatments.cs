using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DentalClinicAutomation
{
    internal class Treatments
    {
        public void AddTreatment(string query)
        {
            DbService MyConnection = new DbService();
            SqlConnection conn = MyConnection.GetConn();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void DeleteTreatment(string query)
        {
            DbService MyConnection = new DbService();
            SqlConnection conn = MyConnection.GetConn();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void UpdateTreatment(string query)
        {
            DbService MyConnection = new DbService();
            SqlConnection conn = MyConnection.GetConn();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public DataSet ShowTreatment(string query)
        {
            DbService MyConnection = new DbService();
            DataSet ds = new DataSet();

            // SqlConnection ve SqlCommand için using blokları kullanılıyor
            using (SqlConnection conn = MyConnection.GetConn())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                try
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
            return ds;
        }
    }
}
