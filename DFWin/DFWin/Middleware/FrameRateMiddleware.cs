﻿using System;
using DFWin.Core.Middleware;
using DFWin.Core.Models;
using DFWin.Core.Screens;
using DFWin.Core.States;
using Microsoft.Xna.Framework;

namespace DFWin.Middleware
{
    public class FrameRateMiddleware : IScreenMiddleware
    {
        private readonly ContentManager contentManager;

        public FrameRateMiddleware(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public void Draw(GameState gameState, ScreenTools screenTools, Action<GameState, ScreenTools> next)
        {
            next(gameState, screenTools);
#if DEBUG
            screenTools.SpriteBatch.DrawString(contentManager.LargeFont, "FPS: " + gameState.FrameRate, new Vector2(5, 0), Color.White);
#endif
        }
    }
}
