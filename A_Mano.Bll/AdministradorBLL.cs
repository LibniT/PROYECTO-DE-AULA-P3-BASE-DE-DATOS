using System;
using System.Collections.Generic;
using System.Linq;
using A_Mano.DAl;
using A_Mano.Entity;


namespace BLL
{
    public class AdministradorBLL
    {
        private readonly AdministradorDAL _adminDAL;
        private readonly ClienteDAL _clienteDAL;
        private readonly VentaDAL _ventaDAL;
        private readonly CreditoDAL _creditoDAL;

        public AdministradorBLL()
        {
            _adminDAL = new AdministradorDAL();
            _clienteDAL = new ClienteDAL();
            _ventaDAL = new VentaDAL();
            _creditoDAL = new CreditoDAL();
        }

        public AdministradorEntity AutenticarAdministrador(string usuario, string contraseña = null)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("Usuario no puede estar vacío");

            var admin = _adminDAL.VerificarCredenciales(usuario, contraseña);
            if (admin == null)
            {
                if (contraseña == null)
                    throw new Exception("Usuario no encontrado");
                else
                    throw new Exception("Credenciales incorrectas");
            }

            return admin;
        }

        public int RegistrarVenta(VentaEnProceso ventaData, DateTime fechaVencimiento)
        {
            // Validar que el cliente existe y no esté bloqueado
            var cliente = _clienteDAL.ObtenerPorId(ventaData.ClienteId);
            if (cliente == null)
                throw new Exception("Cliente no encontrado");

            if (cliente.Estado.ToUpper() == "BLOQUEADO")
                throw new Exception("No se pueden registrar ventas para clientes bloqueados");

            // Crear entidad de venta
            var venta = new VentaEntity
            {
                Descripcion = ventaData.Descripcion,
                IdCliente = ventaData.ClienteId,
                Total = ventaData.Total,
                Fecha = DateTime.Now
            };

            // Insertar venta
            int ventaId = _ventaDAL.Insertar(venta);

            // Crear crédito asociado
            var credito = new CreditoEntity
            {
                Estado = "Pendiente",
                IdVenta = ventaId,
                FechaVencimiento = fechaVencimiento
            };

            _creditoDAL.Insertar(credito);

            return ventaId;
        }

        public int AprobarCompra(CompraEnProceso compra, DateTime fechaVencimiento, string nombreAdministrador)
        {
            // Validar que el cliente existe y no esté bloqueado
            var cliente = _clienteDAL.ObtenerPorId(compra.ClienteId);
            if (cliente == null)
                throw new Exception("Cliente no encontrado");

            if (cliente.Estado.ToUpper() == "BLOQUEADO")
                throw new Exception("No se pueden aprobar compras para clientes bloqueados");

            // Crear entidad de venta
            var venta = new VentaEntity
            {
                Descripcion = $"{compra.Descripcion}\n(Aprobado por: {nombreAdministrador})",
                IdCliente = compra.ClienteId,
                Total = compra.Total,
                Fecha = DateTime.Now
            };

            // Insertar venta
            int ventaId = _ventaDAL.Insertar(venta);

            // Crear crédito asociado
            var credito = new CreditoEntity
            {
                Estado = "Pendiente",
                IdVenta = ventaId,
                FechaVencimiento = fechaVencimiento
            };

            _creditoDAL.Insertar(credito);

            return ventaId;
        }


        public string GenerarListaClientes()
        {
            var clientes = _clienteDAL.ObtenerTodos();

            if (!clientes.Any())
                return "No hay clientes registrados.";

            string resultado = "📋 *LISTA DE CLIENTES* \n";
            int contador = 0;

            foreach (var cliente in clientes)
            {
                string estadoEmoji = ObtenerEmojiEstado(cliente.Estado);

                resultado += $"*Cliente #{cliente.Id}*\n" +
                           $"👤 Nombre: {cliente.Nombre}\n" +
                           $"🆔 Cédula: {cliente.Cedula}\n" +
                           $"💵 Deuda: ${cliente.DeudaTotal:N2}\n" +
                           $"💳 Límite: ${cliente.LimiteCredito:N2}\n" +
                           $"🔄 Estado: {estadoEmoji} {cliente.Estado}\n" +
                           $"⚠️ Strikes: {cliente.Strikes}/3\n\n";

                contador++;

                // Para evitar mensajes demasiado largos
                if (contador % 8 == 0)
                {
                    resultado += "Continúa en el siguiente mensaje...";
                    break;
                }
            }

            return resultado;
        }

        public string BuscarClientesPorNombre(string nombre)
        {
            var clientes = _clienteDAL.BuscarPorNombre(nombre);

            if (!clientes.Any())
                return $"No se encontraron clientes con el nombre '{nombre}'.";

            string resultado = $"🔍 *RESULTADOS DE BÚSQUEDA: '{nombre}'* \n";

            foreach (var cliente in clientes)
            {
                string estadoEmoji = ObtenerEmojiEstado(cliente.Estado);

                resultado += $"*Cliente #{cliente.Id}*\n" +
                           $"👤 Nombre: {cliente.Nombre}\n" +
                           $"🆔 Cédula: {cliente.Cedula}\n" +
                           $"💵 Deuda: ${cliente.DeudaTotal:N2}\n" +
                           $"💳 Límite: ${cliente.LimiteCredito:N2}\n" +
                           $"🔄 Estado: {estadoEmoji} {cliente.Estado}\n" +
                           $"⚠️ Strikes: {cliente.Strikes}/3\n\n";
            }

            return resultado;
        }

        public string GenerarListaClientesEnMora()
        {
            var clientes = _clienteDAL.ObtenerClientesEnMora();

            if (!clientes.Any())
                return "No hay clientes en mora actualmente.";

            string resultado = "🚨 *CLIENTES EN MORA* \n";

            foreach (var cliente in clientes)
            {
                string estadoEmoji = ObtenerEmojiEstado(cliente.Estado);

                resultado += $"*Cliente #{cliente.Id}*\n" +
                           $"👤 Nombre: {cliente.Nombre}\n" +
                           $"🆔 Cédula: {cliente.Cedula}\n" +
                           $"💵 Deuda Total: ${cliente.DeudaTotal:N2}\n" +
                           $"🔄 Estado: {estadoEmoji} {cliente.Estado}\n" +
                           $"⚠️ Strikes: {cliente.Strikes}/3\n\n";
            }

            return resultado;
        }

        public string GenerarListaClientesAlDia()
        {
            var clientes = _clienteDAL.ObtenerClientesAlDia();

            if (!clientes.Any())
                return "No hay clientes con deudas al día actualmente.";

            string resultado = "✅ *CLIENTES AL DÍA* \n";

            foreach (var cliente in clientes)
            {
                decimal porcentajeUtilizado = cliente.LimiteCredito > 0 ? (cliente.DeudaTotal / cliente.LimiteCredito) * 100 : 0;
                string estadoEmoji = ObtenerEmojiEstado(cliente.Estado);

                resultado += $"*Cliente #{cliente.Id}*\n" +
                           $"👤 Nombre: {cliente.Nombre}\n" +
                           $"🆔 Cédula: {cliente.Cedula}\n" +
                           $"💵 Deuda Total: ${cliente.DeudaTotal:N2}\n" +
                           $"💳 Límite: ${cliente.LimiteCredito:N2}\n" +
                           $"🔄 Estado: {estadoEmoji} {cliente.Estado}\n" +
                           $"⚠️ Strikes: {cliente.Strikes}/3\n" +
                           $"📊 Utilizado: {porcentajeUtilizado:N1}%\n\n";
            }

            return resultado;
        }

        public string GenerarVencimientosProximos()
        {
            var vencimientos = _creditoDAL.ObtenerVencimientosProximos();

            if (!vencimientos.Any())
                return "No hay vencimientos próximos en los siguientes 7 días.";

            string resultado = "📆 *VENCIMIENTOS PRÓXIMOS (7 DÍAS)* \n";

            foreach (var vencimiento in vencimientos)
            {
                int diasRestantes = (vencimiento.FechaVencimiento - DateTime.Now).Days;

                resultado += $"*Crédito #{vencimiento.CreditoId}*\n" +
                           $"👤 Cliente: {vencimiento.NombreCliente} (ID: {vencimiento.ClienteId})\n" +
                           $"💵 Saldo: ${vencimiento.Monto:N2}\n" +
                           $"📅 Vence: {vencimiento.FechaVencimiento:dd/MM/yyyy}\n" +
                           $"⏱️ Faltan: {diasRestantes} días\n\n";
            }

            return resultado;
        }

        public List<CreditoNotificacion> ObtenerNotificacionesVencimientos()
        {
            var notificaciones = new List<CreditoNotificacion>();

            // Créditos que vencen en 2 días
            notificaciones.AddRange(_creditoDAL.ObtenerPorVencer(2));

            // Créditos que vencen hoy
            notificaciones.AddRange(_creditoDAL.ObtenerPorVencer(0));

            return notificaciones;
        }

        private string ObtenerEmojiEstado(string estado)
        {
            switch (estado.ToUpper())
            {
                case "ACTIVO":
                    return "✅";
                case "RIESGO":
                    return "⚠️";
                case "BLOQUEADO":
                    return "🚫";
                default:
                    return "❓";
            }
        }

    }
}
