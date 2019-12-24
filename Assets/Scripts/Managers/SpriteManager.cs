using Assets.Scripts.Gui;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SpriteManager : IGameManager
    {
        private static SpriteManager instance = null;

        public static SpriteManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpriteManager();
                }
                return instance;
            }
        }

        private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

        public void SetSprite(Image image, string spriteName)
        {
            if (spriteCache.ContainsKey(spriteName))
            {
                image.sprite = spriteCache[spriteName];
            }
            else
            {
                Sprite sprite = Resources.Load<Sprite>("Icons/" + spriteName);
                image.sprite = sprite;

                spriteCache.Add(spriteName, sprite);
            }
        }
    }
}