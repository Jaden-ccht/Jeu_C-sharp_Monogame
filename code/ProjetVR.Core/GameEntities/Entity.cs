using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjetVR.Core.GameEntities
{
    public abstract class Entity : DrawableGameComponent
    {
        public Vector2 entityPosition { get; set; }

        public float entitySpeed { get; set; }

        public Texture2D entityTexture { get; set; }

        public Entity(Game game) : base(game)
        {
            entityPosition = new Vector2(400, 400);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }
    }
}
