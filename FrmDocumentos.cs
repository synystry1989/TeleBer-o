using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TeleBerço.DsProdutosTableAdapters;

namespace TeleBerço
{
    public partial class FrmDocumentos : Form
    {
        // Classe interna para itens de conteúdo na impressão
        private class ContentItem
        {
            public Bitmap Image { get; set; }
            public float OriginalWidth { get; set; }
            public float OriginalHeight { get; set; }
        }

        // Datasets e TableAdapters
        private DsClientes dsClientes = new DsClientes();

        private QuerryProdutosTableAdapter querryProdutosTableAdapter = new QuerryProdutosTableAdapter();

        // Variáveis de controle
        private PrintDocument printDocument = new PrintDocument();

        public FrmDocumentos()
        {
            InitializeComponent();

        }

        #region Configuração e Eventos



        #endregion

        #region Carregamento e Preenchimento de Dados

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
            dsProdutos.CarregaCategorias();
            dsProdutos.CarregarMarcas();
            dsDocumentos.CarregaTipoDoc();

            // Configurar ComboBoxes

            TxtCodigoDoc.Text = "";
            TxtCodigoDoc.Focus();
            txtEstado.Items.AddRange(new string[] { "Pronto", "Em Preparação", "Cancelado", "Em Espera" });
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
                        if (produtoRow.CodPr  != "")
                        {
                            txtEquipNome.Text = produtoRow.NomeProduto;
                            txtCat.Text = querryProdutosTableAdapter.NomeCategoria(produtoRow.Categorias).ToString();
                            txtMarca.Text = querryProdutosTableAdapter.NomeMarca(produtoRow.Marcas);
                            txtImei.Text = produtoRow.IMEI;
                            txtObservacoes.Text = produtoRow.Observacao;
                        }
                    }

                    // Preencher dados do cliente associado
                    var clienteRow = dsClientes.PesquisaCliente(rowPesquisada.Cliente);
                    if (clienteRow != null)
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

                    // Carregar linhas do documento
                    dsDocumentos.CarregaLinhas(rowPesquisada.ID);
                    AdicionarDescricoesPr();
                    DesabilitarBotoes();

                }
                else
                {
                    NrDoc.Text = dsDocumentos.DaNrDocSeguinte(TxtCodigoDoc.Text).ToString();
                    HabilitarBotoes();
                    LimparCliente();
                    txtTotal.Text = "0";
                    txtEstado.Text="";
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

        #endregion

        #region Validações

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

        #endregion

        #region Eventos dos Controles

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
            var clienteRow = dsClientes.PesquisaCliente(TxtCodigoCl.Text);

            if (clienteRow != null)
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

        private void txtEstado_SelectedValueChanged(object sender, EventArgs e)
        {
            tsGravarDoc.Enabled = true;
        }

        #endregion

        #region Eventos dos Botões



        private void btnNovoPr_Click(object sender, EventArgs e)
        {
            try
            {
                dsProdutos.NovoArtigo();
                HabilitarProduto();
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
                if (!string.IsNullOrEmpty(txtEquipNome.Text))
                {
                    var produtoRow = dsProdutos.Produtos.NewProdutosRow();
                    produtoRow.NomeProduto = txtEquipNome.Text;
                    produtoRow.Categorias = txtCat.SelectedValue.ToString();
                    produtoRow.Marcas = (int)txtMarca.SelectedValue;
                    produtoRow.IMEI = txtImei.Text;
                    produtoRow.Observacao = txtObservacoes.Text;
                    produtoRow.Tipo = txtTipoPr.Text;

                    dsProdutos.Produtos.AddProdutosRow(produtoRow);
                    dsProdutos.UpdateArtigos();

                    MessageBox.Show("Produto salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Preencha os campos obrigatórios do produto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                if (frmDados.RowSelecionada is DsProdutos.ProdutosRow produtoRow)
                {
                    dsDocumentos.NovaLinhaArtigos(produtoRow);
                    txtObservacoes.Text += produtoRow.Observacao;
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
                    var id = Guid.Parse(DgridArtigos.CurrentRow.Cells["ID"].Value.ToString());
                    dsDocumentos.EliminarLinha(id);
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
                if (ValidaPreenchimentoDocumento())
                {
                    var docRow = dsDocumentos.CabecDocumento.NewCabecDocumentoRow();
                    docRow.TipoDocumento = TxtCodigoDoc.Text;
                    docRow.NrDocumento = int.Parse(NrDoc.Text);
                    docRow.Cliente = TxtCodigoCl.Text;
                    docRow.Total = decimal.Parse(txtTotal.Text);
                    docRow.Estado = txtEstado.Text;
                    docRow.Observacoes = txtObservacoes.Text;
                    docRow.DataEntrega = dateTimePicker1.Value.Date;
                    docRow.DataRececao = DataMod.Value.Date;

                    // Obter código do produto
                    var codProduto = querryProdutosTableAdapter.CodProduto(txtEquipNome.Text, (int)txtMarca.SelectedValue, txtCat.SelectedValue.ToString());
                    if (codProduto != null)
                    {
                        docRow.CodProduto = codProduto.ToString();
                    }

                    dsDocumentos.CabecDocumento.AddCabecDocumentoRow(docRow);
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
                dsDocumentos.NovoDocumento();
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

                if (frmDados.RowSelecionada is DsProdutos.ProdutosRow produtoRow)
                {
                    dsProdutos.NovoArtigo();
                    txtEquipNome.Text = produtoRow.NomeProduto;
                    txtCat.SelectedValue = produtoRow.Categorias;
                    txtMarca.SelectedValue = produtoRow.Marcas;
                    txtImei.Text = produtoRow.IMEI;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao consultar produtos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Métodos Auxiliares

        private void AbrirSelecaoClientes()
        {
            try
            {
                FrmDados frmDados = new FrmDados();
                frmDados.MostrarTabelaDados("DsClientes");

                if (frmDados.RowSelecionada is DsClientes.ClientesRow clienteRow)
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

        private void TxtDesconto_TextChanged(object sender, EventArgs e)
        {
            AplicarDesconto();
        }

        private void CBoxDesconto_CheckedChanged(object sender, EventArgs e)
        {
            AplicarDesconto();
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
                    }
                    else if (cBoxPercent.Checked)
                    {
                        total -= (total * desconto / 100);
                    }
                    txtTotal.Text = total.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao aplicar desconto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void FormatarTabelas()
        {

            DgridArtigos.Columns["precoUntDataGridViewTextBoxColumn"].DefaultCellStyle.Format = "F2";
            DgridArtigos.Columns["quantidadeDataGridViewTextBoxColumn"].DefaultCellStyle.Format = "F2";
            DgridArtigos.Sort(DgridArtigos.Columns["numLInhaDataGridViewTextBoxColumn"], ListSortDirection.Ascending);
        }

        private void LimparFormulario()
        {
            NrDoc.Text = "0";
            TxtCodigoDoc.SelectedIndex = -1;
            TxtDescricaoDoc.Text = string.Empty;
            DataMod.Value = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;
            txtTotal.Text = "0.00";
            txtObservacoes.Text = string.Empty;
            txtEstado.SelectedIndex = -1;

            LimparCliente();
            LimparProduto();

            dsClientes.Clientes.Clear();
            dsDocumentos.CabecDocumento.Clear();
            dsDocumentos.ListaProdutos.Clear();
        }

        private void LimparCliente()
        {
            TxtCodigoCl.Text= "";
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
            txtObservacoes.Text = string.Empty;
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
        }

        private void HabilitarBotoes()
        {
            BtnNovo.Enabled = true;
            BtnEliminar.Enabled = true;
            tsGravarDoc.Enabled = true;
            btnAbrirPr.Enabled = true;
            tsImprimir.Enabled = true;
            btnAbrirCliente.Enabled = true;
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

        #endregion

        #region Impressão

        private void ConfigurarImpressao()
        {
            printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.Landscape = true;
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
                // Definir margens
                float marginLeft = e.MarginBounds.Left;
                float marginTop = e.MarginBounds.Top - 70;
                float pageWidth = e.MarginBounds.Width;
                float pageHeight = e.MarginBounds.Height;

                // Definir as fontes
                Font fonteLoja = new Font("Arial", 22, FontStyle.Bold);
                Font fonteDescricao = new Font("Arial", 14, FontStyle.Bold);
                Font fonteTexto = new Font("Arial", 12, FontStyle.Regular);
                Brush brush = Brushes.Black;

                // Desenhar o cabeçalho (logotipo e nome da loja)
                DrawHeader(e, ref marginTop, pageWidth, fonteLoja, brush);

                // Desenhar a linha separadora
                DrawSeparator(e, ref marginTop, pageWidth);

                // Desenhar a descrição
                //marginTop = DrawDescription(e, marginTop, pageWidth, fonteDescricao, brush);

                // Capturar os painéis e o DataGridView
                List<ContentItem> contentItems = CaptureContentItems();

                // Desenhar os itens de conteúdo ajustados
                marginTop = DrawContentItems(e, contentItems, marginTop, pageWidth);

                // Desenhar a assinatura e a data
                marginTop = DrawSignatureAndDate(e, marginTop, pageWidth, fonteTexto, brush);

                // Desenhar a imagem de fundo
                DrawBackgroundImage(e, marginTop, pageWidth);

                // Indica que não há mais páginas a serem impressas
                e.HasMorePages = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no PrintPage: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.HasMorePages = false;
            }
        }

        private void DrawHeader(PrintPageEventArgs e, ref float yPos, float pageWidth, Font fonteLoja, Brush brush)
        {
            string logoPath = "C:\\Users\\synys\\source\\repos\\TeleBerço\\Resources\\transferir.jpeg";
            string storeName = TxtDescricaoDoc.Text;

            if (File.Exists(logoPath))
            {
                using (Image logo = Image.FromFile(logoPath))
                {
                    float logoHeight = 110;
                    float logoWidth = (logo.Width / (float)logo.Height) * logoHeight + 60;

                    SizeF storeNameSize = e.Graphics.MeasureString(storeName, fonteLoja);


                    float combinedX = e.MarginBounds.Left - 60;

                    e.Graphics.DrawImage(logo, combinedX, yPos, logoWidth, logoHeight);
                    float storeNameX = e.MarginBounds.Left + (pageWidth - storeNameSize.Width) / 2;
                    float storeNameY = yPos + (logoHeight - storeNameSize.Height) / 2;
                    e.Graphics.DrawString(storeName, fonteLoja, brush, storeNameX, storeNameY);
                }
            }
            else
            {
                SizeF storeNameSize = e.Graphics.MeasureString(storeName, fonteLoja);
                float storeNameX = e.MarginBounds.Left + (pageWidth - storeNameSize.Width) / 2;
                e.Graphics.DrawString(storeName, fonteLoja, brush, storeNameX, yPos);
            }

            yPos += 110; // Avançar o yPos após o logotipo e o nome da loja
        }

        private void DrawSeparator(PrintPageEventArgs e, ref float yPos, float pageWidth)
        {
            e.Graphics.DrawLine(Pens.Black, e.MarginBounds.Left, yPos, e.MarginBounds.Left + pageWidth + 55, yPos);
            yPos += 20; // Aumentar o espaçamento após a linha
        }



        private List<ContentItem> CaptureContentItems()
        {
            List<ContentItem> contentItems = new List<ContentItem>();

            // Capturar o painel 'Clientes' (Panel2)
            if (Panel2 != null)
            {
                Bitmap panel2Bitmap = CapturePanel(Panel2);
                contentItems.Add(new ContentItem { Image = panel2Bitmap, OriginalWidth = panel2Bitmap.Width - 350, OriginalHeight = panel2Bitmap.Height });
            }

            // Capturar o DataGridView
            if (DgridArtigos != null && DgridArtigos.Rows.Count > 0)
            {
                Bitmap dgvBitmap = new Bitmap(DgridArtigos.Width, DgridArtigos.Height - 180);
                DgridArtigos.DrawToBitmap(dgvBitmap, new Rectangle(0, 0, DgridArtigos.Width, DgridArtigos.Height));
                contentItems.Add(new ContentItem { Image = dgvBitmap, OriginalWidth = dgvBitmap.Width, OriginalHeight = dgvBitmap.Height });
            }

            // Capturar o painel 'Objeto' (Panel3)
            if (panel3 != null)
            {
                Bitmap panel3Bitmap = CapturePanel(panel3);
                contentItems.Add(new ContentItem { Image = panel3Bitmap, OriginalWidth = panel3Bitmap.Width, OriginalHeight = panel3Bitmap.Height });
            }

            return contentItems; // Retornar a lista de itens de conteúdo
        }

        private float DrawContentItems(PrintPageEventArgs e, List<ContentItem> contentItems, float yPos, float pageWidth)
        {

            // Calcular a altura disponível para os itens de conteúdo
            float availableHeight = e.MarginBounds.Height - yPos + 70; // 80 é um espaço reservado para assinatura e data (2-3 linhas)

            // Calcular a altura total dos itens de conteúdo
            float totalOriginalContentHeight = contentItems.Sum(item => item.OriginalHeight);
            float scaleFactor = availableHeight / totalOriginalContentHeight;

            foreach (var item in contentItems)
            {
                float newWidth = pageWidth + e.MarginBounds.Left + 20;  /*(item.OriginalWidth) * scaleFactor;*/
                float newHeight = item.OriginalHeight * scaleFactor + 3;

                // Verificar se a nova altura excede a altura disponível
                if (yPos + newHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true; // Indicar que há mais páginas
                    return yPos; // Retornar a posição atual
                }

                // Centralizar horizontalmente
                float imageXPos = e.MarginBounds.Left + (pageWidth - newWidth) / 2;

                e.Graphics.DrawImage(item.Image, imageXPos, yPos, newWidth, newHeight);
                yPos += newHeight + 10; // 10 é o espaçamento após cada imagem

                // Liberar recursos da imagem
                item.Image.Dispose();
            }

            // Ajustar a posição para a assinatura e data
            yPos += 20; // Espaçamento antes da assinatura

            return yPos; // Retornar a nova posição Y
        }


        private float DrawSignatureAndDate(PrintPageEventArgs e, float yPos, float pageWidth, Font fonteTexto, Brush brush)
        {
            string assinaturaText = "Assinatura";
            string dataMod = DataMod.Text;

            // Desenhar a assinatura
            SizeF assinaturaTextSize = e.Graphics.MeasureString(assinaturaText, fonteTexto);
            float signatureLineWidth = pageWidth / 2;
            float totalSignatureWidth = assinaturaTextSize.Width + signatureLineWidth;
            float signatureBlockX = e.MarginBounds.Left - 60/*+ (pageWidth - totalSignatureWidth) / 2*/;

            e.Graphics.DrawString(assinaturaText, fonteTexto, brush, signatureBlockX, yPos);
            float lineY = (yPos) + assinaturaTextSize.Height / 2;
            e.Graphics.DrawLine(Pens.Black, signatureBlockX + assinaturaTextSize.Width, lineY, signatureBlockX + assinaturaTextSize.Width + signatureLineWidth, lineY);

            yPos += assinaturaTextSize.Height; // Espaçamento após 'Assinatura'

            // Desenhar a data
            if (!string.IsNullOrEmpty(dataMod))
            {
                SizeF dataTextSize = e.Graphics.MeasureString(dataMod, fonteTexto);
                float dataX = (e.MarginBounds.Left - 60 + totalSignatureWidth / 2) - dataTextSize.Width / 2/*+ (pageWidth - dataTextSize.Width) / 2*/;
                e.Graphics.DrawString(dataMod, fonteTexto, brush, dataX, yPos);
                yPos += dataTextSize.Height + 20; // Espaçamento após a data
            }

            return yPos; // Retornar a nova posição Y
        }

        private void DrawBackgroundImage(PrintPageEventArgs e, float yPos, float pageWidth)
        {
            string bottomImagePath = "C:\\Users\\synys\\source\\repos\\TeleBerço\\Resources\\Morada2.jpeg";

            if (File.Exists(bottomImagePath))
            {
                using (Image bottomImage = Image.FromFile(bottomImagePath))
                {
                    float desiredBottomImgWidth = 600;
                    float bottomImgHeight = bottomImage.Height * (desiredBottomImgWidth / bottomImage.Width) - 75;

                    // Posicionar a imagem no fundo da página, centralizada horizontalmente
                    float bottomImageX = (e.MarginBounds.Left + pageWidth + 70) - desiredBottomImgWidth;
                    float bottomImageYPosition = e.MarginBounds.Top + e.MarginBounds.Height - bottomImgHeight + 110;

                    e.Graphics.DrawImage(bottomImage, bottomImageX, bottomImageYPosition, desiredBottomImgWidth, bottomImgHeight);
                }
            }
        }

        private Bitmap CapturePanel(Panel panel)
        {
            if (panel == Panel2)
            {
                Bitmap bitmap = new Bitmap(panel.Width - 150, panel.Height - 30);
                panel.DrawToBitmap(bitmap, new Rectangle(0, 0, panel.Width, panel.Height));
                return bitmap;

            }
            else
            {
                Bitmap bitmap = new Bitmap(panel.Width - 20, panel.Height - 10);
                panel.DrawToBitmap(bitmap, new Rectangle(0, 0, panel.Width, panel.Height));
                return bitmap;
            }
        }


        #endregion

        private void btnAbrirCliente_Click_1(object sender, EventArgs e)
        {
            AbrirSelecaoClientes();
        }

        private void btnNovoCliente_Click(object sender, EventArgs e)
        {
            try
            {
                dsClientes.NovoCliente();
                LimparCliente();
                TxtCodigoCl.Text = dsClientes.DaProxNrCliente();
                BtnGravarCliente.Enabled = true;
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
                if (ValidaPreenchimentoCliente())
                {
                    var clienteRow = dsClientes.Clientes[0];
                    clienteRow.Nome = TxtNomeCl.Text;
                    clienteRow.CodCl = TxtCodigoCl.Text;
                    clienteRow.Telefone = TxtTelefone.Text;
                    clienteRow.Email = TxtEmail.Text;

                    dsClientes.UpdateClientes();

                    MessageBox.Show("Cliente salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimparCliente();
                    BtnGravarCliente.Enabled = false;
                    TxtNomeCl.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Preencha todos os campos obrigatórios do cliente.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                if (frmDados.RowSelecionada is DsProdutos.ProdutosRow produtoRow)
                {
                    txtEquipNome.Text = produtoRow.NomeProduto;
                    txtCat.SelectedValue = produtoRow.Categorias;
                    txtMarca.SelectedValue = produtoRow.Marcas;
                    txtImei.Text = produtoRow.IMEI;
                    txtObservacoes.Text = produtoRow.Observacao;
                    HabilitarProduto();
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
    }
}
