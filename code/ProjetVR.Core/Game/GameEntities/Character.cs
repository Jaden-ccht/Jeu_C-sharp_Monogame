using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjetVR.Core.Game.Animations;

namespace ProjetVR.Core.GameEntities
{
    public class Character : Entity
    {
        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;

        private AnimationPlayer sprite;
        private Animation idleAnimation;
        private Animation runAnimation;
        private Animation hitAnimation;

        private int movement;

        private SpriteEffects flip = SpriteEffects.None;

        private Rectangle localBounds;

        public Character()
        {
            this.entitySpeed = 120f;
            this.sprite = new AnimationPlayer();
        }

        public void LoadContent(ContentManager c)
        {
            idleAnimation = new Animation(c.Load<Texture2D>("idle"), 0.1f, true, 6);
            runAnimation = new Animation(c.Load<Texture2D>("run"), 0.1f, true, 6);
            hitAnimation = new Animation(c.Load<Texture2D>("hit"), 0.1f, true, 4);

            int width = (int)(idleAnimation.FrameWidth * 0.4);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.4);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
        }

        public void Update(GameTime gameTime,
            KeyboardState keyboardState)
        {
            if (sprite.Animation == null)
                sprite.PlayAnimation(idleAnimation);
            if(checkHit())
            {
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Z))
                {
                    this.entityPosition = new Vector2(this.entityPosition.X, this.entityPosition.Y - this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    sprite.PlayAnimation(runAnimation);
                }

                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                {
                    this.entityPosition = new Vector2(this.entityPosition.X, this.entityPosition.Y + this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    sprite.PlayAnimation(runAnimation);
                }

                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.Q))
                {
                    movement = 1;
                    this.entityPosition = new Vector2(this.entityPosition.X - this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, this.entityPosition.Y);
                    sprite.PlayAnimation(runAnimation);
                }

                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    movement = 2;
                    this.entityPosition = new Vector2(this.entityPosition.X + this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, this.entityPosition.Y);
                    sprite.PlayAnimation(runAnimation);
                }
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    sprite.PlayAnimation(hitAnimation);
                }
                if (keyboardState.GetPressedKeyCount() == 0)
                {
                    sprite.PlayAnimation(idleAnimation);
                }
            }

        }

        private bool checkHit()
        {
            if(sprite.Animation == hitAnimation)
            {
                if (sprite.FrameIndex == hitAnimation.FrameCount - 1)
                    return true;
                return false;
            }
            return true;
        }
        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Flip the sprite to face the way we are moving.
            if (movement == 2)
                flip = SpriteEffects.None;
            else if (movement == 1)
                flip = SpriteEffects.FlipHorizontally;

            // Draw that sprite.
            sprite.Draw(gameTime, spriteBatch, entityPosition, flip);
        }
    }
}
