using Microsoft.Xna.Framework;
using System;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Conditions = Terraria.GameContent.ItemDropRules.Conditions;
using StarsAbove.Items.BossBags;
using StarsAbove.Projectiles.Bosses.Vagrant;
using StarsAbove.NPCs.Vagrant;
using StarsAbove.Buffs.Boss;
using StarsAbove.Utilities;
using StarsAbove.Projectiles.Bosses.Nalhaun;
using SubworldLibrary;
using StarsAbove.Projectiles.Bosses.Tsukiyomi;
using StarsAbove.Projectiles.Bosses.Dioskouroi;
using Terraria.DataStructures;
using StarsAbove.NPCs.Dioskouroi;
using StarsAbove.Projectiles.Bosses.WarriorOfLight;
using StarsAbove.NPCs.WarriorOfLight;
using StarsAbove.Systems;
using StarsAbove.Subworlds;
using StarsAbove.Projectiles.Bosses.OldBossAttacks;
using StarsAbove.Projectiles.Extra;
using StarsAbove.Projectiles.Bosses.Thespian;
using StarsAbove.Projectiles.Bosses.Penthesilea;
using StarsAbove.NPCs.Penthesilea;
using StarsAbove.Projectiles.Bosses.Arbitration;
using StarsAbove.NPCs.Tsukiyomi;
using StarsAbove.NPCs.Arbitration;
using SteelSeries.GameSense.DeviceZone;
using StarsAbove.Projectiles.Bosses.Starfarers;
using StarsAbove.NPCs.Starfarers;
using StarsAbove.NPCs.Nalhaun;
using StarsAbove.Systems;
using StarsAbove.Buffs.Melee.PenthesileaMuse;

namespace StarsAbove.NPCs.AttackLibrary
{
    public class AttackLibrary
	{
		// Here we define an enum we will use with the State slot. Using an ai slot as a means to store "state" can simplify things greatly. Think flowchart.
		// All bosses will look at this ActionState and go from there. Remember this takes the role of NPC.ai[0] so nothing else can go there.
		public enum ActionState
		{
			Spawning,
			Casting,
			PersistentCast,
			Idle,
			Dying,

			
		}
        public static bool ColorblindEnabled = false;
        //The Vagrant of Space and Time's attacks begin here.
        #region Vagrant
        //Vagrant attack. Charges forward, dealing contact damage and firing projectiles from starting position.
        public static void VorpalAssault(Player target, NPC npc)//This is the attack template for future attacks.
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.VorpalAssault");//The name of the attack.
				npc.ai[3] = 50;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);

				SoundEngine.PlaySound(SoundID.Item1, npc.Center);
				//Charge towards the target player.

				

				Vector2 vector8 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height * 0.5f));
				{
					float rotation = (float)Math.Atan2((vector8.Y) - (Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)), (vector8.X) - (Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)));
					npc.velocity.X = (float)(Math.Cos(rotation) * 32) * -1;
					npc.velocity.Y = (float)(Math.Sin(rotation) * 32) * -1;
				}

				
				
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);
				
				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
			




		}

		//Vorpal Barrage (Vagrant's bow attack.)
		public static void VorpalBarrage(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.VorpalBarrage");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.SFX_bowstring, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBowSprite>(), 0, 0, Main.myPlayer);

					//Portal before teleporting.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

					if (npc.ai[2] % 2 == 0)
					{
						Vector2 moveTo = new Vector2(target.Center.X + 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					else
					{
						Vector2 moveTo = new Vector2(target.Center.X - 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}

					//Portal after teleporting
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				
				float Speed = 6f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage;  //projectile damage
				int type = ProjectileType<VagrantStar>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
				
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Green, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float numberProjectiles = 3;
					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}


					
				}
				

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void VorpalSnipe(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.VorpalSnipe");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.SFX_bowstring, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBowSprite>(), 0, 0, Main.myPlayer);

					//Portal before teleporting.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

					if (npc.ai[2] % 2 == 0)
					{
						Vector2 moveTo = new Vector2(target.Center.X + 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					else
					{
						Vector2 moveTo = new Vector2(target.Center.X - 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}

					//Portal after teleporting
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 30f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage;  //projectile damage
				int type = ProjectileType<VagrantStar>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Green, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float numberProjectiles = 3;
					float adjustedRotation = MathHelper.ToRadians(3);
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}


					/*Projectile.NewProjectile(npc.GetSource_FromAI(), 
						StartPosition.X, 
						StartPosition.Y, 
						(float)((Math.Cos(rotation) * Speed) * -1), 
						(float)((Math.Sin(rotation) * Speed) * -1), 
						type, 
						damage, 
						0f, 
						Main.myPlayer);*/
				}
				

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//"Overconfidence equals death."
		public static void VorpalSiege(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{
				//Boss dialogue
				 
				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.VorpalSiege")}", false, false);
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.VorpalSiege");//The name of the attack.

				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.SFX_bowstring, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBowSprite>(), 0, 0, Main.myPlayer);

					//Portal before teleporting.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

					if (npc.ai[2] % 2 == 0)
					{
						Vector2 moveTo = new Vector2(target.Center.X + 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					else
					{
						Vector2 moveTo = new Vector2(target.Center.X - 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}

					//Portal after teleporting
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);
				float Speed = 5f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage;  //projectile damage
				int type = ProjectileType<VagrantStar>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Green, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float numberProjectiles = 5;
					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}


					/*Projectile.NewProjectile(npc.GetSource_FromAI(), 
						StartPosition.X, 
						StartPosition.Y, 
						(float)((Math.Cos(rotation) * Speed) * -1), 
						(float)((Math.Sin(rotation) * Speed) * -1), 
						type, 
						damage, 
						0f, 
						Main.myPlayer);*/
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void InverseStarfall(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.InverseStarfall");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)

					float kitingOffsetX = Utils.Clamp(target.velocity.X * 16, -100, 100);
					Vector2 position = target.Bottom + new Vector2(kitingOffsetX, 500);

					int type = ProjectileType<VagrantStar>();
					int damage = npc.damage;
					var entitySource = npc.GetSource_FromAI();
					for (int d = 0; d < 5; d += 1)
					{
						SoundEngine.PlaySound(SoundID.Item29, new Vector2(position.X - 200 + (d * 100), position.Y));
						Projectile.NewProjectile(entitySource, new Vector2(position.X - 200 + (d * 100), position.Y), -Vector2.UnitY*4, type, damage, 0f, Main.myPlayer);
						for (int ir = 0; ir < 30; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(position.X - 200 + (d * 100), position.Y), (float)ir / 30);

							Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Green, null, 240, default(Color), 0.7f);
							da.fadeIn = 0.3f;
							da.noLight = true;
							da.noGravity = true;

						}
					}

				}
				

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 50;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void Starfall(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Starfall");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)

					float kitingOffsetX = Utils.Clamp(target.velocity.X * 16, -100, 100);
					Vector2 position = target.Bottom + new Vector2(kitingOffsetX, -500);

					int type = ProjectileType<VagrantStar>();
					int damage = npc.damage;
					var entitySource = npc.GetSource_FromAI();

					if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
						for (int d = 0; d < 5; d += 1)
						{
							Projectile.NewProjectile(entitySource, new Vector2(position.X - 200 + (d * 100), position.Y), Vector2.UnitY * 4, type, damage, 0f, Main.myPlayer);

						}


					}
					for (int d = 0; d < 5; d += 1)
					{
						SoundEngine.PlaySound(SoundID.Item29, new Vector2(position.X - 200 + (d * 100), position.Y));

						for (int ir = 0; ir < 30; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(position.X - 200 + (d * 100), position.Y), (float)ir / 30);
							
							Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Green, null, 240, default(Color), 0.7f);
							da.fadeIn = 0.3f;
							da.noLight = true;
							da.noGravity = true;

						}
					}

				}
				
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 50;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		public static void HypertunedInverseStarfall(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.HypertunedInverseStarfall");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)

					float kitingOffsetX = Utils.Clamp(target.velocity.X * 16, -100, 100);
					Vector2 position = target.Bottom + new Vector2(kitingOffsetX, 500);

					int type = ProjectileType<VagrantStar>();
					int damage = npc.damage;
					var entitySource = npc.GetSource_FromAI();
					for (int d = 0; d < 8; d += 1)
					{
						SoundEngine.PlaySound(SoundID.Item29, new Vector2(position.X - 400 + (d * 100), position.Y));
						Projectile.NewProjectile(entitySource, new Vector2(position.X - 400 + (d * 100), position.Y), -Vector2.UnitY * 4, type, damage, 0f, Main.myPlayer);
						for (int ir = 0; ir < 30; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(position.X - 200 + (d * 100), position.Y), (float)ir / 30);

							Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Green, null, 240, default(Color), 0.7f);
							da.fadeIn = 0.3f;
							da.noLight = true;
							da.noGravity = true;

						}
					}

				}
				

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 50;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void HypertunedStarfall(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.HypertunedStarfall");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)

					float kitingOffsetX = Utils.Clamp(target.velocity.X * 16, -100, 100);
					Vector2 position = target.Bottom + new Vector2(kitingOffsetX, -500);

					int type = ProjectileType<VagrantStar>();
					int damage = npc.damage;
					var entitySource = npc.GetSource_FromAI();
					for (int d = 0; d < 8; d += 1)
					{
						SoundEngine.PlaySound(SoundID.Item29, new Vector2(position.X - 400 + (d * 100), position.Y));

						Projectile.NewProjectile(entitySource, new Vector2(position.X - 400 + (d * 100), position.Y), Vector2.UnitY * 4, type, damage, 0f, Main.myPlayer);
						for (int ir = 0; ir < 30; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(position.X - 200 + (d * 100), position.Y), (float)ir / 30);

							Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Green, null, 240, default(Color), 0.7f);
							da.fadeIn = 0.3f;
							da.noLight = true;
							da.noGravity = true;

						}
					}

				}
				

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 50;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void MeteorShower(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
				
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.MeteorShower");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < 7; i++)
					{
						// Random upward vector.
						Vector2 vector2 = new Vector2(Main.rand.NextFloat(-16, 16), Main.rand.NextFloat(-9, -20));

						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vector2, ProjectileID.DD2BetsyFireball, npc.damage, 0, Main.myPlayer, npc.whoAmI, 1);
					}
					

				}
				

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void PrototokiaAster(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{
				//Boss dialogue
				 
				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.prototokia")}", false, false);

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.DemiPrototokia");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)

					float kitingOffsetX = Utils.Clamp(target.velocity.X * 16, -100, 100);
					Vector2 position = target.Bottom + new Vector2(kitingOffsetX, -500);
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Green, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					//Play a sound effect.
					SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);

					int type = ProjectileType<BossTheofania>();
					int damage = npc.damage + 20;
					var entitySource = npc.GetSource_FromAI();
					Projectile.NewProjectile(entitySource, new Vector2(position.X, position.Y - 700), Vector2.UnitY * 14, type, damage, 0f, Main.myPlayer);
					for (int ir = 0; ir < 100; ir++)
					{
							Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(position.X, position.Y - 600), (float)ir / 100);

							Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Green, null, 240, default(Color), 2f);
							da.fadeIn = 0.3f;
							da.noLight = true;
							da.noGravity = true;

					}
					
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Keep your wits about you.
		public static void StarSundering(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)


					int type = ProjectileType<VagrantPlanetslash>();
					int damage = npc.damage;
					var entitySource = npc.GetSource_FromAI();

					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X, npc.Center.Y - 150), Vector2.Zero, ProjectileID.PrincessWeapon, 0, 0f, Main.myPlayer);
					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X, npc.Center.Y - 150), Vector2.Zero, type, damage, 0f, Main.myPlayer);
					SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);

				}


				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.UmbralUpsurge")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.StarSundering");//The name of the attack.
				npc.ai[3] = 130;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSwordSprite>(), 0, 0, Main.myPlayer);


				#region attack
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Green, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		
		//"Backs against the wall- how will you respond?"

		//A: Looks like he's trapped you in... a gravitational loop!
		//E: What's happening? Did he ensnare you in a gravitational loop..?
		public static void GeneralRelativity(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);
				 
 
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.GeneralRelativity");//The name of the attack.
				npc.ai[3] = 100;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//Portal before teleporting.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

				Vector2 moveTo = new Vector2(target.Center.X, target.Center.Y - 80);
				npc.Center = moveTo;

				//Portal after teleporting
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);


				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSlamSprite>(), 0, 0, Main.myPlayer);

				SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
				#region attack

				int index = NPC.NewNPC(null, (int)Main.player[npc.target].Center.X, (int)Main.player[npc.target].Center.Y, ModContent.NPCType<VagrantWalls>());
				NetMessage.SendData(MessageID.SyncNPC, number: index);


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void AdvancedRelativityVertical(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);
				
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AdvancedRelativity");//The name of the attack.
				npc.ai[3] = 100;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//Portal before teleporting.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

				Vector2 moveTo = new Vector2(target.Center.X, target.Center.Y - 80);
				npc.Center = moveTo;

				//Portal after teleporting
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);


				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSlamSprite>(), 0, 0, Main.myPlayer);


				#region attack
				SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
				int index = NPC.NewNPC(null, (int)Main.player[npc.target].Center.X, (int)Main.player[npc.target].Center.Y, ModContent.NPCType<VagrantWallsVertical>());
				NetMessage.SendData(MessageID.SyncNPC, number: index);


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void AdvancedRelativityHorizontal(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);
				 
				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.AdvancedRelativity")}", false, false);
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AdvancedRelativity");//The name of the attack.
				npc.ai[3] = 100;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//Portal before teleporting.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

				Vector2 moveTo = new Vector2(target.Center.X, target.Center.Y - 80);
				npc.Center = moveTo;

				//Portal after teleporting
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);


				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSlamSprite>(), 0, 0, Main.myPlayer);

				SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
				#region attack

				int index = NPC.NewNPC(null, (int)Main.player[npc.target].Center.X, (int)Main.player[npc.target].Center.Y, ModContent.NPCType<VagrantWallsHorizontal>());
				NetMessage.SendData(MessageID.SyncNPC, number: index);

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void Microcosmos(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);
				 
				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Microcosmos");//The name of the attack.
				npc.ai[3] = 100;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//Portal before teleporting.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

				Vector2 moveTo = new Vector2(target.Center.X, target.Center.Y - 80);
				npc.Center = moveTo;

				//Portal after teleporting
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);


				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSlamSprite>(), 0, 0, Main.myPlayer);

				SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)


					int type = ProjectileType<VagrantBulletSwarm>();
					int damage = npc.damage + 5;
					var entitySource = npc.GetSource_FromAI();
					
					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X, npc.Center.Y - 100), Vector2.Zero, type, damage, 0f, Main.myPlayer);
					for (int ir = 0; ir < 50; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X, npc.Center.Y - 100), (float)ir / 50);

						Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Green, null, 240, default(Color), 0.7f);
						da.fadeIn = 0.3f;
						da.noLight = true;
						da.noGravity = true;

					}

				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
#endregion
        //Nalhaun, the Burnished King's attacks begin here.
        #region Nalhaun
        //Purely visual. Use before using Bladework attacks.
        public static void ManifestBlade(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.InfernalIgnition");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Nalhaun.ManifestBlade"), 241, 255, 180); }

					SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					npc.AddBuff(BuffType<NalhaunSword>(), 7200);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<NalhaunSwordSprite>(), 0, 0, Main.myPlayer);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_RuinationIsCome, npc.Center);
				Main.LocalPlayer.GetModPlayer<StarsAbovePlayer>().screenShakeTimerGlobal = -80;


				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//Purely visual.
		public static void RelinquishBlade(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Relinquishment");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int index = npc.FindBuffIndex(BuffType<NalhaunSword>());
					if (index >= 0)
						npc.DelBuff(index);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<NalhaunLoseSwordSprite>(), 0, 0, Main.myPlayer);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.


				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//Purely visual. Use before calling "Fake" attacks.
		public static void MonarchFeint(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.MonarchFeint");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Nalhaun.Feint"), 241, 255, 180); }

					SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);
					
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_RuinationIsCome, npc.Center);



				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}

		//Teleport ability
		public static void Transplacement(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Transplacement");//The name of the attack.
				npc.ai[3] = 40;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//Portal before teleporting.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);

				Vector2 moveTo = new Vector2(target.Center.X, target.Center.Y - 140);
				npc.Center = moveTo;

				//Portal after teleporting
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);


				SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
				

				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}


		//4 consecutive rotating slashes.
		public static void Bladework1(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			
			

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{
				//Global attack-specific variables
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BurnishedBladework");//The name of the attack.
				npc.ai[3] = 20;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);

				//Uses castTime so he holds the sword ready right before striking.
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.ai[0] corresponds to the delay before the swing attack.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<NalhaunSwordAttackSprite>(), 0, 0, Main.myPlayer, 0 + 20);
				}

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int type = ProjectileType<BladeworkSlash>();
					int damage = npc.damage/2 + 20/2;
					var entitySource = npc.GetSource_FromAI();

					//4 rotating slashes at the player's position.

					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);
					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 45f, 25f);
					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 90f, 50f);
					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 135f, 75f);

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//Triangle centered on player.
		public static void Bladework2(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BurnishedBladework");//The name of the attack.
				npc.ai[3] = 10;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.ai[0] corresponds to the delay before the swing attack.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<NalhaunSwordAttackSprite>(), 0, 0, Main.myPlayer, 0 + 30);
				}

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int type = ProjectileType<BladeworkSlash>();
					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();

					//3 rotating slashes forming a triangle

					Projectile.NewProjectile(entitySource, new Vector2(target.Center.X - 70, target.Center.Y - 70), Vector2.Zero, type, damage, 0f, Main.myPlayer, 135f, 0f);
					Projectile.NewProjectile(entitySource, new Vector2(target.Center.X + 70, target.Center.Y - 70), Vector2.Zero, type, damage, 0f, Main.myPlayer, 45f, 5f);
					Projectile.NewProjectile(entitySource, new Vector2(target.Center.X, target.Center.Y + 70), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 10f);

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//Slashes in a + formation.
		public static void Bladework3(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BurnishedBladework");//The name of the attack.
				npc.ai[3] = 20;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.ai[0] corresponds to the delay before the swing attack.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<NalhaunSwordAttackSprite>(), 0, 0, Main.myPlayer, 0 + 30);
				}
				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int type = ProjectileType<BladeworkSlash>();
					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();

					//2 rotating slashes forming a +

					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);
					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 90f, 0f);

					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 120f);
					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 90f, 120f);


				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}

		//Columns of vertical slashes and one horizontal slash on player.
		public static void BladeworkStrong1(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BurnishedBladework");//The name of the attack.
				npc.ai[3] = 40;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.ai[0] corresponds to the delay before the swing attack.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<NalhaunSwordAttackSprite>(), 0, 0, Main.myPlayer, 0 + 30);
				}

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int type = ProjectileType<BladeworkSlash>();
					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();


					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 600, target.Center.Y), new Vector2(target.Center.X + 600, target.Center.Y), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 90f, ir*2);
						

					}
					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);





				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//2 grids of slashes originating from left and right.
		public static void BladeworkStrong2(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BurnishedBladework");//The name of the attack.
				npc.ai[3] = 40;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				if(npc.type == ModContent.NPCType<NalhaunBoss>() || npc.type == ModContent.NPCType<NalhaunBossPhase2>())
				SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_TheHeartsOfMen, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.ai[0] corresponds to the delay before the swing attack.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<NalhaunSwordAttackSprite>(), 0, 0, Main.myPlayer, 20 + 30);
				}

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int type = ProjectileType<BladeworkSlash>();
					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();


					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 600, target.Center.Y + 600), new Vector2(target.Center.X + 600, target.Center.Y - 600), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 90f * ir, ir * 2 + 30);


					}
					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X + 600, target.Center.Y + 600), new Vector2(target.Center.X - 600, target.Center.Y - 600), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 90f * ir, ir * 2 + 120);


					}
					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 45f, 30f);
					Projectile.NewProjectile(entitySource, target.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 135f, 30f);





				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//Rapidly spinning slashes.
		public static void BladeworkStrong3(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BurnishedBladework");//The name of the attack.
				npc.ai[3] = 40;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.ai[0] corresponds to the delay before the swing attack.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<NalhaunSwordAttackSprite>(), 0, 0, Main.myPlayer, 0 + 30);
				}

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int type = ProjectileType<BladeworkSlash>();
					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();


					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = target.Center;

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir*30, ir * 10);


					}
					





				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}

		//Chaotic slashes in "random" directions
		public static void BladeworkStrong4(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BurnishedBladework");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

                #region attack
                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);
                if (npc.type == ModContent.NPCType<NalhaunBoss>() || npc.type == ModContent.NPCType<NalhaunBossPhase2>())
                    SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_ComeShowMeMore, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.ai[0] corresponds to the delay before the swing attack.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<NalhaunSwordAttackSprite>(), 0, 0, Main.myPlayer, 60 + 30);
				}

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int type = ProjectileType<BladeworkSlash>();
					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();

					//Wave 1
					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 700, npc.Center.Y - 100), new Vector2(npc.Center.X + 700, npc.Center.Y + 100), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 29, ir * 10 + 60);


					}
					for (int ir = 0; ir < 4; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X + 600, npc.Center.Y), new Vector2(npc.Center.X - 600, npc.Center.Y), (float)ir / 5);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 32, ir * 10 + 60);


					}


					//Wave 2
					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 600, npc.Center.Y), new Vector2(npc.Center.X + 600, npc.Center.Y), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 69, ir * 10 + 220);


					}
					for (int ir = 0; ir < 4; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 600, npc.Center.Y - 200), new Vector2(npc.Center.X + 600, npc.Center.Y + 200), (float)ir / 5);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 32, ir * 10 + 220);


					}
					for (int ir = 0; ir < 5; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X + 600, npc.Center.Y + 200), new Vector2(npc.Center.X - 600, npc.Center.Y - 200), (float)ir / 5);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 74, ir * 10 + 220);


					}



				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -240;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//Random ivory stake projectiles from the sides and a slow barrage from the sky.
		public static void IvoryStake1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 80);
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.IvoryAdmonishment");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)

					float kitingOffsetX = Utils.Clamp(target.velocity.X * 16, -100, 100);
					Vector2 position = target.Bottom + new Vector2(kitingOffsetX, -500);

					int type = ProjectileType<IvoryStake>();
					int damage = npc.damage/2 + 50/2;
					var entitySource = npc.GetSource_FromAI();

					float Speed = 30f;  //projectile speed
					Vector2 StartPosition1 = new Vector2(target.Center.X - 1000, target.Center.Y - 400);
					Vector2 StartPosition2 = new Vector2(target.Center.X + 1000, target.Center.Y - 400);




					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					for (int i = 0; i < 4; i++)
					{
						float rotation1 = (float)Math.Atan2(StartPosition1.Y - (target.position.Y + (target.height * 0.5f)), StartPosition1.X - (target.position.X + (target.width * 0.5f)));
						float rotation2 = (float)Math.Atan2(StartPosition2.Y - (target.position.Y + (target.height * 0.5f)), StartPosition2.X - (target.position.X + (target.width * 0.5f)));

						Vector2 velocity1 = new Vector2((float)((Math.Cos(rotation1) * Speed) * -1 - i), (float)((Math.Sin(rotation1) * Speed) * -1 - i));
						Vector2 velocity2 = new Vector2((float)((Math.Cos(rotation2) * Speed) * -1 - i), (float)((Math.Sin(rotation2) * Speed) * -1 - i));

						float numberProjectiles = 4;
						float adjustedRotation = MathHelper.ToRadians(15);

						Vector2 perturbedSpeed1 = new Vector2(velocity1.X, velocity1.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f * (i+1); // Watch out for dividing by 0 if there is only 1 projectile.
						
						Vector2 perturbedSpeed2 = new Vector2(velocity2.X, velocity2.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f * (i+1); // Watch out for dividing by 0 if there is only 1 projectile.

						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition1.X, StartPosition1.Y, perturbedSpeed1.X, perturbedSpeed1.Y, type, damage, 0, Main.myPlayer);
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition2.X, StartPosition2.Y, perturbedSpeed2.X, perturbedSpeed2.Y, type, damage, 0, Main.myPlayer);

						
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 600, target.Center.Y - 500), new Vector2(target.Center.X + 600, target.Center.Y - 500), (float)i / 10);
						
						//SoundEngine.PlaySound(SoundID.Item29, positionNew);

						
						

					}
					for (int d = 0; d < 6; d += 1)
					{
						SoundEngine.PlaySound(SoundID.Item29, new Vector2(position.X - 300 + (d * 100), position.Y));

						Projectile.NewProjectile(entitySource, new Vector2(position.X - 300 + (d * 100), position.Y - 200 - d * 100), new Vector2(d * 2 - 4, 8), type, damage, 0f, Main.myPlayer);
						
					}
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 50;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//The same as Ivory Stake 1 but sky barrage is fewer and faster.
		public static void IvoryStake2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 40);
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.IvoryAdmonishment");//The name of the attack.
				npc.ai[3] = 40;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)

					float kitingOffsetX = Utils.Clamp(target.velocity.X * 16, -100, 100);
					Vector2 position = target.Bottom + new Vector2(kitingOffsetX, -500);

					int type = ProjectileType<IvoryStake>();
					int damage = npc.damage/2 + 50/2;
					var entitySource = npc.GetSource_FromAI();

					float Speed = 30f;  //projectile speed
					Vector2 StartPosition1 = new Vector2(target.Center.X - 1000, target.Center.Y - 400);
					Vector2 StartPosition2 = new Vector2(target.Center.X + 1000, target.Center.Y - 400);




					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					for (int i = 0; i < 3; i++)
					{
						float rotation1 = (float)Math.Atan2(StartPosition1.Y - (target.position.Y + (target.height * 0.5f)), StartPosition1.X - (target.position.X + (target.width * 0.5f)));
						float rotation2 = (float)Math.Atan2(StartPosition2.Y - (target.position.Y + (target.height * 0.5f)), StartPosition2.X - (target.position.X + (target.width * 0.5f)));

						Vector2 velocity1 = new Vector2((float)((Math.Cos(rotation1) * Speed) * -1 - i), (float)((Math.Sin(rotation1) * Speed) * -1 - i));
						Vector2 velocity2 = new Vector2((float)((Math.Cos(rotation2) * Speed) * -1 - i), (float)((Math.Sin(rotation2) * Speed) * -1 - i));

						float numberProjectiles = 3;
						float adjustedRotation = MathHelper.ToRadians(15);

						Vector2 perturbedSpeed1 = new Vector2(velocity1.X, velocity1.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f * (i + 1); // Watch out for dividing by 0 if there is only 1 projectile.

						Vector2 perturbedSpeed2 = new Vector2(velocity2.X, velocity2.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f * (i + 1); // Watch out for dividing by 0 if there is only 1 projectile.

						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition1.X, StartPosition1.Y, perturbedSpeed1.X, perturbedSpeed1.Y, type, damage, 0, Main.myPlayer);
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition2.X, StartPosition2.Y, perturbedSpeed2.X, perturbedSpeed2.Y, type, damage, 0, Main.myPlayer);


						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 600, target.Center.Y - 500), new Vector2(target.Center.X + 600, target.Center.Y - 500), (float)i / 10);

						//SoundEngine.PlaySound(SoundID.Item29, positionNew);




					}

					for (int d = 0; d < 3; d += 1)
					{
						SoundEngine.PlaySound(SoundID.Item29, new Vector2(position.X - 200 + (d * 200), position.Y));

						Projectile.NewProjectile(entitySource, new Vector2(position.X - 200 + (d * 200), position.Y - 200 - d*100), new Vector2(d*2 - 2,18), type, damage, 0f, Main.myPlayer);
						
					}

				}
				

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 50;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Cleaves the right half of the arena.
		public static void RightwardRend(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.RightwardRend");//The name of the attack.
				npc.ai[3] = 240;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_EscapeIsNotSoEasilyGranted, npc.Center);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 230);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<LaevateinnSwingRight>(), 0, 0, Main.myPlayer, 230);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);
				
				
				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//Cleaves the right half of the arena, but cast name is "Leftward Rend"
		public static void FakeRightwardRend(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.RightwardRend");//The name of the attack.
				npc.ai[3] = 240;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_EscapeIsNotSoEasilyGranted, npc.Center);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 230);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<LaevateinnSwingLeft>(), 0, 0, Main.myPlayer, 230);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{


				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//After a short delay, cleaves the right half of the arena. Timed with Outer/Inner Agony
		public static void DelayedRightwardRend(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.DelayedRightwardRend");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_EscapeIsNotSoEasilyGranted, npc.Center);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 120);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<LaevateinnSwingRight>(), 0, 0, Main.myPlayer, 420);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{


				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 50;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		public static void LeftwardRend(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.LeftwardRend");//The name of the attack.
				npc.ai[3] = 240;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_EscapeIsNotSoEasilyGranted, npc.Center);

					//10 less
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 230);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<LaevateinnSwingLeft>(), 0, 0, Main.myPlayer, 230);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					


					

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		public static void FakeLeftwardRend(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.LeftwardRend");//The name of the attack.
				npc.ai[3] = 240;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_EscapeIsNotSoEasilyGranted, npc.Center);

					//10 less
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 230);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<LaevateinnSwingRight>(), 0, 0, Main.myPlayer, 230);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{





				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		public static void DelayedLeftwardRend(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.DelayedLeftwardRend");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_EscapeIsNotSoEasilyGranted, npc.Center);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 120);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X, npc.position.Y, 0, 0, ModContent.ProjectileType<LaevateinnSwingLeft>(), 0, 0, Main.myPlayer, 420);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{


				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 50;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}

		//Deals damage to all players in the circle
		public static void InnerAgony(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.InnerAgony");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_PityDisplay, npc.Center);

					//10 less
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 240);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<InnerAgony>(), 40, 0, Main.myPlayer, 120);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					/*
					int type = ProjectileType<NalhaunCleave>();
					int damage = npc.damage/2 + 100/2;
					var entitySource = npc.GetSource_FromAI();



					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 300, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);

					for (int i = 0; i < 60; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-38, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 1.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.Flare, 0f + Main.rand.Next(-28, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 3.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-48, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 2.7f);

					}*/

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		public static void DelayedInnerAgony(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.DelayedInnerAgony");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_PityDisplay, npc.Center);

					//10 less
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 120);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<InnerAgony>(), 40, 0, Main.myPlayer, 480);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					/*
					int type = ProjectileType<NalhaunCleave>();
					int damage = npc.damage/2 + 100/2;
					var entitySource = npc.GetSource_FromAI();



					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 300, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);

					for (int i = 0; i < 60; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-38, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 1.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.Flare, 0f + Main.rand.Next(-28, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 3.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-48, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 2.7f);

					}*/

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		public static void FakeInnerAgony(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.InnerAgony");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_PityDisplay, npc.Center);

					//10 less
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 240);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<OuterAgony>(), 40, 0, Main.myPlayer, 120);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					/*
					int type = ProjectileType<NalhaunCleave>();
					int damage = npc.damage/2 + 100/2;
					var entitySource = npc.GetSource_FromAI();



					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 300, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);

					for (int i = 0; i < 60; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-38, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 1.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.Flare, 0f + Main.rand.Next(-28, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 3.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-48, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 2.7f);

					}*/

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//Deals damage to all players outside the circle.
		public static void OuterAgony(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.OuterAgony");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
					SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_PityDisplay, npc.Center);

					//10 less
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 240);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<OuterAgony>(), 40, 0, Main.myPlayer, 120);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					/*
					int type = ProjectileType<NalhaunCleave>();
					int damage = npc.damage/2 + 100/2;
					var entitySource = npc.GetSource_FromAI();



					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 300, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);

					for (int i = 0; i < 60; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-38, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 1.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.Flare, 0f + Main.rand.Next(-28, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 3.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-48, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 2.7f);

					}*/

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		public static void DelayedOuterAgony(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.DelayedOuterAgony");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_PityDisplay, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);

					//10 less
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 120);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<OuterAgony>(), 40, 0, Main.myPlayer, 480);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					/*
					int type = ProjectileType<NalhaunCleave>();
					int damage = npc.damage/2 + 100/2;
					var entitySource = npc.GetSource_FromAI();



					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 300, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);

					for (int i = 0; i < 60; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-38, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 1.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.Flare, 0f + Main.rand.Next(-28, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 3.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-48, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 2.7f);

					}*/

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		public static void FakeOuterAgony(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.OuterAgony");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_PityDisplay, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);

					//10 less
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 120);

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<InnerAgony>(), 40, 0, Main.myPlayer, 120);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					/*
					int type = ProjectileType<NalhaunCleave>();
					int damage = npc.damage/2 + 100/2;
					var entitySource = npc.GetSource_FromAI();



					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 300, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);

					for (int i = 0; i < 60; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-38, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 1.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.Flare, 0f + Main.rand.Next(-28, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 3.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-48, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 2.7f);

					}*/

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}

		//
		public static void VelvetApogee(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.VelvetApogee");//The name of the attack.
				npc.ai[3] = 40;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 40);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 3f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y - 800);
				int damage = npc.damage/2 + 30/2;  //projectile damage
				int type = ProjectileType<NalhaunBolt>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.Item124, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float numberProjectiles = 12;
					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}


					/*Projectile.NewProjectile(npc.GetSource_FromAI(), 
						StartPosition.X, 
						StartPosition.Y, 
						(float)((Math.Cos(rotation) * Speed) * -1), 
						(float)((Math.Sin(rotation) * Speed) * -1), 
						type, 
						damage, 
						0f, 
						Main.myPlayer);*/
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void VelvetAzimuth(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.VelvetAzimuth");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_WereYouExpectingRust, npc.Center);
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 80);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 6f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 30/2;  //projectile damage
				int type = ProjectileType<NalhaunOrbitingBolt>(); //Type of projectile

				SoundEngine.PlaySound(SoundID.Item124, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float numberProjectiles = 12;
					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < numberProjectiles; i++)
					{
						//For orbiting projectiles = ai[0] is the max orbit distance, ai[1] is the rotation starting position
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i*40 + 150,i*80);

					}
					

				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Summon 1 ruby
		public static void CarrionCall(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CarrionCall");//The name of the attack.
				npc.ai[3] = 100;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_MyDefenses, npc.Center);
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 80);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)


					int type = ProjectileType<NalhaunRuby>();
					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();

					SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, new Vector2(npc.Center.X, npc.Center.Y - 300));

					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X, npc.Center.Y - 300), Vector2.Zero, type, damage, 0f, Main.myPlayer);

					for (int ir = 0; ir < 50; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X, npc.Center.Y - 300), (float)ir / 50);

						Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Red, null, 240, default(Color), 0.7f);
						da.fadeIn = 0.3f;
						da.noLight = true;
						da.noGravity = true;

					}

				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Summons 2 rubies
		public static void CarrionCall2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CarrionCall");//The name of the attack.
				npc.ai[3] = 100;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_MyDefenses, npc.Center);
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 80);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)


					int type = ProjectileType<NalhaunRuby>();
					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();

					SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, new Vector2(npc.Center.X, npc.Center.Y - 300));

					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X + 300, npc.Center.Y - 300), Vector2.Zero, type, damage, 0f, Main.myPlayer);
					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 300, npc.Center.Y - 300), Vector2.Zero, type, damage, 0f, Main.myPlayer);

					for (int ir = 0; ir < 50; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X, npc.Center.Y - 300), (float)ir / 50);

						Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Red, null, 240, default(Color), 0.7f);
						da.fadeIn = 0.3f;
						da.noLight = true;
						da.noGravity = true;

					}
					for (int ir = 0; ir < 50; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X - 300, npc.Center.Y - 300), (float)ir / 50);

						Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Red, null, 240, default(Color), 0.7f);
						da.fadeIn = 0.3f;
						da.noLight = true;
						da.noGravity = true;

					}

				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		public static void ArsLaevateinn(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ArsLaevateinn");//The name of the attack.
				npc.ai[3] = 200;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_AndNowTheScalesWillTip, npc.Center);
				SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<BossLaevateinn>(), 0, 0, Main.myPlayer, 80);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					

				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		public static void Apostasy(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Apostasy");//The name of the attack.
				npc.ai[3] = 340;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                if (npc.type == ModContent.NPCType<NalhaunBoss>() || npc.type == ModContent.NPCType<NalhaunBossPhase2>())
                    SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_TheGodsWillNotBeWatching, npc.Center);
				SoundEngine.PlaySound(StarsAboveAudio.SFX_EnterDarkness, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					

					int type = ProjectileType<NalhaunExplosionIndicator>();
					int damage = npc.damage/2 + 0/2;
					var entitySource = npc.GetSource_FromAI();


					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 700, npc.Center.Y - 100), new Vector2(npc.Center.X + 700, npc.Center.Y - 100), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5);


					}
					for (int ir = 0; ir < 8; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 600, npc.Center.Y + 100), new Vector2(npc.Center.X + 600, npc.Center.Y + 100), (float)ir / 8);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5);


					}
					for (int ir = 0; ir < 5; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 800, npc.Center.Y - 100), new Vector2(npc.Center.X + 800, npc.Center.Y + 100), (float)ir / 5);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5);


					}
					for (int ir = 0; ir < 5; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 800, npc.Center.Y + 100), new Vector2(npc.Center.X + 800, npc.Center.Y - 100), (float)ir / 5);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5);


					}
					//Second Burst

					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 700, npc.Center.Y - 250), new Vector2(npc.Center.X + 700, npc.Center.Y - 250), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5 + 60);


					}
					for (int ir = 0; ir < 8; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 600, npc.Center.Y + 250), new Vector2(npc.Center.X + 600, npc.Center.Y + 250), (float)ir / 8);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5 + 60);


					}
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{


				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		public static void Apostasy2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Apostasy");//The name of the attack.
				npc.ai[3] = 340;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_TheGodsWillNotBeWatching, npc.Center);
				SoundEngine.PlaySound(StarsAboveAudio.SFX_EnterDarkness, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					

					int type = ProjectileType<NalhaunExplosionIndicator>();
					int damage = npc.damage/2 + 0/2;
					var entitySource = npc.GetSource_FromAI();


					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 300, npc.Center.Y + 300), new Vector2(npc.Center.X - 300, npc.Center.Y - 300), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5);


					}
					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X + 300, npc.Center.Y + 300), new Vector2(npc.Center.X + 300, npc.Center.Y - 300), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5);


					}
					for (int ir = 0; ir < 5; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X, npc.Center.Y - 300), new Vector2(npc.Center.X, npc.Center.Y + 300), (float)ir / 5);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5);


					}
					for (int ir = 0; ir < 5; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 800, npc.Center.Y), new Vector2(npc.Center.X + 800, npc.Center.Y), (float)ir / 5);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5);


					}

					//Second Burst

					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y - 250), new Vector2(npc.Center.X + 400, npc.Center.Y + 250), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5 + 60);


					}
					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y - 250), new Vector2(npc.Center.X + 400, npc.Center.Y + 250), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5 + 60);


					}

					//Third

					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y - 250), new Vector2(npc.Center.X - 400, npc.Center.Y + 250), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5 + 120);


					}
					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X + 400, npc.Center.Y - 250), new Vector2(npc.Center.X + 400, npc.Center.Y + 250), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 340 + ir * 5, ir * 5 + 120);


					}
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{


				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
        #endregion

        #region Arbitration

		//Spawns orbiting projectiles that fire inward.
        public static void ShadowsWithal(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ShadowsWithal");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                if (npc.type == ModContent.NPCType<ArbitrationBoss>())
                {
                    SoundEngine.PlaySound(StarsAboveAudio.Arbitration_Voice, npc.Center);


                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {

                    //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 80);
                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                float Speed = 6f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<ArbitrationOrbitingTurretWeakProjectiles>(); //Type of projectile

                SoundEngine.PlaySound(SoundID.Item124, npc.Center);
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float numberProjectiles = 3;
                    float adjustedRotation = MathHelper.ToRadians(135);
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        //For orbiting projectiles = ai[0] is the max orbit distance, ai[1] is the rotation starting position
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 700, i * 135);

                    }

                    //Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<ArbitrationStationaryTurret>(), damage, 0, Main.myPlayer);

                }


                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void DarkDeluge(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.DarkDeluge");//The name of the attack.
                npc.ai[3] = 30;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."




                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float speed = 30f;
                    int type = ProjectileType<ArbitrationBolt>();
                    int damage = npc.damage;
                    var entitySource = npc.GetSource_FromAI();
                    SoundEngine.PlaySound(SoundID.Item124, npc.Center);

                    for (int d = 0; d < 30; d++)
                    {
                        Dust.NewDust(npc.Center, 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                    }

                    for (int ir = 0; ir < 5; ir++)
                    {
                        Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 600, target.Center.Y - 800), new Vector2(target.Center.X + 600, target.Center.Y - 800), (float)ir / 5);
                        float rotation = (float)Math.Atan2(positionNew.Y - (target.position.Y + (target.height * 0.5f)), positionNew.X - (target.position.X + (target.width * 0.5f)));
                        Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * speed) * -1), (float)((Math.Sin(rotation) * speed) * -1));
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y) * .2f;

                        Projectile.NewProjectile(entitySource, positionNew, perturbedSpeed, type, damage, 0f, Main.myPlayer);


                    }

                }


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 100;
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void OrdainedEnd(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.OrdainedEnd");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {



                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack

                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int type = ProjectileType<ArbitrationStationaryTurret>();
                    int damage = npc.damage;
                    var entitySource = npc.GetSource_FromAI();

                    for (int ir = 0; ir < 50; ir++)
                    {
                        Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X, npc.Center.Y - 600), (float)ir / 30);

                        Dust da = Dust.NewDustPerfect(positionNew, DustID.LifeDrain, null, 240, default(Color), 1.7f);
                        da.fadeIn = 0.3f;
                        da.noLight = true;
                        da.noGravity = true;

                    }

                    Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X, npc.Center.Y - 600), Vector2.Zero, type, damage, 0f, Main.myPlayer);


                }
                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void DeathRecital(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.DeathRecital");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                if (npc.type == ModContent.NPCType<ArbitrationBoss>())
                {
                    SoundEngine.PlaySound(StarsAboveAudio.Arbitration_Voice, npc.Center);


                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {

                    //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 80);
                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                float Speed = 6f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage / 2 + 30 / 2;  //projectile damage
                int type = ProjectileType<ArbitrationOrbitingTurretSlow>(); //Type of projectile

                SoundEngine.PlaySound(SoundID.Item124, npc.Center);
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 600, 10);



                }


                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void DarkDesign(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.DarkDesign");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                if (npc.type == ModContent.NPCType<ArbitrationBoss>())
                {
                    SoundEngine.PlaySound(StarsAboveAudio.Arbitration_Voice, npc.Center);


                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {

                    //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<NalhaunCastSprite>(), 0, 0, Main.myPlayer, 80);
                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                float Speed = 6f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage / 2 + 30 / 2;  //projectile damage
                int type = ProjectileType<ArbitrationOrbitingTurret>(); //Type of projectile

                SoundEngine.PlaySound(SoundID.Item124, npc.Center);
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float numberProjectiles = 2;
                    float adjustedRotation = MathHelper.ToRadians(180);
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        //For orbiting projectiles = ai[0] is the max orbit distance, ai[1] is the rotation starting position
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i * 400, i * 180);

                    }


                }


                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void StygianAugur(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.StygianAugur");//The name of the attack.
                npc.ai[3] = 60;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


                if (Main.netMode != NetmodeID.MultiplayerClient)
                {

                    npc.velocity = Vector2.Zero;
                    npc.netUpdate = true;
                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                float Speed = 20f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type1 = ProjectileType<StygianAugurRight>(); //Type of projectile
                int type2 = ProjectileType<StygianAugurLeft>(); //Type of projectile


                //float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                //Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 0.5f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 600; i += 100)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1050, npc.position.Y - 200 + i);

                        float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));
                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type1, damage, 0f, Main.myPlayer);
                    }
                    for (int i = 0; i < 550; i += 100)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X + 950, npc.position.Y + 250 - i);

                        float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));
                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type2, damage, 0f, Main.myPlayer);
                    }
                }


                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 240;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void BloodMoon1(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BloodMoon");//The name of the attack.
                npc.ai[3] = 60;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                
				if (npc.type == ModContent.NPCType<ArbitrationBoss>())
                {

                }

                float Speed = 150f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<BladeworkIndicator>(); //Type of projectile

                float rotation = 0f;
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));


                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


                    float numberProjectiles = 4;
                    float adjustedRotation = MathHelper.ToRadians(90);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(adjustedRotation * i) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, 0, Main.myPlayer);
                    }


                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack
                SoundEngine.PlaySound(SoundID.Item124, npc.Center);

                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }

                float Speed = 60f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<ArbitrationBolt>(); //Type of projectile

                float rotation = 0f;
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item43, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


                    float numberProjectiles = 4;
                    float adjustedRotation = MathHelper.ToRadians(90);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(adjustedRotation * i) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, 0, Main.myPlayer);
                    }


                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 120;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void BloodMoon2(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BloodMoon");//The name of the attack.
                npc.ai[3] = 60;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                if (npc.type == ModContent.NPCType<ArbitrationBoss>())
                {

                }

                float Speed = 150f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<BladeworkIndicator>(); //Type of projectile

                float rotation = 0f;
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));


                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


                    float numberProjectiles = 4;
                    float adjustedRotation = MathHelper.ToRadians(90);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(adjustedRotation * i + MathHelper.ToRadians(45)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, 0, Main.myPlayer);
                    }


                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack
                SoundEngine.PlaySound(SoundID.Item124, npc.Center);

                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }

                float Speed = 60f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<ArbitrationBolt>(); //Type of projectile

                float rotation = 0f;
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item43, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


                    float numberProjectiles = 4;
                    float adjustedRotation = MathHelper.ToRadians(90);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(adjustedRotation * i + MathHelper.ToRadians(45)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, 0, Main.myPlayer);
                    }


                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 120;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void BloodMoon3(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BloodMoon");//The name of the attack.
                npc.ai[3] = 60;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                if (npc.type == ModContent.NPCType<ArbitrationBoss>())
                {

                }

                float Speed = 150f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<BladeworkIndicator>(); //Type of projectile

                float rotation = 0f;
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));


                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


                    float numberProjectiles = 8;
                    float adjustedRotation = MathHelper.ToRadians(45);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(adjustedRotation * i) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, 0, Main.myPlayer);
                    }


                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack
                SoundEngine.PlaySound(SoundID.Item124, npc.Center);

                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }

                float Speed = 60f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<ArbitrationBolt>(); //Type of projectile

                float rotation = 0f;
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item43, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


                    float numberProjectiles = 8;
                    float adjustedRotation = MathHelper.ToRadians(45);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(adjustedRotation * i) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, 0, Main.myPlayer);
                    }


                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }

		//Spawns 2 towers, one with a tether. Target must drag tether to the other tower or die
        public static void SeveredFate1(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.SeveranceOfFate");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack

                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
                    // (The projectiles accelerate from their initial velocity)


                    int type = ProjectileType<ArbitrationTower>();
                    int damage = 0;
                    var entitySource = npc.GetSource_FromAI();

                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
                    target.AddBuff(BuffType<TerrorTether>(), 600);
                    Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 400, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer,0,0);

                    Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X + 400, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer,0,1);

                    for (int ir = 0; ir < 50; ir++)
                    {
                        Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X - 400, npc.Center.Y), (float)ir / 50);

                        Dust da = Dust.NewDustPerfect(positionNew, DustID.Electric, null, 240, default(Color), 0.7f);
                        da.fadeIn = 0.3f;
                        da.noLight = true;
                        da.noGravity = true;

                    }
                    for (int ir = 0; ir < 50; ir++)
                    {
                        Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X + 400, npc.Center.Y), (float)ir / 50);

                        Dust da = Dust.NewDustPerfect(positionNew, DustID.Electric, null, 240, default(Color), 0.7f);
                        da.fadeIn = 0.3f;
                        da.noLight = true;
                        da.noGravity = true;

                    }
                }
                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 120;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void SeveredFate2(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.SeveranceOfFate");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack

                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
                    // (The projectiles accelerate from their initial velocity)


                    int type = ProjectileType<ArbitrationTower>();
                    int damage = 0;
                    var entitySource = npc.GetSource_FromAI();

                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
					target.AddBuff(BuffType<TerrorTether>(), 600);
                    Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 400, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0, 1);

                    Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X + 400, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0, 0);

                    for (int ir = 0; ir < 50; ir++)
                    {
                        Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X - 400, npc.Center.Y), (float)ir / 50);

                        Dust da = Dust.NewDustPerfect(positionNew, DustID.Electric, null, 240, default(Color), 0.7f);
                        da.fadeIn = 0.3f;
                        da.noLight = true;
                        da.noGravity = true;

                    }
                    for (int ir = 0; ir < 50; ir++)
                    {
                        Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X + 400, npc.Center.Y), (float)ir / 50);

                        Dust da = Dust.NewDustPerfect(positionNew, DustID.Electric, null, 240, default(Color), 0.7f);
                        da.fadeIn = 0.3f;
                        da.noLight = true;
                        da.noGravity = true;

                    }
                }
                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 120;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void Titanomachia(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {
                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Titanomachia");//The name of the attack.
                npc.ai[3] = 90;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                if (npc.type == ModContent.NPCType<ArbitrationBoss>())
                {
                    SoundEngine.PlaySound(StarsAboveAudio.Arbitration_Voice, npc.Center);


                }
                float Speed = 30f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type1 = ProjectileType<BladeworkIndicator>(); //Type of projectile
                
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1000 + i, npc.position.Y - 1000);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, 0, Speed, type1, damage, 0f, Main.myPlayer);
                    }
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1000, npc.position.Y - 1000 + i);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, Speed, 0, type1, damage, 0f, Main.myPlayer);
                    }
                }
                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                float Speed = 40f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type1 = ProjectileType<ArbitrationBolt>(); //Type of projectile

                //float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                //Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item124, npc.Center);
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 0.5f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1000 + i, npc.position.Y - 1000);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, 0, Speed, type1, damage, 0f, Main.myPlayer);
                    }
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1000, npc.position.Y - 1000 + i);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, Speed, 0, type1, damage, 0f, Main.myPlayer);
                    }
                }


                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] =  60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void Titanomachia2(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {
                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Titanomachia");//The name of the attack.
                npc.ai[3] = 90;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                if (npc.type == ModContent.NPCType<ArbitrationBoss>())
                {
                    SoundEngine.PlaySound(StarsAboveAudio.Arbitration_Voice, npc.Center);


                }
                float Speed = 30f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type1 = ProjectileType<BladeworkIndicator>(); //Type of projectile

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1050 + i, npc.position.Y + 1050);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, 0, -Speed, type1, damage, 0f, Main.myPlayer);
                    }
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X + 1050, npc.position.Y - 1050 + i);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, -Speed, 0, type1, damage, 0f, Main.myPlayer);
                    }
                }
                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                float Speed = 40f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type1 = ProjectileType<ArbitrationBolt>(); //Type of projectile

                //float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                //Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item124, npc.Center);
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 0.5f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1050 + i, npc.position.Y + 1050);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, 0, -Speed, type1, damage, 0f, Main.myPlayer);
                    }
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X + 1050, npc.position.Y - 1050 + i);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, -Speed, 0, type1, damage, 0f, Main.myPlayer);
                    }
                }


                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void VisionsBeyond(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.VisionsBeyond");//The name of the attack.
                npc.ai[3] = 60;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                if (npc.type == ModContent.NPCType<ArbitrationBoss>())
                {
                    SoundEngine.PlaySound(StarsAboveAudio.Arbitration_Voice, npc.Center);


                }


                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {
                #region attack
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Arbitration.VisionsBeyond"), 241, 255, 180); }
                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player player = Main.player[i];
                        if (player.active)
                        {
                            player.AddBuff(BuffType<Buffs.Boss.VisionsBeyondBuff>(), 120);
                            for (int d = 0; d < 30; d++)
                            {
                                Dust.NewDust(target.Center, 0, 0, DustID.GemSapphire, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
                            }
                        }
                    }
                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 00;//Reset the cast.
                npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        #endregion

        #region Starfarers
        public static void TwinPrototokia(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.TwinPrototokia");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SoundEngine.PlaySound(StarsAboveAudio.SFX_LimitBreakActive, npc.Center);

                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                float Speed = 30f;  //projectile speed
                Vector2 StartPosition1 = new Vector2(npc.Center.X + 700, npc.Center.Y - 800);
                Vector2 StartPosition2 = new Vector2(npc.Center.X - 700, npc.Center.Y - 800);

                int damage = npc.damage + 100;  //projectile damage
                int type = ProjectileType<BossPrototokia>(); //Type of projectile

                float rotation1 = (float)Math.Atan2(StartPosition1.Y - (target.position.Y + (target.height * 0.5f)), StartPosition1.X - (target.position.X + (target.width * 0.5f)));
                float rotation2 = (float)Math.Atan2(StartPosition2.Y - (target.position.Y + (target.height * 0.5f)), StartPosition2.X - (target.position.X + (target.width * 0.5f)));
                Vector2 velocity1 = new Vector2((float)((Math.Cos(rotation1) * Speed) * -1), (float)((Math.Sin(rotation1) * Speed) * -1));
                Vector2 velocity2 = new Vector2((float)((Math.Cos(rotation2) * Speed) * -1), (float)((Math.Sin(rotation2) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item124, npc.Center);
                for (int d = 0; d < 10; d++)
                {
                    Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y + 75), 0, 0, DustID.GemSapphire, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }
                for (int d = 0; d < 10; d++)
                {
                    Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y + 75), 0, 0, DustID.GemAmethyst, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition1.X, StartPosition1.Y, velocity1.X * 5, velocity1.Y * 5, type, damage, 0, Main.myPlayer);
                    Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition2.X, StartPosition2.Y, velocity2.X * 5, velocity2.Y * 5, type, damage, 0, Main.myPlayer);

                }


                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }

        public static void GeminiBurst(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {
                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.GeminiBurst");//The name of the attack.
                npc.ai[3] = 90;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                if (npc.type == ModContent.NPCType<StarfarerBoss>())
                {
                    //SoundEngine.PlaySound(StarsAboveAudio.Arbitration_Voice, npc.Center);


                }
                float Speed = 30f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type1 = ProjectileType<BladeworkIndicator>(); //Type of projectile

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1000 + i, npc.position.Y - 1000);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, Speed, Speed, type1, damage, 0f, Main.myPlayer);
                    }
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1000, npc.position.Y - 1000 + i);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, Speed, Speed, type1, damage, 0f, Main.myPlayer);
                    }
                }
                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                float Speed = 40f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type1 = ProjectileType<ArbitrationBolt>(); //Type of projectile

                //float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                //Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item124, npc.Center);
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 0.5f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1000 + i, npc.position.Y - 1000);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, Speed, Speed, type1, damage, 0f, Main.myPlayer);
                    }
                    for (int i = 0; i < 2000; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X - 1000, npc.position.Y - 1000 + i);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, Speed, Speed, type1, damage, 0f, Main.myPlayer);
                    }
                }


                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        #endregion

        #region Dioskouroi
        //Castor uses different logic for his attacks.
        public static void CastorEnrage(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttackAlt = LangHelper.GetTextValue($"BossAttacks.PhyrricGemini");//The name of the attack.
				npc.ai[3] = 1200;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<radiate>(), 0, 0, Main.myPlayer, 0f);
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Player player = Main.player[i];
					if (player.active && player.Distance(npc.Center) < 1000)
					{
						player.GetModPlayer<StarsAbovePlayer>().screenShakeTimerGlobal = 0;
						player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was obliterated!"), 500, 0);

					}


				}
				Vector2 vector8 = new Vector2(npc.Center.X, npc.Center.Y);
				for (int d = 0; d < 100; d++)
				{
					Dust.NewDust(vector8, 0, 0, 269, 0f + Main.rand.Next(-40, 40), 0f + Main.rand.Next(-40, 40), 150, default(Color), 1.5f);
				}
				for (int d = 0; d < 65; d++)
				{
					Dust.NewDust(vector8, 0, 0, 21, 0f + Main.rand.Next(-45, 45), 0f + Main.rand.Next(-45, 45), 150, default(Color), 1.5f);
				}
				for (int d = 0; d < 35; d++)
				{
					Dust.NewDust(vector8, 0, 0, 50, 0f + Main.rand.Next(-45, 45), 0f + Main.rand.Next(-45, 45), 150, default(Color), 1.5f);
				}
				for (int d = 0; d < 35; d++)
				{
					Dust.NewDust(vector8, 0, 0, 55, 0f + Main.rand.Next(-45, 45), 0f + Main.rand.Next(-45, 45), 150, default(Color), 1.5f);
				}
				npc.active = false;


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttackAlt(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttackAlt = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void BlazingSky(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttackAlt = LangHelper.GetTextValue($"BossAttacks.BlazingAzimuth");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 3f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y - 800);
				int damage = npc.damage/2 + 30/2;  //projectile damage
				int type = ProjectileType<CastorBolt>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.Item124, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float numberProjectiles = 12;
					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}


					/*Projectile.NewProjectile(npc.GetSource_FromAI(), 
						StartPosition.X, 
						StartPosition.Y, 
						(float)((Math.Cos(rotation) * Speed) * -1), 
						(float)((Math.Sin(rotation) * Speed) * -1), 
						type, 
						damage, 
						0f, 
						Main.myPlayer);*/
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttackAlt(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttackAlt = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void Balefire(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttackAlt = LangHelper.GetTextValue($"BossAttacks.Balefire");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{

					float Speed = 6f;  //projectile speed
					Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
					int damage = npc.damage/2 + 30/2;  //projectile damage
					int type = ProjectileType<CastorIgnitionBolt>(); //Type of projectile

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					float numberProjectiles = 6;
					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < numberProjectiles; i++)
					{
						//For this attack = ai[0] is the starting rotation, ai[1] is the rotation starting position
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 120 + i * 5);

					}


				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttackAlt(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttackAlt = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void IgnitionRite(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttackAlt = LangHelper.GetTextValue($"BossAttacks.RiteOfIgnition");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.Item8, new Vector2(npc.position.X, npc.position.Y));
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					int type = ProjectileType<CastorFireCircle>();
					int damage = npc.damage/2 + 20/2;
					var entitySource = npc.GetSource_FromAI();

					for (int ir = 0; ir < 7; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 300, target.Center.Y + 50), new Vector2(target.Center.X + 400, target.Center.Y + 50), (float)ir / 7);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 80, -90);


					}
					
				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				
				


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttackAlt(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttackAlt = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void IgnitionWall(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttackAlt = LangHelper.GetTextValue($"BossAttacks.WallOfIgnition");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.Item8, new Vector2(npc.position.X, npc.position.Y));
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					int type = ProjectileType<CastorFireCircle>();
					int damage = npc.damage/2 + 20/2;
					var entitySource = npc.GetSource_FromAI();

					for (int ir = 0; ir < 7; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 100, npc.Center.Y - 150), new Vector2(npc.Center.X - 100, npc.Center.Y + 150), (float)ir / 7);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 80, 0);


					}

				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack




				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttackAlt(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttackAlt = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void BlazingIgnition(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttackAlt = LangHelper.GetTextValue($"BossAttacks.InfernoOfIgnition");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.Item8, new Vector2(npc.position.X, npc.position.Y));
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					int type = ProjectileType<CastorFireCircle>();
					int damage = npc.damage/2 + 20/2;
					var entitySource = npc.GetSource_FromAI();

					for (int ir = 0; ir < 7; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 300, target.Center.Y - 20), new Vector2(target.Center.X - 300, target.Center.Y - 140), (float)ir / 7);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 80 + ir * 10, 0 + ir * 5);


					}

				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack




				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttackAlt(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttackAlt = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void Exoflare(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttackAlt = LangHelper.GetTextValue($"BossAttacks.Exoflare");//The name of the attack.
				npc.ai[3] = 240;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_TheGodsWillNotBeWatching, npc.Center);
					//SoundEngine.PlaySound(StarsAboveAudio.SFX_EnterDarkness, npc.Center);

					int type = ProjectileType<NalhaunExplosionIndicator>();
					int damage = npc.damage/2 + 0/2;
					var entitySource = npc.GetSource_FromAI();


					for (int ir = 0; ir < 8; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 700, target.Center.Y - 200), new Vector2(target.Center.X + 700, target.Center.Y - 200), (float)ir / 8);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 240 + ir * 5, ir * 5);


					}
					for (int ir = 0; ir < 8; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 600, target.Center.Y), new Vector2(target.Center.X + 600, target.Center.Y), (float)ir / 8);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 330 + ir * 5, ir * 5);


					}
					for (int ir = 0; ir < 8; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 600, target.Center.Y + 200), new Vector2(target.Center.X + 600, target.Center.Y + 200), (float)ir / 8);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 390 + ir * 5, ir * 5);


					}
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{


				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttackAlt(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttackAlt = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void Eruption(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				modPlayer.NextAttackAlt = LangHelper.GetTextValue($"BossAttacks.Eruption");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)

					Vector2 position = new Vector2(npc.Bottom.X + 840, npc.Center.Y + 800);

					int type = ProjectileType<CastorBolt>();
					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();
					for (int d = 0; d < 8; d += 1)
					{
						SoundEngine.PlaySound(SoundID.Item29, new Vector2(position.X - 400 + (d * 100), position.Y));

						Projectile.NewProjectile(entitySource, new Vector2(position.X - 400 + (d * 100), position.Y), Vector2.UnitY * -7, type, damage, 0f, Main.myPlayer);
						for (int ir = 0; ir < 30; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(position.X - 400 + (d * 100), position.Y), (float)ir / 30);

							Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Red, null, 240, default(Color), 0.7f);
							da.fadeIn = 0.3f;
							da.noLight = true;
							da.noGravity = true;

						}
					}

				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttackAlt(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttackAlt = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//The "Clash" lowers both Baleborn to the HP of the lowest Baleborn so they die at the same time-ish.
		public static void ClashCastor(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttackAlt = LangHelper.GetTextValue($"BossAttacks.BalebornClash");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 2f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 20/2;  //projectile damage
				int type = ProjectileType<CastorClashProjectile>(); //Type of projectile

				for (int g = 0; g < 4; g++)
				{
					int goreIndex = Gore.NewGore(null, new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) + 44f), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 1.5f;
					Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
					Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
					goreIndex = Gore.NewGore(null, new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) + 44f), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 1.5f;
					Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
					Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
					goreIndex = Gore.NewGore(null, new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) + 44f), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 1.5f;
					Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
					Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
					goreIndex = Gore.NewGore(null, new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) + 44f), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 1.5f;
					Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
					Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
				}
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC other = Main.npc[i];

					if (other.active && (other.type == ModContent.NPCType<PolluxBoss>()) && other.life > npc.life && other.life > 1)

					{
						other.life = npc.life;
						return;
					}
				}
				if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Dioskouroi.Clash"), 241, 255, 180); }

				SoundEngine.PlaySound(SoundID.Item1, npc.Center);
				Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, Speed,0, type, damage, 0, Main.myPlayer, 120);



				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttackAlt(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttackAlt = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Pollux's attacks.
		public static void PolluxEnrage(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.PhyrricGemini");//The name of the attack.
				npc.ai[3] = 1200;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<radiate>(), 0, 0, Main.myPlayer, 0f);
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Player player = Main.player[i];
					if (player.active && player.Distance(npc.Center) < 1000)
					{
						player.GetModPlayer<StarsAbovePlayer>().screenShakeTimerGlobal = 0;
						player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was obliterated!"), 500, 0);

					}


				}
				Vector2 vector8 = new Vector2(npc.Center.X, npc.Center.Y);
				for (int d = 0; d < 100; d++)
				{
					Dust.NewDust(vector8, 0, 0, 269, 0f + Main.rand.Next(-40, 40), 0f + Main.rand.Next(-40, 40), 150, default(Color), 1.5f);
				}
				for (int d = 0; d < 65; d++)
				{
					Dust.NewDust(vector8, 0, 0, 21, 0f + Main.rand.Next(-45, 45), 0f + Main.rand.Next(-45, 45), 150, default(Color), 1.5f);
				}
				for (int d = 0; d < 35; d++)
				{
					Dust.NewDust(vector8, 0, 0, 50, 0f + Main.rand.Next(-45, 45), 0f + Main.rand.Next(-45, 45), 150, default(Color), 1.5f);
				}
				for (int d = 0; d < 35; d++)
				{
					Dust.NewDust(vector8, 0, 0, 55, 0f + Main.rand.Next(-45, 45), 0f + Main.rand.Next(-45, 45), 150, default(Color), 1.5f);
				}
				npc.active = false;


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void FreezingSky(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.FreezingAzimuth");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 3f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y - 800);
				int damage = npc.damage/2 + 30/2;  //projectile damage
				int type = ProjectileType<PolluxBolt>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.Item124, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float numberProjectiles = 12;
					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}


					/*Projectile.NewProjectile(npc.GetSource_FromAI(), 
						StartPosition.X, 
						StartPosition.Y, 
						(float)((Math.Cos(rotation) * Speed) * -1), 
						(float)((Math.Sin(rotation) * Speed) * -1), 
						type, 
						damage, 
						0f, 
						Main.myPlayer);*/
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void ChillingHail(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ChillingHail");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)

					float kitingOffsetX = Utils.Clamp(target.velocity.X * 16, -100, 100);
					Vector2 position = target.Bottom + new Vector2(kitingOffsetX, -700);

					int type = ProjectileType<PolluxBolt>();
					int damage = npc.damage/2 + 20/2;
					var entitySource = npc.GetSource_FromAI();
					for (int d = 0; d < 5; d += 1)
					{
						SoundEngine.PlaySound(SoundID.Item29, new Vector2(position.X - 200 + (d * 100), position.Y));

						Projectile.NewProjectile(entitySource, new Vector2(position.X - 200 + (d * 100), position.Y), Vector2.UnitY * 4, type, damage, 0f, Main.myPlayer);
						for (int ir = 0; ir < 30; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(position.X - 200 + (d * 100), position.Y), (float)ir / 30);

							Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Blue, null, 240, default(Color), 0.7f);
							da.fadeIn = 0.3f;
							da.noLight = true;
							da.noGravity = true;

						}
					}

				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void ClashPollux(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BalebornClash");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = -2f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 20/2;  //projectile damage
				int type = ProjectileType<PolluxClashProjectile>(); //Type of projectile


				SoundEngine.PlaySound(SoundID.Item1, npc.Center);
				Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, Speed, 0, type, damage, 0, Main.myPlayer, 120);

				for (int g = 0; g < 4; g++)
				{
					int goreIndex = Gore.NewGore(null, new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) + 44f), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 1.5f;
					Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
					Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
					goreIndex = Gore.NewGore(null, new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) + 44f), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 1.5f;
					Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
					Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.5f;
					goreIndex = Gore.NewGore(null, new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) + 44f), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 1.5f;
					Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.5f;
					Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
					goreIndex = Gore.NewGore(null, new Vector2(npc.position.X + (float)(npc.width / 2) - 24f, npc.position.Y + (float)(npc.height / 2) + 44f), default(Vector2), Main.rand.Next(61, 64), 1f);
					Main.gore[goreIndex].scale = 1.5f;
					Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.5f;
					Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.5f;
				}
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC other = Main.npc[i];

					if (other.active && (other.type == ModContent.NPCType<CastorBoss>()) && other.life > npc.life && other.life > 1)

					{
						other.life = npc.life;
						return;
					}
				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void DiamondDust(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.DiamondDust");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_TheGodsWillNotBeWatching, npc.Center);
					//SoundEngine.PlaySound(StarsAboveAudio.SFX_EnterDarkness, npc.Center);

					int type = ProjectileType<PolluxDiamondDust>();
					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();


					for (int ir = 0; ir < 2; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 50, target.Center.Y), new Vector2(target.Center.X + 350, target.Center.Y), (float)ir / 2);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 120 + ir * 5, 60 + ir * 5);


					}


				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{


				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void FrozenArsenal(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.FrozenArsenal");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					
					//SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_KeyOfTheKingsLaw, npc.Center);



				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				SoundEngine.PlaySound(StarsAboveAudio.SFX_summoning, npc.Center);

				Vector2 StartPosition = new Vector2(npc.Center.X - 800, npc.Center.Y);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 10f;
					int type = ProjectileType<PolluxIceBlades>();
					int damage = npc.damage/2 + 30/2;



					for (int ir = 0; ir < 12; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(StartPosition.X - 600, StartPosition.Y - 800), new Vector2(StartPosition.X + 700, StartPosition.Y - 800), (float)ir / 12);
						float rotation = (float)Math.Atan2(positionNew.Y - (StartPosition.Y + (5 * 0.5f)), positionNew.X - (StartPosition.X + (5 * 0.5f)));
						Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * speed) * -1), (float)((Math.Sin(rotation) * speed) * -1));
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y) * .2f;

						Projectile.NewProjectile(npc.GetSource_FromAI(), positionNew, perturbedSpeed, type, damage, 0f, Main.myPlayer);


					}
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void ArmoryOfIce(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ArmoryOfIce");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					//SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_KeyOfTheKingsLaw, npc.Center);



				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				SoundEngine.PlaySound(StarsAboveAudio.SFX_summoning, npc.Center);

				Vector2 StartPosition = new Vector2(target.Center.X, target.Center.Y);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 10f;
					int type = ProjectileType<PolluxIceBlades>();
					int damage = npc.damage/2 + 30/2;



					for (int ir = 0; ir < 6; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(StartPosition.X - 200, StartPosition.Y - 800), new Vector2(StartPosition.X + 200, StartPosition.Y - 800), (float)ir / 6);
						float rotation = (float)Math.Atan2(positionNew.Y - (StartPosition.Y + (5 * 0.5f)), positionNew.X - (StartPosition.X + (5 * 0.5f)));
						Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * speed) * -1), (float)((Math.Sin(rotation) * speed) * -1));
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y) * .2f;

						Projectile.NewProjectile(npc.GetSource_FromAI(), positionNew, perturbedSpeed, type, damage, 0f, Main.myPlayer);


					}
					for (int ir = 0; ir < 3; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(StartPosition.X - 200, StartPosition.Y + 800), new Vector2(StartPosition.X + 200, StartPosition.Y + 800), (float)ir / 3);
						float rotation = (float)Math.Atan2(positionNew.Y - (StartPosition.Y + (5 * 0.5f)), positionNew.X - (StartPosition.X + (5 * 0.5f)));
						Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * speed) * -1), (float)((Math.Sin(rotation) * speed) * -1));
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y) * .2f;

						Projectile.NewProjectile(npc.GetSource_FromAI(), positionNew, perturbedSpeed, type, damage, 0f, Main.myPlayer);


					}
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		#endregion

		#region Thespian
		//Projectiles emerge and move upwards.
		public static void StygianAugurUp(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Argyropeia");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					
					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 20f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage;  //projectile damage
				int type1 = ProjectileType<StygianAugurUp>(); //Type of projectile

				//float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				//Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 0.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < 1000; i += 100)
					{

						Vector2 vector8 = new Vector2(npc.position.X - 400 + i, npc.position.Y + 300);

						float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));
						Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type1, damage, 0f, Main.myPlayer);
					}
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 240;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void StygianAugurDown(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Argyropeia");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 20f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage;  //projectile damage
				int type1 = ProjectileType<StygianAugurDown>(); //Type of projectile

				//float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				//Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 0.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < 1050; i += 100)
					{

						Vector2 vector8 = new Vector2(npc.position.X - 450 + i, npc.position.Y - 200);

						float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));
						Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type1, damage, 0f, Main.myPlayer);
					}
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 240;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void StygianAugurLeft(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Argyropeia");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 20f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage;  //projectile damage
				int type1 = ProjectileType<StygianAugurRight>(); //Type of projectile

				//float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				//Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 0.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < 600; i += 100)
					{

						Vector2 vector8 = new Vector2(npc.position.X - 450 , npc.position.Y - 200 + i);

						float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));
						Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type1, damage, 0f, Main.myPlayer);
					}
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 240;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void StygianAugurRight(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Argyropeia");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 20f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage;  //projectile damage
				int type1 = ProjectileType<StygianAugurLeft>(); //Type of projectile

				//float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				//Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 0.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < 550; i += 100)
					{

						Vector2 vector8 = new Vector2(npc.position.X + 550, npc.position.Y + 250 - i);

						float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));
						Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type1, damage, 0f, Main.myPlayer);
					}
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 240;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Prepare 'apostasy' projectiles, and line the arena with them
		public static void PhlogistonPyrotechnics(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.PhlogistonPyrotechnics");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{


					int type = ProjectileType<ThespianExplosionIndicator>();
					int damage = npc.damage;
					var entitySource = npc.GetSource_FromAI();


					for (int ir = 0; ir < 10; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 700, npc.Center.Y - 200), new Vector2(npc.Center.X + 700, npc.Center.Y - 200), (float)ir / 10);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 240 + ir * 5, ir * 5);


					}
					for (int ir = 0; ir < 8; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 600, npc.Center.Y + 200), new Vector2(npc.Center.X + 600, npc.Center.Y + 200), (float)ir / 8);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 240 + ir * 5, ir * 5);


					}
					
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{


				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void Lixiviate(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Lixiviate");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
					{
						float speed = 4f;
						int type = ProjectileType<BladeworkIndicator>();
						int damage = npc.damage / 2 + 0 / 2;
						var entitySource = npc.GetSource_FromAI();


						for (int ir = 0; ir < 12; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y - 800), new Vector2(npc.Center.X + 1200, npc.Center.Y - 800), (float)ir / 12);
							Vector2 velocity = new Vector2(-16, speed + ir * 4);

							Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


						}

					}
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 1f;
					int type = ProjectileType<Projectiles.Bosses.Thespian.ThespianBolt>();
					int damage = npc.damage/2;
					var entitySource = npc.GetSource_FromAI();

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 0.5f);
					}

					for (int ir = 0; ir < 12; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y - 800), new Vector2(npc.Center.X + 1200, npc.Center.Y - 800), (float)ir / 12);
						Vector2 velocity = new Vector2(-4, speed + ir);

						Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


					}

				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void AthanoricArena(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AthanoricArena");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				#region attack
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Thespian.AthanoricCurse"), 241, 255, 180); }
				if (npc.HasValidTarget)
				{
					for (int i = 0; i < Main.maxPlayers; i++)
					{
						Player player = Main.player[i];
						if (player.active)
						{
							player.AddBuff(BuffType<Buffs.Boss.AthanoricCurse>(), 600);
							for (int d = 0; d < 30; d++)
							{
								Dust.NewDust(target.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
							}
						}
					}
				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 00;//Reset the cast.
				npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Random either move left or move right
		public static void RingmastersWill(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.RingmastersWill");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Thespian.RingmastersWill"), 241, 255, 180); }
				if (npc.HasValidTarget)
				{
					for (int i = 0; i < Main.maxPlayers; i++)
					{
						Player player = Main.player[i];
						if (player.active)
						{
							if(Main.rand.NextBool())
                            {
								player.AddBuff(BuffType<Buffs.Boss.ForceMoveLeft>(), 240);

							}
							else
                            {
								player.AddBuff(BuffType<Buffs.Boss.ForceMoveRight>(), 240);

							}
							for (int d = 0; d < 30; d++)
							{
								Dust.NewDust(target.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
							}
						}
					}
				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
        public static void RingmastersWillLongDelay(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.RingmastersWill");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Thespian.RingmastersWill"), 241, 255, 180); }
                if (npc.HasValidTarget)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player player = Main.player[i];
                        if (player.active)
                        {
                            if (Main.rand.NextBool())
                            {
                                player.AddBuff(BuffType<Buffs.Boss.ForceMoveLeft>(), 600);

                            }
                            else
                            {
                                player.AddBuff(BuffType<Buffs.Boss.ForceMoveRight>(), 600);

                            }
                            for (int d = 0; d < 30; d++)
                            {
                                Dust.NewDust(target.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
                            }
                        }
                    }
                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 120;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        //Same thing except movement is stopped
        public static void RingmastersWillStopMoving(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.RingmastersWill");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
				if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Thespian.RingmastersWill"), 241, 255, 180); }
				if (npc.HasValidTarget)
				{
					for (int i = 0; i < Main.maxPlayers; i++)
					{
						Player player = Main.player[i];
						if (player.active)
						{
							player.AddBuff(BuffType<Buffs.Boss.ForceStopMoving>(), 240);
							for (int d = 0; d < 30; d++)
							{
								Dust.NewDust(target.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
							}
						}
					}
				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

        public static void ParadigmOfChaos(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ParadigmOfChaos");//The name of the attack.
                npc.ai[3] = 60;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {
				//
				
                #region attack
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				//Confetti
                for (int g = 0; g < 24; g++)
                {
                    int goreIndex = Gore.NewGore(null, new Vector2(npc.position.X + npc.width / 2 - 24f, npc.position.Y + npc.height / 2 - 24f), new Vector2(Main.rand.Next(-12,12), Main.rand.Next(-12, 12)), Main.rand.Next(276, 283), 1f);
                }

                //Clear old emotion
                int index = npc.FindBuffIndex(BuffType<ThespianAngry>());
                if (index >= 0)
                    npc.DelBuff(index);

                int index2 = npc.FindBuffIndex(BuffType<ThespianSad>());
                if (index2 >= 0)
                    npc.DelBuff(index2);

                int random = Main.rand.Next(0, 3);
				
				switch (random)
				{
					case 0:
                        for (int d = 0; d < 30; d++)
                        {
                            Dust.NewDust(npc.Center, 0, 0, DustID.GemTopaz, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 2f);
                        }
                        npc.AddBuff(BuffType<ThespianTransition>(), 10);

                        break;

					case 1:
                        for (int d = 0; d < 30; d++)
                        {
                            Dust.NewDust(npc.Center, 0, 0, DustID.GemRuby, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 2f);
                        }
                        npc.AddBuff(BuffType<ThespianAngry>(), 180000);
                        npc.AddBuff(BuffType<ThespianTransition>(), 10);

                        break;

					case 2:
                        for (int d = 0; d < 30; d++)
                        {
                            Dust.NewDust(npc.Center, 0, 0, DustID.GemSapphire, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 2f);
                        }
                        npc.AddBuff(BuffType<ThespianSad>(), 180000);
                        npc.AddBuff(BuffType<ThespianTransition>(), 10);

                        break;
				}

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 120;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
		//Random effect based on Paradigm of Chaos emotion
        public static void AlchemicalAnarchy(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AlchemicalAnarchy");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				if(npc.HasBuff(BuffType<ThespianSad>()))
				{
                    if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Thespian.Sad"), 241, 255, 180); }
                }
                else if (npc.HasBuff(BuffType<ThespianAngry>()))
                {
                    if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Thespian.Angry"), 241, 255, 180); }

                }
                else
				{
                    if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Thespian.Happy"), 241, 255, 180); }

                }


                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {
                //if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Thespian.Chaos"), 241, 255, 180); }


                #region attack
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				int state = 0;//happy
                if (npc.HasBuff(BuffType<ThespianSad>()))
                {
					state = 2;
				}
                else if (npc.HasBuff(BuffType<ThespianAngry>()))
                {
					state = 1;

                }
                else
                {

                }

				//Debug
				//state = 0;

                switch (state)
                {
                    case 0://Happy
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int type = ProjectileType<ThespianExplosionIndicator>();
                            int damage = npc.damage;
                            var entitySource = npc.GetSource_FromAI();


                            for (int ir = 0; ir < 10; ir++)
                            {
                                Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 700, npc.Center.Y - 200), new Vector2(npc.Center.X + 700, npc.Center.Y - 200), (float)ir / 10);

                                Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 240 + ir * 5, ir * 5);


                            }
                            for (int ir = 0; ir < 8; ir++)
                            {
                                Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 600, npc.Center.Y + 200), new Vector2(npc.Center.X + 600, npc.Center.Y + 200), (float)ir / 8);

                                Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 240 + ir * 5, ir * 5);


                            }
                        }
                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            for (int i = 0; i < Main.maxPlayers; i++)
                            {
                                Player player = Main.player[i];
                                if (player.active)
                                {
                                    player.AddBuff(BuffType<Buffs.Boss.ThespianHappyAttack>(), 190);
                                    for (int d = 0; d < 30; d++)
                                    {
                                        Dust.NewDust(target.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
                                    }
                                }
                            }
                        }
                        
                        break;

                    case 1://Angry
                        for (int d = 0; d < 30; d++)
                        {
                            Dust.NewDust(target.Center, 0, 0, DustID.GemRuby, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 2f);
                        }
                        //Inflict all players with either a left or right debuff; once this ends, they will be launched in that direction
                        for (int i = 0; i < Main.maxPlayers; i++)
                        {
                            Player player = Main.player[i];
                            if (player.active)
                            {
                                player.AddBuff(BuffType<Buffs.Boss.AthanoricCurse>(), 220);

                                if (Main.rand.NextBool())
                                {
                                    player.AddBuff(BuffType<Buffs.Boss.LaunchLeft>(), 190);

                                }
                                else
                                {
                                    player.AddBuff(BuffType<Buffs.Boss.LaunchRight>(), 190);

                                }
                                for (int d = 0; d < 30; d++)
                                {
                                    Dust.NewDust(target.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
                                }
                            }
                        }
                        break;

                    case 2://Sad
                        for (int d = 0; d < 30; d++)
                        {
                            Dust.NewDust(target.Center, 0, 0, DustID.GemSapphire, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 2f);
                        }
                        for (int i = 0; i < Main.maxPlayers; i++)
                        {
                            Player player = Main.player[i];
                            if (player.active)
                            {
                                player.AddBuff(BuffType<Buffs.Boss.ThespianSadAttack>(), 190);
                                for (int d = 0; d < 30; d++)
                                {
                                    Dust.NewDust(target.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
                                }
                            }
                        }

                        break;
                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }

        public static void LetsCelebrate(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.LetsCelebrate");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack
                SoundEngine.PlaySound(SoundID.Item156, npc.Center);

                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }

                float Speed = 6f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y - 400);
                int damage = npc.damage / 2;  //projectile damage
				int type = ProjectileID.HappyBomb; //Type of projectile

                float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item43, npc.Center);
                //Confetti
                for (int g = 0; g < 24; g++)
                {
                    int goreIndex = Gore.NewGore(null, new Vector2(npc.position.X + npc.width / 2 - 24f, npc.position.Y + npc.height / 2 - 24f), new Vector2(Main.rand.Next(-12, 12), Main.rand.Next(-12, 12)), Main.rand.Next(276, 283), 1f);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


                    float numberProjectiles = 5;
                    float adjustedRotation = MathHelper.ToRadians(80);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
                    }


                }



                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }

        public static void AethericAlkaheist(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AethericAlkaheist");//The name of the attack.
                npc.ai[3] = 60;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                float Speed = 0f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type1 = ProjectileType<ThespianPotion>(); //Type of projectile

                //float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                //Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 0.5f);
                }             
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 600; i += 200)
                    {
                        Vector2 vector8 = new Vector2(npc.position.X - 450, npc.position.Y - 200 + i);

                        float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));
                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type1, damage, 0f, Main.myPlayer, 100 + i, i);
                    }
                    for (int i = 0; i < 550; i += 200)
                    {

                        Vector2 vector8 = new Vector2(npc.position.X + 550, npc.position.Y + 250 - i);

                        float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));
                        Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type1, damage, 0f, Main.myPlayer, 100 + i, i);
                    }
                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 240;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        #endregion

        #region Penthesilea
        //Projectiles emerge and move upwards.
        


		//Repeats the last attack (currently X)
        public static void ArcipluvianArcanaLinearMystics(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ArcipluvianArcana");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                if (Main.rand.NextBool())
                {
                    if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                        SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_WouldntItBeSoFunny, npc.Center);

                }
                else
                {
                    if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                        SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_MoreWhereThatCameFrom, npc.Center);

                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {

                    npc.velocity = Vector2.Zero;
                    npc.netUpdate = true;
                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        float Speed = -i - 6;  //projectile speed
                                               //Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                        Vector2 position = new Vector2(target.position.X + Main.rand.Next(-300, 300), target.position.Y - Main.rand.Next(100, 500) - (i * 50));
                        int damage = 0;  //projectile damage
                        int type = ProjectileType<Geometry>();

                        float rotation = MathHelper.ToRadians(90);

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromAI(), position.X, position.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, Main.myPlayer);
                        }
                    }



                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }

        public static void ArcipluvianArcanaChromaticCascade(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ArcipluvianArcana");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if(Main.rand.NextBool())
				{
                    if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                        SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_WouldntItBeSoFunny, npc.Center);

                }
                else
				{
                    if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                        SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_MoreWhereThatCameFrom, npc.Center);

                }

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {

                    npc.velocity = Vector2.Zero;
                    npc.netUpdate = true;
                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack

                SoundEngine.PlaySound(SoundID.Item124, npc.Center);

                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }

                float Speed = 4f;  //projectile speed
                Vector2 StartPosition = new Vector2(target.Center.X, target.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<InkClot>(); //Type of projectile

                float rotation = 0f;
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item43, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);

                    float dustAmount = 30f;
                    for (int i = 0; (float)i < dustAmount; i++)
                    {
                        Vector2 spinningpoint5 = Vector2.UnitX * 0f;
                        spinningpoint5 += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 2f / dustAmount)) * new Vector2(4f, 4f);
                        spinningpoint5 = spinningpoint5.RotatedBy(npc.velocity.ToRotation());
                        int dust = Dust.NewDust(StartPosition, 0, 0, DustID.GemSapphire);
                        Main.dust[dust].scale = 2f;
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].position = StartPosition + spinningpoint5;
                        Main.dust[dust].velocity = npc.velocity * 0f + spinningpoint5.SafeNormalize(Vector2.UnitY) * 6f;
                    }
                    float numberProjectiles = 6;
                    float adjustedRotation = MathHelper.ToRadians(180);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
                    }


                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }

        public static void InkOver(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.InkOver");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                {
                    if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                        SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_WhatColor, npc.Center);


                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {
                #region attack
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player player = Main.player[i];
                    if (player.active)
                    {
                        player.ClearBuff(BuffType<RedPaint>());
                        player.ClearBuff(BuffType<BluePaint>());
                        player.ClearBuff(BuffType<YellowPaint>());

                        int color = Main.rand.Next(0, 3);
                        switch (color)
                        {
                            case 0:
                                if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Penthesilea.RedPaint"), 241, 255, 180); }

                                player.AddBuff(BuffType<RedPaint>(), 900);
                                for (int d = 0; d < 30; d++)
                                {
                                    Dust.NewDust(target.Center, 0, 0, DustID.Firework_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
                                }
                                break;
                            case 1:
                                if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Penthesilea.BluePaint"), 241, 255, 180); }

                                player.AddBuff(BuffType<BluePaint>(), 900);
                                for (int d = 0; d < 30; d++)
                                {
                                    Dust.NewDust(target.Center, 0, 0, DustID.Firework_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
                                }
                                break;
                            case 2:
                                if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Penthesilea.YellowPaint"), 241, 255, 180); }

                                player.AddBuff(BuffType<YellowPaint>(), 900);
                                for (int d = 0; d < 30; d++)
                                {
                                    Dust.NewDust(target.Center, 0, 0, DustID.Firework_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1f);
                                }
                                break;
                        }

                    }
                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 00;//Reset the cast.
                npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void TeleportLeftOrRight(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ArtisticDissonance");//The name of the attack.
                npc.ai[3] = 180;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                {
                    if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                        SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_IDontThinkYoullLikeWhatComesNext, npc.Center);


                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {
                #region attack
                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                if (npc.HasValidTarget)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player player = Main.player[i];
                        if (player.active)
                        {
                            int randonBuff = Main.rand.Next(0, 2);
                            if (randonBuff == 0)
                            {
                                player.AddBuff(BuffType<LeftDebuff>(), 200);
                                for (int d = 0; d < 25; d++)
                                {
                                    Dust.NewDust(player.Center, 0, 0, DustID.PurificationPowder, 0f + Main.rand.Next(-25, 25), 0f + Main.rand.Next(-25, 25), 150, default(Color), 0.5f);
                                }
                            }
                            if (randonBuff == 1)
                            {
                                player.AddBuff(BuffType<RightDebuff>(), 200);
                                for (int d = 0; d < 25; d++)
                                {
                                    Dust.NewDust(player.Center, 0, 0, DustID.PurificationPowder, 0f + Main.rand.Next(-25, 25), 0f + Main.rand.Next(-25, 25), 150, default(Color), 0.5f);
                                }
                            }

                        }
                    }
                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 00;//Reset the cast.
                npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        //Spray the arena with the <> projectiles
        public static void LinearMystics(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.LinearMystics");//The name of the attack.
                npc.ai[3] = 60;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {


                    
                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack

                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        float Speed = -i - 6;  //projectile speed
                                               //Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                        Vector2 position = new Vector2(target.position.X + Main.rand.Next(-300, 300), target.position.Y - Main.rand.Next(100, 500) - (i * 50));
                        int damage = 0;  //projectile damage
                        int type = ProjectileType<Geometry>();

                        float rotation = MathHelper.ToRadians(90);

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            Projectile.NewProjectile(npc.GetSource_FromAI(), position.X, position.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, Main.myPlayer);
                        }
                    }



                }
                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void SplatteredSundering(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.SplatteredSundering");//The name of the attack.
                npc.ai[3] = 200;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                {
                    if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                        SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_AlrightMyTurn, npc.Center);


                }
                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int nextType = 0;
                    if (!ColorblindEnabled)
                    {
                        for (int d = 0; d < 5040; d += 420)
                        {

                            Vector2 placement = new Vector2((npc.Center.X) + d, npc.position.Y);
                            int type;
                            if (nextType == 1)
                            {
                                type = ProjectileType<RedSplatter>();
                                if (Main.netMode != NetmodeID.MultiplayerClient) { Projectile.NewProjectile(npc.GetSource_FromAI(), placement.X - 2500, placement.Y, 0, 0, type, 0, 0f, Main.myPlayer); }
                            }
                            if (nextType == 2)
                            {
                                type = ProjectileType<BlueSplatter>();
                                if (Main.netMode != NetmodeID.MultiplayerClient) { Projectile.NewProjectile(npc.GetSource_FromAI(), placement.X - 2500, placement.Y, 0, 0, type, 0, 0f, Main.myPlayer); }
                            }
                            if (nextType == 3)
                            {
                                type = ProjectileType<YellowSplatter>();
                                if (Main.netMode != NetmodeID.MultiplayerClient) { Projectile.NewProjectile(npc.GetSource_FromAI(), placement.X - 2500, placement.Y, 0, 0, type, 0, 0f, Main.myPlayer); }
                                nextType = 0;
                            }

                            nextType++;
                        }
                    }
                    else
                    {
                        for (int d = 0; d < 3500; d += 500)
                        {

                            Vector2 placement = new Vector2((npc.Center.X) + d, npc.position.Y);
                            int type;
                            type = ProjectileType<RendHeaven>();
                            if (Main.netMode != NetmodeID.MultiplayerClient) { Projectile.NewProjectile(npc.GetSource_FromAI(), placement.X - 1500, placement.Y, 0, 0, type, 0, 0f, Main.myPlayer); }
                        }
                    }

                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack
                SoundEngine.PlaySound(StarsAboveAudio.SFX_splat, npc.Center);
                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player player = Main.player[i];
                        if (player.active)
                        {
							player.GetModPlayer<StarsAbovePlayer>().screenShakeTimerGlobal = -90;

                        }
                    }
                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void ElegantBrushwork(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ElegantBrushwork");//The name of the attack.
                npc.ai[3] = 60;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                    SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_QuicklyNow, npc.Center);

                float dustAmount = 60f;
                for (int i = 0; (float)i < dustAmount; i++)
                {
                    Vector2 spinningpoint5 = Vector2.UnitX * 0f;
                    spinningpoint5 += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 2f / dustAmount)) * new Vector2(4f, 4f);
                    spinningpoint5 = spinningpoint5.RotatedBy(npc.velocity.ToRotation());
                    int dust = Dust.NewDust(npc.Center, 0, 0, DustID.GemTopaz);
                    Main.dust[dust].scale = 2f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].position = npc.Center + spinningpoint5;
                    Main.dust[dust].velocity = npc.velocity * 0f + spinningpoint5.SafeNormalize(Vector2.UnitY) * 30f;
                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }

                float Speed = 6f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<PenthBrush>(); //Type of projectile

                float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item43, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float numberProjectiles = 3;
                    float adjustedRotation = MathHelper.ToRadians(35);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
                    }

                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void EfflouresentBrushwork(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.EfflouresentBrushwork");//The name of the attack.
                npc.ai[3] = 240;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                    SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_WrappedThingsUp, npc.Center);
                float dustAmount = 60f;
                for (int i = 0; (float)i < dustAmount; i++)
                {
                    Vector2 spinningpoint5 = Vector2.UnitX * 0f;
                    spinningpoint5 += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 2f / dustAmount)) * new Vector2(4f, 4f);
                    spinningpoint5 = spinningpoint5.RotatedBy(npc.velocity.ToRotation());
                    int dust = Dust.NewDust(npc.Center, 0, 0, DustID.GemTopaz);
                    Main.dust[dust].scale = 2f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].position = npc.Center + spinningpoint5;
                    Main.dust[dust].velocity = npc.velocity * 0f + spinningpoint5.SafeNormalize(Vector2.UnitY) * 30f;
                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }

                float Speed = 6f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<PenthBrush>(); //Type of projectile

                float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item43, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float numberProjectiles = 6;
                    float adjustedRotation = MathHelper.ToRadians(40);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
                    }

                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }

        }
        public static void ArcipluvianArcanaEfflouresentBrushwork(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {


                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ArcipluvianArcana");//The name of the attack.
                npc.ai[3] = 240;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                if (Main.rand.NextBool())
                {
                    if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                        SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_WouldntItBeSoFunny, npc.Center);

                }
                else
                {
                    if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                        SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_MoreWhereThatCameFrom, npc.Center);

                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {

                #region attack
                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }

                float Speed = 6f;  //projectile speed
                Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<PenthBrush>(); //Type of projectile

                float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item43, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float numberProjectiles = 6;
                    float adjustedRotation = MathHelper.ToRadians(40);

                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
                    }

                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }

        }
        public static void SpilledViolet(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.SpilledViolet");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                    SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_RainButPrettier, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {



                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack

                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int type = ProjectileType<PenthesileaInkRain>();
                    int damage = npc.damage;
                    var entitySource = npc.GetSource_FromAI();

                    for (int ir = 0; ir < 50; ir++)
                    {
                        Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X, npc.Center.Y - 600), (float)ir / 30);

                        Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Pink, null, 240, default(Color), 1.7f);
                        da.fadeIn = 0.3f;
                        da.noLight = true;
                        da.noGravity = true;

                    }
                    
                    Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X, npc.Center.Y - 600), Vector2.Zero, type, damage, 0f, Main.myPlayer);


                }
                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = -100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        public static void PenthesileaRightSide(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Rhabtophobia");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                    SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_HandsOn, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {



                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack

                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int type = ProjectileType<PenthesileaCast>();
                    int damage = npc.damage;
                    var entitySource = npc.GetSource_FromAI();
                    float dustAmount = 30f;
					Vector2 EndPosition = new Vector2(npc.Center.X + 600, npc.Center.Y);
                    for (int i = 0; (float)i < dustAmount; i++)
                    {
                        Vector2 spinningpoint5 = Vector2.UnitX * 0f;
                        spinningpoint5 += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 2f / dustAmount)) * new Vector2(4f, 4f);
                        spinningpoint5 = spinningpoint5.RotatedBy(npc.velocity.ToRotation());
                        int dust = Dust.NewDust(npc.Center, 0, 0, DustID.GemSapphire);
                        Main.dust[dust].scale = 2f;
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].position = npc.Center + spinningpoint5;
                        Main.dust[dust].velocity = npc.velocity * 0f + spinningpoint5.SafeNormalize(Vector2.UnitY) * 6f;
                    }

                    for (int i = 0; (float)i < dustAmount; i++)
                    {
                        Vector2 spinningpoint5 = Vector2.UnitX * 0f;
                        spinningpoint5 += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 2f / dustAmount)) * new Vector2(4f, 4f);
                        spinningpoint5 = spinningpoint5.RotatedBy(npc.velocity.ToRotation());
                        int dust = Dust.NewDust(EndPosition, 0, 0, DustID.GemSapphire);
                        Main.dust[dust].scale = 2f;
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].position = EndPosition + spinningpoint5;
                        Main.dust[dust].velocity = npc.velocity * 0f + spinningpoint5.SafeNormalize(Vector2.UnitY) * 6f;
                    }
                    Projectile.NewProjectile(entitySource, EndPosition, Vector2.Zero, type, damage, 0f, Main.myPlayer);


                }
                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 120;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        //Paint from the sky. Some of the paints explode into more pain(t)
        public static void SkyViolet(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BlottedWhims");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


                if (Main.netMode != NetmodeID.MultiplayerClient)
                {



                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack

                if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

                    
                    if (Main.netMode != NetmodeID.MultiplayerClient)
					{
                        float Speed = 8f;  //projectile speed
                        Vector2 StartPosition = new Vector2(npc.Center.X - 800, npc.Center.Y);
                        int damage = npc.damage / 2;  //projectile damage
                        int type = ProjectileType<InkBlot>();
                        int type2 = ProjectileType<InkClot>();

                        float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                        Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                        float numberProjectiles = 3;
                        float adjustedRotation = MathHelper.ToRadians(40);

                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                            Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 2, perturbedSpeed.Y * 2, type2, damage, 0, Main.myPlayer);
                        }
                        numberProjectiles = 7;
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                            Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
                        }
                        float dustAmount = 60f;
                        for (int i = 0; (float)i < dustAmount; i++)
                        {
                            Vector2 spinningpoint5 = Vector2.UnitX * 0f;
                            spinningpoint5 += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 2f / dustAmount)) * new Vector2(4f, 4f);
                            spinningpoint5 = spinningpoint5.RotatedBy(npc.velocity.ToRotation());
                            int dust = Dust.NewDust(StartPosition, 0, 0, DustID.GemAmethyst);
                            Main.dust[dust].scale = 2f;
                            Main.dust[dust].noGravity = true;
                            Main.dust[dust].position = StartPosition + spinningpoint5;
                            Main.dust[dust].velocity = npc.velocity * 0f + spinningpoint5.SafeNormalize(Vector2.UnitY) * 6f;
                        }
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        float Speed = 8f;  //projectile speed
                        Vector2 StartPosition = new Vector2(npc.Center.X + 800, npc.Center.Y);
                        int damage = npc.damage / 2;  //projectile damage
                        int type = ProjectileType<InkBlot>();
                        int type2 = ProjectileType<InkClot>();

                        float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
                        Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                        float numberProjectiles = 3;
                        float adjustedRotation = MathHelper.ToRadians(40);

                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                            Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 2, perturbedSpeed.Y * 2, type2, damage, 0, Main.myPlayer);
                        }
                        numberProjectiles = 7;
                        for (int i = 0; i < numberProjectiles; i++)
                        {
                            Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                            Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
                        }
                        float dustAmount = 60f;
                        for (int i = 0; (float)i < dustAmount; i++)
                        {
                            Vector2 spinningpoint5 = Vector2.UnitX * 0f;
                            spinningpoint5 += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 2f / dustAmount)) * new Vector2(4f, 4f);
                            spinningpoint5 = spinningpoint5.RotatedBy(npc.velocity.ToRotation());
                            int dust = Dust.NewDust(StartPosition, 0, 0, DustID.GemAmethyst);
                            Main.dust[dust].scale = 2f;
                            Main.dust[dust].noGravity = true;
                            Main.dust[dust].position = StartPosition + spinningpoint5;
                            Main.dust[dust].velocity = npc.velocity * 0f + spinningpoint5.SafeNormalize(Vector2.UnitY) * 6f;
                        }
                    }
                }
                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = -100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        //Create a circle of projectiles that burst in a circle after a delay
        public static void ChromaticCascade(Player target, NPC npc)//
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            //Each attack in the Library has 3 important segments.
            //Part 1: Name of the attack and cast time. (ActionState.Idle)
            //Part 2: The actual execution of the attack. (ActionState.Casting)
            //Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

            //Global attack-specific variables

            if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
            {

                //Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
                //Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

                //CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

                modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ChromaticCascade");//The name of the attack.
                npc.ai[3] = 120;//This is the time it takes for the cast to finish.
                npc.localAI[3] = 0;//This resets the cast time.
                npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
                npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

                SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);
                if (npc.type == ModContent.NPCType<PenthesileaBoss>())
                    SoundEngine.PlaySound(StarsAboveAudio.Penthesilea_TooMuchColor, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {



                }

                return;
            }
            if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
            {

                return;
            }
            if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
            {


                #region attack

                SoundEngine.PlaySound(SoundID.Item124, npc.Center);

                for (int d = 0; d < 30; d++)
                {
                    Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
                }

                float Speed = 4f;  //projectile speed
                Vector2 StartPosition = new Vector2(target.Center.X, target.Center.Y);
                int damage = npc.damage;  //projectile damage
                int type = ProjectileType<InkClot>(); //Type of projectile

                float rotation = 0f;
                Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

                SoundEngine.PlaySound(SoundID.Item43, npc.Center);

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    //Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


                    float numberProjectiles = 6;
                    float adjustedRotation = MathHelper.ToRadians(180);
                    float dustAmount = 30f;
                    for (int i = 0; (float)i < dustAmount; i++)
                    {
                        Vector2 spinningpoint5 = Vector2.UnitX * 0f;
                        spinningpoint5 += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 2f / dustAmount)) * new Vector2(4f, 4f);
                        spinningpoint5 = spinningpoint5.RotatedBy(npc.velocity.ToRotation());
                        int dust = Dust.NewDust(StartPosition, 0, 0, DustID.GemSapphire);
                        Main.dust[dust].scale = 2f;
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].position = StartPosition + spinningpoint5;
                        Main.dust[dust].velocity = npc.velocity * 0f + spinningpoint5.SafeNormalize(Vector2.UnitY) * 6f;
                    }
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
                        Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
                    }


                }

                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

                npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
                modPlayer.NextAttack = "";//Empty the UI text.
                npc.localAI[3] = 0;//Reset the cast.
                npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
                npc.ai[2] += 1;//Increment the rotation counter.
                npc.netUpdate = true;//NetUpdate for good measure.

                return;
            }
        }
        #endregion

        #region Tsukiyomi
        //Cutscene activation. Teleports the boss to the right (the other arena.)

        public static void TsukiyomiPhaseChange(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AsAboveEverBelow");//The name of the attack.
				npc.ai[3] = 240;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Player player = Main.player[i];
					if (player.active)
					{

						player.GetModPlayer<BossPlayer>().tsukiCutsceneProgress = 270;
						player.GetModPlayer<BossPlayer>().VideoDuration = 600 + 270;
					}


				}
				if(DownedBossSystem.downedTsuki)//doesn't work yet?
                {
					SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_TakeThisOutside, npc.Center);

				}




				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Player player = Main.player[i];
					if (player.active)
					{
						player.AddBuff(BuffType<Buffs.Invincibility>(), 600);

						npc.localAI[0] = 1;

					}


				}
				npc.AddBuff(BuffType<TsukiyomiTeleportBuff>(), 500);
				if (SubworldSystem.IsActive<EternalConfluence>())
				{
					Vector2 moveTo = new Vector2(npc.position.X + 4900, npc.position.Y + 100);
					npc.Center = moveTo;

				}

				if (!Main.expertMode && !Main.masterMode)
				{
					if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Tsukiyomi.FinalPhase"), 241, 255, 180); }
				}

				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				

				npc.ai[1] = -600;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}

		}
		public static void TsukiyomiPhaseChange2(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.EdinGenesisQuasar");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Player player = Main.player[i];
					if (player.active)
					{

						player.GetModPlayer<BossPlayer>().tsukiCutscene2Progress = 210;
						player.GetModPlayer<BossPlayer>().VideoDuration = 180 + 210;

					}


				}
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_Insignificant, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.localAI[0] = 2;
					//SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_Tedious, npc.Center);
				}
				npc.AddBuff(BuffType<TsukiyomiTeleportBuff>(), 180);

				npc.netUpdate = true;
				#endregion

				if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.Tsukiyomi.FinalPhase"), 241, 255, 180); }

				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -180;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}

		}
		//Laser beams converging on the target.
		//Purely visual. Use before using Bladework attacks.
		public static void TsukiyomiAspectedWeapons(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.TheFirstAspect");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_ForgettingSomething, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 40;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		public static void ThreadsOfFate1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ThreadsOfFate");//The name of the attack.
				npc.ai[3] = 10;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."




				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				
				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 110f;
					int type = ProjectileType<ThreadsOfFate>();
					int damage = npc.damage/2 + 40/2;
					var entitySource = npc.GetSource_FromAI();
					SoundEngine.PlaySound(SoundID.Item124, npc.Center);

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					for (int ir = 0; ir < 5; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 600, target.Center.Y - 800), new Vector2(target.Center.X + 600, target.Center.Y - 800), (float)ir / 5);
						float rotation = (float)Math.Atan2(positionNew.Y - (target.position.Y + (target.height * 0.5f)), positionNew.X - (target.position.X + (target.width * 0.5f)));
						Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * speed) * -1), (float)((Math.Sin(rotation) * speed) * -1));
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y) * .2f;

						Projectile.NewProjectile(entitySource, positionNew, perturbedSpeed, type, damage, 0f, Main.myPlayer);


					}
					
				}


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Laser beam rows centered on the target.
		public static void ThreadsOfFate2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ThreadsOfFate");//The name of the attack.
				npc.ai[3] = 10;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."




				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 110f;
					int type = ProjectileType<ThreadsOfFate>();
					int damage = npc.damage/2 + 40/2;
					var entitySource = npc.GetSource_FromAI();
					SoundEngine.PlaySound(SoundID.Item124, npc.Center);

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					for (int ir = 0; ir < 5; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 300, target.Center.Y - 800), new Vector2(target.Center.X + 300, target.Center.Y - 800), (float)ir / 5);
						Vector2 velocity = new Vector2(0, speed);

						Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


					}

				}


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Laser beam rows centered on the boss.
		public static void ThreadsOfFate3(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ThreadsOfFate");//The name of the attack.
				npc.ai[3] = 10;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."




				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 110f;
					int type = ProjectileType<ThreadsOfFate>();
					int damage = npc.damage/2 + 40/2;
					var entitySource = npc.GetSource_FromAI();
					SoundEngine.PlaySound(SoundID.Item124, npc.Center);

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					for (int ir = 0; ir < 7; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 600, npc.Center.Y - 800), new Vector2(npc.Center.X + 600, npc.Center.Y - 800), (float)ir / 7);
						Vector2 velocity = new Vector2(0, speed);

						Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


					}

				}


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Spray a ton of meteors in the air.
		public static void HypertunedMeteorShower(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				
				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.HypertunedMeteorShower");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < 17; i++)
					{
						// Random upward vector.
						Vector2 vector2 = new Vector2(Main.rand.NextFloat(-16, 16), Main.rand.NextFloat(-9, -20));

						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, vector2, ProjectileID.DD2BetsyFireball, 17, 0, Main.myPlayer, npc.whoAmI, 1);
					}


				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Summon a planet on the target that gets slashed, splitting into different planets.
		public static void Anosios1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Anosios");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."




				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 10f;
					int type = ProjectileType<Projectiles.Bosses.Tsukiyomi.PlusPlanet>();
					int damage = npc.damage/2 + 40/2;
					var entitySource = npc.GetSource_FromAI();
					SoundEngine.PlaySound(SoundID.Item124, npc.Center);

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					Vector2 positionNew = new Vector2(target.Center.X, target.Center.Y - 280);
					Vector2 velocity = new Vector2(0, speed);

					Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);
				}


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 100;
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void Anosios2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AnosiosTriumverate");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."




				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 10f;
					int type = ProjectileType<Projectiles.Bosses.Tsukiyomi.PlusPlanet>();
					int damage = npc.damage/2 + 40/2;
					var entitySource = npc.GetSource_FromAI();
					SoundEngine.PlaySound(SoundID.Item124, npc.Center);

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					Vector2 positionNew1 = new Vector2(target.Center.X, target.Center.Y - 280);
					Vector2 positionNew2 = new Vector2(target.Center.X - 250, target.Center.Y - 280);
					Vector2 positionNew3 = new Vector2(target.Center.X + 250, target.Center.Y - 280);
					Vector2 velocity = new Vector2(0, speed);

					Projectile.NewProjectile(entitySource, positionNew1, velocity, type, damage, 0f, Main.myPlayer);
					Projectile.NewProjectile(entitySource, positionNew2, velocity, type, damage, 0f, Main.myPlayer);
					Projectile.NewProjectile(entitySource, positionNew3, velocity, type, damage, 0f, Main.myPlayer);

				}


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 100;
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Expanding orbiting projectiles.
		public static void CelestialOpposition(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CelestialOpposition");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_NowhereYouCanRun, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{

					float Speed = 6f;  //projectile speed
					Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
					int damage = npc.damage/2 + 30/2;  //projectile damage
					int type = ProjectileType<TsukiExpandingBolt>(); //Type of projectile

					SoundEngine.PlaySound(SoundID.Item124, npc.Center);
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					float numberProjectiles = 12;
					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < numberProjectiles; i++)
					{
						//For this attack = ai[0] is the starting rotation, ai[1] is the rotation starting position
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i * 5, i * 30);

					}


				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void HypertunedCelestialOpposition(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.HypertunedCelestialOpposition");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_NowhereYouCanRun, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{


				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{

					float Speed = 6f;  //projectile speed
					Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
					int damage = npc.damage/2 + 30/2;  //projectile damage
					int type = ProjectileType<TsukiExpandingBolt>(); //Type of projectile

					SoundEngine.PlaySound(SoundID.Item124, npc.Center);
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					float numberProjectiles = 12;
					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < numberProjectiles; i++)
					{
						//For this attack = ai[0] is the starting rotation, ai[1] is the rotation starting position
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i * 5, i * 30);

					}


				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Rows of concentric rings the player must dodge.
		public static void Pandaemonium1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CelestialPandaemonium");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if(npc.type == NPCType<TsukiyomiBoss>())
				{
                    SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_ThousandStars, npc.Center);

                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
				{


				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{

					float Speed = 6f;  //projectile speed
					Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
					int damage = npc.damage/2 + 30/2;  //projectile damage
					int type = ProjectileType<TsukiExpandingBoltDelay>(); //Type of projectile

					SoundEngine.PlaySound(SoundID.Item124, npc.Center);
					for (int d = 0; d < 60; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < 36; i++)
					{
						if (i < 28 || i > 32)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 200, i * 10);

						}
						

					}
					for (int i = 0; i < 36; i++)
					{
						if (i < 18 || i > 22)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 100, i * 10);

						}
						

					}
					for (int i = 0; i < 36; i++)
					{
						if (i < 10 || i > 16)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 1, i * 10);

						}


					}
					
				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -120;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void Pandaemonium2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CelestialPandaemonium");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_ThousandStars, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{


				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{

					float Speed = 6f;  //projectile speed
					Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
					int damage = npc.damage/2 + 30/2;  //projectile damage
					int type = ProjectileType<TsukiExpandingBoltDelay>(); //Type of projectile

					SoundEngine.PlaySound(SoundID.Item124, npc.Center);
					for (int d = 0; d < 60; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < 36; i++)
					{
						if (i < 30 || i > 34)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 1, i * 10);

						}


					}
					for (int i = 0; i < 36; i++)
					{
						if (i < 22 || i > 26)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 100, i * 10);

						}


					}
					for (int i = 0; i < 36; i++)
					{
						if (i < 16 || i > 20)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 200, i * 10);

						}


					}

				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -180;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Raining stars.
		public static void OriginStarfall(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.OriginStarfall");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_Struggle, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
					{
						float speed = 4f;
						int type = ProjectileType<BladeworkIndicator>();
						int damage = npc.damage/2 + 0/2;
						var entitySource = npc.GetSource_FromAI();
						

						for (int ir = 0; ir < 12; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y - 800), new Vector2(npc.Center.X + 1200, npc.Center.Y - 800), (float)ir / 12);
							Vector2 velocity = new Vector2(-16, speed + ir*4);

							Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


						}

					}
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 1f;
					int type = ProjectileType<Projectiles.Bosses.Tsukiyomi.TsukiyomiStar>();
					int damage = npc.damage/2 + 40/2;
					var entitySource = npc.GetSource_FromAI();

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					for (int ir = 0; ir < 12; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y - 800), new Vector2(npc.Center.X + 1200, npc.Center.Y - 800), (float)ir / 12);
						Vector2 velocity = new Vector2(-4, speed + ir);

						Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


					}

				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void OriginStarfall2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.OriginStarsnipe");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 1f;
					int type = ProjectileType<Projectiles.Bosses.Tsukiyomi.TsukiyomiStar>();
					int damage = npc.damage/2 + 40/2;
					var entitySource = npc.GetSource_FromAI();

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					for (int ir = 0; ir < 12; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 1200, target.Center.Y - 800), new Vector2(target.Center.X + 10, target.Center.Y - 800), (float)ir / 12);
						Vector2 velocity = new Vector2(16, speed + ir * 4);

						Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


					}

				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Attacks from the left and right moving from up and down and down to up.
		public static void CosmicUpsurge(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CosmicUpsurge");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_TryHarder, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
					{
						float speed = 40f;
						int type = ProjectileType<BladeworkIndicator>();
						int damage = npc.damage/2 + 0/2;
						var entitySource = npc.GetSource_FromAI();

						for (int ir = 0; ir < 9; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X + 800, npc.Center.Y - 800), new Vector2(npc.Center.X + 800, npc.Center.Y + 800), (float)ir / 9);
							Vector2 velocity = new Vector2(-speed, 0);

							Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


						}
						for (int ir = 0; ir < 7; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 800, npc.Center.Y + 800), new Vector2(npc.Center.X - 800, npc.Center.Y - 800), (float)ir / 7);
							Vector2 velocity = new Vector2(speed, 0);

							Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


						}
					}

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 7f;
					int type = ProjectileType<Projectiles.Bosses.Tsukiyomi.TsukiyomiStar>();
					int damage = npc.damage/2 + 20/2;
					var entitySource = npc.GetSource_FromAI();

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					for (int ir = 0; ir < 9; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X + 800, npc.Center.Y - 800), new Vector2(npc.Center.X + 800, npc.Center.Y + 800), (float)ir / 9);
						Vector2 velocity = new Vector2(-speed, 0);

						Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


					}
					for (int ir = 0; ir < 7; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 800, npc.Center.Y + 800), new Vector2(npc.Center.X - 800, npc.Center.Y - 800), (float)ir / 7);
						Vector2 velocity = new Vector2(speed, 0);

						Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


					}
				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Attacks from the left and right moving from up and down and down to up.
		public static void CosmicUpsurgeFast(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CosmicUpsurge");//The name of the attack.
				npc.ai[3] = 30;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_TryHarder, npc.Center);

					if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
					{
						float speed = 40f;
						int type = ProjectileType<BladeworkIndicator>();
						int damage = npc.damage/2 + 0/2;
						var entitySource = npc.GetSource_FromAI();

						for (int ir = 0; ir < 9; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X + 800, npc.Center.Y - 800), new Vector2(npc.Center.X + 800, npc.Center.Y + 800), (float)ir / 9);
							Vector2 velocity = new Vector2(-speed, 0);

							Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


						}
						for (int ir = 0; ir < 7; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 800, npc.Center.Y + 800), new Vector2(npc.Center.X - 800, npc.Center.Y - 800), (float)ir / 7);
							Vector2 velocity = new Vector2(speed, 0);

							Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


						}
					}

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 7f;
					int type = ProjectileType<Projectiles.Bosses.Tsukiyomi.TsukiyomiStar>();
					int damage = npc.damage/2 + 20/2;
					var entitySource = npc.GetSource_FromAI();

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					for (int ir = 0; ir < 9; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X + 800, npc.Center.Y - 800), new Vector2(npc.Center.X + 800, npc.Center.Y + 800), (float)ir / 9);
						Vector2 velocity = new Vector2(-speed, 0);

						Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


					}
					for (int ir = 0; ir < 7; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 800, npc.Center.Y + 800), new Vector2(npc.Center.X - 800, npc.Center.Y - 800), (float)ir / 7);
						Vector2 velocity = new Vector2(speed, 0);

						Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


					}
				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 80;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Black hole that pulls in players.
		public static void GraspingVoid(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.GraspingVoid");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_AfraidOfTheDark, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{


				}


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int type = ProjectileType<TsukiBlackHole>();
					int damage = npc.damage/2 + 0/2;
					var entitySource = npc.GetSource_FromAI();

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					Vector2 positionNew = new Vector2(npc.Center.X, npc.Center.Y + 150);

					Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer);
				}


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 90;
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Knock players back
		public static void AethericDisruption(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AethericDisruption");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_TryHarder, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{


				}


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<fastRadiate>(), 0, 0, Main.myPlayer, 0f);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				for (int i = 0; i < Main.maxPlayers; i++)
				{
					Player player = Main.player[i];
					if (player.active && player.Distance(npc.Center) < 1600)
					{
						player.velocity = Vector2.Normalize(npc.Center - player.Center) * -16f;
					}
				}
				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 90;
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Aspected Weapon attacks.



		//Carian Dark Moon conal attack facing the target.
		public static void CarianDarkMoon1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CarianDarkMoon");//The name of the attack.
				npc.ai[3] = 160;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                if (npc.type == ModContent.NPCType<TsukiyomiBoss>())
                    SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_CarianDarkMoon, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, -15, ModContent.ProjectileType<TsukiDarkmoonSpawn>(), 0, 0, Main.myPlayer);


					
					
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}

				float Speed = 10f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 75/2;  //projectile damage
				int type = ProjectileType<TsukiMoonlightAttack>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
				
				SoundEngine.PlaySound(SoundID.Item43, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


					float numberProjectiles = 6;
					float adjustedRotation = MathHelper.ToRadians(80);

					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}
					

				}

				
				
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Unused. Fast, targetted moonlight.
		public static void CarianDarkMoon2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.HypertunedCarianDarkMoon");//The name of the attack.
				npc.ai[3] = 160;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_CarianDarkMoon, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, -15, ModContent.ProjectileType<TsukiDarkmoonSpawn>(), 0, 0, Main.myPlayer);




				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}

				float Speed = 20f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 55/2;  //projectile damage
				int type = ProjectileType<TsukiMoonlightAttack>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.Item43, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


					float numberProjectiles = 3;
					float adjustedRotation = MathHelper.ToRadians(8);

					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}


				}
				

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Spawns 3 orbiting projectiles that fire attacks at the target.
		public static void CaesuraOfDespair(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CaesuraOfDespair");//The name of the attack.
				npc.ai[3] = 160;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_CaesuraOfDespair, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, -6, ModContent.ProjectileType<TsukiCaesuraSpawn>(), 0, 0, Main.myPlayer);




				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				SoundEngine.PlaySound(StarsAboveAudio.SFX_summoning, npc.Center);

				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiCaesuraProjectile>(), 30, 0, Main.myPlayer, 0, 120);

					Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiCaesuraProjectile>(), 30, 0, Main.myPlayer, 0, 0);

					Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiCaesuraProjectile>(), 30, 0, Main.myPlayer, 0, 240);


				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		
		//Summons a periodic rain of King's Law projectiles to attack the player.
		public static void KeyOfTheKingsLaw1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.KeyOfTheKingsLaw");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_KeyOfTheKingsLaw, npc.Center);


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<TsukiKeyOfTheKingsLaw>(), 0, 0, Main.myPlayer, 80);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<TsukiWings>(), 0, 0, Main.myPlayer, 80);




				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				SoundEngine.PlaySound(StarsAboveAudio.SFX_summoning, npc.Center);

				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiKingsLawAttack>(), 0, 0, Main.myPlayer, 400, 0);

					
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void HypertunedKeyOfTheKingsLaw(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.HypertunedKeyOfTheKingsLaw");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_KeyOfTheKingsLaw, npc.Center);


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<TsukiKeyOfTheKingsLaw>(), 0, 0, Main.myPlayer, 80);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<TsukiWings>(), 0, 0, Main.myPlayer, 80);




				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				SoundEngine.PlaySound(StarsAboveAudio.SFX_summoning, npc.Center);

				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiKingsLawAttack2>(), 0, 0, Main.myPlayer, 400, 0);


				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Flips away and becomes intangible. After cast finishes, appears on the right targetting a player, and dashes through the arena, leaving a trail of scythes.
		public static void StygianMemento(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.StygianMemento");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				if (Main.rand.NextBool())
				{
					SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_StygianNymph, npc.Center);

				}
				else
				{
					SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_MementoMuse, npc.Center);

				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Center.Y), Vector2.Zero, ProjectileType<TsukiWormhole>(), 0, 0f, Main.myPlayer);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiTeleport>(), 0, 0f, Main.myPlayer);

					npc.AddBuff(BuffType<TsukiyomiTeleportBuff>(), 380);

					



				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, new Vector2(npc.position.X + 800, target.position.Y));

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X + 1200, target.position.Y, 0, 0, ProjectileType<TsukiStygianMemento>(), 50, 0, Main.myPlayer, 200);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X + 1200, target.position.Y, 0, 0, ProjectileType<TsukiStygianIndicator>(), 0, 0, Main.myPlayer, 200);

				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -240;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void HypertunedStygianMemento(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.HypertunedStygianMemento");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				if (Main.rand.NextBool())
				{
					SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_StygianNymph, npc.Center);

				}
				else
				{
					SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_MementoMuse, npc.Center);

				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Center.Y), Vector2.Zero, ProjectileType<TsukiWormhole>(), 0, 0f, Main.myPlayer);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiTeleport>(), 0, 0f, Main.myPlayer);

					npc.AddBuff(BuffType<TsukiyomiTeleportBuff>(), 380);





				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, new Vector2(npc.position.X + 800, target.position.Y));

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X + 1200, target.position.Y, 0, 0, ProjectileType<TsukiStygianMemento2>(), 50, 0, Main.myPlayer, 200);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X + 1200, target.position.Y, 0, 0, ProjectileType<TsukiStygianIndicator>(), 0, 0, Main.myPlayer, 200);

				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -240;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Flips away and becomes intangible. After cast finishes, appears on the left moving downwards, and fires a ton of projectiles.
		public static void VoiceOfTheOutbreak(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.VoiceOfTheOutbreak");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_VoiceOfTheOutbreak, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Center.Y), Vector2.Zero, ProjectileType<TsukiWormhole>(), 0, 0f, Main.myPlayer);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiTeleport>(), 0, 0f, Main.myPlayer);

					npc.AddBuff(BuffType<TsukiyomiTeleportBuff>(), 420);

					



				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, new Vector2(npc.position.X + 800, target.position.Y));

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X - 800, target.position.Y, 0, 0, ProjectileType<TsukiVoiceOfTheOutbreak>(), 50, 0, Main.myPlayer, 240);

				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -220;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void HypertunedVoiceOfTheOutbreak(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.HypertunedVoiceOfTheOutbreak");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_VoiceOfTheOutbreak, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					Projectile.NewProjectile(npc.GetSource_FromAI(), new Vector2(npc.Center.X, npc.Center.Y), Vector2.Zero, ProjectileType<TsukiWormhole>(), 0, 0f, Main.myPlayer);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiTeleport>(), 0, 0f, Main.myPlayer);

					npc.AddBuff(BuffType<TsukiyomiTeleportBuff>(), 420);





				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, new Vector2(npc.position.X + 800, target.position.Y));

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.position.X - 800, target.position.Y, 0, 0, ProjectileType<TsukiVoiceOfTheOutbreak2>(), 70, 0, Main.myPlayer, 240);

				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -220;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//The teleports after cast finishes and places are targetted with her afterimage and then immediately get slashed
		public static void ShadowlessCerulean1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ShadowlessCerulean");//The name of the attack.
				npc.ai[3] = 160;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_ShadowlessCerulean, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiShadowlessCerulean>(), 0, 0f, Main.myPlayer, 160);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiWings>(), 0, 0f, Main.myPlayer, 160);
				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, new Vector2(npc.position.X, npc.position.Y));
				npc.AddBuff(BuffType<TsukiyomiTeleportBuff>(), 220);

				int type = ProjectileType<TsukiShadowlessCeruleanAfterimage>();
				int damage = npc.damage/2 + 50/2;
				var entitySource = npc.GetSource_FromAI();


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					for (int ir = 0; ir < 2; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X, target.Center.Y - 100), new Vector2(target.Center.X, target.Center.Y + 100), (float)ir / 2);


						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, 0, 0f, Main.myPlayer, ir * 30 + 40);
						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, ProjectileType<TsukiCeruleanBladeworkSlash>(), damage, 0f, Main.myPlayer, 0, ir * 30 + 30);

					}
					for (int ir = 0; ir < 3; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y - 100), new Vector2(npc.Center.X + 400, npc.Center.Y + 400), (float)ir / 3);
						

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, 0, 0f, Main.myPlayer, ir * 30 + 80);
						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, ProjectileType<TsukiCeruleanBladeworkSlash>(), damage, 0f, Main.myPlayer, 0, ir * 30 + 70);

					}
					for (int ir = 0; ir < 5; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 200, npc.Center.Y - 100), new Vector2(npc.Center.X + 200, npc.Center.Y + 400), (float)ir / 5);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, 0, 0f, Main.myPlayer, ir * 10 + 50);
						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, ProjectileType<TsukiCeruleanBladeworkSlash>(), damage, 0f, Main.myPlayer, 0, ir * 10 + 40);


					}
				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -220;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//The arena is marked with a wide variety of tentacles which burst forth after a short delay.
		public static void Takonomicon1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Takonomicon");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                
                if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
                    if (npc.type == ModContent.NPCType<TsukiyomiBoss>())
                    {
                        SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_Takonomicon, npc.Center);
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiTakonomicon>(), 0, 0f, Main.myPlayer, 120);
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiWings>(), 0, 0f, Main.myPlayer, 120);
                    }
                    
				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				SoundEngine.PlaySound(SoundID.Item8, new Vector2(npc.position.X, npc.position.Y));
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				int type = ProjectileType<TsukiTentacleCircle>();
				int damage = npc.damage/2 + 50/2;
				var entitySource = npc.GetSource_FromAI();



				
				for (int ir = 0; ir < 4; ir++)
				{
					Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 700, target.Center.Y), new Vector2(target.Center.X + 700, target.Center.Y), (float)ir / 4);

					Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 32);


				}
				for(int ir = 0; ir < 3; ir++)
				{
					Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 500, target.Center.Y - 100), new Vector2(target.Center.X + 500, target.Center.Y), (float)ir / 3);

					Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 122);


				}
				
				
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 40;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Another tentacle attack, faster this time and without voice line.
		public static void Takonomicon2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Takonomicon");//The name of the attack.
				npc.ai[3] = 40;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				//SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_Takonomicon, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiTakonomicon>(), 0, 0f, Main.myPlayer, 40);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiWings>(), 0, 0f, Main.myPlayer, 40);
				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				SoundEngine.PlaySound(SoundID.Item8, new Vector2(npc.position.X, npc.position.Y));
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				int type = ProjectileType<TsukiTentacleCircle>();
				int damage = npc.damage/2 + 30/2;
				var entitySource = npc.GetSource_FromAI();




				for (int ir = 0; ir < 4; ir++)
				{
					Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 300, target.Center.Y), new Vector2(target.Center.X + 300, target.Center.Y), (float)ir / 4);

					Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 162);


				}
				for (int ir = 0; ir < 3; ir++)
				{
					Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 200, target.Center.Y - 100), new Vector2(target.Center.X + 200, target.Center.Y), (float)ir / 3);

					Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 92);


				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void LuminaryWand(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.LuminaryWand");//The name of the attack.
				npc.ai[3] = 80;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_LuminaryWand, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiLuminaryWand>(), 0, 0f, Main.myPlayer, 80);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiWings>(), 0, 0f, Main.myPlayer, 80);
				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				SoundEngine.PlaySound(SoundID.Item8, new Vector2(npc.position.X, npc.position.Y));
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				SoundEngine.PlaySound(StarsAboveAudio.SFX_StarbitCollected, npc.Center);

				int type = ProjectileType<TsukiStarchild>();
				int damage = npc.damage/2 + 20/2;
				var entitySource = npc.GetSource_FromAI();
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(entitySource, npc.Center, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0);
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 40;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Target the player, and unleash slashes in a + then X formation.
		public static void TheOnlyThingIKnowForReal1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.JetstreamBloodshed");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_TheOnlyThingIKnowForReal, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiBloodshedSheathe>(), 0, 0f, Main.myPlayer, 120);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiWings>(), 0, 0f, Main.myPlayer, 120);


					int type = ProjectileType<TsukiBloodshedIndicator>();
					int damage = npc.damage/2 + 0/2;
					var entitySource = npc.GetSource_FromAI();


					for (int ir = 0; ir < 6; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 400, target.Center.Y), new Vector2(target.Center.X + 550, target.Center.Y), (float)ir / 6);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 140 + ir * 2, ir * 2);


					}
					for (int ir = 0; ir < 6; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X, target.Center.Y - 400), new Vector2(target.Center.X, target.Center.Y + 600), (float)ir / 6);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 140 + ir * 2, ir * 2);


					}

					//Second Burst

					for (int ir = 0; ir < 6; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X - 400, target.Center.Y - 400), new Vector2(target.Center.X + 550, target.Center.Y + 450), (float)ir / 6);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 140 + ir * 2, ir * 2 + 20);


					}
					for (int ir = 0; ir < 6; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(target.Center.X + 400, target.Center.Y - 400), new Vector2(target.Center.X - 550, target.Center.Y + 450), (float)ir / 6);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 140 + ir * 2, ir * 2 + 20);


					}

				} 
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				npc.AddBuff(BuffType<TsukiyomiTeleportBuff>(), 60);
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<fastRadiate>(), 0, 0, Main.myPlayer, 0f);

				//SoundEngine.PlaySound(StarsAboveAudio.SFX_BuryTheLightPrep, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//After a delay, unleash a large variety of slashes on the screen.
		public static void BuryTheLight1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BuryTheLight");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
                    if (npc.type == ModContent.NPCType<TsukiyomiBoss>())
                    {
                        SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_BuryTheLight, npc.Center);
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiBuryTheLight>(), 0, 0f, Main.myPlayer, 120);
                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiWings>(), 0, 0f, Main.myPlayer, 120);
                    }
                    
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				npc.AddBuff(BuffType<TsukiyomiTeleportBuff>(), 220);

				
				SoundEngine.PlaySound(StarsAboveAudio.SFX_BuryTheLightPrep, npc.Center);
				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
                {
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<fastRadiate>(), 0, 0, Main.myPlayer, 0f);

                    int type = ProjectileType<TsukiBladeworkSlash>();
                    int damage = npc.damage/2 + 50/2;
                    var entitySource = npc.GetSource_FromAI();

					

					//Wave 1
					for (int ir = 0; ir < 15; ir++)
                    {
                        Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 800, npc.Center.Y + 100), new Vector2(npc.Center.X + 800, npc.Center.Y + 100), (float)ir / 15);

                        Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 69, 130);


                    }
                    for (int ir = 0; ir < 4; ir++)
                    {
                        Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 600, npc.Center.Y - 200), new Vector2(npc.Center.X + 600, npc.Center.Y + 400), (float)ir / 4);

                        Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 32, 130);


                    }
                    for (int ir = 0; ir < 5; ir++)
                    {
                        Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X + 600, npc.Center.Y + 400), new Vector2(npc.Center.X - 600, npc.Center.Y - 200), (float)ir / 5);

                        Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 74, 130);


                    }




                }


                #endregion


                //After the attack ends, we do some cleanup.
                ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -200;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void BuryTheLight2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.BuryTheLight");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_BuryTheLight, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiBuryTheLight>(), 0, 0f, Main.myPlayer, 120);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiWings>(), 0, 0f, Main.myPlayer, 120);
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				npc.AddBuff(BuffType<TsukiyomiTeleportBuff>(), 220);


				SoundEngine.PlaySound(StarsAboveAudio.SFX_BuryTheLightPrep, npc.Center);
				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<fastRadiate>(), 0, 0, Main.myPlayer, 0f);

					int type = ProjectileType<TsukiBladeworkSlash>();
					int damage = npc.damage/2 + 50/2;
					var entitySource = npc.GetSource_FromAI();



					//Wave 1
					for (int ir = 0; ir < 7; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y), new Vector2(npc.Center.X + 900, npc.Center.Y), (float)ir / 7);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 12, 130);


					}
					for (int ir = 0; ir < 3; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 600, npc.Center.Y - 300), new Vector2(npc.Center.X + 600, npc.Center.Y + 100), (float)ir / 3);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 66, 130);


					}
					for (int ir = 0; ir < 3; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X + 600, npc.Center.Y + 600), new Vector2(npc.Center.X - 300, npc.Center.Y), (float)ir / 3);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f + ir * 149, 130);


					}
					for (int ir = 0; ir < 12; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 900, npc.Center.Y), new Vector2(npc.Center.X + 700, npc.Center.Y), (float)ir / 12);

						Projectile.NewProjectile(entitySource, positionNew, Vector2.Zero, type, damage, 0f, Main.myPlayer, 90 + ir * 84, 130);


					}




				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -200;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		public static void DeathInFourActs1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.DeathInFourActs");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_DeathInFourActs, npc.Center);
				SoundEngine.PlaySound(StarsAboveAudio.SFX_DeathInFourActsReload, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiDeathInFourActs>(), 0, 0f, Main.myPlayer, 180);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiWings>(), 0, 0f, Main.myPlayer, 180);
				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void HypertunedDeathInFourActs(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.HypertunedDeathInFourActs");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.Tsukiyomi_DeathInFourActs, npc.Center);
				SoundEngine.PlaySound(StarsAboveAudio.SFX_DeathInFourActsReload, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiDeathInFourActs2>(), 0, 0f, Main.myPlayer, 180);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileType<TsukiWings>(), 0, 0f, Main.myPlayer, 180);
				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		#endregion

		#region Warrior
		public static void TheBitterEnd(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.TheBitterEnd");//The name of the attack.
				npc.ai[3] = 40;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
                    {
                        SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_TheBitterEnd, npc.Center);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 40);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
                    {
                        SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_TheBitterEnd, npc.Center);

                        Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 40);

					}
					//Portal before teleporting.
					//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
					/*
					if (npc.ai[2] % 2 == 0)
					{
						Vector2 moveTo = new Vector2(target.Center.X + 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					else
					{
						Vector2 moveTo = new Vector2(target.Center.X - 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					*/

					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 10f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 100/2;  //projectile damage
				int type = ProjectileType<TheBitterEndProjectile>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(StarsAboveAudio.SFX_GunbladeImpact, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//float numberProjectiles = 3;
					float adjustedRotation = MathHelper.ToRadians(1);
					Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, 1)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
					Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, velocity.X, velocity.Y, type, damage, 0, Main.myPlayer);


					/*Projectile.NewProjectile(npc.GetSource_FromAI(), 
						StartPosition.X, 
						StartPosition.Y, 
						(float)((Math.Cos(rotation) * Speed) * -1), 
						(float)((Math.Sin(rotation) * Speed) * -1), 
						type, 
						damage, 
						0f, 
						Main.myPlayer);*/
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Coruscant Saber. Randomly picks between get in, and get out.
		public static void CoruscantSaber(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CoruscantSaber");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_BegoneSpawnOfShadow, npc.Center);
				SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int damage = npc.damage/2 + 100/2;
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 180);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 180);

					}
					//if npc has imbue buffs: (no imbued buffs yet)

					//else
					if (Main.rand.NextBool())
                    {
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<CoruscantSaberIn>(), damage, 0, Main.myPlayer, 180);
					}
					else
                    {
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<CoruscantSaberOut>(), damage, 0, Main.myPlayer, 180);
					}

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					/*
					int type = ProjectileType<NalhaunCleave>();
					int damage = npc.damage/2 + 100/2;
					var entitySource = npc.GetSource_FromAI();



					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 300, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);

					for (int i = 0; i < 60; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-38, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 1.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.Flare, 0f + Main.rand.Next(-28, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 3.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-48, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 2.7f);

					}*/

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		//Will always be In
		public static void CoruscantSaberIn(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.CoruscantSaber");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_BegoneSpawnOfShadow, npc.Center);
				SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int damage = npc.damage/2 + 100/2;
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 180);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 180);

					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<CoruscantSaberIn>(), damage, 0, Main.myPlayer, 180);


				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					/*
					int type = ProjectileType<NalhaunCleave>();
					int damage = npc.damage/2 + 100/2;
					var entitySource = npc.GetSource_FromAI();



					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 300, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);

					for (int i = 0; i < 60; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-38, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 1.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.Flare, 0f + Main.rand.Next(-28, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 3.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-48, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 2.7f);

					}*/

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}





		}
		public static void ImbuedSaber(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ImbuedSaber");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 120);
						SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_RadiantBraver, npc.Center);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
					if (Main.rand.NextBool())
					{
						Main.NewText(LangHelper.GetTextValue($"CombatText.WarriorOfLight.ImbueFire"), 241, 255, 180);
						//The boss is imbued with Fire
						npc.AddBuff(BuffType<WarriorFireImbueBuff>(), 3600);
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<ImbuedSaberEffect>(), 0, 0, Main.myPlayer, 120, 0);
					}
					else
					{
						Main.NewText(LangHelper.GetTextValue($"CombatText.WarriorOfLight.ImbueIce"), 241, 255, 180);
						//The boss is imbued with Ice
						npc.AddBuff(BuffType<WarriorIceImbueBuff>(), 3600);
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<ImbuedSaberEffect>(), 0, 0, Main.myPlayer, 120, 1);
					}

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}

		}
		//Coruscant Saber. Randomly picks between get in, and get out. Depending on the buff the boss has, the attack has an additional effect.
		public static void ImbuedCoruscance(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ImbuedCoruscance");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, npc.Center);
				Main.NewText(LangHelper.GetTextValue($"CombatText.WarriorOfLight.UseImbue"), 241, 255, 180);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int damage = npc.damage/2 + 100/2;
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 180);
						SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_BegoneSpawnOfShadow, npc.Center);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_RadiantBraver, npc.Center);

						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 180);

					}

					//else
					if (Main.rand.NextBool())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<CoruscantSaberIn>(), damage, 0, Main.myPlayer, 180);
					}
					else
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<CoruscantSaberOut>(), damage, 0, Main.myPlayer, 180);
					}

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_Fools, npc.Center);

				#region attack
				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSpearSprite>(), 30, 0, Main.myPlayer);


				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					//if npc has imbue buffs:
					

					if (npc.HasBuff(BuffType<WarriorIceImbueBuff>()))
                    {
						int index = npc.FindBuffIndex(BuffType<WarriorIceImbueBuff>());
						if (index >= 0)
							npc.DelBuff(index);
						float Speed = 8f;  //projectile speed
						Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
						int damage = npc.damage/2 + 65/2;  //projectile damage
						int type = ProjectileType<WarriorIcebolt>(); //Type of projectile

						float rotation = MathHelper.ToRadians(45);
						Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

						SoundEngine.PlaySound(SoundID.Item43, npc.Center);

						float numberProjectiles = 16;
						float adjustedRotation = MathHelper.ToRadians(45);

						for (int i = 0; i < numberProjectiles; i++)
						{
							Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * -5, perturbedSpeed.Y * -5, type, damage, 0, Main.myPlayer);
						}
					}
					if (npc.HasBuff(BuffType<WarriorFireImbueBuff>()))
					{
						int index = npc.FindBuffIndex(BuffType<WarriorFireImbueBuff>());
						if (index >= 0)
							npc.DelBuff(index);
						float Speed = 8f;  //projectile speed
						Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
						int damage = npc.damage/2 + 65/2;  //projectile damage
						int type = ProjectileType<WarriorFirebolt>(); //Type of projectile

						float rotation = MathHelper.ToRadians(-45f);
						Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

						SoundEngine.PlaySound(SoundID.Item43, npc.Center);

						float numberProjectiles = 16;
						float adjustedRotation = MathHelper.ToRadians(45);

						for (int i = 0; i < numberProjectiles; i++)
						{
							Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * -5, perturbedSpeed.Y * -5, type, damage, 0, Main.myPlayer);
						}
					}
					
					/*
					int type = ProjectileType<NalhaunCleave>();
					int damage = npc.damage/2 + 100/2;
					var entitySource = npc.GetSource_FromAI();



					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X - 300, npc.Center.Y), Vector2.Zero, type, damage, 0f, Main.myPlayer, 0f, 0f);

					for (int i = 0; i < 60; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-38, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 1.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.Flare, 0f + Main.rand.Next(-28, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 3.7f);

					}
					for (int i = 0; i < 50; i++)
					{
						Dust.NewDust(new Vector2(npc.Center.X - 60, npc.Center.Y), 0, 0, DustID.LifeDrain, 0f + Main.rand.Next(-48, 0), 0f + Main.rand.Next(-18, 18), 150, default(Color), 2.7f);

					}*/

				}
				npc.netUpdate = true;
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}

		}
		//Searing Light- two variatons of a barrage of light projectiles
		public static void SearingLight(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.SearingLight");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 60);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 60);

					}
					//Portal before teleporting.
					//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
					/*
					if (npc.ai[2] % 2 == 0)
					{
						Vector2 moveTo = new Vector2(target.Center.X + 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					else
					{
						Vector2 moveTo = new Vector2(target.Center.X - 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					*/

					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 10f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 100/2;  //projectile damage
				int type = ProjectileType<WarriorLightblast>(); //Type of projectile

				//float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				//Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(StarsAboveAudio.SFX_summoning, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < 1200; i += 100)
					{

						Vector2 vector8 = new Vector2(npc.position.X - 1200 + i, npc.position.Y - 1200);

						float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));

						Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, Main.myPlayer);
					}
					for (int i4 = 0; i4 < 1200; i4 += 100)
					{

						Vector2 vector8 = new Vector2(npc.position.X - 1200, npc.position.Y - 1200 + i4);

						float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));

						Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, Main.myPlayer);
					}
					for (int i2 = 1200; i2 > 0; i2 -= 100)
					{

						Vector2 vector8 = new Vector2(npc.position.X + 1200 - i2, npc.position.Y + 1200);

						float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));

						Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, Main.myPlayer);
					}

					for (int i3 = 1200; i3 > 0; i3 -= 100)
					{

						Vector2 vector8 = new Vector2(npc.position.X + 1200, npc.position.Y + 1200 - i3);

						float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));

						Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, Main.myPlayer);
					}

				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Orbiting light projectiles.
		public static void HopeConfluence(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.HopesConfluence");//The name of the attack.
				npc.ai[3] = 100;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_RefulgentEther, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 100);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 100);

					}
					//Portal before teleporting.
					//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
					/*
					if (npc.ai[2] % 2 == 0)
					{
						Vector2 moveTo = new Vector2(target.Center.X + 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					else
					{
						Vector2 moveTo = new Vector2(target.Center.X - 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					*/

					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 10f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 100/2;  //projectile damage
				int type = ProjectileType<WarriorOrbitingLight>(); //Type of projectile

				//float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				//Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(StarsAboveAudio.SFX_summoning, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					float numberProjectiles = 8;
					for (int i = 0; i < numberProjectiles; i++)
					{
						//For orbiting projectiles = ai[0] is the max orbit distance, ai[1] is the rotation starting position
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 250, i * 45, 1);
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 250, i * 45, -1);

						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 450, i * 45, 1);
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 450, i * 45, -1);

						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 650, i * 45, 1);
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 650, i * 45, -1);

						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 850, i * 45, 1);
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 850, i * 45, -1);
					}
					
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//A line of blades emerge and go up and down.
		public static void EphemeralEdge(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.EphemeralEdge");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                if (npc.type == ModContent.NPCType<WarriorOfLightBoss>() || (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>()))
                    SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_RadiantBraver, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 60);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 60);

					}
					
					//Portal before teleporting.
					//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
					/*
					if (npc.ai[2] % 2 == 0)
					{
						Vector2 moveTo = new Vector2(target.Center.X + 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					else
					{
						Vector2 moveTo = new Vector2(target.Center.X - 400, target.Center.Y - 80);
						npc.Center = moveTo;
					}
					*/

					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				float Speed = 20f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 100/2;  //projectile damage
				int type1 = ProjectileType<WarriorAbsoluteBlade>(); //Type of projectile
				int type2 = ProjectileType<WarriorAbsoluteBlade2>(); //Type of projectile

				//float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				//Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(StarsAboveAudio.SFX_summoning, npc.Center);
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < 2400; i += 100)
					{
						
						Vector2 vector8 = new Vector2(npc.position.X - 1200 + i, npc.position.Y);

						float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));
						SoundEngine.PlaySound(StarsAboveAudio.SFX_swordAttackFinish, vector8);
						 Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type1, damage, 0f, Main.myPlayer);
					}
					for (int i2 = 0; i2 < 2400; i2 += 100)
					{
						
						Vector2 vector8 = new Vector2(npc.position.X - 1200 + i2, npc.position.Y);

						float rotation = (float)Math.Atan2(vector8.Y - (target.position.Y + (target.height * 0.5f)), vector8.X - (target.position.X + (target.width * 0.5f)));
						SoundEngine.PlaySound(StarsAboveAudio.SFX_swordAttackFinish, vector8);
						Projectile.NewProjectile(npc.GetSource_FromAI(), vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type2, damage, 0f, Main.myPlayer);
					}
				}


				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}


		//First Warrior of Light summoning mechanic.
		//Summons vortex to make the bottom half of the arena unusable
		//Summons 3 nebula at the top
		//Summons 1 stardust in the middle
		//Summons two solar on either side
		//Solution: fly up, bait The Bitter End, fly to the left or right
		public static void WarriorSummoning1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AbsoluteSummoning");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 120);
						SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_ToMeWarriorsOfLight, npc.Center);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_NowToTakeYourMeasure, npc.Center);

						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
				}
				for (int i = 0; i < Main.maxNPCs; i++)//The sprite will always face what the boss is facing.
				{
					NPC other = Main.npc[i];

					if (other.active && other.type == ModContent.NPCType<NPCs.WarriorOfLight.WarriorWallsNPC>())
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
						}
						SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
						Vector2 moveTo = new Vector2(other.Center.X, other.Center.Y - 140);
						npc.Center = moveTo;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
						}
						npc.netUpdate = true;//NetUpdate for good measure.
						return;
					}
				}
				

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					//AI 0 is the delay, AI 1 is rotation, AI 2 is the direction
					SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, new Vector2(npc.Center.X, npc.Center.Y));

					int typeSolar = ProjectileType<WarriorSolarSummon>();
					int typeNebula = ProjectileType<WarriorNebulaSummon>();
					int typeVortex = ProjectileType<WarriorVortexSummon>();
					int typeStardust = ProjectileType<WarriorStardustSummon>();

					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();

					
					for(int i = 0; i < 3; i++)
                    {
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 300 + (i * 300), npc.Center.Y - 150), // Spawns here
						Vector2.Zero, typeNebula, damage, 0f, Main.myPlayer, 240 + (i * 20), (i * 30), 0);
					}

					Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X, npc.Center.Y + 100), // Spawns here
						Vector2.Zero, typeStardust, damage, 0f, Main.myPlayer, 240, 0, 0);


					Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X + 300, npc.Center.Y), // Spawns here
						Vector2.Zero, typeSolar, damage, 0f, Main.myPlayer, 240, 40, Main.rand.Next(0, 2));
					Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 300, npc.Center.Y), // Spawns here
						Vector2.Zero, typeSolar, damage, 0f, Main.myPlayer, 240, 40, Main.rand.Next(0,2));

					for (int i = 0; i < 4; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 880, npc.Center.Y + 250 + (i * 100)), // Spawns here
						Vector2.Zero, typeVortex, damage, 0f, Main.myPlayer, 240 + (i * 5), (i * 5), 0);
					}
					for (int i = 0; i < 5; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X + 880, npc.Center.Y + 200 + (i * 100)), // Spawns here
						Vector2.Zero, typeVortex, damage, 0f, Main.myPlayer, 240 + (i * 5), (i * 5), 180);
					}
					Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 880, npc.Center.Y + 20), // Spawns here
						Vector2.Zero, typeVortex, damage, 0f, Main.myPlayer, 240, 60, 0);
					Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X + 880, npc.Center.Y - 20), // Spawns here
						Vector2.Zero, typeVortex, damage, 0f, Main.myPlayer, 240, 0, 180);
				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Second Warrior of Light summoning mechanic
		//Uses vortex to make an impassable X of arrows
		//Solar in each quadrant
		//Nebula in the middle
		public static void WarriorSummoning2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AbsoluteSummoning");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 120);
						SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_ToMeWarriorsOfLight, npc.Center);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_NowToTakeYourMeasure, npc.Center);

						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
				}
				for (int i = 0; i < Main.maxNPCs; i++)//The sprite will always face what the boss is facing.
				{
					NPC other = Main.npc[i];

					if (other.active && other.type == ModContent.NPCType<NPCs.WarriorOfLight.WarriorWallsNPC>())
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
						}
						SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
						Vector2 moveTo = new Vector2(other.Center.X, other.Center.Y - 140);
						npc.Center = moveTo;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
						}
						npc.netUpdate = true;//NetUpdate for good measure.
						return;
					}
				}


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					//AI 0 is the delay, AI 1 is rotation, AI 2 is the direction
					SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, new Vector2(npc.Center.X, npc.Center.Y));

					int typeSolar = ProjectileType<WarriorSolarSummon>();
					int typeNebula = ProjectileType<WarriorNebulaSummon>();
					int typeVortex = ProjectileType<WarriorVortexSummon>();
					int typeStardust = ProjectileType<WarriorStardustSummon>();

					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();


					for (int i = 0; i < 10; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 600 + (i * 130), npc.Center.Y - 300 + i*100), // Spawns here
						Vector2.Zero, typeNebula, damage, 0f, Main.myPlayer, 240 + (i * 10), (i * 30), 0);
					}

					
					
					for (int i = 0; i < 4; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 600 + (i * 400), npc.Center.Y - 200), // Spawns here
						Vector2.Zero, typeSolar, damage, 0f, Main.myPlayer, 240 + (i * 10), (i * 10), 0);
					}
					for (int i = 0; i < 4; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 600 + (i * 400), npc.Center.Y + 400), // Spawns here
						Vector2.Zero, typeSolar, damage, 0f, Main.myPlayer, 240 + (i * 10), (i * 10), 0);
					}
					/*Projectile.NewProjectile(entitySource,
						//Bottom left
						new Vector2(npc.Center.X - 880, npc.Center.Y + 600), // Spawns here
						Vector2.Zero, typeVortex, damage, 0f, Main.myPlayer, 120, 10, -45);
					Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X + 880, npc.Center.Y - 400), // Spawns here
						Vector2.Zero, typeVortex, damage, 0f, Main.myPlayer, 120, 20, 135);
					Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X + 880, npc.Center.Y + 600), // Spawns here
						Vector2.Zero, typeVortex, damage, 0f, Main.myPlayer, 120, 30, -135);
					//Upper left
					Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 880, npc.Center.Y - 400), // Spawns here
						Vector2.Zero, typeVortex, damage, 0f, Main.myPlayer, 120, 40, 45);*/
				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Third Warrior of Light summoning mechanic
		//Solar on the sides to make them impassable
		//Nebula on the top and bottom to make the player move
		//One vortex shooting downwards to split the middle
		public static void WarriorSummoning3(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AbsoluteSummoning");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 120);
						SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_ToMeWarriorsOfLight, npc.Center);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_NowToTakeYourMeasure, npc.Center);

						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
				}
				for (int i = 0; i < Main.maxNPCs; i++)//The sprite will always face what the boss is facing.
				{
					NPC other = Main.npc[i];

					if (other.active && other.type == ModContent.NPCType<NPCs.WarriorOfLight.WarriorWallsNPC>())
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
						}
						SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
						Vector2 moveTo = new Vector2(other.Center.X, other.Center.Y - 140);
						npc.Center = moveTo;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
						}
						npc.netUpdate = true;//NetUpdate for good measure.
						return;
					}
				}


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					//AI 0 is the delay, AI 1 is rotation, AI 2 is the direction
					SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, new Vector2(npc.Center.X, npc.Center.Y));

					int typeSolar = ProjectileType<WarriorSolarSummon>();
					int typeNebula = ProjectileType<WarriorNebulaSummon>();
					int typeVortex = ProjectileType<WarriorVortexSummon>();
					int typeStardust = ProjectileType<WarriorStardustSummon>();

					int damage = npc.damage/2 + 30/2;
					var entitySource = npc.GetSource_FromAI();


					
					for (int i = 0; i < 10; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 400 + (i * 100), npc.Center.Y - 400), // Spawns here
						Vector2.Zero, typeVortex, damage, 0f, Main.myPlayer, 60 + (i * 20), (i * 30), 90);
					}
					for (int i = 0; i < 4; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X, npc.Center.Y), // Spawns here
						Vector2.Zero, typeVortex, damage, 0f, Main.myPlayer, 60 + (i * 20), (i * 30), 0 + (i * 90));
					}

					
					for (int i = 0; i < 4; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X + 580, npc.Center.Y - 300 + (i * 300)), // Spawns here
						Vector2.Zero, typeSolar, damage, 0f, Main.myPlayer, 240 + (i * 5), (i * 5), 180);
					}
					for (int i = 0; i < 4; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 580, npc.Center.Y - 300 + (i * 300)), // Spawns here
						Vector2.Zero, typeSolar, damage, 0f, Main.myPlayer, 240 + (i * 5), (i * 5), 180);
					}
				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//Tsukiyomi's concentric ring attack.
		public static void PassageOfArms1(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.PassageOfArms");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_GleamingSteelLightMyPath, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 180);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 180);

					}
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				for (int d = 0; d < 60; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{

					float Speed = 6f;  //projectile speed
					Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
					int damage = npc.damage/2 + 60/2;  //projectile damage
					int type = ProjectileType<WarriorExpandingBladesDelay>(); //Type of projectile

					SoundEngine.PlaySound(StarsAboveAudio.SFX_HolyStab, npc.Center);
					

					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < 36; i++)
					{
						if (i < 20 || i > 26)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 200, i * 10);

						}


					}
					for (int i = 0; i < 36; i++)
					{
						if (i < 26 || i > 30)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 100, i * 10);

						}


					}
					for (int i = 0; i < 36; i++)
					{
						if (i < 30 || i > 34)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 1, i * 10);

						}


					}

				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void PassageOfArms2(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.PassageOfArms");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_GleamingSteelLightMyPath, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 180);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 180);

					}
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				for (int d = 0; d < 60; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{

					float Speed = 6f;  //projectile speed
					Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
					int damage = npc.damage/2 + 60/2;  //projectile damage
					int type = ProjectileType<WarriorExpandingBladesDelay>(); //Type of projectile

					SoundEngine.PlaySound(StarsAboveAudio.SFX_HolyStab, npc.Center);
					

					float adjustedRotation = MathHelper.ToRadians(15);
					for (int i = 0; i < 36; i++)
					{
						if (i < 30 || i > 34)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 1, i * 10);

						}


					}
					for (int i = 0; i < 36; i++)
					{
						if (i < 22 || i > 26)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 100, i * 10);

						}


					}
					for (int i = 0; i < 36; i++)
					{
						if (i < 16 || i > 20)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, 200, i * 10);

						}


					}

				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = -60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Phase change.
		public static void Ascendance(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Ascendance");//The name of the attack.
				npc.ai[3] = 360;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_IWillStrikeYouDown, null);
				if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.WarriorOfLight.PhaseChange"), 232, 65, 65); }
				npc.localAI[1] = 1;
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 360);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 360);

					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<fastRadiate>(), 0, 0, Main.myPlayer, 0f);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<TransitionDustEffect>(), 0, 0, Main.myPlayer, 360);

					
					

				}
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    SoundEngine.PlaySound(StarsAboveAudio.SFX_WarriorStun, npc.Center);

                    Player player = Main.player[i];
                    if (player.active && player.Distance(npc.Center) < 1600)
                    {
                        player.AddBuff(BuffType<DownForTheCount>(), 360);
                    }
                }

                return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				SoundEngine.PlaySound(StarsAboveAudio.SFX_LimitBreakActive, npc.Center);

				for (int d = 0; d < 50; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-6, 6), 0, default(Color), 1.5f);
				}
				for (int d = 0; d < 50; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.GemTopaz, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 0, default(Color), 1.5f);
				}
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<radiate>(), 0, 0, Main.myPlayer, 0f);
				Main.LocalPlayer.GetModPlayer<StarsAbovePlayer>().screenShakeTimerGlobal = -70;
				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Binding Light
		public static void BindingLight(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AbsoluteZero");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_ForVictory, null);
				npc.localAI[1] = 1;

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
					

				}


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				for (int d = 0; d < 50; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-6, 6), 0, default(Color), 1.5f);
				}
				for (int d = 0; d < 50; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.GemTopaz, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 0, default(Color), 1.5f);
				}
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					
					for (int i = 0; i < Main.maxPlayers; i++)
					{
						SoundEngine.PlaySound(StarsAboveAudio.SFX_WarriorStun, npc.Center);

						Player player = Main.player[i];
						if (player.active && player.Distance(npc.Center) < 1600)
						{
							player.AddBuff(BuffType<BindingLight>(), 600);
						}
					}
					if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.WarriorOfLight.QTE"), 255, 185, 0); }

					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<radiate>(), 0, 0, Main.myPlayer, 0f);
					Main.LocalPlayer.GetModPlayer<StarsAbovePlayer>().screenShakeTimerGlobal = -70;
				}
				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Fires swords towards the player. After a short delay they come back.
		public static void RadiantReprobation(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.RadiantReprobation");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                if (npc.type == ModContent.NPCType<WarriorOfLightBoss>() || (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>()))
                    SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_DarknessMustBeDestroyed, npc.Center);
				float Speed = 12f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 0/2;  //projectile damage
				int type = ProjectileType<BladeworkIndicator>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.Item43, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

				

				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 120);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 120);

					}
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack
				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}

				float Speed = 12f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 65/2;  //projectile damage
				int type = ProjectileType<WarriorBladeOfLightReturning>(); //Type of projectile

				float rotation = (float)Math.Atan2(StartPosition.Y - (target.position.Y + (target.height * 0.5f)), StartPosition.X - (target.position.X + (target.width * 0.5f)));
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.Item43, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


					float numberProjectiles = 7;
					float adjustedRotation = MathHelper.ToRadians(35);

					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}
					
				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Fires swords in a clock formation from the boss. After a delay they spin around and come back the way they came.
		public static void RefulgentReprobation(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.RefulgentReprobation");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                if (npc.type == ModContent.NPCType<WarriorOfLightBoss>() || (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>()))
                    SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_DarknessMustBeDestroyed, npc.Center);
				if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 120);

				}
				if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 120);

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}

				float Speed = 7f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 65/2;  //projectile damage
				int type = ProjectileType<WarriorBladeOfLightReturning>(); //Type of projectile

				float rotation = 0f;
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.Item43, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


					float numberProjectiles = 8;
					float adjustedRotation = MathHelper.ToRadians(180);

					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}


				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Empowered version of Radiant Reprobation. Clock formation and Warrior summons
		
		public static void ResoluteReprobation(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ResoluteReprobation");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                if (npc.type == ModContent.NPCType<WarriorOfLightBoss>() || (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>()))
                    SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_MySoulKnowsNoSurrender, npc.Center);
                if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 120);

				}
				if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 120);

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn projectile randomly below target, based on horizontal velocity to make kiting harder, starting velocity 1f upwards
					// (The projectiles accelerate from their initial velocity)


					int type = ProjectileType<WarriorBladeOfLightReturningTurret>();
					int damage = npc.damage/2 + 50/2;
					var entitySource = npc.GetSource_FromAI();

					for (int ir = 0; ir < 50; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X, npc.Center.Y - 800), (float)ir / 30);

						Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Yellow, null, 240, default(Color), 1.7f);
						da.fadeIn = 0.3f;
						da.noLight = true;
						da.noGravity = true;

					}
					for (int ir = 0; ir < 50; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(npc.Center, new Vector2(npc.Center.X, npc.Center.Y + 800), (float)ir / 30);

						Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Yellow, null, 240, default(Color), 1.7f);
						da.fadeIn = 0.3f;
						da.noLight = true;
						da.noGravity = true;

					}
					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X , npc.Center.Y - 800), Vector2.Zero, type, damage, 0f, Main.myPlayer);
					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X , npc.Center.Y + 800), Vector2.Zero, type, damage, 0f, Main.myPlayer);



				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Fires flaming projectiles in a X formation.
		public static void AbsoluteFire(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AbsoluteFire");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_MySoulKnowsNoSurrender, npc.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
				}
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}

				float Speed = 3f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 65/2;  //projectile damage
				int type = ProjectileType<WarriorFirebolt>(); //Type of projectile

				float rotation = MathHelper.ToRadians(-45f);
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.Item43, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


					float numberProjectiles = 16;
					float adjustedRotation = MathHelper.ToRadians(45);

					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * -5, perturbedSpeed.Y * -5, type, damage, 0, Main.myPlayer);
					}


				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void AbsoluteIce(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.AbsoluteIce");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_MySoulKnowsNoSurrender, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}

				float Speed = 3f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 65/2;  //projectile damage
				int type = ProjectileType<WarriorIcebolt>(); //Type of projectile

				float rotation = MathHelper.ToRadians(45f);
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.Item43, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


					float numberProjectiles = 16;
					float adjustedRotation = MathHelper.ToRadians(45);

					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * -5, perturbedSpeed.Y * -5, type, damage, 0, Main.myPlayer);
					}


				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Unused
		
		public static void Recenter(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Transplacement");//The name of the attack.
				npc.ai[3] = 40;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."



				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{
				for (int i = 0; i < Main.maxNPCs; i++)//The sprite will always face what the boss is facing.
				{
					NPC other = Main.npc[i];

					if (other.active && other.type == ModContent.NPCType<NPCs.WarriorOfLight.WarriorWallsNPC>())
					{
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
						}
						SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
						Vector2 moveTo = new Vector2(other.Center.X, other.Center.Y - 140);
						npc.Center = moveTo;

						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantWormhole>(), 0, 0, Main.myPlayer);
						}
						npc.netUpdate = true;//NetUpdate for good measure.
					}
				}
				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 100;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}

		//PHASE 3 MECHANICS
		public static void ScionsAndSinners(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ScionsAndSinners");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
                                     //The NPC will recieve the message when this code is run: "Oh, I'm casting."
                                     //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
                if (npc.type == ModContent.NPCType<WarriorOfLightBoss>() || (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>()))
                    SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_TheBitterEnd, npc.Center);

				if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 120);

				}
				if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 120);

				}
				//do projectiles here, invisible with indicator
				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				#region attack

				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}

				float Speed = 22f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 65/2;  //projectile damage
				int type = ProjectileType<TheBitterEndProjectile>(); //Type of projectile

				float rotation = 0f;
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(StarsAboveAudio.SFX_LegendarySlash, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


					float numberProjectiles = 8;
					float adjustedRotation = MathHelper.ToRadians(180);

					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}


				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void ToTheLimit(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);


				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ToTheLimit");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_PowerFromBeyond, null);
				if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.WarriorOfLight.ChargeNova"), 255, 185, 0); }

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}

				}


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{

				SoundEngine.PlaySound(StarsAboveAudio.SFX_LimitBreakCharge, npc.Center);
				Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ProjectileType<fastRadiate>(), 0, 0, Main.myPlayer, 0f);
				for (int d = 0; d < 50; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-6, 6), 0, default(Color), 1.5f);
				}
				for (int d = 0; d < 50; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.GemTopaz, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 0, default(Color), 1.5f);
				}
				Main.LocalPlayer.GetModPlayer<StarsAbovePlayer>().screenShakeTimerGlobal = -90;
				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void WarriorArsLaevateinn(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ArsLaevateinn");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(StarsAboveAudio.SFX_LimitBreakActive, target.Center);

					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 60);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 60);

					}

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
                    if (npc.type == ModContent.NPCType<WarriorOfLightBoss>() || (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>()))
                    {
                        SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_TheBitterEnd, npc.Center);

                    }
					SoundEngine.PlaySound(StarsAboveAudio.SFX_prototokiaActive, target.Center);
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(target.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					for (int ir = 0; ir < 30; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(npc.Center, target.Center, (float)ir / 30);

						Dust da = Dust.NewDustPerfect(positionNew, DustID.FireworkFountain_Red, null, 240, default(Color), 0.7f);
						da.fadeIn = 0.3f;
						da.noLight = true;
						da.noGravity = true;

					}
					Projectile.NewProjectile(npc.GetSource_FromAI(), target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<BossLaevateinn>(), 0, 0, Main.myPlayer, 80);

					
				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void WarriorGardenOfAvalon(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.GardenOfAvalon");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					SoundEngine.PlaySound(StarsAboveAudio.SFX_LimitBreakActive, target.Center);

					if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}
					if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 120);

					}

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_EveryDream, npc.Center);

				SoundEngine.PlaySound(StarsAboveAudio.SFX_GardenOfAvalonActivated, npc.Center);
				if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.WarriorOfLight.GardenOfAvalon"), 255, 185, 0); }

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient && !NPC.AnyNPCs(ModContent.NPCType<GardenOfAvalonNPC>()))
				{
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					NPC minionNPC = NPC.NewNPCDirect(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<GardenOfAvalonNPC>(), npc.whoAmI);
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.SyncNPC, number: minionNPC.whoAmI);
					}

				}
				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		//Upgraded clock returning blades
		public static void MortalInstants(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.MortalInstants");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."
				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_FlamesOfBattle, npc.Center);

				if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightSwingingSprite>(), 0, 0, Main.myPlayer, 120);

				}
				if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseSwingingSprite>(), 0, 0, Main.myPlayer, 120);

				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				for (int d = 0; d < 30; d++)
				{
					Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Yellow, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
				}

				float Speed = 7f;  //projectile speed
				Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);
				int damage = npc.damage/2 + 65/2;  //projectile damage
				int type = ProjectileType<WarriorBladeOfLightReturning>(); //Type of projectile

				float rotation = 0f;
				Vector2 velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));

				SoundEngine.PlaySound(SoundID.Item43, npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, ProjectileType<TsukiMoonlightSwing>(), 0, 0, Main.myPlayer);


					float numberProjectiles = 8;
					float adjustedRotation = MathHelper.ToRadians(180);

					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-adjustedRotation, adjustedRotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
						Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, perturbedSpeed.X * 5, perturbedSpeed.Y * 5, type, damage, 0, Main.myPlayer);
					}
					for (int i = 0; i < 10; i++)
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(),
						new Vector2(npc.Center.X - 1000 + (i * 200), npc.Center.Y + 500 - (i * 100)), // Spawns here
						Vector2.Zero, ProjectileType<WarriorNebulaSummon>(), damage, 0f, Main.myPlayer, 240 + (i * 5), (i * 50), 0);
					}
					for (int i = 0; i < 10; i++)
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(),
						new Vector2(npc.Center.X - 1000 + (i * 200), npc.Center.Y - 400), // Spawns here
						Vector2.Zero, ProjectileType<WarriorVortexSummon>(), damage, 0f, Main.myPlayer, 240 + (i * 20), (i * 30), 90);
					}
				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 60;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void CosmicIgnition(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Katakrisis");//The name of the attack.
				npc.ai[3] = 120;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.WarriorOfLight.ChargeStarfarer"), 255, 80, 199); }

				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_TheseMagicksAreNotForYou, npc.Center);

				if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 120);

				}
				if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 120);

				}
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{

					if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<SpacePowerBackground>(), 0, 0, Main.myPlayer, 0,0, 360);

						float speed = 4f;
						int type = ProjectileType<BladeworkIndicator>();
						int damage = npc.damage/2 + 0/2;
						var entitySource = npc.GetSource_FromAI();


						for (int ir = 0; ir < 12; ir++)
						{
							Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y - 800), new Vector2(npc.Center.X + 1200, npc.Center.Y - 800), (float)ir / 12);
							Vector2 velocity = new Vector2(-16, speed + ir * 4);

							Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


						}

					}
				}

				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 1f;
					int type = ProjectileType<Projectiles.Bosses.Tsukiyomi.TsukiyomiStar>();
					int damage = npc.damage/2 + 50/2;
					var entitySource = npc.GetSource_FromAI();

					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(npc.Center, 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}

					for (int ir = 0; ir < 12; ir++)
					{
						Vector2 positionNew = Vector2.Lerp(new Vector2(npc.Center.X - 400, npc.Center.Y - 800), new Vector2(npc.Center.X + 1200, npc.Center.Y - 800), (float)ir / 12);
						Vector2 velocity = new Vector2(-4, speed + ir);

						Projectile.NewProjectile(entitySource, positionNew, velocity, type, damage, 0f, Main.myPlayer);


					}
					for (int i = 0; i < 2; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X, npc.Center.Y - 300 + (i * 600)), // Spawns here
						Vector2.Zero, ProjectileType<WarriorStardustSummon>(), damage, 0f, Main.myPlayer, 240 + (i * 5), (i * 5), 180);
					}

					for (int i = 0; i < 4; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X + 580, npc.Center.Y - 300 + (i * 300)), // Spawns here
						Vector2.Zero, ProjectileType<WarriorSolarSummon>(), damage, 0f, Main.myPlayer, 240 + (i * 5), (i * 5), 180);
					}
					for (int i = 0; i < 4; i++)
					{
						Projectile.NewProjectile(entitySource,
						new Vector2(npc.Center.X - 580, npc.Center.Y - 300 + (i * 300)), // Spawns here
						Vector2.Zero, ProjectileType<WarriorSolarSummon>(), damage, 0f, Main.myPlayer, 240 + (i * 5), (i * 5), 180);
					}

				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void ParadiseLost(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.ParadiseLost");//The name of the attack.
				npc.ai[3] = 180;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."

				if (Main.netMode != NetmodeID.Server) { Main.NewText(LangHelper.GetTextValue($"CombatText.WarriorOfLight.ChargeStarfarer"), 255, 80, 199); }

				SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_WillLightEmbrace, npc.Center);

				if (npc.type == ModContent.NPCType<WarriorOfLightBoss>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightCastingSprite>(), 0, 0, Main.myPlayer, 180);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<SpacePowerBackground>(), 0, 0, Main.myPlayer,0,0, 440);

				}
				if (npc.type == ModContent.NPCType<WarriorOfLightBossFinalPhase>())
				{
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<WarriorOfLightFinalPhaseCastingSprite>(), 0, 0, Main.myPlayer, 180);
					Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<SpacePowerBackground>(), 0, 0, Main.myPlayer,0,0, 440);

				}


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				#region attack
				SoundEngine.PlaySound(SoundID.Item124, npc.Center);

				if (npc.HasValidTarget && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float speed = 1f;
					int type = ProjectileType<WarriorOrbitingLight>();
					int damage = npc.damage/2 + 100/2;
					var entitySource = npc.GetSource_FromAI();

					
					Vector2 StartPosition = new Vector2(npc.Center.X, npc.Center.Y);

					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						float numberProjectiles = 8;
						for (int i = 0; i < numberProjectiles; i++)
						{
							//For orbiting projectiles = ai[0] is the max orbit distance, ai[1] is the rotation starting position
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 250, i * 45, 1);
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 250, i * 45, -1);

							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 550, i * 45, 1);
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 550, i * 45, -1);

							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 950, i * 45, 1);
							Projectile.NewProjectile(npc.GetSource_FromAI(), StartPosition.X, StartPosition.Y, 0, 0, type, damage, 0, Main.myPlayer, i + 950, i * 45, -1);

							
						}

					}
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y - 400), 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					for (int d = 0; d < 30; d++)
					{
						Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y + 400), 0, 0, DustID.FireworkFountain_Blue, 0f + Main.rand.Next(-20, 20), 0f + Main.rand.Next(-20, 20), 150, default(Color), 1.5f);
					}
					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X, npc.Center.Y - 400), Vector2.Zero, ProjectileType<Projectiles.Bosses.Tsukiyomi.PlusPlanet>(), damage * 2, 0f, Main.myPlayer);
					Projectile.NewProjectile(entitySource, new Vector2(npc.Center.X, npc.Center.Y + 400), Vector2.Zero, ProjectileType<Projectiles.Bosses.Tsukiyomi.PlusPlanet>(), damage * 2, 0f, Main.myPlayer);
					
				}

				#endregion


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] += 1;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		#endregion
		public static void ResetState(Player target, NPC npc)//
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

			//Each attack in the Library has 3 important segments.
			//Part 1: Name of the attack and cast time. (ActionState.Idle)
			//Part 2: The actual execution of the attack. (ActionState.Casting)
			//Part 3: If the attack lasts longer than the initial attack, execute the active code. (ActionState.PersistentCast)

			//Global attack-specific variables

			if (npc.ai[0] == (float)ActionState.Idle && npc.ai[1] > 0)//If this is the first time the attack is being called.
			{

				//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
				//Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center.X, npc.Center.Y, 0, 0, ModContent.ProjectileType<VagrantBurstSprite>(), 0, 0, Main.myPlayer);

				//CombatText.NewText(textPos, new Color(43, 255, 43, 240), $"{LangHelper.GetTextValue($"BossDialogue.Vagrant.Microcosmos")}", false, false);

				modPlayer.NextAttack = LangHelper.GetTextValue($"BossAttacks.Debug: Reset State");//The name of the attack.
				npc.ai[3] = 60;//This is the time it takes for the cast to finish.
				npc.localAI[3] = 0;//This resets the cast time.
				npc.ai[0] = (float)ActionState.Casting;//The boss is now in a "casting" state, and can run different animations, etc.
				npc.netUpdate = true;//NetUpdate for good measure.
									 //The NPC will recieve the message when this code is run: "Oh, I'm casting."
									 //Then it will think "I'm going to wait the cast time, then ask the Library what to do next."


				return;
			}
			if (npc.ai[0] == (float)ActionState.PersistentCast)//If an attack lasts, it'll be moved to PersistentCast until the cast finishes.
			{

				return;
			}
			if (npc.ai[0] == (float)ActionState.Casting && npc.localAI[3] >= npc.ai[3])//If this attack is called again (which means the cast finished)
			{


				//After the attack ends, we do some cleanup.
				ResetAttack(target, npc);

				npc.ai[0] = (float)ActionState.Idle;//If the attack continues, change ActionState to PersistentCast instead
				modPlayer.NextAttack = "";//Empty the UI text.
				npc.localAI[3] = 0;//Reset the cast.
				npc.ai[1] = 0;//Reset the internal clock before the next attack. Higher values means less of a delay before the next attack.
				npc.ai[2] = 0;//Increment the rotation counter.
				npc.netUpdate = true;//NetUpdate for good measure.

				return;
			}
		}
		public static void ResetAttack(Player target, NPC npc)
        {
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();
			
			
			modPlayer.CastTime = 0;
			modPlayer.NextAttack = "";

		}
		public static void ResetAttackAlt(Player target, NPC npc)
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();


			modPlayer.CastTimeAlt = 0;
			modPlayer.NextAttackAlt = "";

		}
	}
}