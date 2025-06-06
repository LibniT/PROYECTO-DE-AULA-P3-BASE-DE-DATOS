using System;
using System.Collections.Generic;
using System.Linq;
using A_Mano.DAl;
using A_Mano.Entity;

namespace BLL
{
    public class ClienteBLL
    {
        private readonly ClienteDAL _clienteDAL;
        private readonly CreditoDAL _creditoDAL;
        private readonly VentaDAL _ventaDAL;

        public ClienteBLL()
        {
            _clienteDAL = new ClienteDAL();
            _creditoDAL = new CreditoDAL();
            _ventaDAL = new VentaDAL();
        }

        public ClienteEntity AutenticarCliente(string cedula)
        {
            if (!ValidarCedula(cedula))
                throw new ArgumentException("Formato de cédula inválido. La cédula debe contener solo números y tener mínimo 8 dígitos.");

            var cliente = _clienteDAL.BuscarPorCedula(cedula);
            if (cliente == null)
                throw new Exception("No se encontró ningún cliente con esa cédula.");

            return cliente;
        }

        public string GenerarResumenCuenta(int clienteId)
        {
            var cliente = _clienteDAL.ObtenerPorId(clienteId);
            if (cliente == null)
                return "Cliente no encontrado";

            var creditos = _creditoDAL.ObtenerPendientesPorCliente(clienteId);
            int creditosPendientes = creditos.Count;
            decimal disponible = cliente.LimiteCredito - cliente.DeudaTotal;

            string estadoEmoji = ObtenerEmojiEstado(cliente.Estado);

            return $"📊 *RESUMEN DE CUENTA* 📊\n\n" +
                   $"👤 *Cliente:* {cliente.Nombre}\n" +
                   $"🆔 *Cédula:* {cliente.Cedula}\n" +
                   $"💵 *Deuda Total:* ${cliente.DeudaTotal:N2}\n" +
                   $"💳 *Límite de Crédito:* ${cliente.LimiteCredito:N2}\n" +
                   $"✅ *Disponible:* ${disponible:N2}\n" +
                   $"🔄 *Estado:* {estadoEmoji} {cliente.Estado}\n" +
                   $"⚠️ *Strikes:* {cliente.Strikes}/3\n" +
                   $"📝 *Créditos Pendientes:* {creditosPendientes}";
        }

        public string GenerarListaCreditos(int clienteId)
        {
            var creditos = _creditoDAL.ObtenerPendientesPorCliente(clienteId);

            if (!creditos.Any())
                return "No tienes créditos pendientes. ¡Felicidades! 🎉";

            string resultado = "📝 *CRÉDITOS PENDIENTES* \n";
            decimal totalDeuda = 0;

            foreach (var credito in creditos)
            {
                int diasRestantes = (credito.FechaVencimiento - DateTime.Now).Days;
                string estadoVencimiento = diasRestantes < 0 ? "❗VENCIDO❗" :
                                         diasRestantes < 5 ? "⚠️ Próximo a vencer" : "✅ Al día";

                resultado += $"*Crédito #{credito.IdCredito}*\n" +
                           $"💵 Monto Total: ${credito.MontoTotal:N2}\n" +
                           $"💰 Abonado: ${credito.TotalAbonado:N2}\n" +
                           $"💸 Saldo: ${credito.SaldoPendiente:N2}\n" +
                           $"📅 Vence: {credito.FechaVencimiento:dd/MM/yyyy}\n" +
                           $"📊 Estado: {estadoVencimiento}\n\n";

                totalDeuda += credito.SaldoPendiente;
            }

            resultado += $"*TOTAL ADEUDADO: ${totalDeuda:N2}*";
            return resultado;
        }

        public string GenerarHistorialCompras(int clienteId)
        {
            var ventas = _ventaDAL.ObtenerPorCliente(clienteId);

            if (!ventas.Any())
                return "No tienes compras registradas.";

            string resultado = "💰 *COMPRAS POR MONTO* \n";

            foreach (var venta in ventas)
            {
                resultado += $"*Compra #{venta.IdVenta}*\n" +
                           $"📅 Fecha: {venta.Fecha:dd/MM/yyyy}\n" +
                           $"📝 Descripción: {venta.Descripcion}\n" +
                           $"💵 Total: ${venta.Total:N2}\n\n";
            }

            return resultado;
        }

        public string GenerarComprasPorFecha(int clienteId, DateTime fecha)
        {
            var ventas = _ventaDAL.ObtenerPorClienteYFecha(clienteId, fecha);

            if (!ventas.Any())
                return $"No se encontraron compras para la fecha {fecha:dd/MM/yyyy}.";

            string resultado = $"📅 *COMPRAS DEL {fecha:dd/MM/yyyy}* \n";
            decimal totalDia = 0;

            foreach (var venta in ventas)
            {
                resultado += $"*Compra #{venta.IdVenta}*\n" +
                           $"⏰ Hora: {venta.Fecha:HH:mm}\n" +
                           $"📝 Descripción: {venta.Descripcion}\n" +
                           $"💵 Total: ${venta.Total:N2}\n\n";

                totalDia += venta.Total;
            }

            resultado += $"*TOTAL DEL DÍA: ${totalDia:N2}*";
            return resultado;
        }

        public string GenerarProximosVencimientos(int clienteId)
        {
            var creditos = _creditoDAL.ObtenerPendientesPorCliente(clienteId);

            if (!creditos.Any())
                return "No tienes próximos vencimientos.";

            string resultado = "📆 *PRÓXIMOS VENCIMIENTOS* \n";

            foreach (var credito in creditos)
            {
                int diasRestantes = (credito.FechaVencimiento - DateTime.Now).Days;
                string indicadorDias = diasRestantes < 0 ? "❗VENCIDO HACE" : "FALTAN";
                diasRestantes = Math.Abs(diasRestantes);

                resultado += $"*Crédito #{credito.IdCredito}*\n" +
                           $"💵 Saldo: ${credito.SaldoPendiente:N2}\n" +
                           $"📅 Vence: {credito.FechaVencimiento:dd/MM/yyyy}\n" +
                           $"⏱️ {indicadorDias} {diasRestantes} DÍAS\n\n";
            }

            return resultado;
        }

        private bool ValidarCedula(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
                return false;

            cedula = cedula.Trim();

            if (!cedula.All(char.IsDigit))
                return false;

            if (cedula.Length < 8)
                return false;

            return true;
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
