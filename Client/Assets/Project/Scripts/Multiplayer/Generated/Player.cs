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
		[Type(0, "number")]
		public float x = default(float);

		[Type(1, "number")]
		public float z = default(float);

		[Type(2, "uint16")]
		public ushort score = default(ushort);

		[Type(3, "uint8")]
		public byte details = default(byte);

		[Type(4, "uint8")]
		public byte skin = default(byte);
	}
}

