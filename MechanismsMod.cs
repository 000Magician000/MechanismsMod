using Terraria.ModLoader;

namespace MechanismsMod
{
	public class MechanismsMod : Mod
	{
        public static MechanismsMod Instance;
        public MechanismsMod() {
            Instance = this;
            Properties = new ModProperties {
                Autoload = true
            };
		}
	}
}