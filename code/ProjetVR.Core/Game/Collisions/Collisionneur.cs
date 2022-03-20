using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using ProjetVR.Core.Game.GameEntities;
using ProjetVR.Core.GameEntities;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace ProjetVR.Core.Game.Collisions
{
    /// <summary>
    /// Classe Collisionneur
    /// Permet la gestion de toutes les collisions et autres interractions pour une map précise
    /// </summary>
    class Collisionneur
    {
        /// <summary>
        /// La map sur laquelle gérer les collisions
        /// </summary>
        private readonly TiledMap MapToCheck;

        /// <summary>
        /// Constructeur de Collisionneur
        /// </summary>
        /// <param name="map"></param>
        public Collisionneur(TiledMap map)
        {
            MapToCheck = map;
        }

        /// <summary>
        /// Vérifie les éventuelles collisions aux coordonnées passées en paramètre pour un type d'Entity lui aussi passé en paramètre
        /// </summary>
        /// <param name="ett"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public bool IsCollision(Entity ett, Vector2 vec)
        {
            TiledMapTileLayer layer = (TiledMapTileLayer) MapToCheck.GetLayer("collisions");
            ushort x1 = (ushort) (Math.Ceiling(vec.X / 32) -1);
            ushort y1 = (ushort)(Math.Ceiling(vec.Y / 32));
            ushort x2, y2;
            if (ett.GetType() == typeof(Player))
            {
                x2 = (ushort)(Math.Ceiling((vec.X + (float)26) / 32) - 1);
                y2 = (ushort)(Math.Ceiling((vec.Y + (float)10) / 32));
            }
            else if (ett.GetType() == typeof(Gobelin))
            {
                y1 = (ushort)(Math.Ceiling((vec.Y - (float)15) / 32));
                x2 = (ushort)(Math.Ceiling((vec.X + (float)14) / 32) - 1);
                y2 = (ushort)(Math.Ceiling((vec.Y - (float)10) / 32));
            }
            else
            {
                y1 = (ushort)(Math.Ceiling((vec.Y - (float)15) / 32));
                x2 = (ushort)(Math.Ceiling((vec.X + (float)12) / 32) - 1);
                y2 = (ushort)(Math.Ceiling((vec.Y - (float)10) / 32));
            }
            
            // Vérifie s'il n'y a pas de Tile aux coordonnées souhaitées
            try
            {
                if (layer.GetTile(x1, y1).IsBlank && layer.GetTile(x1, y2).IsBlank && layer.GetTile(x2, y1).IsBlank && layer.GetTile(x2, y2).IsBlank)
                    return false;
                return true;
            }
            // Si les coordonnées sont hors-map, il y a collision
            catch
            {
                return true;
            }
        }

        /// <summary>
        /// Permet de vérifier si le joueur a atteint une zone de changement de niveau (suivant)
        /// Même principe que IsCollision()
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public bool NextLevelReached(Vector2 vec)
        {
            TiledMapTileLayer layer = (TiledMapTileLayer)MapToCheck.GetLayer("next");
            if (layer == null)
                return false;
            ushort x1 = (ushort)(Math.Ceiling(vec.X / 32) - 1);
            ushort y1 = (ushort)(Math.Ceiling(vec.Y / 32));

            ushort x2 = (ushort)(Math.Ceiling((vec.X + (float)26) / 32) - 1);
            ushort y2 = (ushort)(Math.Ceiling((vec.Y + (float)10) / 32));
            if (layer.GetTile(x1, y1).IsBlank && layer.GetTile(x1, y2).IsBlank && layer.GetTile(x2, y1).IsBlank && layer.GetTile(x2, y2).IsBlank)
                return false;
            return true;
        }

        /// <summary>
        /// Permet de vérifier si le joueur a atteint une zone de changement de niveau (précédent)
        /// Même principe que IsCollision()
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public bool PreviousLevelReached(Vector2 vec)
        {
            TiledMapTileLayer layer = (TiledMapTileLayer)MapToCheck.GetLayer("previous");
            if (layer == null)
                return false;
            ushort x1 = (ushort)(Math.Ceiling(vec.X / 32) - 1);
            ushort y1 = (ushort)(Math.Ceiling(vec.Y / 32));

            ushort x2 = (ushort)(Math.Ceiling((vec.X + (float)26) / 32) - 1);
            ushort y2 = (ushort)(Math.Ceiling((vec.Y + (float)10) / 32));
            if (layer.GetTile(x1, y1).IsBlank && layer.GetTile(x1, y2).IsBlank && layer.GetTile(x2, y1).IsBlank && layer.GetTile(x2, y2).IsBlank)
                return false;
            return true;
        }

        /// <summary>
        /// Permet de vérifier si le joueur a atteint la zone de fin de jeu
        /// Même principe que IsCollision
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public bool EndReached(Vector2 vec)
        {
            TiledMapTileLayer layer = (TiledMapTileLayer)MapToCheck.GetLayer("end");
            if (layer == null)
                return false;
            ushort x1 = (ushort)(Math.Ceiling(vec.X / 32) - 1);
            ushort y1 = (ushort)(Math.Ceiling(vec.Y / 32));

            ushort x2 = (ushort)(Math.Ceiling((vec.X + (float)26) / 32) - 1);
            ushort y2 = (ushort)(Math.Ceiling((vec.Y + (float)10) / 32));
            if (layer.GetTile(x1, y1).IsBlank && layer.GetTile(x1, y2).IsBlank && layer.GetTile(x2, y1).IsBlank && layer.GetTile(x2, y2).IsBlank)
                return false;
            return true;
        }

        /// <summary>
        /// Permet de vérifier si un Mob est entré en contact avec le joueur
        /// </summary>
        /// <param name="player"></param>
        /// <param name="mob"></param>
        /// <returns></returns>
        public bool IsPlayerTouched(Player player, Mob mob)
        {
            if((player.EntityPosition.X - mob.EntityPosition.X) > 0)
            {
                if ((Math.Abs((player.EntityPosition.X + 15) - mob.EntityPosition.X) < 20) && (Math.Abs(player.EntityPosition.Y + 20 - mob.EntityPosition.Y) < 30))
                    return true;
            }
            else
            {
                if ((Math.Abs(player.EntityPosition.X - mob.EntityPosition.X) < 20) && (Math.Abs(player.EntityPosition.Y + 20 - mob.EntityPosition.Y) < 30))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Permet de vérifier si le coup porté par le joueur a touché le mob passé en paramètre
        /// </summary>
        /// <param name="player"></param>
        /// <param name="mob"></param>
        /// <returns></returns>
        public bool IsHitCollision(Player player, Mob mob)
        {
            if ((player.EntityPosition.X - mob.EntityPosition.X) > 0)
            {
                if (Math.Abs((player.EntityPosition.X) - mob.EntityPosition.X) < 25)
                {
                    if ((mob.EntityPosition.Y > player.EntityPosition.Y && mob.EntityPosition.Y < (player.EntityPosition.Y + 30)) || ((mob.EntityPosition.Y + 30) > player.EntityPosition.Y && (mob.EntityPosition.Y + 30) < (player.EntityPosition.Y + 30)))
                        return true;
                }
                return false;
            }
            else
            {
                if (Math.Abs((player.EntityPosition.X+45) - (mob.EntityPosition.X+30)) < 50)
                {
                    if ((mob.EntityPosition.Y > player.EntityPosition.Y && mob.EntityPosition.Y < (player.EntityPosition.Y + 30)) || ((mob.EntityPosition.Y + 30) > player.EntityPosition.Y && (mob.EntityPosition.Y + 30) < (player.EntityPosition.Y + 30)))
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Permet de vérifier si les coordonnées passées en paramètres sont situées dans une zone de spawn de la map
        /// Utilisée pour le spawn aléatoire des créatures
        /// </summary>
        /// <param name="ett"></param>
        /// <param name="vec"></param>
        /// <returns></returns>
        public bool IsSpawnZone(Entity ett, Vector2 vec)
        {
            TiledMapTileLayer layer = (TiledMapTileLayer)MapToCheck.GetLayer("spawnzone");
            ushort x1 = (ushort)(Math.Ceiling(vec.X / 32) - 1);
            ushort y1 = (ushort)(Math.Ceiling((vec.Y - (float)15) / 32));
            ushort x2, y2;
            y2 = (ushort)(Math.Ceiling((vec.Y - (float)10) / 32));
            if (ett.GetType() == typeof(Gobelin))
                x2 = (ushort)(Math.Ceiling((vec.X + (float)14) / 32) - 1);
            else
                x2 = (ushort)(Math.Ceiling((vec.X + (float)12) / 32) - 1);
            try
            {
                if (!layer.GetTile(x1, y1).IsBlank && !layer.GetTile(x1, y2).IsBlank && !layer.GetTile(x2, y1).IsBlank && !layer.GetTile(x2, y2).IsBlank)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
