using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoManyPixels.ArcEngine.A2D.Components;
using SoManyPixels.ArcEngine.A2D.Entities;

namespace SoManyPixels.LukesWall.Game
{
    public class ShootingEnemyScript : A2DScriptComponent
    {
        A2DEntityWorld _world;

        public ShootingEnemyScript(A2DEntityWorld entityworld)
        {
            _world = entityworld;
        }

        public override bool OnUpdate(float elapsedtime)
        {
            return false;
        }
    }
}
