using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using ProjetVR.Core.GameEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetVR.Core.Game.Levels
{
    class Level
    {
        public Character Player
        {
            get { return player; }
        }
        Character player;
        public TiledMap Map
        {
            get { return map; }
        }

        TiledMap map;

        public TiledMapRenderer MapRenderer
        {
            get { return mapRenderer; }
        }

        TiledMapRenderer mapRenderer;

        public Boolean NextLevelReached
        {
            get { return nextLevelReached; }
        }

        Boolean nextLevelReached;

        public Level(TiledMap newmap, TiledMapRenderer newmaprenderer, Character crt)
        {
            map = newmap;
            mapRenderer = newmaprenderer;
            player = crt;
            nextLevelReached = false;
        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {

            //GraphicsDevice.Clear(Color.Black);
            var scale = Matrix.CreateScale(2f);

            foreach (TiledMapLayer layer in map.Layers)
            {
                if (layer.Name != "trees")
                {
                    mapRenderer.Draw(layer, scale);
                }
            }
            // TODO: Add your drawing code here
            //a utiliser par la suite
            Player.Draw(gameTime);
            //_spriteBatch.Draw(character.entityTexture, character.entityPosition, this.rectangleSource, Color.White, 0f, new Vector2(character.entityTexture.Width / 2, character.entityTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);

            _spriteBatch.End();
            MapRenderer.Draw(Map.GetLayer("trees"), scale);
            MapRenderer.Draw(Map.GetLayer("trees2"), scale);

        }

        public void Update(GameTime gameTime)
        {
           if(Player.EntityPosition.Y < 10)
            {
                nextLevelReached = true;
            }

        }
    }
}
