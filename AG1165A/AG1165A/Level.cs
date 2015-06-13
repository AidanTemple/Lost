#region Using Statements
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Input;
#endregion

namespace AG1165A
{
    public class Level
    {
        #region Private Members

        private GraphicsDevice m_Graphics;
        private SpriteBatch m_SpriteBatch;

        private Main m_Main;

        private Countdown m_Countdown = new Countdown();

        private TileMap m_Map;
        private TileLayer m_Layer;

        private float m_CameraPositionX;
        private float m_CameraPositionY;

        private Player[] m_Players;

        private Viewport m_MainViewport;
        private Viewport m_LeftViewport;
        private Viewport m_RightViewport;

        private GameTimer m_GameTimer;
        private TimeSpan m_PointTimer = TimeSpan.FromSeconds(1.5);

        private const int m_PointsPerSeconds = 10;

        private Key m_Key;

        private List<Boost> m_Boost = new List<Boost>();

        private bool m_WasBoosting;
        private bool m_WasHoldingKey;
        private bool m_IsGamePaused;

        // Used to indicate if a controller is connected.
        private bool m_IsConnected;

        private Texture2D m_Background;

        #endregion

        Interface m_InterfaceYellow;
        Interface m_InterfaceBlue;

        FloatingScore m_Score = new FloatingScore();

        #region Properties

        public List<TileLayer> TileLayers
        {
            get { return m_Map.TileLayers; }
            set { m_Map.TileLayers = value; }
        }

        public List<Rectangle> BoundingBoxes
        {
            get { return m_Map.BoundingBoxes; }
            set { m_Map.BoundingBoxes = value; }
        }

        public Player[] Players
        {
            get { return m_Players; }
            set { m_Players = value; }
        }

        public Key Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        public List<Boost> BoostCollectibles
        {
            get { return m_Boost; }
            set { m_Boost = value; }
        }

        public bool IsGamePaused 
        {
            get { return m_IsGamePaused; }
            set { m_IsGamePaused = value; }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        public Level(Main main, List<Stream> streams, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            m_Graphics = graphics;
            m_SpriteBatch = spriteBatch;
            m_Main = main;

            m_MainViewport = graphics.Viewport;

            m_LeftViewport = m_MainViewport;
            m_LeftViewport.Width = m_LeftViewport.Width / 2;
            
            m_RightViewport = m_MainViewport;
            m_RightViewport.Width = m_RightViewport.Width / 2;
            m_RightViewport.X = m_LeftViewport.Width;

            m_Background = ContentManager.LevelBackground;

            m_Players = new Player[2];

            m_Map = new TileMap();

            foreach (Stream stream in streams)
            {
                m_Layer = new TileLayer(stream, graphics, this, new Vector2(m_CameraPositionX,
                    m_CameraPositionY));

                m_Map.TileLayers.Add(m_Layer);
            }

            m_GameTimer = new GameTimer();

            m_InterfaceYellow = new Interface(ContentManager.HUD_Yellow);
            m_InterfaceBlue = new Interface(ContentManager.HUD_Blue);

            m_WasBoosting = false;
            m_WasHoldingKey = false;
            m_IsGamePaused = false;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Collision GetViewportBounds(int x, int y)
        {
            if (x < 0 || x >= m_Layer.Width)
            {
                return Collision.Impassable;
            }

            if (y < 0 || y >= m_Layer.Height)
            {
                return Collision.Impassable;
            }

            return m_Layer.Tiles[x, y].Collision;
        }

        private void CheckPlayerCollision(GameTime gameTime)
        {
            if (m_Players[0].BoundingRectangle.Intersects(m_Players[1].BoundingRectangle))
            {
                if (m_Players[0].IsPunching)
                {
                    if (!m_Players[1].IsStunned)
                    {
                        m_Players[1].IsStunned = true;
                        m_Players[1].State = PlayerState.Stunned;
                    }

                    if (m_Players[1].IsHoldingKey)
                    {
                        m_Players[1].IsHoldingKey = false;
                        m_Players[1].Speed = 3.5f;

                        m_Key.IsAlive = true;
                        m_Key.IsHoldingKey = false;
                        m_Key.PlayerPosition = m_Players[0].Position;
                        m_Key.State = KeyState.Dropped;
                    }
                }

                if (m_Players[1].IsPunching)
                {
                    if (!m_Players[0].IsStunned)
                    {
                        m_Players[0].IsStunned = true;
                        m_Players[0].State = PlayerState.Stunned;
                    }

                    if (m_Players[0].IsHoldingKey)
                    {
                        m_Players[0].IsHoldingKey = false;
                        m_Players[0].Speed = 3.5f;

                        m_Key.IsAlive = true;
                        m_Key.IsHoldingKey = false;
                        m_Key.PlayerPosition = m_Players[0].Position;
                        m_Key.State = KeyState.Dropped;
                    }
                }
            }            
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Handle and update input
            UpdateInput();

            // Check whether or not the game is paused
            if (!IsGamePaused)
            {
                m_Countdown.Update(gameTime);

                if (m_GameTimer.TimeRemaining == TimeSpan.FromMinutes(3.0) 
                    && !m_Countdown.IsCountingDown)
                {
                    AudioHandler.PlaySoundEffect(ContentManager.ThemeInstance, 1f, 0f, 0f);
                }

                if (m_GameTimer.TimeRemaining != TimeSpan.Zero && !m_Countdown.IsCountingDown)
                {
                    CheckPlayerCollision(gameTime);

                    m_GameTimer.Update(gameTime);

                    UpdatePlayers(gameTime);

                    UpdateKeyCollectible(gameTime);

                    UpdateBoostCollectible(gameTime);
                }

                if (m_GameTimer.TimeRemaining < TimeSpan.Zero)
                {
                    MediaPlayer.Stop();

                    m_Main.winScene.Players = m_Players;
                    m_Main.SwitchScene(m_Main.winScene);
                }
            }
        }

        private void UpdateInput()
        {
            // Retrieve the current state of available controllers.
            GamePadState gamePadState = InputHandler.GamePadStates[(int)PlayerIndex.One];

            // Determines whether a controller is connected.
            m_IsConnected = (gamePadState.IsConnected);
        }

        private void UpdatePlayers(GameTime gameTime)
        {
            m_Players[0].Update(gameTime);
            m_Players[1].Update(gameTime);

            m_Players[0].UpdateIdentifier(m_Players[0].Position, m_Players[1].Position);
            m_Players[1].UpdateIdentifier(m_Players[1].Position, m_Players[0].Position);

            if (!m_Players[0].IsHoldingKey && m_Players[1].IsHoldingKey)
            {
                m_Players[0].OpponentHasKey = true;
            }
            else
            {
                m_Players[0].OpponentHasKey = false;
            }

            if (!m_Players[1].IsHoldingKey && m_Players[0].IsHoldingKey)
            {
                m_Players[1].OpponentHasKey = true;
            }
            else
            {
                m_Players[1].OpponentHasKey = false;
            }
        }

        /// <summary>
        /// Responsible for updating the players speed and score by determining if 
        /// the player is holding a key.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void UpdateKeyCollectible(GameTime gameTime)
        {
            if (m_Key != null)
            {
                if (m_Players[0].IsHoldingKey)
                {
                    if (m_PointTimer == TimeSpan.FromSeconds(1))
                    {
                        m_Score.Position = m_Players[0].Position;
                    }

                    if (m_PointTimer <= TimeSpan.Zero)
                    {
                        m_Score.Position = m_Players[0].Position;
                        m_Players[0].Score += m_PointsPerSeconds;
                        m_PointTimer = TimeSpan.FromSeconds(1.5);
                    }

                    m_PointTimer -= gameTime.ElapsedGameTime;
                }

                if (m_Players[1].IsHoldingKey)
                {
                    m_PointTimer -= gameTime.ElapsedGameTime;

                    m_Score.Position = m_Players[1].Position;

                    if (m_PointTimer <= TimeSpan.Zero)
                    {
                        m_Players[1].Score += m_PointsPerSeconds;
                        m_PointTimer = TimeSpan.FromSeconds(1.5);
                    }
                }

                if (m_Key.IsAlive)
                {
                    if (m_Key.BoundingRectangle.Intersects(m_Players[0].BoundingRectangle))
                    {
                        if (m_Key.CanPickup)
                        {
                            m_Players[0].IsHoldingKey = true;
                            m_Players[0].Speed = 3.5f;

                            m_WasHoldingKey = true;
                            m_Key.State = KeyState.Held;
                        }
                    }
                    else if (m_Key.BoundingRectangle.Intersects(m_Players[1].BoundingRectangle))
                    {
                        if (m_Key.CanPickup)
                        {
                            m_Players[1].IsHoldingKey = true;
                            m_Players[1].Speed = 3.5f;

                            m_WasHoldingKey = true;
                            m_Key.State = KeyState.Held;
                        }
                    }
                }

                // Check if player has dropped the key
                if (m_WasHoldingKey && m_Key.WasHoldingKey &&
                    !m_Key.IsHoldingKey)
                {
                    if (m_Players[0].IsHoldingKey)
                    {
                        m_Players[0].IsHoldingKey = false;
                        m_Players[0].Speed = 5f;
                    }

                    if (m_Players[1].IsHoldingKey)
                    {
                        m_Players[1].IsHoldingKey = false;
                        m_Players[1].Speed = 5f;
                    }

                    m_WasHoldingKey = false;
                    m_Key.WasHoldingKey = false;

                    m_Key.State = KeyState.Dropped;
                }

                m_Key.Update(gameTime);

                m_Score.PointTimer = m_PointTimer;
                m_Score.Update(gameTime);
            }
        }

        private void UpdateBoostCollectible(GameTime gameTime)
        {
            if (m_Boost != null)
            {
                for (int i = 0; i < m_Boost.Count; i++)
                {
                    m_Boost[i].Update(gameTime);

                    if (m_Boost[i].IsAlive)
                    {
                        if (m_Boost[i].BoundingRectangle.Intersects(m_Players[0].BoundingRectangle))
                        {
                            m_Boost[i].IsAlive = false;
                            m_Boost[i].IsBoosting = true;

                            m_Players[0].IsBoosting = true;
                            m_Players[0].Speed = (m_Boost[i].IsBoosting) ? m_Boost[i].Speed : 3.5f;

                            m_WasBoosting = true;
                        }

                        if (m_Boost[i].BoundingRectangle.Intersects(m_Players[1].BoundingRectangle))
                        {
                            m_Boost[i].IsAlive = false;
                            m_Boost[i].IsBoosting = true;

                            m_Players[1].IsBoosting = true;
                            m_Players[1].Speed = (m_Boost[i].IsBoosting) ? m_Boost[i].Speed : 3.5f;

                            m_WasBoosting = true;
                        }
                    }

                    if (m_WasBoosting && !m_Boost[i].IsBoosting)
                    {
                        if (m_Players[0].IsBoosting)
                        {
                            m_Players[0].Speed = 3.5f;
                        }

                        if (m_Players[1].IsBoosting)
                        {
                            m_Players[1].Speed = 3.5f;
                        }

                        m_WasBoosting = false;
                    }
                }
            }
        }

        private void ScrollCamera(SpriteBatch spriteBatch, Player player, Viewport viewport)
        {
            const float viewMargin = 0.5f;

            float width = viewport.Width * viewMargin;
            float height = viewport.Height * viewMargin;

            float left = m_CameraPositionX + width;
            float right = m_CameraPositionX + viewport.Width - width;
            float top = m_CameraPositionY + viewport.Height * viewMargin;
            float bottom = m_CameraPositionY + viewport.Height - viewport.Height * viewMargin;

            float deltaX = 0.0f;
            float deltaY = 0.0f;

            if (player.Position.X < left)
                deltaX = player.Position.X - left;

            if (player.Position.X > right)
                deltaX = player.Position.X - right;

            if (player.Position.Y < top)
                deltaY = player.Position.Y - top;

            if (player.Position.Y > bottom)
                deltaY = player.Position.Y - bottom;

            float maxCameraPositionXOffset = Tile.Width * m_Layer.Width - viewport.Width;
            float maxCameraPositionYOffset = Tile.Height * m_Layer.Height - viewport.Height;

            m_CameraPositionX = MathHelper.Clamp(m_CameraPositionX + deltaX, 0.0f, maxCameraPositionXOffset);
            m_CameraPositionY = MathHelper.Clamp(m_CameraPositionY + deltaY, 0.0f, maxCameraPositionYOffset);
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawPlayers(gameTime, spriteBatch, m_Players[0], m_Players[1], 0);
            DrawPlayers(gameTime, spriteBatch, m_Players[1], m_Players[0], 1);

            for (int i = 0; i < m_Players.Length; i++)
            {
                DrawViewportInterface(spriteBatch, i);
            }

            m_Graphics.Viewport = m_MainViewport;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            spriteBatch.Draw(ContentManager.Viewport_Edge, new Vector2(m_Graphics.Viewport.Width / 2, 0), Color.Black);

            m_GameTimer.Draw(spriteBatch, m_Graphics.Viewport);

            m_Countdown.Draw(spriteBatch, m_Graphics.Viewport);

            if (IsGamePaused)
            {
                if (m_IsConnected)
                {
                    spriteBatch.Draw(ContentManager.ControllerPauseOverlay, Vector2.Zero, Color.White);
                }
                else
                {
                    spriteBatch.Draw(ContentManager.KeyboardPauseOverlay, Vector2.Zero, Color.White);
                }
            }

            spriteBatch.End();
        }

        private void DrawPlayers(GameTime gameTime, SpriteBatch spriteBatch, Player player, Player opponent, int id)
        {
            m_Graphics.Viewport = (id == 0) ? m_LeftViewport : m_RightViewport;

            ScrollCamera(spriteBatch, player, m_Graphics.Viewport);
            Matrix transform = Matrix.CreateTranslation((int)Math.Floor(-m_CameraPositionX), (int)Math.Floor(-m_CameraPositionY), 0);

            // Enable SamplerState.PointClamp to prevent visual artifacts appearing when scrolling camera
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, transform);

            spriteBatch.Draw(m_Background, Vector2.Zero, Color.White);

            m_Map.Draw(spriteBatch, m_CameraPositionX, m_CameraPositionY);

            spriteBatch.Draw(ContentManager.LevelInteriorWalls, Vector2.Zero, Color.White);

            foreach (Boost collectible in m_Boost)
                collectible.Draw(spriteBatch);

            if (m_Key.IsAlive)
                m_Key.Draw(spriteBatch, gameTime);

            player.Draw(spriteBatch, gameTime);
            opponent.Draw(spriteBatch, gameTime);

            if (player.IsHoldingKey)
            {
                m_Score.Draw(m_SpriteBatch, m_PointsPerSeconds);
            }

            spriteBatch.End();
        }

        private void DrawViewportInterface(SpriteBatch spriteBatch, int id)
        {
            m_Graphics.Viewport = (id == 0) ? m_LeftViewport : m_RightViewport;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (id == 0)
            {
                m_InterfaceYellow.Draw(spriteBatch, m_Graphics.Viewport, 
                    Vector2.Zero);

                if (m_Players[id].IsHoldingKey)
                {
                    spriteBatch.Draw(ContentManager.HUD_Key, new Vector2(143, 145), Color.White);
                }

                DrawScore(spriteBatch, m_Graphics.Viewport, id);

                spriteBatch.Draw(m_Players[0].Avatar, new Rectangle(m_Graphics.Viewport.X + 40, m_Graphics.Viewport.Y + 45,
                    m_Players[0].Avatar.Width, m_Players[0].Avatar.Height), Color.White);
            }
            
            if(id == 1)
            {
                m_InterfaceBlue.Draw(spriteBatch, m_Graphics.Viewport, new Vector2(
                    m_Graphics.Viewport.Width - m_InterfaceBlue.Width, m_Graphics.Viewport.Y));

                if (m_Players[id].IsHoldingKey)
                {
                    spriteBatch.Draw(ContentManager.HUD_Key, new Vector2(442, 145), Color.White);
                }

                DrawScore(spriteBatch, m_Graphics.Viewport, id);

                spriteBatch.Draw(m_Players[1].Avatar, new Rectangle(m_Graphics.Viewport.Width - (m_Players[1].Avatar.Width) - 35,
                    m_Graphics.Viewport.Y + 45, m_Players[1].Avatar.Width, m_Players[1].Avatar.Height), Color.White);
            }

            spriteBatch.End();
        }

        private void DrawScore(SpriteBatch spriteBatch, Viewport viewport, int id)
        {
            string score = string.Format("{0}", m_Players[id].Score.ToString("0000"));

            Vector2 size = ContentManager.ScoreFont.MeasureString(score);
            Vector2 position = Vector2.Zero;

            if (id == 0)
            {
                position = new Vector2(130, 22);
            }

            if (id == 1)
            {
                position = new Vector2(viewport.Width - (size.X * 2.6f), 22);
            }

            spriteBatch.DrawString(ContentManager.ScoreFont, score, position + new Vector2(2, 2), Color.Black);
            spriteBatch.DrawString(ContentManager.ScoreFont, score, position, Color.White);
                
        }

        #endregion
    }
}