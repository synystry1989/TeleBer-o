
using System.Data;
using System;


using System.Linq;


using TeleBerço.DsClientesTableAdapters;
using TeleBerço.DsDocumentosTableAdapters;
using TeleBerço;
using static TeleBerço.DsProdutos;



namespace TeleBerço
{
    partial class DsDocumentos
    {


        public CabecDocumentoTableAdapter adpCabecDocumento = new CabecDocumentoTableAdapter();
        public TipoDocumentosTableAdapter adpTipoDocumentos = new TipoDocumentosTableAdapter();
        public ClientesTableAdapter adpClientes = new ClientesTableAdapter();
        public ListaProdutosTableAdapter adpLinhas = new ListaProdutosTableAdapter();

        public void CarregaTipoDoc()
        {
            TipoDocumentos.Clear();
            adpTipoDocumentos.Fill(TipoDocumentos);
        }

        public void CarregaDocumento(string tipo, int nr)
        {
            CabecDocumento.Clear();
            adpCabecDocumento.FillByTpNr_Doc(CabecDocumento, tipo, nr);
        }

        public void CarregaLinhas(Guid id)
        {
            ListaProdutos.Clear();
            adpLinhas.FillByCabecDoc(ListaProdutos, id);
        }

        public void UpdateDoc()
        {
            adpCabecDocumento.Update(CabecDocumento);
        }

        public void UpdateLinhas()
        {
            adpLinhas.Update(ListaProdutos);
        }

        public void CarregarDocumentos()
        {
            CabecDocumento.Clear();
            adpCabecDocumento.Fill(CabecDocumento);

        }

        public int DaNrDocSeguinte(string tipoDoc)
        {        /*cast explicito*/
            return (int)adpCabecDocumento.UltmNrDoc(tipoDoc) + 1;
        }

        public CabecDocumentoRow PesquisaDocumento(string tipoDoc, int nrDoc)
        {

            CarregaDocumento(tipoDoc, nrDoc);
            if (CabecDocumento.Rows.Count > 0)
            {
                return CabecDocumento[0];
            }
            else
            {
                NovoDocumento();
                return CabecDocumento[0];
            }
        }

        public void NovoDocumento()
        {
            if (CabecDocumento.Rows.Count == 0)
            {
                CabecDocumentoRow newDocRow = CabecDocumento.NewCabecDocumentoRow();
                newDocRow.ID = Guid.NewGuid();
                newDocRow.TipoDocumento = "";
                newDocRow.NrDocumento = 0;
                newDocRow.DataRececao = DateTime.Now.Date;
                newDocRow.Cliente = "";
                newDocRow.DataEntrega = new DateTime().Date;
                newDocRow.Observacoes = "";
                newDocRow.Total = 0;
                newDocRow.CodProduto = "";
                newDocRow.Estado = "";
                newDocRow.NomeCliente = "";
                newDocRow.Desconto = 0;

                ListaProdutos.Clear();

                CabecDocumento.Rows.Add(newDocRow);
            }
        }

        public void NovaLinhaArtigos(ProdutosRow rowSelecionada)
        {
            NovoDocumento();

            ListaProdutosRow novaLinha = ListaProdutos.NewListaProdutosRow();

            novaLinha.ID = Guid.NewGuid();
            novaLinha.CabProdutoID = CabecDocumento[0].ID;

            int contador = 0;
            foreach (ListaProdutosRow row in ListaProdutos.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    contador++;
                }
            }
            novaLinha.NumLInha = contador + 1;
            novaLinha.NomeProduto = rowSelecionada.NomeProduto;
            novaLinha.Produto = rowSelecionada.CodPr;
            novaLinha.Observacao = rowSelecionada.Observacao;
            novaLinha.Quantidade = 0;
            novaLinha.PrecoUnt = rowSelecionada.PreçoVenda;
            novaLinha.Total = (decimal)0.00;
            novaLinha.IMEI = rowSelecionada.IMEI;
            novaLinha.Marca = rowSelecionada.Marcas;
            novaLinha.Categoria = rowSelecionada.Categorias;
            novaLinha.NomeMarca = rowSelecionada.Marca;
            novaLinha.NomeCategoria = rowSelecionada.Categoria;

            ListaProdutos.AddListaProdutosRow(novaLinha);
        }

        public void EliminarLinha(Guid id)
        {
            ListaProdutosRow linhaSelecionada = ListaProdutos.FindByID(id);

            foreach (ListaProdutosRow row in ListaProdutos.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row.NumLInha > linhaSelecionada.NumLInha)
                {
                    row.NumLInha -= 1;
                }
            }
            //if (linhaSelecionada != null)
            //{
            //    linhaSelecionada.Delete();
            //}
            linhaSelecionada?.Delete();
        }


    }


}
