using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DentalClinicAutomation
{
    public partial class Appointment : Form
    {
        int _appointmentId;
        DbService myConn = new DbService();

        public Appointment(int _adminId)
        {
            InitializeComponent();
        }

        private void X_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Appointments Appointment = new Appointments();
            try
            {
                var patientID = cbNameSurname.SelectedValue;
                var treatmentID = cbTreatment.SelectedValue;
                var userID = cbDentist.SelectedValue;
                string query = "insert into Appointment (PatientID, TreatmentID, AppointmentDate, AppointmentTime, DoctorID)" + "values('" + patientID + "','" + treatmentID + "','" + dtDate.Text + "','" + cbHour.Text + "','" + userID + "')";
                Appointment.AddAppointment(query);
                MessageBox.Show("Randevu başarıyla eklendi.");
                UpdateDG();
                Reset();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage(_appointmentId);
            this.Hide();
            mainPage.ShowDialog();

        }

        private void btnPatient_Click(object sender, EventArgs e)
        {
            Patient patient = new Patient(_appointmentId);
            this.Hide();
            patient.ShowDialog();
        }

        private void btnTreatment_Click(object sender, EventArgs e)
        {
            Treatment treatment = new Treatment(_appointmentId);
            this.Hide();
            treatment.ShowDialog();
        }

        private void btnPrescriptions_Click(object sender, EventArgs e)
        {
            Prescriptions prescriptions = new Prescriptions(_appointmentId);
            this.Hide();
            prescriptions.ShowDialog();
        }

        private void UpdateCBPatient()
        {
            //SOLID prensipleri, 
            SqlConnection conn = myConn.GetConn();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT PatientID, PatientName FROM Patient", conn);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("PatientName", typeof(string));
            dt.Columns.Add("PatientID", typeof(string));

            dt.Load(reader);
            cbNameSurname.ValueMember = "PatientID";
            cbNameSurname.DisplayMember = "PatientName";
            cbNameSurname.DataSource = dt;
            conn.Close();
        }

        private void UpdateCBDentist()
        {
            SqlConnection conn = myConn.GetConn();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT UserID,NameSurname FROM Users WHERE UserTypeID = 2", conn);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("NameSurname", typeof(string));
            dt.Columns.Add("UserID", typeof(string));

            dt.Load(reader);
            cbDentist.ValueMember = "UserID";
            cbDentist.DisplayMember = "NameSurname";
            cbDentist.DataSource = dt;
            conn.Close();
        }

        private void UpdateCBTreatment()
        {
            SqlConnection conn = myConn.GetConn();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT TreatmentID,TreatmentName FROM Treatment", conn);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("TreatmentName", typeof(string));
            dt.Columns.Add("TreatmentID", typeof(string));

            dt.Load(reader);
            cbTreatment.ValueMember = "TreatmentID";
            cbTreatment.DisplayMember = "TreatmentName";
            cbTreatment.DataSource = dt;
            conn.Close();
        }

        void UpdateDG()
        {
            Appointments appointments = new Appointments();
            string query = "select a.AppointmentID, p.PatientName as HastaAdı, a.AppointmentDate RandevuTarihi, a.AppointmentTime RandevuSaati, t.TreatmentName TedaviAdı, u.NameSurname as DoktorAdı from Appointment a\r\ninner join Patient p on  a.PatientID = p.PatientID\r\ninner join Treatment t on t.TreatmentID = a.TreatmentID\r\ninner join Users u on a.DoctorID = u.UserID\r\n";
            DataSet ds = appointments.ShowAppointment(query);
            dgAppointment.DataSource = ds.Tables[0];
        }

        void Reset()
        {
            cbNameSurname.Text = "";
            dtDate.Text = "";
            cbHour.Text = "";
            cbTreatment.Text = "";
            cbDentist.Text = "";
        }

        private void Appointment_Load(object sender, EventArgs e)
        {
            UpdateCBDentist();
            UpdateCBPatient();
            UpdateCBTreatment();
            UpdateDG();
            Reset();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgAppointment.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgAppointment.SelectedRows[0];
                Appointments appointments = new Appointments();
                try
                {
                    string query = $"UPDATE Appointment set PatientID = '{cbNameSurname.SelectedValue}', AppointmentDate = '{dtDate.Text}', AppointmentTime = '{cbHour.Text}', TreatmentID = '{cbTreatment.SelectedValue}', DoctorID = '{cbDentist.SelectedValue}'  WHERE AppointmentID = {row.Cells["AppointmentID"].Value}";
                    appointments.DeleteAppointment(query);
                    MessageBox.Show("Randevu başarıyla güncellendi.");
                    UpdateDG();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgAppointment.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgAppointment.SelectedRows[0];
                Appointments appointments = new Appointments();
                try
                {
                    string query = $"DELETE FROM Appointment WHERE AppointmentID= {row.Cells["AppointmentID"].Value}";
                    appointments.DeleteAppointment(query);
                    MessageBox.Show("Randevu başarıyla silindi.");
                    UpdateDG();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void dgAppointment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satıra tıklandığından emin olun
            {
                DataGridViewRow row = dgAppointment.Rows[e.RowIndex];
                cbNameSurname.Text = row.Cells["HastaAdı"].Value.ToString();
                dtDate.Text = row.Cells["RandevuTarihi"].Value?.ToString();
                cbHour.Text = row.Cells["RandevuSaati"].Value?.ToString();
                cbTreatment.Text = row.Cells["TedaviAdı"].Value?.ToString();
                cbDentist.Text = row.Cells["DoktorAdı"].Value.ToString();
            }
        }

        private void Filter()
        {
            Appointments appointments = new Appointments();
            string query = "select a.AppointmentID, p.PatientName as HastaAdı, p.PatientName, a.AppointmentDate, a.AppointmentTime, t.TreatmentName, u.NameSurname from Appointment a\r\ninner join Patient p on  a.PatientID = p.PatientID\r\ninner join Treatment t on t.TreatmentID = a.TreatmentID\r\ninner join Users u on a.DoctorID = u.UserID\r\nWHERE p.PatientName LIKE '%" + txtSearch.Text + "%'";
            DataSet ds = appointments.ShowAppointment(query);
            dgAppointment.DataSource = ds.Tables[0];
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void __Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnAppointment_Click(object sender, EventArgs e)
        {

        }

        private void dgAppointment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
