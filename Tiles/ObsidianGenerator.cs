using MechanismsMod.Tiles.TEMech;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MechanismsMod.Tiles
{
    public class ObsidianGenerator : ModTile
    {
        public override void SetDefaults()
        {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);

            TileObjectData.newTile.StyleMultiplier = 5;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(mod.GetTileEntity<TEObsidianGenerator>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addAlternate(0);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
            TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(mod.GetTileEntity<TEObsidianGenerator>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addAlternate(1);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
            TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(mod.GetTileEntity<TEObsidianGenerator>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addAlternate(2);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 0);
            TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(mod.GetTileEntity<TEObsidianGenerator>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addAlternate(3);
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.AnchorWall = true;
            TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(mod.GetTileEntity<TEObsidianGenerator>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addAlternate(4);

            TileObjectData.addTile(Type);

            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            dustType = -1;
            drop = mod.ItemType("ObsidianGenerator");

            AddMapEntry(Color.Gray);
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out var tileEntity) && tileEntity is TEObsidianGenerator te)
            {
                te.Draw(spriteBatch);
            }
        }

        public override void HitWire(int i, int j)
        {
            if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out var tileEntity) && tileEntity is TEObsidianGenerator te)
            {
                te.WireHit();
            }
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
            mod.GetTileEntity("TEObsidianGenerator").Kill(i, j);
        }
        //    public override void SetDefaults()
        //    {
        //        Main.tileSolid[Type] = true;
        //        Main.tileMergeDirt[Type] = true;
        //        Main.tileBlockLight[Type] = true;
        //        Main.tileLighted[Type] = true;
        //        dustType = -1;
        //        drop = mod.ItemType("TreeChopper");
        //        AddMapEntry(Color.Gray);
        //    }

        //    Vector2 rect = new Vector2(8, 4);
        //    Vector2 origin = new Vector2(12, 12);
        //    Vector2 heights = new Vector2(0, -16);

        //    public Texture2D item
        //    {
        //        get { return Main.itemTexture[ItemID.CopperAxe]; }
        //    }

        //    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        //    {
        //        DrawItem(i, j, spriteBatch);
        //        if (Main.tile[i, j].inActive())
        //        {
        //            Active(i, j);
        //        }

        //        for (int side = -1; side <= 1; side += 2)
        //        {
        //            for (int y = (j - (int)rect.Y) * 16; y <= (j + (int)rect.Y + 1) * 16; y++)
        //            {
        //                var d = Dust.NewDustPerfect(new Vector2((i + (side == 1 ? 1 : 0) + rect.X * side) * 16, y), DustID.WitherLightning);
        //                d.velocity = Vector2.Zero;
        //                d.noGravity = true;
        //                d.scale = 0.3f;
        //            }
        //        }

        //        return base.PreDraw(i, j, spriteBatch);
        //    }

        //    public void DrawItem(int i, int j, SpriteBatch spriteBatch)
        //    {
        //        Vector2 pos = (new Vector2(i, j) + origin) * 16 + heights - Main.screenPosition;
        //        spriteBatch.Draw(item, new Rectangle((int)pos.X, (int)pos.Y, 16, 16), Color.White * Main.essScale);
        //    }

        //    int[] tilesToDestroy = { TileID.Trees };
        //    public override void HitWire(int i, int j)
        //    {
        //        Main.tile[i, j].inActive(!Main.tile[i, j].inActive());
        //    }

        //    public void Active(int i, int j)
        //    {
        //        for (int x = i - (int)rect.X; x <= i + (int)rect.X; x++)
        //        {
        //            for (int y = j - (int)rect.Y; y <= j + (int)rect.Y; y++)
        //            {

        //                if (WorldGen.InWorld(x, y))
        //                {
        //                    var t = Main.tile[x, y].type;
        //                    var up = Main.tile[x, y - 1].type;
        //                    var up2 = Main.tile[x, y - 2].type;
        //                    var under = Main.tile[x, y + 1].type;

        //                    if (tilesToDestroy.Contains(t) && under == TileID.Grass)
        //                    {
        //                        TileHelper.DamageTile(x, y, 5);
        //                        if (up == TileID.Trees && up2 == TileID.Trees)
        //                        {
        //                            WorldGen.PlaceTile(x, y, 20);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}