#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Represents a score which floats beside a player 
    /// and which can fade in/out.
    /// </summary>
    public class FloatingScore
    {
        #region Private Members

        // Position of the score text.
        private Vector2 m_Position;

        // The amount which the score text should 
        // be faded out each frame.
        private float m_FadeValue;

        // Position of the score text in the Y
        // axis. Used to move the text up the screen.
        private float m_PositionY;

        private TimeSpan m_PointTimer;
        private TimeSpan m_FadeTime;

        // Color of the score text.
        private Color m_Color;

        // Color of te score text shadow.
        private Color m_Shadow;

        #endregion

        #region Properties

        /// <summary>
        /// Timer used to indicate when the score 
        /// should be updated.
        /// </summary>
        public TimeSpan PointTimer
        {
            get { return m_PointTimer; }
            set { m_PointTimer = value; }
        }

        /// <summary>
        /// Position of the score.
        /// </summary>
        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FloatingScore()
        {
            // Initialise the scores Y coordinate to zero, this
            // is used to move the score as it fades out.
            m_PositionY = 0;

            // Set the fade value to 1 i.e. opaque.
            m_FadeValue = 1f;

            // Initialise the fade timer to 1.5 secs.
            m_FadeTime = TimeSpan.FromSeconds(1.25);

            // Set the color of the score and its shadow.
            m_Color = new Color(0, 205, 14);
            m_Shadow = new Color(64, 111, 64);
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the position and state of the floating score each frame.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            if (m_PointTimer < m_FadeTime)
            {
                // Decrease the fade value each frame. This will cause
                // the score to fade out.
                m_FadeValue -= 0.015f;
            }
            else
            {
                // Reset the fade value to 1
                m_FadeValue = 1f;

                // Reset the scores Y coordinate to its initial value.
                m_PositionY = 0;
            }

            // Move the floating score up 3 pixels per frame in the Y axis.
            m_PositionY -= 3;
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws a floating score
        /// </summary>
        /// <param name="spriteBatch">Enables a group of sprites to be drawn using the same settings.</param>
        /// <param name="score">Represents a players score</param>
        public void Draw(SpriteBatch spriteBatch, int score)
        {
            string text = string.Format("+{0}", score);

            // Returns the width and height of a string as a vector
            Vector2 size = ContentManager.ScoreFont.MeasureString(text);

            // Assign the floating score position the position of the player
            Vector2 position = m_Position;

            // Create a new vector which represents a shadows positon and whos 
            // position matches the position of the floating score.
            Vector2 shadow = position;

            // Draw the floating score and offset it by 2 pixels in the X, Y axis. Used to represent a 
            // shadow and to give depth to the floating score.
            spriteBatch.DrawString(ContentManager.ScoreFont, text, shadow + new Vector2(2, (m_PositionY + 2)), m_Shadow * m_FadeValue,
                0f, new Vector2((size.X * 1.5f), (size.Y / 2f)), 0.8f, SpriteEffects.None, 0f);

            // Draw the floating score in front of the shadow but with no 2 pixel offset i.e.
            // shifted up and left 3 pixels.
            spriteBatch.DrawString(ContentManager.ScoreFont, text, position + new Vector2(0, m_PositionY), m_Color * m_FadeValue,
                0f, new Vector2((size.X * 1.5f), (size.Y / 2f)), 0.8f, SpriteEffects.None, 0f);
        }

        #endregion
    }
}