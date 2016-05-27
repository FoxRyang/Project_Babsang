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
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.DebugViews;
using FarseerPhysics.Factories;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1.Scenes
{
    class BattleScene:Scene
    {
        Player player1;
        Player player2;
        Ball[] ball = new Ball[10];
        Baby baby;
        Table table = new Table();
        ContentManager Content;
        Texture2D background;


        World world;
        Body floor;
        DebugViewXNA view;
        public Game1 game;
        Camera2D Camera;
        SpriteFont font;
        SpriteFont help_font;
        float slow_time = 0;
        int N = 10;   
        int Dice_number = 1;
        int count;

        public static SoundEffect glass_sfx;
        public static SoundEffect hit1_sfx;
        public static SoundEffect hit2_sfx;
              Song ending;
  
     
        Song bgm;

        public string help = " Player 1 \n up/left/down/right : R/D/F/G \n serve : Z\n Player 2 \n up/left/down/right : [up]/[left]/[down]/[right]  \n serve : [enter]";
       public bool slow_motion = false;
     
        public override void Update(GameTime gt)
        {
            //////////키보드/////////
            KeyboardState ks = Keyboard.GetState();
            player1.handleKeyboard(ks);
            player2.handleKeyboard(ks);
            // TODO: Add your update logic here

            ////////로직 실행//////////

            if (baby.state != BabyState.Serve && baby.state != BabyState.End)
            {
                if (player1.state != State.SERVING)
                {
                    if (player1.A_time > 0.3f)
                    {
                        player1.texture = this.Content.Load<Texture2D>("dad_hit");
                    
                    }
                    else if (player1.A_time < 0)
                    {
                        player1.texture = this.Content.Load<Texture2D>("dad");
                    }

                    if (player2.A_time > 0.3f)
                    {
                        player2.texture = this.Content.Load<Texture2D>("mom_hit");
                    }
                    else if (player2.A_time < 0)
                    {
                        player2.texture = this.Content.Load<Texture2D>("mom");
                    }

                    if (((int)(baby.time * 2)) % 2 == 0 && baby.cry < 0)
                    {
                        baby.texture = this.Content.Load<Texture2D>("baby");
                    }
                    else if (baby.cry < 0)
                        baby.texture = this.Content.Load<Texture2D>("baby_2");
                }
                else
                {

                    if (player1.A_time > 1.85)
                    {
                        player1.texture = this.Content.Load<Texture2D>("dad_stand");
                    }
                    else if (player1.A_time > 1.7)
                    {
                      
                        player1.texture = this.Content.Load<Texture2D>("dad_table_2");
                    }
                    else if (player1.A_time > 1.3)
                    {
                        table.body.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(3000, -10000)));
                        table.body.AngularVelocity = 20.0f;
                        for (int i = 0; i < N; i++)
                        {
                            ball[i].body.ApplyForce(ConvertUnits.ToSimUnits(new Vector2(50 + 200 * i, -1000)));

                        }      
                        show_help = false;
                        player1.texture = this.Content.Load<Texture2D>("dad_table_3");
                        
                    }
                    else if (player1.A_time > 0.9)
                    {
                        player1.texture = this.Content.Load<Texture2D>("dad_table_4");
                        player2.texture = this.Content.Load<Texture2D>("mom_stand_1");

                    }
                    else if (player1.A_time > 0.0)
                    {
                        player1.texture = this.Content.Load<Texture2D>("dad_table_5");
                        table.body.CollisionCategories = Category.None;
                    }
                    else
                    {
                        player1.state = State.STABLE;
                        player2.state = State.STABLE;
                        for (int i = 0; i < N; i++)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (i != j)
                                    ball[i].body.RestoreCollisionWith(ball[j].body);
                            }
                        }
                    }
                        
                }

                floor.LinearVelocity = Vector2.Zero;


               
                

                if (baby.time <= 0)
                {
                    if (player1.score > player2.score)
                    {
                        EndScene.WhoWin = Who.FATHER;
                    }
                    else
                    {
                        EndScene.WhoWin = Who.MOTHER;
                    }
                    
                    SceneManager.instance.NextScene();
                    MediaPlayer.Play(ending);
               }

                //////업데이트//////
                player1.update(gt);
                player2.update(gt);

                count = 0;
                for (int i = 0; i < N; i++)
                {
                    if (ball[i].state == BallState.END)
                        count++;
                }
                if (count == N)
                    baby.time = 0;
              
               
            } ////////서브 한다/////////////
            else if(player1.state == State.SERVE && Keyboard.GetState().IsKeyDown(Keys.Z))   ////////////////서브하는 중////////////
            {
                player1.state = State.SERVING;
                player1.A_time = 2;
                          
                baby.state = BabyState.Stable;

            }

            ///////////슬로우 이펙터////////////////
            if (baby.time < 22.55 && baby.time > 21)
            {
                world.Step(Math.Min((float)gt.ElapsedGameTime.TotalSeconds, (0.001f)));                
            }
            else
            {
                world.Step(Math.Min((float)gt.ElapsedGameTime.TotalSeconds, (1f / 30f)));                

                slow_time -= gt.ElapsedGameTime.Milliseconds / 1000.0f;
            }
                
                
            

            if (slow_time <= 0)
            {
                slow_motion = false;
            }
            for (int i = 0; i < N; i++)
                ball[i].update(gt);
            baby.update(gt);

        }

        public override void LoadContent(ContentManager content)
        {
            this.Content = content;
            player1.texture = content.Load<Texture2D>("dad_sat");
            player2.texture = content.Load<Texture2D>("mom_sat");
            table.texture = content.Load<Texture2D>("table");
            ending = content.Load<Song>("co");
            
            
            font = content.Load<SpriteFont>("SpriteFont1");
            for (int i = 0; i < N; i++)
            {
                ball[i].texture = content.Load<Texture2D>("ball");
            }
            ball[0].texture = content.Load<Texture2D>("ttukbaegi");
            ball[1].texture = content.Load<Texture2D>("rice_1");
            ball[2].texture = content.Load<Texture2D>("kimchi");
            ball[3].texture = content.Load<Texture2D>("kimchi");
            ball[4].texture = content.Load<Texture2D>("ttukbaegi");
            ball[5].texture = content.Load<Texture2D>("rice_1");
            ball[6].texture = content.Load<Texture2D>("rice_2");
            ball[7].texture = content.Load<Texture2D>("kimchi");
            ball[8].texture = content.Load<Texture2D>("ttukbaegi");
            ball[9].texture = content.Load<Texture2D>("kimchi");

            baby.texture = this.Content.Load<Texture2D>("baby");

            background = this.Content.Load<Texture2D>("background");




            view = new DebugViewXNA(world);
            view.AppendFlags(DebugViewFlags.Shape);
            view.RemoveFlags(DebugViewFlags.Joint);
            view.DefaultShapeColor = Color.White;
            view.SleepingShapeColor = Color.LightGray;
            view.LoadContent(game.GraphicsDevice, Content);
            if (Camera == null)
            {
                Camera = new Camera2D(game.GraphicsDevice);
            }
            else
            {
                Camera.ResetCamera();
            }
            glass_sfx = content.Load<SoundEffect>("glass");

            hit1_sfx = content.Load<SoundEffect>("hit1");
            hit2_sfx = content.Load<SoundEffect>("hit2");
            bgm = content.Load<Song>("bgm");
            MediaPlayer.Play(bgm);
            MediaPlayer.IsRepeating = true;

            help_font = content.Load<SpriteFont>("SpriteFont2");
            
           

        }

        public override void Initialize()
        {
            //////플레이어////////////
            player1 = new Player();
            player1.who = Who.MOTHER;
            player1.position = new Vector2(50, 100);
            player1.speed = Vector2.Zero;
            player1.state = State.SERVE;      

            player2 = new Player();
            player2.who = Who.FATHER;
            player2.position = new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth - 50, 500);
            player2.speed = Vector2.Zero;
            player2.state = State.WAIT;

            baby = new Baby();
            baby.position = new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth / 2 - 30, 450);
            baby.state = BabyState.Serve;

            world = new World(ConvertUnits.ToSimUnits(new Vector2(0, 500f)));

            table.state = TableState.Serve;
            table.position = new Vector2(170,500);

            ////////////맵////////

            floor = new Body(world);
            floor.Position = new Vector2(ConvertUnits.ToSimUnits(0),ConvertUnits.ToSimUnits(0));
            floor.BodyType = BodyType.Static;
            floor.Restitution = 0.5f;
            EdgeShape floorShape = new EdgeShape(new Vector2(ConvertUnits.ToSimUnits(0), ConvertUnits.ToSimUnits(550)), new Vector2(ConvertUnits.ToSimUnits(1280), ConvertUnits.ToSimUnits(550)));
             floor.CreateFixture(floorShape);

             Body ceilingwall = new Body(world);
             ceilingwall.Position = new Vector2(ConvertUnits.ToSimUnits(0), ConvertUnits.ToSimUnits(0));
             ceilingwall.BodyType = BodyType.Static;
             ceilingwall.Restitution = 0.5f;
             EdgeShape ceilingShape = new EdgeShape(new Vector2(ConvertUnits.ToSimUnits(0), ConvertUnits.ToSimUnits(0)), new Vector2(ConvertUnits.ToSimUnits(1280), ConvertUnits.ToSimUnits(0)));
             ceilingwall.CreateFixture(ceilingShape);


             Body leftwall = new Body(world);
             leftwall.Position = new Vector2(ConvertUnits.ToSimUnits(0), ConvertUnits.ToSimUnits(0));
             leftwall.BodyType = BodyType.Static;
             leftwall.IsStatic = true;
             leftwall.Restitution = 0.5f;
             EdgeShape leftwallshape = new EdgeShape(new Vector2(ConvertUnits.ToSimUnits(0), ConvertUnits.ToSimUnits(0)), new Vector2(ConvertUnits.ToSimUnits(0), ConvertUnits.ToSimUnits(800)));
             leftwall.CreateFixture(leftwallshape);

             Body rightwall = new Body(world);
             rightwall.Position = new Vector2(ConvertUnits.ToSimUnits(0), ConvertUnits.ToSimUnits(0));
             rightwall.BodyType = BodyType.Static;
             rightwall.IsStatic = true;
             rightwall.Restitution = 0.5f;
             EdgeShape rightwallshape = new EdgeShape(new Vector2(ConvertUnits.ToSimUnits(800), ConvertUnits.ToSimUnits(0)), new Vector2(ConvertUnits.ToSimUnits(800), ConvertUnits.ToSimUnits(800)));
             leftwall.CreateFixture(rightwallshape);

             /*Body midwall = new Body(world);
             midwall.Position = new Vector2(ConvertUnits.ToSimUnits(0), ConvertUnits.ToSimUnits(0));
             midwall.BodyType = BodyType.Static;
             midwall.IsStatic = true;
             midwall.Restitution = 0.5f;
             EdgeShape midwallshape = new EdgeShape(new Vector2(ConvertUnits.ToSimUnits(400), ConvertUnits.ToSimUnits(400)), new Vector2(ConvertUnits.ToSimUnits(400), ConvertUnits.ToSimUnits(800)));
             midwall.CreateFixture(midwallshape);*/

             Body uppermidwall = new Body(world);
             uppermidwall.Position = new Vector2(ConvertUnits.ToSimUnits(0), ConvertUnits.ToSimUnits(0));
             uppermidwall.BodyType = BodyType.Static;
             uppermidwall.IsStatic = true;
             uppermidwall.Restitution = 0.5f;
             EdgeShape uppermidwallshape = new EdgeShape(new Vector2(ConvertUnits.ToSimUnits(400), ConvertUnits.ToSimUnits(0)), new Vector2(ConvertUnits.ToSimUnits(400), ConvertUnits.ToSimUnits(400)));
             uppermidwall.CreateFixture(uppermidwallshape);


            //////////바디//////////

            player1.body = BodyFactory.CreateRectangle(world, 0.7f, 2, 5.0f);
            player1.body.FixedRotation = true;
            player1.body.Position = new Vector2(ConvertUnits.ToSimUnits(40), ConvertUnits.ToSimUnits(480));
            player1.body.BodyType = BodyType.Dynamic;
     


            player2.body = BodyFactory.CreateRectangle(world, 0.7f, 2, 5.0f);
            player2.body.FixedRotation = true;
            player2.body.Position = new Vector2(ConvertUnits.ToSimUnits(750), ConvertUnits.ToSimUnits(480));
            player2.body.BodyType = BodyType.Dynamic;

            baby.body = BodyFactory.CreateRectangle(world, 0.7f, 1, 5.0f);
            baby.body.FixedRotation = true;
            baby.body.Position = new Vector2(ConvertUnits.ToSimUnits(400), ConvertUnits.ToSimUnits(500));
            baby.body.BodyType = BodyType.Static;

            table.body = BodyFactory.CreateRectangle(world, 1.7f, 0.7f, 0.5f);
            table.body.Position = new Vector2(ConvertUnits.ToSimUnits(170), ConvertUnits.ToSimUnits(500));
            table.body.BodyType = BodyType.Dynamic;

 
            for (int i = 0; i < N; i++)
            {
                ball[i] = new Ball();
                ball[i].position = new Vector2(100 + i * 30, 450);
                ball[i].speed = new Vector2(0, 0);
                ball[i].state = BallState.STABLE;
                ball[i].player1 = player1;
                ball[i].player2 = player2;
                ball[i].body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(18f), 1.0f, ConvertUnits.ToSimUnits( ball[i].position));
                
                ball[i].body.BodyType = BodyType.Dynamic;
                ball[i].body.Position = new Vector2(ConvertUnits.ToSimUnits(135 + i * 10), ConvertUnits.ToSimUnits(460));
                ball[i].body.Mass = 0.1f;
                ball[i].body.Restitution = 0.8f;

                ball[i].body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
                ball[i].body.IgnoreCollisionWith(uppermidwall);
                ball[i].body.ApplyForce(new Vector2(0,0));

                for (int j = 0; j < i; j++)
                {
                    if (i != j)
                        ball[i].body.IgnoreCollisionWith(ball[j].body);
                }

                ball[i].baby = baby;
                
            }

            player1.baby = baby;
            player2.baby = baby;
            table.baby = baby;

           
            

      
            
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            
            if(fixtureB.Body == table.body)
            {
                hit1_sfx.Play();
            }
            int i;
            for (i = 0; fixtureA.Body != ball[i].body; i++){}

            if ((fixtureB.Body == player1.body || fixtureB.Body == player2.body))
            {
                
                               // fixtureA.Body.LinearVelocity *= 0.9f;
                if(fixtureB.Body == player1.body && player1.state == State.ATTACK)
                {

                    //play hit sound
                    Random random = new Random();

                    if (random.Next(0, 1) == 0)
                    {
                        hit1_sfx.Play();
                    }
                    else
                    {
                        hit2_sfx.Play();
                    }
                    Game1.ScreenPosition += new Vector2(random.Next(-80, 80), random.Next(-80, 80));

                    ball[i].state = BallState.SPIKE_L;
                    //fixtureA.IgnoreCollisionWith(fixtureB);
                    fixtureA.Body.LinearVelocity = Vector2.Zero;
                    fixtureA.Body.Position += ConvertUnits.ToSimUnits(new Vector2(120, 0));
                    
                    if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.G))
                    {
                        
                        if (Keyboard.GetState().IsKeyDown(Keys.R))
                        {
                            fixtureA.Body.LinearVelocity = new Vector2(11, -7);
                            
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.F))
                        {

                            fixtureA.Body.LinearVelocity = new Vector2(11, 7);
                        }else
                            fixtureA.Body.LinearVelocity = new Vector2(12, 0);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        fixtureA.Body.LinearVelocity = new Vector2(7, -10);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.F))
                    {
                        fixtureA.Body.LinearVelocity = new Vector2(7, 10);
                    }else
                    {
                        fixtureA.Body.LinearVelocity = new Vector2(6, 0);
                    }


                    
                    slow_motion = true;
                    slow_time = 0.15f;
                }
                else if (fixtureB.Body == player2.body && player2.state == State.ATTACK)
                {

                    // play hit sound

                    Random random = new Random();

                    if (random.Next(0, 1) == 0)
                    {
                        hit1_sfx.Play();
                    }
                    else
                    {
                        hit2_sfx.Play();
                    }
                    Game1.ScreenPosition += new Vector2(random.Next(-80, 80), random.Next(-80, 80));
                    ball[i].state = BallState.SPIKE_R;
                    if (fixtureA.Body.Position.X > fixtureB.Body.Position.X)
                    {
                    }
                    //fixtureA.IgnoreCollisionWith(fixtureB);
                    fixtureA.Body.LinearVelocity = Vector2.Zero;
                    fixtureA.Body.Position += ConvertUnits.ToSimUnits(new Vector2(-120, 0));
                    
                    
                    if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        
                        if (Keyboard.GetState().IsKeyDown(Keys.Up))
                        {
                            fixtureA.Body.LinearVelocity = new Vector2(-11, -7);
                            
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                        {
                            fixtureA.Body.LinearVelocity = new Vector2(-11, 7);
                        }else
                            fixtureA.Body.LinearVelocity = new Vector2(-12, 0);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        fixtureA.Body.LinearVelocity = new Vector2(-7, -10);
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        fixtureA.Body.LinearVelocity = new Vector2(-7, 10);
                    }else
                    {
                        fixtureA.Body.LinearVelocity = new Vector2(-6, 0);
                    }


                    
                    slow_motion = true;
                    slow_time = 0.15f;
                }
                else
                {
                    if (fixtureA.Body.LinearVelocity.Y > 0)
                        fixtureA.Body.LinearVelocity = new Vector2(fixtureA.Body.LinearVelocity.X, fixtureA.Body.LinearVelocity.Y * -1.5f);
                    if (fixtureA.Body.LinearVelocity.Length() > 9.0f)
                    {
                        fixtureA.Body.LinearVelocity *= 9.0f / fixtureA.Body.LinearVelocity.Length();
                    }
                    else if (fixtureA.Body.LinearVelocity.Length() > 0.0f && fixtureA.Body.LinearVelocity.Length() < 5.0f)
                    {
                        fixtureA.Body.LinearVelocity *= 5.0f / fixtureA.Body.LinearVelocity.Length();
                    }
                    if (fixtureB.Body == player1.body && ball[i].state == BallState.SPIKE_R)
                    {
                        player1.score -= 50;
                    }
                    else if(ball[i].state == BallState.SPIKE_L)
                    {
                        player2.score -= 50;
                    }
                    ball[i].state =BallState.STABLE;
                }
            }else if (fixtureB.Body == baby.body) {
                baby.texture = this.Content.Load<Texture2D>("baby_crying_1");
                baby.cry = 0.3f;
                baby.time -= 0.5f;
                ball[i].state = BallState.END;
                
                ball[i].body.LinearDamping = 1.5f;
                ball[i].body.LinearVelocity = new Vector2(ball[i].body.LinearVelocity.X, 0);
                ball[i].body.IgnoreCollisionWith(player1.body);
                ball[i].body.IgnoreCollisionWith(player2.body);
            }
            return true;
        }

        Boolean show_help = true;
        public override void Draw(SpriteBatch batch)
        {
            
            batch.Draw(background, new Rectangle(0,0,800,600), Color.White);

            batch.DrawString(font, player1.score.ToString(), new Vector2(100, 100), Color.White);

            batch.DrawString(font, player2.score.ToString(), new Vector2(600, 100), Color.White);

            batch.DrawString(font, ((int)baby.time).ToString(), new Vector2(350, 50), Color.White);

            player1.draw(batch);
            player2.draw(batch);

             batch.Draw(baby.texture,
                    baby.position,
                    new Rectangle(0, 0,
                        baby.texture.Width,
                        baby.texture.Height),
                    Color.White,
                    0f,
                    new Vector2(0, 0),
                    0.6f,
                    SpriteEffects.None,
                    1);

            table.draw(batch);

            for (int i = 0; i < N; i++)
                ball[i].draw(batch);

            if (show_help)
            {
                
                batch.DrawString(help_font, help, new Vector2(200,200), Color.Red);
            }
        }
        public void DrawDebug()
        {
            Matrix projection = Camera.SimProjection;
            Matrix view2 = Camera.SimView;

            view.RenderDebugData(ref projection, ref view2);
        }
    }
}
