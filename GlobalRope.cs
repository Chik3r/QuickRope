using System;
using Terraria;
using Terraria.ModLoader;

namespace QuickRope
{
	internal class GlobalRope : GlobalTile
	{
		public override void MouseOverFar(int i, int j, int type)
		{
			// If the tile isn't a rope, return
			if (!Main.tileRope[type])
				return;

			Item heldItem = Main.LocalPlayer.HeldItem;
			// If the held item doesn't create the same type as the one under the mouse
			// or the tile isn't a rope, return
			if (heldItem.createTile != type || !Main.tileRope[heldItem.createTile])
				return;

			// Get the position of the player as tile coordinates
			var playerPos = Main.LocalPlayer.Center.ToTileCoordinates().ToVector2();
			// Get the distance and max range of the player
			float xDistance = Math.Abs(playerPos.X - i);
			var yDistance = Math.Abs(playerPos.Y - j);
			float maxRange = Main.LocalPlayer.HeldItem.tileBoost + 4;
			// Check if the tile is inside the cursor range
			bool insideRange = (xDistance <= maxRange && yDistance <= maxRange);

			// Check the rope placement timer
			RopePlayer player = Main.LocalPlayer.GetModPlayer<RopePlayer>();
			bool ropeTimerValid = player.RopeTimer == 0;

			if (Main.mouseRight && insideRange && ropeTimerValid)
			{
				GenerateRopes(i, j, type, heldItem);
				player.RopeTimer = QuickRope.TimeBetweenRopes;
			}
		}

		private void GenerateRopes(int i, int j, int type, Item heldItem)
		{
			int tileTargetX = i;
			int tileTargetY = j;
			bool shouldPlace = true;

			for (int k = 0; k < QuickRope.RopesToPlace; k++)
			{
				if (heldItem.stack <= 0)
				{
					heldItem.TurnToAir();
					return;
				}

				// The code below was copied from Player.PlaceThing()

				// If the tile at the target pos isn't active, or it isn't a rope, end the function
				if (!Main.tile[tileTargetX, tileTargetY].active() || !Main.tileRope[Main.tile[tileTargetX, tileTargetY].type])
					return;

				int num23 = tileTargetY;
				int num24 = tileTargetX;
				// Iterate until an empty tile is found?
				while (Main.tile[num24, num23].active() && Main.tileRope[Main.tile[num24, num23].type] && num23 < Main.maxTilesX - 5 && Main.tile[num24, num23 + 2] != null && !Main.tile[num24, num23 + 1].lava())
				{
					num23++;
					if (Main.tile[num24, num23] == null)
					{
						shouldPlace = false;
						num23 = tileTargetY;
					}
				}

				if (!Main.tile[num24, num23].active())
					tileTargetY = num23;
				else
					return;

				// If the target isn't valid (shouldPlace is false), return
				if (!shouldPlace)
					return;

				// Place the tile
				WorldGen.PlaceTile(tileTargetX, tileTargetY, type);
				heldItem.stack--;
			}
		}
	}
}
