using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos {
    public static class Debugger {
       
        public static List<string> logs = new List<string>();
        public static void Log(string str = "") {
            logs.Add(str);
        }
        public static void Expose() {
            foreach (string str in logs) {
                Debug.Log(str);
            }
            logs = new List<string>();

        }
    }

}