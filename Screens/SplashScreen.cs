using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SoManyPixels.ArcEngine.A2D;
using SoManyPixels.ArcEngine.A2D.Entities;
using SoManyPixels.ArcEngine.A2D.ScreenManagement;
using SoManyPixels.ArcEngine.A2D.Systems;
using SoManyPixels.ArcEngine.Base.DataTypes;
using SoManyPixels.LukesWall.Game;

namespace SoManyPixels.Lukes_Wall.Screens
{
    public class SplashScreen : GameScreen
    {
        const string PressStartText = "Press Start";
        const float PressStartYFraction = 1.0f / 3.0f;  // a third of the way down the viewport

        private A2DEntityWorld _world;
        private A2DRenderSystem _renderSystem;
        private SplashLogoScript _logoScript;

        private bool _startPressed;

        private A2DIntVector2 _pressStartPosition;
        public A2DColorLerp ColorLerp { get; set; }

        public SplashScreen()
            : base()
        {
            this.IsPopup = false;

            _startPressed = false;

            TransitionOnTime = TimeSpan.FromSeconds(6.0);

            ColorLerp = new A2DColorLerp(A2DColor.Black, A2DColor.PortalBlue);

            _world = new A2DEntityWorld();
        }

        public override void LoadContent()
        {
            ScreenManager.A2DGraphics.AddTextureFromContent("Logo", "Logo");

            CreateLogoEntity();
            _pressStartPosition = GetPressStartPosition();

            _renderSystem = new A2DRenderSystem(ScreenManager, _world);
            _world.AddSystem(new A2DScriptSystem(_world));
        }

        A2DIntVector2 GetPressStartPosition()
        {
            A2DIntRectangle viewport = ScreenManager.A2DGraphics.Viewport;

            int textWidth = 0;
            if (ScreenManager.A2DGraphics.SpriteFont != null)
            {
                textWidth = (int)ScreenManager.A2DGraphics.SpriteFont.MeasureString(PressStartText).X;
            }

            return new A2DIntVector2(
                viewport.X + (viewport.Width - textWidth) / 2,
                viewport.Y + (int)(viewport.Height * PressStartYFraction));
        }

        void CreateLogoEntity()
        {
            int id = _world.CreateEntity("SplashLogo");
            _world.ComponentFactory.CreateTransformComponent(id);
            A2DIntVector2 logoSize = ScreenManager.A2DGraphics.GetTextureDimensions("Logo");
            _world.ComponentFactory.CreateSpriteComponent(id, "Logo", logoSize.X, logoSize.Y);
            _logoScript = new SplashLogoScript(_world, ScreenManager.A2DGraphics.Viewport);
            _world.AddComponent(id, _logoScript);
            _logoScript.PlaceAtStart();
            _world.Start(id);
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.A2DGraphics.ClearDevice(ColorLerp.GetLerpColor(1 - this.TransitionPosition));

            ScreenManager.A2DGraphics.SpriteBatchBegin();

            _renderSystem.Draw();

            if (_logoScript.IsFinished && !this._startPressed)
            {
                ScreenManager.A2DGraphics.DrawString(_pressStartPosition, PressStartText, A2DColor.Black);
            }

            ScreenManager.A2DGraphics.SpriteBatchEnd();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            _world.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);

            if (!_logoScript.IsFinished && (ScreenManager.PlayerInput.ButtonStartNew || ScreenManager.PlayerInput.ButtonANew))
            {
                _logoScript.SkipToEnd();
                this.TransitionPosition = 0;

                return;
            }

            if (_logoScript.IsFinished)
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
