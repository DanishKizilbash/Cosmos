using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos {
    public static class TextureManager {
        public static Texture2D ClearTexture;
        private static Dictionary<string, List<Texture2D>> textures = new Dictionary<string, List<Texture2D>>();
        private static Dictionary<string, SpriteAtlas> spriteAtlases = new Dictionary<string, SpriteAtlas>();
        public static List<string> textureCategories;
        public static SpriteAtlas DefaultAtlas;
        private static int TileSize;



        public static void Init(int atlasTileSize) {
            TileSize = atlasTileSize;
            FindTextureCategories();
            CreateTransparentTexture();
            PopulateDictionaries();
            MakeDefaultAtlas();
        }
        private static void MakeDefaultAtlas() {
            DefaultAtlas = new SpriteAtlas(TileSize, "DefaultAtlas");
            List<Texture2D> tempList = new List<Texture2D>();
            tempList.Add(ClearTexture);
            DefaultAtlas.AddTextureList(tempList);
        }
        private static void CreateTransparentTexture() {
            Texture2D transparentTex = new Texture2D((int)TileSize, (int)TileSize, TextureFormat.RGBA32, false);
            for (int x = 0; x <= transparentTex.width + 1; x++) {
                for (int y = 0; y <= transparentTex.height + 1; y++) {
                    transparentTex.SetPixel(x, y, Color.clear);
                }
            }
            transparentTex.Apply();
            ClearTexture = transparentTex;

        }
        private static void FindTextureCategories() {
            textureCategories = new List<string>();
            Table textureTable = Finder.GetTable("Textures");
            textureCategories = textureTable.GetFieldKeys();
            Debugger.Log("Found " + textureCategories.Count + " texture categories");
        }
        private static void PopulateDictionaries() {
            Debugger.Log("----Start Texture List Fill----");
            foreach (string cat in textureCategories) {
                List<Dictionary<string, Field>> records = Finder.GetTable("Textures").GetAllRecords(cat);
                List<Texture2D> curTexList = new List<Texture2D>();
                foreach (Dictionary<string, Field> rec in records) {
                    foreach (Field field in rec.Values) {
                        Texture2D tex = (Texture2D)field.value;
                        //tex.name = field.name;
                        curTexList.Add(tex);
                    }
                }
                //
                textures.Add(cat, curTexList);
                Debugger.Log(textures[cat].Count + " " + cat + "s found");
                int maxWidth = 1;
                foreach (Texture2D tex in curTexList) {
                    if (tex.width > maxWidth) {
                        maxWidth = tex.width;
                    }
                }
                
                SpriteAtlas curSpriteAtlas = new SpriteAtlas(maxWidth, cat + "Atlas");
                curSpriteAtlas.AddTextureList(curTexList);
                spriteAtlases.Add(cat, curSpriteAtlas);
                curSpriteAtlas.Init();
                
            }
            Finder.GetTable("SpriteAtlas").Print();
            Debugger.Log("----End Texture List Fill----");
        }

    }
}