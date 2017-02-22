using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos {
    public static class EntityManager {
        public static Table table {
        get {
                return Finder.GetTable("Entity");
            }
        }
        public static void Init() {
        }
        public static void AddEntity() {
           Entity entity = new Entity();
            entity.Init();
        }
    }
}