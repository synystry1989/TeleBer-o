using TeleBerço.DsClientesTableAdapters;
using TeleBerço;
using static TeleBerço.DsDocumentos;
using System.Data;
using System;





namespace TeleBerço
{
    partial class DsClientes
    {
        public ClientesTableAdapter adpClientes = new ClientesTableAdapter();


        public void CarregaClientes()
        {
            Clientes.Clear();
            adpClientes.Fill(Clientes);
        }
        public void UpdateClientes()
        {
            adpClientes.Update(Clientes);
        }

        public string DaUltimoCodCliente()
        {
            return adpClientes.UltmCodCl();
        }

        public void NovoCliente()
        {
            if (Clientes.Rows.Count == 0)
            {
                ClientesRow novoCliente = (ClientesRow)Clientes.NewRow();

                novoCliente.CodCl = DaProxNrCliente();
                novoCliente.Nome = "";
                novoCliente.Telefone = "";
                novoCliente.Email = "";


                Clientes.AddClientesRow(novoCliente);
            }
        }
        public string DaProxNrCliente()
        {

            string codCl = DaUltimoCodCliente();
            int valor = 0;
            if (codCl != null)
            {
                //retiramos o CL ao cod incrementamos 
                valor = int.Parse(codCl.Substring(2));
                valor++;
                return $"CL{valor:000}";
            }
            else
            //se nao existir 
            {
                valor = 001;
                return $"CL{valor:000}";
            }
        }


        public ClientesRow PesquisaCliente(string codCl)
        {
            adpClientes.FillByCodCl(Clientes, codCl);

            if (Clientes.Rows.Count > 0)
            {
                return Clientes[0];
            }
            NovoCliente();
            return Clientes[0];
        }

        public string CarregaNomeCliente(string codCl)
        {
            return adpClientes.NomeCliente(codCl);
        }

        public void EliminarCliente(string id)
        {
            ClientesRow linhaSelecionada = Clientes.FindByCodCl(id);


            linhaSelecionada?.Delete();

            UpdateClientes();
        }
    }

}

