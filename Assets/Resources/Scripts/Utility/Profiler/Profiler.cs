using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos {
 
    public class Profiler {
        public string name;
        public float startTime;
        public float totalLapTime;
        public float lapStartTime;
        public int lapCounter;
        public bool running;
        public Profiler(string Name) {
            name = Name;
        }
        public void Start() {                  
            Reset();
            Debugger.Log("Starting Profiler: " + name + " --"); 
        }
        public void StartLap() {
            if (running) EndLap();
            running = true;   
            lapStartTime = Time.realtimeSinceStartup;
            lapCounter++;
        }
        public void EndLap() {           
            totalLapTime += Time.realtimeSinceStartup - lapStartTime;
            running = false;
        }
       
        public void Reset() {
            EndLap();
            startTime = Time.realtimeSinceStartup;
            totalLapTime = 0;
            lapStartTime = startTime;
            lapCounter = 0;

        }
        public void Report() {
            EndLap();
            float totalTime = Time.realtimeSinceStartup - startTime;
            Debugger.Log("Total : " + (totalTime) + " | Loop time: "+(totalTime-totalLapTime) + " | Laps: " + lapCounter + " | Lap time: "+totalLapTime);
        }

    }
}