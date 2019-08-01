using Terraria.DataStructures;
using Terraria.ModLoader;

namespace MechanismsMod.API.TileEntities
{
    public abstract class MechanicalTE : ModTileEntity
    {
        public virtual void HitWire() { }

        public virtual void RightClick() { }

        public virtual void MouseOver() { }

        public virtual void MouseOverFar() { }
    }

    public class Mechanical : GlobalTile
    {
        
    }
}