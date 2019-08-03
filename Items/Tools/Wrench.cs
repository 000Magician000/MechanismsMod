﻿using MechanismsMod.API;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace MechanismsMod.Items.Tools {
    public class Wrench : ModItem {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Hit machine to configure/scan");
        }

        public override void SetDefaults() {
            item.UseSound = SoundID.Item1;
            item.useAnimation = 20;
            item.useTime = 20;
            item.useStyle = 1;
            item.width = 30;
            item.height = 28;
            item.rare = ItemRarityID.Green;
            item.autoReuse = true;
        }

        public override bool CanRightClick() => true;

        public override bool AltFunctionUse(Player player) {
            if (player.altFunctionUse == 2) {
                if (TileEntity.ByPosition.TryGetValue(new Point16((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16),
                out var tileEntity) && tileEntity is IWrenchConfigurable mechanic) {

                    mechanic.AltConfigure(player);
                    Main.PlaySound(0, player.Center, 0);
                }
            }

            return true;
        }

        public override bool UseItem(Player player) {
            if (TileEntity.ByPosition.TryGetValue(new Point16((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16),
                out var tileEntity) && tileEntity is IWrenchConfigurable mechanic) {

                mechanic.Configure(player);
                Main.PlaySound(0, player.Center, 0);
            }

            return true;
        }
    }
}
