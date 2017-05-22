using System;
using Autofac.Features.Indexed;
using DFWin.Core.Constants;
using DFWin.Core.Services;
using DFWin.Core.User32Extensions.Models;
using DFWin.Core.User32Extensions.Services;
using DFWin.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace DFWin
{
    public class DwarfFortress : Game
    {
        private readonly Window dwarfFortressWindow;
        private readonly ContentManager contentManager;
        private readonly ScreenManager screenManager;
        private readonly IWindowService windowService;
        private readonly IGameGridService gameGridService;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public DwarfFortress(IIndex<DependencyKeys.Window, Window> windows, ContentManager contentManager, ScreenManager screenManager, IWindowService windowService, IGameGridService gameGridService)
        {
            dwarfFortressWindow = windows[DependencyKeys.Window.DwarfFortress];
            this.contentManager = contentManager;
            this.screenManager = screenManager;
            this.windowService = windowService;
            this.gameGridService = gameGridService;
            previousWidth = 0;

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Sizes.DwarfFortressDefaultScreenSize.Width,
                PreferredBackBufferHeight = Sizes.DwarfFortressDefaultScreenSize.Height
            };

            Content.RootDirectory = "Content";

            Window.Title = Names.GameTitle;
            Window.AllowUserResizing = true;
            Window.AllowAltF4 = false;
            Window.OrientationChanged += Window_ClientSizeChanged;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        protected override void Initialize()
        {
            base.Initialize();

            CentreWindow();
        }

        private void CentreWindow()
        {
            Window.Position = new Point((GraphicsDevice.DisplayMode.Width - GraphicsDevice.Viewport.Width) / 2, (GraphicsDevice.DisplayMode.Height - GraphicsDevice.Viewport.Height) / 2);
        }
        
        protected override void LoadContent()
        {
            contentManager.Load(GraphicsDevice, Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            MediaPlayer.Volume = 0.4f;
            MediaPlayer.Play(contentManager.Song);
        }

        private int previousWidth;
        private int previousHeight;

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            Resize();
        }

        private void Resize()
        {
            if (Window.ClientBounds.Width == previousWidth && Window.ClientBounds.Height == previousHeight) return;

            previousWidth = Window.ClientBounds.Width;
            previousHeight = Window.ClientBounds.Height;
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            Resize();

            var screenshot = windowService.Capture(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize, true).GetAwaiter().GetResult();
            var tiles = gameGridService.ParseScreenshot(screenshot);
            
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            var screenTools = new ScreenTools(GraphicsDevice, spriteBatch);
            screenManager.Draw(screenTools);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
