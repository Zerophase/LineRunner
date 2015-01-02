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
    public interface ICellAnimationManager {}

    public sealed partial class CellAnimationManager : Microsoft.Xna.Framework.GameComponent, ICellAnimationManager
    {
        private Dictionary<string, CellAnimation> animations = new Dictionary<string, CellAnimation>();
        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        //LOOK UP SHARING CONTENT MANAGER
        private ContentManager content;
        private string contentPath;
        public Dictionary<string, CellAnimation> Animations { get { return animations; } }

        public CellAnimationManager(Game game)
            : base(game)
        {
            content = game.Content;
            contentPath = game.Content.RootDirectory;

            game.Services.AddService(typeof(ICellAnimationManager), this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        
        public void AddAnimation(string animationKey, string textureName,
            CellCount cellCount, int fps)
        {
            if (!textures.ContainsKey(textureName))
            {
                textures.Add(textureName, content.Load<Texture2D>(textureName));
            }

            int cellWidth = (int)(textures[textureName].Width / cellCount.Columns);
            int cellHeight = (int)(textures[textureName].Height / cellCount.Rows);

            int cells = cellCount.Columns * cellCount.Rows;

            //we create a cel range by passing in start location of 1,1
            //and end with number of column and rows.
            //If 2 rows 1 column = 1 firstCellX, 1 firstCellY, 2 lastCellX, 1 lastCellY 
            //2,1  =   1,1,2,1  ;    4,2  =  1,1,4,2

            AddAnimation(animationKey, textureName,
                new CellRange(1, 1, cellCount.Columns, cellCount.Rows),
                cellWidth, cellHeight, cells, fps);
        }

        public void AddAnimation(string animationKey, string textureName,
            Texture2D texture, CellCount cellCount, int fps)
        {
            if (!textures.ContainsKey(textureName))
            {
                textures.Add(textureName, texture);
            }

            int cellWidth = (int)(textures[textureName].Width / cellCount.Columns);
            int cellHeight = (int)(textures[textureName].Height / cellCount.Rows);

            int cells = cellCount.Columns * cellCount.Rows;

            AddAnimation(animationKey, textureName,
                new CellRange(1, 1, cellCount.Columns, cellCount.Rows),
                cellWidth, cellHeight, cells, fps);
        }

        public void AddAnimation(string animationKey, string textureName,
            CellRange cellRange, int cellWidth, int cellHeight,
            int cells, int fps)//Should this be private?
        {
            CellAnimation cellAnimation = new CellAnimation(textureName, cellRange, fps);

            if (!textures.ContainsKey(textureName))
            {
                textures.Add(textureName, content.Load<Texture2D>(contentPath + textureName));
            }

            cellAnimation.CellWidth = cellWidth;
            cellAnimation.CellHeight = cellHeight;

            cellAnimation.Cells = cells;

            cellAnimation.CellsPerRow = textures[textureName].Width / cellWidth;

            if (animations.ContainsKey(animationKey))
                animations[animationKey] = cellAnimation;//VALUE ASSIGNED TO KEY
            else
                animations.Add(animationKey, cellAnimation);
        }

        public void ToggleAnimation(string animationKey, bool paused)
        {
            if (animations.ContainsKey(animationKey))
                animations[animationKey].Paused = paused;
        }

        public void ResetAnimation(string animationKey)
        {
            if (animations.ContainsKey(animationKey))
            {
                animations[animationKey].Frame = animations[animationKey].StillFrame;
                animations[animationKey].Paused = true;
                animations[animationKey].LoopCount = 0;
            }
        }

        public void ToggleAnimation(string animationKey)
        {
            if (animations.ContainsKey(animationKey))
                animations[animationKey].Paused = !animations[animationKey].Paused;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<string, CellAnimation> animation in animations)
            {
                CellAnimation cellAnimation = animation.Value;

                if (cellAnimation.Paused)
                    continue;

                cellAnimation.TotalElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (cellAnimation.TotalElapsedTime > cellAnimation.TimePerFrame)
                {
                    cellAnimation.Frame++;

                    //min: 0, max: total cells
                    if (cellAnimation.Frame >= cellAnimation.Cells)
                        cellAnimation.LoopCount++;

                    cellAnimation.Frame = cellAnimation.Frame % (cellAnimation.Cells);

                    //reset timer
                    cellAnimation.TotalElapsedTime -= cellAnimation.TimePerFrame;
                }
            }
            base.Update(gameTime);
        }

        public void Draw(float elapsedTime, string animationKey, SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(elapsedTime, animationKey, spriteBatch, animations[animationKey].Frame, position);
        }

        public void Draw(float elapsedTime, string animationKey, SpriteBatch batch, int frame, Vector2 position)
        {
            Draw(elapsedTime, animationKey, batch, animations[animationKey].Frame, position, Color.White);
        }
        public Rectangle GetCurrentDrawRect(float elapsedTime, string animationKey)
        {
            return GetCurrentDrawRect(elapsedTime, animationKey, 0.0f);
        }

        public Rectangle GetCurrentDrawRect(float elapsedTime, string animationKey, float scale)
        {
            if (!animations.ContainsKey(animationKey))
            {
                throw new Exception("animationKey not found");
            }

            CellAnimation cellAnimation = animations[animationKey];

            int xIncrease = (cellAnimation.Frame + cellAnimation.CellRange.FirstCellX - 1);
            int xWrapped = xIncrease % cellAnimation.CellsPerRow;
            int x = xWrapped * cellAnimation.CellWidth;

            int yIncrease = xIncrease / cellAnimation.CellsPerRow;
            int y = (yIncrease + cellAnimation.CellRange.FirstCellY - 1) * cellAnimation.CellHeight;

            Rectangle cell = new Rectangle(x, y,
                (int)(cellAnimation.CellWidth),
                (int)(cellAnimation.CellHeight));
            return cell;
            
        }

        public Texture2D GetTexture(string textureName)
        {
            if (!textures.ContainsKey(textureName))
            {
                throw new Exception("textureName not found");
            }

            return textures[textureName];
        }

        public int GetLoopCount(string animationKey)
        {
            CellAnimation cellAnimation = animations[animationKey];
            return cellAnimation.LoopCount;
        }

        public void Draw(float elapsedTime, string animationKey,
            SpriteBatch spriteBatch, int frame, Vector2 position, Color color)
        {
            if (!animations.ContainsKey(animationKey))
                return;

            CellAnimation cellAnimation = animations[animationKey];

            Rectangle cell = this.GetCurrentDrawRect(elapsedTime, animationKey);

            spriteBatch.Draw(textures[cellAnimation.TextureName], position, cell, color); 
        }

        public void DrawBottomCenter(float elapsedTime, string animationKey, SpriteBatch spriteBatch, Vector2 position)
        {
            DrawBottomCenter(elapsedTime, animationKey, spriteBatch, animations[animationKey].Frame, position);
        }

        public void DrawBottomCenter(float elapsedTime, string animationKey, SpriteBatch spriteBatch, int frame, Vector2 position)
        {
            DrawBottomCenter(elapsedTime, animationKey, spriteBatch, animations[animationKey].Frame, position, Color.White);
        }

        public void DrawBottomCenter(float elapsedTime, string animationKey, SpriteBatch spriteBatch, int frame, Vector2 position, Color color)
        {
            if (!animations.ContainsKey(animationKey))
                return;

            CellAnimation cellAnimation = animations[animationKey];

            int xIncrease = (cellAnimation.Frame + cellAnimation.CellRange.FirstCellX - 1);
            int xWrapped = xIncrease % cellAnimation.CellsPerRow;
            int x = xWrapped * cellAnimation.CellWidth;

            int yIncrease = xIncrease / cellAnimation.CellsPerRow;
            int y = (yIncrease + cellAnimation.CellRange.FirstCellY - 1) * cellAnimation.CellHeight;

            Rectangle cell = new Rectangle(x, y, cellAnimation.CellWidth, cellAnimation.CellHeight);

            Vector2 origin = new Vector2((cellAnimation.CellWidth / 2), (cellAnimation.CellHeight));

            spriteBatch.Draw(textures[cellAnimation.TextureName], position, cell, color, 0.0f, origin, 1.0f, SpriteEffects.None, 0f);
        }
    }

    public class CellAnimation
    {
        private string textureName;
        private CellRange cellRange;
        private int fps;
        private float timePerFrame;

        public float TotalElapsedTime = 0.0f;
        public int CellWidth;
        public int CellHeight;
        public int Cells;
        public int CellsPerRow;//MIGHT RENAME
        public int Frame;
        public int StillFrame;
        public bool Paused = false;

        private int loopCount;
        public int LoopCount { get { return loopCount; } set { loopCount = value; } }

        public CellAnimation(string textureName, CellRange cellRange, int fps)
        {
            this.textureName = textureName;
            this.cellRange = cellRange;
            this.fps = fps;
            this.timePerFrame = 1.0f / (float)fps;
            this.Frame = 0;
            this.StillFrame = 0;//CHECK FOR MODIFYING TO STOP ON CURRENT FRAME
            this.loopCount = 0;
        }

        public string TextureName
        {
            get { return (textureName); }
        }
        public CellRange CellRange
        {
            get { return (cellRange); }
        }

        public int FPS
        {
            get { return (fps); }
        }
        public float TimePerFrame
        {
            get { return (timePerFrame); }
        }
    }

    public struct CellCount
    {
        public int Columns;
        public int Rows;

        public CellCount(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
        }
    }

    public struct CellRange
    {
        public int FirstCellX;
        public int FirstCellY;
        public int LastCellX;
        public int LastCellY;

        public CellRange(int firstCellX, int firstCellY, int lastCellX, int lastCellY)
        {
            FirstCellX = firstCellX;
            FirstCellY = firstCellY;
            LastCellX = lastCellX;
            LastCellY = lastCellY;
        }
    }
}
