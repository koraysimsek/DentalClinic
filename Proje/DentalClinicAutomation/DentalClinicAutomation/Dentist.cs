using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DentalClinicAutomation
{
    public partial class Dentist : Form
    {
        int _adminId;
        public Dentist(int adminId)
        {
            InitializeComponent();
            _adminId = adminId;

        }

        private void __Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void X_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void UpdateDG()
        {
            Patients patients = new Patients();
            string query = $"select p.PatientName HastaAdı, a.AppointmentDate RandevuTarihi, a.AppointmentTime RandevuSaati, t.TreatmentName TedaviAdı from Appointment a\r\ninner join Patient p on  a.PatientID = p.PatientID\r\ninner join Treatment t on t.TreatmentID = a.TreatmentID\r\ninner join Users u on a.DoctorID = u.UserID WHERE DoctorID = {_adminId}";
            DataSet ds = patients.ShowPatient(query);
            dgAppointment.DataSource = ds.Tables[0];
        }

        private void Dentist_Load(object sender, EventArgs e)
        {
            UpdateDG();
            updateDentist();
        }

        void updateDentist()
        {
            DbService myConn = new DbService();
            SqlConnection conn = myConn.GetConn();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE UserID = '" + _adminId + "'", conn);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                lblDentistName.Text = dr["NameSurname"].ToString();
            }
            conn.Close();

        }
    }
}

