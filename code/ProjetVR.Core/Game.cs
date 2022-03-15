using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjetVR.Core.GameEntities;
using Apos.Gui;
using Apos.Input;
using FontStashSharp;
using ProjetVR.Core.Game.GameEntities;

namespace ProjetVR
{
    public class ProjetVRGame : Game
    {
        Character character;
        Gobelin gob;
        Bear bear;

        private KeyboardState keyboardState;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _s;
        private IMGUI _ui;
        private ICondition _quit =
            new AnyCondition(
                new KeyboardCondition(Keys.Escape),
                new GamePadCondition(GamePadButton.Back, 0)
            );

        public ProjetVRGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.AllowUserResizing = false;
            
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 527;
            _graphics.ApplyChanges();

            _s = new SpriteBatch(GraphicsDevice);
            this.character = new Character(_s, this);
            this.gob = new Gobelin(_s, this);
            this.bear = new Bear(_s, this);

            gob.setEntityPosition(100, 100);
            character.setEntityPosition(100, 100);
            bear.setEntityPosition(200, 200);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //_s = new SpriteBatch(GraphicsDevice);
            _background = Content.Load<Texture2D>("bg");

            // TODO: use this.Content to load your game content here
            FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/dpcomic.ttf"));

            GuiHelper.Setup(this, fontSystem);
            _ui = new IMGUI();
            GuiHelper.CurrentIMGUI = _ui;

            this.character.LoadContent(Content);
            this.character.entityTexture = Content.Load<Texture2D>("player");
            this.gob.LoadContent(Content);
            this.bear.LoadContent(Content);
        }

        private string _name = string.Empty;
        private Texture2D _background;

        protected override void Update(GameTime gameTime)
        {
            GuiHelper.UpdateSetup(gameTime);

            if (_quit.Pressed())
                Exit();

            _ui.UpdateAll(gameTime);

            MenuPanel.Push().XY = new Vector2(50, 50);
            if (_menu == Menu.Main)
            {
                Label.Put("Projet Realite Virtuelle", 45, Color.Khaki, 0, false);
                Label.Put("");
                if (_name == string.Empty)
                    Label.Put("Bienvenue !", 38, Color.Khaki, 0, false);
                else
                    Label.Put($"Votre pseudo est {_name} ", 30, Color.Khaki, 0, false);
                Label.Put("");
                if (Button.Put("Jouer", 30, Color.Khaki, 0, false).Clicked) _menu = Menu.PreJeu;
                Label.Put("");
                if (Button.Put("Quitter", 30, Color.Khaki, 0, false).Clicked) _menu = Menu.Quit;
            }
            else if (_menu == Menu.PreJeu)
            {
                Label.Put("Entrez votre pseudo", 30, Color.Khaki, 0, false);
                Textbox.Put(ref _name, 30, Color.Khaki, 0, false);
                Label.Put("");
                if (Button.Put("Continuer", 30, Color.Khaki, 0, false).Clicked && _name.Length > 0) _menu = Menu.Jeu;
                if (Button.Put("Retour", 30, Color.Khaki, 0, false).Clicked) _menu = Menu.Main;
            }
            else if (_menu == Menu.Quit)
            {
                Label.Put("Voulez-vous quittez le jeu ?", 30, Color.Khaki, 0, false);
                Label.Put("");
                if (Button.Put("Oui", 30, Color.Khaki, 0, false).Clicked) Exit();

                if (Button.Put("Non", 30, Color.Khaki, 0, false).Clicked) _menu = Menu.Main;
            }
            else if (_menu == Menu.Jeu)
            {
                Label.Put("Voici la vue temporaire du jeu", 30, Color.Khaki, 0, false);
            }
            MenuPanel.Pop();

            GuiHelper.UpdateCleanup();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            HandleInput(gameTime);
            character.Update(gameTime, keyboardState);
            gob.Update(gameTime, character);
            bear.Update(gameTime, character);

            base.Update(gameTime);
        }

        enum Menu
        {
            Main,
            PreJeu,
            Jeu,
            Quit
        }
        Menu _menu = Menu.Main;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            _s.Draw(_background, new Vector2(0, 0), Color.White);
            //a utiliser par la suite

            

            if (_menu == Menu.Jeu)
            {
                character.Draw(gameTime);
                gob.Draw(gameTime);
                bear.Draw(gameTime);
                GraphicsDevice.Clear(Color.Black);
            }
                
            
            //_spriteBatch.Draw(character.entityTexture, character.entityPosition, this.rectangleSource, Color.White, 0f, new Vector2(character.entityTexture.Width / 2, character.entityTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            _s.End();

            _ui.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            // get all of our input states
            keyboardState = Keyboard.GetState();
        }
    }
}
