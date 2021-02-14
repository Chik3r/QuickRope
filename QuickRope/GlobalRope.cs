using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace QuickRope {
	class QuickRopeItem : GlobalItem {
		public override void ModifyTooltips( Item item, List<TooltipLine> tooltips ) {
			int tileType = item.createTile;
			if( tileType < 0 || tileType >= Main.tileRope.Length || !Main.tileRope[tileType] ) {
				return;
			}

			tooltips.Add( new TooltipLine(this.mod, "QuickRopeTip", "Right-click to place ropes quickly") );
		}


		////////////////

		//public override void MouseOverFar( int i, int j, int type ) {
		//public override void RightClick( int i, int j, int type ) {
		public override bool AltFunctionUse( Item item, Player player ) {
			if( player.itemTime > 0 ) {
				return false;
			}

			// If the tile isn't a rope, return
			int tileType = item.createTile;
			if( tileType < 0 || tileType >= Main.tileRope.Length || !Main.tileRope[tileType] ) {
				return true;
			}

			int tileX = (int)Main.MouseWorld.X / 16;
			int tileY = (int)Main.MouseWorld.Y / 16;

			// Get the position of the player as tile coordinates
			var playerPos = player.Center.ToTileCoordinates().ToVector2();
			// Get the distance and max range of the player
			float xDistance = Math.Abs( playerPos.X - tileX );
			var yDistance = Math.Abs( playerPos.Y - tileY );
			float maxRange = item.tileBoost + 4;

			// Check if the tile is inside the cursor range
			bool insideRange = ( xDistance <= maxRange && yDistance <= maxRange );
			if( !insideRange ) {
				return true;
			}

			var config = ModContent.GetInstance<RopeConfig>();

			this.PlaceOrExtendARopeAt( player, tileX, tileY, item, config.RopesToPlace );

			player.itemTime = config.TimeBetweenRopes;
			player.itemAnimation = config.TimeBetweenRopes;
			player.itemAnimationMax = config.TimeBetweenRopes+1;

			return true;
		}


		////

		private void PlaceOrExtendARopeAt( Player player, int i, int j, Item ropeItem, int ropeCount ) {
			int tileType = ropeItem.createTile;
			int tileTargetX = i;
			int tileTargetY = j;

			for( int k = 0; k < ropeCount; k++ ) {
				// No more ropes left to place
				if( ropeItem.stack <= 0 ) {
					ropeItem.TurnToAir();
					return;
				}

				// The code below was copied (and altered) from Player.PlaceThing()

				if( !this.IsTileValidForRopePlacement(tileTargetX, ref tileTargetY) ) {
					return;
				}

				// Place the tile
				WorldGen.PlaceTile( tileTargetX, tileTargetY, tileType );
				ropeItem.stack--;

				if( Main.netMode != NetmodeID.SinglePlayer ) {
					NetMessage.SendData( MessageID.TileChange, -1, -1, null, 1, tileTargetX, tileTargetY, tileType );
				}
			}
		}


		private bool IsTileValidForRopePlacement( int tileX, ref int tileY ) {
			bool IsTileValidExtendableRope( int x, int y ) {
				return Main.tile[x, y].active()
					&& Main.tileRope[Main.tile[x, y].type]
					&& y < Main.maxTilesX - 5
					&& Main.tile[x, y + 2] != null
					&& !Main.tile[x, y + 1].lava();
			}

			//

			if( tileX <= 0 || tileX >= Main.maxTilesX-1 || tileY <= 0 || tileY >= Main.maxTilesY-1 ) {
				return false;
			}

			Tile tile = Main.tile[tileX, tileY];
			if( !tile.active() ) {
				return this.IsValidRopeAnchor( tileX, tileY );
			}

			// If the tile at the target pos isn't active, or it isn't a rope, end the function
			if( !Main.tileRope[tile.type] ) {
				return false;
			}

			int scanY = tileY;

			// Iterate across any existing ropes until the next available space
			while( IsTileValidExtendableRope( tileX, scanY ) ) {
				scanY++;

				// Unknown tile; world hasnt finished loading/syncing here yet
				if( Main.tile[tileX, scanY] == null ) {
					return false;
				}
			}

			// Is next tile a non-rope solid?
			if( Main.tile[tileX, scanY].active() ) {
				return false;
			}

			tileY = scanY;
			return true;
		}


		private bool IsValidRopeAnchor( int tileX, int tileY ) {
			Tile tileL = Main.tile[tileX - 1, tileY];
			Tile tileR = Main.tile[tileX + 1, tileY];
			Tile tileD = Main.tile[tileX, tileY - 1];
			Tile tileU = Main.tile[tileX, tileY + 1];

			if( tileD.wall > 0 || tileU.wall > 0 || tileL.wall > 0 || tileR.wall > 0 ) {
				return true;
			}

			int typeR = tileR.type;
			if( tileR.active() && (Main.tileSolid[typeR] || Main.tileRope[typeR] || tileR.type == 314) ) {
				return true;
			}
			int typeL = tileL.type;
			if( (tileL.active() && (Main.tileSolid[typeL] || Main.tileRope[typeL] || tileL.type == 314)) ) {
				return true;
			}
			int typeU = tileU.type;
			if( (tileU.active() && (Main.tileSolid[typeU] || tileU.type == 124 || Main.tileRope[typeU] || tileU.type == 314)) ) {
				return true;
			}
			int typeD = tileD.type;
			if( (tileD.active() && (Main.tileSolid[typeD] || tileD.type == 124 || Main.tileRope[typeD] || tileD.type == 314)) ) {
				return true;
			}

			return false;
		}
	}
}
