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
    /// <summary>
    /// Classe MovementManager :
    /// Gère les déplacements et actions du joueur
    /// Gère aussi les déplacements des créatures
    /// </summary>
    class MovementManager
    {
        /// <summary>
        /// Compteur permettant de connaître le temps depuis le dernier coup porté par le joueur
        /// Permet d'appliquer un cooldown pour éviter le spam de coups portés
        /// </summary>
        private float timeSinceLastHit = 1f;

        /// <summary>
        /// Permet la gestion des actions du joueur en fonction des inputs du claviers à chaque update
        /// </summary>
        /// <param name="player"></param>
        /// <param name="col"></param>
        /// <param name="gameTime"></param>
        /// <param name="keyboardState"></param>
        public void Deplacer(Player player, Collisionneur col, GameTime gameTime, KeyboardState keyboardState)
        {
            Vector2 pos;
            timeSinceLastHit += (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                    if (keyboardState.IsKeyDown(Keys.Space) && timeSinceLastHit >= 1f)
                    {
                        player.Sprite.PlayAnimation(player.HitAnimation);
                        timeSinceLastHit = 0f;
                    }
                    if (keyboardState.GetPressedKeyCount() == 0)
                    {
                        player.Sprite.PlayAnimation(player.IdleAnimation);
                    }
                }
            }
        }

        /// <summary>
        /// Permet le déplacement d'une créature en fonction des coordonnées du joueur
        /// Les déplacements de sont réalisés que si le joueur est proche de la créature
        /// </summary>
        /// <param name="mob"></param>
        /// <param name="player"></param>
        /// <param name="col"></param>
        /// <param name="gameTime"></param>
        public void DeplacerMob(Mob mob, Player player, Collisionneur col, GameTime gameTime)
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

        /// <summary>
        /// Permet de vérifier si un joueur est proche d'une créature
        /// </summary>
        /// <param name="player"></param>
        /// <param name="mob"></param>
        /// <returns></returns>
        private bool IsArround(Player player, Mob mob)
        {
            if ((Math.Abs(player.EntityPosition.X - mob.EntityPosition.X) < 300) && (Math.Abs(player.EntityPosition.Y - mob.EntityPosition.Y) < 200))
                return true;
            return false;
        }
    }
}
