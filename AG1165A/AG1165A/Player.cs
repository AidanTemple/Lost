#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace AG1165A
{
    #region Player State

    public enum PlayerState
    {
        Idle,
        Walking,
        Punching,
        Stunned
    }

    #endregion

    public class Player
    {
        #region Private Members

        private GraphicsDevice m_Graphics;

        private Vector2 m_Position;
        private Vector2 m_Origin;
        private Vector2 m_Velocity;
        private Vector2 m_SmokePosition;
        private Vector2 m_SmokeOrigin;

        private float m_Rotation;
        private float m_Speed;
        private float m_StunTime;
        private float m_MaxStunTime;
        private float m_PunchTime;
        private float m_MaxPunchTime;

        private Level m_Level;

        private PlayerIndex m_PlayerIndex;
        private PlayerIndex m_ThrowAway;

        private AnimationManager m_PlayerAnimation;
        private Animation m_IdleAnimation;
        private Animation m_WalkAnimation;
        private Animation m_PunchAnimation;
        private Animation m_StunAnimation;

        private AnimationManager m_SmokeAnimationManager;
        private Animation m_SmokeAnimation;

        private Texture2D m_Avatar;
        private Texture2D m_IdleAvatar;
        private Texture2D m_StunAvatar;
        private Texture2D m_HappyAvatar;
        private Texture2D m_SadAvatar;

        private bool m_IsPunching;
        private bool m_IsPunchingObject;
        private bool m_WasPunching;
        private bool m_WasPunched;
        private bool m_IsStunned;
        private bool m_WasStunned;
        private bool m_IsholdingKey;
        private bool m_OpponentHasKey;

        private int m_Score;

        private Rectangle m_BoundingRect;

        private PlayerState m_State;

        private SpriteEffects m_Effects;

        private Identifier m_Identifier;

        private Random m_Random;

        #endregion

        #region Properties

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public Vector2 Origin
        {
            get { return m_Origin; }
            set { m_Origin = value; }
        }

        public float Speed
        {
            get { return m_Speed; }
            set { m_Speed = value; }
        }

        public bool IsPunching
        {
            get { return m_IsPunching; }
            set { m_IsPunching = value; }
        }

        public bool IsStunned
        {
            get { return m_IsStunned; }
            set { m_IsStunned = value; }
        }

        public bool WasStunned
        {
            get { return m_WasStunned; }
            set { m_WasStunned = value; }
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
                int left = (int)Math.Round(m_Position.X - m_Origin.X) + m_BoundingRect.X;
                int top = (int)Math.Round(m_Position.Y - m_Origin.Y) + m_BoundingRect.Y;

                return new Rectangle(left, top, m_BoundingRect.Width, m_BoundingRect.Height);
            }
        }

        public PlayerState State
        {
            get { return m_State; }
            set { m_State = value; }
        }

        public Texture2D Avatar
        {
            get { return m_Avatar; }
            set { m_Avatar = value; }
        }

        public bool IsHoldingKey 
        {
            get { return m_IsholdingKey; }
            set { m_IsholdingKey = value; }
        }

        public bool OpponentHasKey 
        {
            get { return m_OpponentHasKey; }
            set { m_OpponentHasKey = value; }
        }

        public bool IsBoosting { get; set; }

        #endregion

        #region Initialisation

        public Player(GraphicsDevice graphics, Level level, PlayerIndex playerIndex, Vector2 position)
        {
            m_Graphics = graphics;
            m_Level = level;
            m_PlayerIndex = playerIndex;
            m_Position = position;

            if (m_PlayerIndex == PlayerIndex.One)
            {
                m_IdleAnimation = new Animation(ContentManager.Player_Idle_A, 0.1f, true);
                m_WalkAnimation = new Animation(ContentManager.Player_Walk_A, 0.1f, true);
                m_PunchAnimation = new Animation(ContentManager.Player_Punch_A, 0.1f, false);
                m_StunAnimation = new Animation(ContentManager.Player_Stun_A, 0.1f, true);
                m_SmokeAnimation = new Animation(ContentManager.Smoke_Cloud, 0.1f, false);

                m_IdleAvatar = ContentManager.PlayerADefault;
                m_StunAvatar = ContentManager.PlayerAStun;
                m_HappyAvatar = ContentManager.PlayerAWin;
                m_SadAvatar = ContentManager.PlayerALose;
            }

            if (m_PlayerIndex == PlayerIndex.Two)
            {
                m_IdleAnimation = new Animation(ContentManager.Player_Idle_B, 0.1f, true);
                m_WalkAnimation = new Animation(ContentManager.Player_Walk_B, 0.1f, true);
                m_PunchAnimation = new Animation(ContentManager.Player_Punch_B, 0.1f, false);
                m_StunAnimation = new Animation(ContentManager.Player_Stun_B, 0.1f, true);
                m_SmokeAnimation = new Animation(ContentManager.Smoke_Cloud, 0.1f, false);

                m_IdleAvatar = ContentManager.PlayerBDefault;
                m_StunAvatar = ContentManager.PlayerBStun;
                m_HappyAvatar = ContentManager.PlayerBWin;
                m_SadAvatar = ContentManager.PlayerBLose;
            }

            if (m_IdleAnimation != null)
            {
                m_Origin = new Vector2(m_IdleAnimation.FrameWidth / 2,
                    m_IdleAnimation.FrameHeight / 2);
            }

            m_State = PlayerState.Idle;

            m_BoundingRect = new Rectangle(24, 24, 71, 78);

            m_Speed = 5f;

            m_MaxPunchTime = 0.3f;
            m_MaxStunTime = 3.0f;

            m_PlayerAnimation.PlayAnimation(m_IdleAnimation);
            m_Avatar = m_IdleAvatar;

            m_Identifier = new Identifier(m_PlayerIndex);
            m_Identifier.Position = m_Position;

            level.Key = new Key(level);

            m_Random = new Random();
        }

        #endregion

        #region Helper Methods

        private void CheckTileCollision()
        {
            int left = (int)Math.Floor((float)BoundingRectangle.Left / Tile.Width);
            int top = (int)Math.Floor((float)BoundingRectangle.Top / Tile.Height);
            int right = (int)Math.Ceiling(((float)BoundingRectangle.Right / Tile.Width)) - 1;
            int bottom = (int)Math.Ceiling(((float)BoundingRectangle.Bottom / Tile.Height)) - 1;

            foreach(TileLayer tileLayer in m_Level.TileLayers)
            {
                for (int y = top; y <= bottom; ++y)
                {
                    for (int x = left; x <= right; ++x)
                    {
                        Collision collisionType = tileLayer.Tiles[x, y].Collision;

                        Rectangle tileBounds;

                        if (collisionType != Collision.Passable)
                        {
                            tileBounds = tileLayer.Tiles[x, y].BoundingRectangle;

                            if(BoundingRectangle.Intersects(tileBounds))
                            {
                                Vector2 depth = Extensions.GetRectangleIntersectionDepth(BoundingRectangle, tileBounds);

                                if (depth != Vector2.Zero)
                                {
                                    float absDepthX = Math.Abs(depth.X);
                                    float absDepthY = Math.Abs(depth.Y);

                                    if (absDepthY < absDepthX)
                                    {
                                        if (collisionType == Collision.Impassable)
                                        {
                                            m_Position = new Vector2(m_Position.X, m_Position.Y + depth.Y - 1);

                                            UpdateTile(x, y);
                                        }
                                    }
                                    else if (collisionType == Collision.Impassable)
                                    {
                                        m_Position = new Vector2(m_Position.X + depth.X - 1, m_Position.Y);

                                        UpdateTile(x, y);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool CanPlayerMove(int x, int y)
        {
            Collision type = m_Level.GetViewportBounds(x, y);

            if (type == Collision.Impassable)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void UpdateTile(int x, int y)
        {
            if (m_State == PlayerState.Punching)
            {
                m_IsPunchingObject = true;

                foreach (TileLayer tileLayer in m_Level.TileLayers)
                {
                    m_SmokePosition = new Vector2(x * Tile.Width, y * Tile.Height);
                    m_SmokeAnimationManager.PlayAnimation(m_SmokeAnimation);

                    if (tileLayer.Tiles[x, y].Damage != 0 && m_WasPunching)
                    {
                        tileLayer.Tiles[x, y].Damage -= 1;
                    }

                    if (tileLayer.Tiles[x, y].Damage == 0)
                    {
                        tileLayer.Tiles[x, y].Damage = 0;

                        tileLayer.Tiles[x, y] = tileLayer.SwitchTile(tileLayer.Tiles[x, y].Type, x, y,
                            tileLayer.Tiles[x, y].Rotation);

                        if (!m_Level.Key.IsHoldingKey && !m_Level.Key.IsAlive &&
                            m_Level.Key.SpawnTimer <= TimeSpan.Zero)
                        {
                            m_Level.Key.PlayerPosition = m_Position;
                            m_Level.Key.State = KeyState.Alive;
                        }

                        // Randomly select and play a break noise.
                        int i = m_Random.Next(0, 2);
                        AudioHandler.PlaySoundEffect(ContentManager.BreakSounds[i]);
                    }

                    m_WasPunching = !m_WasPunching;
                }
            }
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            UpdateInput(gameTime);

            CheckTileCollision();

            UpdateAvatar();

            m_Identifier.Position = m_Position;

            if(m_State != PlayerState.Punching)
                m_IsPunchingObject = false;

            switch (m_State)
            {
                case PlayerState.Idle:
                    Idle();
                    break;

                case PlayerState.Walking:
                    Walking();
                    break;

                case PlayerState.Punching:
                    Punch(gameTime);
                    break;

                case PlayerState.Stunned:
                    Stun(gameTime);
                    break;
            }
        }

        private void UpdateInput(GameTime gameTime)
        {
            GamePadState gamePadState = InputHandler.GamePadStates[(int)m_PlayerIndex];
            KeyboardState keyboardState = Keyboard.GetState();

            // Only allow the player to move when they're not stunned
            if (!m_IsStunned)
            {
                Vector2 direction;

                if (gamePadState.IsConnected)
                {
                    direction = gamePadState.ThumbSticks.Left;
                    direction.Y *= -1;
                }
                else
                {
                    direction = Vector2.Zero;
                }

                if (InputHandler.IsKeyDown(Keys.W) && m_PlayerIndex == PlayerIndex.One
                    || InputHandler.IsKeyDown(Keys.Up) && m_PlayerIndex == PlayerIndex.Two)
                {
                    direction.Y -= 1;

                    m_Rotation = MathHelper.ToRadians(-90);
                    m_State = PlayerState.Walking;
                }

                if (InputHandler.IsKeyDown(Keys.A) && m_PlayerIndex == PlayerIndex.One
                    || InputHandler.IsKeyDown(Keys.Left) && m_PlayerIndex == PlayerIndex.Two)
                {
                    direction.X -= 1;

                    m_Rotation = MathHelper.ToRadians(180);
                    m_State = PlayerState.Walking;
                }

                if (InputHandler.IsKeyDown(Keys.S) && m_PlayerIndex == PlayerIndex.One
                    || InputHandler.IsKeyDown(Keys.Down) && m_PlayerIndex == PlayerIndex.Two)
                {
                    direction.Y += 1;

                    m_Rotation = MathHelper.ToRadians(90);
                    m_State = PlayerState.Walking;
                }

                if (InputHandler.IsKeyDown(Keys.D) && m_PlayerIndex == PlayerIndex.One
                    || InputHandler.IsKeyDown(Keys.Right) && m_PlayerIndex == PlayerIndex.Two)
                {
                    direction.X += 1;

                    m_Rotation = MathHelper.ToRadians(0);
                    m_State = PlayerState.Walking;
                }

                if (direction.LengthSquared() > 1)
                {
                    direction.Normalize();
                }

                m_Velocity = m_Speed * direction;
                m_Position += m_Velocity;

                if (!m_IsPunching)
                {
                    if (m_Velocity.X != 0 || m_Velocity.Y != 0)
                    {
                        m_State = PlayerState.Walking;
                    }

                    if (InputHandler.IsButtonDown(m_PlayerIndex, Buttons.A, out m_ThrowAway))
                    {
                        m_IsPunching = !m_IsPunching;
                        m_WasPunching = true;

                        m_State = PlayerState.Punching;
                    }

                    if(InputHandler.IsKeyDown(Keys.LeftControl) && m_PlayerIndex == PlayerIndex.One)
                    {
                        m_IsPunching = true;
                        m_WasPunching = true;

                        m_State = PlayerState.Punching;
                    }

                    if(InputHandler.IsKeyDown(Keys.RightControl) && m_PlayerIndex == PlayerIndex.Two)
                    {
                        m_IsPunching = true;
                        m_WasPunching = true;

                        m_State = PlayerState.Punching;
                    }
                }

                if (gamePadState.IsConnected && m_Velocity.LengthSquared() > 0)
                {
                    m_Rotation = m_Velocity.ToAngle();
                }
            }

            InputHandler.Update();
        }

        public void UpdateIdentifier(Vector2 posA, Vector2 posB)
        {
            m_Identifier.Score = m_Score;
            m_Identifier.IsArrowVisible = (OpponentHasKey) ? true : false;

            m_Identifier.Update(posA, posB);
        }

        private void UpdateAvatar()
        {
            if (!m_IsStunned && !m_IsholdingKey)
            {
                m_Avatar = m_IdleAvatar;
            }

            if (IsStunned)
            {
                m_Avatar = m_StunAvatar;
            }

            if (m_IsholdingKey)
            {
                m_Avatar = m_HappyAvatar;
            }

            if (m_OpponentHasKey && !m_IsholdingKey)
            {
                m_Avatar = m_SadAvatar;
            }
        }

        private void Idle()
        {
            if (m_IsPunching)
            {
                m_IsPunching = false;
            }

            if (m_Velocity.X == 0 || m_Velocity.Y == 0)
            {
                m_PlayerAnimation.PlayAnimation(m_IdleAnimation);
            }
            else
            {
                m_State = PlayerState.Walking;
            }
        }

        private void Walking()
        {
            if (m_Velocity.X != 0 || m_Velocity.Y != 0)
            {
                m_PlayerAnimation.PlayAnimation(m_WalkAnimation);

                AudioHandler.PlaySoundEffect(ContentManager.WalkInstance);
            }
            else
            {
                AudioHandler.StopSoundEffect(ContentManager.WalkInstance);

                m_State = PlayerState.Idle;
            }
        }

        private void Punch(GameTime gameTime)
        {
            if (m_IsPunching)
            {
                if (!m_WasPunched || m_PunchTime > 0.0f && m_PunchTime <= m_MaxPunchTime)
                {
                    if (m_PunchTime == 0.0f)
                    {
                        AudioHandler.PlaySoundEffect(ContentManager.PunchSound, 0.1f, 0, 0);
                    }

                    m_PunchTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    m_PlayerAnimation.PlayAnimation(m_PunchAnimation);
                }
                else
                {
                    m_IsPunching = false;
                    m_PunchTime = 0.0f;

                    m_PlayerAnimation.ResetAnimation();
                    m_SmokeAnimationManager.ResetAnimation(-1);

                    m_State = PlayerState.Idle;
                }
            }
            else
            {
                m_IsPunching = false;
                m_PunchTime = 0.0f;

                m_PlayerAnimation.ResetAnimation();
                m_SmokeAnimationManager.ResetAnimation(-1);

                m_State = PlayerState.Idle;
            }

            m_WasPunched = m_IsPunching;
        }

        public void Stun(GameTime gameTime)
        {
            if (this.IsStunned)
            {
                if (m_StunTime > m_MaxStunTime)
                {
                    this.IsStunned = false;
                    this.IsPunching = false;

                    m_StunTime = 0f;

                    m_State = PlayerState.Idle;

                    m_PlayerAnimation.PlayAnimation(m_IdleAnimation);
                }

                if (!this.m_WasStunned || this.m_StunTime > 0.0f && this.m_StunTime < this.m_MaxStunTime)
                {
                    if(m_StunTime == 0.0f)
                        AudioHandler.PlaySoundEffect(ContentManager.StunInstance);

                    m_StunTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (m_PlayerIndex == PlayerIndex.One)
                    {
                        m_PlayerAnimation.PlayAnimation(m_StunAnimation);
                    }

                    if (m_PlayerIndex == PlayerIndex.Two)
                    {
                        m_PlayerAnimation.PlayAnimation(m_StunAnimation);
                    }

                    InputHandler.SetVibration(m_PlayerIndex, 1.0f, 0.0f);

                    if (m_StunTime >= 0.8f)
                    {
                        InputHandler.SetVibration(m_PlayerIndex, 0.0f, 0.0f);
                    }

                    if (this.IsHoldingKey)
                    {
                        this.IsHoldingKey = false;
                        m_Level.Key.IsHoldingKey = false;
                        m_Level.Key.Position = m_Position;
                    }
                }
                else
                {
                    AudioHandler.StopSoundEffect(ContentManager.StunInstance);

                    this.IsStunned = false;
                    m_StunTime = 0.0f;
                }
            }
            else
            {
                AudioHandler.StopSoundEffect(ContentManager.StunInstance);

                this.IsStunned = false;
                m_StunTime = 0.0f;
            }

            this.m_WasStunned = this.IsStunned;
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            m_Identifier.Draw(spriteBatch);

            m_PlayerAnimation.Draw(spriteBatch, gameTime, m_Position, m_Origin, m_Rotation, m_Effects);

            if (m_IsPunchingObject)
            {
                m_SmokeAnimationManager.Draw(spriteBatch, gameTime, m_SmokePosition,
                    m_SmokeOrigin, 0f, SpriteEffects.None);
            }
        }

        #endregion
    }
}