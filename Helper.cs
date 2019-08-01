using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

public static class Timers
{
    private static Dictionary<long, List<long>> Inner;
    private static Dictionary<int, Action> Actions;
    private static int NextID;
    private static long Time;

    static Timers()
    {
        Inner = new Dictionary<long, List<long>>();
        Actions = new Dictionary<int, Action>();
    }

    public static long New(double ticks, Action action)
    {
        var time = Time + (long)(ticks);
        var id = NextID;
        NextID++;

        Actions.Add(id, action);
        if (Inner.ContainsKey(time))
        {
            Inner[time].Add(id);
        }
        else
        {
            Inner.Add(time, new List<long>(
                new long[] { id }
            ));
        }

        return id;
    }

    public static bool Active(int id)
    {
        return Actions.ContainsKey(id);
    }

    public static bool Interrupt(int id)
    {
        if (!Active(id)) return false;

        Actions.Remove(id);
        return true;
    }

    public static void Update()
    {
        if (Inner.Count == 0) NextID = 0;

        if (Inner.ContainsKey(Time))
        {
            foreach (int id in Inner[Time])
            {
                if (Actions.ContainsKey(id))
                {
                    Actions[id]();
                    Actions.Remove(id);
                }
            }

            Inner.Remove(Time);
        }

        Time++;
    }
}
public static class PlayerHelper
{
    public static Player Closest(Vector2 point, float max = int.MaxValue)
    {
        float range = max;
        var i = Main.player.Where(p => p.active && Vector2.Distance(p.Center,point)<=max);
        var ii = Main.player.Where(p => Vector2.Distance(p.Center, point) <= max);
        return i.Count()>0 ? i.First() : ii.First();
    }
}

public static class ProjectileHelper
{
    public static void VelocityToRotation(this Projectile projectile)
    {
        projectile.rotation = projectile.velocity.ToRotation();
    }

    public static void FrameAnim(this Projectile projectile,int speed)
    {
        if (++projectile.frameCounter >= speed)
        {
            projectile.frameCounter = 0;
            if (++projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }
    }

    public static void FromTo(this Projectile projectile, Vector2 to, float speed, float smooth)
    {
        Vector2 range = VectorHelper.FromTo(projectile.Center, to, speed);

        projectile.velocity += (range - projectile.velocity) / smooth;
    }
}

public static class NPCHelper
{
    public static int Closest(Vector2 point, float max = int.MaxValue, bool dontCheckDontTakeDmg = false, bool dontCheckFriendly = true, int[] disallowedNPCs = null)
    {
        if (disallowedNPCs == null)
        {
            disallowedNPCs = new[] {-1};
        }
        float range = max;
        int n = -1;
        foreach (var npc in Main.npc.Where(npc => npc.active && !disallowedNPCs.Contains(npc.netID) && (dontCheckFriendly ? !npc.friendly : true) && (dontCheckDontTakeDmg ? !npc.dontTakeDamage : true)))
        {
            float dist = Vector2.Distance(npc.Center, point);
            if (dist < range)
            {
                range = dist;
                n = npc.whoAmI;
            }
        }
        return n;
    }
    
    public static void FrameAnim(this NPC npc, int speed)
    {
        int height = Main.npcTexture[npc.type].Height;
        int frames = Main.npcFrameCount[npc.type];
        int frHeight = height/frames;
        
        if (++npc.frameCounter >= speed)
        {
            npc.frameCounter = 0;

            if (npc.frame.Y/frHeight < frames-1)
            {
                npc.frame.Y += frHeight;
            }
            else
            {
                npc.frame.Y = 0;
            }
        }
    }
}

public static class VectorHelper
{
    public static List<Vector2> Circle(int count, float distance = 1, float start = 0f)
    {
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < count; i++)
        {
            Vector2 vec = (start + MathHelper.TwoPi / count * i).ToRotationVector2() * distance;
            list.Add(vec);
        }
        return list;
    }

    public static Vector2 FromTo(Vector2 from, Vector2 to, float speed)
    {
        Vector2 range = to - from;
                        
        float magnitude = (float) Math.Sqrt(range.X * range.X + range.Y * range.Y);

        if (magnitude > 0)
        {
            range *= speed / magnitude;
        }
        else
        {
            range = new Vector2(0f, speed);
        }
        return range;
    }
}

public static class DustHelper
{
    public static Dust CreateDust(Vector2 pos, int type, Color color, Vector2 velocity,
        int alpha = 0, float scale = 1,
        bool noGrav = false, float fadeIn = 0f, ArmorShaderData shader = null)
    {
        Dust dust = Dust.NewDustPerfect(pos, type, null, alpha, color, scale);
        dust.noGravity = noGrav;
        dust.fadeIn = fadeIn;
        dust.velocity = velocity;
        if (shader != null)
            dust.shader = shader;
        return dust;
    }
}

public static class DrawHelper
{
    public static void DrawBar(SpriteBatch sb, float full, float value, Vector2 position, float width, float height, Color back,
        Color front)
    {
        float bar = (float)width / (float)full;
        Vector2 start = position;
        Vector2 end = new Vector2((float)position.X + (float)width, (float)position.Y);
        Vector2 end2 = new Vector2((float)position.X + (float)bar * (float)value, (float)position.Y);
        Utils.DrawLine(sb, start, end, back, back, height);
        Utils.DrawLine(sb, start, end2, front, front, height);
    }
}

public static class TileHelper
{
    public static void DamageTile(int i, int j, int damage, bool simulate = false)
    {
        var id = Main.LocalPlayer.hitTile.HitObject(i, j, 1);
        if (WorldGen.CanKillTile(i, j) && Main.LocalPlayer.hitTile.AddDamage(id, damage, !simulate) >= 100)
        {
            Main.LocalPlayer.hitTile.Clear(id);
            WorldGen.KillTile(i, j);
            if (Main.netMode == 1)
            {
                NetMessage.SendData(17, number2: i, number3: j);
            }
        }
    }
}

public static class Helper
{
    public static bool TryChance(float chance)
    {
        if (Main.rand.NextFloat() < chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static Vector2 Mouse()
    {
        return new Vector2(Main.screenPosition.X+Main.mouseX,Main.screenPosition.Y+Main.mouseY);
    }

    public static void CombatTxt(Vector2 position, string text, Color color)
    {
        CombatText.NewText(new Rectangle((int) position.X, (int) position.Y, 0, 0), color, text);
    }

    public static void CombatTxt(Vector2 position, string text)
    {
        CombatText.NewText(new Rectangle((int) position.X, (int) position.Y, 0, 0), Color.White, text);
    }

    public static void Kill(this Player player)
    {
        player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " dead"), 0, 0);
    }

    public static void Kill(this Item item)
    {
        item.TurnToAir();
    }

    public static void String(Vector2 pos, int i, int ii, int iii, int iiii)
    {
        string a = string.Format("{0}{1}{2}{3}", i, ii, iii, iiii);
        CombatText.NewText(new Rectangle((int) pos.X, (int) pos.Y, 0, 0), Color.White, a);
    }
}

