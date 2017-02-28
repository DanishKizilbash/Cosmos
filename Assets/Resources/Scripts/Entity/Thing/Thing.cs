using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos
{
	public abstract class Thing:Entity
	{
		public PhysicsObject physicsObject;
		public float constructionPointsRequired;
		private bool _isConstructed;
		public bool isConstructed {
			get {
				return _isConstructed;
			}
			set {
				SetConstructed (value);
			}
		}
		//
		public override void Init (string defID)
		{
			base.Init (defID);
			physicsObject = new PhysicsObject (this, new Vector3 (1, 1, 1));
		}
		public override void Destroy ()
		{
			physicsObject.Destroy ();
			base.Destroy ();
		}
		public override void SetAttributes ()
		{
			base.SetAttributes ();
			constructionPointsRequired = Parser.StringToFloat ((string)def.GetAttribute ("ConstructionPoints"));
		}
		public override void Tick ()
		{
			base.Tick ();
		}
		public virtual void SetConstructed (bool value)
		{
			_isConstructed = value;
		}
	}
}