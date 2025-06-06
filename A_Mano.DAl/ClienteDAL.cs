using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using A_Mano.Entity;

namespace A_Mano.DAl
{
    public class ClienteDAL : ConexionOracle
    {
        public ClienteEntity BuscarPorCedula(string cedula)
        {
            ClienteEntity cliente = null;
            try
            {
                AbrirConexion();
                string query = "SELECT id_cliente, nombre, cedula, estado, strikes, deudaTotal, limiteCredito FROM CLIENTE WHERE cedula = :cedula";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    cmd.Parameters.Add(new OracleParameter("cedula", cedula));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new ClienteEntity
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Cedula = reader.GetString(2),
                                Estado = reader.GetString(3),
                                Strikes = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                DeudaTotal = ConvertirDecimalOracle(reader.GetValue(5)),
                                LimiteCredito = ConvertirDecimalOracle(reader.GetValue(6))
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar cliente por cédula: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return cliente;
        }

        public ClienteEntity ObtenerPorId(int clienteId)
        {
            ClienteEntity cliente = null;
            try
            {
                AbrirConexion();
                string query = "SELECT id_cliente, nombre, cedula, estado, strikes, deudaTotal, limiteCredito FROM CLIENTE WHERE id_cliente = :clienteId";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    cmd.Parameters.Add(new OracleParameter("clienteId", clienteId));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new ClienteEntity
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Cedula = reader.GetString(2),
                                Estado = reader.GetString(3),
                                Strikes = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                                DeudaTotal = ConvertirDecimalOracle(reader.GetValue(5)),
                                LimiteCredito = ConvertirDecimalOracle(reader.GetValue(6))
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener cliente: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return cliente;
        }

        public List<ClienteEntity> ObtenerTodos()
        {
            var clientes = new List<ClienteEntity>();
            try
            {
                AbrirConexion();
                string query = @"
                    SELECT id_cliente, nombre, cedula, deudaTotal, limiteCredito, estado, strikes
                    FROM CLIENTE
                    ORDER BY id_cliente";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new ClienteEntity
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Cedula = reader.GetString(2),
                                DeudaTotal = ConvertirDecimalOracle(reader.GetValue(3)),
                                LimiteCredito = ConvertirDecimalOracle(reader.GetValue(4)),
                                Estado = reader.GetString(5),
                                Strikes = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener lista de clientes: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return clientes;
        }

        public List<ClienteEntity> BuscarPorNombre(string nombre)
        {
            var clientes = new List<ClienteEntity>();
            try
            {
                AbrirConexion();
                string query = @"
                    SELECT id_cliente, nombre, cedula, deudaTotal, limiteCredito, estado, strikes
                    FROM CLIENTE
                    WHERE UPPER(nombre) LIKE UPPER(:nombre)
                    ORDER BY id_cliente";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    cmd.Parameters.Add(new OracleParameter("nombre", $"%{nombre}%"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new ClienteEntity
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Cedula = reader.GetString(2),
                                DeudaTotal = ConvertirDecimalOracle(reader.GetValue(3)),
                                LimiteCredito = ConvertirDecimalOracle(reader.GetValue(4)),
                                Estado = reader.GetString(5),
                                Strikes = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar clientes por nombre: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return clientes;
        }

        public bool Existe(int clienteId)
        {
            bool existe = false;
            try
            {
                AbrirConexion();
                string query = "SELECT COUNT(*) FROM CLIENTE WHERE id_cliente = :clienteId";
                using (var cmd = new OracleCommand(query, conexion))
                {
                    cmd.Parameters.Add(new OracleParameter("clienteId", clienteId));
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    existe = count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar existencia de cliente: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return existe;
        }

        public List<ClienteEntity> ObtenerClientesEnMora()
        {
            var clientes = new List<ClienteEntity>();
            try
            {
                AbrirConexion();
                string query = @"
                    SELECT DISTINCT c.id_cliente, c.nombre, c.cedula, c.deudaTotal, c.estado, c.strikes,
                           (SELECT MIN(cr.FechaVencimiento) 
                            FROM CREDITO cr 
                            JOIN VENTA v ON cr.id_venta = v.id_venta 
                            WHERE v.id_cliente = c.id_cliente AND cr.estado != 'Pagado' AND cr.FechaVencimiento < SYSDATE) as primeraFechaVencida
                    FROM CLIENTE c
                    WHERE EXISTS (
                        SELECT 1 FROM CREDITO cr 
                        JOIN VENTA v ON cr.id_venta = v.id_venta 
                        WHERE v.id_cliente = c.id_cliente AND cr.estado != 'Pagado' AND cr.FechaVencimiento < SYSDATE
                    )
                    ORDER BY primeraFechaVencida";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new ClienteEntity
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Cedula = reader.GetString(2),
                                DeudaTotal = ConvertirDecimalOracle(reader.GetValue(3)),
                                Estado = reader.GetString(4),
                                Strikes = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener clientes en mora: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return clientes;
        }

        public List<ClienteEntity> ObtenerClientesAlDia()
        {
            var clientes = new List<ClienteEntity>();
            try
            {
                AbrirConexion();
                string query = @"
                    SELECT c.id_cliente, c.nombre, c.cedula, c.deudaTotal, c.limiteCredito, c.estado, c.strikes
                    FROM CLIENTE c
                    WHERE NOT EXISTS (
                        SELECT 1 FROM CREDITO cr 
                        JOIN VENTA v ON cr.id_venta = v.id_venta 
                        WHERE v.id_cliente = c.id_cliente AND cr.estado != 'Pagado' AND cr.FechaVencimiento < SYSDATE
                    )
                    AND c.deudaTotal > 0
                    ORDER BY c.deudaTotal DESC";

                using (var cmd = new OracleCommand(query, conexion))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new ClienteEntity
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Cedula = reader.GetString(2),
                                DeudaTotal = ConvertirDecimalOracle(reader.GetValue(3)),
                                LimiteCredito = ConvertirDecimalOracle(reader.GetValue(4)),
                                Estado = reader.GetString(5),
                                Strikes = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener clientes al día: {ex.Message}");
            }
            finally
            {
                CerrarConexion();
            }
            return clientes;
        }
    }
}
