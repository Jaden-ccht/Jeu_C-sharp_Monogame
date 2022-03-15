﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjetVR.Core.Game.Animations;
using ProjetVR.Core.GameEntities;
using System.IO;

namespace ProjetVR.Core.Game.GameEntities
{
    public class Bear : Entity
    {
        private AnimationPlayer sprite;
        private Animation idleAnimation;
        private Animation runAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private Rectangle localBounds;
        private int movement;
        public Bear(SpriteBatch _s, Microsoft.Xna.Framework.Game game) 
            : base(_s, game)
        {
            this.sprite = new AnimationPlayer();
            this.entitySpeed = 40f;
        }

        public void LoadContent(ContentManager c)
        {
            idleAnimation = new Animation(c.Load<Texture2D>("idle3"), 0.1f, true, 4);
            runAnimation = new Animation(c.Load<Texture2D>("run3"), 0.1f, true, 6);

            int width = (int)(idleAnimation.FrameWidth * 0.4);
            int left = (idleAnimation.FrameWidth - width) / 2;
            int height = (int)(idleAnimation.FrameHeight * 0.4);
            int top = idleAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
        }

        public void Update(GameTime gameTime, 
            Character character)
        {
            if (this.entityPosition == character.entityPosition)
                sprite.PlayAnimation(idleAnimation);
            if (this.entityPosition.Y > character.entityPosition.Y) 
            {
                this.entityPosition = new Vector2(this.entityPosition.X, this.entityPosition.Y - this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                sprite.PlayAnimation(runAnimation);
            }
                
            if (this.entityPosition.Y < character.entityPosition.Y)
            {
                this.entityPosition = new Vector2(this.entityPosition.X, this.entityPosition.Y + this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                sprite.PlayAnimation(runAnimation);
            }

            if (this.entityPosition.X > character.entityPosition.X)
            {
                movement = 1;
                this.entityPosition = new Vector2(this.entityPosition.X - this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, this.entityPosition.Y);
                sprite.PlayAnimation(runAnimation);
            }

            if (this.entityPosition.X < character.entityPosition.X)
            {
                movement = 2;
                this.entityPosition = new Vector2(this.entityPosition.X + this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, this.entityPosition.Y);
                sprite.PlayAnimation(runAnimation);
            }
        }


        public override void Draw(GameTime gameTime)
        {
            if (movement == 1)
                flip = SpriteEffects.None;
            else if (movement == 2)
                flip = SpriteEffects.FlipHorizontally;

            sprite.Draw(gameTime, _sb, entityPosition, flip);
        }
    }
}
