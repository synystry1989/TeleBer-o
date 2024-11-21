using System;
using System.Data;
using System.Windows.Forms;
using TeleBerço;
using TeleBerço.DsClientesTableAdapters;

namespace TeleBerço
{
    public partial class FrmClientes : Form
    {
        private DsClientes dsClientes = new DsClientes();
        private ClientesTableAdapter clientesTableAdapter = new ClientesTableAdapter();
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
                var clienteRow = (DsClientes.ClientesRow)RowSelecionada;
                TxtCodigoCl.Text = clienteRow.CodCl;
                TxtNomeCl.Text = clienteRow.Nome;
                TxtTelefone.Text = clienteRow.Telefone;
                TxtEmail.Text = clienteRow.Email;

                HabilitarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepararNovoCliente()
        {
            LimparFormulario();
            DesabilitarCampos();
            TxtCodigoCl.Text = "CL";
        }

    
        private void PreencherCliente()
        {
            if (!string.IsNullOrEmpty(TxtCodigoCl.Text))
            {
                var clienteRow = dsClientes.PesquisaCliente(TxtCodigoCl.Text);

                if ((clienteRow != null) && (clienteRow.CodCl!=""))

                {
                    TxtNomeCl.Text = clienteRow.Nome;
                    TxtTelefone.Text = clienteRow.Telefone;
                    TxtEmail.Text = clienteRow.Email;
                    HabilitarCampos();
                }
                else
                {
                    MessageBox.Show("Cliente não encontrado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimparFormulario();
                    DesabilitarCampos();
                    TxtCodigoCl.Text ="CL";
                }
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
            LimparFormulario();
            TxtCodigoCl.Text = dsClientes.DaProxNrCliente();
            HabilitarCampos();
        }

        private void BtnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarPreenchimento())
                {
                    var novoCliente = dsClientes.Clientes.NewClientesRow();
                    novoCliente.CodCl = TxtCodigoCl.Text;
                    novoCliente.Nome = TxtNomeCl.Text;
                    novoCliente.Telefone = TxtTelefone.Text;
                    novoCliente.Email = TxtEmail.Text;

                    dsClientes.Clientes.AddClientesRow(novoCliente);
                    clientesTableAdapter.Update(dsClientes.Clientes);

                    MessageBox.Show("Cliente salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparFormulario();
                    DesabilitarCampos();
                }
                else
                {
                    MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        var clienteRow = dsClientes.Clientes.FindByCodCl(TxtCodigoCl.Text);

                        if (clienteRow != null)
                        {
                            clienteRow.Delete();
                            clientesTableAdapter.Update(dsClientes.Clientes);

                            MessageBox.Show("Cliente excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimparFormulario();
                            DesabilitarCampos();
                        }
                        else
                        {
                            MessageBox.Show("Cliente não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
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
            this.Close();
        }

          
        private void TxtCodigoCl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                if (frmDados.DialogResult == DialogResult.OK)
                {
                    frmDados.MostrarTabelaDados("DsClientes");
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
