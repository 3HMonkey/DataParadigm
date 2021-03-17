using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataParadigm.UI;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Serilog;
using Num = System.Numerics;

namespace DataParadigm
{
    /// <summary>
    /// Simple FNA + ImGui example
    /// </summary>
    public class MainGame : Game
    {
        private readonly ILogger _logger;
        

        private GraphicsDeviceManager _graphics;
        private ImGuiRenderer _imGuiRenderer;

        private Texture2D _xnaTexture;
        private IntPtr _imGuiTexture;

        private DockSpaceWindow _dockSpaceWindow;
        private InfoOverlayWindow _infoOverlayWindow;
        private DebugWindow _debugWindow;
        private ToolsWindow _toolsWindow;
        

        public static uint MainDockspaceID = 0;

        public MainGame(ILogger logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1200,
                PreferredBackBufferHeight = 768,
                PreferMultiSampling = true,
                IsFullScreen = false,
                SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight,
                GraphicsProfile = GraphicsProfile.HiDef,
            };

            IsMouseVisible = true;

            Window.AllowUserResizing = true;

           
            _dockSpaceWindow = (DockSpaceWindow)serviceProvider.GetService(typeof(DockSpaceWindow)); 
            _infoOverlayWindow = (InfoOverlayWindow)serviceProvider.GetService(typeof(InfoOverlayWindow)); 
            _debugWindow = (DebugWindow)serviceProvider.GetService(typeof(DebugWindow)); 
            _toolsWindow = (ToolsWindow)serviceProvider.GetService(typeof(ToolsWindow)); 
            

        }

        protected override void Initialize()
        {
            _logger.Information("Initializing...");
            _imGuiRenderer = new ImGuiRenderer(this);
            _imGuiRenderer.RebuildFontAtlas();
            ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.DockingEnable;
            _logger.Information("Initializing...Done");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _logger.Information("Loading Content...");
            // Texture loading example

            // First, load the texture as a Texture2D (can also be done using the XNA/FNA content pipeline)
            _xnaTexture = CreateTexture(GraphicsDevice, 300, 150, pixel =>
            {
                var red = (pixel % 300) / 2;
                return new Color(red, 1, 1);
            });

            // Then, bind it to an ImGui-friendly pointer, that we can use during regular ImGui.** calls (see below)
            _imGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);

            base.LoadContent();
            _logger.Information("Loading Content...Done");
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(clear_color.X, clear_color.Y, clear_color.Z));

            // Call BeforeLayout first to set things up
            _imGuiRenderer.BeforeLayout(gameTime);

            // Draw our UI
            ImGuiLayout();

            // Call AfterLayout now to finish up and draw all the things
            _imGuiRenderer.AfterLayout();

            base.Draw(gameTime);
        }

        // Direct port of the example at https://github.com/ocornut/imgui/blob/master/examples/sdl_opengl2_example/main.cpp
        private float f = 0.0f;

        
        private Num.Vector3 clear_color = new Num.Vector3(114f / 255f, 144f / 255f, 154f / 255f);
        private byte[] _textBuffer = new byte[100];

        protected virtual void ImGuiLayout()
        {
          
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("Menu"))
                {
                    //if( ImGui.MenuItem("New", "Ctrl+N", false, true) ) NewProjectWindow.IsVisible = !NewProjectWindow.IsVisible;
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Options"))
                {
                    if (ImGui.MenuItem("Settings", null, false, true))
                    {
                       // _windowService.SettingsWindow.ToggleVisibility();
                    }
                    

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Help"))
                {
                    if (ImGui.MenuItem("About", null, false, true))
                    {
                        //_windowService.AboutWindow.ToggleVisibility();
                    }

                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("View"))
                {
                    if (ImGui.MenuItem("Info Box", null, _infoOverlayWindow.IsVisible, true))
                    {
                        _infoOverlayWindow.ToggleVisibility();
                    }
                    

                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }

            if (_dockSpaceWindow.IsVisible)
            {
                _dockSpaceWindow.Show(MainDockspaceID);
            }

            if (_infoOverlayWindow.IsVisible)
            {
                _infoOverlayWindow.Show(0);
            }

            if (_debugWindow.IsVisible)
            {
                _debugWindow.Show(0);
            }

            if (_toolsWindow.IsVisible)
            {
                _toolsWindow.Show(0);
            }




        }

        

        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
        {
            //initialize a texture
            var texture = new Texture2D(device, width, height);

            //the array holds the color for each pixel in the texture
            Color[] data = new Color[width * height];
            for (var pixel = 0; pixel < data.Length; pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = paint(pixel);
            }

            //set the color
            texture.SetData(data);

            return texture;
        }
    }
}