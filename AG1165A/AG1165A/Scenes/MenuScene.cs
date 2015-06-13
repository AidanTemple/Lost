#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Game component which represents the menu scene.
    /// </summary>
    public class MenuScene : SceneManager
    {
        #region Private Members

        // Font used in the menu.
        private SpriteFont font;

        // Menu background.
        private Texture2D background;

        // Defines the window dimensions of a render-target.
        private Viewport viewport;

        // Used to indicate if a controller is connected.
        private bool m_IsConnected;

        #endregion

        #region Protected Members

        // Enables a group of sprites to be drawn using the same settings.
        protected SpriteBatch spriteBatch = null;

        // Represents a menu component.
        protected MenuComponent menu;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the index of a menu element.
        /// </summary>
        public int Index
        {
            get { return menu.Index; }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">Provides basic graphics device initialization, game logic and rendering code.</param>
        /// <param name="unselected">Represents the menu font for unselected menu elements.</param>
        /// <param name="selected">Represents the menu font for selected menu elements.</param>
        public MenuScene(Game game, SpriteFont unselected, SpriteFont selected)
            : base(game)
        {
            // Retrieve the SpriteBatch service.
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            // Assign the viewport parameters.
            viewport = spriteBatch.GraphicsDevice.Viewport;

            // Initialize the background textures.
            background = ContentManager.SplashScreen;

            // Initialize the menu elements.
            string[] items = { "New Game", "Controls", "Quit" };

            // Initialize and add a new menu component.
            menu = new MenuComponent(game, unselected, selected);
            menu.SetMenuItems(items);
            Components.Add(menu);

            // Initialize the menu font.
            font = ContentManager.UnselectedFont;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Used to show the scene.
        /// </summary>
        public override void ShowScene()
        {
            menu.Position = Vector2.Zero;

            menu.Visible = false;
            menu.Enabled = false;

            base.ShowScene();
        }

        /// <summary>
        /// Used to hide the scene.
        /// </summary>
        public override void HideScene()
        {
            base.HideScene();
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the start scene.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (!menu.Visible)
            {
                menu.Visible = true;
                menu.Enabled = true;
            }

            UpdateInput();

            base.Update(gameTime);
        }

        /// <summary>
        /// Used to handle and update any user input
        /// </summary>
        private void UpdateInput()
        {
            // Retrieve the current state of available controllers.
            GamePadState gamePadState = InputHandler.GamePadStates[(int)PlayerIndex.One];

            // Determines whether a controller is connected.
            m_IsConnected = (gamePadState.IsConnected);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the start scene.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Update the view so that it fits the entire window.
            spriteBatch.GraphicsDevice.Viewport = viewport;

            spriteBatch.Begin();

            // Draw the menu background.
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            if (m_IsConnected)
            {
                spriteBatch.Draw(ContentManager.ControllerSelectScreen, Vector2.Zero, Color.White);
            }
            else
            {
                spriteBatch.Draw(ContentManager.KeyboardSelectScreen, Vector2.Zero, Color.White);
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}