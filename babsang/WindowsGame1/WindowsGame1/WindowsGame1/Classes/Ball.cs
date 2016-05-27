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
using WindowsGame1.Scenes;
namespace WindowsGame1.Classes
{
    enum BallState
    {
        STABLE,
        SPIKE_L,
        SPIKE_R,
        END,
        SERVE
    }
    class Ball
    {
        public Vector2 position;
        public Vector2 speed;
        public BallState state;
        public Texture2D texture;
        public int time = 0;
        public Body body;

        public Player player1;
        public Player player2;
        public Baby baby;
        
        public void draw(SpriteBatch spriteBatch)
        {
           
           //spriteBatch.Draw(this.texture,                this.position, new Rectangle(0, 0, this.texture.Width, this.texture.Height), Color.White, speed.Y / 3.0f, new Vector2(this.texture.Width / 2, this.texture.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0);
            spriteBatch.Draw(this.texture,
                ConvertUnits.ToDisplayUnits(this.body.Position),
                new Rectangle(0, 0, this.texture.Width, this.texture.Height), 
                Color.White,body.Rotation,
                new Vector2(this.texture.Width / 2, this.texture.Height / 2),
                new Vector2(0.25f, 0.25f),
                SpriteEffects.None,
                0);
        
        }
        public void update(GameTime gt)
        {
            if (baby.state != BabyState.Serve && baby.state != BabyState.End)
            {
                /*
            
                 if(this.body.LinearVelocity.Length()>9.0f)
                 {
                     this.body.LinearVelocity *= 9.0f / this.body.LinearVelocity.Length();
                 }*/
                if (ConvertUnits.ToDisplayUnits(this.body.Position.Y) >= 520 && this.state != BallState.END)
                {
                    this.state = BallState.END;
                    BattleScene.glass_sfx.Play();
                    if (ConvertUnits.ToDisplayUnits(this.body.Position.X) < GraphicsDeviceManager.DefaultBackBufferWidth / 2)
                    {
                        player2.score += 200;
                    }
                    else
                        player1.score += 200;

                    this.body.LinearDamping = 1.5f;

                    this.body.LinearVelocity = new Vector2(this.body.LinearVelocity.X, 0);

                    this.body.IgnoreCollisionWith(player1.body);
                    this.body.IgnoreCollisionWith(player2.body);

                    //this.body.DestroyFixture(this.body.FixtureList[0]);
                    //this.body.Enabled = false;
                }


                /*
                Rectangle ballRect = new Rectangle((int)this.position.X, (int)this.position.Y, this.texture.Width, this.texture.Height);
                Rectangle player1Rect = new Rectangle((int)player1.position.X - player1.texture.Width / 4
                    , (int)player1.position.Y - player1.texture.Height / 4,
                    player1.texture.Width / 2, player1.texture.Height / 2);
                Rectangle player2Rect = new Rectangle((int)player2.position.X - player2.texture.Width / 4
                    , (int)player2.position.Y - player2.texture.Height / 4,
                    player2.texture.Width / 2, player2.texture.Height / 2);

                if ((ballRect.Intersects(player1Rect) && this.time >= 0))
                {
                    if(player1.state == State.ATTACK){
                        this.speed.X = 12;
                        this.state = BallState.SPIKE;
                    }else{
                        this.speed.X *= -1;
                        this.speed.Y *= -1;
                        if (this.state == BallState.SPIKE)
                        {
                            this.state = BallState.STABLE;
                        }
                        this.speed.X /= 9;
                        this.speed.X += player1.speed.X;
                    }
                    this.time = -20;
                    this.state = BallState.END;
                }
                else if (ballRect.Intersects(player2Rect) && this.time <= 0)
                {
                    if (player2.state == State.ATTACK && player2.atk == 0)
                    {
                        this.speed.X = -20;
                        this.state = BallState.SPIKE;
                        player2.atk = 1;
                    }else{
                        this.speed.X *= -1;
                        this.speed.Y *= -1;
                        if (this.state == BallState.SPIKE)
                        {
                            this.state = BallState.STABLE;
                        }
                        this.speed.X /= 9;
                        this.speed.X += player2.speed.X;
                    }
                    this.time = +20;
                }*/
                if (time > 0)
                    time--;
            }
        }
    }
}
