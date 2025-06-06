using BLL;
using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace PRIMERA_ENTREGA_PROYECTO_DE_AULA
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            // Build configuration from appsettings.json
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var config = new ConfigurationBuilder()
                .SetBasePath(projectDirectory) // Apunta a PRIMERA ENTREGA PROYECTO DE AULA
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Retrieve the bot token from the config
            string token = config["TelegramBot:Token"];



            // Initialize the bot asynchronously
            var bot = new BotChatBLL(token);

            try
            {
                // Initialize the bot asynchronously (awaitable)
                bot.InicializarBot().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing the bot: {ex.Message}");
                return; // Exit the program if the bot fails to initialize
            }

            // Standard Windows Forms application setup
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }
    }
}
