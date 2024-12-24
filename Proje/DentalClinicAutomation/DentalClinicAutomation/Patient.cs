using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DentalClinicAutomation
{
    public partial class Patient : Form
    {
        DbService dbService;
        int _patientId;
        public Patient(int adminId)
        {
            InitializeComponent();
            _patientId = adminId;
            dbService = new DbService();
        }

        public Patient()
        {
            InitializeComponent();
        }
        void UpdateDG()
        {
            Patients patients = new Patients();
            string query = "SELECT PatientID, PatientName, PatientPhone, PatientAddress, Birthday, Gender, Allergy FROM Patient";
            DataSet ds = patients.ShowPatient(query);
            dgPatient.DataSource = ds.Tables[0];
        }
        void Reset()
        {
            txtNameSurname.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtAllergy.Text = "";
            cbGender.Text = "";
            dtBirthDate.Text = "";
        }
        void Filter()
        {
            Patients patients = new Patients();
            string query = "SELECT * FROM Patient WHERE PatientName LIKE '%"+txtSearch.Text+"%'";
            DataSet ds = patients.ShowPatient(query);
            dgPatient.DataSource = ds.Tables[0];
        }
        private void Patient_Load(object sender, EventArgs e)
        {
            UpdateDG();
        }

        private void X_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage(_patientId);
            mainPage.ShowDialog();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Patients Patient = new Patients();
            try
            {
                string query = "insert into Patient (PatientName, PatientPhone, PatientAddress, Birthday, Gender, Allergy)" + "values('" + txtNameSurname.Text + "','" + txtPhone.Text + "','" + txtAddress.Text + "','" + dtBirthDate.Value + "','" + cbGender.SelectedItem.ToString() + "','" + txtAllergy.Text + "')";
                Patient.AddPatient(query);
                MessageBox.Show("Hasta başarıyla eklendi.");
                UpdateDG();
                Reset();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void dgPatient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satıra tıklandığından emin olun
            {
                DataGridViewRow row = dgPatient.Rows[e.RowIndex];
                txtNameSurname.Text = row.Cells["PatientName"].Value.ToString();
                txtPhone.Text = row.Cells["PatientPhone"].Value?.ToString();
                txtAddress.Text = row.Cells["PatientAddress"].Value?.ToString();
                dtBirthDate.Text = row.Cells["Birthday"].Value?.ToString();
                cbGender.Text = row.Cells["Gender"].Value?.ToString();
                txtAllergy.Text = row.Cells["Allergy"].Value?.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgPatient.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgPatient.SelectedRows[0];
                Patients patients = new Patients();
                try
                {
                    string query = $"DELETE FROM Patient WHERE PatientID= {row.Cells["PatientID"].Value}";
                    patients.DeletePatient(query);
                    MessageBox.Show("Hasta başarıyla silindi.");
                    UpdateDG();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgPatient.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgPatient.SelectedRows[0];
                Patients patients = new Patients();
                try
                {
                    string query = $"UPDATE Patient set PatientName = '{txtNameSurname.Text}', PatientPhone='{txtPhone.Text}', PatientAddress = '{txtAddress.Text}', Birthday = '{dtBirthDate.Text}', Gender = '{cbGender.SelectedItem.ToString()}', Allergy = '{txtAllergy.Text}'  WHERE PatientID = {row.Cells["PatientID"].Value}";
                    patients.DeletePatient(query);
                    MessageBox.Show("Hasta bilgileri başarıyla güncellendi.");
                    UpdateDG();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void btnAppointment_Click(object sender, EventArgs e)
        {
            Appointment appointment = new Appointment(_patientId);
            this.Hide();
            appointment.ShowDialog();
        }

        private void btnTreatment_Click(object sender, EventArgs e)
        {
            Treatment treatment = new Treatment(_patientId);
            this.Hide();
            treatment.ShowDialog();
        }

        private void btnPrescriptions_Click(object sender, EventArgs e)
        {
            Prescriptions prescriptions = new Prescriptions(_patientId);
            this.Hide();
            prescriptions.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void __Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnPatient_Click(object sender, EventArgs e)
        {

        }
    }
}

