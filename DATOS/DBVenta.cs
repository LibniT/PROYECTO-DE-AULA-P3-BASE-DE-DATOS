using DATOS;
using ENTIDADES;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class DBVenta : ConexionOracle
    {
        public string SaveData(VENTA venta)
        {
            string query = "INSERT INTO VENTA (FECHA, DESCRIPCION, ID_CLIENTE, TOTAL) " +
                           "VALUES (:Fecha, :Descripcion, :ClienteId, :Total)";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Fecha", venta.Fecha));
                    command.Parameters.Add(new OracleParameter("Descripcion", venta.Descripcion ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("ClienteId", venta.ClienteId));
                    command.Parameters.Add(new OracleParameter("Total", venta.Total));

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

        public List<VENTA> GetAll()
        {
            List<VENTA> listaVentas = new List<VENTA>();
            string query = "SELECT * FROM VENTA";

            try
            {
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    AbrirConexion();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaVentas.Add(new VENTA
                            {
                                Id = Convert.ToInt32(reader["id_venta"]),
                                Fecha = Convert.ToDateTime(reader["fecha"]),
                                Descripcion = Convert.ToString(reader["descripcion"]),
                                ClienteId = Convert.ToInt32(reader["id_cliente"]),
                                Total = Convert.ToDecimal(reader["total"])
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
            return listaVentas;
        }

        public VENTA GetByID(int id)
        {
            return GetAll().Find(x => x.Id == id);
        }

        public string UpdateData(VENTA venta)
        {
            string query = "UPDATE VENTA SET FECHA = :Fecha, DESCRIPCION = :Descripcion, " +
                           "ID_CLIENTE = :ClienteId, TOTAL = :Total WHERE id_venta = :Id";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Id", venta.Id));
                    command.Parameters.Add(new OracleParameter("Fecha", venta.Fecha));
                    command.Parameters.Add(new OracleParameter("Descripcion", venta.Descripcion ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("ClienteId", venta.ClienteId));
                    command.Parameters.Add(new OracleParameter("Total", venta.Total));

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
            string query = "DELETE FROM VENTA WHERE id_venta = :Id";

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
