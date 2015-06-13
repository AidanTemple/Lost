#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endregion

namespace AG1165A
{
    public class Interface
    {
        #region Private Members

        // Represents the user interface texture
        private Texture2D m_Interface;

        #endregion

        #region Properties

        /// <summary>
        /// Width of the interface texture.
        /// </summary>
        public int Width
        {
            get { return m_Interface.Width; }
        }

        /// <summary>
        /// Height of the interface texture.
        /// </summary>
        public int Height
        {
            get { return m_Interface.Height; }
        }

        #endregion

        #region Initialise

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="texture">Represents the interface texture.</param>
        public Interface(Texture2D texture)
        {
            m_Interface = texture;
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draw the interface on the screen.
        /// </summary>
        /// <param name="spriteBatch">Enables a group of sprites to be drawn using the same settings.</param>
        /// <param name="viewport">Defines the window dimensions of a render-target.</param>
        /// <param name="position">Position to draw the interface. This position is the top left 
        /// corner of the interface texture</param>
        public void Draw(SpriteBatch spriteBatch, Viewport viewport, Vector2 position)
        {
            spriteBatch.Draw(m_Interface, position, Color.White);
        }

        #endregion
    }
}
