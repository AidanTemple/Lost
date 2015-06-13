#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Controls playback of an Animation.
    /// </summary>
    struct AnimationManager
    {
        #region Private Members

        private Animation m_Animation;

        private int m_FrameIndex;

        // The amount of time in seconds that the current frame 
        // has been shown for.
        private float m_ElapsedTime;

        private Rectangle m_Source;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        public Animation Animation
        {
            get { return m_Animation; }
        }

        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex
        {
            get { return m_FrameIndex; }
            set { m_FrameIndex = value; }
        }

        /// <summary>
        /// Gets a texture origin at the bottom center of each frame.
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight); }
        }

        /// <summary>
        /// A rectangle that specifies in texels the source
        /// location of a frame on a texture. 
        /// </summary>
        public Rectangle Source
        {
            get { return m_Source; }
            set { m_Source = value; }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Resets the animations frame index to its initial frame.
        /// </summary>
        public void ResetAnimation()
        {
            m_FrameIndex = 0;
        }

        /// <summary>
        /// Resets the animations frame index to its initial frame.
        /// </summary>
        /// <param name="frame">Frame to reset the animation to.</param>
        public void ResetAnimation(int frame)
        {
            m_FrameIndex = frame;
        }

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(Animation animation)
        {
            // If this animation is already running, do not restart it.
            if (Animation == animation)
                return;

            this.m_Animation = animation;

            m_FrameIndex = 0;
            m_ElapsedTime = 0.0f;
        }

        #endregion

        #region Draw

        /// <summary>
        /// Advances the time position and draws the current frame of the animation.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 position, Vector2 origin, float rotation,
            SpriteEffects effects)
        {
            m_ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Animation == null)
            {
                Console.WriteLine("Animation not set to an instance of an object.");

                return;
            }

            while (m_ElapsedTime > Animation.FrameTime)
            {
                m_ElapsedTime -= Animation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                {
                    m_FrameIndex = (m_FrameIndex + 1) % Animation.FrameCount;
                }
                else
                {
                    m_FrameIndex = Math.Min(m_FrameIndex + 1, Animation.FrameCount - 1);
                }
            }

            // Calculate the source rectangle of the current frame.
            m_Source = new Rectangle(m_FrameIndex * Animation.FrameHeight, 0, Animation.FrameHeight, Animation.FrameHeight);

            // Draw the current frame.
            spriteBatch.Draw(Animation.Texture, position, m_Source, Color.White, rotation, origin, 1.0f, effects, 0.0f);
        }

        #endregion
    }
}