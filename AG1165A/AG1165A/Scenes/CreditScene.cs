#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Game component which represents the credit scene.
    /// </summary>
    public class CreditScene : SceneManager
    {
        #region Private Members

        // Enables a group of sprites to be drawn using the same settings.
        private SpriteBatch spriteBatch = null;

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">Provides basic graphics device initialization, game logic and rendering code.</param>
        /// <param name="graphicsDevice">Performs primitive based rendering.</param>
        public CreditScene(Game game, GraphicsDevice graphics)
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
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Allows the game component draw your content in game screen
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            // Draw the credits texture.
            spriteBatch.Draw(ContentManager.Credits, Vector2.Zero, Color.White);

            base.Draw(gameTime);

            spriteBatch.End();
        }

        #endregion
    }
}
