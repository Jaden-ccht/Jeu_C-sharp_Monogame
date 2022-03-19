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
using System.Collections.Generic;
using System;

namespace ProjetVR
{
    public class ProjetVRGame : Game
    {
        private Character Player;

        private Level currentLevel;

        private List<Level> LevelList;

        private TiledMap _tiledMap1;
        private TiledMap _tiledMap2;
        private TiledMap _tiledMap3;
        private TiledMapRenderer _tiledMapRenderer1;
        private TiledMapRenderer _tiledMapRenderer2;
        private TiledMapRenderer _tiledMapRenderer3;

        private string _name = string.Empty;
        private Texture2D _background;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _s;
        private IMGUI _ui;
        private ICondition _quit = new AnyCondition(new KeyboardCondition(Keys.Escape), new GamePadCondition(GamePadButton.Back, 0));

        public ProjetVRGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _menu = Menu.Main;
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

            LoadLevels();
            CreateMobs();

            foreach (Level lvl in LevelList)
            {
                lvl.LoadContent(Content);
            }
            Player.LoadContent(Content);
        }

        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MenuUpdate(gameTime);

            if (_menu == Menu.Jeu)
            { 
                currentLevel.Update(gameTime);

                if (currentLevel.LevelEnded)
                {
                    _menu = Menu.Main;
                    currentLevel.LevelEnded = false;
                    ReloadGame();
                }

                if (currentLevel.NextLevelReached)
                {
                    currentLevel.SetMobPositions();
                    currentLevel.NextLevelReached = false;
                    currentLevel.PreviousLevelReached = false;
                    int currentLevelIndex = LevelList.IndexOf(currentLevel);
                    if(currentLevelIndex == 0)
                        Player.EntityPosition = new Vector2(currentLevel.Player.EntityPosition.X, 470);
                    else
                        Player.EntityPosition = new Vector2(35, currentLevel.Player.EntityPosition.Y);
                    currentLevel = LevelList[currentLevelIndex + 1];
                }
                if (currentLevel.PreviousLevelReached)
                {
                    currentLevel.SetMobPositions();
                    currentLevel.NextLevelReached = false;
                    currentLevel.PreviousLevelReached = false;
                    int currentLevelIndex = LevelList.IndexOf(currentLevel);
                    if (currentLevelIndex == 1)
                        Player.EntityPosition = new Vector2(currentLevel.Player.EntityPosition.X, 20);
                    else
                        Player.EntityPosition = new Vector2(960, currentLevel.Player.EntityPosition.Y);
                    currentLevel = LevelList[currentLevelIndex - 1];
                }
                if (currentLevel.LevelEndReached)
                {
                    currentLevel.LevelEndReached = false;
                    if ((LevelList[0].EnemyList.Count + LevelList[1].EnemyList.Count + LevelList[2].EnemyList.Count) == 0)
                    {
                        _menu = Menu.Main;
                        ReloadGame();
                    }
                }

            }
            base.Update(gameTime);
        }


        enum Menu
        {
            Main,
            PreJeu,
            Jeu,
            Quit
        }
        private Menu _menu;

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
            

            if (_menu != Menu.Jeu)
            {
                _s.Draw(_background, new Vector2(0, 0), Color.White);
                
            }

            if (_menu == Menu.Jeu)
            {
                currentLevel.Draw(gameTime, _s);
            }
            else
                _s.End();
            _ui.Draw(gameTime);

            base.Draw(gameTime);
        }

        private void ReloadGame()
        {
            Player.IsAlive = true;
            Player.EntityPosition = new Vector2(480, 288);
            foreach (Level lvl in LevelList)
            {
                lvl.EnemyList = new List<Mob>();
            }
            CreateMobs();
            foreach (Level lvl in LevelList)
            {
                lvl.LoadContent(Content);
            }
            currentLevel = LevelList[0];
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
                Label.Put("Mystic Woods", 50, Color.Khaki, 0, false);
                Label.Put("");
                if (_name == string.Empty)
                    Label.Put("Bienvenue !", 38, Color.Khaki, 0, false);
                else
                    Label.Put($"Arriveras-tu a sortir des bois {_name} ?", 30, Color.Khaki, 0, false);
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

        private void LoadLevels()
        {
            _tiledMap1 = Content.Load<TiledMap>("level1");
            _tiledMapRenderer1 = new TiledMapRenderer(GraphicsDevice, _tiledMap1);
            Level lvl1 = new Level(_tiledMap1, _tiledMapRenderer1, Player);


            _tiledMap2 = Content.Load<TiledMap>("level2");
            _tiledMapRenderer2 = new TiledMapRenderer(GraphicsDevice, _tiledMap2);
            Level lvl2 = new Level(_tiledMap2, _tiledMapRenderer2, Player);

            _tiledMap3 = Content.Load<TiledMap>("level3");
            _tiledMapRenderer3 = new TiledMapRenderer(GraphicsDevice, _tiledMap3);
            Level lvl3 = new Level(_tiledMap3, _tiledMapRenderer3, Player);

            LevelList = new List<Level>();
            LevelList.Add(lvl1);
            LevelList.Add(lvl2);
            LevelList.Add(lvl3);
            currentLevel = LevelList[0];
        }

        private void CreateMobs()
        {
            LevelList[0].EnemyList.AddRange(new List<Mob>
            {
                new Gobelin(_s, this),
                new Gobelin(_s, this),
                new Mush(_s, this),
            });

            LevelList[1].EnemyList.AddRange(new List<Mob>
            {
                new Gobelin(_s, this),
                new Gobelin(_s, this),
                new Gobelin(_s, this),
                new Mush(_s, this),
                new Mush(_s, this),
                new Mush(_s, this),
            });


            LevelList[2].EnemyList.AddRange(new List<Mob>
            {
                new Gobelin(_s, this),
                new Gobelin(_s, this),
                new Gobelin(_s, this),
                new Gobelin(_s, this),
                new Mush(_s, this),
                new Mush(_s, this),
                new Mush(_s, this),
                new Mush(_s, this),
            });


            foreach (Level lvl in LevelList)
                lvl.SetMobPositions();
        }
    }
}
