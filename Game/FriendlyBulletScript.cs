using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.ArcEngine.A2D.Entities;

namespace SoManyPixels.LukesWall.Game
{
    public class FriendlyBulletScript : A2DScriptComponent
    {
        public FriendlyBulletScript(A2DEntityWorld world)
        {
            _entityWorld = world;
        }

        public override bool OnUpdate(float elapsedtime)
        {
            A2DTransformComponent transformComponent = GetComponent<A2DTransformComponent>();

            transformComponent.PositionX += (int)transformComponent.Direction.X * (int)(0.2f * elapsedtime);
            transformComponent.PositionY += (int)transformComponent.Direction.Y * (int)(0.2f * elapsedtime);

            if (transformComponent.PositionX < -32
                || transformComponent.PositionX > 1920
                || transformComponent.PositionY < -32
                || transformComponent.PositionY > 1080)
            {
                StopSelf();
            }

            return true;
        }

        public override bool OnCollisionActivate(ArcEngine.A2D.A2DCollision collision)
        {
            if (!collision.IncomingCollider.OwnerEntityName.StartsWith("Player"))
            {
                StopSelf();
            }

            return true;
        }
    }
}
