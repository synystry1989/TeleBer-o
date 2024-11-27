using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TeleBerço.DsProdutosTableAdapters;

namespace TeleBerço
{
    public partial class FrmDados : Form
    {
        private enum TipoDados
        {
            Clientes,
            Produtos,
            Documentos,
            Marcas,
            Categorias
        }

        // Datasets
        private DsClientes dsClientes = new DsClientes();
        private DsProdutos dsArtigos = new DsProdutos();
        private DsDocumentos dsDocumentos = new DsDocumentos();

        // TableAdapters

        private QuerryProdutosTableAdapter querryProdutosTableAdapter = new QuerryProdutosTableAdapter();


        public DataRow RowSelecionada { get; private set; }
        private TipoDados tipoDadosAtual;
        private DataView dataViewAtual;

        public FrmDados()
        {
            InitializeComponent();

        }

        public void MostrarTabelaDados(string tabela)
        {
            switch (tabela)
            {
                case "DsClientes":
                    tipoDadosAtual = TipoDados.Clientes;
                    CarregarClientes();
                    break;
                case "DsArtigos":
                    tipoDadosAtual = TipoDados.Produtos;
                    CarregarProdutos();
                    break;
                case "DsDocumentos":
                    tipoDadosAtual = TipoDados.Documentos;
                    CarregarDocumentos();
                    break;
                case "DsCategorias":
                    tipoDadosAtual = TipoDados.Categorias;
                    CarregarCategorias();
                    break;
                case "DsMarcas":
                    tipoDadosAtual = TipoDados.Marcas;
                    CarregarMarcas();
                    break;
            }

            ConfigurarDataGridView();
            ConfigurarControles();
            ShowDialog();
        }

        private void ConfigurarControles()
        {
            try
            {
                // Configura os controles de filtro e pesquisa com base no tipo de dados atual
                cbFiltro.Items.Clear();
                cbOrdenar.Items.Clear();
                txtNome.Text = string.Empty;
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;

                switch (tipoDadosAtual)
                {
                    case TipoDados.Clientes:
                        cbOrdenar.Enabled = false;

                        break;

                    case TipoDados.Produtos:
                        cbFiltro.Enabled = true;
                        cbOrdenar.Items.AddRange(new string[] { "Categoria", "Marca", "Tipo" });
                        break;

                    case TipoDados.Documentos:
                        cbOrdenar.Enabled = true;
                        cbOrdenar.Items.AddRange(new string[] { "Data", "Tipo Doc", "Estado" });
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro nas configurações iniciais: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }
        private void ConfigurarDataGridView()
        {
            try
            {
                switch (tipoDadosAtual)
                {
                    case TipoDados.Clientes:
                        DgridDados.Columns["CodCl"].HeaderText = "Código Cliente";
                        break;

                    case TipoDados.Produtos:
                        DgridDados.Columns["Marcas"].Visible = false;
                        DgridDados.Columns["Categorias"].Visible = false;

                        DgridDados.Columns["PrecoCusto"].DefaultCellStyle.Format = "F2";
                        DgridDados.Columns["PreçoVenda"].DefaultCellStyle.Format = "F2";
                        break;

                    case TipoDados.Documentos:
                        DgridDados.Columns["Cliente"].Visible = false;

                        DgridDados.Columns["Total"].DefaultCellStyle.Format = "F2";
                        DgridDados.Columns["NomeCliente"].HeaderText = "Cliente";
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro nas configuracoes iniciais: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CarregarClientes()
        {
            try
            {
                dsClientes.CarregaClientes();
                dataViewAtual = new DataView(dsClientes.Clientes);
                DgridDados.DataSource = dataViewAtual;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar clientes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void CarregarProdutos()
        {
            try
            {
                dsArtigos.CarregaArtigos();
                AdicionarDetalhesProdutos();
                dataViewAtual = new DataView(dsArtigos.Produtos);
                DgridDados.DataSource = dataViewAtual;
            }
            catch
            {
                MessageBox.Show("Erro ao carregar Produtos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CarregarDocumentos()
        {
            try
            {
                dsDocumentos.CarregarDocumentos();
                AdicionarNomeClientesAosDocumentos();
                dataViewAtual = new DataView(dsDocumentos.CabecDocumento);
                DgridDados.DataSource = dataViewAtual;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao carregar Documentos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }
        private void CarregarCategorias()
        {
            try
            {
                dsArtigos.CarregaCategorias();
                dataViewAtual = new DataView(dsArtigos.Categorias);
                DgridDados.DataSource = dataViewAtual;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao carregar Categorias", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void CarregarMarcas()
        {
            try
            {
                dsArtigos.CarregarMarcas();
                dataViewAtual = new DataView(dsArtigos.Marcas);
                DgridDados.DataSource = dataViewAtual;

            }
            catch
            {
                MessageBox.Show("Erro ao carregar Marcas", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }            
        }

        private void AdicionarDetalhesProdutos()
        {
            // Adiciona colunas de Marca e Categoria se não existirem
            try
            {
                foreach (DataRow row in dsArtigos.Produtos.Rows)
                {
                    // Preenche a coluna Marca
                    if (row["Marcas"] != DBNull.Value)
                        row["Marca"] = querryProdutosTableAdapter.NomeMarca((int)row["Marcas"]);

                    // Preenche a coluna Categoria
                    if (row["Categorias"] != DBNull.Value)
                        row["Categoria"] = querryProdutosTableAdapter.NomeCategoria(row["Categorias"].ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao carregar Marcas", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }          
        }

        private void AdicionarNomeClientesAosDocumentos()
        {
            // Adiciona a coluna NomeCliente se não existir

            foreach (DataRow row in dsDocumentos.CabecDocumento.Rows)
            {
                // Preenche a coluna NomeCliente
                if (row["Cliente"] != DBNull.Value)
                    row["NomeCliente"] = dsClientes.CarregaNomeCliente(row["Cliente"].ToString());
            }
        }

        private void PesquisarClientes(string nome)
        {
            dataViewAtual.RowFilter = $"Nome LIKE '%{nome}%' OR Email LIKE '%{nome}%' OR Telefone LIKE '%{nome}%' OR CodCl LIKE '%{nome}%'";
        }

        private void PesquisarProdutos(string termo)
        {
            // Pesquisa em várias colunas
            dataViewAtual.RowFilter = $"NomeProduto LIKE '%{termo}%' OR Marca LIKE '%{termo}%' OR Categoria LIKE '%{termo}%'OR IMEI LIKE '%{termo}%' OR Tipo LIKE '%{termo}%'";
        }

        private void PesquisarDocumentosPorCliente(string cliente)
        {
            // Primeiro, tenta filtrar pelo nome do cliente
            dataViewAtual.RowFilter = $"NomeCliente LIKE '%{cliente}%'";

            // Se não encontrar, tenta buscar pelo código do cliente
            if (dataViewAtual.Count == 0)
            {
                string codCliente = dsClientes.Clientes.AsEnumerable()
                    .FirstOrDefault(c => c.Nome.Equals(cliente, StringComparison.OrdinalIgnoreCase))?.CodCl;

                if (!string.IsNullOrEmpty(codCliente))
                {
                    dataViewAtual.RowFilter = $"Cliente = '{codCliente}'";
                }
                else
                {
                    MessageBox.Show("Cliente não encontrado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void AplicarFiltro(string campo, string valor)
        {
            switch (tipoDadosAtual)
            {
                case TipoDados.Produtos:
                    if (campo == "Categoria" || campo == "Marca" || campo == "Tipo")
                    {
                        dataViewAtual.RowFilter = $"{campo} = '{valor}'";
                    }
                    break;

                case TipoDados.Documentos:
                    if ((campo == "Tipo Doc") || (campo == "Estado"))
                    {
                        dataViewAtual.RowFilter = $"{campo}  = '{valor}'";
                    }

                    break;
            }
        }

        private void AplicarFiltroPorDatas(DateTime dataInicio, DateTime dataFim)
        {
            string campoData = cbFiltro.SelectedItem.ToString().Replace(" ", "");
            dataViewAtual.RowFilter = $"{campoData} >= '{dataInicio:yyyy-MM-dd}' AND {campoData} <= '{dataFim:yyyy-MM-dd}'";
        }

        private void AplicarFiltroPorData(DateTime data)
        {
            string campoData = cbFiltro.SelectedItem.ToString().Replace(" ", "");
            dataViewAtual.RowFilter = $"{campoData} = '{data:yyyy-MM-dd}'";
        }

        private void SelecionarLinhaAtual()
        {
            try
            {
                if (DgridDados.CurrentRow != null && DgridDados.CurrentRow.DataBoundItem != null)
                {
                    RowSelecionada = ((DataRowView)DgridDados.CurrentRow.DataBoundItem).Row;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao selecionar item: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EditarCliente()
        {
            try
            {
                FrmClientes frmClientes = new FrmClientes
                {
                    RowSelecionada = RowSelecionada
                };
                frmClientes.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao editar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EditarProduto()
        {
            try
            {
                FrmProdutos frmProdutos = new FrmProdutos
                {
                    RowSelecionada = RowSelecionada
                };
                frmProdutos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao editar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AdicionarCliente()
        {
            try
            {
                FrmClientes frmClientes = new FrmClientes();
                frmClientes.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void AdicionarCategoria()
        {
            try
            {
                FrmCat_Marca frm = new FrmCat_Marca();
                frm.tipoDadosAtual = FrmCat_Marca.TipoDados.Categorias;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void AdicionarMarca()
        {
            try
            {
                FrmCat_Marca frm = new FrmCat_Marca();
                frm.tipoDadosAtual = FrmCat_Marca.TipoDados.Marcas;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EditarCat()

        {
            try
            {
                FrmCat_Marca frm = new FrmCat_Marca
                {
                    RowSelecionada = RowSelecionada,

                };
                frm.tipoDadosAtual = FrmCat_Marca.TipoDados.Categorias;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao editar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void EditarMarca()

        {
            try
            {
                FrmCat_Marca frm = new FrmCat_Marca
                {
                    RowSelecionada = RowSelecionada
                };
                frm.tipoDadosAtual = FrmCat_Marca.TipoDados.Marcas;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao editar cliente: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void AdicionarProduto()
        {
            try
            {
                FrmProdutos frmProdutos = new FrmProdutos();
                frmProdutos.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void BtnOk_Click(object sender, EventArgs e)
        {
            SelecionarLinhaAtual();
            DialogResult = DialogResult.OK;
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            SelecionarLinhaAtual();

            if (RowSelecionada != null)
            {
                switch (tipoDadosAtual)
                {
                    case TipoDados.Clientes:
                        EditarCliente();
                        break;
                    case TipoDados.Produtos:
                        EditarProduto();
                        break;
                    case TipoDados.Categorias:
                        EditarCat();


                        break;
                    case TipoDados.Marcas:
                        EditarMarca();

                        break;
                }
            }
        }

        private void BtnAdicionar_Click(object sender, EventArgs e)
        {
            switch (tipoDadosAtual)
            {
                case TipoDados.Clientes:
                    AdicionarCliente();
                    break;
                case TipoDados.Produtos:
                    AdicionarProduto();
                    break;
                case TipoDados.Categorias:
                    AdicionarCategoria();
                    break;
                case TipoDados.Marcas:
                    AdicionarMarca();
                    break;
            }
        }


        private void BtnSair_Click(object sender, EventArgs e)
        {
            try
            {
                RowSelecionada = null;
                DialogResult = DialogResult.Cancel;
                Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao encerrar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //selecionar na grid com enter
        private void DgridDados_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelecionarLinhaAtual();
                if (RowSelecionada != null)
                {
                    DialogResult = DialogResult.OK;
                }

                e.Handled = true;
            }

        }
        //pesquisar entre datas pressionar enter
        private void dateTimePicker2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    AplicarFiltroPorData(dateTimePicker2.Value.Date);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao Pesquisar, " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dateTimePicker2.Value > dateTimePicker1.Value)
                {
                    MessageBox.Show("A data inicial não pode ser maior que a data final.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                AplicarFiltroPorDatas(dateTimePicker2.Value.Date, dateTimePicker1.Value.Date);
            }
        }
        //carregar fontes no botao de filtro

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNome.Text))
            {
                MessageBox.Show("Por favor, insira um termo para pesquisa.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            switch (tipoDadosAtual)
            {
                case TipoDados.Clientes:
                    PesquisarClientes(txtNome.Text);
                    break;

                case TipoDados.Produtos:
                    PesquisarProdutos(txtNome.Text);
                    break;

                case TipoDados.Documentos:
                    PesquisarDocumentosPorCliente(txtNome.Text);
                    break;
            }
        }
        //fill da grid consoante os filtros
        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (cbOrdenar.SelectedItem == null || cbFiltro.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione um campo e um valor para filtrar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string campo = cbOrdenar.SelectedItem.ToString();
            string valor = cbFiltro.SelectedItem.ToString();

            AplicarFiltro(campo, valor);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Limpa filtros e recarrega os dados
            txtNome.Text = string.Empty;
            cbOrdenar.Text = "";
            cbFiltro.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker1.Visible = false;
            dateTimePicker2.Visible = false;


            switch (tipoDadosAtual)
            {
                case TipoDados.Clientes:
                    CarregarClientes();
                    break;
                case TipoDados.Produtos:
                    CarregarProdutos();
                    break;
                case TipoDados.Documentos:
                    CarregarDocumentos();
                    break;
                case TipoDados.Categorias:

                    CarregarCategorias();
                    break;
                case TipoDados.Marcas:

                    CarregarMarcas();
                    break;
            }

        }

        private void cbOrdenar_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbFiltro.Items.Clear();

            if (cbOrdenar.SelectedItem == null)
                return;

            string campo = cbOrdenar.SelectedItem.ToString();

            switch (tipoDadosAtual)
            {
                case TipoDados.Produtos:
                    if (campo == "Categoria")
                    {
                        dsArtigos.CarregaCategorias();
                        var categorias = dsArtigos.Categorias.AsEnumerable().Select(c => c.Nome).ToArray();
                        cbFiltro.Items.AddRange(categorias);
                    }
                    else if (campo == "Marca")
                    {
                        dsArtigos.CarregarMarcas();
                        var marcas = dsArtigos.Marcas.AsEnumerable().Select(m => m.Nome).ToArray();
                        cbFiltro.Items.AddRange(marcas);
                    }
                    else if (campo == "Tipo")
                    {
                        cbFiltro.Items.AddRange(new string[] { "Cliente", "Reparaçao", "Venda" }); // Ajuste conforme necessário
                    }
                    break;

                case TipoDados.Documentos:
                    if (campo == "Data")
                    {
                        cbFiltro.Items.AddRange(new string[] { "Data Entrega", "Data Rececao" }); // Ajuste conforme necessário

                        dateTimePicker1.Visible = true;
                        dateTimePicker2.Visible = true;
                    }
                    else if (campo == "Tipo Doc")
                    {
                        dsDocumentos.CarregaTipoDoc();
                        var tiposDoc = dsDocumentos.TipoDocumentos.AsEnumerable().Select(t => t.Descricao).ToArray();
                        cbFiltro.Items.AddRange(tiposDoc);
                    }
                    else if (campo == "Estado")
                    {
                        cbFiltro.Items.AddRange(new string[] { "Pronto", "Em Preparacao", "Cancelado", "Em Espera" }); // Ajuste conforme necessário
                    }
                    break;
            }

            cbFiltro.Enabled = cbFiltro.Items.Count > 0;
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

            if ((string.IsNullOrEmpty(txtNome.Text)) && (cbFiltro.Text == ""))
            {
                // Se o texto for apagado, recarrega os dados originais
                button1_Click(sender, e);
            }

        }


    }

}





