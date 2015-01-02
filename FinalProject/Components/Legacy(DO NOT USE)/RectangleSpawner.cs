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

namespace FinalProject.Components
{
    interface IRectangleSpawner
    { 
    
    }
    public class RectangleSpawner : SpriteCharacter, IRectangleSpawner
    {
        public enum RectangleType { HORIZONTAL, VERTICAL, BLANK };

        private RectangleType currentType;
        public RectangleType CurrentType { set { currentType = value;} }

        public HorizontalRect horizontalRect;
        public Rects.VerticalRect verticleRect;


        public RectangleSpawner(Game game, int rectCount, RectangleType type)
            : base(game)
        {
            currentType = type;
            //spriteTexture = content.Load<Texture2D>("YellowTile");

            

            if (currentType == RectangleType.HORIZONTAL)
            {
                horizontalRect = new HorizontalRect(game, rectCount);
                //blankTextureName = "BlankTexture";
                //spriteTexture = content.Load<Texture2D>("YellowTile");
                //horizontalRect = new Rectangle((int)x, (int)y, 100, 100);
                //this.locationRect = horizontalRect;
            }
            if (currentType == RectangleType.VERTICAL)
            {
                //blankTextureName = null;
                verticleRect = new Rects.VerticalRect(game, rectCount);
            }

            if (currentType == RectangleType.BLANK)
            {
                //spriteTexture = content.Load<Texture2D>("BlankTexture");
                horizontalRect = new HorizontalRect(game, rectCount);
                //this.locationRect = horizontalRect;
            }
        }
    }
}
