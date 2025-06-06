using DAL;
using ENTIDADES;
using System.Collections.Generic;

namespace BLL
{
    public class DBLAdministrador
    {
        private DBAdministrador dbAdmin;

        public DBLAdministrador()
        {
            dbAdmin = new DBAdministrador();
        }

        public string AddAdministrador(ADMINISTRADOR administrador)
        {
            if (string.IsNullOrEmpty(administrador.Nombre))
            {
                return "El nombre es obligatorio.";
            }
            return dbAdmin.SaveData(administrador);
        }

        public List<ADMINISTRADOR> GetAllAdministradores()
        {
            return dbAdmin.GetAll();
        }

        public ADMINISTRADOR GetAdministradorById(int id)
        {
            return dbAdmin.GetByID(id);
        }

        public string UpdateAdministrador(ADMINISTRADOR administrador)
        {
            if (string.IsNullOrEmpty(administrador.Nombre))
            {
                return "El nombre es obligatorio.";
            }
            return dbAdmin.UpdateData(administrador);
        }

        public string DeleteAdministrador(int id)
        {
            return dbAdmin.DeleteData(id);
        }

        public bool IsAdmin(string user, string password)
        {
            return dbAdmin.IsAdmin(user, password);
        }

    }
}
