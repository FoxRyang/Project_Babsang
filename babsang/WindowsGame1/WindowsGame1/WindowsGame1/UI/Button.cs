using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WindowsGame1.Classes;

namespace WindowsGame1.UI
{
    public class Button
    {
        private Texture2D texture;
        public Vector2 position;
        public Rectangle buttonRect;

        public delegate void MouseDown();
        public MouseDown mouseDown;
        public Button()
        {
         
        }
        public void setTexture(Texture2D texture)
        {
            this.texture = texture;
            this.buttonRect = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

        }
        public void update(GameTime gt)
        {
            MouseState ms = Mouse.GetState();
            if(this.buttonRect.Contains(new Point(ms.X,ms.Y)) &&ms.LeftButton == ButtonState.Pressed)
            {
                this.OnClick();
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, buttonRect, Color.White);
            
        }
        public void OnClick()
        {
            if (mouseDown != null)
            {

                mouseDown();
            }
        }
    }
}
