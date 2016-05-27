using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1.Classes
{
    enum BabyState
    {
        Serve,
        Stable,
        Sad,
        Angry,
        End
    }
    class Baby
    {
        public Texture2D texture;
        public Vector2 position;
        public BabyState state;
        public Body body;
        public float time = 23;
        public float cry = -1;

        public void update(GameTime gt)
        {
           if(this.state != BabyState.Serve && this.state != BabyState.End){
               this.time -= gt.ElapsedGameTime.Milliseconds / 1000.0f;
           }
            if(this.cry >0)
               this.cry -= gt.ElapsedGameTime.Milliseconds / 1000.0f;
        }
    }

}
