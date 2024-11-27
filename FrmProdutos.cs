using System;
using System.Data;
using System.Windows.Forms;
using static TeleBerço.DsProdutos;

namespace TeleBerço
{
    public partial class FrmProdutos : Form
    {
        public DataRow RowSelecionada { get; set; }
        private FrmDados frmDados = new FrmDados();
        private DsProdutos dsArtigos = new DsProdutos();


        public FrmProdutos()
        {
            InitializeComponent();
        }

        private void FrmProdutos_Load(object sender, EventArgs e)
        {
            CarregarMarcasECategorias();
            TxtCodigoPr.Focus();
            if (RowSelecionada != null)
            {
                dsArtigos.CarregaArtigos();
                CarregarProdutoSelecionado();
                HabilitarCampos();
            }
            else
            {
                PrepararNovoProduto();
            }

        }

        private void CarregarMarcasECategorias()
        {
            try
            {
                dsArtigos.CarregaCategorias();
                dsArtigos.CarregarMarcas();

                txtMarca.DataSource = dsArtigos.Marcas;
                txtMarca.DisplayMember = "Nome";
                txtMarca.ValueMember = "Id";

                txtModelo.DataSource = dsArtigos.Categorias;
                txtModelo.DisplayMember = "Nome";
                txtModelo.ValueMember = "CodCat";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparFormulario()
        {
            TxtCodigoPr.Text = string.Empty;
            TxtNomeProduto.Text = string.Empty;
            TxtObservacao.Text = string.Empty;
            TxtCusto.Text = string.Empty;
            TxtPreco.Text = string.Empty;
            txtImei.Text = string.Empty;
            txtTipoPr.Text = string.Empty;
            txtMarca.SelectedIndex = -1;
            txtModelo.SelectedIndex = -1;
        }

        private void HabilitarCampos()
        {
            TxtCodigoPr.Enabled = true;
            TxtNomeProduto.Enabled = true;
            TxtObservacao.Enabled = true;
            TxtCusto.Enabled = true;
            TxtPreco.Enabled = true;
            txtImei.Enabled = true;
            txtTipoPr.Enabled = true;
            txtMarca.Enabled = true;
            txtModelo.Enabled = true;

            BtnGravar.Enabled = true;
        }

        private void DesabilitarCampos()
        {
            TxtCodigoPr.Enabled = false;
            TxtNomeProduto.Enabled = false;
            TxtObservacao.Enabled = false;
            TxtCusto.Enabled = false;
            TxtPreco.Enabled = false;
            txtImei.Enabled = false;
            txtTipoPr.Enabled = false;
            txtMarca.Enabled = false;
            txtModelo.Enabled = false;

            BtnGravar.Enabled = false;
        }

        private bool ValidarPreenchimento()
        {
            return !string.IsNullOrWhiteSpace(TxtCodigoPr.Text) &&
                   !string.IsNullOrWhiteSpace(TxtNomeProduto.Text) &&
                   !string.IsNullOrWhiteSpace(txtMarca.Text) &&
                   !string.IsNullOrWhiteSpace(txtModelo.Text) &&
                   !string.IsNullOrWhiteSpace(TxtCusto.Text);

        }

        private void CarregarProdutoSelecionado()
        {
            try
            {
                var produtoRow = (ProdutosRow)RowSelecionada;
                if (produtoRow != null)
                {
                    TxtCodigoPr.Text = produtoRow.CodPr;
                    PreencherProduto();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepararNovoProduto()
        {
            try
            {
                dsArtigos.NovoArtigo();
                LimparFormulario();
                TxtCodigoPr.Text = dsArtigos.DaProxCodArtigo();
            }
            catch( Exception ex )
            {
                MessageBox.Show($"Erro ao preparar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PreencherProduto()
        {
            try
            {
                var produtoRow = dsArtigos.PesquisarArtigo(TxtCodigoPr.Text);

                if (produtoRow.CodPr != dsArtigos.DaProxCodArtigo())
                {
                    TxtNomeProduto.Text = produtoRow.NomeProduto;
                    TxtObservacao.Text = produtoRow.Observacao;
                    TxtCusto.Text = produtoRow.PrecoCusto.ToString("F2");
                    TxtPreco.Text = produtoRow.PreçoVenda.ToString("F2");
                    txtImei.Text = produtoRow.IMEI;
                    txtTipoPr.Text = produtoRow.Tipo;
                    txtMarca.SelectedValue = produtoRow.Marcas;
                    txtModelo.SelectedValue = produtoRow.Categorias;
                }
                else
                {
                    LimparFormulario();
                    TxtCodigoPr.Text = produtoRow.CodPr;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao preencher produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtnNovo_Click(object sender, EventArgs e)
        {
            try
            {
                PrepararNovoProduto();
                HabilitarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar novo produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarPreenchimento())
                {
                    ProdutosRow produtoRow = dsArtigos.Produtos[0];

                    produtoRow.CodPr = TxtCodigoPr.Text;
                    produtoRow.NomeProduto = TxtNomeProduto.Text;
                    produtoRow.Observacao = TxtObservacao.Text;
                    produtoRow.PrecoCusto = decimal.Parse(TxtCusto.Text);
                    produtoRow.PreçoVenda = decimal.Parse(TxtPreco.Text);
                    produtoRow.IMEI = txtImei.Text;
                    produtoRow.Tipo = txtTipoPr.Text;
                    produtoRow.Marcas = (int)txtMarca.SelectedValue;
                    produtoRow.Categorias = txtModelo.SelectedValue.ToString();

                    dsArtigos.UpdateArtigos();

                    MessageBox.Show("Produto salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparFormulario();
                    DesabilitarCampos();

                }
                else
                {
                    MessageBox.Show("Por favor, preencha corretamente todos os campos .", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gravar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TxtCodigoPr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                frmDados.MostrarTabelaDados("DsArtigos");
                if (frmDados.DialogResult == DialogResult.OK)
                {
                    CarregarProdutoSelecionado();
                    HabilitarCampos();
                }
            }
        }
        private void txtMarca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                FrmDados frmDados = new FrmDados();
                frmDados.MostrarTabelaDados("DsMarcas");
                dsArtigos.CarregarMarcas();
            }
        }

        private void txtModelo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                FrmDados frmDados = new FrmDados();
                frmDados.MostrarTabelaDados("DsCategorias");
                dsArtigos.CarregaCategorias();
            }
        }

        private void TxtCodigoPr_Leave(object sender, EventArgs e)
        {
            PreencherProduto();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                var resultado = MessageBox.Show("Deseja realmente excluir este Produto?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    TxtCodigoPr.Select();
                    dsArtigos.EliminarPr(TxtCodigoPr.Text);
                    LimparFormulario();
                    DesabilitarCampos();
                    MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Produto não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
