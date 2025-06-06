//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DAL;
//using ENTIDADES;

//namespace pruebas
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            DBAdministrador dbAdmin = new DBAdministrador();
//            DBCliente dbCliente = new DBCliente();
//            DBProducto dbProducto = new DBProducto();
//            DBVenta dbVenta = new DBVenta();
//            DBEfectivo dbEfectivo = new DBEfectivo();
//            DBDeuda dbDeuda = new DBDeuda();

//            while (true)
//            {
//                Console.WriteLine("Seleccione una opción:");
//                Console.WriteLine("1. Agregar Administrador");
//                Console.WriteLine("2. Listar Administradores");
//                Console.WriteLine("3. Agregar Cliente");
//                Console.WriteLine("4. Listar Clientes");
//                Console.WriteLine("5. Agregar Producto");
//                Console.WriteLine("6. Listar Productos");
//                Console.WriteLine("7. Salir");
//                Console.Write("Opción: ");
//                string option = Console.ReadLine();

//                switch (option)
//                {
//                    case "1":
//                        // Agregar Administrador
//                        var admin = new ADMINISTRADOR
//                        {
//                            Id = 1, // Cambia esto según tu lógica
//                            Nombre = "Admin Test",
//                            Telefono = "123456789",
//                            Email = "admin@test.com",
//                            Usuario = "admin",
//                            Contraseña = "password"
//                        };
//                        Console.WriteLine(dbAdmin.SaveData(admin));
//                        break;

//                    case "2":
//                        // Listar Administradores
//                        var admins = dbAdmin.GetAll();
//                        foreach (var a in admins)
//                        {
//                            Console.WriteLine($"ID: {a.Id}, Nombre: {a.Nombre}, Telefono: {a.Telefono}, Email: {a.Email}");
//                        }
//                        break;

//                    case "3":
//                        // Agregar Cliente
//                        var cliente = new CLIENTE
//                        {
//                            Id = 1, // Cambia esto según tu lógica
//                            Nombre = "Cliente Test",
//                            Telefono = "987654321",
//                            Email = "cliente@test.com",
//                            DeudaTotal = 0,
//                            Direccion = "Calle Falsa 123",
//                            LimiteCredito = 1000,
//                            Estado = "ACTIVO",
//                            Strikes = 0
//                        };
//                        Console.WriteLine(dbCliente.SaveData(cliente));
//                        break;

//                    case "4":
//                        // Listar Clientes
//                        var clientes = dbCliente.GetAll();
//                        foreach (var c in clientes)
//                        {
//                            Console.WriteLine($"ID: {c.Id}, Nombre: {c.Nombre}, Telefono: {c.Telefono}, Email: {c.Email}");
//                        }
//                        break;

//                    case "5":
//                        // Agregar Producto
//                        var producto = new PRODUCTO
//                        {
//                            Id = 1, // Cambia esto según tu lógica
//                            Nombre = "Producto Test",
//                            Precio = 100,
//                            CantidadStock = 50
//                        };
//                        Console.WriteLine(dbProducto.SaveData(producto));
//                        break;

//                    case "6":
//                        // Listar Productos
//                        var productos = dbProducto.GetAll();
//                        foreach (var p in productos)
//                        {
//                            Console.WriteLine($"ID: {p.Id}, Nombre: {p.Nombre}, Precio: {p.Precio}, Cantidad Stock: {p.CantidadStock}");
//                        }
//                        break;

//                    case "7":
//                        // Salir
//                        return;

//                    default:
//                        Console.WriteLine("Opción no válida.");
//                        break;
//                }
//            }
//        }
//    }

//}
