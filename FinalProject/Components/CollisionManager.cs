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
    interface ICollisionManager
    {
    
    }
    public class CollisionManager 
        : Microsoft.Xna.Framework.GameComponent,/*SpriteCharacter,*/ ICollisionManager
    {
        //private SpriteCharacter collisionSprite;

        //public SpriteCharacter CollisionSprite { get { return collisionSprite; } set { collisionSprite = value; } }
        //private SpriteCharacter collidedSprite;
        public CollisionManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(ICollisionManager), this);
        }


        public bool Intersects(SpriteCharacter spriteCharacter, SpriteCharacter collisionSprite)
        {
            return Intersects(collisionSprite.LocationRect, spriteCharacter.LocationRect);
        }
        public static bool Intersects(Rectangle a, Rectangle b)
        {
            return (a.Right > b.Left && a.Left < b.Right
                && a.Bottom > b.Top && a.Top < b.Bottom);
        }

        public virtual bool PerPixelCollision(SpriteCharacter spriteCharacter, SpriteCharacter collisionSprite)
        {
            return IntersectPixels(spriteCharacter.SpriteTransform, spriteCharacter.SpriteTexture.Width, spriteCharacter.SpriteTexture.Height,
                spriteCharacter.SpriteTextureData, collisionSprite.SpriteTransform, collisionSprite.SpriteTexture.Width,
                collisionSprite.SpriteTexture.Height, collisionSprite.SpriteTextureData
                );
        }
        public static bool IntersectPixels(
           Matrix transformA, int widthA, int heightA, Color[] dataA,
           Matrix transformB, int widthB, int heightB, Color[] dataB) //Stretching 1 x 1 texture to rect is bugging this out.
        {
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            Vector2 yPositionInB = Vector2.Transform(Vector2.Zero, transformAToB);
            for (int yA = 0; yA < heightA; yA++)
			{
                Vector2 positionInB = yPositionInB;

                for (int xA = 0; xA < widthA; xA++)
			    {
                    int xB = (int)Math.Round(positionInB.X);
                    int yB = (int)Math.Round(positionInB.Y);

                    if(0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        if (colorA.A != 0 && colorB.A != 0)
	                    {
		                    return true;
	                    }
                    }
                    
                    positionInB += stepX;
			    }
                
                yPositionInB += stepY;
			}
            
            return false;
        }
    }
}
