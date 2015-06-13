#region Using Statements
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
#endregion

namespace AG1165A
{
    public class TileMap
    {
        #region Private Members

        private List<TileLayer> m_Layers = new List<TileLayer>();

        private List<Rectangle> m_BoundingBoxes = new List<Rectangle>();

        #endregion

        #region Properties

        public List<TileLayer> TileLayers
        {
            get { return m_Layers; }
            set { m_Layers = value; }
        }

        public List<Rectangle> BoundingBoxes
        {
            get { return m_BoundingBoxes; }
            set { m_BoundingBoxes = value; }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch, float cameraPosX, float cameraPosY)
        {
            foreach (TileLayer layer in m_Layers)
            {
                layer.Draw(spriteBatch, cameraPosX, cameraPosY);
            }        
        }

        #endregion
    }
}
