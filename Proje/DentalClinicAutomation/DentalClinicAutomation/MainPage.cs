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
    public partial class MainPage : Form
    {
        DbService dbService;
        int _adminId;
        public MainPage(int adminId)
        {
            InitializeComponent();
            _adminId = adminId;
            dbService = new DbService();
        }

        private void MainPage_Load(object sender, EventArgs e)
        {

        }

        private void btnPatient_Click(object sender, EventArgs e)
        {
            Patient patient = new Patient(_adminId);
            this.Hide();
            patient.ShowDialog();
        }

        private void X_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnAppointment_Click(object sender, EventArgs e)
        {
            Appointment appointment = new Appointment(_adminId);
            this.Hide();
            appointment.ShowDialog();
        }

        private void btnTreatment_Click(object sender, EventArgs e)
        {
            Treatment treatment = new Treatment(_adminId);
            this.Hide();
            treatment.ShowDialog();
        }

        private void btnPrescriptions_Click(object sender, EventArgs e)
        {
            Prescriptions prescriptions = new Prescriptions(_adminId);
            this.Hide();
            prescriptions.ShowDialog();
        }
    }
}
