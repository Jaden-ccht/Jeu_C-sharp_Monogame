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
    class Collisionneur
    {
        public TiledMap MapToCheck
        {
            get { return mapToCheck; }
        }
        private TiledMap mapToCheck;

        public Collisionneur(TiledMap map)
        {
            mapToCheck = map;
        }

        public bool IsCollision(Entity ett, Vector2 vec)
        {
            TiledMapTileLayer layer = (TiledMapTileLayer) mapToCheck.GetLayer("collisions");
            ushort x1 = (ushort) (Math.Ceiling(vec.X / 32) -1);
            ushort y1 = (ushort)(Math.Ceiling(vec.Y / 32));
            ushort x2, y2;
            if (ett.GetType() == typeof(Character))
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
            
            try
            {
                if (layer.GetTile(x1, y1).IsBlank && layer.GetTile(x1, y2).IsBlank && layer.GetTile(x2, y1).IsBlank && layer.GetTile(x2, y2).IsBlank)
                    return false;
                return true;
            }
            catch (IndexOutOfRangeException exc)
            {
                return true;
            }
        }


        public bool NextLevelReached(Vector2 vec)
        {
            TiledMapTileLayer layer = (TiledMapTileLayer)mapToCheck.GetLayer("next");
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

        public bool PreviousLevelReached(Vector2 vec)
        {
            TiledMapTileLayer layer = (TiledMapTileLayer)mapToCheck.GetLayer("previous");
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

        public bool EndReached(Vector2 vec)
        {
            TiledMapTileLayer layer = (TiledMapTileLayer)mapToCheck.GetLayer("end");
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

        public bool IsPlayerTouched(Character player, Mob mob)
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

        public bool IsHitCollision(Character player, Mob mob)
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
    }
}
