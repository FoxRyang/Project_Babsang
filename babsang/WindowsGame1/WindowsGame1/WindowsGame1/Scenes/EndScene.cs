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
using WindowsGame1.UI;
namespace WindowsGame1.Scenes
{
    public class EndScene:Scene
    {

        Texture2D MotherWin, FatherWin,background;
        public static Who WhoWin;
  
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 600), Color.White);
            if (WhoWin == Who.FATHER)
            {
                spriteBatch.Draw(FatherWin, new Rectangle(0, 0, 800, 600), Color.White);
            }
            else
            {
                spriteBatch.Draw(MotherWin, new Rectangle(0, 0, 800, 600), Color.White);
            }
         
        }
        public override void Update(GameTime gt)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                
                
                SceneManager.instance.PrevScene();
            }
        }
        public override void LoadContent(ContentManager content)
        {
            
            MotherWin = content.Load<Texture2D>("mom_win");
            FatherWin = content.Load<Texture2D>("dad_win");
            background = content.Load<Texture2D>("background");

        }
        public override void Initialize()
        {

        }
        public void md()
        {
            SceneManager.instance.NextScene();
        }

    }
}
