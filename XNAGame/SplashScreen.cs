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
        //KeyboardState keyState;
        SpriteFont spriteFont;
        List<FadeAnimation> fade;
        List<Texture2D> images;

        FileManager fileManager;

        int imageNumber;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            if (spriteFont == null)
                spriteFont = content.Load<SpriteFont>("SpriteFont1");

            imageNumber = 0;

            fileManager = new FileManager();
            fade = new List<FadeAnimation>();
            images = new List<Texture2D>();

            fileManager.LoadContent("Load/Splash.cme", attributes, contents);

            for(int i = 0; i < attributes.Count; i++)
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
                fade[i].LoadContent(content, images[i], "", new Vector2(-384, -216));
                //new Vector2() -> most random numbers I've ever seen (1920:1080 -> 1152:648 = -384, -216)
                fade[i].Scale = 0.6f;
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
            inputManager.Update();

            fade[imageNumber].Update(gameTime);

            if (fade[imageNumber].Alpha == 0.0f)
                imageNumber++;

            if(imageNumber >= fade.Count - 1 || inputManager.KeyPressed(Keys.Z))
            {
                if (fade[imageNumber].Alpha != 1.0f)
                    ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager, fade[imageNumber].Alpha);
                else
                    ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //not needed anymore
            //spriteBatch.DrawString(spriteFont, "SplashScreen", new Vector2(100, 100), Color.Black);
            fade[imageNumber].Draw(spriteBatch);
        }
    }
}