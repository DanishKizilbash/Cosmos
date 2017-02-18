using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Cosmos
{

	public class Game
	{
        Profiler profiler = new Profiler("Main Game");

		public Game ()
		{
            profiler.Start();
		}
		public void Start ()
		{
		}

		public void Update ()
		{
            profiler.Report();
            profiler.Start();
           
		}
        public void LateUpdate() {
            Debugger.Expose();
        }
	}
}
