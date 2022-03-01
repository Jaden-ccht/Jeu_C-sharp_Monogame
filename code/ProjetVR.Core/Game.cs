using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjetVR.Core.GameEntities;

namespace ProjetVR
{
    public class ProjetVRGame : Game
    {
        Character character;

        private KeyboardState keyboardState;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public ProjetVRGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            this.character = new Character();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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

            this.character.LoadContent(Content);
            this.character.entityTexture = Content.Load<Texture2D>("player");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            HandleInput(gameTime);
            character.Update(gameTime, keyboardState);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            //a utiliser par la suite
            character.Draw(gameTime, _spriteBatch);
            //_spriteBatch.Draw(character.entityTexture, character.entityPosition, this.rectangleSource, Color.White, 0f, new Vector2(character.entityTexture.Width / 2, character.entityTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            // get all of our input states
            keyboardState = Keyboard.GetState();
        }
    }
}
