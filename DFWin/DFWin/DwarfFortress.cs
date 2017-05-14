using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using DFWin.Core.Constants;
using DFWin.Core.Services;
using DFWin.Core.User32Extensions.Models;
using DFWin.Core.User32Extensions.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using Color = Microsoft.Xna.Framework.Color;

namespace DFWin
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DwarfFortress : Game
    {
        private readonly Window dwarfFortressWindow;
        private readonly IWindowService windowService;
        private readonly IGameGridService gameGridService;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Texture2D background;
        private Texture2D dwarf;
        private Texture2D axe;

        private SpriteFont defaultFont;

        private SoundEffect axeSwing;
        private Song song;

        private Vector2 dwarfPosition = new Vector2(100, 300);

        private Vector2 axePosition;
        private bool hasShot;

        public DwarfFortress(IIndex<DependencyKeys.Window, Window> windows, IWindowService windowService, IGameGridService gameGridService)
        {
            dwarfFortressWindow = windows[DependencyKeys.Window.DwarfFortress];
            this.windowService = windowService;
            this.gameGridService = gameGridService;

            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Sizes.DwarfFortressDefaultScreenSize.Width,
                PreferredBackBufferHeight = Sizes.DwarfFortressDefaultScreenSize.Height
            };

            Content.RootDirectory = "Content";

            Window.AllowUserResizing = true;
            Window.AllowAltF4 = false;
            Window.OrientationChanged += Window_ClientSizeChanged;
            Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            CentreWindow();
        }

        private void CentreWindow()
        {
            Window.Position = new Microsoft.Xna.Framework.Point((GraphicsDevice.DisplayMode.Width - GraphicsDevice.Viewport.Width) / 2, (GraphicsDevice.DisplayMode.Height - GraphicsDevice.Viewport.Height) / 2);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            background = Content.Load<Texture2D>("ForestBackground");
            dwarf = Content.Load<Texture2D>("RunningDwarf");
            axe = Content.Load<Texture2D>("Pickaxe");

            defaultFont = Content.Load<SpriteFont>("DefaultFont");

            axeSwing = Content.Load<SoundEffect>("AxeSwing");

            song = Content.Load<Song>("Vindsvept - Heart of Ice");

            MediaPlayer.Volume = 0.4f;
            MediaPlayer.Play(song);
        }

        private int previousWidth = 0;
        private int previousHeight = 0;

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

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Resize();
            var screenshot = windowService.Capture(dwarfFortressWindow, Sizes.DwarfFortressPreferredClientSize, true).GetAwaiter().GetResult();
            var tiles = gameGridService.ParseScreenshot(screenshot);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            var movement = Vector2.Zero;

            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Right))
            {
                movement.X += 2;
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                movement.X -= 2;
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                movement.Y += 2;
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                movement.Y -= 2;
            }

            if (hasShot) axePosition += new Vector2(5, 0);

            if (axePosition.X > GraphicsDevice.Viewport.Width)
            {
                hasShot = false;
            }

            if (keyState.IsKeyDown(Keys.Space) && !hasShot)
            {
                var instance = axeSwing.CreateInstance();
                instance.Volume = 0.4f;
                instance.Play();
                
                axePosition = dwarfPosition + new Vector2(dwarf.Width, (dwarf.Height - axe.Height) / 2f);
                hasShot = true;
            }

            dwarfPosition += movement;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            spriteBatch.Begin();
            spriteBatch.Draw(background, GraphicsDevice.PresentationParameters.Bounds, Color.White);
            spriteBatch.Draw(dwarf, dwarfPosition, Color.White);
            if (hasShot) spriteBatch.Draw(axe, axePosition, Color.White);
            CentreString(defaultFont, "Welcome to Dwarf Fortress", new Vector2(GraphicsDevice.Viewport.Width / 2f, 20), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CentreString(SpriteFont font, string text, Vector2 position, Color colour)
        {
            var size = font.MeasureString(text);

            spriteBatch.DrawString(font, text, new Vector2(position.X - (size.X / 2f), position.Y), colour);
        }
    }
}
