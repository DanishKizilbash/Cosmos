using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos {

    public class Game {
        Profiler profiler = new Profiler("Main Game");

        public Game() {
            profiler.Start();
        }
        public void Start() {
            for (int i = 0; i < 10; i++) {
                EntityManager.AddThing();
            }
        }

        public void Update() {

            //profiler.Start();
            //Finder.GetTable("Entity").Print();
            //profiler.Report();
            

        }
        public void LateUpdate() {
            Debugger.Expose();
        }
    }
}
