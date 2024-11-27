using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TeleBerço.DsProdutosTableAdapters;
using static TeleBerço.DsClientes;
using static TeleBerço.DsProdutos;

namespace TeleBerço
{
    public partial class FrmDocumentos : Form
    {
        // Datasets e TableAdapters
        private DsClientes dsClientes = new DsClientes();
        private QuerryProdutosTableAdapter querryProdutosTableAdapter = new QuerryProdutosTableAdapter();
        // Variáveis de controle
        private PrintDocument printDocument = new PrintDocument();

        public FrmDocumentos()
        {
            InitializeComponent();
        }

        private void FrmDocumentos_Load(object sender, EventArgs e)
        {
            try
            {
                FormatarTabelas();
                CarregarDadosIniciais();
                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar o formulário: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarDadosIniciais()
        {
            try
            {
                dsProdutos.CarregaCategorias();
                dsProdutos.CarregarMarcas();
                dsDocumentos.CarregaTipoDoc();
                TxtCodigoDoc.Focus();
                // Configurar ComboBoxes

                TxtCodigoDoc.Text = "";


            }

            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PreencheDocumento(string tipoDoc, int nrDoc)
        {
            try
            {
                var rowPesquisada = dsDocumentos.PesquisaDocumento(tipoDoc, nrDoc);

                if (rowPesquisada.Cliente != "")
                {
                    LimparProduto();

                    // Preencher dados do produto associado
                    if (!rowPesquisada.IsNull("CodProduto"))
                    {
                        var produtoRow = dsProdutos.PesquisarArtigo(rowPesquisada.CodProduto);
                        if (produtoRow.CodPr != dsProdutos.DaProxCodArtigo())
                        {
                            txtEquipNome.Text = produtoRow.NomeProduto;
                            txtCat.Text = querryProdutosTableAdapter.NomeCategoria(produtoRow.Categorias).ToString();
                            txtMarca.Text = querryProdutosTableAdapter.NomeMarca(produtoRow.Marcas);
                            txtImei.Text = produtoRow.IMEI;

                        }
                    }

                    // Preencher dados do cliente associado
                    var clienteRow = dsClientes.PesquisaCliente(rowPesquisada.Cliente);
                    if (clienteRow.CodCl != dsClientes.DaProxNrCliente())
                    {
                        TxtCodigoCl.Text = clienteRow.CodCl;
                        TxtNomeCl.Text = clienteRow.Nome;
                        TxtTelefone.Text = clienteRow.Telefone;
                        TxtEmail.Text = clienteRow.Email;
                    }

                    // Preencher dados do documento
                    DataMod.Value = rowPesquisada.DataRececao;
                    dateTimePicker1.Value = rowPesquisada.DataEntrega;
                    txtTotal.Text = rowPesquisada.Total.ToString("F2");
                    txtObservacoes.Text = rowPesquisada.Observacoes;
                    txtEstado.Text = rowPesquisada.Estado;
                    txtDesconto.Text = rowPesquisada.Desconto.ToString();

                    // Carregar linhas do documento
                    dsDocumentos.CarregaLinhas(rowPesquisada.ID);
                    AdicionarDescricoesPr();
                    DesabilitarBotoes();
                }
                else
                {
                    NrDoc.Text = dsDocumentos.DaNrDocSeguinte(tipoDoc).ToString();
                    HabilitarBotoes();
                    LimparCliente();
                    txtTotal.Text = "0";
                    txtEstado.Text = "";
                    txtDesconto.Text = "0";
                    LimparProduto();
                    TxtCodigoCl.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao preencher documento: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidaPreenchimentoCliente()
        {
            return !string.IsNullOrWhiteSpace(TxtCodigoCl.Text) &&
                   !string.IsNullOrWhiteSpace(TxtNomeCl.Text) &&
                   !string.IsNullOrWhiteSpace(TxtTelefone.Text);
        }

        private bool ValidaPreenchimentoDocumento()
        {
            return !string.IsNullOrWhiteSpace(TxtCodigoDoc.Text) &&
                   !string.IsNullOrWhiteSpace(NrDoc.Text) &&
                   !string.IsNullOrWhiteSpace(TxtDescricaoDoc.Text) &&
                   !string.IsNullOrWhiteSpace(TxtCodigoCl.Text) &&
                   !string.IsNullOrWhiteSpace(TxtNomeCl.Text) &&
                   !string.IsNullOrWhiteSpace(txtEstado.Text);
        }

        private bool ValidaPreenchimentoProdutos()
        {
            return !string.IsNullOrWhiteSpace(txtCat.Text) &&
                   !string.IsNullOrWhiteSpace(txtMarca.Text) &&
                   !string.IsNullOrWhiteSpace(txtEquipNome.Text) &&
                   !string.IsNullOrWhiteSpace(TxtCusto.Text);
        }

        private void TxtCodigoDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (TxtCodigoDoc.Text != "")
                {
                
                    var tipoRow = dsDocumentos.TipoDocumentos.FindByCodDoc(TxtCodigoDoc.SelectedValue.ToString());

                    if (tipoRow != null)
                    {
                        TxtDescricaoDoc.Text = tipoRow.Descricao;
                        NrDoc.Text = dsDocumentos.DaNrDocSeguinte(TxtCodigoDoc.Text).ToString();

                        HabilitarBotoes();
                        HabilitarCampos();
                        LimparProduto();
                        dsDocumentos.ListaProdutos.Clear();
                    }
                    EstadoDoc();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar tipo de documento: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void TxtCodigoDoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                AbrirSelecaoDocumentos();
                EstadoDoc();
            }
        }

        private void TxtCodigoCl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                AbrirSelecaoClientes();
            }
        }

        private void TxtCodigoCl_Leave(object sender, EventArgs e)
        {
            try
            {
                var clienteRow = dsClientes.PesquisaCliente(TxtCodigoCl.Text);

                if (clienteRow.Nome != "")
                {
                    TxtNomeCl.Text = clienteRow.Nome;
                    TxtTelefone.Text = clienteRow.Telefone;
                    TxtEmail.Text = clienteRow.Email;
                    HabilitarCliente();
                }
                else
                {
                    TxtCodigoCl.Text = dsClientes.DaProxNrCliente().ToString();
                    TxtNomeCl.Enabled = true;
                    BtnGravarCliente.Enabled = true;
                    HabilitarCliente();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao preencher cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnNovoPr_Click(object sender, EventArgs e)
        {
            try
            {

                LimparProduto();
                txtObservacoes.Text = "";
                HabilitarProduto();
                dsProdutos.NovoArtigo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar novo produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGravarPr_Click(object sender, EventArgs e)
        {
            try
            {
                ProdutosRow produtoRow = dsProdutos.Produtos[0];
                if (ValidaPreenchimentoProdutos())
                {

                    if (produtoRow.CodPr == dsProdutos.DaProxCodArtigo())
                    {
                        produtoRow.NomeProduto = txtEquipNome.Text;
                        produtoRow.Categorias = txtCat.SelectedValue.ToString();
                        produtoRow.Marcas = (int)txtMarca.SelectedValue;
                        produtoRow.IMEI = txtImei.Text;
                        produtoRow.Observacao = txtObservacoes.Text;
                        produtoRow.Tipo = txtTipoPr.Text;

                        dsProdutos.UpdateArtigos();

                        MessageBox.Show("Produto salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Preencha corretamente todos os campos .", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gravar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            try
            {
                FrmDados frmDados = new FrmDados();
                frmDados.MostrarTabelaDados("DsArtigos");

                if (frmDados.RowSelecionada is ProdutosRow produtoRow)
                {
                    dsDocumentos.NovaLinhaArtigos(produtoRow);
                    txtObservacoes.Text += produtoRow.Observacao + ";";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (DgridArtigos.CurrentRow != null)
                {
                    var id = Guid.Parse(DgridArtigos.CurrentRow.Cells["iDDataGridViewTextBoxColumn"].Value.ToString());
                    dsDocumentos.EliminarLinha(id);
                    txtObservacoes.Clear();
                    foreach (DataGridViewRow linha in DgridArtigos.Rows)
                    {
                        txtObservacoes.Text += linha.Cells["Observacao"].Value.ToString() + ";";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao eliminar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsGravarDoc_Click(object sender, EventArgs e)
        {
            try
            {
                var docRow = dsDocumentos.CabecDocumento[0];

                if (ValidaPreenchimentoDocumento())
                {
                    docRow.TipoDocumento = TxtCodigoDoc.Text;
                    docRow.NrDocumento = int.Parse(NrDoc.Text);
                    docRow.Cliente = TxtCodigoCl.Text;
                    docRow.Total = decimal.Parse(txtTotal.Text);
                    docRow.Estado = txtEstado.Text;
                    docRow.Observacoes = txtObservacoes.Text;
                    docRow.DataEntrega = dateTimePicker1.Value.Date;
                    docRow.DataRececao = DataMod.Value.Date;
                    docRow.Desconto = int.Parse(txtDesconto.Text);
                    if ((txtCat.Text == "") || (txtMarca.Text == ""))
                    {// Obter código do produto
                        docRow.CodProduto = "PR000";
                    }
                    else
                    {
                        docRow.CodProduto = txtCodPr.Text;
                    }

                    dsDocumentos.UpdateDoc();
                    dsDocumentos.UpdateLinhas();

                    MessageBox.Show("Documento salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparFormulario();
                }
                else
                {
                    MessageBox.Show("Preencha todos os campos obrigatórios do documento.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gravar documento: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsNovoDoc_Click(object sender, EventArgs e)
        {
            try
            {
                LimparFormulario();
                HabilitarCampos();
                HabilitarBotoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar novo documento: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurarImpressao();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao imprimir documento: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsAddCliente_Click(object sender, EventArgs e)
        {
            try
            {
                FrmClientes frmClientes = new FrmClientes();
                frmClientes.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir formulário de clientes: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsConsultarCliente_Click(object sender, EventArgs e)
        {
            AbrirSelecaoClientes();
        }

        private void TsConsultarDocumentos_Click(object sender, EventArgs e)
        {
            AbrirSelecaoDocumentos();
        }

        private void TsAddProduto_Click(object sender, EventArgs e)
        {
            try
            {
                FrmProdutos frmProdutos = new FrmProdutos();
                frmProdutos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir formulário de produtos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TsConsultaProduto_Click(object sender, EventArgs e)
        {
            try
            {
                FrmDados frmDados = new FrmDados();
                frmDados.MostrarTabelaDados("DsArtigos");

                if (frmDados.RowSelecionada is ProdutosRow produtoRow)
                {

                    txtEquipNome.Text = produtoRow.NomeProduto;
                    txtCat.Text = querryProdutosTableAdapter.NomeCategoria(produtoRow.Categorias).ToString();
                    txtMarca.Text = querryProdutosTableAdapter.NomeMarca(produtoRow.Marcas);
                    txtImei.Text = produtoRow.IMEI;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao consultar produtos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgridArtigos_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (DgridArtigos.Rows[e.RowIndex].Cells["precoUntDataGridViewTextBoxColumn"].Value != null &&
                    DgridArtigos.Rows[e.RowIndex].Cells["quantidadeDataGridViewTextBoxColumn"].Value != null)
                {
                    decimal precoUnitario = Convert.ToDecimal(DgridArtigos.Rows[e.RowIndex].Cells["precoUntDataGridViewTextBoxColumn"].Value);
                    decimal quantidade = Convert.ToDecimal(DgridArtigos.Rows[e.RowIndex].Cells["quantidadeDataGridViewTextBoxColumn"].Value);
                    decimal totalLinha = precoUnitario * quantidade;
                    DgridArtigos.Rows[e.RowIndex].Cells["totalDataGridViewTextBoxColumn"].Value = Math.Round(totalLinha, 2);
                }

                CalcularTotalDocumento();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao atualizar linha: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAbrirCliente_Click_1(object sender, EventArgs e)
        {
            AbrirSelecaoClientes();
        }


        private void btnNovoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                LimparCliente();
                TxtCodigoCl.Text = dsClientes.DaProxNrCliente();
                BtnGravarCliente.Enabled = true;
                dsClientes.NovoCliente();
                HabilitarCliente();
                TxtNomeCl.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar novo cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGravarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                ClientesRow novoCliente = dsClientes.Clientes[0];
                if (ValidaPreenchimentoCliente())
                {
                    if (novoCliente.CodCl == dsClientes.DaProxNrCliente())
                    {

                        novoCliente.Nome = TxtNomeCl.Text;
                        novoCliente.CodCl = TxtCodigoCl.Text;
                        novoCliente.Telefone = TxtTelefone.Text;
                        novoCliente.Email = TxtEmail.Text;

                        dsClientes.UpdateClientes();

                        MessageBox.Show("Cliente Criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimparCliente();
                        BtnGravarCliente.Enabled = false;
                        TxtNomeCl.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("Preencha todos os campos corretamente.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gravar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cBoxEuro_CheckedChanged(object sender, EventArgs e)
        {
            AplicarDesconto();
        }

        private void btnAbrirPr_Click(object sender, EventArgs e)
        {
            try
            {
                FrmDados frmDados = new FrmDados();
                frmDados.MostrarTabelaDados("DsArtigos");

                if (frmDados.RowSelecionada is ProdutosRow produtoRow)
                {
                    txtCodPr.Text = produtoRow.CodPr;
                    txtEquipNome.Text = produtoRow.NomeProduto;
                    txtCat.Text = querryProdutosTableAdapter.NomeCategoria(produtoRow.Categorias).ToString();
                    txtMarca.Text = querryProdutosTableAdapter.NomeMarca(produtoRow.Marcas);
                    txtImei.Text = produtoRow.IMEI;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NrDoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreencheDocumento(TxtCodigoDoc.Text, int.Parse(NrDoc.Text));
        }

        private void AplicarDesconto()
        {
            try
            {
                if (decimal.TryParse(txtTotal.Text, out decimal total) && decimal.TryParse(txtDesconto.Text, out decimal desconto))
                {
                    if (cBoxEuro.Checked)
                    {
                        total -= desconto;
                        cBoxPercent.Checked = false;
                        cBoxPercent.Enabled = false;
                        txtTotal.Text = total.ToString("F2");
                    }

                    else if (cBoxPercent.Checked)
                    {
                        total -= (total * (desconto / 100));
                        cBoxEuro.Enabled = false;
                        cBoxEuro.Checked = false;
                        txtTotal.Text = total.ToString("F2");
                    }
                    else
                    {
                        cBoxEuro.Enabled = true;
                        cBoxPercent.Enabled = true;
                        CalcularTotalDocumento();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao aplicar desconto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirSelecaoClientes()
        {
            try
            {
                FrmDados frmDados = new FrmDados();
                frmDados.MostrarTabelaDados("DsClientes");

                if (frmDados.RowSelecionada is ClientesRow clienteRow)
                {
                    TxtCodigoCl.Text = clienteRow.CodCl;
                    TxtNomeCl.Text = clienteRow.Nome;
                    TxtTelefone.Text = clienteRow.Telefone;
                    TxtEmail.Text = clienteRow.Email;
                    HabilitarCliente();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao selecionar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirSelecaoDocumentos()
        {
            try
            {
                FrmDados frmDados = new FrmDados();
                frmDados.MostrarTabelaDados("DsDocumentos");

                if (frmDados.RowSelecionada is DsDocumentos.CabecDocumentoRow docRow)
                {
                    TxtCodigoDoc.Text = docRow.TipoDocumento;
                    TxtDescricaoDoc.Text = dsDocumentos.TipoDocumentos
                        .FirstOrDefault(t => t.CodDoc == docRow.TipoDocumento)?.Descricao;
                    NrDoc.Text = docRow.NrDocumento.ToString();
                    PreencheDocumento(docRow.TipoDocumento, docRow.NrDocumento);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao selecionar documento: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EstadoDoc()
        {
            if (TxtCodigoDoc.Text == "ORC")
            {
                txtEstado.Items.Clear();
                txtEstado.Items.AddRange(new string[] { "Entregue", "Aceite", "Cancelado", "Finalizado" });
            }
            else if (TxtCodigoDoc.Text == "FC")
            {
                txtEstado.Items.Clear();
                txtEstado.Items.AddRange(new string[] { "Emitida", "Concluida", "Anulada" });
            }
            else if (TxtCodigoDoc.Text == "NDE")
            {
                txtEstado.Items.Clear();
                txtEstado.Items.AddRange(new string[] { "Realizada", "Em espera", "Cancelada", "Em andamento" });
            }
            else if (TxtCodigoDoc.Text == "NDE")
            {
                txtEstado.Items.Clear();
                txtEstado.Items.AddRange(new string[] { "Numerario", "Artigo", "Cancelada" });
            }


        }
        private void CalcularTotalDocumento()
        {
            try
            {
                decimal soma = 0;

                foreach (DataGridViewRow linha in DgridArtigos.Rows)
                {
                    if (linha.Cells["totalDataGridViewTextBoxColumn"].Value != null)
                    {
                        soma += Convert.ToDecimal(linha.Cells["totalDataGridViewTextBoxColumn"].Value);
                    }
                }

                txtTotal.Text = soma.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao calcular total: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AdicionarDescricoesPr()
        {
            try
            {
                foreach (DataGridViewRow linha in DgridArtigos.Rows)
                {
                    // Preenche a coluna Marca
                    if (linha.Cells["Marca"].Value != DBNull.Value)
                    {
                        linha.Cells["NomeMarca"].Value = querryProdutosTableAdapter.NomeMarca(int.Parse(linha.Cells["Marca"].Value.ToString()));
                    }
                    // Preenche a coluna Categoria
                    if (linha.Cells["Categoria"].Value != DBNull.Value)
                    {
                        linha.Cells["NomeCategoria"].Value = querryProdutosTableAdapter.NomeCategoria(linha.Cells["Categoria"].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar descricao: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatarTabelas()
        {

            DgridArtigos.Columns["precoUntDataGridViewTextBoxColumn"].DefaultCellStyle.Format = "F2";
            DgridArtigos.Columns["quantidadeDataGridViewTextBoxColumn"].DefaultCellStyle.Format = "F2";
            DgridArtigos.Sort(DgridArtigos.Columns["numLInhaDataGridViewTextBoxColumn"], ListSortDirection.Ascending);

        }

        private void LimparFormulario()
        {
            NrDoc.Text = "0";
            TxtCodigoDoc.Text = "";
            TxtDescricaoDoc.Text = string.Empty;
            DataMod.Value = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;
            txtTotal.Text = "0.00";
            txtObservacoes.Text = string.Empty;
            txtEstado.SelectedIndex = -1;

            LimparCliente();
            LimparProduto();
            txtObservacoes.Text = "";
            dsClientes.Clientes.Clear();
            dsDocumentos.CabecDocumento.Clear();
            dsDocumentos.ListaProdutos.Clear();
        }

        private void LimparCliente()
        {
            TxtCodigoCl.Text = "";
            TxtNomeCl.Text = "Nome";
            TxtTelefone.Text = "Telefone";
            TxtEmail.Text = "Email";
        }

        private void LimparProduto()
        {
            txtEquipNome.Text = string.Empty;
            txtCat.Text = "";
            txtMarca.Text = "";
            txtImei.Text = string.Empty;
            txtObservacoes.Text = "";

        }

        private void HabilitarCampos()
        {
            DataMod.Enabled = true;
            TxtCodigoCl.Enabled = true;
            NrDoc.Enabled = true;
            txtEstado.Enabled = true;
            dateTimePicker1.Enabled = true;
            txtDesconto.Enabled = true;
            tsGravarDoc.Enabled = true;
        }

        private void DesabilitarBotoes()
        {
            BtnNovo.Enabled = false;
            BtnEliminar.Enabled = false;
            btnAbrirPr.Enabled = false;
            btnGravarPr.Enabled = false;
            TxtCodigoCl.Enabled = false;
            tsGravarDoc.Enabled = false;
            txtDesconto.Enabled = false;
            dateTimePicker1.Enabled = false;
        }

        private void HabilitarBotoes()
        {
            BtnNovo.Enabled = true;
            BtnEliminar.Enabled = true;
            tsGravarDoc.Enabled = true;
            btnAbrirPr.Enabled = true;
            tsImprimir.Enabled = true;
            btnAbrirCliente.Enabled = true;
            txtDesconto.Enabled = true;
            dateTimePicker1.Enabled = true;
        }

        private void HabilitarCliente()
        {
            TxtNomeCl.Enabled = true;
            TxtTelefone.Enabled = true;
            TxtEmail.Enabled = true;
        }

        private void HabilitarProduto()
        {
            txtEquipNome.Enabled = true;
            txtCat.Enabled = true;
            txtMarca.Enabled = true;
            txtImei.Enabled = true;
            txtObservacoes.Enabled = true;
            txtTipoPr.Enabled = true;
            btnGravarPr.Enabled = true;
        }

        private void ConfigurarImpressao()
        {
            printDocument = new PrintDocument();

            printDocument.BeginPrint += BeginPrint;
            printDocument.PrintPage += PrintPage;
            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument,
                Text = "Pré-visualização de Impressão",
                WindowState = FormWindowState.Maximized,
                StartPosition = FormStartPosition.CenterScreen,
                UseAntiAlias = true
            };
            previewDialog.ShowDialog();
        }

        private void BeginPrint(object sender, PrintEventArgs e)
        {
            // Exibir o PrintDialog apenas se for uma impressão real
            if (e.PrintAction == PrintAction.PrintToPrinter)
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = (PrintDocument)sender;
                printDialog.AllowSomePages = true;
                printDialog.AllowSelection = true;
                printDialog.AllowCurrentPage = true;
                printDialog.AllowPrintToFile = true;
                printDialog.UseEXDialog = true;
                printDialog.ShowHelp = true;

                if (printDialog.ShowDialog() != DialogResult.OK)
                {
                    e.Cancel = true; // Cancela a impressão se o usuário não confirmar
                }
                else
                {
                    // Definindo a orientação da página para paisagem

                    // Captura as configurações selecionadas pelo usuário
                    ((PrintDocument)sender).PrinterSettings = printDialog.PrinterSettings;
                    ((PrintDocument)sender).DefaultPageSettings = printDialog.PrinterSettings.DefaultPageSettings;



                    // Verifica se o usuário selecionou "Imprimir em Arquivo"
                    if (printDialog.PrinterSettings.PrintToFile)
                    {
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "Arquivos PDF (*.pdf)|*.pdf";
                        saveDialog.DefaultExt = "pdf";
                        saveDialog.AddExtension = true;

                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Define o nome do arquivo para onde a impressão será direcionada
                            ((PrintDocument)sender).PrinterSettings.PrintFileName = saveDialog.FileName;
                        }
                        else
                        {
                            e.Cancel = true; // Cancela a impressão se o usuário não fornecer um nome de arquivo
                        }
                    }
                }
            }
        }


        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                // Definir margens e dimensões
                float marginLeft = e.MarginBounds.Left;
                float marginTop = e.MarginBounds.Top; // Ajuste conforme necessário
                float pageWidth = e.MarginBounds.Width;
                float pageHeight = e.MarginBounds.Height;

                // Definir as fontes
                Font fonteTitulo = new Font("Arial", 22, FontStyle.Bold);
                Font fonteSubtitulo = new Font("Arial", 14, FontStyle.Bold);
                Font fonteTexto = new Font("Arial", 12, FontStyle.Regular);

                Brush brush = Brushes.Black;

                // Posicionamento vertical inicial
                float yPos = marginTop;

                // Desenhar o cabeçalho (logotipo inicial, nome da loja e informações do documento)
                yPos = DrawHeader(e.Graphics, marginLeft, yPos, pageWidth, fonteTitulo, fonteTexto, brush);


                // Desenhar as informações do cliente
                yPos = DrawClientInfo(e.Graphics, marginLeft, yPos, pageWidth, fonteSubtitulo, fonteTexto, brush);

                // Desenhar a tabela de itens (DataGridView) com 7 colunas
                yPos = DrawItemsTable(e.Graphics, marginLeft, yPos, pageWidth, brush);


                // Desenhar as informações do produto após a tabela, com o título "Objeto"
                yPos = DrawObservacoesDescontoTotal(e.Graphics, marginLeft, yPos, TxtCodigoDoc.Text, pageWidth, fonteSubtitulo, fonteTexto, brush);

                yPos = DrawSeparator(e.Graphics, marginLeft, yPos, pageWidth);

                yPos = DrawFinalLogo(e.Graphics, marginLeft, yPos, marginTop, pageWidth, pageHeight, brush);

                // Desenhar a assinatura e a data
                DrawSignatureAndDate(e.Graphics, marginLeft, yPos, pageWidth, fonteTexto, brush);

                // Desenhar o logotipo final

                // Indica que não há mais páginas a serem impressas
                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no PrintPage: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.HasMorePages = false;
            }
        }

        private float DrawHeader(Graphics graphics, float marginLeft, float yPos, float pageWidth, Font fonteTitulo, Font fonteTexto, Brush brush)
        {
            string logoPath = "C:\\Users\\synys\\source\\repos\\TeleBerço\\Resources\\transferir.jpeg";
            string storeName = TxtDescricaoDoc.Text; // Nome do documento
            string codDocumento = TxtCodigoDoc.Text; // Código do documento
            int nrDocumento = NrDoc.TabIndex + 1; // Número do documento
            string documento = codDocumento + $"{nrDocumento:000}";
            string dataValor = DataMod.Value.ToShortDateString();

            float logoHeight = 110;
            float logoWidth = 0;

            // **1. Logo Centralizado e Ajustado à Largura da Página**
            if (File.Exists(logoPath))
            {
                using (Image logo = Image.FromFile(logoPath))
                {
                    // Calcular a largura do logo mantendo a proporção
                    float aspectRatio = logo.Width / (float)logo.Height;
                    logoWidth = aspectRatio * logoHeight + 30;

                    // Garantir que o logo não ultrapasse a largura da página
                    if (logoWidth > pageWidth)
                    {
                        logoWidth = pageWidth;
                        logoHeight = logoWidth / aspectRatio;
                    }

                    // Centralizar o logo horizontalmente
                    float logoX = marginLeft + (pageWidth - logoWidth) / 2;
                    graphics.DrawImage(logo, logoX, yPos, logoWidth, logoHeight);
                }
            }

            yPos += logoHeight + 10; // Espaço após o logo

            // **2. Linha Separadora**
            graphics.DrawLine(Pens.Black, marginLeft, yPos, marginLeft + pageWidth, yPos);
            yPos += 40; // Espaçamento de 40 unidades após a linha

            // **3. Nome do Documento Centralizado**
            SizeF storeNameSize = graphics.MeasureString(storeName, fonteTitulo);
            float storeNameX = marginLeft + (pageWidth - storeNameSize.Width) / 2;
            graphics.DrawString(storeName, fonteTitulo, brush, storeNameX, yPos);
            yPos += storeNameSize.Height + 10; // Espaço após o nome do documento

            // **4. Número da Fatura e Data na Mesma Linha dentro de um Retângulo**
            Font fonteNegrito = new Font(fonteTexto.FontFamily, fonteTexto.Size, FontStyle.Bold);
            Font fonteNormal = fonteTexto;

            string numeroLabel = "Número: ";
            string numeroValor = documento;
            string dataLabel = "Data: ";
            // dataValor já definido anteriormente

            // Medir alturas para alinhamento vertical
            float lineHeight = Math.Max(
                graphics.MeasureString(numeroLabel + numeroValor, fonteTexto).Height,
                graphics.MeasureString(dataLabel + dataValor, fonteTexto).Height);

            // Medir larguras dos textos

            SizeF dataSize = graphics.MeasureString(dataLabel + dataValor, fonteTexto);

            // **Desenhar Retângulo que Envolve o Texto do "Número" e da "Data"**
            float rectX = marginLeft;
            float rectY = yPos - 5; // Ajuste vertical para incluir o texto e um pequeno espaçamento
            float rectWidth = pageWidth;
            float rectHeight = lineHeight + 3; // Altura do texto mais espaçamento superior e inferior

            // Desenhar retângulo ao redor do texto do "Número" e "Data"
            graphics.DrawRectangle(Pens.Black, rectX, rectY, rectWidth, rectHeight);

            // **Número da Fatura Alinhado à Esquerda dentro do Retângulo**
            float numeroX = marginLeft + 5; // Espaçamento interno à esquerda
            float textoY = yPos; // Posição vertical do texto

            graphics.DrawString(numeroLabel, fonteNegrito, brush, numeroX, textoY);
            float numeroLabelWidth = graphics.MeasureString(numeroLabel, fonteNegrito).Width;
            graphics.DrawString(numeroValor, fonteNormal, brush, numeroX + numeroLabelWidth, textoY);

            // **Data Alinhada à Direita dentro do Retângulo**
            float dataX = marginLeft + pageWidth - dataSize.Width - 10; // Espaçamento interno à direita
            graphics.DrawString(dataLabel, fonteNegrito, brush, dataX, textoY);
            float dataLabelWidth = graphics.MeasureString(dataLabel, fonteNegrito).Width;
            graphics.DrawString(dataValor, fonteNormal, brush, dataX + dataLabelWidth, textoY);

            yPos += rectHeight + 20; // Avançar o yPos após esta seção, incluindo espaçamento adicional

            return yPos;
        }

        private float DrawClientInfo(Graphics graphics, float marginLeft, float yPos, float pageWidth, Font fonteSubtitulo, Font fonteTexto, Brush brush)
        {
            // Título da seção
            graphics.DrawString("Informações do Cliente ", fonteSubtitulo, brush, marginLeft, yPos);
            yPos += fonteSubtitulo.GetHeight(graphics) + 5;

            // Definir fontes
            Font fonteNegrito = new Font(fonteTexto.FontFamily, fonteTexto.Size, FontStyle.Bold);
            Font fonteNormal = fonteTexto;

            // Preparar informações do cliente
            string[] clienteLabels = { "Nome:", "Telefone:", "Email:" };
            string[] clienteValores = { TxtNomeCl.Text, TxtTelefone.Text, TxtEmail.Text };

            // Preparar informações do produto
            string[] produtoLabels = { "Categoria:", "Marca:", "Produto:", "IMEI: " };
            string[] produtoValores = { txtCat.Text, txtMarca.Text, txtEquipNome.Text, txtImei.Text };

            // Calcular a altura necessária para as informações do cliente e do produto
            float lineHeight = fonteTexto.GetHeight(graphics);
            int numLinhasCliente = clienteLabels.Length;
            int numLinhasProduto = produtoLabels.Length;
            int numLinhas = Math.Max(numLinhasCliente, numLinhasProduto);

            // Altura total do conteúdo dentro do retângulo
            float rectHeight = (lineHeight + 5) * numLinhas + 15; // 5 unidades de espaçamento entre linhas, 20 de espaçamento interno

            // Definir dimensões do retângulo
            float rectX = marginLeft;
            float rectY = yPos;
            float rectWidth = pageWidth;

            // Desenhar retângulo
            graphics.DrawRectangle(Pens.Black, rectX, rectY, rectWidth, rectHeight);

            // Espaçamento interno
            float paddingX = 10;
            float paddingY = 10;

            // Posição inicial para desenhar o texto dentro do retângulo
            float textY = rectY + paddingY + 15;

            // **Desenhar informações do cliente alinhadas à margem esquerda**
            float clienteX = rectX + paddingX;

            for (int i = 0; i < numLinhasCliente; i++)
            {
                // Desenhar label
                graphics.DrawString(clienteLabels[i], fonteNegrito, brush, clienteX, textY);
                float labelWidth = graphics.MeasureString(clienteLabels[i], fonteNegrito).Width;
                // Desenhar valor
                graphics.DrawString(clienteValores[i], fonteNormal, brush, clienteX + labelWidth + 5, textY);

                textY += lineHeight + 5; // Atualizar Y para a próxima linha
            }

            // **Desenhar informações do produto alinhadas à margem direita**
            // Resetar textY para a posição inicial
            textY = rectY + paddingY + 3;

            // Medir a largura máxima das labels e valores do produto
            float maxProdutoLabelWidth = 0;
            float maxProdutoValorWidth = 0;
            for (int i = 0; i < numLinhasProduto; i++)
            {
                float labelWidth = graphics.MeasureString(produtoLabels[i], fonteNegrito).Width;
                float valorWidth = graphics.MeasureString(produtoValores[i], fonteNormal).Width;
                if (labelWidth > maxProdutoLabelWidth) maxProdutoLabelWidth = labelWidth;
                if (valorWidth > maxProdutoValorWidth) maxProdutoValorWidth = valorWidth;
            }

            // Calcular a posição X inicial para as informações do produto
            float produtoX = rectX + rectWidth - paddingX - (maxProdutoLabelWidth + 5 + maxProdutoValorWidth);

            for (int i = 0; i < numLinhasProduto; i++)
            {
                // Desenhar label
                graphics.DrawString(produtoLabels[i], fonteNegrito, brush, produtoX, textY);
                float labelWidth = graphics.MeasureString(produtoLabels[i], fonteNegrito).Width;
                // Desenhar valor
                graphics.DrawString(produtoValores[i], fonteNormal, brush, produtoX + labelWidth + 5, textY);

                textY += lineHeight + 5; // Atualizar Y para a próxima linha
            }

            // Atualizar yPos para a próxima seção
            yPos += rectHeight + 30; // Espaçamento após o retângulo

            return yPos;
        }
        private float DrawObservacoesDescontoTotal(Graphics graphics, float marginLeft, float yPos, string codDocumento, float pageWidth, Font fonteSubtitulo, Font fonteTexto, Brush brush)
        {
            Font fonteNegrito = new Font(fonteTexto.FontFamily, 12, FontStyle.Bold);

            Font fonteNormal = new Font(fonteTexto.FontFamily, 10, FontStyle.Bold);
            // Espaçamento interno e entre elementos
            float padding = 10;
            float lineHeight = fonteTexto.GetHeight(graphics) + 5;

            // **1. Preparar textos e valores**
            string observacoesLabel = "Observações";
            string observacoesTexto = txtObservacoes.Text;
            string descontoValor = txtDesconto.Text;
            string descontoLabel = "Desconto:";
            if (cBoxPercent.Checked)
            {
                descontoValor = txtDesconto.Text + " %";
            }
            else if (cBoxEuro.Checked)
            {
                descontoValor = txtDesconto.Text + " €";
            }
            string totalLabel = "Total:";
            string totalValor = txtTotal.Text + "€";

            string previsaoLabel = "Previsão:";
            string previsaoTexto = dateTimePicker1.Value.Date.ToString("dd/MM/yyyy");

            // **2. Definir áreas para Observações e Desconto/Total**
            // Dividiremos a largura disponível em duas partes
            float availableWidth = pageWidth;
            float observacoesWidth = availableWidth * 0.7f; // 60% para Observações
            float valoresWidth = availableWidth * 0.3f;     // 40% para Desconto e Total

            // **3. Desenhar a seção de Observações**
            // **3.1. Desenhar o rótulo "Observações"**
            graphics.DrawString(observacoesLabel, fonteSubtitulo, brush, marginLeft, yPos);
            yPos += lineHeight;

            // **3.2. Desenhar o retângulo para o texto das Observações**
            float observacoesRectX = marginLeft;
            float observacoesRectY = yPos;
            float observacoesRectHeight = (lineHeight * 3) + (padding * 2); // 3 linhas de texto + espaçamento interno
            graphics.DrawRectangle(Pens.Black, observacoesRectX, observacoesRectY, observacoesWidth, observacoesRectHeight);

            // **3.3. Desenhar o texto das Observações dentro do retângulo**
            RectangleF observacoesTextRect = new RectangleF(
                observacoesRectX + padding,
                observacoesRectY + padding,
                observacoesWidth - (padding * 2),
                observacoesRectHeight - (padding * 2)
            );
            // Limitar o texto a 3 linhas
            StringFormat stringFormat = new StringFormat();
            stringFormat.Trimming = StringTrimming.EllipsisWord;
            stringFormat.FormatFlags = StringFormatFlags.LineLimit;

            graphics.DrawString(observacoesTexto, fonteTexto, brush, observacoesTextRect, stringFormat);

            // **4. Desenhar a seção de Desconto e Total**
            float valoresX = marginLeft + observacoesWidth;
            float valoresY = yPos;

            if (codDocumento == "ORC" || codDocumento == "NDC")
            {
                graphics.DrawString(previsaoLabel, fonteNormal, brush, valoresX + padding, valoresY + padding);
                float labelDireitaWidth = graphics.MeasureString(previsaoLabel, fonteNegrito).Width;
                graphics.DrawString(previsaoTexto, fonteNormal, brush, valoresX + padding + labelDireitaWidth, valoresY + padding);
            }
            // **4.1. Desenhar "Desconto" e valor**
            graphics.DrawString(descontoLabel, fonteNormal, brush, valoresX + padding, valoresY + padding + 20);
            float descontoLabelWidth = graphics.MeasureString(descontoLabel, fonteSubtitulo).Width;
            graphics.DrawString(descontoValor, fonteNormal, brush, valoresX + padding + descontoLabelWidth - 15, valoresY + padding + 20);

            // **4.2. Desenhar "Total" e valor abaixo de "Desconto"**
            float totalY = valoresY + lineHeight + padding;
            graphics.DrawString(totalLabel, fonteNegrito, brush, valoresX + padding + 50, totalY + padding + 20);
            float totalLabelWidth = graphics.MeasureString(totalLabel, fonteSubtitulo).Width;
            graphics.DrawString(totalValor, fonteTexto, brush, valoresX + padding + totalLabelWidth + 50, totalY + padding + 20);

            // **4.3. Desenhar retângulo ao redor de Desconto e Total**
            float valoresHeight = observacoesRectHeight; // Mesmo altura da seção de Observações
            graphics.DrawRectangle(Pens.Black, valoresX, valoresY, valoresWidth, valoresHeight);

            // **5. Atualizar yPos para a próxima seção**
            yPos += observacoesRectHeight + 40; // Espaçamento após a seção

            return yPos;
        }

        private float DrawSeparator(Graphics graphics, float marginLeft, float yPos, float pageWidth)
        {
            graphics.DrawLine(Pens.Black, marginLeft, yPos, marginLeft + pageWidth, yPos);
            yPos += 20; // Aumentar o espaçamento após a linha
            return yPos;
        }

        private void DrawSignatureAndDate(Graphics graphics, float marginLeft, float yPos, float pageWidth, Font fonteTexto, Brush brush)
        {
            string assinaturaText = "Assinatura";
            // Desenhar a assinatura
            SizeF assinaturaTextSize = graphics.MeasureString(assinaturaText, fonteTexto);
            float signatureLineWidth = pageWidth / 2;
            float totalSignatureWidth = assinaturaTextSize.Width + signatureLineWidth;
            float signatureBlockX = marginLeft + pageWidth / 2 - (totalSignatureWidth / 2);

            // Espaçamento após 'Assinatura'

            graphics.DrawString(assinaturaText, fonteTexto, brush, signatureBlockX, yPos);
            float lineY = yPos + assinaturaTextSize.Height / 2;
            graphics.DrawLine(Pens.Black, signatureBlockX + assinaturaTextSize.Width, lineY, signatureBlockX + assinaturaTextSize.Width + signatureLineWidth, lineY);
        }

        private float DrawItemsTable(Graphics graphics, float marginLeft, float yPos, float pageWidth, Brush brush)
        {
            // Título da seção
            Font fonteSubtitulo = new Font("Arial", 14, FontStyle.Bold);
            Font fonteTexto = new Font("Arial", 11, FontStyle.Bold);
            Font fonteTabela = new Font("Arial", 9, FontStyle.Regular);
            graphics.DrawString("Descritivo", fonteSubtitulo, brush, marginLeft, yPos);
            yPos += fonteSubtitulo.GetHeight(graphics) + 5; // Espaço após o título

            // Definir colunas
            int numColumns = 7;

            // Definir cabeçalhos das colunas
            var columnsToPrint = new[] { "NomeCategoria", "NomeMarca", "NomeProduto", "iMEIDataGridViewTextBoxColumn", "precoUntDataGridViewTextBoxColumn", "quantidadeDataGridViewTextBoxColumn", "totalDataGridViewTextBoxColumn" };
            var columnHeaders = new[] { "Categoria", "Marca", "Produto", "Código", "Preço Un", "Quantidade", "Total" };

            // Calcular largura das colunas
            float totalAvailableWidth = pageWidth; // Espaço disponível para a tabela
            float columnWidth = totalAvailableWidth / numColumns;
            float[] columnWidths = Enumerable.Repeat(columnWidth, numColumns).ToArray();

            float xPos = marginLeft;
            float lineHeight = fonteTexto.GetHeight(graphics) + 10;

            Pen pen = Pens.Black;

            // Desenhar cabeçalho da tabela
            yPos += 5; // Espaçamento antes da tabela
            for (int i = 0; i < numColumns; i++)
            {
                // Desenhar retângulo da célula do cabeçalho
                graphics.DrawRectangle(pen, xPos, yPos, columnWidths[i], lineHeight);

                // Centralizar texto no cabeçalho
                string headerText = columnHeaders[i];
                SizeF headerSize = graphics.MeasureString(headerText, fonteTexto);
                float textX = xPos + (columnWidths[i] - headerSize.Width) / 2;
                float textY = yPos + (lineHeight - headerSize.Height) / 2;

                graphics.DrawString(headerText, fonteTexto, brush, textX, textY);
                xPos += columnWidths[i];
            }
            yPos += lineHeight;
            xPos = marginLeft;

            // Desenhar linhas da tabela com os dados
            foreach (DataGridViewRow row in DgridArtigos.Rows)
            {
                if (!row.IsNewRow)
                {
                    for (int i = 0; i < numColumns; i++)
                    {
                        // Desenhar retângulo da célula
                        graphics.DrawRectangle(pen, xPos, yPos, columnWidths[i], lineHeight - 2);

                        // Obter o valor da célula
                        string cellValue = row.Cells[columnsToPrint[i]].Value?.ToString() ?? "";

                        // Centralizar texto na célula
                        SizeF cellSize = graphics.MeasureString(cellValue, fonteTabela);
                        float textX = xPos + (columnWidths[i] - cellSize.Width) / 2;
                        float textY = yPos + (lineHeight - cellSize.Height) / 2;

                        graphics.DrawString(cellValue, fonteTabela, brush, textX, textY);
                        xPos += columnWidths[i];
                    }
                    yPos += lineHeight;
                    xPos = marginLeft;
                }
            }

            yPos += 20; // Espaçamento após a tabela
            return yPos;
        }

        private float DrawFinalLogo(Graphics graphics, float marginLeft, float yPos, float marginTop, float pageWidth, float pageHeight, Brush brush)
        {
            string bottomImagePath = "C:\\Users\\synys\\source\\repos\\TeleBerço\\Resources\\Morada2.jpeg";

            if (File.Exists(bottomImagePath))
            {
                using (Image bottomImage = Image.FromFile(bottomImagePath))
                {
                    float desiredBottomImgWidth = 600;
                    float bottomImgHeight = bottomImage.Height * (desiredBottomImgWidth / bottomImage.Width) - 75;

                    // Posicionar a imagem no fundo da página, centralizada horizontalmente
                    float bottomImageX = marginLeft + pageWidth - desiredBottomImgWidth;
                    float bottomImageYPosition = marginTop + pageHeight - bottomImgHeight; // Posicionar logo após o último conteúdo

                    graphics.DrawImage(bottomImage, bottomImageX, bottomImageYPosition, desiredBottomImgWidth, bottomImgHeight);
                    yPos = marginTop + pageHeight - bottomImgHeight - 70;

                }
            }
            return yPos;
        }

        private void txtDesconto_TextChanged_1(object sender, EventArgs e)
        {
            if ((txtDesconto.Text == "0") || (txtDesconto.Text == ""))
            {
                CalcularTotalDocumento();
            }
            else
            {
                AplicarDesconto();
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            try
            {
                FrmDados frmDados = new FrmDados();
                frmDados.MostrarTabelaDados("DsCategorias");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar Categorias: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void tsMarcas_Click(object sender, EventArgs e)
        {
            try
            {
                FrmDados frmDados = new FrmDados();
                frmDados.MostrarTabelaDados("DsMarcas");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar Marcas: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsAddMarcas_Click(object sender, EventArgs e)
        {
            try
            {
                FrmCat_Marca frmCat_Marca = new FrmCat_Marca();
                frmCat_Marca.tipoDadosAtual = FrmCat_Marca.TipoDados.Marcas;
                frmCat_Marca.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir formulario: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsAddCategorias_Click(object sender, EventArgs e)
        {
            try
            {
                FrmCat_Marca frmCat_Marca = new FrmCat_Marca();
                frmCat_Marca.tipoDadosAtual = FrmCat_Marca.TipoDados.Categorias;
                frmCat_Marca.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao abrir formulario: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cBoxPercent_CheckedChanged(object sender, EventArgs e)
        {
            AplicarDesconto();
        }
    }
}

