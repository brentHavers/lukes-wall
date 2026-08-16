using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.ArcEngine.Base.DataTypes;

namespace SoManyPixels.LukesWall.Components
{
    public class WeaponBullet : A2DComponent
    {
        private int _bulletCount = 2;
        public int BulletCount
        {
            get { return _bulletCount; }
            set { _bulletCount = value; }
        }
    }
}
