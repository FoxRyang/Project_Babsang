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
    public class MainScene:Scene
    {
        Button btn;
        Texture2D title;
        Random random;
        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(title, new Rectangle(random.Next(-3,3),random.Next(-3,3),800,600), Color.White);
            btn.draw(spriteBatch);
           
        }
        public override void Update(GameTime gt)
        {
            if (random.Next(0, 100) > 96)
            {
                ((BattleScene)SceneManager.instance.scenes[1]).slow_motion = true;
            }
            else
            {
                ((BattleScene)SceneManager.instance.scenes[1]).slow_motion = false;
            }
            btn.update(gt);
        }
        public override void LoadContent(ContentManager content)
        {
            Texture2D btn_texture = content.Load<Texture2D>("start");
          
            btn.setTexture(btn_texture);
            title = content.Load<Texture2D>("title_test");
            btn.position = new Vector2((800-btn_texture.Width)/2,300);
            btn.buttonRect = new Rectangle((800-300)/2,300,300, 250);
        }
        public override void Initialize()
        {
            random = new Random();
            btn = new Button();
            btn.position.Y = 250;
            btn.mouseDown = this.md;
        }
        public void md()
        {
            SceneManager.instance.NextScene();
        }
    

    }
}
