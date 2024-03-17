using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using StarsAbove.Items.Essences;
using Terraria.Audio;
using StarsAbove.Systems;
using StarsAbove.Projectiles.Ranged.Tartaglia;
using StarsAbove.Systems;

namespace StarsAbove.Items.Weapons.Ranged
{
    public class Tartaglia : ModItem
	{
		public override void SetStaticDefaults() {
			/* Tooltip.SetDefault("[c/F7D76A:Hold left click to charge the weapon; the longer the weapon is charged, the stronger the resulting attack will become]" +
				"\nCharging the bow to its maximum before firing will turn the arrow into a unique [c/25ACF0:Riptide Bolt]" +
				"\n[c/25ACF0:Riptide Bolts] will apply [c/4A91FF:Riptide] to foes struck for 12 seconds" +
				"\nRight click to consume 200 mana, entering [c/2361C1:Raging Tides] for 8 seconds" +
				"\nWithin [c/2361C1:Raging Tides], attacks are changed into rapid close-ranged melee strikes that deal double damage and mana will not regenerate" +
				"\nAdditonally, gain extra Movement Speed for the buff's duration" +
				"\nAttacking foes afflicted with [c/4A91FF:Riptide] in this stance will deal extra damage" +
				"\nThis weapon does not use ammo" +
				"\n'Shouldn't let your guard down!'" +
				$""); */

			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

		}

		public override void SetDefaults() {
			Item.damage = 154;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 65;
			Item.height = 70;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 4;

			Item.rare = ItemRarityID.Cyan;
			//item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.channel = true;//Important for all "bows"
			Item.shoot = ProjectileType<Projectiles.Ranged.Tartaglia.tartagliaSwing>(); ;
			Item.shootSpeed = 15f;
			Item.value = Item.buyPrice(gold: 1);           //The value of the weapon

		}
		bool meleeStance = false;
		int meleeStanceTimer = 0;

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			
			if (Main.myPlayer == player.whoAmI)
			{
				
				if (player.altFunctionUse == 2)
				{
					if (player.statMana >= 200)
					{
						if (meleeStance == false)
						{
							SoundEngine.PlaySound(StarsAboveAudio.SFX_swordAttackFinish, player.Center);

							SoundEngine.PlaySound(SoundID.Drown, player.position);
							player.statMana -= 200;//water burst particles
							player.AddBuff(BuffType<Buffs.Ranged.Tartaglia.RagingTidesStance>(), 480);//cosmetic buff
							meleeStanceTimer = 480;
							meleeStance = true;
							for (int d = 0; d < 22; d++)
							{
								Dust.NewDust(player.Center, 0, 0, 15, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default(Color), 0.8f);
							}
							for (int d = 0; d < 22; d++)
							{
								Dust.NewDust(player.Center, 0, 0, 60, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default(Color), 0.8f);
							}
							for (int d = 0; d < 22; d++)
							{
								Dust.NewDust(player.Center, 0, 0, 20, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default(Color), 0.8f);
							}
							for (int d = 0; d < 12; d++)
							{
								Dust.NewDust(player.Center, 0, 0, 135, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default(Color), 0.8f);
							}
							for (int d = 0; d < 88; d++)
							{
								Dust.NewDust(player.Center, 0, 0, 135, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default(Color), 0.8f);
							}
							return true;
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
			}
			return true;

		}
		public override void HoldItem(Player player)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				float launchSpeed = 2f + (int)Math.Round(player.GetModPlayer<WeaponPlayer>().bowCharge / 10);
				Vector2 mousePosition = Main.MouseWorld;
				Vector2 direction = Vector2.Normalize(mousePosition - player.Center);
				Vector2 arrowVelocity = direction * launchSpeed;
				meleeStanceTimer--;
				if (meleeStanceTimer <= 0)
				{
					meleeStance = false;
				}
				if (meleeStance)
				{
					player.manaRegenDelay = 300;
				}
				if (player.altFunctionUse == 2)
				{

				}
				else
				{
					if (meleeStance)
					{

						Item.useTime = 15;
						Item.useAnimation = 15;
						Item.noUseGraphic = true;

					}
					else
					{

						Item.useTime = 40;
						Item.useAnimation = 40;
						Item.noUseGraphic = false;
					}

					if (meleeStance == false)
					{
						if (player.channel)
						{
							Item.useTime = 2;
							Item.useAnimation = 2;
							player.GetModPlayer<WeaponPlayer>().bowChargeActive = true;
							player.GetModPlayer<WeaponPlayer>().bowCharge+=5;
							if (player.GetModPlayer<WeaponPlayer>().bowCharge == 1)
							{
								SoundEngine.PlaySound(StarsAboveAudio.SFX_bowstring, player.Center);
							}
							if (player.GetModPlayer<WeaponPlayer>().bowCharge == 99)
							{
								for (int d = 0; d < 22; d++)
								{
									Dust.NewDust(player.Center, 0, 0, 15, 0f + Main.rand.Next(-12, 12), 0f + Main.rand.Next(-12, 12), 150, default(Color), 0.8f);
								}

								SoundEngine.PlaySound(SoundID.Splash, player.position);
							}
							if (player.GetModPlayer<WeaponPlayer>().bowCharge < 100)
							{
								for (int i = 0; i < 30; i++)
								{//Circle
									Vector2 offset = new Vector2();
									double angle = Main.rand.NextDouble() * 2d * Math.PI;
									offset.X += (float)(Math.Sin(angle) * (100 - player.GetModPlayer<WeaponPlayer>().bowCharge));
									offset.Y += (float)(Math.Cos(angle) * (100 - player.GetModPlayer<WeaponPlayer>().bowCharge));

									Dust d2 = Dust.NewDustPerfect(player.MountedCenter + offset, 15, player.velocity, 200, default(Color), 0.5f);
									d2.fadeIn = 0.1f;
									d2.noGravity = true;
								}
								//Charge dust
								Vector2 vector = new Vector2(
									Main.rand.Next(-28, 28) * (0.003f * 40 - 10),
									Main.rand.Next(-28, 28) * (0.003f * 40 - 10));
								Dust d = Main.dust[Dust.NewDust(
									player.MountedCenter + vector, 1, 1,
									15, 0, 0, 255,
									new Color(0.8f, 0.4f, 1f), 0.8f)];
								d.velocity = -vector / 12;
								d.velocity -= player.velocity / 8;
								d.noLight = true;
								d.noGravity = true;

							}
							else
							{
								Dust.NewDust(player.Center, 0, 0, 15, 0f + Main.rand.Next(-5, 5), 0f + Main.rand.Next(-5, 5), 150, default(Color), 0.8f);
							}
						}
						else
						{
							Item.useTime = 40;
							Item.useAnimation = 40;

							if (player.GetModPlayer<WeaponPlayer>().bowCharge >= 100)
							{
								SoundEngine.PlaySound(SoundID.Item5, player.position);
								Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),player.MountedCenter.X, player.MountedCenter.Y, arrowVelocity.X, arrowVelocity.Y, ProjectileType<RiptideBolt>(), 290, 4, player.whoAmI, 0f);
								player.GetModPlayer<WeaponPlayer>().bowChargeActive = false;
								player.GetModPlayer<WeaponPlayer>().bowCharge = 0;


							}
							else
							{
								if (player.GetModPlayer<WeaponPlayer>().bowCharge > 0)
								{//
									SoundEngine.PlaySound(SoundID.Item5, player.position);

									player.GetModPlayer<WeaponPlayer>().bowChargeActive = false;
									player.GetModPlayer<WeaponPlayer>().bowCharge = 0;

									Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),player.MountedCenter.X, player.MountedCenter.Y, arrowVelocity.X, arrowVelocity.Y, ProjectileType<tartagliaShot>(), 186 + (int)Math.Round(player.GetModPlayer<WeaponPlayer>().bowCharge / 10), 4 + (int)Math.Round(player.GetModPlayer<WeaponPlayer>().bowCharge / 10), player.whoAmI, 0f);
								}
							}
						}
					}
				}
			}
			//item.shootSpeed = 8f + (int)Math.Round(player.GetModPlayer<WeaponPlayer>().bowCharge / 10);
		}

		
		/*
		 * Feel free to uncomment any of the examples below to see what they do
		 */

		// What if I wanted this gun to have a 38% chance not to consume ammo?
		

		// What if I wanted it to work like Uzi, replacing regular bullets with High Velocity Bullets?
		// Uzi/Molten Fury style: Replace normal Bullets with Highvelocity
		/*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (type == ProjectileID.Bullet) // or ProjectileID.WoodenArrowFriendly
			{
				type = ProjectileID.BulletHighVelocity; // or ProjectileID.FireArrow;
			}
			return true; // return true to allow tmodloader to call Projectile.NewProjectile as normal
		}*/

		// What if I wanted it to shoot like a shotgun?
		// Shotgun style: Multiple Projectiles, Random spread 
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{

			}
			else
			{
				if (meleeStance)
				{
					if (player.ownedProjectileCounts[Item.shoot] < 1)
					{
						SoundEngine.PlaySound(SoundID.Item1, player.position);
						Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),player.MountedCenter.X, player.MountedCenter.Y, velocity.X, velocity.Y,type, damage*2, 2, player.whoAmI, 0f);
					}
					return false;
				}
			}
			if (player.channel)
			{

			}
			else
			{
				
			}
			return false;
		}

		// What if I wanted an inaccurate gun? (Chain Gun)
		// Inaccurate Gun style: Single Projectile, Random spread 
		/*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(30));
			velocity.X = perturbedSpeed.X;
			velocity.Y = perturbedSpeed.Y;
			return true;
		}*/

		// What if I wanted multiple projectiles in a even spread? (Vampire Knives) 
		// Even Arc style: Multiple Projectile, Even Spread 
		/*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float numberProjectiles = 3 + Main.rand.Next(3); // 3, 4, or 5 shots
			float rotation = MathHelper.ToRadians(45);
			position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false;
		}*/


		// How can I make the shots appear out of the muzzle exactly?
		// Also, when I do this, how do I prevent shooting through tiles?

		// How can I get a "Clockwork Assault Rifle" effect?
		// 3 round burst, only consume 1 ammo for burst. Delay between bursts, use reuseDelay
		/*	The following changes to SetDefaults()
		 	item.useAnimation = 12;
			item.useTime = 4;
			item.reuseDelay = 14;
		public override void OnConsumeAmmo(Player player)
		{
			// Because of how the game works, player.itemAnimation will be 11, 7, and finally 3. (UseAmination - 1, then - useTime until less than 0.) 
			// We can get the Clockwork Assault Riffle Effect by not consuming ammo when itemAnimation is lower than the first shot.
			return !(player.itemAnimation < item.useAnimation - 2);
		}*/

		// How can I shoot 2 different projectiles at the same time?
		/*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Here we manually spawn the 2nd projectile, manually specifying the projectile type that we wish to shoot.
			Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),position.X, position.Y, velocity.X, velocity.Y,ProjectileID.GrenadeI, damage, knockback, player.whoAmI);
			// By returning true, the vanilla behavior will take place, which will shoot the 1st projectile, the one determined by the ammo.
			return true;
		}*/

		// How can I choose between several projectiles randomly?
		/*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Here we randomly set type to either the original (as defined by the ammo), a vanilla projectile, or a mod projectile.
			type = Main.rand.Next(new int[] { type, ProjectileID.GoldenBullet, ProjectileType<Projectiles.ExampleBullet>() });
			return true;
		}*/
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.Tsunami, 1)
				.AddIngredient(ItemID.SpectreBar, 20)
				.AddIngredient(ItemID.SoulofNight, 20)
				.AddIngredient(ItemType<EssenceOfTheHarbinger>())
				.AddTile(TileID.Anvils)
				.Register();
		}
	}
}
