using System.Collections.Generic;
using SoManyPixels.ArcEngine.A2D.Entities;
using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.ArcEngine.Base.DataTypes;

namespace SoManyPixels.LukesWall.Game
{
    public class BulletManager
    {
        private readonly A2DEntityWorld _world;
        private List<int> _bulletList;

        public BulletManager(A2DEntityWorld world, string owner, A2DEntityGroupManager groupmanager)
        {
            _world = world;
            _bulletList = groupmanager.GetGroupEntityList(owner);
        }

        public bool SpawnBullet(A2DIntVector2 position, A2DFloatVector direction)
        {
            if (_bulletList == null)
            {
                return false;
            }

            foreach (int entityId in _bulletList)
            {
                if (!_world.IsActive(entityId))
                {
                    _world.GetComponent<A2DTransformComponent>(entityId).Position = position;
                    _world.GetComponent<A2DTransformComponent>(entityId).Direction = direction;

                    _world.Start(entityId);

                    return true;
                }
            }

            return false;
        }
    }
}
