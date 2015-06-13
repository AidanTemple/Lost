#region Using Statements
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endregion

namespace AG1165A
{
    public class Countdown
    {
        #region Private Members

        // Text to display each time the countdown is updated.
        private string m_Text;

        // Array of text to display each time the countdown is updated.
        private string[] m_CountdownText;

        // Maximum number of times the countdown should blink.
        private int m_Blinks;

        // The amount the countdown text fades out each frame.
        private float m_FadeValue;

        // Scale of the countdown text.
        private float m_Scale;

        private float m_CountBlinkTime;
        private float m_CurrentCountBlinkTime;

        private bool m_CountBlink;

        // Used to enable the countdown.
        private bool m_EnableCountDown;

        #endregion

        #region Properties

        /// <summary>
        /// Used to determine if the countdown is active.
        /// </summary>
        public bool IsCountingDown { get; set; }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        public Countdown()
        {
            m_Text = "";

            m_CountdownText = new string[]
            {
                "3", "2", "1", "Go!", ""
            };

            m_Blinks = 0;
            m_FadeValue = 1f;

            m_Scale = 1f;
            m_CountBlinkTime = 0.8f;

            m_CountBlink = false;
            m_EnableCountDown = true;
        }

        #endregion

        #region Update

        /// <summary>
        /// Used to update the countdown each frame.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // If the countdown has been enabled...
            if (m_EnableCountDown)
            {
                // ...then begin countdowning down.
                IsCountingDown = true;

                // Increment the current blink time by the amount of time elapsed
                // since the last update call.
                m_CurrentCountBlinkTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // If current blink time is greater than blink time / 2 then increase 
                // the scale of the text by 1 pixel per frame and fade out.
                if (m_CurrentCountBlinkTime > (m_CountBlinkTime / 2))
                {
                    m_Scale += 1;
                    m_FadeValue -= 0.04f;
                }

                // If current blink time is greater than blink time then
                // reset the countdown.
                if (m_CurrentCountBlinkTime > m_CountBlinkTime)
                {
                    m_CountBlink = !m_CountBlink;
                    m_CurrentCountBlinkTime -= m_CountBlinkTime;
                    m_CountBlinkTime = 1.8f;

                    m_Text = m_CountdownText[m_Blinks];

                    if (m_Blinks < 3)
                    {
                        AudioHandler.PlaySoundEffect(ContentManager.CountdownShort, 1f, 0f, 0f);
                    }
                    else
                    {
                        AudioHandler.PlaySoundEffect(ContentManager.CountdownLongInstance, 1f, 0f, 0f);
                    }
                    
                    if(m_Blinks > 4)
                    {
                        AudioHandler.StopSoundEffect(ContentManager.CountdownLongInstance);
                    }

                    m_Blinks += 1;
                    m_Scale = 1f;
                    m_FadeValue = 1f;
                }

                // Once we reach the end of our countdown disable it.
                if (m_Blinks > 4)
                {
                    m_EnableCountDown = false;
                    IsCountingDown = false;
                }
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws a countdown timer on a specified viewport.
        /// </summary>
        /// <param name="spriteBatch">Enables a group of sprites to be drawn using the same settings.</param>
        /// <param name="viewport">Gets or sets a viewport identifying the portion of the render target to receive draw calls. </param>
        public void Draw(SpriteBatch spriteBatch, Viewport viewport)
        {
            // Returns the width and height of a string as a vector.
            Vector2 size = ContentManager.CountdownFont.MeasureString(m_Text);

            // Set the countdowns position to the center of the viewport
            Vector2 position = new Vector2((viewport.Width / 2), 
                (viewport.Height / 2));

            // Create a new vector which represents a shadows positon and whos 
            // position matches the position of the floating score. 
            Vector2 shadow = position;

            if (IsCountingDown)
            {
                // Draws the countdown offset by 3 pixels in the X, y axis. Used to represent a 
                // shadow and to give depth to the countdown.
                spriteBatch.DrawString(ContentManager.CountdownFont, m_Text, shadow + new Vector2(3, 3), Color.Black * m_FadeValue,
                    0f, new Vector2(size.X / 2, size.Y / 2), m_Scale, SpriteEffects.None, 0f);

                // Draws the countdown in front of the shadow but with no 3 pixel offset i.e. 
                // shift up and left 3 pixels.
                spriteBatch.DrawString(ContentManager.CountdownFont, m_Text, position, Color.White * m_FadeValue,
                    0f, new Vector2(size.X / 2, size.Y / 2), m_Scale, SpriteEffects.None, 0f);
            }
        }

        #endregion
    }
}