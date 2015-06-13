#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Game component which represents the start scene.
    /// </summary>
    public class WinScene : SceneManager
    {
        #region Private Members

        // Performs primitive-based rendering.
        private GraphicsDevice graphics;

        // Enables a group of sprites to be drawn using the same settings.
        private SpriteBatch spriteBatch = null;

        // Defines the window dimensions of a render-target.
        private Viewport viewport;

        // Array of players.
        private Player[] m_Players;

        // Used to store text.
        private string text;

        // Represents the player index
        private int i;

        #endregion

        #region Public Methods

        /// <summary>
        /// Array of available players.
        /// </summary>
        public Player[] Players 
        {
            get { return m_Players; }
            set { m_Players = value; }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">Provides basic graphics device initialization, game logic and rendering code.</param>
        /// <param name="graphicsDevice">Performs primitive based rendering.</param>
        public WinScene(Game game, GraphicsDevice graphicsDevice)
            : base(game)
        {
            // Assign the graphics device parameters.
            graphics = graphicsDevice;

            // Retrieve the SpriteBatch service.
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            // Assign the viewport parameters.
            viewport = spriteBatch.GraphicsDevice.Viewport;
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
            base.Update(gameTime);
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

            // Initialize the font.
            SpriteFont font = ContentManager.SelectedFont;

            // Check which player has the higher score and output
            // that to our text variable.
            if (m_Players[0].Score == m_Players[1].Score)
            {
                text = string.Format("You Draw!");
            }
            else
            {
                i = (m_Players[0].Score > m_Players[1].Score) ? 1 : 2;
                text = string.Format("Player {0} Wins!", i);
            }

            float positionY = spriteBatch.GraphicsDevice.Viewport.Height;

            // Measure the text string and position it in the center of the screen.
            Vector2 size = font.MeasureString(text);
            Vector2 center = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2,
                positionY - (size.Y * 6));

            // Render the text to the screen.
            spriteBatch.DrawString(font, text, center - (size / 2), Color.White);

            // Draw the background texture.
            spriteBatch.Draw(ContentManager.Statistics, Vector2.Zero, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
