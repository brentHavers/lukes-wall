using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SoManyPixels.ArcEngine.A2D;
using SoManyPixels.ArcEngine.A2D.ScreenManagement;
using SoManyPixels.ArcEngine.Base.DataTypes;

namespace SoManyPixels.Lukes_Wall.Screens
{
    public class SplashScreen : GameScreen
    {
        private double _startTime = 0.0;

        private bool _logoMoving;
        private bool logoMoved;
        private bool _startPressed;

        private A2DIntVector2 _splashNameStartPosition;
        private A2DIntVector2 _pressStartPosition;
        public A2DColorLerp ColorLerp { get; set; }

        public SplashScreen()
            : base()
        {
            this.IsPopup = false;

            _logoMoving = false;
            logoMoved = false;
            _startPressed = false;

            _splashNameStartPosition = new A2DIntVector2(448, 270);
            _pressStartPosition = new A2DIntVector2(600, 360);

            TransitionOnTime = TimeSpan.FromSeconds(6.0);

            ColorLerp = new A2DColorLerp(A2DColor.Black, A2DColor.PortalBlue);
        }

        public override void LoadContent()
        {
            ScreenManager.A2DGraphics.AddTextureFromContent("Logo", "Logo");
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.A2DGraphics.ClearDevice(ColorLerp.GetLerpColor(1 - this.TransitionPosition));

            ScreenManager.A2DGraphics.SpriteBatchBegin();

            ScreenManager.A2DGraphics.DrawObject("Logo", _splashNameStartPosition, 0.0f, A2DColor.White);

            if (logoMoved &&  !this._startPressed)
            {
                ScreenManager.A2DGraphics.DrawString(_pressStartPosition, "Press Start", A2DColor.Black);
            }

            ScreenManager.A2DGraphics.SpriteBatchEnd(); 
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (!logoMoved && (ScreenManager.PlayerInput.ButtonStartNew || ScreenManager.PlayerInput.ButtonANew))
            {
                _logoMoving = false;
                logoMoved = true;
                _splashNameStartPosition.Y = 80;
                this.TransitionPosition = 0;

                return;
            }

            if (!logoMoved)
            {
                if (_startTime == 0)
                {
                    _startTime = gameTime.TotalGameTime.Seconds;
                }

                if (gameTime.TotalGameTime.Seconds - _startTime >= 3)
                {
                    _logoMoving = true;
                }

                if (_splashNameStartPosition.Y > 80)
                {
                    if (_logoMoving)
                    {
                        _splashNameStartPosition.Y -= (int)(150.0f * gameTime.ElapsedGameTime.TotalSeconds);
                    }
                }
                else
                {
                    _logoMoving = false;
                    logoMoved = true;
                    _splashNameStartPosition.Y = 80;
                }
            }

            if (logoMoved)
            {
                if (ScreenManager.PlayerInput.ButtonBackPressed)
                {
                    ScreenManager.RemoveScreen(this);
                }

                if (ScreenManager.PlayerInput.ButtonStartNew || ScreenManager.PlayerInput.ButtonANew)
                {
                    ScreenManager.AddScreen(new MainMenuScreen(), null);

                    this._startPressed = true;
                }
            }
        }
    }
}
