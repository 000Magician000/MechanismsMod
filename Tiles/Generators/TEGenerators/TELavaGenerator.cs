using MechanismsMod.API;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MechanismsMod.Tiles.Generators.TEGenerators {
    public class TELavaGenerator : ModTileEntity, IWrenchConfigurable
    {
        public void AltConfigure(Player player)
        {

        }

        public void Configure(Player player)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public (int, int)[] sides = { (0, 1), (0, -1), (1, 0), (-1, 0) };

        public override void Update()
        {
            foreach (var side in sides)
            {

            }
        }

        public override void OnKill()
        {

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
            return Main.tile[i, j].type == mod.TileType("LavaGenerator");
        }

    }
}