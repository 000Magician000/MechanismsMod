using MechanismsMod.Tiles.TEMech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MechanismsMod.Tiles {
    public class ItemCatcher : ModTile
    {
        public override void SetDefaults() {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.HookPostPlaceMyPlayer = 
                new PlacementHook(mod.GetTileEntity<TEItemCatcher>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);

            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            dustType = -1;
            drop = mod.ItemType("ItemCatcher");

            AddMapEntry(Color.Gray);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
            if (!fail)
            {
                mod.GetTileEntity("TEItemCatcher").Kill(i, j);
            }
        }

        public override void HitWire(int i, int j) {
            if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out var tileEntity) && tileEntity is TEItemCatcher te) {
                te.WireHit();
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
            if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out var tileEntity) && tileEntity is TEItemCatcher te) {
                te.Draw(spriteBatch);
            }
        }
    }
}