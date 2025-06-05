using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Mano.Entity
{
    public enum AuthenticationState
    {
        NotAuthenticated,
        SelectingUserType,
        WaitingForClientCedula,
        WaitingForAdminUsername,
        WaitingForAdminPassword,
        Authenticated
    }

    // Enumeraciones para el rol del usuario
    public enum UserRole
    {
        Cliente,
        Administrador
    }

    // Enumeraciones para el estado del menú
    public enum MenuState
    {
        Main,
        ComprasSubmenu,
        BuscandoFecha,
        BuscandoClientePorNombre,
        ViendoDetallesCliente,
        IngresandoVenta,
        EsperandoClienteIdVenta,
        EsperandoDescripcionVenta,
        EsperandoTotalVenta,
        EsperandoFechaVencimientoVenta,
        ConfirmandoVenta,
        RealizandoCompra,
        EsperandoDescripcionCompra,
        ConfirmandoCompra,
        EsperandoSubtotalAprobacion,
        EsperandoFechaVencimientoAprobacion
    }

    // Clase para almacenar el estado del usuario
    public class UserState
    {
        public AuthenticationState AuthState { get; set; } = AuthenticationState.NotAuthenticated;
        public UserRole UserRole { get; set; }
        public MenuState MenuState { get; set; } = MenuState.Main;

        // Datos del cliente
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCedula { get; set; }

        // Datos del administrador
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public string TempAdminUsername { get; set; }

        // Estados temporales para procesos
        public VentaEnProceso VentaActual { get; set; }
        public CompraEnProceso CompraActual { get; set; }
        public CompraEnProceso CompraAprobacion { get; set; }
    }

    // Clase para venta en proceso
    public class VentaEnProceso
    {
        public int ClienteId { get; set; }
        public string Descripcion { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }

    // Clase para compra en proceso
    public class CompraEnProceso
    {
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string Descripcion { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Domicilio { get; set; } = 5000; // Valor fijo de domicilio
        public decimal Total => SubTotal + Domicilio;
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
    }

    // Clase para notificaciones de crédito
    public class CreditoNotificacion
    {
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public int CreditoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }

    // Clase para clientes en mora
    public class ClienteMora
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal DeudaTotal { get; set; }
        public string Estado { get; set; }
        public int Strikes { get; set; }
    }
}
