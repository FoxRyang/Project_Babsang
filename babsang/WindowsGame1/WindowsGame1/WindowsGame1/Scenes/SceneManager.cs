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
using System.Collections;
namespace WindowsGame1.Scenes
{
    public class SceneManager
    {
        public ArrayList scenes;
        public Scene currentScene;
        private int sceneIndex = 0;
        public static SceneManager instance;
        public SceneManager()
        {
            scenes = new ArrayList();
            SceneManager.instance = this;
        }
        public void NextScene()
        {   
            sceneIndex += 1;
            currentScene = (Scene)scenes[sceneIndex];

        }
        public void PrevScene()
        {
            sceneIndex -= 1;
            currentScene = (Scene)scenes[sceneIndex];
        } 
           
        
    }
}
