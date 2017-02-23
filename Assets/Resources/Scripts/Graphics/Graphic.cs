using UnityEngine;
using System.Collections;
namespace Cosmos
{
	public class Graphic
	{
		public string name;
		public SpriteAtlas spriteAtlas;
		public Vector2 atlasVector;
		public Vector2 scale;
        public string type;
		public Graphic (string Name,string Type, Vector2 Scale= default(Vector2))
		{
            type = Type;
			Debugger.Log ("Making new graphic: " + Name);
			name = Name;
			if (Scale == default(Vector2)) {
				Scale = new Vector2 (1, 1);
			}
			scale = Scale;
            Finder.GetTable("Graphics").UpdateField(name, new Field("Graphic", this));
            Init ();
			//Finder.graphicDatabase.Add (this);
		}

		public void Init ()
		{
			FindAtlas ();
		}
		private void FindAtlas ()
		{
			spriteAtlas = (SpriteAtlas)Finder.GetTable("SpriteAtlas").GetValue (type+"Atlas","SpriteAtlas");
            if (spriteAtlas != null) {
                atlasVector = spriteAtlas.GetTextureVector(name);
            } else {
                Debugger.Log("No SpriteAtlas found for: " + name);
            }
		}

		public override string ToString ()
		{
			return name;
		}
	}
}