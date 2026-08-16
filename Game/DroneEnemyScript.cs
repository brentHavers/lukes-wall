using System;
using SoManyPixels.ArcEngine.A2D;
using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.ArcEngine.A2D.Entities;
using SoManyPixels.ArcEngine.Base.DataTypes;

namespace SoManyPixels.LukesWall.Game
{
    public class DroneEnemyScript : A2DScriptComponent
    {
        const int ScreenWidth = 1920;

        bool _isFirstPass = true;
        float _positionX = 0.0f;
        float _positionY = 0.0f;
        bool _horizontalWallVelocityFlippedThisFrame;
        bool _verticalWallVelocityFlippedThisFrame;

        A2DTransformComponent _transformComponent;
        A2DSpriteComponent _spriteComponent;

        public DroneEnemyScript(A2DEntityWorld world)
        {
            _entityWorld = world;
        }

        public override bool OnUpdate(float elapsedtime)
        {
            if (_isFirstPass)
            {
                _transformComponent = GetComponent<A2DTransformComponent>();
                _spriteComponent = GetComponent<A2DSpriteComponent>();

                if (_transformComponent == null)
                {
                    return false;
                }

                _positionX = _transformComponent.PositionX;
                _positionY = _transformComponent.PositionY;

                _isFirstPass = false;
            }

            _horizontalWallVelocityFlippedThisFrame = false;
            _verticalWallVelocityFlippedThisFrame = false;

            float dt = elapsedtime / 1000.0f;
            _positionX += _transformComponent.DirectionX * dt;
            _positionY += _transformComponent.DirectionY * dt;
            _transformComponent.PositionX = (int)Math.Round(_positionX);
            _transformComponent.PositionY = (int)Math.Round(_positionY);

            int spriteWidth = _spriteComponent.CurrentAnimationFrame.Width;

            if (_transformComponent.PositionX < -spriteWidth)
            {
                StopSelf();
            }
            else if (_transformComponent.PositionX > ScreenWidth + spriteWidth)
            {
                StopSelf();
            }

            return base.OnUpdate(elapsedtime);
        }

        public override bool OnCollisionActivate(A2DCollision collision)
        {
            A2DTransformComponent transformComponent = GetComponent<A2DTransformComponent>();
            A2DPlayerStatsComponent healthComponent = GetComponent<A2DPlayerStatsComponent>();

            if (transformComponent == null || healthComponent == null)
            {
                return false;
            }

            if (collision.IncomingCollider.OwnerEntityName.StartsWith("FriendlyBullet") && collision.Collider.OwnerEntityName != "PlayerOne")
            {
                healthComponent.HealthValue--;
            }

            if (collision.IncomingCollider.Kind == CollisionKinds.Wall)
            {
                HandleWallCollision(collision, transformComponent);
            }

            if (healthComponent.HealthValue <= 0 && collision.Collider.OwnerEntityName != "PlayerOne")
            {
                StopSelf();

                EntityFactory.CreateExplosion(_entityWorld, transformComponent.PositionX, transformComponent.PositionY, 0, 0);
            }

            return true;
        }

        public override bool OnCollisionPersist(A2DCollision collision)
        {
            A2DTransformComponent transformComponent = GetComponent<A2DTransformComponent>();
            A2DPlayerStatsComponent healthComponent = GetComponent<A2DPlayerStatsComponent>();

            if (transformComponent == null || healthComponent == null)
            {
                return false;
            }

            if (healthComponent.HealthValue <= 0 && collision.Collider.OwnerEntityName != "PlayerOne")
            {
                StopSelf();

                EntityFactory.CreateExplosion(_entityWorld, transformComponent.PositionX, transformComponent.PositionY, 0, 0);
            }
            else if (collision.IncomingCollider.Kind == CollisionKinds.Wall)
            {
                HandleWallCollision(collision, transformComponent);
            }

            return true;
        }

        // Resolves a wall overlap by pushing the entity back to whichever side of the
        // wall its box center is currently on. The reported contact side only picks the
        // axis; it is not trusted for direction because it is recomputed from overlap
        // geometry each frame and flips once an entity is embedded in a thin wall,
        // which previously teleported entities through to the far side.
        void HandleWallCollision(A2DCollision collision, A2DTransformComponent transformComponent)
        {
            A2DBoundingBoxComponent ourBoundingBox = (A2DBoundingBoxComponent)collision.Collider;
            A2DTransformComponent wallTransform = _entityWorld.GetComponent<A2DTransformComponent>(collision.IncomingCollider.OwnerId);
            A2DBoundingBoxComponent wallBoundingBox = _entityWorld.GetComponent<A2DBoundingBoxComponent>(collision.IncomingCollider.OwnerId);

            if (ourBoundingBox == null || wallTransform == null || wallBoundingBox == null)
            {
                return;
            }

            A2DIntRectangle ourBox = ourBoundingBox.BoundingBox;
            A2DIntRectangle wallBox = wallBoundingBox.BoundingBox;

            int wallX = wallTransform.PositionX + wallBox.X;
            int wallY = wallTransform.PositionY + wallBox.Y;

            bool horizontalContact = collision.ColliderContactSides == A2DCollisionContactFlags.Left
                || collision.ColliderContactSides == A2DCollisionContactFlags.Right;
            bool verticalContact = collision.ColliderContactSides == A2DCollisionContactFlags.Top
                || collision.ColliderContactSides == A2DCollisionContactFlags.Bottom;

            if (horizontalContact)
            {
                int ourCenterX = transformComponent.PositionX + ourBox.X + (ourBox.Width / 2);
                int wallCenterX = wallX + (wallBox.Width / 2);
                bool entityIsLeftOfWall = ourCenterX <= wallCenterX;

                if (entityIsLeftOfWall)
                {
                    if (transformComponent.DirectionX > 0 && !_horizontalWallVelocityFlippedThisFrame)
                    {
                        transformComponent.DirectionX *= -1;
                        _horizontalWallVelocityFlippedThisFrame = true;
                    }

                    transformComponent.PositionX = wallX - ourBox.X - ourBox.Width;
                }
                else
                {
                    if (transformComponent.DirectionX < 0 && !_horizontalWallVelocityFlippedThisFrame)
                    {
                        transformComponent.DirectionX *= -1;
                        _horizontalWallVelocityFlippedThisFrame = true;
                    }

                    transformComponent.PositionX = wallX + wallBox.Width - ourBox.X;
                }

                SyncInternalMovementState(transformComponent);
            }

            if (verticalContact)
            {
                int ourCenterY = transformComponent.PositionY + ourBox.Y + (ourBox.Height / 2);
                int wallCenterY = wallY + (wallBox.Height / 2);
                bool entityIsAboveWall = ourCenterY <= wallCenterY;

                if (entityIsAboveWall)
                {
                    if (transformComponent.DirectionY > 0 && !_verticalWallVelocityFlippedThisFrame)
                    {
                        transformComponent.DirectionY *= -1;
                        _verticalWallVelocityFlippedThisFrame = true;
                    }

                    transformComponent.PositionY = wallY - ourBox.Y - ourBox.Height;
                }
                else
                {
                    if (transformComponent.DirectionY < 0 && !_verticalWallVelocityFlippedThisFrame)
                    {
                        transformComponent.DirectionY *= -1;
                        _verticalWallVelocityFlippedThisFrame = true;
                    }

                    transformComponent.PositionY = wallY + wallBox.Height - ourBox.Y;
                }

                SyncInternalMovementState(transformComponent);
            }
        }

        void SyncInternalMovementState(A2DTransformComponent transformComponent)
        {
            _positionX = transformComponent.PositionX;
            _positionY = transformComponent.PositionY;
        }

        public override string ToString()
        {
            return $"{_transformComponent.DirectionX}";
        }
    }
}
