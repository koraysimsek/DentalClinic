using System;
using System.Linq;
using System.Windows.Forms;

namespace DentalClinicAutomation
{
    public partial class Login : Form
    {
        private DbService dbService;
        public Login()
        {
            InitializeComponent();
            dbService = new DbService();
        }
        private void X_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPassword.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {


            string username = txtUsername.Text;
            string password = txtPassword.Text;
            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                var result = dbService.Login(username, password).FirstOrDefault();
                int type = result.Key;
                if (type == 0)
                {
                    MessageBox.Show("Kullanıcı bulunamamıştır. Kullanıcı adı veya şifre hatalı");
                }
                else if (type == 1)
                {
                    // doktor
                    MainPage admin = new MainPage(result.Value);
                    this.Hide();
                    admin.ShowDialog();
                }
                else if (type == 2)
                {
                    // sekreter
                    Dentist dentist = new Dentist(result.Value);
                    this.Hide();
                    dentist.ShowDialog();
                }
                else if (type == 3)
                {
                    // muhasebeci

                }
            }
          
        }

        private void __Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
