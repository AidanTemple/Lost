#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Represents a players identifier 
    /// </summary>
    public class Identifier
    {
        #region Private Members

        private Texture2D m_Circle;
        private Texture2D m_Arrow;

        private Vector2 m_Position;
        private Vector2 m_Origin;

        private float m_Rotation;

        private Color m_Color;

        private bool m_IsArrowVisible;

        private int m_Score;

        #endregion

        #region Properties

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public Color Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        public bool IsArrowVisible
        {
            get { return m_IsArrowVisible; }
            set { m_IsArrowVisible = value; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="playerIndex">Specifies the game controller associated with a player.</param>
        public Identifier(PlayerIndex playerIndex)
        {
            // Assign player one the yellow identifier and arrow
            if (playerIndex == PlayerIndex.One)
            {
                m_Circle = ContentManager.Identifier_Yellow;
                m_Arrow = ContentManager.Identifier_Arrow_Yellow;
            }

            // Assign player two the blue identifier and arrow
            if (playerIndex == PlayerIndex.Two)
            {
                m_Circle = ContentManager.Identifier_Blue;
                m_Arrow = ContentManager.Identifier_Arrow_Blue;
            }

            if (m_Circle != null)
            {
                // Specify the origin of the identifier, in this case the
                // center point of the texture.
                m_Origin = new Vector2(m_Circle.Width / 2, m_Circle.Height / 2);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the player identifier
        /// </summary>
        /// <param name="posA">Position of the player</param>
        /// <param name="posB">Position of the opponent holding the key</param>
        public void Update(Vector2 posA, Vector2 posB)
        {
            // Update the identifiers rotation so that it always points
            // towards the player holding the key.
            m_Rotation = (float)Math.Atan2(posA.Y - posB.Y, posA.X - posB.X);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the player identifier circle
        /// </summary>
        /// <param name="spriteBatch">Enables a group of sprites to be drawn using the same settings.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draws the players identifier circle
            spriteBatch.Draw(m_Circle, m_Position, null, Color.White, 0f, m_Origin,
                1f, SpriteEffects.None, 0f);

            DrawScore(spriteBatch);

            if (m_IsArrowVisible)
            {
                // Draws an arrow which points at the player holding the key
                spriteBatch.Draw(m_Arrow, m_Position, null, Color.White, m_Rotation,
                    m_Origin, 1f, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Draws the players score
        /// </summary>
        /// <param name="spriteBatch">Enables a group of sprites to be drawn using the same settings.</param>
        private void DrawScore(SpriteBatch spriteBatch)
        {
            string score = string.Format("{0}", m_Score.ToString("0000"));

            // Returns the width and height of a string as a vector
            Vector2 size = ContentManager.IdentifierFont.MeasureString(score);

            // Positions and draws the players score
            spriteBatch.DrawString(ContentManager.IdentifierFont, score, m_Position, Color.Black,
                0f, new Vector2(size.X / 2f, size.Y * -3.18f), 1f, SpriteEffects.None, 0f);
        }

        #endregion
    }
}