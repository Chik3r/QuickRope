using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace QuickRope
{
	internal class RopeConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Label("Amount of ropes to place when a rope is right clicked")]
		[DefaultValue(5)]
		[Range(1, 10)]
		[Slider]
		[DrawTicks]
		public int RopesToPlace = 5;

		[Label("Ticks between placement")]
		[Tooltip("Amount of ticks before a rope can be placed with right click again\n" +
		         "One second is the same as 60 ticks")]
		[Range(0, 300)]
		[Increment(30)]
		[DefaultValue(30)]
		[Slider]
		[DrawTicks]
		public int TimeBetweenRopes = 30;

		public override void OnChanged()
		{
			QuickRope.RopesToPlace = RopesToPlace;
			QuickRope.TimeBetweenRopes = TimeBetweenRopes;
		}
	}
}
