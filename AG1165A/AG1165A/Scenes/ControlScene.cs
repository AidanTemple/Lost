#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Game component which represents the control scene.
    /// </summary>
    public class ControlScene : SceneManager
    {
        #region Private Members

        // Enables a group of sprites to be drawn using the same settings.
        private SpriteBatch spriteBatch = null;

        // Used to indicate if a controller is connected.
        private bool m_IsConnected;

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">Provides basic graphics device initialization, game logic and rendering code.</param>
        /// <param name="graphicsDevice">Performs primitive based rendering.</param>
        public ControlScene(Game game, GraphicsDevice graphics)
            : base(game)
        {
            // Retrieve the SpriteBatch service.
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
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
            spriteBatch.Begin();

            if (m_IsConnected)
            {
                // Draw the control instructions texture
                spriteBatch.Draw(ContentManager.ControllerControls, Vector2.Zero, Color.White);
            }
            else
            {
                spriteBatch.Draw(ContentManager.KeyboardControls, Vector2.Zero, Color.White);
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
