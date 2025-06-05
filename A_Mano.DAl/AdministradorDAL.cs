using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A_Mano.Entity;

namespace A_Mano.DAl
{
    public class AdministradorDAL : ConexionOracle
    {
        public AdministradorEntity VerificarCredenciales(string usuario, string contraseña = null)
        {
            AdministradorEntity admin = null;
            try
            {
                AbrirConexion();
                string query;

                if (contraseña == null)
                {
                    // Solo verificar si existe el usuario
                    query = "SELECT id_admin, nombre, usuario FROM ADMINISTRADOR WHERE usuario = :usuario";
                }
                else
                {
                    // Verificar usuario y contraseña
                    query = "SELECT id_admin, nombre, usuario FROM ADMINISTRADOR WHERE usuario = :usuario AND contraseña = :contraseña";
                }

                using (var cmd = new OracleCommand(query, conexion))
                {
                    cmd.Parameters.Add(new OracleParameter("usuario", usuario));
                    if (contraseña != null)
                    {
                        cmd.Parameters.Add(new OracleParameter("contraseña", contraseña));
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            admin = new AdministradorEntity
                            {
                                IdAdmin = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Usuario = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar administrador: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return admin;
        }
    }
}
