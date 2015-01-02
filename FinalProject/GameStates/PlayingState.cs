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
using Utilities;
using FinalProject.Components;

namespace FinalProject.GameStates
{
    public interface IPlayingState : IGameState { }

    //IF Possible REMOVE UTILITIES FROM STATES THAT DO NOT NEED IT.
    public class PlayingState : BaseGameState, IPlayingState
    {
        SpriteFont font;

        CollisionManager collisionManager;
        ChunkConstructor chunkConstructor;
        Score score;
        BackgroundManager backgroundManager;
        PlayerSprite playerSprite;

        public PlayingState(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IPlayingState), this);

            collisionManager = new CollisionManager(theGame);
            // TODO Refactor so all chunks exist in chunkConstructor
            chunkConstructor = new ChunkConstructor(theGame);

            score = new Score(theGame);

            backgroundManager = new BackgroundManager(theGame);
            playerSprite = new PlayerSprite(theGame);

            theGame.Components.Add(collisionManager);

            theGame.Components.Add(backgroundManager);
            
            theGame.Components.Add(chunkConstructor);

            theGame.Components.Add(score);

            theGame.Components.Add(playerSprite);

            backgroundManager.Visible = false;
            backgroundManager.Enabled = false;

            playerSprite.Visible = false;
            playerSprite.Enabled = false;
            //Might want to change
            chunkConstructor.Visible = false;
            chunkConstructor.Enabled = false;

            score.Visible = false;
            score.Enabled = false;
        }

        protected override void LoadContent()
        {
            font = theGame.Content.Load<SpriteFont>("MyFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (input.WasKeyPressed(Keys.Escape) || input.WasKeyPressed(Keys.P))
                gameManager.PushState(theGame.PausedState);

            if (playerSprite.Location.X < theGame.Window.ClientBounds.X ||
                    playerSprite.Location.Y > theGame.Window.ClientBounds.Height)
                        gameManager.PushState(theGame.GameOverState);

            base.Update(gameTime);
        }

        protected override void StateChanged(object sender, EventArgs e)
        {
            if (gameManager.State == theGame.PausedState)
            {
                this.Enabled = false;
                backgroundManager.Enabled = false;
                playerSprite.Enabled = false;
                chunkConstructor.Enabled = false;

            }
            else if (gameManager.State == theGame.GameOverState)
            {
                playerSprite.Location.X = 100;
                playerSprite.Location.Y = 480;

                playerSprite.Reset();
                chunkConstructor.Reset();

                this.Enabled = false;
                this.Visible = false;

                backgroundManager.Enabled = false;
                backgroundManager.Visible = false;


                playerSprite.Enabled = false;
                playerSprite.Visible = false;


                chunkConstructor.Enabled = false;
                chunkConstructor.Visible = false;

                score.Visible = false;
                score.Enabled = false;
            }
            else if (gameManager.State == theGame.StartMenuState)
            {
                this.Enabled = this.Visible = false;
            }
            else if (gameManager.State != this.Value)
            {
                Visible = true;
                Enabled = true;

                backgroundManager.Visible = true;
                playerSprite.Visible = true;
                chunkConstructor.Visible = true;
                score.Visible = true;
            }
            else
            {
                backgroundManager.Visible = true;
                backgroundManager.Enabled = true;

                playerSprite.Visible = true;
                playerSprite.Enabled = true;

                chunkConstructor.Visible = true;
                chunkConstructor.Enabled = true;

                score.Visible = true;
                score.Enabled = true;
            }

            base.StateChanged(sender, e);
        }

        public override void Draw(GameTime gameTime)
        {
            
            base.Draw(gameTime);
        }
    }
}
