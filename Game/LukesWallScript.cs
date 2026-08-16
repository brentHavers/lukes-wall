using SoManyPixels.ArcEngine.A2D;
using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.ArcEngine.Base.DataTypes;

namespace SoManyPixels.LukesWall.Game
{
    public class WallScript : A2DScriptComponent
    {
        public override bool OnCollisionActivate(A2DCollision collision)
        {
            A2DTransformComponent transformComponent = GetComponent<A2DTransformComponent>();
            A2DBoundingBoxComponent boundingBoxComponent = GetComponent<A2DBoundingBoxComponent>();
            A2DSpriteComponent spriteComponent = GetComponent<A2DSpriteComponent>();

            if (transformComponent == null || boundingBoxComponent == null)
            {
                return false;
            }

            if (spriteComponent != null)
            {
                spriteComponent.DrawColor = A2DColor.Purple;
            }

            if (transformComponent != null)
            {
                transformComponent.DirectionX = 0;
            }

            return true;
        }

        public override bool OnCollisionDeactivate(A2DCollision collision)
        {
            A2DSpriteComponent spriteComponent = GetComponent<A2DSpriteComponent>();
            if (spriteComponent != null)
            {
                spriteComponent.DrawColor = A2DColor.White;
            }

            return true;
        }

        public override bool OnCollisionPersist(A2DCollision collision)
        {
            return true;
        }
    }
}
