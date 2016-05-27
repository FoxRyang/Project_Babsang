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
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics;

namespace WindowsGame1.Classes
{
    enum TableState{
        Serve,
        Power,
        Bye
    }
    class Table
    {
        public Body body;
        public Texture2D texture;
        public TableState state;
        public Vector2 position;
        public Baby baby;

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture,
                ConvertUnits.ToDisplayUnits(this.body.Position),
                new Rectangle(0, 0, this.texture.Width, this.texture.Height),
                Color.White, body.Rotation,
                new Vector2(this.texture.Width / 2, this.texture.Height / 2),
                new Vector2(0.8f, 0.8f),
                SpriteEffects.None,
                0);

        }
    }
}
