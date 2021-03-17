using ImGuiNET;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataParadigm.UI
{
    internal sealed class ToolsWindow : Window
    {
        private DebugWindow _debugWindow;
        public ToolsWindow(IServiceProvider serviceProvider, ILogger logger)
        {
            
            _debugWindow = (DebugWindow)serviceProvider.GetService(typeof(DebugWindow));
            _isVisible = true;
        }

        public override bool Show(uint dockSpaceId)
        {
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(100, 450));

            if (ImGui.Begin("Tools", ref _isVisible, ImGuiWindowFlags.NoResize))
            {
                if (ImGui.Button("Test Button"))
                {
                    
                    _debugWindow.Add("dfsdfsdfsdfsd");
                    Console.WriteLine("fdsfsdfsdf");
                }
                ImGui.End();
                return true;
            }

            return false;
        }
    }
}
