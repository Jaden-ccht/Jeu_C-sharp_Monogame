using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjetVR.Core.Game.Animations;
using ProjetVR.Core.GameEntities;
using System;
using System.IO;

namespace ProjetVR.Core.Game.GameEntities
{
    /// <summary>
    /// Classe Mush :
    /// Hérite de Mob, correspond aux champignons qui apparaissent sur la map
    /// </summary>
    public class Mush : Mob
    {
        /// <summary>
        /// Le constructeur attribue une vitesse aléatoire
        /// </summary>
        /// <param name="_s"></param>
        /// <param name="game"></param>
        public Mush(SpriteBatch _s, Microsoft.Xna.Framework.Game game) 
            : base(_s, game)
        {
            Sprite = new AnimationPlayer();
            int random_number = new Random().Next(60, 90);
            EntitySpeed = random_number;
            IsAlive = true;
        }

        public override void LoadContent(ContentManager c)
        {
            IdleAnimation = new Animation(c.Load<Texture2D>("idle3"), 0.1f, true, 6);
            RunAnimation = new Animation(c.Load<Texture2D>("run3"), 0.1f, true, 8);
            DeathAnimation = new Animation(c.Load<Texture2D>("death3"), 0.1f, false, 6);
            Sprite.PlayAnimation(IdleAnimation);
        }

        /// <summary>
        /// Méthode permettant d'afficher le Mush
        /// Même principe que la classe Player
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Movement == 2)
                flip = SpriteEffects.None;
            else if (Movement == 1)
                flip = SpriteEffects.FlipHorizontally;

            Sprite.Draw(gameTime, _sb, EntityPosition, flip);
        }
    }
}
