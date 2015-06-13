#region Using Statements
using Microsoft.Xna.Framework;
using System.Collections.Generic;
#endregion

namespace AG1165A
{
    /// <summary>
    /// This is the base class for all game scenes.
    /// </summary>
    public class SceneManager : DrawableGameComponent
    {
        #region Private Members

        /// <summary>
        /// List of child GameComponents
        /// </summary>
        private readonly List<GameComponent> m_Components;

        #endregion

        #region Properties

        /// <summary>
        /// Assigns read access to the list of game components
        /// </summary>
        public List<GameComponent> Components
        {
            get { return m_Components; }
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="game">Provides a snapshot of timing values.</param>
        public SceneManager(Game game)
            : base(game)
        {
            m_Components = new List<GameComponent>();

            // Indicates whether draw should be called.
            Visible = false;

            // Indicates whether GameComponent.Update should
            // be called when Game.Update is called.
            Enabled = false;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Used to show the scene.
        /// </summary>
        public virtual void ShowScene()
        {
            Visible = true;
            Enabled = true;
        }

        /// <summary>
        /// Used to hide the scene.
        /// </summary>
        public virtual void HideScene()
        {
            Visible = false;
            Enabled = false;
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // Update all child components
            for (int i = 0; i < m_Components.Count; i++)
            {
                if (m_Components[i].Enabled)
                {
                    m_Components[i].Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Allows the game component draw your content in game screen
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // Draw each child component if they're visible.
            for (int i = 0; i < m_Components.Count; i++)
            {
                GameComponent component = Components[i];

                if ((component is DrawableGameComponent) &&
                    ((DrawableGameComponent)component).Visible)
                {
                    ((DrawableGameComponent)component).Draw(gameTime);
                }
            }

            base.Draw(gameTime);
        }

        #endregion
    }
}