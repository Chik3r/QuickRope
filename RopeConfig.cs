using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace QuickRope {
	internal class RopeConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[DefaultValue( 3 )]
		[Range( 1, 20 )]
		[Slider]
		[DrawTicks]
		public int RopesToPlace = 3;

		[Range( 0, 100 )]
		[Increment( 5 )]
		[DefaultValue( 5 )]
		[Slider]
		[DrawTicks]
		public int TimeBetweenRopes = 5;
	}
}
