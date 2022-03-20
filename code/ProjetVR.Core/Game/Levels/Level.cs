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
    /// <summary>
    /// Classe Level :
    /// Agit comme un manager pour chaque niveau avec une map, une liste de créatures différentes
    /// </summary>
    class Level
    {
        /// <summary>
        /// La liste de créatures pour un niveau
        /// </summary>
        public List<Mob> EnemyList
        {
            get { return enemyList; }
            set { enemyList = value; }
        }
        private List<Mob> enemyList;

        /// <summary>
        /// Le joueur
        /// </summary>
        public Player Player
        {
            get { return player; }
        }
        private readonly Player player;

        /// <summary>
        /// La map
        /// </summary>
        public TiledMap Map
        {
            get { return map; }
        }
        private readonly TiledMap map;

        /// <summary>
        /// Le mapRenderer
        /// </summary>
        public TiledMapRenderer MapRenderer
        {
            get { return mapRenderer; }
        }
        private readonly TiledMapRenderer mapRenderer;

        /// <summary>
        /// Un movement manager gérant tous les déplacements et autres actions
        /// </summary>
        public MovementManager Mvm
        {
            get { return mvm; }
        }
        private readonly MovementManager mvm;

        /// <summary>
        /// Un collisionneur
        /// </summary>
        public Collisionneur Col
        {
            get { return col; }
        }
        private readonly Collisionneur col;

        /// <summary>
        /// Bool permettant de savoir si le niveau suivant a été atteint
        /// </summary>
        public bool NextLevelReached
        {
            get { return nextLevelReached; }
            set { nextLevelReached = value; }
        }
        private bool nextLevelReached;

        /// <summary>
        /// Bool permettant de savoir si le niveau précédent a été atteint
        /// </summary>
        public bool PreviousLevelReached
        {
            get { return previousLevelReached; }
            set { previousLevelReached = value; }
        }
        private bool previousLevelReached;

        /// <summary>
        /// Bool permettant de savoir si la zone de fin de jeu a été atteinte
        /// </summary>
        public bool LevelEndReached
        {
            get { return levelEndReached; }
            set { levelEndReached = value; }
        }
        private bool levelEndReached;

        /// <summary>
        /// Bool permettant de savoir si le niveau c'est terminé suite à la mort du joueur
        /// </summary>
        public bool LevelEnded
        {
            get { return levelEnded; }
            set { levelEnded = value; }
        }
        private bool levelEnded;

        /// <summary>
        /// Constructeur de Level
        /// </summary>
        /// <param name="newmap"></param>
        /// <param name="newmaprenderer"></param>
        /// <param name="crt"></param>
        public Level(TiledMap newmap, TiledMapRenderer newmaprenderer, Player crt)
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

        /// <summary>
        /// Charge le contenu de toutes les créatures de la liste
        /// </summary>
        /// <param name="ctt"></param>
        public void LoadContent(ContentManager ctt)
        {
            foreach(Mob mob in EnemyList)
            {
                mob.LoadContent(ctt);
            }
        }

        /// <summary>
        /// Dessine les différentes Layer de la map ainsi que les entités dans un ordre logique afin de simuler une légère perspective
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="_spriteBatch"></param>
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

        /// <summary>
        /// Update du niveau :
        /// La logique du jeu est dans cette méthode
        /// Vérifie un éventuel changement de niveau
        /// Déplace le joueur en fonctions des inputs du clavier
        /// Pour chaque monstre : s'il se trouve dans une zone de coup porté par le joueur, son animation de mort est déclenchée
        ///     si le monste est envie
        ///         le déplace
        ///         s'il touche le joueur, lance son animation de mort et attend 2 secondes avant de déclarer la fin du jeu
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
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

        /// <summary>
        /// Propriété permettant de sauvegarder le moment de mort du joueur
        /// </summary>
        private TimeSpan TimeOfDeath;

        /// <summary>
        /// Propriété permettant de vérifier si la mort du joueur a déjà été enregistrée
        /// </summary>
        private bool TimeOfDeathHasBeenRegistered = false;

        /// <summary>
        /// Permet de set les coordonnées de spawn des monstres aléatoirement dans la zone de spawn correspondante à la map du niveau
        /// </summary>
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
