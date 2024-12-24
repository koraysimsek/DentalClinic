using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DentalClinicAutomation
{
    public class DbService
    {
        private string _connectionString;
        public DbService()
        {
            _connectionString = "Server=KORAY;Database=DentalClinic;Trusted_Connection=True;";
        }
        public Dictionary<int, int> Login(string username, string password)
        {
            Dictionary<int, int> data = new Dictionary<int, int>();
            int userId = 0;
            int userTypeId = 0;
            try
            {
                // SQL Server bağlantısı için SqlConnection kullanılır
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT \"UserID\", \"UserTypeId\" FROM \"Users\" WHERE \"Username\" = @username AND \"Password\" = @password";

                        // Parametreleri ekleyerek sorguyu SQL Server'a uygun hale getiriyoruz
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        // SqlDataReader kullanarak veri okuma işlemi
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0);
                                userTypeId = reader.GetInt32(1);
                            }
                        }
                        // Verileri Dictionary'ye ekle
                        data.Add(userTypeId, userId);
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                data.Add(0, 0);
                return data;
            }
        }
        public SqlConnection GetConn()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Server=KORAY;Database=DentalClinic;Trusted_Connection=True;";
            return conn;
        }
        private int GetPatientID(string patientName)
        {
            string query = "SELECT PatientID FROM Patient WHERE PatientName = @PatientName";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PatientName", patientName);
                    int patientID = (int)cmd.ExecuteScalar();  // PatientID'yi döndürüyor
                    return patientID;
                }
            }
        }

        private int GetTreatmentID(string treatmentName)
        {
            string query = "SELECT TreatmentID FROM Treatment WHERE TreatmentName = @TreatmentName";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TreatmentName", treatmentName);
                    int treatmentID = (int)cmd.ExecuteScalar();  // TreatmentID'yi döndürüyor
                    return treatmentID;
                }
            }
        }

        private int GetPatientDetails(string nameSurname)
        {
            string query = "SELECT UserID FROM Users WHERE NameSurname = @NameSurname";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NameSurname", nameSurname);
                    int userID = (int)cmd.ExecuteScalar();  // UserID'yi döndürüyor
                    return userID;
                }
            }
        }
    }
}
