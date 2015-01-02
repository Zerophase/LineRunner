using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Utilities
{
    public interface IInputHandler
    {
        bool WasPressed(int playerIndex, InputHandler.ButtonType button, Keys keys);
        bool WasKeyPressed( Keys keys);
        bool WasButtonPressed(int playerIndex, InputHandler.ButtonType button);

        KeyboardHandler KeyboardState { get; }

        GamePadState[] GamePads { get; }
        GamePadHandler ButtonHandler {get;}

#if !XBOX360
        MouseHandler mouseState { get; }
#endif
    }
    public partial class InputHandler
        : Microsoft.Xna.Framework.GameComponent, IInputHandler
    {
        public enum ButtonType { A, B, Back, LeftShoulder, LeftStick, RightShoulder, RightStick, Start, X, Y }

        private KeyboardHandler keyboard;
        private GamePadHandler gamePadHandler = new GamePadHandler();
        private GamePadState[] gamePads = new GamePadState[4];
#if !XBOX360
        private MouseHandler mouse;
#endif
        public InputHandler(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IInputHandler), this);

            keyboard = new KeyboardHandler();
            mouse = new MouseHandler();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            keyboard.Update();
            mouse.Update();
            gamePadHandler.Update();

            if (GamePad.GetState(PlayerIndex.One).IsConnected)
                gamePads[0] = gamePadHandler.GamePadStates[0];
            if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                gamePads[1] = gamePadHandler.GamePadStates[1];
            if (GamePad.GetState(PlayerIndex.Three).IsConnected)
                gamePads[2] = gamePadHandler.GamePadStates[2];
            if (GamePad.GetState(PlayerIndex.Four).IsConnected)
                gamePads[3] = gamePadHandler.GamePadStates[3];
            base.Update(gameTime);
        }

        public bool WasPressed(int playerIndex, ButtonType button, Keys keys)
        {
            if (keyboard.WasKeyPressed(keys) || gamePadHandler.WasButtonPressed(playerIndex, button))
                return (true);
            else
                return (false);
        }

        public bool WasButtonPressed(int playerIndex, ButtonType button)
        {
            return gamePadHandler.WasButtonPressed(playerIndex, button);
        }

        //Look into if keyboard index is needed for networked game
        public bool WasKeyPressed(Keys keys)
        {
            return keyboard.WasKeyPressed(keys);
        }

        public GamePadHandler ButtonHandler
        {
            get { return (gamePadHandler); }
        }

        public KeyboardHandler KeyboardState
        {
            get { return (keyboard); }
        }

        public GamePadState[] GamePads
        {
            get { return (gamePads); }
        }
        public MouseHandler mouseState
        {
            get { return (mouse); }
        }
    }

    public class KeyboardHandler
    {
        private KeyboardState prevKeyboardState;
        private KeyboardState keyboardState;

        private double keyboardTimer = 0;
        private bool timerOn = false;

        public KeyboardHandler()
        {
            prevKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return (keyboardState.IsKeyDown(key));
        }

        public bool IsKeyUp(Keys key)
        {
            return (keyboardState.IsKeyUp(key));
        }

        public bool IsHoldingKey(Keys key)
        {
            return (prevKeyboardState.IsKeyDown(key) && keyboardState.IsKeyDown(key));
        }

        public bool WasKeyPressed(Keys key)
        {
            return (keyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key));
        }

        public bool HasReleasedKey(Keys key)
        {
            return (keyboardState.IsKeyUp(key) && prevKeyboardState.IsKeyDown(key));
        }

        public bool RapidKeyPress(Keys keys, GameTime gameTime)
        {
            if (timerOn && keyboardState.IsKeyDown(keys) && prevKeyboardState.IsKeyUp(keys))
            {
                timerOn = false;
                keyboardTimer = 0;
                return keyboardState.IsKeyDown(keys);
            }
            if (prevKeyboardState.IsKeyDown(keys) && keyboardState.IsKeyUp(keys))
            {
                timerOn = true;
            }
            if (timerOn)
            {
                keyboardTimer += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (keyboardTimer > 100)
            {
                keyboardTimer = 0;
                timerOn = false;
            }   
            
            return false;
        }

        public void Update()
        {
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            
        }
    
    }

    public class GamePadHandler
    {
        private GamePadState[] prevGamePadsState = new GamePadState[4];
        private GamePadState[] gamePadsState = new GamePadState[4];
        public GamePadState[] GamePadStates { get { return gamePadsState; } }

        public GamePadHandler()
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
                prevGamePadsState[0] = GamePad.GetState(PlayerIndex.One); 
            if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                gamePadsState[1] = GamePad.GetState(PlayerIndex.Two);
            if (GamePad.GetState(PlayerIndex.Three).IsConnected)
                gamePadsState[2] = GamePad.GetState(PlayerIndex.Three);
            if (GamePad.GetState(PlayerIndex.Four).IsConnected)
                gamePadsState[3] = GamePad.GetState(PlayerIndex.Four);
        }

        public void Update()
        {
            prevGamePadsState[0] = gamePadsState[0];
            prevGamePadsState[1] = gamePadsState[1];
            prevGamePadsState[2] = gamePadsState[2];
            prevGamePadsState[3] = gamePadsState[3];

            if (GamePad.GetState(PlayerIndex.One).IsConnected)
                gamePadsState[0] = GamePad.GetState(PlayerIndex.One);
            if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                gamePadsState[1] = GamePad.GetState(PlayerIndex.Two);
            if (GamePad.GetState(PlayerIndex.Three).IsConnected)
                gamePadsState[2] = GamePad.GetState(PlayerIndex.Three);
            if (GamePad.GetState(PlayerIndex.Four).IsConnected)
                gamePadsState[3] = GamePad.GetState(PlayerIndex.Four);
        }

        //public bool IsButtonHeld(int playerIndex, InputHandler.ButtonType button)
        //{
        //    int pi = playerIndex;

        //    //return //(prevGamePadsState[pi].WasButtonPressed(pi, button) && gamePadsState[pi].WasButtonPressed(pi, button));
        //}
        public bool WasButtonPressed(int playerIndex, InputHandler.ButtonType button)
        {
            {
                int pi = playerIndex;

                switch (button)
                {
                     case InputHandler.ButtonType.A:
                    {
                        return (gamePadsState[pi].Buttons.A == ButtonState.Pressed &&
                            prevGamePadsState[pi].Buttons.A == ButtonState.Released);
                    }
                    case InputHandler.ButtonType.B:
                        {
                            return (gamePadsState[pi].Buttons.B == ButtonState.Pressed &&
                                prevGamePadsState[pi].Buttons.B == ButtonState.Released);
                        }
                    case InputHandler.ButtonType.Back:
                        {
                            return (gamePadsState[pi].Buttons.Back == ButtonState.Pressed &&
                                prevGamePadsState[pi].Buttons.Back == ButtonState.Released);
                        }
                    case InputHandler.ButtonType.LeftShoulder:
                        {
                            return (gamePadsState[pi].Buttons.LeftShoulder == ButtonState.Pressed &&
                                prevGamePadsState[pi].Buttons.LeftShoulder == ButtonState.Released);
                        }
                    case InputHandler.ButtonType.LeftStick:
                        {
                            return (gamePadsState[pi].Buttons.LeftStick == ButtonState.Pressed &&
                                prevGamePadsState[pi].Buttons.LeftStick == ButtonState.Released);
                        }
                    case InputHandler.ButtonType.RightShoulder:
                        {
                            return (gamePadsState[pi].Buttons.RightShoulder == ButtonState.Pressed &&
                                prevGamePadsState[pi].Buttons.RightShoulder == ButtonState.Released);
                        }
                    case InputHandler.ButtonType.RightStick:
                        {
                            return (gamePadsState[pi].Buttons.RightStick == ButtonState.Pressed &&
                                prevGamePadsState[pi].Buttons.RightStick == ButtonState.Released);
                        }
                    case InputHandler.ButtonType.Start:
                        {
                            return (gamePadsState[pi].Buttons.Start == ButtonState.Pressed &&
                                prevGamePadsState[pi].Buttons.Start == ButtonState.Released);
                        }
                    case InputHandler.ButtonType.X:
                        {
                            return (gamePadsState[pi].Buttons.X == ButtonState.Pressed &&
                                prevGamePadsState[pi].Buttons.X == ButtonState.Released);
                        }
                    case InputHandler.ButtonType.Y:
                        {
                            return (gamePadsState[pi].Buttons.Y == ButtonState.Pressed &&
                                prevGamePadsState[pi].Buttons.Y == ButtonState.Released);
                        }
                    default:
                        throw (new ArgumentException());
                }
            }
        }
    }

    public class MouseHandler
    {
        private MouseState prevMouseState;
        private MouseState mouseState;

        //private Vector2 position;

        public MouseHandler()
        {
            prevMouseState = Mouse.GetState();
            //position = new Vector2((float)mouseState.X, (float)mouseState.Y);
        }

        public bool IsLeftClickDown()
        {
            if (mouseState.LeftButton == ButtonState.Pressed
                && prevMouseState.LeftButton != ButtonState.Pressed)
                return true;
            else
                return false;
        }

        public MouseState MousePosition()
        {
            return mouseState;
        }

        public void Update()
        {
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
        }
    }
}
