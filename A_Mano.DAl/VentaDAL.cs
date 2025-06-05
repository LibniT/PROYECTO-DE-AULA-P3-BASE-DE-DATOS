using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A_Mano.Entity;

namespace A_Mano.DAl
{
    public class VentaDAL : ConexionOracle
    {
        public int Insertar(VentaEntity venta)
        {
            int ventaId = 0;
            try
            {
                AbrirConexion();
                using (var transaction = conexion.BeginTransaction())
                {
                    try
                    {
                        // Insertar venta
                        string queryVenta = @"
                            INSERT INTO VENTA (descripcion, id_cliente, total)
                            VALUES (:descripcion, :clienteId, :total)";

                        using (var cmd = new OracleCommand(queryVenta, conexion))
                        {
                            cmd.Transaction = transaction;
                            cmd.Parameters.Add(new OracleParameter("descripcion", venta.Descripcion));
                            cmd.Parameters.Add(new OracleParameter("clienteId", venta.IdCliente));
                            cmd.Parameters.Add(new OracleParameter("total", venta.Total));
                            cmd.ExecuteNonQuery();
                        }

                        // Obtener el ID de la venta recién insertada
                        string queryGetId = @"
                            SELECT MAX(id_venta) 
                            FROM VENTA 
                            WHERE id_cliente = :clienteId 
                            AND total = :total 
                            AND descripcion = :descripcion";

                        using (var cmd = new OracleCommand(queryGetId, conexion))
                        {
                            cmd.Transaction = transaction;
                            cmd.Parameters.Add(new OracleParameter("clienteId", venta.IdCliente));
                            cmd.Parameters.Add(new OracleParameter("total", venta.Total));
                            cmd.Parameters.Add(new OracleParameter("descripcion", venta.Descripcion));

                            var result = cmd.ExecuteScalar();
                            ventaId = Convert.ToInt32(result);
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al insertar venta: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return ventaId;
        }

        public List<VentaEntity> ObtenerPorCliente(int clienteId)
        {
            var ventas = new List<VentaEntity>();
            try
            {
                AbrirConexion();
                string query = @"
                    SELECT v.id_venta, v.fecha, v.total, v.descripcion
                    FROM VENTA v
                    WHERE v.id_cliente = :clienteId
                    ORDER BY v.total DESC
                    FETCH FIRST 10 ROWS ONLY";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    cmd.Parameters.Add(new OracleParameter("clienteId", clienteId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ventas.Add(new VentaEntity
                            {
                                IdVenta = reader.GetInt32(0),
                                Fecha = reader.GetDateTime(1),
                                Total = ConvertirDecimalOracle(reader.GetValue(2)),
                                Descripcion = reader.IsDBNull(3) ? "Sin descripción" : reader.GetString(3),
                                IdCliente = clienteId
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener ventas por cliente: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return ventas;
        }

        public List<VentaEntity> ObtenerPorClienteYFecha(int clienteId, DateTime fecha)
        {
            var ventas = new List<VentaEntity>();
            try
            {
                AbrirConexion();
                string query = @"
                    SELECT v.id_venta, v.fecha, v.total, v.descripcion
                    FROM VENTA v
                    WHERE v.id_cliente = :clienteId 
                    AND TRUNC(v.fecha) = TRUNC(:fecha)
                    ORDER BY v.fecha";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    cmd.Parameters.Add(new OracleParameter("clienteId", clienteId));
                    cmd.Parameters.Add(new OracleParameter("fecha", fecha));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ventas.Add(new VentaEntity
                            {
                                IdVenta = reader.GetInt32(0),
                                Fecha = reader.GetDateTime(1),
                                Total = ConvertirDecimalOracle(reader.GetValue(2)),
                                Descripcion = reader.IsDBNull(3) ? "Sin descripción" : reader.GetString(3),
                                IdCliente = clienteId
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener ventas por fecha: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return ventas;
        }
    }
}
