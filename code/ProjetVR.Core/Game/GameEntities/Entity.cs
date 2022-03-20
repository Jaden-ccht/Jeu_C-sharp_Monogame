using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ProjetVR.Core.Game.Animations;
using ProjetVR.Core.Game.GameEntities;

namespace ProjetVR.Core.Game.GameEntities
{
    /// <summary>
    /// Classe Entity :
    /// Classe abstraite dont héritent les classes Character et Mob
    /// Permet d'instancier des entités ayant des animations, une position, une vitesse et un booléen pour savoir si l'entité est en vie
    /// </summary>
    public abstract class Entity : GameObject
    {
        public AnimationPlayer Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        private AnimationPlayer sprite;


        public Animation IdleAnimation
        {
            get { return idleAnimation; }
            set { idleAnimation = value; }
        }
        private Animation idleAnimation;

        public Animation RunAnimation
        {
            get { return runAnimation; }
            set { runAnimation = value; }
        }
        private Animation runAnimation;

        public Animation DeathAnimation
        {
            get { return deathAnimation; }
            set { deathAnimation = value; }
        }
        private Animation deathAnimation;

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }
        private bool isAlive;


        protected SpriteEffects flip = SpriteEffects.None;

        public int Movement
        {
            get { return movement; }
            set { movement = value; }
        }
        private int movement;

        public Vector2 EntityPosition { get; set; }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        public float EntitySpeed { get; set; }

        public Entity(SpriteBatch _s, Microsoft.Xna.Framework.Game game) 
            : base(_s, game)
        {
            
        }
       
        public void SetEntityPosition(int x, int y)
        {
            EntityPosition = new Vector2(x, y);
        }

        /// <summary>
        /// Permet de vérifier si l'animation de mort d'une entité est terminée ou non
        /// </summary>
        /// <returns>bool</returns>
        public bool CheckDead()
        {
            if (Sprite.Animation == DeathAnimation)
            {
                if (Sprite.FrameIndex == DeathAnimation.FrameCount - 1)
                    return true;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Méthode abstraite permettant de charger le contenu des entités (textures pour chaque animation)
        /// </summary>
        /// <param name="c"></param>
        public abstract void LoadContent(ContentManager c);
    }
}
