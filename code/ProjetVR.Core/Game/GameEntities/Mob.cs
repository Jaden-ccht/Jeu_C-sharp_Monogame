using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetVR.Core.Game.Animations;
using ProjetVR.Core.GameEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetVR.Core.Game.GameEntities
{
    /// <summary>
    /// Classe abstraite Mob :
    /// Hérite d'Entity, correspond aux créatures qui apparaissent sur la map
    /// </summary>
    public abstract class Mob : Entity
    {
        public Mob(SpriteBatch _s, Microsoft.Xna.Framework.Game game)
            : base(_s, game)
        {
            this.Sprite = new AnimationPlayer();
        }
    }
}
