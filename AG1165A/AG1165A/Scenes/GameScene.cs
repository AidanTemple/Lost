#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections.Generic;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Game component which represents the game scene.
    /// </summary>
    public class GameScene : SceneManager
    {
        #region Private Members

        // Performs primitive-based rendering.
        private GraphicsDevice m_Graphics;

        // Enables a group of sprites to be drawn using the same settings.
        private SpriteBatch spriteBatch = null;

        // Represnts a new instance of a level.
        private Level level;

        private List<Stream> m_Streams;

        #endregion

        #region Properties

        /// <summary>
        /// Holds an instance of a level.
        /// </summary>
        public Level Level 
        {
            get { return level; }
            set { level = value; }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">Provides basic graphics device initialization, game logic and rendering code.</param>
        /// <param name="graphicsDevice">Performs primitive based rendering.</param>
        public GameScene(Game game, Main main, GraphicsDevice graphics)
            : base(game)
        {
            // Initialize the graphics object.
            m_Graphics = graphics;

            // Retrieve the SpriteBatch service.
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            m_Streams = new List<Stream>();

            // Load and parse tile layer from a txt file.
            Stream streamA = TitleContainer.OpenStream(ContentManager.Layer01);
            m_Streams.Add(streamA);

            // Load and parse tile layer from a txt file.
            Stream streamB = TitleContainer.OpenStream(ContentManager.Layer02);
            m_Streams.Add(streamB);

            if (m_Streams != null)
            {
                level = new Level(main, m_Streams, graphics, spriteBatch);
            }
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
            // Update the level.
            level.Update(gameTime);

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
            // Draw the level.
            level.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }

        #endregion
    }
}