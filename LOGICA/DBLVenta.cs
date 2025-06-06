using DAL;
using ENTIDADES;
using System.Collections.Generic;

namespace BLL
{
    public class DBLVenta
    {
        private DBVenta dbVenta;

        public DBLVenta()
        {
            dbVenta = new DBVenta();
        }

        public string AddVenta(VENTA venta)
        {
            if (venta.ClienteId <= 0)
            {
                return "El ID del cliente es obligatorio.";
            }
            return dbVenta.SaveData(venta);
        }

        public List<VENTA> GetAllVentas()
        {
            return dbVenta.GetAll();
        }

        public VENTA GetVentaById(int id)
        {
            return dbVenta.GetByID(id);
        }

        public string UpdateVenta(VENTA venta)
        {
            if (venta.ClienteId <= 0)
            {
                return "El ID del cliente es obligatorio.";
            }
            return dbVenta.UpdateData(venta);
        }

        public string DeleteVenta(int id)
        {
            return dbVenta.DeleteData(id);
        }
    }
}
