using System;
using System.Data;
using System.Windows.Forms;
using static TeleBerço.DsClientes;

namespace TeleBerço
{
    public partial class FrmClientes : Form
    {
        private DsClientes dsClientes = new DsClientes();

        private FrmDados frmDados = new FrmDados();
        public DataRow RowSelecionada { get; set; }

        public FrmClientes()
        {
            InitializeComponent();
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            if (RowSelecionada != null)
            {
                CarregarClienteSelecionado();
            }
            else
            {
                PrepararNovoCliente();
            }
        }

        private void CarregarClienteSelecionado()
        {
            try
            {
                var clienteRow = (ClientesRow)RowSelecionada;
                TxtCodigoCl.Text = clienteRow.CodCl;
                PreencherCliente();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepararNovoCliente()
        {
            try
            {
                dsClientes.NovoCliente();
                LimparFormulario();              
                TxtCodigoCl.Text = dsClientes.DaProxNrCliente();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao preparar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PreencherCliente()
        {
            try
            {
                var clienteRow = dsClientes.PesquisaCliente(TxtCodigoCl.Text);

                if (clienteRow.CodCl != dsClientes.DaProxNrCliente())
                {
                    TxtNomeCl.Text = clienteRow.Nome;
                    TxtTelefone.Text = clienteRow.Telefone;
                    TxtEmail.Text = clienteRow.Email;

                    TxtCodigoCl.Focus();
                }
                else
                {
                    LimparFormulario();
                    TxtCodigoCl.Text = clienteRow.CodCl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao preencher cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparFormulario()
        {
            TxtCodigoCl.Text = string.Empty;
            TxtNomeCl.Text = string.Empty;
            TxtTelefone.Text = string.Empty;
            TxtEmail.Text = string.Empty;
        }

        private void HabilitarCampos()
        {
            TxtCodigoCl.Enabled = true;
            TxtNomeCl.Enabled = true;
            TxtTelefone.Enabled = true;
            TxtEmail.Enabled = true;

            BtnGravar.Enabled = true;
            BtnEliminar.Enabled = true;
        }

        private void DesabilitarCampos()
        {
            TxtCodigoCl.Enabled = false;
            TxtNomeCl.Enabled = false;
            TxtTelefone.Enabled = false;
            TxtEmail.Enabled = false;

            BtnGravar.Enabled = false;
            BtnEliminar.Enabled = false;
        }

        private bool ValidarPreenchimento()
        {
            return !string.IsNullOrWhiteSpace(TxtCodigoCl.Text) &&
                   !string.IsNullOrWhiteSpace(TxtNomeCl.Text) &&
                   !string.IsNullOrWhiteSpace(TxtTelefone.Text);
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            PrepararNovoCliente();
        }

        private void BtnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarPreenchimento())
                {
                    ClientesRow novoCliente = dsClientes.Clientes[0];

                    novoCliente.CodCl = TxtCodigoCl.Text;
                    novoCliente.Nome = TxtNomeCl.Text;
                    novoCliente.Telefone = TxtTelefone.Text;
                    novoCliente.Email = TxtEmail.Text;

                    dsClientes.UpdateClientes();

                    MessageBox.Show("Cliente salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparFormulario();
                    DesabilitarCampos();
                }
                else
                {
                    MessageBox.Show("Por favor, preencha todos os campos corretamente.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gravar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(TxtCodigoCl.Text))
                {
                    var resultado = MessageBox.Show("Deseja realmente excluir este cliente?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)

                    {
                        TxtCodigoCl.Focus();
                        dsClientes.EliminarCliente(TxtCodigoCl.Text);

                        MessageBox.Show("Cliente excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparFormulario();
                        DesabilitarCampos();
                    }
                    else
                    {
                        MessageBox.Show("Cliente não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                else
                {
                    MessageBox.Show("Nenhum cliente selecionado para exclusão.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao encerrar formulario: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void TxtCodigoCl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4) 
            {
                frmDados.MostrarTabelaDados("DsClientes");
                if (frmDados.DialogResult == DialogResult.OK)
                {
                    CarregarClienteSelecionado();
                }
            }
        }

        private void TxtCodigoCl_Leave(object sender, EventArgs e)
        {
            PreencherCliente();
        }


    }
}
