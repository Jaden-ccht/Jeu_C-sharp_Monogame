using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using ProjetVR.Core.Game.Animations;
using ProjetVR.Core.Game.Collisions;

namespace ProjetVR.Core.Game.GameEntities
{
    public class Character : Entity
    {        
        public Animation HitAnimation
        {
            get { return hitAnimation; }
            set { hitAnimation = value; }
        }
        private Animation hitAnimation;


        public Character(SpriteBatch _s, Microsoft.Xna.Framework.Game game) 
            : base(_s, game)
        {
            this.EntitySpeed = 150f;
            Sprite = new AnimationPlayer();
            IsAlive = true;
            EntityPosition = new Vector2(480, 288);
        }

        public override void LoadContent(ContentManager c)
        {
            IdleAnimation = new Animation(c.Load<Texture2D>("idle"), 0.1f, true, 6);
            RunAnimation = new Animation(c.Load<Texture2D>("run"), 0.1f, true, 6);
            HitAnimation = new Animation(c.Load<Texture2D>("hit"), 0.1f, false, 4);
            DeathAnimation = new Animation(c.Load<Texture2D>("death"), 0.1f, false, 3);
            Sprite.PlayAnimation(IdleAnimation);
        }

        public bool CheckHit()
        {
            if(Sprite.Animation == HitAnimation)
            {
                if (Sprite.FrameIndex == HitAnimation.FrameCount - 1)
                {
                    Sprite.PlayAnimation(IdleAnimation);
                    return true;
                }                    
                return false;
            }
            return true;
        }

        
        public override void Draw(GameTime gameTime)
        {
            // Flip the sprite to face the way we are moving.
            if (Movement == 2)
                flip = SpriteEffects.None;
            else if (Movement == 1)
                flip = SpriteEffects.FlipHorizontally;

            // Draw that sprite.
            Sprite.Draw(gameTime, _sb, EntityPosition, flip);
        }
    }
}
