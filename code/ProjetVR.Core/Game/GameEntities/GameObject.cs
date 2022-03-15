using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetVR.Core.Game.GameEntities
{
    public abstract class GameObject : DrawableGameComponent
    {
        protected SpriteBatch _sb;

        protected GameObject(SpriteBatch _s, Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            this._sb = _s;
        }

       
    }
}
