using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MechanismsMod.Tiles.TEMech {
    public class TETreeChopper : API.TileEntities.MechanicalTE, API.IWrenchConfigurable {
        public void Configure(Player player) {
            forced = !forced;
            distance = forced ? dist1 : dist2;
            Main.NewText("Mode switched to " + (forced ? "forced":"normal"));
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction) {
			if (Main.netMode == 1) {
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
				NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);

				return -1;
			}
			return Place(i, j);
		}

        bool active = false;

        float dist1 = 80;
        float dist2 = 160;
        bool forced = false;
        float distance = 160;
        int alpha = 255 / 2;
        int[] tilesToDestroy = { TileID.Trees };
        int[] tilesGround = { TileID.Grass, TileID.SnowBlock, TileID.Sand };

        public override bool ValidTile(int i, int j) {
            return Main.tile[i, j].type == mod.TileType("TreeChopper");
        }

        public override void Update()
        {
            if (active)
            {
                for (int x = Position.X - 25; x < Position.X + 25; x++)
                {
                    for (int y = Position.Y - 25; y < Position.Y + 25; y++)
                    {
                        if (Vector2.Distance(new Vector2(x, y) * 16, Position.ToWorldCoordinates()) < distance)
                        {
                            if (WorldGen.InWorld(x, y))
                            {
                                int tile = Main.tile[x, y].type;
                                int under = Main.tile[x, y+1].type;
                                int upper1 = Main.tile[x, y - 1].type;
                                int upper2 = Main.tile[x, y - 2].type;
                                if (tilesToDestroy.Contains(tile) && tilesGround.Contains(under))
                                {
                                    TileHelper.DamageTile(x, y, forced ? 5 : 2);

                                    if (forced)
                                    if (tilesToDestroy.Contains(upper1) && tilesToDestroy.Contains(upper2))
                                    {
                                        WorldGen.PlaceTile(x, y, 20);
                                    }
                                }
                            }
                        }
                    }
                }
            }

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
            active = !active;
        }
    }
}