// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

namespace Project.Scripts.Multiplayer.Generated
{
	public partial class FoodState : Schema {
		[Type(0, "string")]
		public string type = default(string);

		[Type(1, "ref", typeof(Vector2Float))]
		public Vector2Float position = new Vector2Float();
	}
}

