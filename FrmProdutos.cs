using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TeleBerço.DsProdutosTableAdapters;

namespace TeleBerço
{
    public partial class FrmProdutos : Form
    {
        public DataRow RowSelecionada { get; set; }
        private FrmDados frmDados = new FrmDados();
        private DsProdutos dsArtigos = new DsProdutos();
        private ProdutosTableAdapter produtosTableAdapter = new ProdutosTableAdapter();

        public FrmProdutos()
        {
            InitializeComponent();
        
        }

        private void FrmProdutos_Load(object sender, EventArgs e)
        {
            CarregarMarcasECategorias();

            if (RowSelecionada != null)
            {
                CarregarProdutoSelecionado();
                HabilitarCampos();
            }
            else
            {
                PrepararNovoProduto();
                DesabilitarCampos();
            }
        }
       
        private void CarregarMarcasECategorias()
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

     

        private void CarregarProdutoSelecionado()
        {
            try
            {
                var produtoRow = (DsProdutos.ProdutosRow)RowSelecionada;

                TxtCodigoPr.Text = produtoRow.CodPr;
                TxtNomeProduto.Text = produtoRow.NomeProduto;
                TxtObservacao.Text = produtoRow.Observacao;
                TxtCusto.Text = produtoRow.PrecoCusto.ToString("F2");
                TxtPreco.Text = produtoRow.PreçoVenda.ToString("F2");
                txtImei.Text = produtoRow.IMEI;
                txtTipoPr.Text = produtoRow.Tipo;

                txtMarca.SelectedValue = produtoRow.Marcas;
                txtModelo.SelectedValue = produtoRow.Categorias;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrepararNovoProduto()
        {
            LimparFormulario();
            TxtCodigoPr.Text = "PR";
        }

        private void PreencherProduto()
        {
            try
            {
                if (!string.IsNullOrEmpty(TxtCodigoPr.Text))
                {
                    var produtoRow = dsArtigos.PesquisarArtigo(TxtCodigoPr.Text);

                    if ((produtoRow != null)&&( produtoRow.CodPr!=""))
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
                        MessageBox.Show("Produto não encontrado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LimparFormulario();
                        
                        TxtCodigoPr.Text = dsArtigos.DaProxCodArtigo();
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao preencher produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                   txtMarca.SelectedValue != null &&
                   txtModelo.SelectedValue != null;
        }

     

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            try
            {
                PrepararNovoProduto();
                 TxtCodigoPr.Text=dsArtigos.DaProxCodArtigo()  ;
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
                    var produtoRow = dsArtigos.Produtos.FindByCodPr(TxtCodigoPr.Text);

                    if (produtoRow == null)
                    {
                        produtoRow = dsArtigos.Produtos.NewProdutosRow();
                        produtoRow.CodPr = TxtCodigoPr.Text;
                        dsArtigos.Produtos.AddProdutosRow(produtoRow);
                    }

                    produtoRow.NomeProduto = TxtNomeProduto.Text;
                    produtoRow.Observacao = TxtObservacao.Text;
                    produtoRow.PrecoCusto = decimal.Parse(TxtCusto.Text);
                    produtoRow.PreçoVenda = decimal.Parse(TxtPreco.Text);
                    produtoRow.IMEI = txtImei.Text;
                    produtoRow.Tipo = txtTipoPr.Text;
                    produtoRow.Marcas = (int)txtMarca.SelectedValue;
                    produtoRow.Categorias = txtModelo.SelectedValue.ToString();

                    produtosTableAdapter.Update(dsArtigos.Produtos);

                    MessageBox.Show("Produto salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show($"Erro ao gravar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

       
        private void TxtCodigoPr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                frmDados.MostrarTabelaDados("DsArtigos");
                if (frmDados.DialogResult == DialogResult.OK)
                {
                    RowSelecionada = frmDados.RowSelecionada;
                    CarregarProdutoSelecionado();
                    HabilitarCampos();
                }
            }
        }

        private void TxtCodigoPr_Leave(object sender, EventArgs e)
        {
            PreencherProduto();
         
        }

        
    }
}
