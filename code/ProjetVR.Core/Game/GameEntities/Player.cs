using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using ProjetVR.Core.Game.Animations;
using ProjetVR.Core.Game.Collisions;

namespace ProjetVR.Core.Game.GameEntities
{
    /// <summary>
    /// Classe Player :
    /// Entité correspondant au joueur
    /// </summary>
    public class Player : Entity
    {
        /// <summary>
        /// Animation propre à Character (lorsque le joueur donne un coup)
        /// </summary>
        public Animation HitAnimation
        {
            get { return hitAnimation; }
            set { hitAnimation = value; }
        }
        private Animation hitAnimation;


        public Player(SpriteBatch _s, Microsoft.Xna.Framework.Game game) 
            : base(_s, game)
        {
            this.EntitySpeed = 150f;
            Sprite = new AnimationPlayer();
            IsAlive = true;
            EntityPosition = new Vector2(480, 288);
        }

        public override void LoadContent(ContentManager c)
        {
            IdleAnimation = new Animation(c.Load<Texture2D>("idle"), 0.1f, true, 6);
            RunAnimation = new Animation(c.Load<Texture2D>("run"), 0.1f, true, 6);
            HitAnimation = new Animation(c.Load<Texture2D>("hit"), 0.1f, false, 4);
            DeathAnimation = new Animation(c.Load<Texture2D>("death"), 0.1f, false, 3);
            Sprite.PlayAnimation(IdleAnimation);
        }

        /// <summary>
        /// Permet de vérifier si l'animation de coup d'une entité est terminée ou non
        /// </summary>
        /// <returns>bool</returns>
        public bool CheckHit()
        {
            if(Sprite.Animation == HitAnimation)
            {
                if (Sprite.FrameIndex == HitAnimation.FrameCount - 1)
                {
                    Sprite.PlayAnimation(IdleAnimation);
                    return true;
                }                    
                return false;
            }
            return true;
        }

        
        /// <summary>
        /// Méthode permettant d'afficher le Player
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Retourne l'image afin d'avoir une orientation gauche et droite
            if (Movement == 2)
                flip = SpriteEffects.None;
            else if (Movement == 1)
                flip = SpriteEffects.FlipHorizontally;

            Sprite.Draw(gameTime, _sb, EntityPosition, flip);
        }
    }
}
