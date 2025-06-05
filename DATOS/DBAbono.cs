using DATOS;
using ENTIDADES;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class DBAbono : ConexionOracle
    {
        public string SaveData(ABONO abono)
        {
            string query = "INSERT INTO ABONO (ID_CREDITO, FECHA_ABONO, MONTO_ABONADO, METODO_PAGO) " +
                           "VALUES (:CreditoId, :FechaAbono, :MontoAbonado, :MetodoPago)";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("CreditoId", abono.CreditoId));
                    command.Parameters.Add(new OracleParameter("FechaAbono", abono.FechaAbono));
                    command.Parameters.Add(new OracleParameter("MontoAbonado", abono.MontoAbonado));
                    command.Parameters.Add(new OracleParameter("MetodoPago", abono.MetodoPago ?? (object)DBNull.Value));

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

        public List<ABONO> GetAll()
        {
            List<ABONO> listaAbonos = new List<ABONO>();
            string query = "SELECT * FROM ABONO";

            try
            {
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    AbrirConexion();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaAbonos.Add(new ABONO
                            {
                                Id = Convert.ToInt32(reader["id_abono"]),
                                CreditoId = Convert.ToInt32(reader["id_credito"]),
                                FechaAbono = Convert.ToDateTime(reader["fecha_abono"]),
                                MontoAbonado = Convert.ToDecimal(reader["monto_abonado"]),
                                MetodoPago = Convert.ToString(reader["metodo_pago"])
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
            return listaAbonos;
        }

        public ABONO GetByID(int id)
        {
            return GetAll().Find(x => x.Id == id);
        }

        public string UpdateData(ABONO abono)
        {
            string query = "UPDATE ABONO SET ID_CREDITO = :CreditoId, FECHA_ABONO = :FechaAbono, " +
                           "MONTO_ABONADO = :MontoAbonado, METODO_PAGO = :MetodoPago WHERE id_abono = :Id";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion();
                transaction = conexion.BeginTransaction();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Id", abono.Id));
                    command.Parameters.Add(new OracleParameter("CreditoId", abono.CreditoId));
                    command.Parameters.Add(new OracleParameter("FechaAbono", abono.FechaAbono));
                    command.Parameters.Add(new OracleParameter("MontoAbonado", abono.MontoAbonado));
                    command.Parameters.Add(new OracleParameter("MetodoPago", abono.MetodoPago ?? (object)DBNull.Value));

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
            string query = "DELETE FROM ABONO WHERE id_abono = :Id";

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
