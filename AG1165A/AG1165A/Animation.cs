#region Using Statements
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Represents an animated texture.
    /// </summary>
    class Animation
    {
        #region Private Members

        // Represents the spritesheet the animation is located.
        private Texture2D m_Texture;

        // Duration of time to show each frame.
        private float m_FrameTime;

        // Loops the animation when being played.
        private bool m_IsLooping;

        #endregion

        #region Properties

        /// <summary>
        /// Represents the spritesheet the animation is located.
        /// </summary>
        public Texture2D Texture
        {
            get { return m_Texture; }
        }

        /// <summary>
        /// Duration of time to show each frame.
        /// </summary>
        public float FrameTime
        {
            get { return m_FrameTime; }
        }

        /// <summary>
        /// Gets the number of frames in the animations.
        /// </summary>
        public int FrameCount
        {
            get { return Texture.Width / FrameWidth; }
        }

        /// <summary>
        /// Gets the width of a frame.
        /// </summary>
        public int FrameWidth
        {
            get { return Texture.Height; }
        }

        /// <summary>
        /// Gets the height of a frame.
        /// </summary>
        public int FrameHeight
        {
            get { return Texture.Height; }
        }

        /// <summary>
        /// Loops the animation when being played.
        /// </summary>
        public bool IsLooping
        {
            get { return m_IsLooping; }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="texture">Represents the spritesheet the animation is located.</param>
        /// <param name="frameTime">Duration of time to show each frame.</param>
        /// <param name="isLooping">Loops the animation when being played.</param>
        public Animation(Texture2D texture, float frameTime, bool isLooping)
        {
            m_Texture = texture;

            m_FrameTime = frameTime;
            m_IsLooping = isLooping;
        }

        #endregion
    }
}