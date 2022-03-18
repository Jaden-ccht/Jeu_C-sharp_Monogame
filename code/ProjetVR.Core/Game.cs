using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjetVR.Core.GameEntities;
using Apos.Gui;
using Apos.Input;
using FontStashSharp;
using ProjetVR.Core.Game.GameEntities;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using ProjetVR.Core.Game.Levels;

namespace ProjetVR
{
    public class ProjetVRGame : Game
    {
        private Level currentLevel;

        Character Player;
        Gobelin gob;
        Bear bear;

        TiledMap _tiledMap1;
        TiledMap _tiledMap2;
        TiledMapRenderer _tiledMapRenderer1;
        TiledMapRenderer _tiledMapRenderer2;

        private string _name = string.Empty;
        private Texture2D _background;

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
            this.Player = new Character(_s, this);
            //this.gob = new Gobelin(_s, this);
            //this.bear = new Bear(_s, this);
            //gob.setEntityPosition(100, 100);
            Player.setEntityPosition(100, 100);
            //bear.setEntityPosition(200, 200);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _background = Content.Load<Texture2D>("bg");

            // Load Menu
            FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/dpcomic.ttf"));
            GuiHelper.Setup(this, fontSystem);
            _ui = new IMGUI();
            GuiHelper.CurrentIMGUI = _ui;

            //Load Entity
            //this.Player.LoadContent(Content);
            //this.gob.LoadContent(Content);
            //this.bear.LoadContent(Content);

            _tiledMap1 = Content.Load<TiledMap>("level1");
            _tiledMapRenderer1 = new TiledMapRenderer(GraphicsDevice, _tiledMap1);

            _tiledMap2 = Content.Load<TiledMap>("level2");
            _tiledMapRenderer2 = new TiledMapRenderer(GraphicsDevice, _tiledMap2);

            currentLevel = new Level(_tiledMap1, _tiledMapRenderer1, Player);

            currentLevel.Player.LoadContent(Content);
        }

        

        protected override void Update(GameTime gameTime)
        {
            MenuUpdate(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
            HandleInput(gameTime);
            if(_menu == Menu.Jeu)
            {
                currentLevel.MapRenderer.Update(gameTime);
                currentLevel.Player.Update(gameTime, keyboardState, currentLevel.Map);
                currentLevel.Update(gameTime);

                if (currentLevel.NextLevelReached)
                {
                    currentLevel.Player.EntityPosition = new Vector2(currentLevel.Player.EntityPosition.X, 500);
                    currentLevel = new Level(_tiledMap2, _tiledMapRenderer2, Player);
                }
            }
            //Player.Update(gameTime, keyboardState);
            
            //gob.Update(gameTime, character);
            //bear.Update(gameTime, character);

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

            if (_menu != Menu.Jeu)
            {
                _s.Draw(_background, new Vector2(0, 0), Color.White);
            }

            if (_menu == Menu.Jeu)
            {
                
                currentLevel.Draw(gameTime, _s);
                //Player.Draw(gameTime);
                //gob.Draw(gameTime);
                //bear.Draw(gameTime);
                //GraphicsDevice.Clear(Color.Black);
            }
            else
                _s.End();
                
            
            //_s.End();

            _ui.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            // get all of our input states
            keyboardState = Keyboard.GetState();
        }

        private void MenuUpdate(GameTime gameTime)
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
            
            MenuPanel.Pop();

            GuiHelper.UpdateCleanup();
        }
    }
}
