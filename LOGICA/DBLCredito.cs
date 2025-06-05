using DAL;
using ENTIDADES;
using System.Collections.Generic;

namespace BLL
{
    public class DBLCredito
    {
        private DBCredito dbCredito;

        public DBLCredito()
        {
            dbCredito = new DBCredito();
        }

        public string AddCredito(CREDITO credito)
        {
            if (string.IsNullOrEmpty(credito.Estado))
            {
                return "El estado es obligatorio.";
            }
            return dbCredito.SaveData(credito);
        }

        public List<CREDITO> GetAllCreditos()
        {
            return dbCredito.GetAll();
        }

        public CREDITO GetCreditoById(int id)
        {
            return dbCredito.GetByID(id);
        }

        public string UpdateCredito(CREDITO credito)
        {
            if (string.IsNullOrEmpty(credito.Estado))
            {
                return "El estado es obligatorio.";
            }
            return dbCredito.UpdateData(credito);
        }

        public string DeleteCredito(int id)
        {
            return dbCredito.DeleteData(id);
        }
    }
}
