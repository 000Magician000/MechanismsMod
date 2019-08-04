using MechanismsMod.Tiles.Generators.TEGenerators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MechanismsMod.Tiles.Generators
{
    class LavaGenerator : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);

            TileObjectData.newTile.StyleMultiplier = 5;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(mod.GetTileEntity<TELavaGenerator>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
            TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(mod.GetTileEntity<TELavaGenerator>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addAlternate(1);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
            TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(mod.GetTileEntity<TELavaGenerator>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addAlternate(2);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
            TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(mod.GetTileEntity<TELavaGenerator>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addAlternate(3);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorWall = true;
            TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(mod.GetTileEntity<TELavaGenerator>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addAlternate(4);

            TileObjectData.addTile(Type);

            Main.tileFrameImportant[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            dustType = -1;
            drop = mod.ItemType("LavaGenerator");

            AddMapEntry(Color.Gray);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
            {
                mod.GetTileEntity("TELavaGenerator").Kill(i, j);
            }
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out var tileEntity) && tileEntity is TELavaGenerator te)
            {
                te.Draw(spriteBatch);
            }
        }
    }
}
