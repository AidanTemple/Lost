#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace AG1165A
{
    public class GameTimer
    {
        #region Private Members

        // Represents the time remaining in the game.
        private TimeSpan m_TimeRemaining;

        // Used to warn the player time is running out.
        private TimeSpan m_WarningTime;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the time remaining in the game.
        /// </summary>
        public TimeSpan TimeRemaining
        {
            get { return m_TimeRemaining; }
        }

        /// <summary>
        /// Used to warn the player time is running out.
        /// </summary>
        public TimeSpan WarningTime
        {
            get { return m_WarningTime; }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameTimer()
        {
            // Set both the time remaining and warning time.
            m_TimeRemaining = TimeSpan.FromMinutes(3.0);
            m_WarningTime = TimeSpan.FromSeconds(10);
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the game timer each frame.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // Countdown from the total time remaining to zero.
            m_TimeRemaining -= gameTime.ElapsedGameTime;
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draw the game timer each frame.
        /// </summary>
        /// <param name="spriteBatch">Enables a group of sprites to be drawn using the same settings.</param>
        /// <param name="viewport">Defines the window dimensions of a render-target.</param>
        public void Draw(SpriteBatch spriteBatch, Viewport viewport)
        {
            // Create the game timer text string
            string timer = string.Format("{0}:{1}", m_TimeRemaining.Minutes.ToString("00"),
                m_TimeRemaining.Seconds.ToString("00"));

            // Position the game timer on screen
            Vector2 size = ContentManager.TimerFont.MeasureString(timer);
            Vector2 position = new Vector2((viewport.Width / 2) - (size.X / 2), 15);
            Vector2 temp = position;

            // Flash the game timer text color red when time remaining
            // is less than the warning time.
            Color color = (m_TimeRemaining > m_WarningTime || (int)m_TimeRemaining.TotalSeconds % 2 == 0)
                ? color = Color.White : color = Color.Red;

            // Draw the game timer text shadow
            spriteBatch.DrawString(ContentManager.TimerFont, timer, temp + new Vector2(3, 3), Color.Black);
            
            // Draw the game timer
            spriteBatch.DrawString(ContentManager.TimerFont, timer, position, color);
        }

        #endregion
    }
}