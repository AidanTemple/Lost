#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
#endregion

namespace AG1165A
{
    /// <summary>
    /// Game component that implements a menu with text elements.
    /// </summary>
    public class MenuComponent : DrawableGameComponent
    {
        #region Private Members

        // Rpresents a series of text elements in a main menu.
        private readonly List<string> m_Elements;

        #endregion

        #region Protected Members

        // Enables a group of sprites to be drawn using the same settings. 
        protected SpriteBatch m_SpriteBatch = null;

        // Position of the menu.
        protected Vector2 m_Position;

        // Index of selected menu item.
        protected int m_Index;

        // Width of the menu.
        protected int m_Width;

        // Height of the menu.
        protected int m_Height;

        // Font to render text with when text is selected.
        protected readonly SpriteFont m_Selected;

        // Font to render text with when text is unselected.
        protected readonly SpriteFont m_Unselected;

        #endregion

        #region Properties

        /// <summary>
        /// Position of MenuComponent on screen
        /// </summary>
        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        /// <summary>
        /// Selected menu item index
        /// </summary>
        public int Index
        {
            get { return m_Index; }
            set { m_Index = value; }
        }

        /// <summary>
        /// Width of the menu.
        /// </summary>
        public int Width
        {
            get { return m_Width; }
        }

        /// <summary>
        /// Height of the menu.
        /// </summary>
        public int Height
        {
            get { return m_Height; }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">Provides basic graphics device initialization, game logic, and rendering code.</param>
        /// <param name="unselected">Unselected menu item</param>
        /// <param name="selected">Selected menu item</param>
        public MenuComponent(Game game, SpriteFont unselected, SpriteFont selected)
            : base(game)
        {
            // Gets the SpriteBatch service. 
            m_SpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            m_Elements = new List<string>();

            this.m_Unselected = unselected;
            this.m_Selected = selected;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Set the Menu Options
        /// </summary>
        /// <param name="elements">Collection of menu elements.</param>
        public void SetMenuItems(string[] elements)
        {
            m_Elements.Clear();
            m_Elements.AddRange(elements);

            CalculateItemBounds();
        }

        /// <summary>
        /// Calculates the bounding box encapsulating each menu item.
        /// </summary>
        protected void CalculateItemBounds()
        {
            m_Width = 0;
            m_Height = 0;

            foreach (string item in m_Elements)
            {
                if (m_Selected != null)
                {
                    Vector2 size = m_Selected.MeasureString(item);

                    if (size.X > m_Width)
                    {
                        m_Width = (int)size.X;
                    }

                    m_Height += m_Selected.LineSpacing;
                }
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Snapshot of the game timing state expressed in values that 
        /// can be used by variable-step (real time) or fixed-step (game time) games.</param>
        public override void Update(GameTime gameTime)
        {
            // Updates the menu when input is detected.
            UpdateInput();

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the menu when input is detected.
        /// </summary>
        public void UpdateInput()
        {
            PlayerIndex throwAway;

            for (int i = 0; i < 4; i++)
            {
                // Selects the previous menu item if the keyboards Up or DPads 
                // Up button is pressed.
                if (InputHandler.WasButtonPressed((PlayerIndex)i, Buttons.DPadUp, out throwAway)
                    || InputHandler.WasKeyPressed(Keys.Up))
                {
                    m_Index--;

                    m_Index = (m_Index == -1) ? m_Elements.Count - 1 : m_Index;

                    // Player a sound effect when we select a new menu item.
                    AudioHandler.PlaySoundEffect(ContentManager.MenuUpSound);
                }
                // Selects the next menu item if the keyboards Down or DPads 
                // Down button is pressed.
                else if (InputHandler.WasButtonPressed((PlayerIndex)i, Buttons.DPadDown, out throwAway)
                    || InputHandler.WasKeyPressed(Keys.Down))
                {
                    m_Index++;

                    m_Index = (m_Index == m_Elements.Count) ? 0 : m_Index;

                    // Player a sound effect when we select a new menu item.
                    AudioHandler.PlaySoundEffect(ContentManager.MenuDownSound);
                }
            }

            InputHandler.Update();
        }

        #endregion

        #region Draw

        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            float positionY = Game.GraphicsDevice.Viewport.Height;

            for (int i = 0; i < m_Elements.Count; i++)
            {
                // Determine the color of a menu item depending on whether
                // or not it is selected.
                SpriteFont font = (i == m_Index) ? m_Selected : m_Unselected;
                Color color = (i == m_Index) ? Color.Yellow : Color.White;

                // Retrieve the size of each menu item and position them
                // in the center of the screen.
                Vector2 size = font.MeasureString(m_Elements[i]);
                Vector2 center = new Vector2(Game.GraphicsDevice.Viewport.Width / 2,
                    positionY - (size.Y * m_Elements.Count) - 250);

                // Draw each menu item below each other.
                m_SpriteBatch.DrawString(font, m_Elements[i], center - (size / 2), color);

                positionY += font.LineSpacing;
            }

            base.Draw(gameTime);
        }

        #endregion
    }
}