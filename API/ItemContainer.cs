using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace MechanismsMod.API {
    public class ItemContainer {
        public int type, stack;
        public ItemContainer(int type, int stack) {
            this.type = type;
            this.stack = stack;
        }

        public TagCompound GetTagCompound() => new TagCompound {
            ["type"] = type,
            ["stack"] = stack
        };
    }
}
