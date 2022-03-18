using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjetVR.Core.Game.GameEntities;

namespace ProjetVR.Core.GameEntities
{
    public abstract class Entity : GameObject
    {
        public Vector2 EntityPosition { get; set; }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        public float EntitySpeed { get; set; }

        public Texture2D entityTexture { get; set; }

        public Entity(SpriteBatch _s, Microsoft.Xna.Framework.Game game) 
            : base(_s, game)
        {
            EntityPosition = new Vector2(100, 100);
        }
       

        public void setEntityPosition(int x, int y)
        {
            EntityPosition = new Vector2(x, y);
        }

        
    }
}
