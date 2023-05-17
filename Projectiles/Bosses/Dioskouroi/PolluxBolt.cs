using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarsAbove.Projectiles.Bosses.Dioskouroi
{
    public class PolluxBolt : ModProjectile
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Freezing Blast");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
		}

		public override void SetDefaults() {
			Projectile.width = 22;               //The width of projectile hitbox
			Projectile.height = 22;              //The height of projectile hitbox
			Projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			Projectile.friendly = true;         //Can the projectile deal damage to enemies?
			Projectile.hostile = true;         //Can the projectile deal damage to the player?
			//projectile.minion = true;           //Is the projectile shoot by a ranged weapon?
			Projectile.penetrate = 99;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.timeLeft = 900;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			Projectile.light = 0.5f;            //How much light emit around the projectile
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false;          //Can the projectile collide with tiles?
			Projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
			AIType = ProjectileID.Bullet;           //Act exactly like default Bullet
		}
		public override bool PreDraw(ref Color lightColor)
		{
			default(Effects.LightBlueTrail).Draw(Projectile);

			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			//If collide with tile, reduce the penetrate.
			//So the projectile can reflect at most 5 times
			
			return false;
		}

		public override void AI()
		{
			
		}

		public override void Kill(int timeLeft)
		{
			for (int d = 0; d < 12; d++)
			{
				Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-3, 3), 0f + Main.rand.Next(-3, 3), 150, default(Color), 1.5f);
			}


		}
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			if(target.type == ModContent.NPCType<NPCs.Dioskouroi.PolluxBoss>())
			{
				modifiers.FinalDamage *= 0;
			}
			if (target.type == ModContent.NPCType<NPCs.Dioskouroi.CastorBoss>())
			{
				modifiers.FinalDamage *= 3;
				modifiers.SetCrit();
			}
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if (target.GetModPlayer<BossPlayer>().temperatureGaugeCold > 0)
			{
				target.GetModPlayer<BossPlayer>().temperatureGaugeCold += 10;

			}

			target.AddBuff(BuffID.Frostburn, 60);
        }
    }
}
