using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QuickRope;

public class QuickRopeModSystem : ModSystem {
    public override void PostSetupContent() {
        for (int i = 0; i < ItemLoader.ItemCount; i++) {
            int tileType = ContentSamples.ItemsByType[i].createTile;
            if ( tileType < 0 || tileType >= Main.tileRope.Length || !Main.tileRope[tileType] )
                continue;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[i] = true;
        }
    }
}