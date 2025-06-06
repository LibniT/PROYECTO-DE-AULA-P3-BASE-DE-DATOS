using DAL;
using ENTIDADES;
using System.Collections.Generic;

namespace BLL
{
    public class DBLAbono
    {
        private DBAbono dbAbono;

        public DBLAbono()
        {
            dbAbono = new DBAbono();
        }

        public string AddAbono(ABONO abono)
        {
            if (abono.MontoAbonado <= 0)
            {
                return "El monto abonado debe ser mayor que 0.";
            }
            return dbAbono.SaveData(abono);
        }

        public List<ABONO> GetAllAbonos()
        {
            return dbAbono.GetAll();
        }

        public ABONO GetAbonoById(int id)
        {
            return dbAbono.GetByID(id);
        }

        public string UpdateAbono(ABONO abono)
        {
            if (abono.MontoAbonado <= 0)
            {
                return "El monto abonado debe ser mayor que 0.";
            }
            return dbAbono.UpdateData(abono);
        }

        public string DeleteAbono(int id)
        {
            return dbAbono.DeleteData(id);
        }
    }
}
