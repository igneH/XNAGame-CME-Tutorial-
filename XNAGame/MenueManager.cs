using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace XNAGame
{
    public class MenuManager
    {
        List<string> animationTypes, linkType, linkID, menuItems;
        List<Texture2D> menuImages;
        List<List<Animation>> animation;

        ContentManager content;
        FileManager fileManager;

        Vector2 position;

        List<List<string>> attributes, contents;

        List<Animation> tempAnimation;

        Rectangle source;

        SpriteFont font;

        int itemNumber, axis;
        string align;

        private void SetMenuItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (menuImages.Count == i)
                    menuImages.Add(ScreenManager.Instance.NullImage);              
            }

            for (int i = 0; i < menuImages.Count; i++)
            {
                if (menuItems.Count == i)
                    menuItems.Add("");          
            }
        }

        private void SetAnimations()
        {
            Vector2 dimensions = Vector2.Zero;
            Vector2 pos = Vector2.Zero;

            if (align.Contains("Center"))
            {
                for(int i = 0; i < menuImages.Count; i++)
                {
                    dimensions.X += font.MeasureString(menuItems[i]).X + menuImages[i].Width;
                    dimensions.Y += font.MeasureString(menuItems[i]).Y + menuImages[i].Height;
                }

                if(axis == 1)
                {
                    pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;
                }
                else if(axis == 2)
                {
                    pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                }
            }
            else
            {
                pos = position;
            }

            tempAnimation = new List<Animation>();
            for (int i = 0; i < menuImages.Count; i++)
                {

                dimensions = new Vector2(font.MeasureString(menuItems[i]).X + menuImages[i].Width,
                   font.MeasureString(menuItems[i]).Y +  menuImages[i].Height);

                if (axis == 1)
                    pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                else
                    pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;

                for (int j = 0; j < animationTypes.Count; j++)
                {
                    switch (animationTypes[j])
                    {
                        case "Fade":
                            tempAnimation.Add(new FadeAnimation());
                            tempAnimation[tempAnimation.Count - 1].LoadContent(content, menuImages[i], menuItems[i], pos);
                            tempAnimation[tempAnimation.Count - 1].Font = font;
                            break;
                    }
                }
            
                if(tempAnimation.Count > 0)
                    animation.Add(tempAnimation);
                tempAnimation = new List<Animation>();

                if(axis == 1)
                    pos.X += dimensions.X;
                else
                    pos.Y  += dimensions.Y;
            }
        }

        public void LoadContent(ContentManager content, string id)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            menuItems = new List<string>();
            animationTypes = new List<string>();
            menuImages = new List<Texture2D>();
            animation = new List<List<Animation>>();
            linkID = new List<string>();
            linkType = new List<string>();

            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            itemNumber = 0;

            position = Vector2.Zero;
            fileManager = new FileManager();
            fileManager.LoadContent("Load/Menus.cme", attributes, contents);

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Font":
                            font = content.Load<SpriteFont>(contents[i][j]);
                            break;
                        case "Items":
                            menuItems.Add(contents[i][j]);
                            break;
                        case "Image":
                            menuImages.Add(content.Load<Texture2D>(contents[i][j]));
                            break;
                        case "Axis":
                            axis = int.Parse(contents[i][j]);
                            break;
                        case "Position":
                            string[] temp = contents[i][j].Split(' ');
                            position = new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));
                            break;
                        case "Source":
                            temp = contents[i][j].Split(' ');
                            source = new Rectangle(int.Parse(temp[0]), int.Parse(temp[1]), int.Parse(temp[2]), int.Parse(temp[3]));
                            break;
                        case "Animation":
                            animationTypes.Add(contents[i][j]);
                            break;
                        case "Align":
                            align = contents[i][j];
                            break;
                        case "LinkType":
                            linkType.Add(contents[i][j]);
                            break;
                        case "LinkID":
                            linkID.Add(contents[i][j]);
                            break;
                    }
                }
            }
            SetMenuItems();
            SetAnimations();
        }

        public void UnloadContent()
        {
            content.Unload();
            fileManager = null;
            position = Vector2.Zero;
            animation.Clear();
            menuItems.Clear();
            animationTypes.Clear();
        }

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            if(axis == 1)
            {
                if (inputManager.KeyPressed(Keys.Right, Keys.D))
                    itemNumber++;
                else if (inputManager.KeyPressed(Keys.Left, Keys.A))
                    itemNumber--;
            }
            else
            {
                if (inputManager.KeyPressed(Keys.Down, Keys.S))
                    itemNumber++;
                else if (inputManager.KeyPressed(Keys.Up, Keys.W))
                    itemNumber--;
            }

            if(inputManager.KeyPressed(Keys.Enter, Keys.Z))
            {
                if (linkType[itemNumber] == "Screen")
                {
                    Type newClass = Type.GetType("XNAGame."+linkID[itemNumber]);
                    ScreenManager.Instance.AddScreen((GameScreen)Activator.CreateInstance(newClass), inputManager);
                }
            }

            if (itemNumber < 0)
                itemNumber = 0;
            else if (itemNumber > menuItems.Count -1)
                itemNumber = menuItems.Count -1;

            for(int i = 0; i < animation.Count; i++)
            {
                for(int j = 0; j < animation[i].Count; j++)
                {
                    if (itemNumber == i)
                        animation[i][j].IsActive = true;
                    else
                        animation[i][j].IsActive = false;
                    animation[i][j].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < animation.Count; i++)
            {
                for(int j = 0; j < animation[i].Count; j++)
                {
                    animation[i][j].Draw(spriteBatch);
                }
            }
        }
    }
}
