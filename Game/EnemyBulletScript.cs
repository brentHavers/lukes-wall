using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.ArcEngine.A2D.Entities;

namespace SoManyPixels.LukesWall.Game
{
    public class EnemyBulletScript : A2DScriptComponent
    {
        public EnemyBulletScript(A2DEntityWorld world)
        {
            _entityWorld = world;
        }

        public override bool OnUpdate(float elapsedtime)
        {
            A2DTransformComponent transformComponent = GetComponent<A2DTransformComponent>();

            transformComponent.PositionX += (int)transformComponent.Direction.X;
            transformComponent.PositionY += (int)transformComponent.Direction.Y;

            if (transformComponent.PositionX < -150)
            {
                StopSelf();
            }

            return base.OnUpdate(elapsedtime);
        }

        public override bool OnCollisionActivate(ArcEngine.A2D.A2DCollision collision)
        {
            if (!collision.IncomingCollider.OwnerEntityName.ToLower().Contains("bullet"))
            {
                StopSelf();
            }

            return true;
        }
    }
}
