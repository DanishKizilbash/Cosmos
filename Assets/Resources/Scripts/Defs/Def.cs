using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Cosmos {
    public class Def {
        public string type = "";
        public string name = "";
        public Dictionary<string, object> data;
        public Def(string Name, string Type = "", Dictionary<string, object> Data = null) {
            name = Name;
            type = Type;
            if (Data == null) {
                data = new Dictionary<string, object>();
            } else {
                data = Data;
            }
        }
        public virtual void UpdateTable() {
            Table table = Finder.GetTable("Def");
            //
            foreach (KeyValuePair<string, object> d in data) {
                table.UpdateField(name, d.Key, d.Value);
            }
            //
        }
        public virtual void Add(string attribute, string value) {
            data.Add(attribute, value);
        }
        public virtual object GetAttribute(string key) {
            object value;
            if (!data.TryGetValue(key, out value) || value == null) {
                value = "";
            }
            return value;
        }
        public override string ToString() {
            return name;
        }

    }
}