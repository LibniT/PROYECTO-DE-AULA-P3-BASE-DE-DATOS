using DAL;
using ENTIDADES;
using System.Collections.Generic;

namespace BLL
{
    public class DBLCliente
    {
        private DBCliente dbCliente;

        public DBLCliente()
        {
            dbCliente = new DBCliente();
        }

        public string AddCliente(CLIENTE cliente)
        {
            if (string.IsNullOrEmpty(cliente.Nombre))
            {
                return "El nombre es obligatorio.";
            }
            return dbCliente.SaveData(cliente);
        }

        public List<CLIENTE> GetAllClientes()
        {
            return dbCliente.GetAll();
        }

        public CLIENTE GetClienteById(int id)
        {
            return dbCliente.GetByID(id);
        }

        public string UpdateCliente(CLIENTE cliente)
        {
            if (string.IsNullOrEmpty(cliente.Nombre))
            {
                return "El nombre es obligatorio.";
            }
            return dbCliente.UpdateData(cliente);
        }

        public string DeleteCliente(int id)
        {
            return dbCliente.DeleteData(id);
        }
    }
}
