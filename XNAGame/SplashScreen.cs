using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XNAGame
{
    //A screen where f.e. the prodcution studios name shows up when starting the game
    public class SplashScreen : GameScreen
    {
        KeyboardState keyState;
        SpriteFont spriteFont;
        List<FadeAnimation> fade;
        List<Texture2D> images;

        FileManager fileManager;

        int imageNumber;

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            if (spriteFont == null)
                spriteFont = content.Load<SpriteFont>("SpriteFont1");

            imageNumber = 0;

            fileManager = new FileManager();
            fade = new List<FadeAnimation>();
            images = new List<Texture2D>();

            fileManager.LoadContent("Load/Splash.cme", attributes, contents);

            for(int i=0; i < attributes.Count; i++)
            {
                for(int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Image":
                            images.Add(content.Load<Texture2D>(contents[i][j]));
                            fade.Add(new FadeAnimation());
                            break;
                    }
                }
            }
            for(int i = 0; i < fade.Count; i++)
            {
                fade[i].LoadContent(content, images[i], "", Vector2.Zero);
                fade[i].Scale = 1.7f;
                fade[i].IsActive = true;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            //if (keyState.IsKeyDown(Keys.Z))
            //ScreenManager.Instance.AddScreen(new TitleScreen());
            fade[imageNumber].Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(spriteFont, "SplashScreen", new Vector2(100, 100), Color.Black);
            fade[imageNumber].Draw(spriteBatch);
        }
    }
}
