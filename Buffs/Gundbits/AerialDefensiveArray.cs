using StarsAbove.Systems;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarsAbove.Buffs.Gundbits
{
    public class AerialDefensiveArray : ModBuff
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Fleeting Spark");
			// Description.SetDefault("Static electricity arcs from your minions");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex) {
			WeaponPlayer modPlayer = player.GetModPlayer<WeaponPlayer>();
			if (player.ownedProjectileCounts[ProjectileType<Projectiles.Gundbit.Gundbits>()] > 0) {
				modPlayer.GundbitsActive = true;
			}
			if (!modPlayer.GundbitsActive) {
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else {
				player.buffTime[buffIndex] = 18000;

			}


		}
	}
}