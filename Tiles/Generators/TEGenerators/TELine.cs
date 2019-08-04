using MechanismsMod.API;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MechanismsMod.Tiles.Generators.TEGenerators
{
    public class TELine : ModTileEntity, IWrenchConfigurable
    {
        public void AltConfigure(Player player)
        {
            if (energy<conduction)
            energy += conduction/2;
        }

        int maxStyles = 11;

        public void Configure(Player player)
        {
            Tile tile = Main.tile[Position.X, Position.Y];
            int style = tile.frameX / 18;
            style++;
            if (style > maxStyles) style = 0;
            tile.frameX = (short)(style * 18);

            if (TileEntity.ByPosition.TryGetValue(new Point16(Position.X, Position.Y), out var tileEntity) && tileEntity is TELine te)
            {
                switch (style)
                {
                    case 0:
                        te.input = te.sides[(int)Sides.Left];
                        te.output = te.sides[(int)Sides.Right];
                        break;
                    case 1:
                        te.input = te.sides[(int)Sides.Down];
                        te.output = te.sides[(int)Sides.Up];
                        break;
                    case 2:
                        te.input = te.sides[(int)Sides.Right];
                        te.output = te.sides[(int)Sides.Left];
                        break;
                    case 3:
                        te.input = te.sides[(int)Sides.Up];
                        te.output = te.sides[(int)Sides.Down];
                        break;
                    case 4:
                        te.input = te.sides[(int)Sides.Left];
                        te.output = te.sides[(int)Sides.Down];
                        break;
                    case 5:
                        te.input = te.sides[(int)Sides.Left];
                        te.output = te.sides[(int)Sides.Up];
                        break;
                    case 6:
                        te.input = te.sides[(int)Sides.Down];
                        te.output = te.sides[(int)Sides.Left];
                        break;
                    case 7:
                        te.input = te.sides[(int)Sides.Down];
                        te.output = te.sides[(int)Sides.Right];
                        break;
                    case 8:
                        te.input = te.sides[(int)Sides.Right];
                        te.output = te.sides[(int)Sides.Up];
                        break;
                    case 9:
                        te.input = te.sides[(int)Sides.Right];
                        te.output = te.sides[(int)Sides.Down];
                        break;
                    case 10:
                        te.input = te.sides[(int)Sides.Up];
                        te.output = te.sides[(int)Sides.Right];
                        break;
                    case 11:
                        te.input = te.sides[(int)Sides.Up];
                        te.output = te.sides[(int)Sides.Left];
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var position = (new Vector2(Position.X, Position.Y) * 16 - Main.screenPosition) + new Vector2(192f, 194f);

            Utils.DrawBorderString(spriteBatch, $"{energy}/{conduction}", position, Color.White*0.3f,0.55f);
        }

        int cooldown = 10;
        int cooldownmax = 10;

        public override void Update()
        {
            Tile tile = Main.tile[Position.X, Position.Y];
            if (tile.frameY != 0)
                tile.frameY = 0;

            if (energy < conduction)
            {
                if (WorldGen.InWorld(Position.X + input.Item1, Position.Y + input.Item2))
                {
                    if (ByPosition.TryGetValue(new Point16(Position.X + input.Item1, Position.Y + input.Item2), out var line) && line is TELine l)
                    {
                        if (l.output == (input.Item1 * -1, input.Item2 * -1) && l.energy > 0)
                        {
                            if (cooldown <= 0)
                            {
                                float lower = Math.Min(l.energy, conduction);
                                energy += lower;
                                l.energy -= lower;
                                cooldown = cooldownmax;
                            }
                            else cooldown--;
                        }
                    }
                }
            }

        }

        public override void OnKill()
        {

        }

        public enum Sides
        {
            Down = 0,
            Up = 1,
            Right = 2,
            Left = 3
        }
        public (int, int)[] sides = { (0, 1), (0, -1), (1, 0), (-1, 0) };
        public (int, int) input = (-1, 0);
        public (int, int) output = (1, 0);

        public float energy = 0;
        const float conduction = 5;

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
            return Main.tile[i, j].type == mod.TileType("Line");
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound();
            tag.Add("energy", energy);
            tag.Add("inX", input.Item1);
            tag.Add("inY", input.Item2);
            tag.Add("outX", output.Item1);
            tag.Add("outY", output.Item2);
            tag.Add("cd", cooldown);
            return tag;
        }

        public override void Load(TagCompound tag)
        {
            energy = tag.GetFloat("energy");
            input = (tag.GetInt("inX"),tag.GetInt("inY"));
            output = (tag.GetInt("outX"), tag.GetInt("outY"));
            cooldown = tag.GetInt("cd");
        }
    }
}