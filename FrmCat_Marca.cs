using System;
using System.Data;
using System.Windows.Forms;
using static TeleBerço.DsClientes;
using static TeleBerço.DsProdutos;

namespace TeleBerço
{
    public partial class FrmCat_Marca : Form
    {
        public FrmCat_Marca()
        {
            InitializeComponent();
        }

        public DataRow RowSelecionada { get;  set; }

        DsProdutos dsProdutos = new DsProdutos();

        public enum TipoDados
        {
            Marcas,
            Categorias
        }
        public TipoDados tipoDadosAtual;
        private void FrmCat_Marca_Load(object sender, EventArgs e)
        {
            
            if (tipoDadosAtual == TipoDados.Categorias)
            {
                if (RowSelecionada != null)
                {
                    var Row = (CategoriasRow)RowSelecionada;
                    TxtNome.Text= Row.Nome ;
                    TxtCodigo.Text= Row.CodCat ;
                }
                else
                {
                    dsProdutos.NovaCategoria();
                
                }
                LabelMarca.Visible = false;
            }
            else if (tipoDadosAtual == TipoDados.Marcas)
            {
                if (RowSelecionada != null)
                {
                    var Row = (MarcasRow)RowSelecionada;

                   TxtNome.Text=Row.Nome;
              
                }
                else
                {
                    dsProdutos.NovaMarca();
                   
                }
                TxtCodigo.Visible = false;
                LabelMarca.Visible = true;
                label1.Visible = false;

            }

        }
        private bool Verificacao()
        {
            return !string.IsNullOrWhiteSpace(TxtCodigo.Text) &&
                   !string.IsNullOrWhiteSpace(TxtNome.Text);

        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            
                TxtCodigo.Text = "";
                TxtNome.Text = string.Empty;
          
        }

        private void BtnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                if (tipoDadosAtual == TipoDados.Categorias)
                {
                    CategoriasRow catRow = dsProdutos.Categorias[0];

                    if ((catRow != null) && (Verificacao()))
                    {

                        catRow.CodCat = TxtCodigo.Text;
                        catRow.Nome = TxtNome.Text;
                        dsProdutos.UpdateCategorias();

                        MessageBox.Show($"Categoria Gravada com sucesso.", "Confirmação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
                else
                {
                    MarcasRow Row = dsProdutos.Marcas[0];

                    if (Row != null) 
                    {                      
                        Row.Nome = TxtNome.Text;
                        dsProdutos.UpdateMarcas();
                        MessageBox.Show($"Marca Gravada com sucesso.", "Confirmação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
            }
            catch (Exception ex)
            {
                if (tipoDadosAtual == TipoDados.Categorias)
                {
                    MessageBox.Show($"Erro ao gravar categoria: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Erro ao gravar Marca: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }

        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (tipoDadosAtual == TipoDados.Categorias)
            {
                CategoriasRow catRow = dsProdutos.Categorias[0];
                if ((catRow != null) && (Verificacao()))
                {


                    catRow.CodCat = TxtCodigo.Text;
                    catRow.Nome = TxtNome.Text;
                    dsProdutos.UpdateCategorias();

                }
            }
            else
            {
                MarcasRow Row = dsProdutos.Marcas[0];
                if ((Row != null) && (Verificacao()))
                {
                    Row.Id = int.Parse(TxtCodigo.Text);
                    Row.Nome = TxtNome.Text;
                    dsProdutos.UpdateMarcas();
                }
            }
        }
    }
}
