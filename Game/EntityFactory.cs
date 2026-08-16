using System;
using System.Collections.Generic;
using SoManyPixels.ArcEngine.A2D.Entities;
using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.ArcEngine.Base.DataTypes;
using SoManyPixels.ArcEngine.A2D.Base.Utils;
using SoManyPixels.LukesWall.Components;

namespace SoManyPixels.LukesWall.Game
{
    public class EntityFactory
    {
        static int positionY = 524;
        private static readonly Random _random = new Random();
        private static List<A2DIntRectangle> explosionFrames = SpriteRectangleGenerator.GenerateRectangles(320, 320, 64, 64);

        public static int CreateEnemy(A2DEntityWorld world, int positionx, int positiony, int directionx, int directiony, int healthvalue, string name)
        {
            const int edge = 8;
            int id = world.CreateEntity(name);

            world.ComponentFactory.CreateTransformComponent(id, positionx, positiony, directionx, directiony, 0);
            world.ComponentFactory.CreateSpriteComponent(id, "GreenSmiley", 64, 64);
            world.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBoxTop", edge, 0, 64 - (edge * 2), edge, CollisionKinds.Enemy);
            world.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBoxBottom", edge, 64 - edge, 64 - (edge * 2), edge, CollisionKinds.Enemy);
            world.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBoxLeft", 0, edge, edge, 64 - 2 * edge, CollisionKinds.Enemy);
            world.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBoxRight", 64 - edge, edge, edge, 64 - 2 * edge, CollisionKinds.Enemy);
            world.ComponentFactory.CreatePlayerStatsComponent(id, healthvalue);
            world.AddComponent(id, new DroneEnemyScript(world));
            world.Start(id);

            return id;
        }

        public static int CreateExplosion(A2DEntityWorld world, int positionx, int positiony, int directionx, int directiony)
        {
            int id = world.CreateEntity("explosion");

            world.ComponentFactory.CreateTransformComponent(id, positionx, positiony, directionx, directiony, 0);
            world.ComponentFactory.CreateSpriteComponent(id, "Explosion", explosionFrames);
            world.ComponentFactory.CreateLifespanComponent(id, 1200d);
            world.Start(id);

            return id;
        }

        public static int CreatePowerUp(A2DEntityWorld world, string powerupname, int positionx, int positiony, int directionx, int directiony, int layer = 0)
        {
            int id = world.CreateEntity("powerup");

            world.ComponentFactory.CreateTransformComponent(id, positionx, positiony, directionx, directiony, layer);
            world.ComponentFactory.CreateSpriteComponent(id, powerupname, 32, 16);
            world.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBox", new A2DIntRectangle(0, 0, 32, 16));
            world.ComponentFactory.CreateLifespanComponent(id, 15000d);
            world.AddComponent(id, new PowerupScript(world));

            return id;
        }

        public static EnemySquadron CreateSquadron(A2DEntityWorld world, bool dropspowerup)
        {
            EnemySquadron enemySquadron = new EnemySquadron(dropspowerup);

            int memberId = 0;

            for (int row = 0; row < 12; row++)
            {
                for (int column = 0; column < 24; column++)
                {
                    int id = world.CreateEntity($"squadronmember{memberId}");
                    memberId++;

                    world.ComponentFactory.CreateTransformComponent(id, 192 + (column * 64), 192 + (row * 64), _random.Next(400, 450), 0 /* _random.Next(-150, 150) */, 0);
                    // world.ComponentFactory.CreateCollisionComponent(id, new A2DIntRectangle(8, 8, 50, 50), CollisionKinds.Enemy);
                    world.ComponentFactory.CreateSpriteComponent(id, "RedSmiley", 64, 64);
                    world.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBox", new A2DIntRectangle(8, 8, 50, 50), CollisionKinds.Enemy);
                    world.ComponentFactory.CreatePlayerStatsComponent(id, 1);
                    world.AddComponent(id, new DroneEnemyScript(world));

                    enemySquadron.Add(id);
                }
            }

            positionY += 48;

            if (positionY > 554)
            {
                positionY = 524;
            }

            return enemySquadron;
        }

        public static int CreateWall(A2DEntityWorld world, int positionx, int positiony, int width, int height)
        {
            int id = world.CreateEntity("Wall");

            world.ComponentFactory.CreateTransformComponent(id, positionx, positiony, 0);
            world.ComponentFactory.CreateSpriteComponent(id, "Bricks", width, height);
            world.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBox", 0, 0, width, height, CollisionKinds.Wall);
            world.AddComponent(id, new WallScript());

            return id;
        }

        public static int CreateBlock(A2DEntityWorld world, int positionx, int positiony, int width, int height)
        {
            int id = world.CreateEntity("Wall");

            world.ComponentFactory.CreateTransformComponent(id, positionx, positiony, 0);
            world.ComponentFactory.CreateSpriteComponent(id, "Bricks", width, height, A2DColor.White);
            world.ComponentFactory.CreateBoundingBoxComponent(id, "BoundingBox", 0, 0, width, height, CollisionKinds.Wall);

            return id;
        }
    }
}
