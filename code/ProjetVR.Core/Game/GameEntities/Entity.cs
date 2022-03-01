using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetVR.Core.GameEntities
{
    public abstract class Entity
    {
        public Vector2 entityPosition { get; set; }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        public float entitySpeed { get; set; }

        public Texture2D entityTexture { get; set; }

        public Entity()
        {
            entityPosition = new Vector2(0, 0);
        }
    }
}
