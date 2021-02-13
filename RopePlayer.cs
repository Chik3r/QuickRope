using Terraria.ModLoader;

namespace QuickRope
{
	class RopePlayer : ModPlayer
	{
		public int RopeTimer;

		public override void PostUpdate()
		{
			if (RopeTimer > 0)
				RopeTimer--;
		}
	}
}
