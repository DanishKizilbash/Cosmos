using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Cosmos {
    public static class Finder {
        private static Profiler finderProfiler = new Profiler("Finder");
        private static Dictionary<string,Table> tables= new Dictionary<string, Table>();
        public static Table GetTable(string name) {
            Table t = null;
            tables.TryGetValue(name, out t);
            //In case table doesn't exist
            if (t == null) t = new Table();
            if (!tables.ContainsKey(name)) tables.Add(name, t);
            //
            return t;

        }
        public static void Init() {
        }
        public static void Update() {
        }
    }
}