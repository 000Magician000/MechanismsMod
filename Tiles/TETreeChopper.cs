using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MechanismsMod.Tiles {
    public class TETreeChopper : ModTileEntity, API.IWrenchConfigurable {
        public void Configure(Player player) {
            Main.NewText("I'm here!");
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction) {
			if (Main.netMode == 1) {
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
				NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);

				return -1;
			}
			return Place(i, j);
		}

        public override bool ValidTile(int i, int j) {
            return Main.tile[i, j].type == mod.TileType("TreeChopper");
        }

    }
}