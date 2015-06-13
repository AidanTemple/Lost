#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Game component which represents the start scene.
    /// </summary>
    public class StartScene : SceneManager
    {
        #region Private Members

        // Enables a group of sprites to be drawn using the same settings.
        private SpriteBatch spriteBatch = null;

        // Start scene font
        private SpriteFont m_Font;

        // Used to indicate if a controller is connected.
        private bool m_IsConnected;

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">Provides basic graphics device initialization, game logic and rendering code.</param>
        /// <param name="graphicsDevice">Performs primitive based rendering.</param>
        public StartScene(Game game, GraphicsDevice graphicsDevice)
            : base(game)
        {
            // Retrieve the SpriteBatch service.
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            // Assign the start scene font.
            m_Font = ContentManager.SelectedFont;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Used to show the scene.
        /// </summary>
        public override void ShowScene()
        {
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
            // Handle and update input
            UpdateInput(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Used to handle and update any user input
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateInput(GameTime gameTime)
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
            // Initialize the start scene texture.
            var texture = ContentManager.Controller_Buttons;

            // Position the texture on the screen.
            Vector2 center = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2 - (83 / 2),
                spriteBatch.GraphicsDevice.Viewport.Height - 150);

            // Position text around the texture.
            Vector2 posA = new Vector2(center.X - Extensions.MeasureString(m_Font, "Press ").X, center.Y + 27);
            Vector2 posB = new Vector2(center.X + 80, center.Y + 27);

            // Position and center keyboard instructions ont he screen.
            Vector2 centerA = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2,
                spriteBatch.GraphicsDevice.Viewport.Height - 150);

            Vector2 position = new Vector2(centerA.X - (Extensions.MeasureString(m_Font, 
                "Press Space to Begin").X / 2), centerA.Y);

            spriteBatch.Begin();

            spriteBatch.Draw(ContentManager.SplashScreen, Vector2.Zero, Color.White);

            // Draw the controller input instructions if one is connected..
            if (m_IsConnected)
            {
                spriteBatch.DrawString(m_Font, "Press ", posA, Color.White);
                spriteBatch.Draw(texture, center, ContentManager.Start_Button, Color.White);
                spriteBatch.DrawString(m_Font, " to Begin.", posB, Color.White);
            }
            // ..otherwise draw input instractions for the keyboard.
            else
            {
                spriteBatch.DrawString(m_Font, "Press Space to Begin", position, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
