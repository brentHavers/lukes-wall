using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SoManyPixels.ArcEngine.A2D.ScreenManagement;
using SoManyPixels.ArcEngine.Base.DataTypes;

namespace SoManyPixels.Lukes_Wall.Screens
{
    public class LogoScreen : GameScreen
    {
        private bool _screenFinished = false;
        private double startTime = 0.0;

        public LogoScreen() : base()
        {
            this.IsPopup = true;

            TransitionOffTime = TimeSpan.FromSeconds(2.0);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            ScreenManager.A2DGraphics.AddTextureFromContent("SMPLogo", "SMPLogo");
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.ScreenState == ArcEngine.A2D.ScreenManagement.ScreenState.TransitionOff)
            {
                ScreenManager.A2DGraphics.ClearDevice(A2DColor.Black);
            }
            else
            {
                ScreenManager.A2DGraphics.ClearDevice(A2DColor.Black);
            }

            ScreenManager.A2DGraphics.SpriteBatchBegin();

            ScreenManager.A2DGraphics.DrawObject("SMPLogo", new A2DIntVector2(512, 108), 0.0f, A2DColor.White);

            ScreenManager.A2DGraphics.SpriteBatchEnd();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            double screenDisplaySeconds = 3;

            if (startTime == 0)
            {
                startTime = gameTime.TotalGameTime.Seconds;
            }

            if ((ScreenManager.PlayerInput.ButtonStartNew || ScreenManager.PlayerInput.ButtonANew || gameTime.TotalGameTime.Seconds - startTime >= screenDisplaySeconds) && !_screenFinished)
            {
                _screenFinished = true;

                this.ExitScreen();
                ScreenManager.AddScreen(new SplashScreen(), null);
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
