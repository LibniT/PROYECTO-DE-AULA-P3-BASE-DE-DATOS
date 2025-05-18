using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ENTIDADES;
using LOGICA;

namespace PRUEBAS
{
    public class PruebasBaseDeDatos
    {
        private OracleService _context;

        public PruebasBaseDeDatos()
        {
            _context = new OracleService();
        }

        public void ProbarInserciones()
        {
            // Probar inserción de CLIENTE
            var cliente = new CLIENTE
            {
                Id = 1,
                Nombre = "Juan Pérez",
                Telefono = "123456789",
                Email = "juan.perez@example.com",
                DeudaTotal = 1000.00m,
                Direccion = "Calle Falsa 123",
                LimiteCredito = 5000.00m,
                Estado = "ACTIVO",
                Strikes = 0
            };
            _context.Clientes.Add(cliente);

            // Probar inserción de PRODUCTO
            var producto = new PRODUCTO
            {
                Id = 1,
                Nombre = "Producto A",
                Precio = 50.00m,
                CantidadStock = 100
            };
            _context.Productos.Add(producto);

            // Probar inserción de ADMINISTRADOR
            var administrador = new ADMINISTRADOR
            {
                Id = 1,
                Nombre = "Admin",
                Telefono = "987654321",
                Email = "admin@example.com",
                Usuario = "admin",
                Contraseña = "password"
            };
            _context.Administradores.Add(administrador);

            // Probar inserción de VENTA
            var venta = new VENTA
            {
                Id = 1,
                Fecha = DateTime.Now,
                cliente_id = cliente.Id,
                Total = 0 // Se calculará más tarde
            };
            _context.Ventas.Add(venta);

            // Probar inserción de CARRITO
            var carrito = new CARRITO
            {
                Id = 1,
                producto_id = producto.Id,
                Cantidad = 2,
                PrecioUnitario = producto.Precio,
                cliente_id = cliente.Id,
                venta_id = venta.Id
            };
            _context.Carritos.Add(carrito);

            // Probar inserción de EFECTIVO
            var efectivo = new EFECTIVO
            {
                Id = 1,
                Estado = "PAGADO",
                MetodoPago = "EFECTIVO",
                venta_id = venta.Id
            };
            _context.Efectivos.Add(efectivo);

            // Probar inserción de DEUDA
            var deuda = new DEUDA
            {
                Id = 1,
                Estado = "PENDIENTE",
                MetodoPago = "DEUDA",
                venta_id = venta.Id,
                FechaVencimiento = DateTime.Now.AddDays(30),
                Interes = 5.0m
            };
            _context.Deudas.Add(deuda);

            // Guardar cambios en la base de datos
            _context.SaveChanges();
            Console.WriteLine("Datos insertados correctamente en la base de datos.");
        }
    }
}
