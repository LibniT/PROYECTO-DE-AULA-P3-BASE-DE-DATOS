using DATOS;
using ENTIDADES;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class DBAdministrador : ConexionOracle
    {
        public string SaveData(ADMINISTRADOR administrador)
        {
            string query = "INSERT INTO ADMINISTRADOR (NOMBRE, TELEFONO, EMAIL, USUARIO, CONTRASEÑA) " +
                           "VALUES (:Nombre, :Telefono, :Email, :Usuario, :Contraseña)";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Nombre", administrador.Nombre));
                    command.Parameters.Add(new OracleParameter("Telefono", administrador.Telefono ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("Email", administrador.Email ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("Usuario", administrador.Usuario));
                    command.Parameters.Add(new OracleParameter("Contraseña", administrador.Contraseña));

                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                return "Registro exitoso";
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                return "Error: " + ex.Message;
            }
            finally
            {
                CerrarConexion();
            }
        }

        public List<ADMINISTRADOR> GetAll()
        {
            List<ADMINISTRADOR> listaAdministradores = new List<ADMINISTRADOR>();
            string query = "SELECT * FROM ADMINISTRADOR";

            try
            {
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    AbrirConexion();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaAdministradores.Add(new ADMINISTRADOR
                            {
                                Id = Convert.ToInt32(reader["id_admin"]),
                                Nombre = Convert.ToString(reader["nombre"]),
                                Telefono = Convert.ToString(reader["telefono"]),
                                Email = Convert.ToString(reader["email"]),
                                Usuario = Convert.ToString(reader["usuario"]),
                                Contraseña = Convert.ToString(reader["contraseña"])
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                CerrarConexion();
            }
            return listaAdministradores;
        }

        public ADMINISTRADOR GetByID(int id)
        {
            return GetAll().Find(x => x.Id == id);
        }

        public string UpdateData(ADMINISTRADOR administrador)
        {
            string query = "UPDATE ADMINISTRADOR SET NOMBRE = :Nombre, TELEFONO = :Telefono, EMAIL = :Email, " +
                           "USUARIO = :Usuario, CONTRASEÑA = :Contraseña WHERE id_admin = :Id";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Id", administrador.Id));
                    command.Parameters.Add(new OracleParameter("Nombre", administrador.Nombre));
                    command.Parameters.Add(new OracleParameter("Telefono", administrador.Telefono ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("Email", administrador.Email ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("Usuario", administrador.Usuario));
                    command.Parameters.Add(new OracleParameter("Contraseña", administrador.Contraseña));

                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                return "Actualización exitosa";
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                return "Error: " + ex.Message;
            }
            finally
            {
                CerrarConexion();
            }
        }

        public string DeleteData(int id)
        {
            string query = "DELETE FROM ADMINISTRADOR WHERE id_admin = :Id";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Id", id));
                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                return "Eliminación exitosa";
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                return "Error: " + ex.Message;
            }
            finally
            {
                CerrarConexion();
            }
        }

        public bool IsAdmin(string user, string password)
        {
            string query = "SELECT COUNT(*) FROM ADMINISTRADOR WHERE USUARIO = :Usuario AND CONTRASEÑA = :Contraseña";
            try
            {
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Usuario", user));
                    command.Parameters.Add(new OracleParameter("Contraseña", password));
                    AbrirConexion();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                CerrarConexion();
            }
        }

    }
}
