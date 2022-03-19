using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using ProjetVR.Core.Game.Collisions;
using ProjetVR.Core.Game.GameEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetVR.Core.Game.Movements
{
    class MovementManager
    {
        public void Deplacer(Character player, Collisionneur col, GameTime gameTime, KeyboardState keyboardState)
        {
            Vector2 pos;
            if (player.Sprite.Animation == null)
                player.Sprite.PlayAnimation(player.IdleAnimation);
            if(!player.IsAlive)
                player.Sprite.PlayAnimation(player.DeathAnimation);
            else
            {
                if (player.CheckHit() && player.CheckDead())
                {
                    if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Z))
                    {
                        pos = new Vector2(player.EntityPosition.X, player.EntityPosition.Y - player.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        if (!col.IsCollision(player, pos))
                        {
                            player.EntityPosition = pos;
                            player.Sprite.PlayAnimation(player.RunAnimation);
                        }
                    }

                    if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                    {
                        pos = new Vector2(player.EntityPosition.X, player.EntityPosition.Y + player.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        if (!col.IsCollision(player, pos))
                        {
                            player.EntityPosition = pos;
                            player.Sprite.PlayAnimation(player.RunAnimation);
                        }
                    }

                    if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.Q))
                    {
                        pos = new Vector2(player.EntityPosition.X - player.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, player.EntityPosition.Y);
                        player.Movement = 1;
                        if (!col.IsCollision(player, pos))
                        {
                            player.EntityPosition = pos;
                            player.Sprite.PlayAnimation(player.RunAnimation);
                        }
                    }

                    if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                    {
                        pos = new Vector2(player.EntityPosition.X + player.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, player.EntityPosition.Y);
                        player.Movement = 2;
                        if (!col.IsCollision(player, pos))
                        {
                            player.EntityPosition = pos;
                            player.Sprite.PlayAnimation(player.RunAnimation);
                        }
                    }
                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        player.Sprite.PlayAnimation(player.HitAnimation);
                    }
                    if (keyboardState.GetPressedKeyCount() == 0)
                    {
                        player.Sprite.PlayAnimation(player.IdleAnimation);
                    }
                }
            }
        }

        public void DeplacerMob(Mob mob, Character player, Collisionneur col, GameTime gameTime)
        {
            if (!player.IsAlive)
                mob.Sprite.PlayAnimation(mob.IdleAnimation);
            else
            {
                if ((mob.EntityPosition.Y > player.EntityPosition.Y + 20) && IsArround(player, mob))
                {
                    Vector2 pos = new Vector2(mob.EntityPosition.X, mob.EntityPosition.Y - (mob.EntitySpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (!col.IsCollision(mob, pos))
                    {
                        mob.EntityPosition = pos;
                        mob.Sprite.PlayAnimation(mob.RunAnimation);
                    }
                }

                if ((mob.EntityPosition.Y < player.EntityPosition.Y + 20) && IsArround(player, mob))
                {
                    Vector2 pos = new Vector2(mob.EntityPosition.X, mob.EntityPosition.Y + (mob.EntitySpeed) * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (!col.IsCollision(mob, pos))
                    {
                        mob.EntityPosition = pos;
                        mob.Sprite.PlayAnimation(mob.RunAnimation);
                    }
                }

                if ((mob.EntityPosition.X > player.EntityPosition.X) && IsArround(player, mob))
                {
                    mob.Movement = 1;
                    Vector2 pos = new Vector2(mob.EntityPosition.X - mob.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, mob.EntityPosition.Y);
                    if (!col.IsCollision(mob, pos))
                    {
                        mob.EntityPosition = pos;
                        mob.Sprite.PlayAnimation(mob.RunAnimation);
                    }
                }

                if ((mob.EntityPosition.X < player.EntityPosition.X) && IsArround(player, mob))
                {
                    mob.Movement = 2;
                    Vector2 pos = new Vector2(mob.EntityPosition.X + mob.EntitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, mob.EntityPosition.Y);
                    if (!col.IsCollision(mob, pos))
                    {
                        mob.EntityPosition = pos;
                        mob.Sprite.PlayAnimation(mob.RunAnimation);
                    }
                }
                if (!IsArround(player, mob))
                    mob.Sprite.PlayAnimation(mob.IdleAnimation);
            }
        }

        private bool IsArround(Character player, Mob mob)
        {
            if ((Math.Abs(player.EntityPosition.X - mob.EntityPosition.X) < 250) && (Math.Abs(player.EntityPosition.Y - mob.EntityPosition.Y) < 150))
                return true;
            return false;
        }
    }
}
