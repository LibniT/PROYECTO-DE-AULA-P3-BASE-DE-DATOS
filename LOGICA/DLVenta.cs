using System;
using System.Collections.Generic;
using ENTIDADES;
using Oracle.ManagedDataAccess.Client;

namespace DATOS
{
    public class DLVenta : ConexionOracle // Hereda de ConexionOracle
    {
        public List<VentaFiadoDTO> ObtenerVentasFiado()
        {
            List<VentaFiadoDTO> ventas = new List<VentaFiadoDTO>();

            try
            {
                AbrirConexion(); // Llama directamente al método de la clase base
                using (var command = new OracleCommand("SELECT v.id_venta, v.fecha, v.total, c.id_cliente, c.nombre, cr.FechaVencimiento FROM VENTA v JOIN CLIENTE c ON v.id_cliente = c.id_cliente JOIN CREDITO cr ON v.id_venta = cr.id_venta WHERE cr.estado = 'Fiado'", ObtenerConexion()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VentaFiadoDTO venta = new VentaFiadoDTO
                            {
                                VentaId = reader.GetInt32(0), // ID de la venta
                                FechaVenta = reader.GetDateTime(1), // Fecha de la venta
                                Total = reader.GetDecimal(2), // Total de la venta
                                ClienteId = reader.GetInt32(3), // ID del cliente
                                NombreCliente = reader.GetString(4), // Nombre del cliente
                                FechaVencimiento = reader.GetDateTime(5) // Fecha de vencimiento
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
                CerrarConexion(); // Llama directamente al método de la clase base
            }

            return ventas; // Devolver la lista de ventas
        }
    }
}
