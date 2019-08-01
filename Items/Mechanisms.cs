using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MechanismsMod.Items
{
    public class TreeChopper : ModItem
    {
        public override string Texture => "MechanismsMod/Items/Mech";
        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = mod.TileType("TreeChopperTile");
        }
    }

    public class TreeChopperTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            dustType = -1;
            drop = mod.ItemType("TreeChopper");
            AddMapEntry(Color.Gray);
        }

        Vector2 rect = new Vector2(8, 4);
        Vector2 origin = new Vector2(12, 12);
        Vector2 heights = new Vector2(0, -16);

        public Texture2D item
        {
            get { return Main.itemTexture[ItemID.CopperAxe]; }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            DrawItem(i, j, spriteBatch);
            if (Main.tile[i, j].inActive())
            {
                Active(i, j);
            }

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

        int[] tilesToDestroy = { TileID.Trees };
        public override void HitWire(int i, int j)
        {
            Main.tile[i, j].inActive(!Main.tile[i, j].inActive());
        }

        public void Active(int i, int j)
        {
            for (int x = i - (int)rect.X; x <= i + (int)rect.X; x++)
            {
                for (int y = j - (int)rect.Y; y <= j + (int)rect.Y; y++)
                {

                    if (WorldGen.InWorld(x, y))
                    {
                        var t = Main.tile[x, y].type;
                        var up = Main.tile[x, y - 1].type;
                        var up2 = Main.tile[x, y - 2].type;
                        var under = Main.tile[x, y + 1].type;

                        if (tilesToDestroy.Contains(t) && under == TileID.Grass)
                        {
                            TileHelper.DamageTile(x, y, 5);
                            if (up == TileID.Trees && up2 == TileID.Trees)
                            {
                                WorldGen.PlaceTile(x, y, 20);
                            }
                        }
                    }
                }
            }
        }
    }

    public class TreeGrower : ModItem
    {
        public override string Texture => "MechanismsMod/Items/Mech";
        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = mod.TileType("TreeGrowerTile");
        }
    }

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

    public class ItemCatcher : ModItem
    {
        public override string Texture => "MechanismsMod/Items/Mech";
        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = mod.TileType("ItemCatcherTile");
        }
    }

    public class ItemCatcherTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            dustType = -1;
            drop = mod.ItemType("ItemCatcher");
            AddMapEntry(Color.Gray);
        }

        int radius = 90;
        Vector2 origin = new Vector2(12, 12);
        Vector2 center = new Vector2(8, 8);
        Vector2 heights = new Vector2(0, -16);

        public Texture2D item
        {
            get { return Main.itemTexture[ItemID.Chest]; }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            DrawItem(i, j, spriteBatch);

            if (Main.tile[i, j].inActive())
            {
                Active(i, j);
            }

            for (float z = 0; z < MathHelper.TwoPi; z += 0.02f)
                if (Helper.TryChance(0.4f))
                {
                    Vector2 a = new Vector2(i, j) * 16 + center;
                    var d = Dust.NewDustPerfect(a + z.ToRotationVector2()*radius, DustID.WitherLightning,Vector2.Zero);
                    d.velocity = Vector2.Zero;
                    d.scale = 0.4f;
                    d.noGravity = true;
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
            Main.tile[i, j].inActive(!Main.tile[i, j].inActive());
        }

        public override void RightClick(int i, int j)
        {
            foreach(Item item in items)
            {
                Item.NewItem(new Vector2(i, j) * 16 + center, item.type, item.stack, prefixGiven: item.prefix);
            }
            items.Clear();
        }

        List<Item> items = new List<Item>();

        public void Active(int i, int j)
        {
            foreach (Item item in Main.item.Where(z => z.active && Vector2.Distance(new Vector2(i, j) * 16 + center, z.Center) < radius))
            {
                item.velocity = VectorHelper.FromTo(item.Center, new Vector2(i, j) * 16 +center, 4);
                if (Vector2.Distance(new Vector2(i, j) * 16 + center, item.Center) < 30)
                {
                    items.Add(item);
                    item.TurnToAir();
                }
            }
        }
    }
}