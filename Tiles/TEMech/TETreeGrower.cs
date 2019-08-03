using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;

namespace MechanismsMod.Tiles.TEMech {
    public class TETreeGrower : API.TileEntities.MechanicalTE, API.IWrenchConfigurable {
        public void AltConfigure(Player player) => Configure(player);
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
            return Main.tile[i, j].type == mod.TileType("TreeGrower");
        }

        float distance = 160;

        int alpha = 255 / 2;
        public override void Update()
        {
            for (float i = 0; i < MathHelper.TwoPi; i += 0.03f)
            {
                if (Helper.TryChance(0.1f))
                {
                    var d = Dust.NewDustPerfect(Position.ToWorldCoordinates() + i.ToRotationVector2() * distance, DustID.WitherLightning);
                    d.velocity = Vector2.Zero;
                    d.noGravity = true;
                    d.scale = 0.3f;
                    d.alpha = alpha;
                }
            }
        }

        public void WireHit()
        {
            for (int x = Position.X - 25; x < Position.X + 25; x++)
            {
                for (int y = Position.Y - 25; y < Position.Y + 25; y++)
                {
                    if (Vector2.Distance(new Vector2(x, y) * 16, Position.ToWorldCoordinates()) < distance)
                    {
                        if (WorldGen.InWorld(x, y))
                        {
                            WorldGen.GrowTree(x, y);
                        }
                    }
                }
            }
        }
    }
}