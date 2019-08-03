using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MechanismsMod.Tiles.TEMech
{
    public class TEObsidianGenerator : API.TileEntities.MechanicalTE, API.IWrenchConfigurable
    {
        public void AltConfigure(Player player) => Configure(player);
        public void Configure(Player player)
        {
            //side *= -1;
            //Main.NewText("Side changed to " + (side == 1 ? "down" : "up"));
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {
            if (Main.netMode == 1)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
                NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);

                return -1;
            }
            return Place(i, j);
        }

        public override bool ValidTile(int i, int j)
        {
            return Main.tile[i, j].type == mod.TileType("ObsidianGenerator");
        }

        int side = 1;

        int water = 0;

        int lava = 0;

        int obsidian = 0;

        public override void Update()
        {
            for (var i = 0; i < Main.item.Length; i++)
            {
                var item = Main.item[i];
                if (!item.IsAir && Vector2.Distance(item.Center, Position.ToWorldCoordinates()) < 30)
                {
                    if (item.type == ItemID.WaterBucket)
                    {
                        water += item.stack;
                        Set(item);
                    }
                    else if (item.type == ItemID.LavaBucket)
                    {
                        lava += item.stack;
                        Set(item);
                    }

                    
                }
            }
        }

        public void Set(Item item)
        {
            var it = Item.NewItem(item.Center, ItemID.EmptyBucket, item.stack);
            Main.item[it].velocity = -item.velocity;
            item.TurnToAir();
            item.active = false;

            int lower = Math.Min(water, lava);
            water -= lower;
            lava -= lower;
            obsidian += lower;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (obsidian > 0)
            {
                var position = (new Vector2(Position.X, Position.Y) * 16 - Main.screenPosition) + new Vector2(184f, 164f);

                Utils.DrawBorderString(spriteBatch, obsidian.ToString(), position, Color.White);
            }
        }

        public void WireHit()
        {
            if (obsidian > 0)
            {
                Item.NewItem(Position.ToWorldCoordinates() - new Vector2(0, 16), ItemID.Obsidian, obsidian);
                obsidian = 0;
            }
            /*
            if (water>0)
            {
                if (WorldGen.InWorld(Position.X,Position.Y+side))
                {
                    var tile = Main.tile[Position.X, Position.Y + side];
                    if (tile.liquid==0)
                    {
                        tile.liquid = 255;
                        WorldGen.WaterCheck();
                        water--;
                    }
                }
            }*/
        }
    }
}