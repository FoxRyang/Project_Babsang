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

using WindowsGame1.Scenes;
namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public SceneManager sceneManager;
        MainScene scene1;
        BattleScene bscene;
        RenderTarget2D renderTarget;
        RenderTarget2D renderTarget2;
        Effect effect;
        public static ContentManager content;
        public static GraphicsDevice device;

        Effect blurEffect;
        public static Vector2 ScreenPosition = Vector2.Zero;
        Vector2 ScreenVelocity = Vector2.Zero;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            Game1.device = this.GraphicsDevice;
            Game1.content = this.Content;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //ScreenPosition = new Vector2(200,200);

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            graphics.ApplyChanges();


            scene1 = new MainScene();
             bscene = new BattleScene();
            EndScene escene = new EndScene();

            bscene.game = this;
            scene1.Initialize();
            bscene.Initialize();
            sceneManager = new SceneManager();
            sceneManager.scenes.Add(scene1);
            sceneManager.scenes.Add(bscene);
            sceneManager.scenes.Add(escene);
            sceneManager.currentScene = scene1;
            renderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);
            renderTarget2 = new RenderTarget2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

             for (int i = 0; i < sceneManager.scenes.Count;i++ )
             {
                 ((Scene)sceneManager.scenes[i]).LoadContent(this.Content);
                
             }
             effect = Content.Load<Effect>("testeffect");
             blurEffect = Content.Load<Effect>("blur");
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
       
            sceneManager.currentScene.Update(gameTime);
            ScreenVelocity -= (ScreenPosition - Vector2.Zero) * 0.3f;
            ScreenPosition += ScreenVelocity;
            ScreenVelocity *= 0.8f;
            base.Update(gameTime);
        }

        /// <summary>C:\Users\admin\Desktop\babsang\WindowsGame1\WindowsGame1\WindowsGame1Content\baby.png
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            blurEffect.CurrentTechnique = blurEffect.Techniques["Blur"];
            effect.CurrentTechnique = effect.Techniques["Inverse"];

            // TODO: Add your drawing code here

            //기본 이미지 만들기
            graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            spriteBatch.Begin();
            sceneManager.currentScene.Draw(spriteBatch);
            spriteBatch.End();

            graphics.GraphicsDevice.SetRenderTarget(null);


            //Inverse  적용, 두번째 이미지 만들기
            Texture2D texture = (Texture2D)renderTarget;
            
            
 
           

            
            graphics.GraphicsDevice.SetRenderTarget(renderTarget2);
     
      
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);  
            if(bscene.slow_motion)
            {
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                
                }
            }
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
           
            spriteBatch.End();
            //Inverse 에 블러 적용
            graphics.GraphicsDevice.SetRenderTarget(null);
            Texture2D texture2 = (Texture2D)renderTarget2;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (bscene.slow_motion)
            {
                foreach (EffectPass pass in blurEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                }
            }
            spriteBatch.Draw(texture2, ScreenPosition, Color.White);
    
            spriteBatch.End();
            /*
            ((BattleScene)sceneManager.scenes[1]).DrawDebug();
            */base.Draw(gameTime);
        }
    }
}
