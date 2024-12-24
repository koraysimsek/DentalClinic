using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DentalClinicAutomation
{
    public partial class Prescriptions : Form
    {
        int _prescriptionsId;
        private string selectedAppointmentID;

        public Prescriptions(int _adminId)
        {
            InitializeComponent();
        }
        void UpdateDG()
        {
            Prescriptionss prescriptionss = new Prescriptionss();
            string query = "select pr.PrescriptionID, p.PatientName, t.TreatmentName, t.TreatmentFee, pr.Medicine, pr.TreatmentAmount, a.AppointmentDate + a.AppointmentTime as Date FROM Prescription pr\r\ninner join Patient p on  pr.PatientID = p.PatientID\r\ninner join Treatment t on t.TreatmentID = pr.TreatmentID\r\ninner join Appointment a on a.TreatmentID = t.TreatmentID";
            DataSet ds = prescriptionss.ShowPrescriptions(query);
            dgPrescriptions.DataSource = ds.Tables[0];
        }
        void Reset()
        {
            cbPatientName.Text = "";
            txtAmount.Text = "";
            txtMedicine.Text = "";
            txtTreatment.Text = "";
            txtTreatmentFee.Text = "";
        }

        private void Prescriptions_Load(object sender, EventArgs e)
        {
            UpdateDG();
            UpdateCBPatient();
            Reset();
        }

        private void X_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage(_prescriptionsId);
            mainPage.ShowDialog();
            this.Close();
        }

        private void btnPatient_Click(object sender, EventArgs e)
        {
            Patient patient = new Patient(_prescriptionsId);
            this.Hide();
            patient.ShowDialog();
        }

        private void btnAppointment_Click(object sender, EventArgs e)
        {
            Appointment appointment = new Appointment(_prescriptionsId);
            this.Hide();
            appointment.ShowDialog();
        }

        private void btnTreatment_Click(object sender, EventArgs e)
        {
            Treatment treatment = new Treatment(_prescriptionsId);
            this.Hide();
            treatment.ShowDialog();
        }
        DbService myConn = new DbService();
        private void UpdateCBPatient()
        {
            SqlConnection conn = myConn.GetConn();
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT PatientID, PatientName FROM Patient", conn);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("PatientName", typeof(string));
            dt.Columns.Add("PatientID", typeof(string));

            dt.Load(reader);
            cbPatientName.ValueMember = "PatientID";
            cbPatientName.DisplayMember = "PatientName";
            cbPatientName.DataSource = dt;
            conn.Close();
        }

        private void GetCBDate()
        {
            try
            {
                // Seçilen hasta ID'sini al
                string selectedPatientId = cbPatientName.SelectedValue.ToString();

                // SQL bağlantısını aç
                using (SqlConnection conn = myConn.GetConn())
                {
                    conn.Open();

                    // Sadece seçilen hasta ID'sine göre randevuları getiren sorgu
                    string query = "SELECT (AppointmentDate + ' ' + AppointmentTime) as AppointmentDate , TreatmentID FROM Appointment WHERE PatientID = @PatientID";
                    //string query = "select p.PatientName, t.TreatmentName, t.TreatmentFee, pr.Medicine, pr.TreatmentAmount FROM Prescription pr\r\ninner join Patient p on  pr.PatientID = p.PatientID\r\ninner join Treatment t on t.TreatmentID = pr.TreatmentID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PatientID", selectedPatientId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Verileri DataTable içine yükle
                            DataTable dt = new DataTable();
                            dt.Columns.Add("AppointmentDate", typeof(string));
                            dt.Columns.Add("AppointmentID", typeof(string));
                            dt.Load(reader);

                            // ComboBox ayarları
                            cbDate.ValueMember = "TreatmentID";
                            cbDate.DisplayMember = "AppointmentDate";
                            cbDate.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void UpdateCBTreatment()
        {
            string selectedAppointmentId = cbDate.SelectedValue.ToString();

            // SQL bağlantısını aç
            using (SqlConnection conn = myConn.GetConn())
            {
                conn.Open();

                // Seçilen AppointmentID'ye göre tedavi bilgisini getiren sorgu
                string query = "SELECT TreatmentName FROM Treatment WHERE AppointmentID = @AppointmentID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", selectedAppointmentId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // TextBox'a tedavi adını yaz
                            txtTreatment.Text = reader["TreatmentName"].ToString();
                        }
                        else
                        {
                            txtTreatment.Text = string.Empty; // Eğer veri bulunamazsa, TextBox'ı temizle
                        }
                    }
                }
            }
        }

    
        private void btnSave_Click(object sender, EventArgs e)
        {
            Prescriptionss Prescriptions = new Prescriptionss();
            try
            {
                // Miktar (txtAmount) alanına girilen değeri sayısal olarak kontrol et
                if (int.TryParse(txtAmount.Text, out int amount))
                {
                    var treatmentId = cbDate.SelectedValue;
                    // Eğer sayısal değer doğruysa, veri ekleme işlemini yap
                    string query = "insert into Prescription (PatientID, TreatmentID, Medicine, TreatmentAmount)" +
                                   "values('" + cbPatientName.SelectedValue + "','" + treatmentId + "','" + txtMedicine.Text +"','" + txtAmount.Text + "')";
                    Prescriptions.AddPrescriptions(query);
                    MessageBox.Show("Reçete başarıyla eklendi.");
                    UpdateDG();
                    Reset();
                }
                else
                {
                    // Eğer miktar sayısal değilse, kullanıcıya hata mesajı göster
                    MessageBox.Show("Lütfen 'Miktar' alanına sadece sayısal bir değer giriniz!", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

        }

        private void cbPatientName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //UpdateCBPrice();
            GetCBDate();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgPrescriptions.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgPrescriptions.SelectedRows[0];
                Prescriptionss prescriptionss = new Prescriptionss();
                try
                {
                    string query = $"DELETE FROM Prescription WHERE PrescriptionID = {row.Cells["PrescriptionID"].Value}";
                    prescriptionss.DeletePrescriptions(query);
                    MessageBox.Show("Reçete başarıyla silindi.");
                    UpdateDG();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void dgPrescriptions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satıra tıklandığından emin olun
            {
                DataGridViewRow row = dgPrescriptions.Rows[e.RowIndex];
                cbPatientName.Text = row.Cells["PatientName"].Value.ToString();
                txtTreatment.Text = row.Cells["TreatmentName"].Value?.ToString();
                txtTreatmentFee.Text = row.Cells["TreatmentFee"].Value?.ToString();
                txtMedicine.Text = row.Cells["Medicine"].Value?.ToString();
                txtAmount.Text = row.Cells["TreatmentAmount"].Value?.ToString();
            }
        }

        Bitmap bitmap;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            int height = dgPrescriptions.Height;
            dgPrescriptions.Height = dgPrescriptions.RowCount * dgPrescriptions.RowTemplate.Height * 2;
            bitmap = new Bitmap(dgPrescriptions.Width, dgPrescriptions.Height);
            dgPrescriptions.DrawToBitmap(bitmap, new Rectangle(0, 10, dgPrescriptions.Width, dgPrescriptions.Height));
            dgPrescriptions.Height = height;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        void Filter()
        {
            Prescriptionss prescriptionss = new Prescriptionss();
            string query = "SELECT p.PatientName, t.TreatmentName, t.TreatmentFee, pr.Medicine, pr.TreatmentAmount FROM Prescription pr\r\ninner join Treatment t on t.TreatmentID = pr.TreatmentID\r\ninner join Patient p on  pr.PatientID = p.PatientID\r\nWHERE PatientName LIKE '%" + txtSearch.Text + "%'";
            DataSet ds = prescriptionss.ShowPrescriptions(query);
            dgPrescriptions.DataSource = ds.Tables[0];
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void __Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void cbDate_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbDate.SelectedItem != null)
            {
                var treatmentId = cbDate.SelectedValue;
                SqlConnection conn = myConn.GetConn();
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Treatment WHERE TreatmentID = '" + treatmentId + "'", conn);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    txtTreatmentFee.Text = dr["TreatmentFee"].ToString();
                    txtTreatment.Text = dr["TreatmentName"].ToString();
                }
                conn.Close();
            }
        }

        private string GetSelectedAppointmentID()
        {
            return selectedAppointmentID; // Seçilen AppointmentID'yi döndürüyoruz
        }
    }
}
