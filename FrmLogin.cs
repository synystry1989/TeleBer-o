using System;
using System.Windows.Forms;

namespace TeleBerço
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

       

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            chckLembrar.Checked = true;

            if (chckLembrar.Checked)
            {
                txtUser.Text = "BrunoFernandes";
                txtPass.Text = "123";
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "BrunoFernandes" && txtPass.Text == "123")
            {
                FrmDocumentos frmDocumentos = new FrmDocumentos();
                MessageBox.Show("Login Efetuado com sucesso", "Confirmacao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                frmDocumentos.ShowDialog();

            }
            else if (txtUser.Text == "" || txtPass.Text == "")
            {
                MessageBox.Show("Preencha ambos os campos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Utilizador nao encontrado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
    }
}


