using System;
using DFWin.Core;
using DFWin.Core.Constants;
using DFWin.Core.Models;
using DFWin.Core.Services;
using DFWin.Core.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;

namespace DFWin
{
    public class DwarfFortress : Game
    {
        private readonly ContentManager contentManager;
        private readonly IScreenManager screenManager;
        private readonly IUpdateManager updateManager;
        private readonly IDwarfFortressService dwarfFortressService;

        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GameState gameState;

        public DwarfFortress(ContentManager contentManager, IScreenManager screenManager,
            IUpdateManager updateManager, IDwarfFortressService dwarfFortressService)
        {
            this.contentManager = contentManager;
            this.screenManager = screenManager;
            this.updateManager = updateManager;
            this.dwarfFortressService = dwarfFortressService;

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

            gameState = GameState.InitialState;
        }

        protected override void Initialize()
        {
            base.Initialize();

            CentreWindow();
            dwarfFortressService.StartScreenScraping();
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
            MediaPlayer.IsRepeating = true;
        }

        private int previousWidth;
        private int previousHeight;

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            EnsureWindowDrawnCorrectly();
        }

        private void EnsureWindowDrawnCorrectly()
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
            EnsureWindowDrawnCorrectly();
            
            gameState = updateManager.Update(gameState);
            
            if (gameState.ShouldExit) Exit();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            var screenTools = new ScreenTools(GraphicsDevice, spriteBatch);
            screenManager.Draw(gameState, screenTools);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            dwarfFortressService.Dispose();

            base.OnExiting(sender, args);
        }
    }
}
