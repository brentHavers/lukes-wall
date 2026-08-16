using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoManyPixels.ArcEngine.Input;
using SoManyPixels.ArcEngine.A2D.ScreenManagement;
using SoManyPixels.ArcEngine.Base.DataTypes;
using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.LukesWall.Game;
using SoManyPixels.ArcEngine.A2D.Systems;
using SoManyPixels.ArcEngine.A2D.Entities;
using SoManyPixels.LukesWall.Components;
using System.Diagnostics;

namespace SoManyPixels.Lukes_Wall.Screens
{
    public class GameplayScreen : GameScreen
    {
        const int ScreenWidth = 1920;
        const int ScreenHeight = 1080;

        A2DEntityWorld LukesWallWorld;

        int enemyBulletEntityId;
        int powerupEntityId1;
        int powerupEntityId2;
        int powerupEntityId3;
        int enemyRoundEntityId;
        int tileMapEntityId1, tileMapEntityId2;

        EnemySquadron enemySquadron;

        A2DRenderSystem renderSystem;
        A2DCollisionSystem collisionSystem;

        BulletManager playerOneBulletManager;
        BulletManager enemyBulletManager;

        A2DEntityGroupManager groupManager;

        public GameplayScreen()
        {
            this.IsPopup = true;

            this.Initialize();
        }

        public void Initialize()
        {
            LukesWallWorld = new A2DEntityWorld();
            groupManager = new A2DEntityGroupManager();

            LukesWallWorld.AddManager(groupManager);
            RegisterCollisionKindPairRules();
            CreateScreenBoundaryBoxes();

            CreateFriendlyBulletPool();
            playerOneBulletManager = new BulletManager(LukesWallWorld, "FriendlyBullets", groupManager);

            CreatePlayerEntity();

            enemyBulletEntityId = LukesWallWorld.CreateEntity("EnemyBullet1");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(enemyBulletEntityId, 0, 0, -3, 0, 0);
            LukesWallWorld.ComponentFactory.CreateSpriteComponent(enemyBulletEntityId, "EnemyLazzzers", 17, 3);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(enemyBulletEntityId, "EnemyBullet1", new A2DIntRectangle(0, 0, 17, 3));
            LukesWallWorld.AddComponent(enemyBulletEntityId, new EnemyBulletScript(LukesWallWorld));

            groupManager.AddEntityToGroup(enemyBulletEntityId, "EnemyBullets");

            enemyBulletManager = new BulletManager(LukesWallWorld, "EnemyBullets", groupManager);

            //enemySquadron = EntityFactory.CreateSquadron(LukesWallWorld, true);
            //groupManager.AddEntitiesToGroup(enemySquadron, "Enemies");
            //enemySquadron.Start();

            powerupEntityId1 = EntityFactory.CreatePowerUp(LukesWallWorld, "PowerUpS", 624, 300, -2, 1, 10);
            groupManager.AddEntityToGroup(powerupEntityId1, "PowerUps");
            LukesWallWorld.Start(powerupEntityId1);

            powerupEntityId2 = EntityFactory.CreatePowerUp(LukesWallWorld, "PowerUpS", 824, 300, -2, 2, 10);
            groupManager.AddEntityToGroup(powerupEntityId2, "PowerUps");
            LukesWallWorld.Start(powerupEntityId2);

            powerupEntityId3 = EntityFactory.CreatePowerUp(LukesWallWorld, "PowerUpS", 1648, 704, -2, 1, 10);
            groupManager.AddEntityToGroup(powerupEntityId3, "PowerUps");
            LukesWallWorld.Start(powerupEntityId3);

            enemyRoundEntityId = LukesWallWorld.CreateEntity("Enemy");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(enemyRoundEntityId, 10, 10);
            LukesWallWorld.ComponentFactory.CreateSpriteComponent(enemyRoundEntityId, "EnemyRound", 16, 16);
            LukesWallWorld.Start(enemyRoundEntityId);

            tileMapEntityId1 = LukesWallWorld.CreateEntity("tileMap1");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(tileMapEntityId1, 0, 0, 0);
            int[] tiles = new int[510];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = 2;
            }
            LukesWallWorld.ComponentFactory.CreateTileMapComponent(tileMapEntityId1, 30, 17, 64, 64, 2048, 2048, tiles, "TileSheet1");
            LukesWallWorld.Start(tileMapEntityId1);

            tileMapEntityId2 = LukesWallWorld.CreateEntity("tileMap2");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(tileMapEntityId2, 700, 150, 1);
            tiles = new int[] { 33, 32, 0, 1 };
            LukesWallWorld.ComponentFactory.CreateTileMapComponent(tileMapEntityId2, 2, 2, 64, 64, 2048, 2048, tiles, "TileSheet1");
            LukesWallWorld.Start(tileMapEntityId2);

            var wallBlockCoords = new List<A2DIntVector2>
            {
                //new A2DIntVector2(192, 128),
                //new A2DIntVector2(192, 160),
                //new A2DIntVector2(192, 192),
                //new A2DIntVector2(192, 224),
                //new A2DIntVector2(192, 256),
                //new A2DIntVector2(192, 288),
                //new A2DIntVector2(192, 320),
                //new A2DIntVector2(224, 320),
                //new A2DIntVector2(256, 320),
                //new A2DIntVector2(288, 320),
                //new A2DIntVector2(320, 320),
                //new A2DIntVector2(352, 320),

                //new A2DIntVector2(416, 128),
                //new A2DIntVector2(416, 160),
                //new A2DIntVector2(416, 192),
                //new A2DIntVector2(416, 224),
                //new A2DIntVector2(416, 256),
                //new A2DIntVector2(416, 288),
                //new A2DIntVector2(416, 320)

                //new A2DIntVector2(1184, 668)
            };

            foreach (var position in wallBlockCoords)
            {
                LukesWallWorld.Start(EntityFactory.CreateWall(LukesWallWorld, position.X, position.Y, 32, 32));
            }
        }

        private void CreateScreenBoundaryBoxes()
        {
            int topWallEntityId = LukesWallWorld.CreateEntity("WallTop");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(topWallEntityId, 0, 0, 0);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(topWallEntityId, "BoundingBox", 0, 0, ScreenWidth, 10, CollisionKinds.Wall);
            LukesWallWorld.Start(topWallEntityId);

            int bottomWallEntityId = LukesWallWorld.CreateEntity("WallBottom");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(bottomWallEntityId, 0, ScreenHeight - 10, 0);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(bottomWallEntityId, "BoundingBox", 0, 0, ScreenWidth, 10, CollisionKinds.Wall);
            LukesWallWorld.Start(bottomWallEntityId);

            int leftWallEntityId = LukesWallWorld.CreateEntity("WallLeft");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(leftWallEntityId, 0, 0, 0);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(leftWallEntityId, "BoundingBox", 0, 0, 10, ScreenHeight, CollisionKinds.Wall);
            LukesWallWorld.Start(leftWallEntityId);

            int rightWallEntityId = LukesWallWorld.CreateEntity("WallRight");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(rightWallEntityId, ScreenWidth - 10, 0, 0);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(rightWallEntityId, "BoundingBox", 0, 0, 10, ScreenHeight, CollisionKinds.Wall);
            LukesWallWorld.Start(rightWallEntityId);

            int extraWallEntityId = LukesWallWorld.CreateEntity("ExtraWall");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(extraWallEntityId, 0, 0, 0);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(extraWallEntityId, "BoundingBox", 1440, 100, 10, ScreenHeight - 200, CollisionKinds.Wall);
            LukesWallWorld.Start(extraWallEntityId);

            int extraWallEntityId2 = LukesWallWorld.CreateEntity("ExtraWall2");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(extraWallEntityId2, 0, 0, 0);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(extraWallEntityId2, "BoundingBox", 480, 100, 10, ScreenHeight - 200, CollisionKinds.Wall);
            LukesWallWorld.Start(extraWallEntityId2);
        }

        private void RegisterCollisionKindPairRules()
        {
            int rulesEntityId = LukesWallWorld.CreateEntity("CollisionKindPairRules");
            LukesWallWorld.AddComponent(rulesEntityId, CreateCollisionKindPairRulesComponent());
            LukesWallWorld.Start(rulesEntityId);
        }

        private static A2DCollisionKindPairRulesComponent CreateCollisionKindPairRulesComponent()
        {
            A2DCollisionKindPairRulesComponent rulesComponent = A2DCollisionKindPairRulesComponent.Create();
            rulesComponent.PairKeys.Add(GetKindPairKey(CollisionKinds.Default, CollisionKinds.Default));
            rulesComponent.PairKeys.Add(GetKindPairKey(CollisionKinds.Default, CollisionKinds.Enemy));
            rulesComponent.PairKeys.Add(GetKindPairKey(CollisionKinds.Default, CollisionKinds.Player));
            rulesComponent.PairKeys.Add(GetKindPairKey(CollisionKinds.Enemy, CollisionKinds.Player));
            rulesComponent.PairKeys.Add(GetKindPairKey(CollisionKinds.Default, CollisionKinds.Wall));
            rulesComponent.PairKeys.Add(GetKindPairKey(CollisionKinds.Enemy, CollisionKinds.Wall));
            rulesComponent.PairKeys.Add(GetKindPairKey(CollisionKinds.Player, CollisionKinds.Wall));
            return rulesComponent;
        }

        private static long GetKindPairKey(int kindA, int kindB)
        {
            int lowKind = Math.Min(kindA, kindB);
            int highKind = Math.Max(kindA, kindB);
            return ((long)lowKind << 32) | (uint)highKind;
        }

        void CreatePlayerEntity()
        {
            int id = LukesWallWorld.CreateEntity("PlayerOne");
            LukesWallWorld.ComponentFactory.CreateTransformComponent(id, 80, 260, 0, 0, 10);
            LukesWallWorld.ComponentFactory.CreateSpriteComponent(id, "Killer Daisy", 104, 96);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBoxLeft", 0, 16, 52, 64, CollisionKinds.Player);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBoxTop", 16, 0, 72, 16, CollisionKinds.Player);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBoxRight", 52, 16, 52, 64, CollisionKinds.Player);
            LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBoxBottom", 16, 60, 72, 36, CollisionKinds.Player);
            LukesWallWorld.ComponentFactory.CreatePlayerStatsComponent(id, 3);
            LukesWallWorld.AddComponent(id, new WeaponBullet());
            LukesWallWorld.AddComponent(id, new PlayerScript(LukesWallWorld, playerOneBulletManager));
            LukesWallWorld.Start(id);
        }

        void CreateFriendlyBulletPool()
        {
            for (int Counter = 0; Counter < 24; Counter++)
            {
                int bulletEntityId = LukesWallWorld.CreateEntity("FriendlyBullet" + Counter.ToString());
                LukesWallWorld.ComponentFactory.CreateTransformComponent(bulletEntityId, 0, 0, 0, 0, 10);
                LukesWallWorld.ComponentFactory.CreateSpriteComponent(bulletEntityId, "Friggin Lazzzers", 17, 3);
                LukesWallWorld.ComponentFactory.CreateBoundingBoxComponent(bulletEntityId, "BoundingBox", new A2DIntRectangle(0, 0, 17, 3));
                LukesWallWorld.AddComponent(bulletEntityId, new FriendlyBulletScript(LukesWallWorld));

                groupManager.AddEntityToGroup(bulletEntityId, "FriendlyBullets");
            }
        }

        public override void LoadContent()
        {
            ScreenManager.A2DGraphics.AddTextureFromContent("Killer Daisy", "Killer Daisy");
            ScreenManager.A2DGraphics.AddTextureFromContent("Friggin Lazzzers", "Friggin Lazzzers");
            ScreenManager.A2DGraphics.AddTextureFromContent("Smiley", "Smiley");
            ScreenManager.A2DGraphics.AddTextureFromContent("RedSmiley", "RedSmiley");
            ScreenManager.A2DGraphics.AddTextureFromContent("GreenSmiley", "GreenSmiley");
            ScreenManager.A2DGraphics.AddTextureFromContent("Explosion", "Explosion");
            ScreenManager.A2DGraphics.AddTextureFromContent("PowerUpS", "PowerUpS");
            ScreenManager.A2DGraphics.AddTextureFromContent("EnemyLazzzers", "EnemyLazzzers");
            ScreenManager.A2DGraphics.AddTextureFromContent("Bricks", "Bricks");
            ScreenManager.A2DGraphics.AddTextureFromContent("EnemyRound", "EnemyRound");
            ScreenManager.A2DGraphics.AddTextureFromContent("TileSheet1", "TileSheet1");

            renderSystem = new A2DRenderSystem(ScreenManager, LukesWallWorld);
            collisionSystem = new A2DCollisionSystem(LukesWallWorld);

            LukesWallWorld.AddSystem(new A2DInputSystem(ScreenManager, LukesWallWorld));
            LukesWallWorld.AddSystem(new A2DScriptSystem(LukesWallWorld));
            LukesWallWorld.AddSystem(collisionSystem);
            LukesWallWorld.AddSystem(new A2DSpriteAnimationSystem(LukesWallWorld));
            LukesWallWorld.AddSystem(new A2DLifespanSystem(LukesWallWorld));
            LukesWallWorld.AddSystem(new A2DHealthSystem(LukesWallWorld));

            var inputSystem = LukesWallWorld.GetSystem<A2DInputSystem>();
            ScreenManager.PlayerInput = new GameInput(inputSystem.Input, GetFirstConnectedGamePadOrNull());
            ScreenManager.PlayerInputUpdatedByWorld = true;

            // Delay for demonstration purposes
            Thread.Sleep(500);
        }

        static PlayerIndex? GetFirstConnectedGamePadOrNull()
        {
            for (int i = 0; i < 4; i++)
            {
                if (GamePad.GetState((PlayerIndex)i).IsConnected)
                {
                    return (PlayerIndex)i;
                }
            }

            return null;
        }

        static void RestoreDefaultScreenManagerPlayerInput(ScreenManager screenManager)
        {
            screenManager.PlayerInputUpdatedByWorld = false;
            PlayerIndex? pad = GetFirstConnectedGamePadOrNull();
            screenManager.PlayerInput = pad.HasValue
                ? new GameInput(pad.Value)
                : new GameInput();
        }

        public override void UnloadContent()
        {
            RestoreDefaultScreenManagerPlayerInput(ScreenManager);
            base.UnloadContent();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ScreenManager.A2DGraphics.ClearDevice(A2DColor.FromRGBA(200, 255, 200, 255));

            ScreenManager.A2DGraphics.SpriteBatchBegin();

            A2DIntVector2 objectOrigin = ScreenManager.A2DGraphics.GetTextureDimensions("Killer Daisy");

            renderSystem.Draw();

#if DEBUG
            renderSystem.DrawBoundingBoxesDebug();
            // Console.WriteLine(collisionSystem.ToString());
#endif

            ScreenManager.A2DGraphics.SpriteBatchEnd();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            LukesWallWorld.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            ScreenManager.PlayerInput.Update();

            if (ScreenManager.PlayerInput.ButtonBackNew)
            {
                ScreenManager.RemoveScreen(this);
            }

            if (ScreenManager.PlayerInput.ButtonRightShoulderNew)
            {
                enemySquadron = EntityFactory.CreateSquadron(LukesWallWorld, true);

                if (enemySquadron != null)
                {
                    groupManager.AddEntitiesToGroup(enemySquadron, "Enemies");
                    enemySquadron.Start(LukesWallWorld);
                }

                EntityFactory.CreateEnemy(LukesWallWorld, 100, 100, 100, 100, 1, "Enemy");
            }

            if (ScreenManager.PlayerInput.ButtonYNew)
            {
                ScreenManager.A2DGraphics.ToggleFullscreen();
            }
        }
    }
}

