﻿
using Microsoft.Xna.Framework;
 
using StarsAbove.Projectiles.Generics;
using StarsAbove.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarsAbove.Projectiles.Melee.RebellionBloodArthur
{
    public class RebellionSwordSpin : StarsAboveSword
    {
        public override string Texture => "StarsAbove/Projectiles/Melee/RebellionBloodArthur/RebellionSwordSpin";
        public override bool UseRecoil => false;
        public override bool DoSpin => true;
        public override float BaseDistance => 90;
        public override Color BackDarkColor => new Color(203, 30, 30);
        public override Color MiddleMediumColor => new Color(223, 68, 68);
        public override Color FrontLightColor => new Color(242, 122, 122);

        public override float EffectScaleAdder => 1.6f;
        public override bool CenterOnPlayer => true;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Melee;

            Projectile.width = 132;
            Projectile.height = 132;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 7;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.X -= 60;
            hitbox.Y -= 60;
            hitbox.Width += 120;
            hitbox.Height += 120;
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            //DrawOriginOffsetY = -6;
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Vanilla has several particles that can easily be used anywhere.
            // The particles from the Particle Orchestra are predefined by vanilla and most can not be customized that much.
            // Use auto complete to see the other ParticleOrchestraType types there are.
            // Here we are spawning the Excalibur particle randomly inside of the target's hitbox.
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.Excalibur,
                new ParticleOrchestraSettings { PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox) },
                Projectile.owner);
            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<WeaponPlayer>().rebellionGauge++;

            // You could also spawn dusts at the enemy position. Here is simple an example:
            // Dust.NewDust(Main.rand.NextVector2FromRectangle(target.Hitbox), 0, 0, ModContent.DustType<Content.Dusts.Sparkle>());
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, Main.rand.Next(-2, 2), Main.rand.Next(-2, 2), ProjectileType<RebellionLightning>(), damageDone / 8, 0, player.whoAmI, Main.rand.Next(0, 360) + 1000f, 1);
            if (Main.rand.NextBool())
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, Main.rand.Next(-2, 2), Main.rand.Next(-2, 2), ProjectileType<RebellionLightning>(), damageDone / 8, 0, player.whoAmI, Main.rand.Next(0, 360) + 1000f, 1);
                if (Main.rand.NextBool())
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center.X, target.Center.Y, Main.rand.Next(-2, 2), Main.rand.Next(-2, 2), ProjectileType<RebellionLightning>(), damageDone / 8, 0, player.whoAmI, Main.rand.Next(0, 360) + 1000f, 1);
                }
            }
            // Set the target's hit direction to away from the player so the knockback is in the correct direction.
            hit.HitDirection = Main.player[Projectile.owner].Center.X < target.Center.X ? 1 : -1;
        }
        public override void OnKill(int timeLeft)
        {


        }

    }
}
