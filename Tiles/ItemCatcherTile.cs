using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MechanismsMod.Tiles {
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
            foreach (Item item in items)
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