using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using ProjetVR.Core.Game.Animations;
using ProjetVR.Core.Game.Collisions;

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

        public Character(SpriteBatch _s, Microsoft.Xna.Framework.Game game) 
            : base(_s, game)
        {
            this.EntitySpeed = 150f;
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
            KeyboardState keyboardState, TiledMap map)
        {
            Collisionneur col = new Collisionneur();
            Vector2 pos;
            if (sprite.Animation == null)
                sprite.PlayAnimation(idleAnimation);
            if (CheckHit())
            {
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Z))
                {
                    pos = new Vector2(this.EntityPosition.X, this.EntityPosition.Y - this.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (!col.IsCollision(pos, map))
                    {
                        this.EntityPosition = pos;
                        sprite.PlayAnimation(runAnimation);
                    }
                }

                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                {
                    pos = new Vector2(this.EntityPosition.X, this.EntityPosition.Y + this.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    if (!col.IsCollision(pos, map))
                    {
                        this.EntityPosition = pos;
                        sprite.PlayAnimation(runAnimation);
                    }
                }

                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.Q))
                {
                    pos = new Vector2(this.EntityPosition.X - this.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, this.EntityPosition.Y);
                    movement = 1;
                    if (!col.IsCollision(pos, map))
                    {
                        this.EntityPosition = pos;
                        sprite.PlayAnimation(runAnimation);
                    }
                }

                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                {
                    pos = new Vector2(this.EntityPosition.X + this.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, this.EntityPosition.Y);
                    movement = 2;
                    if (!col.IsCollision(pos, map))
                    {
                        this.EntityPosition = pos;
                        sprite.PlayAnimation(runAnimation);
                    }
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

        private bool CheckHit()
        {
            if(sprite.Animation == hitAnimation)
            {
                if (sprite.FrameIndex == hitAnimation.FrameCount - 1)
                    return true;
                return false;
            }
            return true;
        }
        
        public override void Draw(GameTime gameTime)
        {
            // Flip the sprite to face the way we are moving.
            if (movement == 2)
                flip = SpriteEffects.None;
            else if (movement == 1)
                flip = SpriteEffects.FlipHorizontally;

            // Draw that sprite.
            sprite.Draw(gameTime, _sb, EntityPosition, flip);
        }
    }
}
