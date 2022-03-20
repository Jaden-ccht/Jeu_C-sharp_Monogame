using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using ProjetVR.Core.Game.Collisions;
using ProjetVR.Core.Game.GameEntities;
using ProjetVR.Core.Game.Movements;
using ProjetVR.Core.GameEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetVR.Core.Game.Levels
{
    class Level
    {
        
        public List<Mob> EnemyList
        {
            get { return enemyList; }
            set { enemyList = value; }
        }
        private List<Mob> enemyList;

        public Character Player
        {
            get { return player; }
        }
        private Character player;

        public TiledMap Map
        {
            get { return map; }
        }
        private TiledMap map;

        public TiledMapRenderer MapRenderer
        {
            get { return mapRenderer; }
        }
        private TiledMapRenderer mapRenderer;

        public MovementManager Mvm
        {
            get { return mvm; }
        }
        private MovementManager mvm;

        public Collisionneur Col
        {
            get { return col; }
        }
        private Collisionneur col;

        public bool NextLevelReached
        {
            get { return nextLevelReached; }
            set { nextLevelReached = value; }
        }
        private bool nextLevelReached;

        public bool PreviousLevelReached
        {
            get { return previousLevelReached; }
            set { previousLevelReached = value; }
        }
        private bool previousLevelReached;

        public bool LevelEndReached
        {
            get { return levelEndReached; }
            set { levelEndReached = value; }
        }
        private bool levelEndReached;

        public bool LevelEnded
        {
            get { return levelEnded; }
            set { levelEnded = value; }
        }
        private bool levelEnded;

        public Level(TiledMap newmap, TiledMapRenderer newmaprenderer, Character crt)
        {
            map = newmap;
            mapRenderer = newmaprenderer;
            player = crt;
            nextLevelReached = false;
            previousLevelReached = false;
            levelEndReached = false;
            enemyList = new List<Mob>();
            mvm = new MovementManager();
            col = new Collisionneur(map);
            levelEnded = false;
        }

        public void LoadContent(ContentManager ctt)
        {
            foreach(Mob mob in EnemyList)
            {
                mob.LoadContent(ctt);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch)
        {
            var scale = Matrix.CreateScale(2f);

            foreach (TiledMapLayer layer in map.Layers)
            {
                if (layer.Name != "trees")
                {
                    mapRenderer.Draw(layer, scale);
                }
            }

            List<Entity> OrderToDraw = new List<Entity>();
            OrderToDraw.AddRange(EnemyList);
            OrderToDraw.Add(player);
            OrderToDraw = OrderToDraw.OrderBy(entity => entity.EntityPosition.Y).ToList();

            foreach (Entity ett in OrderToDraw)
            {
                ett.Draw(gameTime);
            }
            _spriteBatch.End();
            MapRenderer.Draw(Map.GetLayer("trees"), scale);
            MapRenderer.Draw(Map.GetLayer("trees2"), scale);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            enemyList = EnemyList.OrderBy(mob => mob.EntityPosition.Y).ToList();

            MapRenderer.Update(gameTime);

            if(col.NextLevelReached(player.EntityPosition))
                nextLevelReached = true;
            if (col.PreviousLevelReached(player.EntityPosition))
                previousLevelReached = true;
            if(col.EndReached(player.EntityPosition))
                levelEndReached = true;
            mvm.Deplacer(player, col, gameTime, keyboardState);
            foreach (Mob mob in EnemyList.ToList())
            {
                if (keyboardState.IsKeyDown(Keys.Space) && !player.CheckHit())
                {
                    if (col.IsHitCollision(player, mob) && player.Sprite.Animation != player.DeathAnimation)
                    {
                        mob.Sprite.PlayAnimation(mob.DeathAnimation);
                        mob.IsAlive = false;
                    }
                }
                if (mob.IsAlive)
                {
                    mvm.DeplacerMob(mob, player, col, gameTime);
                    if (col.IsPlayerTouched(player, mob))
                    {
                        player.IsAlive = false;
                        player.Sprite.PlayAnimation(player.DeathAnimation);
                        if (!TimeOfDeathHasBeenRegistered)
                        {
                            TimeOfDeath = gameTime.TotalGameTime;
                            TimeOfDeathHasBeenRegistered = true;
                        }
                        if ((gameTime.TotalGameTime - TimeOfDeath) >= TimeSpan.FromSeconds(2))
                        {
                            levelEnded = true;
                            TimeOfDeathHasBeenRegistered = false;
                        }
                    }
                } 
                else if (mob.CheckDead())
                {
                    enemyList.Remove(mob);
                }
            }           
        }

        private TimeSpan TimeOfDeath;
        private bool TimeOfDeathHasBeenRegistered = false;

        public void SetMobPositions()
        {
            Vector2 coo;
            foreach (Mob mob in EnemyList)
            {
                coo = new Vector2(new Random().Next(60, 1000), new Random().Next(20, 460));
                while(!col.IsSpawnZone(mob, coo))
                    coo = new Vector2(new Random().Next(60, 1000), new Random().Next(20, 460));
                mob.EntityPosition = coo;
            }
        }
    }
}
