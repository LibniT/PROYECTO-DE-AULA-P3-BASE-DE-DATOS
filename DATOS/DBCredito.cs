using DATOS;
using ENTIDADES;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class DBCredito : ConexionOracle
    {
        public string SaveData(CREDITO credito)
        {
            string query = "INSERT INTO CREDITO (ESTADO, ID_VENTA, FECHAVENCIMIENTO) " +
                           "VALUES (:Estado, :VentaId, :FechaVencimiento)";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Estado", credito.Estado));
                    command.Parameters.Add(new OracleParameter("VentaId", credito.VentaId));
                    command.Parameters.Add(new OracleParameter("FechaVencimiento", credito.FechaVencimiento));

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

        public List<CREDITO> GetAll()
        {
            List<CREDITO> listaCreditos = new List<CREDITO>();
            string query = "SELECT * FROM CREDITO";

            try
            {
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    AbrirConexion();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaCreditos.Add(new CREDITO
                            {
                                Id = Convert.ToInt32(reader["id_credito"]),
                                Estado = Convert.ToString(reader["estado"]),
                                VentaId = Convert.ToInt32(reader["id_venta"]),
                                FechaVencimiento = Convert.ToDateTime(reader["FechaVencimiento"])
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
            return listaCreditos;
        }

        public CREDITO GetByID(int id)
        {
            return GetAll().Find(x => x.Id == id);
        }

        public string UpdateData(CREDITO credito)
        {
            string query = "UPDATE CREDITO SET ESTADO = :Estado, ID_VENTA = :VentaId, " +
                           "FECHAVENCIMIENTO = :FechaVencimiento WHERE id_credito = :Id";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Id", credito.Id));
                    command.Parameters.Add(new OracleParameter("Estado", credito.Estado));
                    command.Parameters.Add(new OracleParameter("VentaId", credito.VentaId));
                    command.Parameters.Add(new OracleParameter("FechaVencimiento", credito.FechaVencimiento));

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
            string query = "DELETE FROM CREDITO WHERE id_credito = :Id";

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
