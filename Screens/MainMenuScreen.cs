using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SoManyPixels.ArcEngine.A2D.ScreenManagement;
using SoManyPixels.ArcEngine.Base.DataTypes;

namespace SoManyPixels.Lukes_Wall.Screens
{
    public class MainMenuScreen : GameScreen
    {
        public MainMenuScreen()
        {
            this.IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public void Initialize()
        {
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.A2DGraphics.SpriteBatchBegin();

            ScreenManager.A2DGraphics.DrawString(new A2DIntVector2(100, 300),  "Sample Menu", A2DColor.Black);

            ScreenManager.A2DGraphics.SpriteBatchEnd();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (ScreenManager.PlayerInput.ButtonStartNew || ScreenManager.PlayerInput.ButtonANew)
            {
                LoadingScreen.Load(ScreenManager, true, null, new GameplayScreen());
            }

            if (ScreenManager.PlayerInput.ButtonBackNew)
            {
                ScreenManager.RemoveScreen(this);
            }
        }
    }
}
