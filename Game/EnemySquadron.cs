using System.Collections.Generic;
using SoManyPixels.ArcEngine.A2D.Entities;

namespace SoManyPixels.LukesWall.Game
{
    public class EnemySquadron : List<int>
    {
        private bool _dropsPowerUp;
        public bool DropsPowerUp
        {
            get { return _dropsPowerUp; }
            set { _dropsPowerUp = value; }
        }

        public EnemySquadron(bool dropspowerup)
        {
            _dropsPowerUp = dropspowerup;
        }

        public void Start(A2DEntityWorld world)
        {
            foreach (int entityId in this)
            {
                world.Start(entityId);
            }
        }
    }
}
