﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MechanismsMod.Tiles {
    public class TreeGrowerTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            dustType = -1;
            drop = mod.ItemType("TreeGrower");
            AddMapEntry(Color.Gray);
        }

        Vector2 rect = new Vector2(8, 4);
        Vector2 origin = new Vector2(12, 12);
        Vector2 heights = new Vector2(0, -16);

        public Texture2D item
        {
            get { return Main.itemTexture[ItemID.Acorn]; }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            DrawItem(i, j, spriteBatch);

            for (int side = -1; side <= 1; side += 2)
            {
                for (int y = (j - (int)rect.Y) * 16; y <= (j + (int)rect.Y + 1) * 16; y++)
                {
                    var d = Dust.NewDustPerfect(new Vector2((i + (side == 1 ? 1 : 0) + rect.X * side) * 16, y), DustID.WitherLightning);
                    d.velocity = Vector2.Zero;
                    d.noGravity = true;
                    d.scale = 0.3f;
                }
            }

            return base.PreDraw(i, j, spriteBatch);
        }

        public void DrawItem(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 pos = (new Vector2(i, j) + origin) * 16 + heights - Main.screenPosition;
            spriteBatch.Draw(item, new Rectangle((int)pos.X, (int)pos.Y, 16, 16), Color.White * Main.essScale);
        }

        public override void HitWire(int i, int j)
        {
            for (int x = i - (int)rect.X; x <= i + (int)rect.X; x++)
            {
                for (int y = j - (int)rect.Y; y <= j + (int)rect.Y; y++)
                {

                    if (WorldGen.InWorld(x, y))
                    {
                        var t = Main.tile[x, y].type;
                        //var up = Main.tile[x, y - 1].type;
                        var under = Main.tile[x, y + 1].type;

                        if (t == 20 && under == TileID.Grass)
                        {
                            WorldGen.GrowTree(x, y);
                        }
                    }
                }
            }
        }
    }
}