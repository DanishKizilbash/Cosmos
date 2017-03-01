using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
namespace Cosmos {
    public class Entity : Tickable {
        public Def def;
        //Graphics        
        public Material material;
        public Vector2 scale = Vector2.one;
        public bool visible = true;
        public GameObject gameObject;
        public MeshRenderer meshRenderer;
        // Orientation
        public Vector3 position;
        public float rotation;
        public Vector3 rotationPoint;
        public Rect rect {
            get {
                return new Rect(position.x, position.y, scale.x, scale.y);
            }
        }
        //
        public string name = "";
        public bool selected = false;
        //
        public virtual void Init(string defID = "Default") {
            def = (Def)Finder.GetTable("Defs").GetValue(this.GetType().Name, defID);
            if (def == null) {
                Debugger.Log("No def found for: " + this.GetType().Name+ "_" +defID);
                Destroy();
            } else {
                SetAttributes();
            }
            DrawManager.RenderEntity(this);
        }
        public override void Tick() {
            Update();
        }
        public virtual void Update() {
        }
        public override void Destroy() {
        }
        public virtual void SetAttributes() {
            name = (string)def.GetAttribute("Name");
            if (name == "") {
                name = "Entity" + Finder.GetTable("Entity").count.ToString();
            }
            // Def attributes
            string defString = def.GetAttribute("Texture").ToString();
            // Textures
            Table texTable = Finder.GetTable("Textures");
            material = (Material)texTable.GetValue(defString, "Material");
            //Update table
            UpdateTable();
        }
        public virtual void UpdateTable() {
            Table table = Finder.GetTable("Entity");
            //
            table.UpdateField(name, "entity", this);
            table.UpdateField(name, "position", position);
            table.UpdateField(name, "scale", scale);
            table.UpdateField(name, "rotation", rotation);
            table.UpdateField(name, "visible", visible);
            table.UpdateField(name, "material", material);
            //
        }

        public override string ToString() {
            return name;

        }
    }
}