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

namespace FinalProject.GameStates
{
    public partial class BaseGameState : Utilities.GameState
    {
        //Replace with Game1 once you figure out how
        protected Game1 theGame;

        public BaseGameState(Game game)
            : base(game)
        {
            theGame = (Game1)game;
        }
    }
}
