using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
namespace Cosmos {
    public class Entity {
        public Def def;
        //
        public Graphic mainGraphic;
        public MeshDisplay meshDisplay;
        public Vector3 position;
        public Vector2 scale;
        public float rotation;
        public Vector3 rotationPoint;
        public Rect rect {
            get {
                return new Rect(position.x, position.y, scale.x, scale.y);
            }
        }
        //
        public bool visible = true;
        //
        public string name = "";
        public bool selected = false;
        //
        public virtual void Init(string defID = "Default") {
            if (name == "") {
                name = "Entity" + Finder.GetTable("Entity").count.ToString();
            }
            def = (Def)Finder.GetTable("Defs").GetValue("Entity", defID);
            if (def == null) {
                Debugger.Log("No def found for: " + defID);
            } else {
                string defString = def.GetAttribute("Texture").ToString();
                mainGraphic = (Graphic)Finder.GetTable("Graphics").GetValue(defString, "Graphic");
                if (mainGraphic == null) {
                    mainGraphic = new Graphic(defString,def.type);
                }
            }

            UpdateTable();
        }
        public virtual void Update() {
        }
        public virtual void Destroy() {
        }
        public virtual void UpdateTable() {
            Table table = Finder.GetTable("Entity");
            //
            table.UpdateField(name, "entity", this);
            table.UpdateField(name, "position", position);
            table.UpdateField(name, "scale", scale);
            table.UpdateField(name, "rotation", rotation);
            table.UpdateField(name, "visible", visible);
            //
        }

        public override string ToString() {
            return name;

        }
    }
}