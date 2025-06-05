using System;
using System.Collections.Generic;
using ENTIDADES;
using Oracle.ManagedDataAccess.Client;

namespace DATOS
{
    public class DLVenta_ : ConexionOracle
    {
        private ConexionOracle conexionOracle;

        public DLVenta_()
        {
            conexionOracle = new ConexionOracle();
        }

        public List<VentaDTO> ObtenerVentas()
        {
            List<VentaDTO> ventas = new List<VentaDTO>();

            try
            {
                conexionOracle.AbrirConexion();
                using (var command = new OracleCommand("SELECT v.id_venta, v.fecha, v.total, c.id_cliente, c.nombre FROM VENTA v JOIN CLIENTE c ON v.id_cliente = c.id_cliente", conexionOracle.ObtenerConexion()))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VentaDTO venta = new VentaDTO
                            {
                                VentaId = reader.GetInt32(0), // ID de la venta
                                FechaVenta = reader.GetDateTime(1), // Fecha de la venta
                                Total = reader.GetDecimal(2), // Total de la venta
                                ClienteId = reader.GetInt32(3), // ID del cliente
                                NombreCliente = reader.GetString(4) // Nombre del cliente
                            };
                            ventas.Add(venta);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Error al obtener ventas: " + ex.Message);
            }
            finally
            {
                conexionOracle.CerrarConexion();
            }

            return ventas;
        }
    }
}
