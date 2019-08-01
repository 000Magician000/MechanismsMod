using MechanismsMod.API;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MechanismsMod.Tiles {
    public class TEItemCatcher : ModTileEntity, IWrenchConfigurable {
        public void Configure(Player player) {
            if (storedItem != null) {
                player.QuickSpawnItem(storedItem.type, storedItem.stack);
                storedItem = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            if (storedItem == null) {
                return;
            }

            var position = (new Vector2(Position.X, Position.Y) * 16 - Main.screenPosition) + new Vector2(180f, 158f + (10f * Main.essScale));
            spriteBatch.Draw(Main.itemTexture[storedItem.type], position, Color.White);
            Utils.DrawBorderString(spriteBatch, $"x{storedItem.stack}", position + new Vector2(25f, 0f), Color.White);
        }

        public void WireHit() {
            
        }

        public ItemContainer storedItem;
        public override void Update() {
            for (var i = 0; i < Main.item.Length; i++) {
                var item = Main.item[i];
                if (item != null && !item.IsAir && item.active && Vector2.Distance(new Vector2(Position.X, Position.Y) * 16, item.Center) < 50f) {
                    if (storedItem != null) {
                        if (storedItem.type == item.type) {
                            storedItem.stack += item.stack;
                            item.TurnToAir();
                            item.active = false;
                        }

                        continue;
                    }

                    else {
                        storedItem = new ItemContainer(item.type, item.stack);
                        item.TurnToAir();
                        item.active = false;
                    }
                }
            }
        }

        public override void Load(TagCompound tag) => storedItem = new ItemContainer(tag.GetInt("type"), tag.GetInt("stack"));
        public override TagCompound Save() => storedItem != null ? storedItem.GetTagCompound() : null;

        public override void OnKill() {
            if (storedItem != null) {
                Item.NewItem(Position.X * 16, Position.Y * 16, 0, 0, storedItem.type, storedItem.stack);
            }
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction) {
			if (Main.netMode == 1) {
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
				NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);

				return -1;
			}

			return Place(i, j);
		}

        public override bool ValidTile(int i, int j) {
            return Main.tile[i, j].type == mod.TileType("ItemCatcher");
        }
    }
}