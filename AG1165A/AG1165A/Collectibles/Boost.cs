#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
#endregion

namespace AG1165A
{
    public class Boost : Sprite
    {
        #region Private Members

        private Level m_Level;

        private int m_Width;
        private int m_Height;

        private const int m_Speed = 5;

        private TimeSpan m_BoostTimer = TimeSpan.FromSeconds(3);
        private TimeSpan m_SpawnTimer = TimeSpan.FromSeconds(30);

        private bool m_IsBoosting;

        #endregion

        #region Properties

        public Level Level
        {
            get { return m_Level; }
            set { m_Level = value; }
        }

        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }

        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - Origin.X) + Texture.Width - Width;
                int top = (int)Math.Round(Position.Y - Origin.Y) + Texture.Height - Height;

                return new Rectangle(left, top, Width, Height);
            }
        }

        public int Speed
        {
            get { return m_Speed; }
        }

        public bool IsBoosting
        {
            get { return m_IsBoosting; }
            set { m_IsBoosting = value; }
        }

        #endregion

        #region Initialisation

        public Boost(Level level, Vector2 position)
        {
            m_Level = level;

            Texture = ContentManager.Tiles;
            Source = ContentManager.IncreaseSpeedCollectible;

            if (Texture != null)
            {
                m_Width = Texture.Width;
                m_Height = Texture.Height;
            }

            Origin = new Vector2(Width / 2, Height / 2);

            Position = position;

            IsAlive = true;
            m_IsBoosting = false;
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            if (!IsAlive)
            {
                m_SpawnTimer -= gameTime.ElapsedGameTime;

                if (m_SpawnTimer <= TimeSpan.Zero)
                {
                    m_SpawnTimer = TimeSpan.FromSeconds(30);
                    IsAlive = true;
                }
            }

            if (IsBoosting)
            {
                m_BoostTimer -= gameTime.ElapsedGameTime;

                if (m_BoostTimer <= TimeSpan.Zero)
                {
                    m_BoostTimer = TimeSpan.FromSeconds(3);
                    IsBoosting = false;
                }
            }

            if (m_SpawnTimer < TimeSpan.Zero)
                m_SpawnTimer = TimeSpan.Zero;

            if (m_BoostTimer < TimeSpan.Zero)
                m_BoostTimer = TimeSpan.Zero;
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(IsAlive)
            {
                spriteBatch.Draw(Texture, Position, Source, Color.White);

                base.Draw(spriteBatch);
            }
        }

        #endregion
    }
}
