using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNAGame
{
    public class SpriteSheetAnimation : Animation
    {
        int frameCounter, switchFrame;

        Vector2 frames, currentFrame;

        public Vector2 Frames
        {
            set { frames = value; }
        }

        public Vector2 CurrentFrame
        {
            set { currentFrame = value; }
        }

        public int FrameWidth
        {
            get { return image.Width / (int)frames.X; }
        }

        public int FrameHeight
        {
            get { return image.Height / (int)frames.Y};
        }

        public override void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {
            base.LoadContent(Content, image, text, position);
            frameCounter = 0;
            switchFrame = 100;
            //frames of the character: 3 frames, 4 angles
            frames = new Vector2(3, 4);
            currentFrame = new Vector2(0, 0);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gametime)
        {
            if (isActive)
            {
                frameCounter += (int)gametime.ElapsedGameTime.TotalMilliseconds;
                if(frameCounter >= switchFrame)
                {
                    frameCounter = 0;
                    currentFrame.X++;
                    if (currentFrame.X * FrameWidth >= image.Width)
                        currentFrame.X = 0;

                    sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
