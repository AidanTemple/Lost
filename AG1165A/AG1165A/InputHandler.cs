#region Using Statements
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
#endregion

namespace AG1165A
{
    // Specifies the game controller associated with a player. 
    public enum LogicalGamerIndex
    {
        One,
        Two,
        Three,
        Four
    }

    static class InputHandler
    {
        #region Private Members

        // Specifies the game controller associated with a player. 
        private static readonly PlayerIndex[] playerIndices = 
        {
            PlayerIndex.One,
            PlayerIndex.Two,
            PlayerIndex.Three,
            PlayerIndex.Four,
        };

        // Maximum number of controllers the framework can support.
        private const uint maxControllers = 4;

        // Represents a state of keystrokes recorded by a keyboard input device. 
        private static KeyboardState keyboardState;

        // Represents the previous state of keystrokes recorded by a keyboard input device. 
        private static KeyboardState prevKeyboardState;

        // Retrieve the current state of all connected controllers. Represents 
        // specific information about the state of an Xbox 360 Controller, 
        // including the current state of buttons and sticks.
        private static GamePadState[] gamePadState = new GamePadState[maxControllers];

        // Retrieve the previous state of all connected controllers.
        private static GamePadState[] prevGamePadState = new GamePadState[maxControllers];

        // Used to determine which controllers were connected.
        private static bool[] GamePadWasConnected = new bool[maxControllers];

        // Represents the first controller which is defaulted to
        // PlayerIndex.One. Used to allow only one person to control
        // menu systems within the game.
        private static PlayerIndex? controllingPlayer;

        #endregion

        #region Properties

        // Retrieves the state of each controller.
        public static GamePadState[] GamePadStates
        {
            get { return gamePadState; }
        }

        // Represents the first controller which is defaulted to
        // PlayerIndex.One.
        public static PlayerIndex? ControllingPlayer
        {
            get { return controllingPlayer; }
            internal set { controllingPlayer = value; }
        }

        #endregion

        #region Update

        /// <summary>
        /// Reads the latest state of the keyboard and gamepad.
        /// </summary>
        public static void Update()
        {
            // Retrieve the keyboard state.
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            // Retrieve the state of each available controller.
            for (uint i = 0; i < maxControllers; i++)
            {
                prevGamePadState[i] = gamePadState[i];
                gamePadState[i] = GamePad.GetState((PlayerIndex)i);

                // Check which controllers are connected to the computer.
                if (gamePadState[i].IsConnected)
                {
                    // Update each active controllers status to connected.
                    GamePadWasConnected[i] = true;
                }
            }
        }

        #endregion

        #region GamePad Input

        // Returns the game controller index associated with a player.
        public static PlayerIndex GetPlayerIndex(LogicalGamerIndex index)
        {
            return playerIndices[(int)index];
        }

        // Specifies the game controller associated with a player. 
        public static void SetPlayerIndex(LogicalGamerIndex gamerIndex, PlayerIndex playerIndex)
        {
            playerIndices[(int)gamerIndex] = playerIndex;
        }

        /// <summary>
        /// Sets the vibration motor speeds on an Xbox 360 Controller
        /// </summary>
        /// <param name="index">Player index that identifies the controller to set.</param>
        /// <param name="leftMotor">The speed of the left motor between 0.0 and 1.0. This is a low-frequency motor.</param>
        /// <param name="rightMotor">The speed of the right motor between 0.0 and 1.0. This is a high-frequency motor.</param>
        /// <returns>True if the vibration motors were successfully set; false if the controller was unable to process the request.</returns>
        public static bool SetVibration(PlayerIndex? index, float leftMotor, float rightMotor)
        {
            if (index.HasValue)
            {
                PlayerIndex playerIndex = index.Value;

                return GamePad.SetVibration(playerIndex, leftMotor, rightMotor);
            }
            else
            {
                return (SetVibration(PlayerIndex.One, leftMotor, rightMotor)
                    || SetVibration(PlayerIndex.Two, leftMotor, rightMotor)
                    || SetVibration(PlayerIndex.Three, leftMotor, rightMotor)
                    || SetVibration(PlayerIndex.Four, leftMotor, rightMotor));
            }
        }

        /// <summary>
        /// Determines whether a specifed button is currently pressed down.
        /// </summary>
        /// <param name="index">Specifies the game controller associated with a player. </param>
        /// <param name="button">A value of the Button enumeration that identifies the button whose current state you want to determine.</param>
        /// <param name="playerIndex">Specifies the game controller associated with a player.</param>
        /// <returns>True if the specified button is currently pressed down; otherwise, false.</returns>
        public static bool IsButtonDown(PlayerIndex? index, Buttons button, out PlayerIndex playerIndex)
        {
            if (index.HasValue)
            {
                playerIndex = index.Value;
                int i = (int)playerIndex;

                return (gamePadState[i].IsButtonDown(button));
            }
            else
            {
                return (IsButtonDown(PlayerIndex.One, button, out playerIndex)
                    || IsButtonDown(PlayerIndex.Two, button, out playerIndex)
                    || IsButtonDown(PlayerIndex.Three, button, out playerIndex)
                    || IsButtonDown(PlayerIndex.Four, button, out playerIndex));
            }
        }

        /// <summary>
        /// Determines whether specified input device buttons are up (not pressed) in this GamePadState. 
        /// </summary>
        /// <param name="index">Specifies the game controller associated with a player.</param>
        /// <param name="button">A value of the Button enumeration that identifies the button whose current state you want to determine.</param>
        /// <param name="playerIndex">Specifies the game controller associated with a player.</param>
        /// <returns>True if any specified buttons are up; false otherwise.</returns>
        public static bool IsButtonUp(PlayerIndex? index, Buttons button, out PlayerIndex playerIndex)
        {
            if (index.HasValue)
            {
                playerIndex = index.Value;
                int i = (int)playerIndex;

                return (gamePadState[i].IsButtonUp(button));
            }
            else
            {
                return (IsButtonDown(PlayerIndex.One, button, out playerIndex)
                    || IsButtonDown(PlayerIndex.Two, button, out playerIndex)
                    || IsButtonDown(PlayerIndex.Three, button, out playerIndex)
                    || IsButtonDown(PlayerIndex.Four, button, out playerIndex));
            }
        }

        /// <summary>
        /// Determines whether a specifed button was previously pressed down and then released.
        /// </summary>
        /// <param name="index">Specifies the game controller associated with a player.</param>
        /// <param name="button">A value of the Button enumeration that identifies the button whose current state you want to determine.</param>
        /// <param name="playerIndex">Specifies the game controller associated with a player.</param>
        /// <returns></returns>
        public static bool WasButtonPressed(PlayerIndex? index, Buttons button, out PlayerIndex playerIndex)
        {
            if (index.HasValue)
            {
                playerIndex = index.Value;
                int i = (int)playerIndex;

                return (gamePadState[i].IsButtonDown(button) &&
                    prevGamePadState[i].IsButtonUp(button));
            }
            else
            {
                return (WasButtonPressed(PlayerIndex.One, button, out playerIndex)
                    || WasButtonPressed(PlayerIndex.Two, button, out playerIndex)
                    || WasButtonPressed(PlayerIndex.Three, button, out playerIndex)
                    || WasButtonPressed(PlayerIndex.Four, button, out playerIndex));
            }
        }

        /// <summary>
        /// Determines whether a specifed button was previously released and then pressed down.
        /// </summary>
        /// <param name="index">Specifies the game controller associated with a player.</param>
        /// <param name="button">A value of the Button enumeration that identifies the button whose current state you want to determine.</param>
        /// <param name="playerIndex">Specifies the game controller associated with a player.</param>
        /// <returns></returns>
        public static bool IsHoldingButton(PlayerIndex? index, Buttons button, out PlayerIndex playerIndex)
        {
            if (index.HasValue)
            {
                playerIndex = index.Value;
                int i = (int)playerIndex;

                return (gamePadState[i].IsButtonDown(button) &&
                    prevGamePadState[i].IsButtonDown(button));
            }
            else
            {
                return (IsHoldingButton(PlayerIndex.One, button, out playerIndex)
                    || IsHoldingButton(PlayerIndex.Two, button, out playerIndex)
                    || IsHoldingButton(PlayerIndex.Three, button, out playerIndex)
                    || IsHoldingButton(PlayerIndex.Four, button, out playerIndex));
            }
        }

        #endregion

        #region Keyboard Input

        /// <summary>
        /// Returns whether a specified key is currently being pressed. 
        /// </summary>
        /// <param name="key">Enumerated value that specifies the key to query.</param>
        /// <returns>True if the key specified by key is being held down; false otherwise.</returns>
        public static bool IsKeyDown(Keys key)
        {
            return (keyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Returns whether a specified key is currently not pressed. 
        /// </summary>
        /// <param name="key">Enumerated value that specifies the key to query.</param>
        /// <returns>True if the key specified by key is not pressed; false otherwise.</returns>
        public static bool IsKeyUp(Keys key)
        {
            return (keyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Returns whether a specified key is currently held down.
        /// </summary>
        /// <param name="key">Enumerated value that specifies the key to query.</param>
        /// <returns>True if the key specified by key is being held down; false otherwise.</returns>
        public static bool IsHoldingKey(Keys key)
        {
            return (prevKeyboardState.IsKeyDown(key) && keyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Returns whether a specified key has been pressed.
        /// </summary>
        /// <param name="key">Enumerated value that specifies the key to query.</param>
        /// <returns>True if the key specified by key has been pressed; false otherwise.</returns>
        public static bool WasKeyPressed(Keys key)
        {
            return (prevKeyboardState.IsKeyUp(key) && keyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Returns whether a specified key was previously pressed and then released.
        /// </summary>
        /// <param name="key">Enumerated value that specifies the key to query.</param>
        /// <returns>True if the key specified by key was previously pressed and then released; false otherwise.</returns>
        public static bool HasReleasedKey(Keys key)
        {
            return (prevKeyboardState.IsKeyDown(key) && keyboardState.IsKeyUp(key));
        }

        #endregion
    }
}