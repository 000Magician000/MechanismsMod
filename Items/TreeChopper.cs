using System;
using System.IO;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MechanismsMod.Items {
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
            item.createTile = mod.TileType("TreeChopper");
            item.mech = true;
        }
    }
}