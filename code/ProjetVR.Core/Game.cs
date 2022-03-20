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
        private Player Player;

        /// <summary>
        /// Niveau actuel
        /// </summary>
        private Level CurrentLevel;

        /// <summary>
        /// Liste de niveaux
        /// </summary>
        private List<Level> LevelList;

        /// <summary>
        /// Différentes map
        /// </summary>
        private TiledMap _tiledMap1, _tiledMap2, _tiledMap3;
        private TiledMapRenderer _tiledMapRenderer1, _tiledMapRenderer2, _tiledMapRenderer3;

        private string _name = string.Empty;
        private Texture2D _background, _bgwin, _bgloose;

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _s;
        private IMGUI _ui;
        private readonly ICondition _quit = new AnyCondition(new KeyboardCondition(Keys.Escape), new GamePadCondition(GamePadButton.Back, 0));

        public ProjetVRGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _menu = Menu.Main;
        }

        /// <summary>
        /// Initialise le spritebatch ainsi que le joueur
        /// </summary>
        protected override void Initialize()
        {
            Window.AllowUserResizing = false;
            
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 527;
            _graphics.ApplyChanges();

            _s = new SpriteBatch(GraphicsDevice);

            Player = new Player(_s, this);

            base.Initialize();
        }

        /// <summary>
        /// Permet de charger tout le contenu (textures, maps, etc.)
        /// </summary>
        protected override void LoadContent()
        {
            _background = Content.Load<Texture2D>("bg");
            _bgwin = Content.Load<Texture2D>("win");
            _bgloose = Content.Load<Texture2D>("loose");

            // Load Menu
            FontSystem fontSystem = FontSystemFactory.Create(GraphicsDevice, 2048, 2048);
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/dpcomic.ttf"));
            GuiHelper.Setup(this, fontSystem);
            _ui = new IMGUI();
            GuiHelper.CurrentIMGUI = _ui;

            // Chargement des niveaux et création des créatures
            LoadLevels();
            CreateMobs();

            // Chargement du contenu de chaque niveau
            foreach (Level lvl in LevelList)
            {
                lvl.LoadContent(Content);
            }
            Player.LoadContent(Content);
        }

        /// <summary>
        /// Méthode update :
        /// Logique du jeu, permettant le changement de niveau, la fin du jeu ou la transition entre les différents menus
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MenuUpdate(gameTime);

            if (_menu == Menu.Jeu)
            { 
                CurrentLevel.Update(gameTime);

                if (CurrentLevel.LevelEnded)
                {
                    _menu = Menu.Defeat;
                    CurrentLevel.LevelEnded = false;
                    ReloadGame();
                }

                if (CurrentLevel.NextLevelReached)
                {
                    CurrentLevel.SetMobPositions();
                    CurrentLevel.NextLevelReached = false;
                    CurrentLevel.PreviousLevelReached = false;
                    int currentLevelIndex = LevelList.IndexOf(CurrentLevel);
                    if(currentLevelIndex == 0)
                        Player.EntityPosition = new Vector2(CurrentLevel.Player.EntityPosition.X, 470);
                    else
                        Player.EntityPosition = new Vector2(35, CurrentLevel.Player.EntityPosition.Y);
                    CurrentLevel = LevelList[currentLevelIndex + 1];
                }
                if (CurrentLevel.PreviousLevelReached)
                {
                    CurrentLevel.SetMobPositions();
                    CurrentLevel.NextLevelReached = false;
                    CurrentLevel.PreviousLevelReached = false;
                    int currentLevelIndex = LevelList.IndexOf(CurrentLevel);
                    if (currentLevelIndex == 1)
                        Player.EntityPosition = new Vector2(CurrentLevel.Player.EntityPosition.X, 20);
                    else
                        Player.EntityPosition = new Vector2(960, CurrentLevel.Player.EntityPosition.Y);
                    CurrentLevel = LevelList[currentLevelIndex - 1];
                }
                if (CurrentLevel.LevelEndReached)
                {
                    CurrentLevel.LevelEndReached = false;
                    if ((LevelList[0].EnemyList.Count + LevelList[1].EnemyList.Count + LevelList[2].EnemyList.Count) == 0)
                    {
                        _menu = Menu.Win;
                        ReloadGame();
                    }
                }

            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Enum composée de tous les types de fenetres
        /// </summary>
        enum Menu
        {
            Main,
            PreJeu,
            Jeu,
            Quit, 
            Defeat,
            Win
        }
        private Menu _menu;

        /// <summary>
        /// Méthode permettant l'affichage des différentes fênetres et de leur contenu correspondant
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _s.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);


            if (_menu == Menu.Main || _menu == Menu.Quit || _menu == Menu.PreJeu)
            {
                _s.Draw(_background, new Vector2(0, 0), Color.White);

            }
            if (_menu == Menu.Win)
                _s.Draw(_bgwin, new Vector2(0, 0), Color.White);

            if (_menu == Menu.Defeat)
                _s.Draw(_bgloose, new Vector2(0, 0), Color.White);

            if (_menu == Menu.Jeu)
            {
                // Affichage du contenu du niveau actuel
                CurrentLevel.Draw(gameTime, _s);
            }
            else
                _s.End();
            _ui.Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Permet de recharger le jeu en créant de nouvelles créatures et en réinitialisant les propriétés du joueur
        /// </summary>
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
            CurrentLevel = LevelList[0];
        }

        /// <summary>
        /// Update des différents menus
        /// </summary>
        /// <param name="gameTime"></param>
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
            else if (_menu == Menu.Win)
            {
                Label.Put($"Felicitations {_name}, tu as reussi a sortir de ces bois !", 30, Color.Khaki, 0, false);
                if (Button.Put("Accueil", 30, Color.Khaki, 0, false).Clicked && _name.Length > 0) _menu = Menu.Main;
                if (Button.Put("Quitter", 30, Color.Khaki, 0, false).Clicked) _menu = Menu.Quit;
            }
            else if (_menu == Menu.Defeat)
            {
                Label.Put($"{_name}, veux-tu retenter ta chance dans ces dangereux bois ?", 30, Color.Khaki, 0, false);
                if (Button.Put("Accueil", 30, Color.Khaki, 0, false).Clicked && _name.Length > 0) _menu = Menu.Main;
                if (Button.Put("Quitter", 30, Color.Khaki, 0, false).Clicked) _menu = Menu.Quit;
            }

            MenuPanel.Pop();

            GuiHelper.UpdateCleanup();
        }

        /// <summary>
        /// Chargement des différents niveaux avec leur map correspondante
        /// </summary>
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

            LevelList = new List<Level>
            {
                lvl1,
                lvl2,
                lvl3
            };
            CurrentLevel = LevelList[0];
        }

        /// <summary>
        /// Création des créatures pour chaque niveau
        /// </summary>
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
