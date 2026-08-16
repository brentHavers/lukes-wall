using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoManyPixels.ArcEngine.A2D;
using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.ArcEngine.A2D.Entities;

namespace SoManyPixels.LukesWall.Game
{
    public class PowerupScript : A2DScriptComponent
    {
        private bool _firstPass = true;
        private float _defaultY;

        public PowerupScript(A2DEntityWorld world)
        {
            _entityWorld = world;
        }

        public override bool OnUpdate(float elapsedtime)
        {
            A2DTransformComponent transformComponent = GetComponent<A2DTransformComponent>();

            if (_firstPass)
            {
                _defaultY = transformComponent.PositionY;
            }

            System.Diagnostics.Debug.WriteLine($"{elapsedtime} MOVING? {(int)(0.1f * elapsedtime)}");

            transformComponent.PositionX += (int)(transformComponent.Direction.X * 0.5f);
            transformComponent.PositionY += (int)(transformComponent.Direction.Y * 0.8f);

            if (transformComponent.PositionY < (_defaultY - 150) || transformComponent.PositionY > _defaultY)
            {
                transformComponent.DirectionY *= -1;
            }

            if (transformComponent.PositionX < -50)
            {
                StopSelf();
            }

            _firstPass = false;

            return true; // TODO: return type/value?
        }

        public override bool OnCollisionActivate(A2DCollision collision)
        {
            if (collision.IncomingCollider.OwnerEntityName.StartsWith("Player"))
            {
                if (collision.IncomingCollider.World != null && collision.IncomingCollider.OwnerId >= 0)
                {
                    collision.IncomingCollider.World.SendMessage(collision.IncomingCollider.OwnerId, "powerup", collision.IncomingCollider, 10);
                }

                StopSelf();
            }

            return true;
        }
    }
}
