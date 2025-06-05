using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A_Mano.Entity;

namespace A_Mano.DAl
{
    public class CreditoDAL : ConexionOracle
    {
        public void Insertar(CreditoEntity credito)
        {
            try
            {
                AbrirConexion();
                string query = @"
                    INSERT INTO CREDITO (estado, id_venta, FechaVencimiento)
                    VALUES (:estado, :idVenta, :fechaVencimiento)";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    cmd.Parameters.Add(new OracleParameter("estado", credito.Estado));
                    cmd.Parameters.Add(new OracleParameter("idVenta", credito.IdVenta));
                    cmd.Parameters.Add(new OracleParameter("fechaVencimiento", credito.FechaVencimiento));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al insertar crédito: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
        }

        public List<CreditoEntity> ObtenerPendientesPorCliente(int clienteId)
        {
            var creditos = new List<CreditoEntity>();
            try
            {
                AbrirConexion();
                string query = @"
                    SELECT c.id_credito, v.total, c.FechaVencimiento, c.estado,
                           NVL((SELECT SUM(a.monto_abonado) FROM ABONO a WHERE a.id_credito = c.id_credito), 0) as totalAbonado
                    FROM CREDITO c
                    JOIN VENTA v ON c.id_venta = v.id_venta
                    WHERE v.id_cliente = :clienteId AND c.estado != 'Pagado'
                    ORDER BY c.FechaVencimiento";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    cmd.Parameters.Add(new OracleParameter("clienteId", clienteId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {   
                            creditos.Add(new CreditoEntity
                            {
                                IdCredito = reader.GetInt32(0),
                                MontoTotal = ConvertirDecimalOracle(reader.GetValue(1)),
                                FechaVencimiento = reader.GetDateTime(2),
                                Estado = reader.GetString(3),
                                TotalAbonado = ConvertirDecimalOracle(reader.GetValue(4))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener créditos pendientes: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return creditos;
        }

        public List<CreditoNotificacion> ObtenerPorVencer(int dias)
        {
            var creditos = new List<CreditoNotificacion>();
            try
            {
                AbrirConexion();
                string query = @"
                    SELECT cl.id_cliente, cl.nombre, cr.id_credito, v.total, cr.FechaVencimiento,
                           NVL((SELECT SUM(a.monto_abonado) FROM ABONO a WHERE a.id_credito = cr.id_credito), 0) as totalAbonado
                    FROM CLIENTE cl
                    JOIN VENTA v ON v.id_cliente = cl.id_cliente
                    JOIN CREDITO cr ON cr.id_venta = v.id_venta
                    WHERE cr.estado != 'Pagado' 
                    AND TRUNC(cr.FechaVencimiento) = TRUNC(SYSDATE + :dias)
                    ORDER BY cl.id_cliente";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    cmd.Parameters.Add(new OracleParameter("dias", dias));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal montoTotal = ConvertirDecimalOracle(reader.GetValue(3));
                            decimal totalAbonado = ConvertirDecimalOracle(reader.GetValue(5));
                            decimal saldoPendiente = montoTotal - totalAbonado;

                            creditos.Add(new CreditoNotificacion
                            {
                                ClienteId = reader.GetInt32(0),
                                NombreCliente = reader.GetString(1),
                                CreditoId = reader.GetInt32(2),
                                Monto = saldoPendiente,
                                FechaVencimiento = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener créditos por vencer: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return creditos;
        }

        public List<CreditoNotificacion> ObtenerVencimientosProximos()
        {
            var creditos = new List<CreditoNotificacion>();
            try
            {
                AbrirConexion();
                string query = @"
                    SELECT c.id_cliente, cl.nombre, cl.cedula, cr.id_credito, v.total, cr.FechaVencimiento,
                           NVL((SELECT SUM(a.monto_abonado) FROM ABONO a WHERE a.id_credito = cr.id_credito), 0) as totalAbonado
                    FROM CLIENTE cl
                    JOIN VENTA v ON v.id_cliente = cl.id_cliente
                    JOIN CREDITO cr ON cr.id_venta = v.id_venta
                    WHERE cr.estado != 'Pagado' 
                    AND cr.FechaVencimiento BETWEEN SYSDATE AND SYSDATE + 7
                    ORDER BY cr.FechaVencimiento";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal montoTotal = ConvertirDecimalOracle(reader.GetValue(4));
                            decimal totalAbonado = ConvertirDecimalOracle(reader.GetValue(6));
                            decimal saldoPendiente = montoTotal - totalAbonado;

                            creditos.Add(new CreditoNotificacion
                            {
                                ClienteId = reader.GetInt32(0),
                                NombreCliente = reader.GetString(1),
                                CreditoId = reader.GetInt32(3),
                                Monto = saldoPendiente,
                                FechaVencimiento = reader.GetDateTime(5)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener vencimientos próximos: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return creditos;
        }
    }
}
