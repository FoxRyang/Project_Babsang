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

namespace WindowsGame1.Scenes
{
    public abstract class Scene
    {
    
        abstract public void Update(GameTime gt);

        abstract public void LoadContent(ContentManager content);
      
        abstract public void Initialize();

        abstract public void Draw(SpriteBatch batch);

    }
}
