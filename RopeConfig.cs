using System.ComponentModel;
using Terraria.ModLoader.Config;


namespace QuickRope {
	internal class RopeConfig : ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Label( "Amount of ropes to place when a rope is right clicked" )]
		[DefaultValue( 3 )]
		[Range( 1, 20 )]
		[Slider]
		[DrawTicks]
		public int RopesToPlace = 3;

		[Label( "Ticks between placement" )]
		[Tooltip( "Amount of ticks before a rope can be placed with right click again\n" +
				 "One second is the same as 60 ticks" )]
		[Range( 0, 100 )]
		[Increment( 5 )]
		[DefaultValue( 5 )]
		[Slider]
		[DrawTicks]
		public int TimeBetweenRopes = 5;
	}
}
