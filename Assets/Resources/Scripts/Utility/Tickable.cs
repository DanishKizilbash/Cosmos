using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public abstract class Tickable
	{
		public int Interval = 1;
		public bool TickRequired=true;
		public bool AddedToTickManager = false;
		public Tickable ()
		{
			TickManager.AddTicker (this);
		}
		public abstract void Tick ();
		public virtual void Destroy ()
		{
			TickRequired = false;
			TickManager.RemoveTicker (this);
		}
	}
}
