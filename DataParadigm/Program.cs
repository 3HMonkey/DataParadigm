using System;
using System.IO;
using System.Runtime.InteropServices;
using DataParadigm.UI;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DataParadigm
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            // Initialize Services
            services.AddSingleton<ILogger>(logger);
            services.AddSingleton<DockSpaceWindow>();
            services.AddSingleton<InfoOverlayWindow>();
            services.AddSingleton<DebugWindow>();
            services.AddSingleton<ToolsWindow>();
            services.AddSingleton<MainGame>();


            SetupDllPaths();

            Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");


            var serviceProvider = services.BuildServiceProvider();
            using var game = serviceProvider.GetService<MainGame>();
            game.Run();
        }

        private static void SetupDllPaths()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            var libsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fnalibs");
            try
            {
                Kernel32.SetDefaultDllDirectories(Kernel32.LoadLibrarySearchDefaultDirs);
                Kernel32.AddDllDirectory(Path.Combine(libsDirectory, Environment.Is64BitProcess ? "x64" : "x86"));
            }
            catch
            {
                // Pre-Windows 7, KB2533623
                Kernel32.SetDllDirectory(Path.Combine(libsDirectory, Environment.Is64BitProcess ? "x64" : "x86"));
            }
        }
    }
}