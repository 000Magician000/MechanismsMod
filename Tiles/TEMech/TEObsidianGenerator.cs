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

        int queue = 0;
        int obsidian = 0;

        int time = 0;
        int maxtime = 30;

        public override void Update()
        {
            if (WorldGen.InWorld(Position.X,Position.Y-1))
            {
                Tile t = Main.tile[Position.X, Position.Y - 1];
                if (t.liquid>0)
                {
                    
                    if (t.liquidType() == 0)
                    {
                        water += t.liquid/255;
                        t.liquid = 0;
                    }
                    else if (t.liquidType() == 1)
                    {
                        lava += t.liquid/255;
                        t.liquid = 0;
                    }
                    
                    Set();
                }
            }

            if (queue>0)
            {
                if (++time>=maxtime)
                {
                    queue--;
                    obsidian++;
                    time = 0;
                }
            }
        }

        public void Set()
        {
            int lower = Math.Min(water, lava);
            water -= lower;
            lava -= lower;
            queue += lower;
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
                var i = Item.NewItem(Position.ToWorldCoordinates() - new Vector2(0, 16), ItemID.Obsidian, obsidian);
                Main.item[i].velocity = Vector2.Zero;
                obsidian = 0;
            }
        }

        public override void Load(TagCompound tag)
        {
            queue = tag.GetInt("queue");
            obsidian = tag.GetInt("obsidian");
            water = tag.GetInt("water");
            lava = tag.GetInt("lava");
        }

        public override TagCompound Save()
        {
            var tag = new TagCompound();
            tag.Add("queue", queue);
            tag.Add("obsidian", obsidian);
            tag.Add("water", water);
            tag.Add("lava", lava);
            return tag;
        }
    }
}