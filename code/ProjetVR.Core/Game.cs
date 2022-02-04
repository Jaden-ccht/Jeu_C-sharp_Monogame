using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjetVR.Core.GameEntities;

namespace ProjetVR
{
    public class ProjetVRGame : Game
    {
        Character character;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Rectangle rectangleSource;

        public ProjetVRGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            this.character = new Character(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.rectangleSource = new Rectangle(16, 22, 32, 32);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            this.character.entityTexture = Content.Load<Texture2D>("player");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            character.Update(gameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LawnGreen);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(character.entityTexture, character.entityPosition, this.rectangleSource, Color.White, 0f, new Vector2(character.entityTexture.Width / 2, character.entityTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            _spriteBatch.End();

            character.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
