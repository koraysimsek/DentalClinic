using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DentalClinicAutomation
{
    public partial class Treatment : Form
    {
        DbService dbService;
        int _treatmentId;
        public Treatment(int _adminId)
        {
            InitializeComponent();
        }
        void UpdateDG()
        {
            Treatments treatments = new Treatments();
            string query = "SELECT TreatmentID, TreatmentName, TreatmentFee, TreatmentDescription FROM Treatment";
            DataSet ds = treatments.ShowTreatment(query);
            dgTreatment.DataSource = ds.Tables[0];
        }
        void Reset()
        {
            txtTreatment.Text = "";
            txtFee.Text = "";
            txtDescription.Text = "";
        }
        private void Treatment_Load(object sender, EventArgs e)
        {
            UpdateDG();
        }
        private void X_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            Treatments Treatment = new Treatments();

            try
            {
                // Ücretin sayısal bir değer olup olmadığını kontrol et
                if (int.TryParse(txtFee.Text, out int treatmentFee))
                {
                    // Eğer geçerli bir sayı ise, sorguyu çalıştır
                    string query = "INSERT INTO Treatment (TreatmentName, TreatmentFee, TreatmentDescription) " +
                                   "VALUES ('" + txtTreatment.Text + "', '" + treatmentFee + "', '" + txtDescription.Text + "')";
                    Treatment.AddTreatment(query);

                    // Başarılı mesajı ve işlemler
                    MessageBox.Show("Tedavi başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateDG(); // DataGridView'i güncelle
                    Reset();    // Alanları sıfırla
                }
                else
                {
                    // Eğer sayı değilse hata mesajı göster
                    MessageBox.Show("Lütfen 'Tutar' alanına sadece sayısal bir değer giriniz!", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Hata mesajını kullanıcıya göster
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgTreatment.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgTreatment.SelectedRows[0];
                Treatments treatments = new Treatments();
                try
                {
                    // txtFee'nin sayısal olup olmadığını kontrol et
                    if (int.TryParse(txtFee.Text, out int treatmentFee))
                    {
                        // Eğer geçerliyse tedavi güncelleme işlemini yap
                        string query = $"UPDATE Treatment SET TreatmentName = '{txtTreatment.Text}', TreatmentFee = '{treatmentFee}', TreatmentDescription = '{txtDescription.Text}' WHERE TreatmentID = {row.Cells["TreatmentID"].Value}";
                        treatments.DeleteTreatment(query);

                        // İşlem başarılı mesajı
                        MessageBox.Show("Tedavi başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // DataGridView'i güncelle ve alanları sıfırla
                        UpdateDG();
                        Reset();
                    }
                    else
                    {
                        // Sayısal değer girilmemişse hata mesajı göster
                        MessageBox.Show("Lütfen 'Tutar' alanına sadece sayısal bir değer giriniz!", "Geçersiz Giriş", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Genel bir hata oluştuğunda mesajı göster
                    MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir tedavi seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgTreatment.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgTreatment.SelectedRows[0];
                Treatments treatments = new Treatments();
                try
                {
                    string query = $"DELETE FROM Treatment WHERE TreatmentID = {row.Cells["TreatmentID"].Value}";
                    treatments.DeleteTreatment(query);
                    MessageBox.Show("Tedavi başarıyla silindi.");
                    UpdateDG();
                    Reset();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            MainPage mainPage = new MainPage(_treatmentId);
            mainPage.ShowDialog();
            this.Close();
        }

        private void dgTreatment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Geçerli bir satıra tıklandığından emin olun
            {
                DataGridViewRow row = dgTreatment.Rows[e.RowIndex];
                txtTreatment.Text = row.Cells["TreatmentName"].Value.ToString();
                txtFee.Text = row.Cells["TreatmentFee"].Value?.ToString();
                txtDescription.Text = row.Cells["TreatmentDescription"].Value?.ToString();
            }
        }

        private void btnPatient_Click(object sender, EventArgs e)
        {
            Patient patient = new Patient(_treatmentId);
            this.Hide();
            patient.ShowDialog();
        }

        private void btnAppointment_Click(object sender, EventArgs e)
        {
            Appointment appointment = new Appointment(_treatmentId);
            this.Hide();
            appointment.ShowDialog();
        }

        private void btnTreatment_Click(object sender, EventArgs e)
        {
            Treatment treatment = new Treatment(_treatmentId);
            this.Hide();
            treatment.ShowDialog();
        }

        private void btnPrescriptions_Click(object sender, EventArgs e)
        {
            Prescriptions prescriptions = new Prescriptions(_treatmentId);
            this.Hide();
            prescriptions.ShowDialog();
        }
        void Filter()
        {
            Treatments treatments = new Treatments();
            string query = "SELECT * FROM Treatment WHERE TreatmentName LIKE '%" + txtSearch.Text + "%'";
            DataSet ds = treatments.ShowTreatment(query);
            dgTreatment.DataSource = ds.Tables[0];
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Filter();
        }

        private void __Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
