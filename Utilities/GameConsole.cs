using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace Utilities
{
    

    public interface IGameConsole
    {
        string FontName { get; set; }
        string DebugText { get; set; }

        string GetGameConsoleText();
        void GameConsoleWrite(string s);
    }
    public class GameConsole : DrawableGameComponent, IGameConsole
    {
        public enum GameConsoleState { Closed, Open };

        protected string fontName;
        public string FontName { get { return fontName; } set { fontName = value; } }

        protected string debugText;
        public string DebugText { get { return debugText; } set { debugText = value; } }

        protected int maxLines;
        public int MaxLines { get { return maxLines; } set { maxLines = value; } }

        SpriteFont font;
        SpriteBatch spriteBatch;

        protected List<string> gameConsoleText;
        protected GameConsoleState gameConsoleState;

        public Keys ToggleConsoleKey;

        InputHandler input;

        public GameConsole(Game game)
            : base(game)
        {
            this.fontName = "MyFont";
            this.gameConsoleText = new List<string>();
            this.gameConsoleText.Add("Console Initialized");
            //Look into ContentManager if things don't work.
            this.maxLines = 20;
            this.debugText = "Console default \ndebug text";
            this.ToggleConsoleKey = Keys.OemTilde;

            this.gameConsoleState = GameConsoleState.Closed;

            input = (InputHandler)Game.Services.GetService(typeof(IInputHandler));

            if (input == null)
            {
                throw new Exception("GameConsole Depends on Input service please add input service before you add Game Console.");
            }

            game.Services.AddService(typeof(IGameConsole), this);
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("MyFont");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (input.KeyboardState.HasReleasedKey(ToggleConsoleKey))
            {
                if (this.gameConsoleState == GameConsoleState.Closed)
                {
                    this.gameConsoleState = GameConsoleState.Open;
                }
                else
                    this.gameConsoleState = GameConsoleState.Closed;
                
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.gameConsoleState == GameConsoleState.Open)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, GetGameConsoleText(), Vector2.Zero, Color.Wheat);
                spriteBatch.DrawString(font, debugText, new Vector2(200f,0f), Color.Wheat);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public string GetGameConsoleText()
        {
            string Text = "";
            string[] current = new string[Math.Min(gameConsoleText.Count, MaxLines)];
            int offsetLines = (gameConsoleText.Count / maxLines) * maxLines;

            int offset = gameConsoleText.Count - offsetLines;

            int indexStart = offsetLines - (maxLines - offset);
            if (indexStart < 0)
                indexStart = 0;

            gameConsoleText.CopyTo(
                indexStart, current, 0, Math.Min(gameConsoleText.Count, MaxLines));

            foreach (string s in current)
	        {
                Text += s;
                Text += "\n";
	        }
            return Text;
        }

        public void GameConsoleWrite(string s)
        {
            gameConsoleText.Add(s);
        }
    }
}
