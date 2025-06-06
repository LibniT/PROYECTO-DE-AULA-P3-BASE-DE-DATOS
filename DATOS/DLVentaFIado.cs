using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using ENTIDADES;
using DATOS;

namespace DATOS 
{
    public class DLVentaFiado : ConexionOracle
    {
        public List<VentaFiadoDTO> ObtenerVentasFiado()
        {
            List<VentaFiadoDTO> ventas = new List<VentaFiadoDTO>();
            try
            {
                AbrirConexion();
                string query = @"
                    SELECT 
                        c.id_cliente, 
                        c.nombre, 
                        v.id_venta, 
                        v.fecha, 
                        v.total, 
                        cr.FechaVencimiento
                    FROM CREDITO cr
                    JOIN VENTA v ON cr.id_venta = v.id_venta
                    JOIN CLIENTE c ON v.id_cliente = c.id_cliente";

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VentaFiadoDTO venta = new VentaFiadoDTO
                            {
                                ClienteId = reader.GetInt32(0),
                                NombreCliente = reader.GetString(1),
                                VentaId = reader.GetInt32(2),
                                FechaVenta = reader.GetDateTime(3),
                                Total = reader.GetDecimal(4),
                                FechaVencimiento = reader.GetDateTime(5)
                            };
                            ventas.Add(venta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener ventas fiado: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }
            return ventas;
        }
    }
}
