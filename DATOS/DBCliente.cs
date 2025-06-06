using DATOS;
using ENTIDADES;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class DBCliente : ConexionOracle
    {
        public string SaveData(CLIENTE cliente)
        {
            string query = "INSERT INTO CLIENTE (CEDULA, NOMBRE, TELEFONO, EMAIL, DEUDATOTAL, DIRECCION, LIMITECREDITO, ESTADO, STRIKES) " +
                           "VALUES (:Cedula, :Nombre, :Telefono, :Email, :DeudaTotal, :Direccion, :LimiteCredito, :Estado, :Strikes)";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Cedula", cliente.Cedula));
                    command.Parameters.Add(new OracleParameter("Nombre", cliente.Nombre));
                    command.Parameters.Add(new OracleParameter("Telefono", cliente.Telefono ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("Email", cliente.Email ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("DeudaTotal", cliente.DeudaTotal));
                    command.Parameters.Add(new OracleParameter("Direccion", cliente.Direccion ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("LimiteCredito", cliente.LimiteCredito));
                    command.Parameters.Add(new OracleParameter("Estado", cliente.Estado));
                    command.Parameters.Add(new OracleParameter("Strikes", cliente.Strikes));

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

        public List<CLIENTE> GetAll()
        {
            List<CLIENTE> listaClientes = new List<CLIENTE>();
            string query = "SELECT * FROM CLIENTE";

            try
            {
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    AbrirConexion();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaClientes.Add(new CLIENTE
                            {
                                Id = Convert.ToInt32(reader["id_cliente"]),
                                Cedula = Convert.ToString(reader["cedula"]),
                                Nombre = Convert.ToString(reader["nombre"]),
                                Telefono = Convert.ToString(reader["telefono"]),
                                Email = Convert.ToString(reader["email"]),
                                DeudaTotal = Convert.ToDecimal(reader["deudaTotal"]),
                                Direccion = Convert.ToString(reader["direccion"]),
                                LimiteCredito = Convert.ToDecimal(reader["limiteCredito"]),
                                Estado = Convert.ToString(reader["estado"]),
                                Strikes = Convert.ToInt32(reader["strikes"])
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
            return listaClientes;
        }

        public CLIENTE GetByID(int id)
        {
            return GetAll().Find(x => x.Id == id);
        }

        public string UpdateData(CLIENTE cliente)
        {
            string query = "UPDATE CLIENTE SET NOMBRE = :Nombre, TELEFONO = :Telefono, " +
                           "EMAIL = :Email, DIRECCION = :Direccion, LIMITECREDITO = :LimiteCredito " +
                           "WHERE id_cliente = :Id";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Nombre", cliente.Nombre));
                    command.Parameters.Add(new OracleParameter("Telefono", cliente.Telefono ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("Email", cliente.Email ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("Direccion", cliente.Direccion ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("LimiteCredito", cliente.LimiteCredito));
                    command.Parameters.Add(new OracleParameter("Id", cliente.Id)); // Siempre debe ir en el orden correcto

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
            string query = "DELETE FROM CLIENTE WHERE id_cliente = :Id";

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
    }
}
