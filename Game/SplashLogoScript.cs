using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.ArcEngine.A2D.Entities;
using SoManyPixels.ArcEngine.Base.DataTypes;

namespace SoManyPixels.LukesWall.Game
{
    public class SplashLogoScript : A2DScriptComponent
    {
        // Layout as fractions of the viewport so the splash works at any resolution.
        const float START_Y_FRACTION = 0.25f;       // logo starts a quarter of the way down
        const float TARGET_Y_FRACTION = 0.075f;     // and rises to near the top
        const float SPEED_FRACTION = 0.14f;         // viewport heights per second
        const float DELAY_MS = 3000.0f;

        readonly A2DIntRectangle _viewport;

        int _targetY;
        float _speed;  // px/s

        float _elapsedMs = 0.0f;
        float _positionY = 0.0f;
        bool _movementInitialized = false;

        public bool IsFinished { get; private set; }

        public SplashLogoScript(A2DEntityWorld world, A2DIntRectangle viewport)
        {
            _entityWorld = world;
            _viewport = viewport;
        }

        public override bool OnUpdate(float elapsedtime)
        {
            A2DTransformComponent transformComponent = GetComponent<A2DTransformComponent>();

            if (transformComponent == null || IsFinished)
            {
                return false;
            }

            if (!_movementInitialized)
            {
                PlaceAtStart(transformComponent);
            }

            _elapsedMs += elapsedtime;
            if (_elapsedMs < DELAY_MS)
            {
                return true;
            }

            _positionY -= _speed * elapsedtime / 1000.0f;

            if (_positionY <= _targetY)
            {
                _positionY = _targetY;
                IsFinished = true;
            }

            transformComponent.PositionY = (int)_positionY;

            return true;
        }

        public void SkipToEnd()
        {
            A2DTransformComponent transformComponent = GetComponent<A2DTransformComponent>();
            if (transformComponent == null)
            {
                return;
            }

            if (!_movementInitialized)
            {
                PlaceAtStart(transformComponent);
            }

            transformComponent.PositionY = _targetY;
            _positionY = _targetY;
            IsFinished = true;
        }

        /// <summary>
        /// Positions the logo at its start point. Call after the script and its
        /// transform/sprite components are added, so the first frame draws in place.
        /// </summary>
        public void PlaceAtStart()
        {
            A2DTransformComponent transformComponent = GetComponent<A2DTransformComponent>();
            if (transformComponent != null)
            {
                PlaceAtStart(transformComponent);
            }
        }

        void PlaceAtStart(A2DTransformComponent transformComponent)
        {
            int logoWidth = 0;
            A2DSpriteComponent spriteComponent = GetComponent<A2DSpriteComponent>();
            if (spriteComponent != null)
            {
                logoWidth = (int)(spriteComponent.AnimationFrames[0].Width * spriteComponent.Scale);
            }

            int startX = _viewport.X + (_viewport.Width - logoWidth) / 2;
            int startY = _viewport.Y + (int)(_viewport.Height * START_Y_FRACTION);

            _targetY = _viewport.Y + (int)(_viewport.Height * TARGET_Y_FRACTION);
            _speed = _viewport.Height * SPEED_FRACTION;

            transformComponent.SetPosition(startX, startY);
            _positionY = startY;
            _movementInitialized = true;
        }
    }
}
