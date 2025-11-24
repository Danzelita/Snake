// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.46
// 

using Colyseus.Schema;

namespace Project.Scripts.Multiplayer.Generated
{
	public partial class Player : Schema {
		[Type(0, "string")]
		public string sessionId = default(string);

		[Type(1, "number")]
		public float x = default(float);

		[Type(2, "number")]
		public float z = default(float);

		[Type(3, "uint16")]
		public ushort score = default(ushort);

		[Type(4, "uint8")]
		public byte details = default(byte);

		[Type(5, "uint8")]
		public byte skin = default(byte);

		[Type(6, "string")]
		public string name = default(string);
	}
}

