using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SoManyPixels.ArcEngine.A2D.ScreenManagement;
using SoManyPixels.Lukes_Wall.Screens;

#if WINDOWS
using System.IO;
#endif

namespace LukesWall
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class LukesWall : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager ScreenManager;

        int resolutionX = 1920;
        int resolutionY = 1080;

        public LukesWall()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = resolutionX;
            graphics.PreferredBackBufferHeight = resolutionY;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.HardwareModeSwitch = false;

            #if WINDOWS
            if(File.Exists("config.cfg"))
            {
                using (StreamReader sr = new StreamReader("config.cfg"))
                {
                    string fileLine = sr.ReadLine();

                    if(fileLine.ToLower().IndexOf("windowed") > -1)
                    {
                        if (fileLine.Substring(fileLine.LastIndexOf("=") + 1).Trim().ToUpper() == "N")
                        {
                            graphics.IsFullScreen = true;
                        }
                        else
                        {
                            graphics.IsFullScreen = false;
                        }
                    }
                }
            }
            else
            {
                graphics.IsFullScreen = false;
            }
            #endif

            #if XBOX
            graphics.IsFullScreen = true;
            #endif

            Content.RootDirectory = "Content";

            // temp?
            graphics.IsFullScreen = false;
            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = true;
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
            ScreenManager = new ScreenManager(this, graphics, resolutionX, resolutionY);
            Components.Add(ScreenManager);

            ScreenManager.AddScreen(new LogoScreen(), null);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            ScreenManager.A2DGraphics.LoadContentSetFont(Content.Load<SpriteFont>("LukesWallFont"));

            base.LoadContent();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().GetPressedKeys().Contains(Keys.Escape))
                this.Exit();

            ScreenManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            ScreenManager.Draw(gameTime);
        }
    }
}
