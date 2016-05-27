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
using FarseerPhysics;
namespace WindowsGame1.Classes
{


    enum State
    {
        STABLE,
        JUMP,
        WALKING,
        ATTACK,
        BEATEN,
        SERVE,
        SERVING,
        WAIT
    }


   public  enum Who
    {
        MOTHER,
        FATHER
    }

    class Player
    {
        public Vector2 position;
        public Vector2 speed;
        public State state;
        public Texture2D texture;
        public Who who;
        public float A_time = -1;
        private bool dil = true;
        public int score = 1000;
        public int game = 0;
        public int atk = 0;

        public Body body;

        public Baby baby;

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture,
                ConvertUnits.ToDisplayUnits(this.body.Position),
                new Rectangle(0, 0, this.texture.Width, this.texture.Height),
                Color.White,
                0,
                new Vector2(this.texture.Width / 2, this.texture.Height / 2),
                0.15f,
                SpriteEffects.None,
                0);

        }
        public void handleKeyboard(KeyboardState ks)
        {
            if (baby.state != BabyState.Serve && baby.state != BabyState.End && this.state != State.SERVING && this.state != State.WAIT)
            {
                this.speed.X *= 0.9f;
                if (this.who == Who.FATHER)
                {
                    Console.WriteLine(this.state);
                    if (ks.IsKeyDown(Keys.Up) && (this.state == State.STABLE || this.state == State.WALKING))
                    {

                        this.body.ApplyForce(new Vector2(0, ConvertUnits.ToSimUnits(-500 * 50 * 7.5f)));
                    }
                    if (ks.IsKeyDown(Keys.Right))
                    {

                        /*if(this.state == State.ATTACK)
                            this.body.Position += new Vector2(ConvertUnits.ToSimUnits(1), 0);
                        else */
                        this.body.Position += new Vector2(ConvertUnits.ToSimUnits(8), 0);
                        //this.body.ApplyForce(new Vector2(ConvertUnits.ToSimUnits(50*50*5),0));

                    }
                    else if (ks.IsKeyDown(Keys.Left))
                    {
                        /*if (this.state == State.ATTACK)
                            this.body.Position += new Vector2(ConvertUnits.ToSimUnits(-1), 0);
                        else*/
                        this.body.Position += new Vector2(ConvertUnits.ToSimUnits(-8), 0);
                        //   this.body.ApplyForce(new Vector2(ConvertUnits.ToSimUnits(-50*50*5), 0));

                    }
                    if (ks.IsKeyDown(Keys.Enter) && this.state == State.JUMP && dil)
                    {

                        this.state = State.ATTACK;
                        this.A_time = 0.4f;
                        this.dil = false;
                        // this.speed.X = 0;
                        //this.speed.Y = 0;
                        //this.atk = 0;
                        //this.body.LinearVelocity = Vector2.Zero;

                    }
                    if (ks.IsKeyUp(Keys.Enter))
                    {
                        //this.state = State.STABLE;
                        this.dil = true;

                    }

                    Vector2 playerPosition = ConvertUnits.ToDisplayUnits(this.body.Position);
                    Console.Write(playerPosition.X);
                    if (playerPosition.X >= 800)
                    {
                        this.body.Position = new Vector2(ConvertUnits.ToSimUnits(780), this.body.Position.Y);
                    }
                    else if (playerPosition.X < 475)
                    {
                        this.body.Position = new Vector2(ConvertUnits.ToSimUnits(475), this.body.Position.Y);
                    }
                }
                else
                {
                    if (ks.IsKeyDown(Keys.R) && (this.state == State.STABLE || this.state == State.WALKING))
                    {
                        this.body.ApplyForce(new Vector2(0, ConvertUnits.ToSimUnits(-500 * 50 * 7)));
                    }
                    if (ks.IsKeyDown(Keys.G))
                    {
                        this.body.Position += new Vector2(ConvertUnits.ToSimUnits(10), 0);
                        // this.body.ApplyForce(new Vector2(ConvertUnits.ToSimUnits(50 * 50 * 5), 0));
                    }
                    else if (ks.IsKeyDown(Keys.D))
                    {
                        this.body.Position += new Vector2(ConvertUnits.ToSimUnits(-10), 0);
                        // this.body.ApplyForce(new Vector2(ConvertUnits.ToSimUnits(-50 * 50 * 5), 0));
                    }
                    if (ks.IsKeyDown(Keys.Z) && this.state == State.JUMP && dil)
                    {
                        this.state = State.ATTACK;
                        this.A_time = 0.4f;
                        this.dil = false;

                    }
                    if (ks.IsKeyUp(Keys.Z))
                    {
                        //this.state = State.STABLE;
                        this.dil = true;
                    }
                    Vector2 playerPosition = ConvertUnits.ToDisplayUnits(this.body.Position);

                    if (playerPosition.X >= 325)
                    {
                        this.body.Position = new Vector2(ConvertUnits.ToSimUnits(325), this.body.Position.Y);
                    }
                    else if (playerPosition.X < 0)
                    {
                        this.body.Position = new Vector2(ConvertUnits.ToSimUnits(20), this.body.Position.Y);
                    }
                }
            }
        }

        public void update(GameTime gt)
        {
            //  Console.WriteLine(this.state);
            if (this.state != State.SERVING && this.state != State.WAIT)
            {
                if (ConvertUnits.ToDisplayUnits(body.Position.Y) > 443.500671)
                {
                    if (this.state == State.ATTACK)
                        A_time = -1;
                    if (this.body.LinearVelocity == Vector2.Zero)
                        this.state = State.STABLE;
                    else this.state = State.WALKING;
                }
                else if (this.state == State.ATTACK) 
                {
                    A_time -= gt.ElapsedGameTime.Milliseconds / 1000.0f;
                    if (this.A_time < 0)
                    {
                        this.state = State.JUMP;
                    }
                }
                else if (ConvertUnits.ToDisplayUnits(this.body.Position.Y) <= 443.500671)
                {
                    this.state = State.JUMP;
                }

                this.body.ApplyForce(new Vector2(0, ConvertUnits.ToSimUnits(200 * 45)));
            } else
                A_time -= gt.ElapsedGameTime.Milliseconds / 1000.0f;



        }
    }

}
