using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using A_Mano.DAl;
using A_Mano.Entity;

namespace BLL
{
    public class BotChatBLL
    {
        private readonly string _botToken;
        private TelegramBotClient _botClient;
        private System.Timers.Timer _notificationTimer;

        // Servicios de negocio
        private readonly ClienteBLL _clienteBLL;
        private readonly AdministradorBLL _adminBLL;

        // Estados y datos en memoria
        private static Dictionary<long, UserState> _userStates = new Dictionary<long, UserState>();
        private static Dictionary<string, CompraEnProceso> _solicitudesCompra = new Dictionary<string, CompraEnProceso>();
        private List<long> _adminChatIds = new List<long>();

        public BotChatBLL(string botToken)
        {
            _botToken = botToken;
            _clienteBLL = new ClienteBLL();
            _adminBLL = new AdministradorBLL();

            InicializarTimer();
        }

        public async Task InicializarBot()
        {
            _botClient = new TelegramBotClient(_botToken);
            await IniciarReceptor();
        }

        private void InicializarTimer()
        {
            _notificationTimer = new System.Timers.Timer();
            _notificationTimer.Interval = 60 * 60 * 1000; // Revisar cada hora
            _notificationTimer.Elapsed += NotificationTimer_Tick;
            _notificationTimer.Start();
        }

        private async void NotificationTimer_Tick(object sender, EventArgs e)
        {
            // Solo enviar notificaciones a las 9:00 AM
            if (DateTime.Now.Hour == 9)
            {
                await EnviarNotificacionesVencimientos();
            }
        }

        private async Task IniciarReceptor()
        {
            var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<Telegram.Bot.Types.Enums.UpdateType>()
            };

            _botClient.StartReceiving(OnMessage, HandleError, receiverOptions, cancellationToken: cts.Token);
            AddLog("Receptor de mensajes iniciado.");
        }

        public async Task OnMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message != null)
            {
                var message = update.Message;
                var chatId = message.Chat.Id;
                string texto = message.Text?.ToLower() ?? "";

                AddLog($"Mensaje recibido de {message.Chat.FirstName} ({chatId}): {texto}");

                // Verificar si el usuario ya está en nuestro diccionario
                if (!_userStates.ContainsKey(chatId))
                {
                    await InicializarNuevoUsuario(chatId);
                    return;
                }

                var userState = _userStates[chatId];

                // Verificar si el usuario quiere salir
                if (texto == "❌ salir" || texto == "❌ cerrar sesión")
                {
                    await CerrarSesion(chatId);
                    return;
                }

                // Procesar según el estado de autenticación
                if (userState.AuthState != AuthenticationState.Authenticated)
                {
                    await ProcesarAutenticacion(chatId, texto, userState);
                }
                else
                {
                    await ProcesarComandosAutenticados(chatId, texto, userState);
                }
            }
        }

        private async Task InicializarNuevoUsuario(long chatId)
        {
            _userStates[chatId] = new UserState { AuthState = AuthenticationState.NotAuthenticated };

            await _botClient.SendTextMessageAsync(chatId,
                "¡Bienvenido al sistema A MANO! 💸\n" +
                "Por favor, selecciona tu tipo de usuario:");

            await MostrarSeleccionTipoUsuario(chatId);
        }

        private async Task MostrarSeleccionTipoUsuario(long chatId)
        {
            var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("👤 Cliente"), new KeyboardButton("🔑 Administrador") }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Selecciona tu tipo de usuario:",
                replyMarkup: keyboard
            );

            if (!_userStates.ContainsKey(chatId))
            {
                _userStates[chatId] = new UserState();
            }

            _userStates[chatId].AuthState = AuthenticationState.SelectingUserType;
        }

        private async Task ProcesarAutenticacion(long chatId, string mensaje, UserState userState)
        {
            try
            {
                switch (userState.AuthState)
                {
                    case AuthenticationState.SelectingUserType:
                        await ProcesarSeleccionTipoUsuario(chatId, mensaje, userState);
                        break;

                    case AuthenticationState.WaitingForClientCedula:
                        await ProcesarCedulaCliente(chatId, mensaje, userState);
                        break;

                    case AuthenticationState.WaitingForAdminUsername:
                        await ProcesarUsuarioAdmin(chatId, mensaje, userState);
                        break;

                    case AuthenticationState.WaitingForAdminPassword:
                        await ProcesarContraseñaAdmin(chatId, mensaje, userState);
                        break;
                }
            }
            catch (Exception ex)
            {
                await _botClient.SendTextMessageAsync(chatId, $"Error en autenticación: {ex.Message}");
                AddLog($"Error en autenticación: {ex.Message}");
            }
        }

        private async Task ProcesarSeleccionTipoUsuario(long chatId, string mensaje, UserState userState)
        {
            if (mensaje.Contains("cliente"))
            {
                userState.UserRole = UserRole.Cliente;
                userState.AuthState = AuthenticationState.WaitingForClientCedula;

                var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("❌ Salir") }
                })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = false
                };

                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Por favor, ingresa tu número de cédula:\n" +
                          "(Ejemplo: 12345678 o 1234567890)",
                    replyMarkup: keyboard
                );
            }
            else if (mensaje.Contains("administrador"))
            {
                userState.UserRole = UserRole.Administrador;
                userState.AuthState = AuthenticationState.WaitingForAdminUsername;

                var keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new[] { new KeyboardButton("❌ Salir") }
                })
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = false
                };

                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Por favor, ingresa tu nombre de usuario de administrador:",
                    replyMarkup: keyboard
                );
            }
            else
            {
                await _botClient.SendTextMessageAsync(chatId, "Por favor, selecciona una opción válida:");
                await MostrarSeleccionTipoUsuario(chatId);
            }
        }

        private async Task ProcesarCedulaCliente(long chatId, string cedula, UserState userState)
        {
            try
            {
                var cliente = _clienteBLL.AutenticarCliente(cedula);

                if (cliente.Estado.ToUpper() == "BLOQUEADO")
                {
                    await _botClient.SendTextMessageAsync(chatId,
                        "❌ Tu cuenta está bloqueada debido a pagos vencidos.\n" +
                        "Contacta a la tienda para regularizar tu situación.");
                    await MostrarSeleccionTipoUsuario(chatId);
                    return;
                }

                // Autenticación exitosa
                userState.ClientId = cliente.Id;
                userState.ClientName = cliente.Nombre;
                userState.ClientCedula = cedula;
                userState.AuthState = AuthenticationState.Authenticated;

                string estadoEmoji;
switch (cliente.Estado.ToUpper())
{
    case "ACTIVO":
        estadoEmoji = "✅";
        break;
    case "RIESGO":
        estadoEmoji = "⚠️";
        break;
    default:
        estadoEmoji = "❓";
        break;
}


                await _botClient.SendTextMessageAsync(chatId,
                    $"¡Autenticación exitosa! Bienvenido {cliente.Nombre}.\n" +
                    $"Cédula: {cedula}\n" +
                    $"Estado: {estadoEmoji} {cliente.Estado}");

                await MostrarMenuPrincipalCliente(chatId);
            }
            catch (Exception ex)
            {
                await _botClient.SendTextMessageAsync(chatId,
                    $"❌ {ex.Message}\n" +
                    "Verifica el número e intenta nuevamente:");
            }
        }

        private async Task ProcesarUsuarioAdmin(long chatId, string usuario, UserState userState)
        {
            try
            {
                var admin = _adminBLL.AutenticarAdministrador(usuario);
                if (admin != null)
                {
                    userState.TempAdminUsername = usuario;
                    userState.AuthState = AuthenticationState.WaitingForAdminPassword;
                    await _botClient.SendTextMessageAsync(chatId, "Por favor, ingresa tu contraseña:");
                }
                else
                {
                    await _botClient.SendTextMessageAsync(chatId, "Usuario no encontrado. Por favor, intenta nuevamente:");
                }
            }
            catch (Exception ex)
            {
                await _botClient.SendTextMessageAsync(chatId, $"Error: {ex.Message}");
            }
        }

        private async Task ProcesarContraseñaAdmin(long chatId, string contraseña, UserState userState)
        {
            try
            {
                var admin = _adminBLL.AutenticarAdministrador(userState.TempAdminUsername, contraseña);
                if (admin != null)
                {
                    userState.AuthState = AuthenticationState.Authenticated;
                    userState.AdminId = admin.IdAdmin;
                    userState.AdminName = admin.Nombre;

                    if (!_adminChatIds.Contains(chatId))
                    {
                        _adminChatIds.Add(chatId);
                    }

                    await _botClient.SendTextMessageAsync(chatId, $"¡Autenticación exitosa! Bienvenido {admin.Nombre}.");
                    await MostrarMenuPrincipalAdministrador(chatId);
                }
                else
                {
                    await _botClient.SendTextMessageAsync(chatId, "Contraseña incorrecta. Por favor, intenta nuevamente:");
                }
            }
            catch (Exception ex)
            {
                await _botClient.SendTextMessageAsync(chatId, $"Error: {ex.Message}");
            }
        }

        private async Task ProcesarComandosAutenticados(long chatId, string comando, UserState userState)
        {
            try
            {
                if (userState.UserRole == UserRole.Cliente)
                {
                    await ProcesarComandoCliente(chatId, comando, userState);
                }
                else if (userState.UserRole == UserRole.Administrador)
                {
                    await ProcesarComandoAdministrador(chatId, comando, userState);
                }
            }
            catch (Exception ex)
            {
                await _botClient.SendTextMessageAsync(chatId, $"Error al procesar comando: {ex.Message}");
                AddLog($"Error al procesar comando: {ex.Message}");
            }
        }

        private async Task ProcesarComandoCliente(long chatId, string comando, UserState userState)
        {
            switch (comando)
            {
                case "1️⃣ resumen de cuenta":
                    string resumen = _clienteBLL.GenerarResumenCuenta(userState.ClientId);
                    await _botClient.SendTextMessageAsync(chatId, resumen, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    break;

                case "2️⃣ mis créditos pendientes":
                    string creditos = _clienteBLL.GenerarListaCreditos(userState.ClientId);
                    await _botClient.SendTextMessageAsync(chatId, creditos, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    break;

                case "3️⃣ historial de compras":
                    string historial = _clienteBLL.GenerarHistorialCompras(userState.ClientId);
                    await _botClient.SendTextMessageAsync(chatId, historial, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    break;

                case "4️⃣ próximos vencimientos":
                    string vencimientos = _clienteBLL.GenerarProximosVencimientos(userState.ClientId);
                    await _botClient.SendTextMessageAsync(chatId, vencimientos, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    break;

                case "5️⃣ contactar tienda":
                    await _botClient.SendTextMessageAsync(chatId,
                        "Para comunicarte con la tienda puedes:\n" +
                        "- Llamar al: 0800-A MANO\n");
                    break;

                case "/start":
                    await MostrarMenuPrincipalCliente(chatId);
                    break;

                default:
                    await _botClient.SendTextMessageAsync(chatId,
                        "Opción no reconocida. Por favor, selecciona una opción del menú o escribe /start para mostrar el menú principal.");
                    break;
            }
        }

        private async Task ProcesarComandoAdministrador(long chatId, string comando, UserState userState)
        {
            switch (comando)
            {
                case "📊 ver todos los clientes":
                    string listaClientes = _adminBLL.GenerarListaClientes();
                    await _botClient.SendTextMessageAsync(chatId, listaClientes, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    break;

                case "🔍 buscar cliente por nombre":
                    await _botClient.SendTextMessageAsync(chatId, "Ingresa el nombre o parte del nombre del cliente:");
                    userState.MenuState = MenuState.BuscandoClientePorNombre;
                    break;

                case "📋 clientes en mora":
                    string clientesEnMora = _adminBLL.GenerarListaClientesEnMora();
                    await _botClient.SendTextMessageAsync(chatId, clientesEnMora, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    break;

                case "✅ clientes al día":
                    string clientesAlDia = _adminBLL.GenerarListaClientesAlDia();
                    await _botClient.SendTextMessageAsync(chatId, clientesAlDia, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    break;

                case "📅 vencimientos próximos":
                    string vencimientosProximos = _adminBLL.GenerarVencimientosProximos();
                    await _botClient.SendTextMessageAsync(chatId, vencimientosProximos, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    break;

                case "/start":
                    await MostrarMenuPrincipalAdministrador(chatId);
                    break;

                default:
                    // Si estamos buscando un cliente por nombre
                    if (userState.MenuState == MenuState.BuscandoClientePorNombre)
                    {
                        string resultadoBusqueda = _adminBLL.BuscarClientesPorNombre(comando);
                        await _botClient.SendTextMessageAsync(chatId, resultadoBusqueda, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                        userState.MenuState = MenuState.Main;
                        await MostrarMenuPrincipalAdministrador(chatId);
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(chatId,
                            "Opción no reconocida. Por favor, selecciona una opción del menú o escribe /start para mostrar el menú principal.");
                    }
                    break;
            }
        }

        private async Task MostrarMenuPrincipalCliente(long chatId)
        {
            var userState = _userStates[chatId];
            userState.MenuState = MenuState.Main;

            var menuMessage =
                $"🏦 *MENU PRINCIPAL - A MANO* \n" +
                $"Cliente: {userState.ClientName}\n" +
                $"Cédula: {userState.ClientCedula}\n\n" +
                "Selecciona una opción:";

            var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("1️⃣ Resumen de Cuenta"), new KeyboardButton("2️⃣ Mis Créditos Pendientes") },
                new[] { new KeyboardButton("3️⃣ Historial de Compras"), new KeyboardButton("4️⃣ Próximos Vencimientos") },
                new[] { new KeyboardButton("5️⃣ Contactar Tienda") },
                new[] { new KeyboardButton("❌ Cerrar Sesión") }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = false
            };

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: menuMessage,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                replyMarkup: keyboard
            );
        }

        private async Task MostrarMenuPrincipalAdministrador(long chatId)
        {
            var userState = _userStates[chatId];
            userState.MenuState = MenuState.Main;

            var menuMessage = $"🏦 *PANEL DE ADMINISTRADOR - A MANO* \n" +
                             $"Administrador: {userState.AdminName}\n\n" +
                             "Selecciona una opción:";

            var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new[] { new KeyboardButton("📊 Ver Todos los Clientes"), new KeyboardButton("🔍 Buscar Cliente por Nombre") },
                new[] { new KeyboardButton("📋 Clientes en Mora"), new KeyboardButton("✅ Clientes al Día") },
                new[] { new KeyboardButton("📅 Vencimientos Próximos") },
                new[] { new KeyboardButton("❌ Cerrar Sesión") }
            })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = false
            };

            await _botClient.SendTextMessageAsync(
                chatId: chatId,
                text: menuMessage,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                replyMarkup: keyboard
            );
        }

        private async Task CerrarSesion(long chatId)
        {
            if (_userStates.ContainsKey(chatId))
            {
                if (_userStates[chatId].UserRole == UserRole.Administrador)
                {
                    _adminChatIds.Remove(chatId);
                }
                _userStates.Remove(chatId);
            }

            await _botClient.SendTextMessageAsync(chatId, "Has cerrado sesión. ¡Hasta pronto! 👋");
            await MostrarSeleccionTipoUsuario(chatId);
        }

        private async Task EnviarNotificacionesVencimientos()
        {
            try
            {
                var notificaciones = _adminBLL.ObtenerNotificacionesVencimientos();

                foreach (var adminChatId in _adminChatIds)
                {
                    foreach (var notificacion in notificaciones)
                    {
                        string mensaje = $"🔔 *NOTIFICACIÓN DE VENCIMIENTO* \n" +
                                       $"👤 Cliente: {notificacion.NombreCliente} (ID: {notificacion.ClienteId})\n" +
                                       $"💵 Monto: ${notificacion.Monto:N2}\n" +
                                       $"📅 Vence: {notificacion.FechaVencimiento:dd/MM/yyyy}\n";

                        await _botClient.SendTextMessageAsync(adminChatId, mensaje, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"Error al enviar notificaciones: {ex.Message}");
            }
        }

        public Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var apiException = exception as ApiRequestException;
            string errorMessage = apiException != null ? $"Error de API Telegram: {apiException.Message}" : exception.Message;
            AddLog($"Error: {errorMessage}");
            return Task.CompletedTask;
        }

        private void AddLog(string message)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");
        }
    }
}
