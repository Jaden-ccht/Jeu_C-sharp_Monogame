using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace ProjetVR.Core.GameEntities
{
    public class Character : Entity
    {
        public Game game { get; set; }

        public Character(Game game) : base(game)
        {
            this.game = game;
            this.entitySpeed = 100f;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.Z))
                this.entityPosition = new Vector2(this.entityPosition.X, this.entityPosition.Y - this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
                this.entityPosition = new Vector2(this.entityPosition.X, this.entityPosition.Y + this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.Q))
                this.entityPosition = new Vector2(this.entityPosition.X - this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, this.entityPosition.Y);

            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
                this.entityPosition = new Vector2(this.entityPosition.X + this.entitySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, this.entityPosition.Y);

            base.Update(gameTime);
        }
    }
}
