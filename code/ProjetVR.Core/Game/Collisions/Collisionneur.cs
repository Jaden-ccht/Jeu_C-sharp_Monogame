using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using ProjetVR.Core.GameEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetVR.Core.Game.Collisions
{
    class Collisionneur
    {
        public Boolean IsCollision(Vector2 v, TiledMap map)
        {
            TiledMapTileLayer layer = (TiledMapTileLayer) map.GetLayer("collisions");
            TiledMapTileLayer trees = (TiledMapTileLayer)map.GetLayer("trees");
            ushort x1 = (ushort) (Math.Ceiling(v.X / 32) -1);
            ushort y1 = (ushort)(Math.Ceiling(v.Y / 32));

            ushort x2 = (ushort)(Math.Ceiling((v.X + (float)26) / 32) - 1);
            ushort y2 = (ushort)(Math.Ceiling((v.Y +(float)10) / 32));

            if (v.X < 0 || v.Y < 0)
                return true;
            if (layer.GetTile(x1, y1).IsBlank && layer.GetTile(x1, y2).IsBlank && layer.GetTile(x2, y1).IsBlank && layer.GetTile(x2, y2).IsBlank)
                return false;
            return true;
        }
    }
}
