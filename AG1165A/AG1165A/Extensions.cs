#region Using Statements
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Contains additional helper methods for common data types.
    /// </summary>
    static class Extensions
    {
        #region Helper Methods

        /// <summary>
        /// Calculates the signed depth of intersection between two rectangles.
        /// </summary>
        /// <returns>
        /// The amount of overlap between two intersecting rectangles. These
        /// depth values can be negative depending on which wides the rectangles
        /// intersect. This allows callers to determine the correct direction
        /// to push objects in order to resolve collisions.
        /// If the rectangles are not intersecting, Vector2.Zero is returned.
        /// </returns>
        public static Vector2 GetRectangleIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2;
            float halfHeightA = rectA.Height / 2;
            float halfWidthB = rectB.Width / 2;
            float halfHeightB = rectB.Height / 2;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distX = centerA.X - centerB.X;
            float distY = centerA.Y - centerB.Y;
            float minDistX = halfWidthA + halfWidthB;
            float minDistY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distX) >= minDistX || Math.Abs(distX) >= minDistY)
            {
                return Vector2.Zero;
            }

            // Calculate and return intersection depths.
            float depthX = distX > 0 ? minDistX - distX : -minDistX - distX;
            float depthY = distY > 0 ? minDistY - distY : -minDistY - distY;

            return new Vector2(depthX, depthY);
        }

        /// <summary>
        /// Gets the position of the center of the bottom edge of the rectangle.
        /// </summary>
        public static Vector2 GetBottomCenter(this Rectangle rect)
        {
            return new Vector2(rect.X + rect.Width / 2.0f, rect.Bottom);
        }

        /// <summary>
        /// Used to find the angle an entity is facing.
        /// </summary>
        /// <param name="vector">Vector to which an angle should be found.</param>
        /// <returns>Returns the angle whose tangent is the quotient of two specified numbers</returns>
        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        /// <summary>
        /// Returns the width and height of a string as a Vector2
        /// </summary>
        /// <param name="font">Represents a font texture.</param>
        /// <param name="text">The string to measure.</param>
        /// <returns></returns>
        public static Vector2 MeasureString(SpriteFont font, string text)
        {
            Vector2 size = font.MeasureString(text);

            return size;
        }

        #endregion
    }
}