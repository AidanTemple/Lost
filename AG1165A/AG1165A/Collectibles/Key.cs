#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
#endregion

namespace AG1165A
{
    #region Key State

    public enum KeyState
    {
        Null,
        Alive,
        Held,
        Dropped,
    }

    #endregion

    public class Key : Sprite
    {
        #region Private Members

        private Level m_Level;

        private int m_Width;
        private int m_Height;
        private int m_Score;

        private float m_Speed;

        private AnimationManager m_AnimationManager;
        private Animation m_SpawnAnimation;
        private Animation m_IdleAnimation;

        private const int m_PointsPerSecond = 10;

        private Rectangle m_LocalBounds;

        private Vector2 m_PlayerPosition;

        private TimeSpan m_SpawnTimer;
        private TimeSpan m_DropTimer;

        private bool m_IsHoldingKey;
        private bool m_WasHoldingKey;
        private bool m_CanPickup;

        private Random random = new Random();

        private KeyState state = KeyState.Null;

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

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - Origin.X) + m_LocalBounds.X;
                int top = (int)Math.Round(Position.Y - Origin.Y) + m_LocalBounds.Y;

                return new Rectangle(left, top, m_LocalBounds.Width, m_LocalBounds.Height);
            }
        }

        public float Speed
        {
            get { return m_Speed; }
            set { m_Speed = value; }
        }

        public bool IsHoldingKey
        {
            get { return m_IsHoldingKey; }
            set { m_IsHoldingKey = value; }
        }

        public bool WasHoldingKey
        {
            get { return m_WasHoldingKey; }
            set { m_WasHoldingKey = value; }
        }

        public Vector2 PlayerPosition
        {
            get { return m_PlayerPosition; }
            set { m_PlayerPosition = value; }
        }

        public KeyState State
        {
            get { return state; }
            set { state = value; }
        }

        public bool CanPickup
        {
            get { return m_CanPickup; }
            set { m_CanPickup = value; }
        }

        public TimeSpan SpawnTimer
        {
            get { return m_SpawnTimer; }
            set { m_SpawnTimer = value; }
        }

        #endregion

        #region Initialisation

        public Key(Level level)
        {
            m_Level = level;

            //m_SpawnAnimation = new Animation(ContentManager.Key_Spawn, 0.1f, false);
            m_IdleAnimation = new Animation(ContentManager.Key_Idle, 0.18f, true);

            if (m_IdleAnimation != null)
            {
                Origin = new Vector2(m_IdleAnimation.FrameWidth / 2, 
                    m_IdleAnimation.FrameHeight / 2);
            }

            m_LocalBounds = new Rectangle(0, 0, 128, 128);

            m_AnimationManager.PlayAnimation(m_IdleAnimation);

            m_Speed = 3.0f;

            IsAlive = false;
            m_IsHoldingKey = false;
            m_WasHoldingKey = false;

            m_DropTimer = TimeSpan.FromSeconds(5);

#if DEBUG
            m_SpawnTimer = TimeSpan.FromSeconds(5);
#else
            double seconds = random.Next(10, 30);
            m_SpawnTimer = TimeSpan.FromSeconds(seconds);
#endif
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case KeyState.Alive:
                    Spawn();
                    break;

                case KeyState.Held:
                    Held();
                    break;

                case KeyState.Dropped:
                    Dropped();
                    break;
            }

            if (!IsAlive && !WasHoldingKey && !IsHoldingKey)
                m_SpawnTimer -= gameTime.ElapsedGameTime;

            if (m_SpawnTimer <= TimeSpan.Zero)
                m_SpawnTimer = TimeSpan.Zero;

            if(!IsHoldingKey && IsAlive)
                m_DropTimer -= gameTime.ElapsedGameTime;

            if (!IsAlive && IsHoldingKey)
                m_DropTimer = TimeSpan.FromSeconds(5);
        }

        public void Spawn()
        {
            if (!IsAlive && !WasHoldingKey && !IsHoldingKey
                && m_SpawnTimer <= TimeSpan.Zero)
            {
                IsAlive = true;

                Position = m_PlayerPosition + new Vector2(32, 0);

                double seconds = random.Next(5, 30);
                m_SpawnTimer = TimeSpan.FromSeconds(seconds);

                state = KeyState.Dropped;
            }

            //if (!WasHoldingKey && IsHoldingKey && IsAlive)
            //{
            //    state = KeyState.Held;
            //}

            if (m_SpawnTimer > TimeSpan.Zero)
            {
                //state = KeyState.Null;
            }
        }

        private void Held()
        {
            //if (m_DropTimer <= TimeSpan.FromSeconds(3.5))
            //{
            //    m_CanPickup = true;

                if (IsAlive && !IsHoldingKey)
                {
                    IsAlive = false;
                    IsHoldingKey = true;

                    AudioHandler.PlaySoundEffect(ContentManager.KeyPickupSound);
                }

                if (!IsHoldingKey && WasHoldingKey)
                {
                    IsAlive = true;

                    state = KeyState.Dropped;
                }
            //}

            //if (m_DropTimer > TimeSpan.FromSeconds(3.5))
            //{
            //    m_CanPickup = false;
            //}
        }

        private void Dropped()
        {
            if (!IsHoldingKey && IsAlive)
            {
                Position = m_PlayerPosition + new Vector2(128, 0);

                if (m_DropTimer > TimeSpan.FromSeconds(4.98))
                {
                    AudioHandler.PlaySoundEffect(ContentManager.KeyDropSound);
                }

                if (m_DropTimer <= TimeSpan.FromSeconds(3.5))
                {
                    m_CanPickup = true;
                }

                if (m_DropTimer <= TimeSpan.Zero)
                {
                    m_DropTimer = TimeSpan.FromSeconds(5);

                    double seconds = random.Next(5, 30);
                    m_SpawnTimer = TimeSpan.FromSeconds(seconds);

                    Position = Vector2.Zero;
                    IsAlive = false;
                    WasHoldingKey = false;
                    m_CanPickup = false;

                    state = KeyState.Null;
                }
            }

            if (IsHoldingKey)
            {
                state = KeyState.Held;
            }
        }

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (IsAlive)
            {
                m_AnimationManager.Draw(spriteBatch, gameTime, Position, Origin, Rotation, SpriteEffects.None);
            }
        }

        #endregion
    }
}
