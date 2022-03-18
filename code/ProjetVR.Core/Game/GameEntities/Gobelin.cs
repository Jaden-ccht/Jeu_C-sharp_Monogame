using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjetVR.Core.Game.Animations;
using System.IO;

namespace ProjetVR.Core.GameEntities
{
    public class Gobelin : Entity
    {
        private AnimationPlayer sprite;
        private Animation idleAnimation;
        private Animation runAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private Rectangle localBounds;
        private int movement;
        private int offset = 50;
        public Gobelin(SpriteBatch _s, Microsoft.Xna.Framework.Game game) 
            : base(_s, game)
        {
            this.sprite = new AnimationPlayer();
            this.EntitySpeed = 60f;

        }

        public void LoadContent(ContentManager c)
        {
            idleAnimation = new Animation(c.Load<Texture2D>("idle2"), 0.1f, true, 4);
            runAnimation = new Animation(c.Load<Texture2D>("run2"), 0.1f, true, 6);

            int width = (int)(idleAnimation.FrameWidth * 0.4);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.4);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
        }

        public void Update(GameTime gameTime, 
            Character character)
        {
            if (this.EntityPosition == character.EntityPosition)
                sprite.PlayAnimation(idleAnimation);
            if (this.EntityPosition.Y > character.EntityPosition.Y + 20 )
            {
                this.EntityPosition = new Vector2(this.EntityPosition.X, this.EntityPosition.Y - this.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds );
                sprite.PlayAnimation(runAnimation);
            }

            if (this.EntityPosition.Y < character.EntityPosition.Y + 20)
            {
                this.EntityPosition = new Vector2(this.EntityPosition.X, this.EntityPosition.Y + this.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds );
                sprite.PlayAnimation(runAnimation);
            }

            if (this.EntityPosition.X > character.EntityPosition.X)
            {
                movement = 1;
                this.EntityPosition = new Vector2(this.EntityPosition.X - this.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, this.EntityPosition.Y);
                sprite.PlayAnimation(runAnimation);
            }

            if (this.EntityPosition.X < character.EntityPosition.X)
            {
                movement = 2;
                this.EntityPosition = new Vector2(this.EntityPosition.X + this.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, this.EntityPosition.Y);
                sprite.PlayAnimation(runAnimation);
            }
        }
   

        public override void Draw(GameTime gameTime)
        {
            if (movement == 2)
                flip = SpriteEffects.None;
            else if (movement == 1)
                flip = SpriteEffects.FlipHorizontally;

            sprite.Draw(gameTime, _sb, EntityPosition, flip);
        }
    }
}

