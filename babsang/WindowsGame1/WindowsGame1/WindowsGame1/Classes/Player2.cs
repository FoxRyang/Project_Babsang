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

namespace WindowsGame1.Classes
{
    /*enum State
    {
        STABLE,
        JUMP,
        WALKING,
        ATTACK,
        BEATEN,
        SERVE
    }*/

    class Player2
    {
        public Vector2 position;
        public Vector2 speed;
        public State state;
        public Texture2D texture;

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.position, Color.White);
        }
        public void handleKeyboard(KeyboardState ks)
        {

            this.speed.X *= 0.9f;
            if (ks.IsKeyDown(Keys.Up) && (this.state == State.STABLE || this.state == State.WALKING))
            {
                this.speed.Y = -5;
            }
            else if (ks.IsKeyDown(Keys.Down))
            {
                this.speed.Y = 5;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                this.speed.X = 5;
            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                this.speed.X = -5;
            }
        }

        public void update(GameTime gt)
        {
            this.position += this.speed;
            this.speed.Y += 0.1f;
            Console.WriteLine(this.state);
            if (this.position.Y > 300)
            {
                this.position.Y = 300;
                speed.Y = 0;
            }
            if (speed == Vector2.Zero && this.position.Y == 300)
            {
                this.state = State.STABLE;
            }
            else if (position.Y != 300)
            {
                this.state = State.JUMP;
            }
            else if (speed.X != 0)
            {
                this.state = State.WALKING;
            }



        }

    }
}
