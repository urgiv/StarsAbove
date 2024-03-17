using StarsAbove.Systems;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarsAbove.Buffs.Pets
{
    public class DuckHuntDogPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Duck Hunter");
            // Description.SetDefault("It seems the name has become outdated");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<WeaponPlayer>().DuckHuntDogPet = true;
            player.buffTime[buffIndex] = 18000;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ProjectileType<Projectiles.Pets.DuckHuntDogPet>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, ProjectileType<Projectiles.Pets.DuckHuntDogPet>(), 0, 0f, player.whoAmI, 0f, 0f);
            }


        }
    }
}