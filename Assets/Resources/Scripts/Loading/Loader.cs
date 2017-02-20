using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.IO;

namespace Cosmos {
    public static class Loader {
        public enum LoadState {
            Loading,
            Finished
        }
        public static Profiler LoadProfiler = new Profiler("Loader");
        public static LoadState curLoadState = LoadState.Finished;
        public static void Init() {
            LoadProfiler.Start();
            curLoadState = LoadState.Loading;
            LoadTextures();
            Finder.GetTable("Textures").Print();
            LoadXMLPaths();
            curLoadState = LoadState.Finished;
            LoadProfiler.Report();
        }

        public static void LoadXMLPaths() {
            DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/XMLDefs");
            FileInfo[] info = dir.GetFiles("*.xml");
            var pathname = info.Select(f => f.FullName).ToArray();

            foreach (string f in pathname) {
                int lastSlash = f.LastIndexOf("\\");
                string fileName = f.Substring(lastSlash + 1);
                LoadXML("/Resources/XMLDefs/" + fileName);
            }
        }
        public static void LoadTextures() {
            object[] LoadedTextures = LoadPath("Textures", typeof(Texture2D));
            foreach (object t in LoadedTextures) {
                Texture2D tTex = (Texture2D)t;
                string tempName = tTex.name;
                tTex.name = tempName;
                Debug.Log("Adding Texture: " + tempName);
                Finder.GetTable("Textures").UpdateField(tTex.name, "Texture", tTex);
            }
        }

        public static object[] LoadPath(string path, System.Type type = null) {
            Debug.Log("Loading " + path);
            object[] Obj;
            if (type == null) {
                Obj = Resources.LoadAll(path);
            } else {
                Obj = Resources.LoadAll(path, type);
            }
            if (Obj == null) {
                Debug.Log("Resource not found or Path invalid");
            }
            return Obj;
        }

        public static void LoadXML(string path) {
            //Load xml doc
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.dataPath + path);
            XmlNode root = doc.DocumentElement;
            //
            Dictionary<string, object> data;

            XmlNodeList xmlTypes = root.ChildNodes;
            foreach (XmlNode Type in xmlTypes) {
                foreach (XmlNode Name in Type.ChildNodes) {
                    data = new Dictionary<string, object>();
                    foreach (XmlNode field in Name.ChildNodes) {
                        data.Add(field.Name, field.InnerText);
                    }
                    string type = Type.Name;
                    string name = Name.Name;
                    Finder.GetTable("Defs").UpdateField(type, new Field(name, new Def(name, type, data)));
                }
            }
            Finder.GetTable("Defs").Print();
        }
    }
}