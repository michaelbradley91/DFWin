using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using DFWin.Attributes;
using DFWin.Core;
using DFWin.Core.Constants;
using DFWin.Core.Helpers;
using DFWin.Core.States;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Color = Microsoft.Xna.Framework.Color;

namespace DFWin
{
    public class ContentManager
    {
        public Texture2D LoadingBackground { get; private set; }
        public Texture2D WhiteRectangle { get; private set; }
        public SpriteFont MediumFont { get; private set; }
        public Song Song { get; private set; }
        public Texture2D BackupTileSet { get; private set; }

        private readonly IDictionary<Size, RenderTarget2D> renderTargets = new Dictionary<Size, RenderTarget2D>();

        public RenderTarget2D MainRenderTarget { get; private set; }
        public RenderTarget2D BackupRenderTarget { get; private set; }

        private readonly Lazy<IScreenManager> screenManager;

        public ContentManager(Lazy<IScreenManager> screenManager)
        {
            this.screenManager = screenManager;
        }

        public void Load(GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            LoadingBackground = content.Load<Texture2D>("LoadingBackground");
            MediumFont = content.Load<SpriteFont>("Px437_IBM_BIOS_Font");
            Song = content.Load<Song>("Vindsvept - Heart of Ice");
            BackupTileSet = content.Load<Texture2D>("BackupTileSet");
            MainRenderTarget = ScreenHelpers.CreateRenderTarget(graphicsDevice, Sizes.DefaultTargetScreenSize);
            BackupRenderTarget = ScreenHelpers.CreateRenderTarget(graphicsDevice, Sizes.BackupScreenSize);

            WhiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            WhiteRectangle.SetData(new[] { Color.White });

            LoadRenderTargets(graphicsDevice);
        }

        private void LoadRenderTargets(GraphicsDevice graphicsDevice)
        {
            foreach (var screen in screenManager.Value.AllScreens)
            {
                var target = screen.GetType().GetCustomAttributes().OfType<TargetScreenSizeAttribute>().SingleOrDefault();
                if (target == null) continue;

                var size = new Size(target.Width, target.Height);
                if (renderTargets.ContainsKey(size)) continue;

                renderTargets[size] = ScreenHelpers.CreateRenderTarget(graphicsDevice, size);
            }

            if (renderTargets.ContainsKey(Sizes.DefaultTargetScreenSize)) return;
            renderTargets[Sizes.DefaultTargetScreenSize] = ScreenHelpers.CreateRenderTarget(graphicsDevice, Sizes.DefaultTargetScreenSize);
        }

        public RenderTarget2D GetRenderTarget(GameState gameState)
        {
            var screen = screenManager.Value.GetCurrentScreen(gameState);
            var target = screen.GetType().GetCustomAttributes().OfType<TargetScreenSizeAttribute>().SingleOrDefault();

            return target != null ? renderTargets[new Size(target.Width, target.Height)] : renderTargets[Sizes.DefaultTargetScreenSize];
        }
    }
}
