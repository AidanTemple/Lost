#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace AG1165A
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game
    {
        #region Private Members

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SpriteFont unselected;
        private SpriteFont selected;

        private SceneManager sceneManager;
        private StartScene startScene;

        #endregion

        #region Public Members

        public MenuScene menuScene;
        public ControlScene controlScene;
        public GameScene gameScene;
        public WinScene winScene;
        public CreditScene creditScene;

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
#if DEBUG
            this.graphics.IsFullScreen = false;
#else
            this.graphics.IsFullScreen = true;
#endif
            this.graphics.PreferMultiSampling = false;
            this.graphics.SynchronizeWithVerticalRetrace = true;
            this.graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Services.AddService(typeof(SpriteBatch), spriteBatch);

            ContentManager.Load(Content);

            unselected = ContentManager.UnselectedFont;
            selected = ContentManager.SelectedFont;

            startScene = new StartScene(this, graphics.GraphicsDevice);
            Components.Add(startScene);

            menuScene = new MenuScene(this, unselected, selected);
            Components.Add(menuScene);

            controlScene = new ControlScene(this, graphics.GraphicsDevice);
            Components.Add(controlScene);

            gameScene = new GameScene(this, this, graphics.GraphicsDevice);
            Components.Add(gameScene);

            winScene = new WinScene(this, graphics.GraphicsDevice);
            Components.Add(winScene);

            creditScene = new CreditScene(this, graphics.GraphicsDevice);
            Components.Add(creditScene);

            startScene.ShowScene();
            sceneManager = startScene;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
        }

        #endregion

        #region Helper Methods

        public void SwitchScene(SceneManager manager)
        {
            sceneManager.HideScene();
            sceneManager = manager;
            manager.ShowScene();
        }

        private void UnloadScene(SceneManager scene)
        {
            scene = null;
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            UpdateInput();

            base.Update(gameTime);
        }

        private void UpdateInput()
        {
            PlayerIndex throwAway;

            for (int i = 0; i < 4; i++)
            {
                if (sceneManager == startScene)
                {
                    if (InputHandler.WasButtonPressed((PlayerIndex)i, Buttons.Start, out throwAway)
                        || InputHandler.WasKeyPressed(Keys.Space))
                    {
                        AudioHandler.PlaySoundEffect(ContentManager.MenuSelectSound);

                        SwitchScene(menuScene);
                    }
                }
                else if (sceneManager == menuScene)
                {
                    if (InputHandler.IsHoldingButton((PlayerIndex)i, Buttons.A, out throwAway)
                        || InputHandler.IsHoldingKey(Keys.Enter))
                    {
                        AudioHandler.PlaySoundEffect(ContentManager.MenuSelectSound);

                        switch (menuScene.Index)
                        {
                            case 0:
                                gameScene = new GameScene(this, this, graphics.GraphicsDevice);
                                Components.Add(gameScene);

                                SwitchScene(gameScene);
                                break;

                            case 1:
                                SwitchScene(controlScene);
                                break;

                            case 2:
                                Exit();
                                break;
                        }
                    }
                }
                else if (sceneManager == controlScene)
                {
                    if (InputHandler.IsHoldingButton((PlayerIndex)i, Buttons.B, out throwAway)
                        || InputHandler.IsHoldingKey(Keys.Back))
                    {
                        AudioHandler.PlaySoundEffect(ContentManager.MenuBackSound);

                        SwitchScene(menuScene);
                    }
                }
                else if (sceneManager == gameScene)
                {
                    if (InputHandler.IsHoldingButton((PlayerIndex)i, Buttons.Start, out throwAway)
                        || InputHandler.IsHoldingKey(Keys.Escape) && !gameScene.Level.IsGamePaused)
                    {
                        gameScene.Level.IsGamePaused = true;
                    }

                    if (InputHandler.IsHoldingButton((PlayerIndex)i, Buttons.B, out throwAway)
                        || InputHandler.IsHoldingKey(Keys.Back) && gameScene.Level.IsGamePaused)
                    {
                        AudioHandler.StopSoundEffect(ContentManager.ThemeInstance);
                        
                        // Discard the game scene, we do this so we can reset the game
                        // the next time we want to play.
                        UnloadScene(gameScene);

                        SwitchScene(menuScene);
                    }

                    if (InputHandler.IsHoldingButton((PlayerIndex)i, Buttons.A, out throwAway)
                        || InputHandler.IsHoldingKey(Keys.Enter) && gameScene.Level.IsGamePaused)
                    {
                        gameScene.Level.IsGamePaused = false;
                    }
                }
                else if (sceneManager == winScene)
                {
                    if (InputHandler.IsHoldingButton((PlayerIndex)i, Buttons.B, out throwAway)
                        || InputHandler.IsHoldingKey(Keys.Back))
                    {
                        AudioHandler.PlaySoundEffect(ContentManager.MenuBackSound);

                        SwitchScene(menuScene);
                    }
                }
                else if (sceneManager == creditScene)
                {
                    if (InputHandler.IsHoldingButton((PlayerIndex)i, Buttons.B, out throwAway)
                        || InputHandler.IsHoldingKey(Keys.Back))
                    {
                        AudioHandler.PlaySoundEffect(ContentManager.MenuBackSound);

                        SwitchScene(menuScene);
                    }
                }
            }

            InputHandler.Update();
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

        #endregion
    }
}