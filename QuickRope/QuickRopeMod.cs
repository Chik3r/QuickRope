using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;


namespace QuickRope {
	public delegate bool PlaceRopeHook( Player player, Item ropeItem, int tileX, int tileY );




	public class QuickRopeMod : Mod {
		public static bool AddRopePlacementHook( PlaceRopeHook hook ) {
			return ModContent.GetInstance<QuickRopeMod>().PlaceRopeHooks.Add( hook );
		}

		////

		internal static bool RunRopePlacementHooks( Player player, Item item, int tileX, int tileY ) {
			foreach( PlaceRopeHook hook in ModContent.GetInstance<QuickRopeMod>().PlaceRopeHooks ) {
				if( !hook.Invoke(player, item, tileX, tileY) ) {
					return false;
				}
			}
			return true;
		}



		////////////////

		internal ISet<PlaceRopeHook> PlaceRopeHooks = new HashSet<PlaceRopeHook>();
	}
}