#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Used to represent a sprite and its state. Class is marked as abstract
    /// as it's not a complete implementation of a sprite.
    /// </summary>
    public abstract class Sprite
    {
        #region Protected Members

        // A texture which represents a 2D sprite. Only allow 
        // the sprite texture to be accessible by the sprite 
        // class and any derived class instances.
        protected Texture2D Texture;

        #endregion

        #region Public Members

        // A rectangle that specifies in texels the source
        // location of a sprite on a texture. 
        public Rectangle Source;

        // The location in screen coordinates to draw the sprite.
        public Vector2 Position;

        // Origin of the sprite texture. Default position is 
        // (0, 0) which represents the upper-left corner.
        public Vector2 Origin;

        // The color used to tint a sprite. Coloring sprite 
        // white results in no tint.
        public Color Color;

        // Used to apply effects to a sprite i.e. flipping
        // horizontally or vertically.
        public SpriteEffects Effects;

        // Specifies the angle in radians to rotate the 
        // sprite about its center.
        public Single Rotation;

        // Used to scale the sprite texture.
        public Single Scale;

        // The depth of a layer. By default, 0 represents
        // the front layer and 1 represents a back layer.
        public Single Depth;

        // Used to determine whether the sprite should be 
        // updated and/or rendered.
        public bool IsAlive;

        #endregion

        #region Initialisation

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Sprite()
        {
            Texture = null;

            Source = new Rectangle(0, 0, 0, 0);

            Position = Vector2.Zero;
            Origin = Vector2.Zero;

            Color = Color.White;

            Effects = SpriteEffects.None;

            Rotation = 0f;
            Scale = 1f;
            Depth = 0f;

            IsAlive = false;
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates a sprite, abstract modifier used to indicate that the 
        /// method is an incomplete implementation.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Update(GameTime gameTime);

        #endregion

        #region Draw

        /// <summary>
        /// Renders a sprite to the screen.
        /// </summary>
        /// <param name="spriteBatch">Enables a group of sprites to be drawn using the same settings.</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Source, Color, Rotation, Origin, Scale, Effects, Depth);
        }

        #endregion
    }
}