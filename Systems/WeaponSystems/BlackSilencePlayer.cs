using Microsoft.Xna.Framework;
using StarsAbove.Buffs.Celestial.BlackSilence;
using StarsAbove.Items.Armor.BlackSilence;
using StarsAbove.Projectiles.Celestial.BlackSilence;
using StarsAbove.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarsAbove.Systems.WeaponSystems
{
    public class BlackSilencePlayer : ModPlayer
    {

        //Gloves of the Black Silence
        public int blackSilenceWeaponChoice1;
        public int blackSilenceWeaponChoice2;
        public int blackSilenceWeaponChoice3;

        public bool durandalUsed = true;//0. You start with this enabled because you start with Durandal in hand.
        public bool zelkovaUsed;//1
        public bool rangaUsed;//2
        public bool oldBoysUsed;//3
        public bool allasUsed;//4
        public bool mookUsed;//5
        public bool atelierLogicUsed;//6
        public bool crystalAtelierUsed;//7
        public bool wheelsIndustryUsed;//8

        public int chosenWeapon = 0;

        public bool BlackSilenceHeld;
        public bool BlackSilenceUIVisible;

        public float UIAnimateIn;
        public float UIOpacity;

        public bool weaponSwapPrep;
        public bool furiosoReadyPrompt = true;
        public override void PreUpdate()
        {
            if (Player.HasBuff(BuffType<FuriosoBuff>()))
            {
                durandalUsed = false;//0. You start with this enabled because you start with Durandal in hand.
                zelkovaUsed = false;//1
                rangaUsed = false;//2
                oldBoysUsed = false;//3
                allasUsed = false;//4
                mookUsed = false;//5
                atelierLogicUsed = false;//6
                crystalAtelierUsed = false;//7
                wheelsIndustryUsed = false;//8
            }
            if (weaponSwapPrep)
            {
                Player.AddBuff(BuffType<BlackSilenceChoiceCooldown>(), 300);

                weaponSwapPrep = false;
            }

            if (BlackSilenceUIVisible)
            {
                UIOpacity += 0.05f;

                if (UIAnimateIn > 0f)
                {
                    UIAnimateIn -= 2f;
                }
                else
                {
                    UIAnimateIn = 0f;
                }

            }
            else
            {
                UIOpacity = 0f;
                UIAnimateIn = 20f;
            }
        }
        public override void PostUpdateRunSpeeds()
        {
            if (Player.HasBuff(BuffType<CrystalAtelierBuff>()) || Player.HasBuff(BuffType<FuriosoBuff>()))
            {
                Player.thorns = 0.8f;
            }
            if (Player.HasBuff(BuffType<AllasBuff>()) || Player.HasBuff(BuffType<FuriosoBuff>()))
            {
                Player.maxRunSpeed *= 1.3f;
                Player.accRunSpeed *= 1.3f;
            }
            if (Player.HasBuff(BuffType<OldBoysBuff>()) || Player.HasBuff(BuffType<FuriosoBuff>()))
            {
                Player.statDefense += 20;
            }
            if (Player.HasBuff(BuffType<AtelierLogicBuff>()) || Player.HasBuff(BuffType<FuriosoBuff>()))
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 0.2f;//20% incresed Armor Penetration
            }
            if (Player.HasBuff(BuffType<WheelsIndustryBuff>()) || Player.HasBuff(BuffType<FuriosoBuff>()))
            {
                Player.GetDamage(DamageClass.Generic) += 0.1f;
            }
            base.PostUpdateRunSpeeds();
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)/* tModPorter If you don't need the Projectile, consider using ModifyHitNPC instead */
        {

            if (Player.HasBuff(BuffType<MookBuff>()) || Player.HasBuff(BuffType<FuriosoBuff>()))
            {
                target.AddBuff(BuffID.Bleeding, 240);
            }
            if (Player.HasBuff(BuffType<ZelkovaBuff>()) || Player.HasBuff(BuffType<FuriosoBuff>()))
            {
                if (target.life < target.lifeMax / 2)//Less than half HP
                {

                    modifiers.SourceDamage += 1.3f;
                }
            }
            if (Player.HasBuff(BuffType<RangaBuff>()) || Player.HasBuff(BuffType<FuriosoBuff>()))
            {
                modifiers.CritDamage += 1.5f;
            }
            if (Player.HasBuff(BuffType<DurandalBuff>()) || Player.HasBuff(BuffType<FuriosoBuff>()))
            {
                if (target.life < target.lifeMax / 2)
                {
                    modifiers.CritDamage += 1.5f;
                }
            }

        }
        public override void PreUpdateBuffs()
        {
            for (int i = 0; i < Player.CountBuffs(); i++)
                if (Player.buffType[i] == BuffType<FuriosoBuff>())
                {
                    if (Player.buffTime[i] == 1)
                    {//When Furioso runs out...
                        furiosoReadyPrompt = true;
                    }
                }
            base.PreUpdateBuffs();
        }
        public override void PostUpdate()
        {
            if (chosenWeapon == 0)
            {
                durandalUsed = true;
            }
            if (chosenWeapon == 1)
            {
                zelkovaUsed = true;
            }
            if (chosenWeapon == 2)
            {
                rangaUsed = true;
            }
            if (chosenWeapon == 3)
            {
                oldBoysUsed = true;
            }
            if (chosenWeapon == 4)
            {
                allasUsed = true;
            }
            if (chosenWeapon == 5)
            {
                mookUsed = true;
            }
            if (chosenWeapon == 6)
            {
                atelierLogicUsed = true;
            }
            if (chosenWeapon == 7)
            {
                crystalAtelierUsed = true;
            }
            if (chosenWeapon == 8)
            {
                wheelsIndustryUsed = true;
            }
            if (durandalUsed && zelkovaUsed && rangaUsed && oldBoysUsed && allasUsed && mookUsed && atelierLogicUsed && crystalAtelierUsed && wheelsIndustryUsed)
            {//Furioso use condition
                if (furiosoReadyPrompt)
                {
                    Rectangle textPos = new Rectangle((int)Main.LocalPlayer.position.X, (int)Main.LocalPlayer.position.Y - 20, Main.LocalPlayer.width, Main.LocalPlayer.height);
                    CombatText.NewText(textPos, new Color(255, 255, 255, 240), LangHelper.GetTextValue($"UIElements.BlackSilence.FuriosoReady"), false, false);
                    furiosoReadyPrompt = false;
                }

            }
        }
        public override void ResetEffects()
        {
            //When the weapon isn't held, hide the UI.
            if (!BlackSilenceHeld)
            {
                BlackSilenceUIVisible = false;
            }
            BlackSilenceHeld = false;

            base.ResetEffects();
        }
        public override void FrameEffects()
        {
            if (Player.active && !Player.dead)
            {
                if (Player.HasBuff(BuffType<FuriosoBuff>()))
                {

                    Player.head = EquipLoader.GetEquipSlot(Mod, "BlackSilenceHead", EquipType.Head);



                }
                if (BlackSilenceHeld)
                {
                    Player.UpdateVisibleAccessories(new Item(ItemType<BlackSilenceGloves>()), false);

                }


            }





            base.FrameEffects();

        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
        {
            if (BlackSilenceHeld)
            {
                BlackSilenceHitVFX(target);
                if (proj.type == ProjectileType<WheelsIndustrySlamDamage>())
                {//Blunt
                    int randomSound = Main.rand.Next(0, 2);

                    if (randomSound == 0)
                    {
                        SoundEngine.PlaySound(StarsAboveAudio.SFX_BlackSilenceMace, Main.LocalPlayer.Center);

                    }
                    if (randomSound == 1)
                    {
                        SoundEngine.PlaySound(StarsAboveAudio.SFX_BlackSilenceAxe, Main.LocalPlayer.Center);
                    }
                }
                if (proj.type == ProjectileType<RangaDamage>())
                {//Slash

                    SoundEngine.PlaySound(StarsAboveAudio.SFX_BlackSilenceDurandalHit, Main.LocalPlayer.Center);

                }
            }
        }
        public void BlackSilenceHitVFX(NPC target)
        {
            for (int d = 0; d < 18; d++)
            {
                Dust.NewDust(target.Center, 0, 0, DustID.SilverFlame, Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4), 0, default(Color), 0.4f);
                Dust.NewDust(target.Center, 0, 0, DustID.Obsidian, Main.rand.NextFloat(-7, 7), Main.rand.NextFloat(-7, 7), 0, default(Color), 0.5f);
                Dust.NewDust(target.Center, 0, 0, DustID.Smoke, Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5), 0, default(Color), 0.5f);

            }
            // This creates a randomly rotated vector of length 1, which gets it's components multiplied by the parameters
            Vector2 direction = Main.rand.NextVector2CircularEdge(target.width * 0.6f, target.height * 0.6f);
            float distance = 0.3f + Main.rand.NextFloat() * 0.9f;
            Vector2 velocity = new Vector2(0f, -Main.rand.NextFloat() * 0.3f - 1.5f);
            for (int d = 0; d < 6; d++)
            {
                Dust dust = Dust.NewDustPerfect(target.Center + direction * distance, DustID.SilverFlame, velocity);
                dust.scale = 0.5f;
                dust.fadeIn = 1.1f;
                dust.noGravity = true;
                dust.noLight = true;
                //dust.color = Color.Purple;
                dust.alpha = 0;
            }
        }

    }

};