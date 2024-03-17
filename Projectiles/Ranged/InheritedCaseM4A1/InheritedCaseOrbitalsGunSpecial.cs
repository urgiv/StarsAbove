﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
 
using StarsAbove.Buffs.Ranged.InheritedCaseM4A1;
using StarsAbove.Systems;
using StarsAbove.Systems;
using StarsAbove.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarsAbove.Projectiles.Ranged.InheritedCaseM4A1
{
    /* This class is a held projectile that animates a gun firing.
	 * 
	 * */
    public abstract class InheritedCaseOrbitalsGunSpecial : ModProjectile
	{
		public override string Texture => "StarsAbove/Projectiles/Generics/StarsAboveGun";

        //This allows the gun to set its own muzzle flash.
        public abstract string TextureFlash { get; }

        public abstract bool UseRecoil { get; }
        public abstract int FlashDustID { get; }
        public abstract int SmokeDustID { get; }
        public abstract int MuzzleDistance { get; }
        public abstract int StartingState { get; }//0 is shooting, 1 is recoil, 2 is idle.
        public abstract bool KillOnIdle { get; }
        public abstract int ScreenShakeTime { get; }
        public virtual float ScaleModifier { get; } = 1f;
        public virtual bool ReActivateAfterIdle { get; }
        public virtual int ReActivateAfterIdleTimer { get; } = 20;

        Vector2 MuzzlePosition;


        public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 1;
		}
        public enum ActionState
        {
            //Recoil phase is optional and exists for posterity.
            Shooting,
            Recoil,
            Idle
        }
        public override void SetDefaults()
		{
			AIType = 0;

            //Adjust depending on projectile.
			Projectile.width = 90;
			Projectile.height = 90;

			Projectile.timeLeft = 10;
			Projectile.penetrate = -1;
			Projectile.hide = false;
			Projectile.alpha = 0;
			Projectile.netImportant = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}
        public ref float AI_State => ref Projectile.ai[0];
        public ref float Rotation => ref Projectile.ai[1];

        public abstract float BaseDistance { get; } //This changes depending on the size of the gun.
        
        float distance = 0;

        float shootAnimationProgress;
        float shootAnimationProgressMax = 10f;

        double deg;

        float flashAlpha;

        public override bool PreAI()
        {
            //For later, add a check: if player isn't holding this weapon, kill this projectile.
            Player projOwner = Main.player[Projectile.owner];
            if (projOwner.dead && !projOwner.active)
            {//Disappear when player dies
                Projectile.Kill();
            }
			return true;
		}
		public override void AI()
        {
            Player projOwner = Main.player[Projectile.owner];
            Projectile.scale = 1f;
            //Projectile.timeLeft = 10;

            
            switch (AI_State)
            {
                case (float)ActionState.Shooting:
                    ShootAnimation(projOwner);
                    break;
                case (float)ActionState.Recoil:
                    RecoilAnimation(projOwner);
                    break;
                case (float)ActionState.Idle:
                    Idle(projOwner);
                    break;
            }

            
            OrientSprite(projOwner);
            //projOwner.GetModPlayer<WeaponPlayer>().MuzzlePosition = MuzzlePosition;
        }

        private static float recoilRotationStart = 30f;
        private float recoilRotation = 0f;

        private static float recoilDistance;
        private void ShootAnimation(Player projOwner)
        {
            //Remember this is activating every tick.

            //In order:
            //1. The gun starts pointed up and the muzzle flash appears and quickly fades out along with very transparent dust gore. Screen shakes.
            //1a. The gun additionally moves slightly closer to the player (reducing the distance)
            //2. For the duration of the shoot animation, there are smoke particles being emitted from the muzzle.
            //3. The gun lerps towards its resting position. (downwards and back to its original distance.)
            //Depends on a percentage of the player's itemTime and itemTimeMax. The first half of the useTime is dedicated to the shoot animation.
            if (shootAnimationProgress == 0f)
            {
                if(StartingState == 2)
                {
                    shootAnimationProgress = 1f;
                    Projectile.alpha = 0;
                    flashAlpha = 0;
                    AI_State = (float)ActionState.Idle;
                    //return;
                }
                else
                {
                    flashAlpha = 1f;
                    projOwner.GetModPlayer<StarsAbovePlayer>().screenShakeTimerGlobal = -ScreenShakeTime;

                }
                recoilDistance = 10;
                //If the shoot animation has just begun...
                //Projectile.alpha = 255;
                if(UseRecoil)
                {
                    shootAnimationProgressMax = projOwner.itemTimeMax / 2; //Half of the time spent after using the item is this animation.
                }
                else
                {
                    shootAnimationProgressMax = ReActivateAfterIdleTimer; //90% of the time spent after using this item is this animation.
                }

                int fakeRotation = (int)MathHelper.ToDegrees((float)Math.Atan2(Main.MouseWorld.Y - projOwner.Center.Y, Main.MouseWorld.X - projOwner.Center.X)) - 180;
                //Main.NewText(fakeRotation);
                if(fakeRotation < -235 && fakeRotation > -300)
                {
                    recoilRotationStart = 0;
                }
                else
                {
                    recoilRotationStart = 30f;
                }


            }

            Vector2 playerToMouse = Main.MouseWorld - projOwner.Center;
            playerToMouse = Vector2.Normalize(playerToMouse);

            float rotationX = 0;
            float rotationY = 0;

            distance = BaseDistance;
            
            recoilRotation = MathHelper.Lerp(recoilRotationStart, 0, EaseHelper.InOutQuad(shootAnimationProgress / shootAnimationProgressMax));

            if (Projectile.spriteDirection == 1)
            {
                float rotationOffset = MathHelper.ToRadians(-recoilRotation);
                rotationX = (float)(playerToMouse.X * Math.Cos(rotationOffset) - playerToMouse.Y * Math.Sin(rotationOffset));
                rotationY = (float)(playerToMouse.X * Math.Sin(rotationOffset) + playerToMouse.Y * Math.Cos(rotationOffset));
                MuzzlePosition = Projectile.Center + new Vector2(rotationX, rotationY) * MuzzleDistance;
                Rotation = MathHelper.ToDegrees((float)Math.Atan2(Main.MouseWorld.Y - Projectile.Center.Y, Main.MouseWorld.X - Projectile.Center.X)) - 180 - recoilRotation;
            }
            else
            {
                float rotationOffset = MathHelper.ToRadians(recoilRotation);
                rotationX = (float)(playerToMouse.X * Math.Cos(rotationOffset) - playerToMouse.Y * Math.Sin(rotationOffset));
                rotationY = (float)(playerToMouse.X * Math.Sin(rotationOffset) + playerToMouse.Y * Math.Cos(rotationOffset));
                MuzzlePosition = Projectile.Center + new Vector2(rotationX, rotationY) * MuzzleDistance;
                Rotation = MathHelper.ToDegrees((float)Math.Atan2(Main.MouseWorld.Y - Projectile.Center.Y, Main.MouseWorld.X - Projectile.Center.X)) - 180 + recoilRotation;
            }
            if (shootAnimationProgress == 2)
            {
                SpawnSmoke(projOwner, Projectile.Center + Vector2.Normalize(playerToMouse) * (MuzzleDistance - 14));
            }
            if (flashAlpha <= 0.3)
            {
                //Draw the residual smoke from the barrel.
                Dust d = Dust.NewDustPerfect(MuzzlePosition, SmokeDustID, new Vector2(Main.rand.NextFloat(-0.5f,0.5f), -2), 140, default, 1f);
                d.noGravity = true;
            }
            if (shootAnimationProgress >= shootAnimationProgressMax && shootAnimationProgress != 0f)
            {
                //Return to the idle state. (Alternatively, switch to the recoil state but that's unused)
                AI_State = (float)ActionState.Idle;
            }
            //projOwner.GetModPlayer<WeaponPlayer>().MuzzlePosition = MuzzlePosition;
            flashAlpha -= 0.07f;
            shootAnimationProgress += 1f;
        }

        private void SpawnSmoke(Player projOwner, Vector2 MuzzlePos)
        {//
            for (int g = 0; g < 3; g++)
            {
                Gore goreIndex = Gore.NewGorePerfect(projOwner.GetSource_FromThis(),
                    new Vector2(MuzzlePos.X - 16 + Main.rand.Next(-15,15), MuzzlePos.Y - 16 + Main.rand.Next(-15, 15)),
                    Vector2.Zero, Main.rand.Next(61, 64), 1f);
                goreIndex.scale = 1.5f;
                goreIndex.alpha = 210;
                goreIndex.velocity = Vector2.Normalize(Main.MouseWorld - Projectile.Center);
            }
            for (int d = 0; d < 20; d++)
            {
                Vector2 perturbedSpeed = Vector2.Normalize(Main.MouseWorld - Projectile.Center).RotatedByRandom(MathHelper.ToRadians(6));
                float scale = 22f - (Main.rand.NextFloat() * 21f);
                perturbedSpeed *= scale;
                int dustIndex = Dust.NewDust(MuzzlePos, 0, 0, FlashDustID, perturbedSpeed.X, perturbedSpeed.Y, 150, default(Color), 0.7f);
                Main.dust[dustIndex].noGravity = true;
            }
            for (int d = 0; d < 10; d++)
            {
                Vector2 perturbedSpeed = Vector2.Normalize(Main.MouseWorld - Projectile.Center).RotatedBy(MathHelper.ToRadians(90));
                float scale = 6f - (Main.rand.NextFloat() * 1f);
                perturbedSpeed *= scale;
                int dustIndex = Dust.NewDust(MuzzlePos, 0, 0, FlashDustID, perturbedSpeed.X, perturbedSpeed.Y, 150, default(Color), 0.5f);
                Main.dust[dustIndex].noGravity = true;

            }
            for (int d = 0; d < 10; d++)
            {
                Vector2 perturbedSpeed = Vector2.Normalize(Main.MouseWorld - Projectile.Center).RotatedBy(MathHelper.ToRadians(-90));
                float scale = 6f - (Main.rand.NextFloat() * 1f);
                perturbedSpeed *= scale;
                int dustIndex = Dust.NewDust(MuzzlePos, 0, 0, FlashDustID, perturbedSpeed.X, perturbedSpeed.Y, 150, default(Color), 0.5f);
                Main.dust[dustIndex].noGravity = true;

            }
        }

        private void RecoilAnimation(Player projOwner)
        {
            //Pretty much the reverse of the shoot animation- the gun is lowered a little bit, pulled back again, and then reset to idle position.
        }
        
        private void Idle(Player projOwner)
        {
            if(KillOnIdle)
            {
                Projectile.Kill();
            }
            else if(ReActivateAfterIdle)
            {
                Projectile.localAI[2]++;
                if (Projectile.localAI[2] > ReActivateAfterIdleTimer && projOwner.HasBuff(BuffType<M16A1Counterfire>()))
                {
                    AI_State = (float)ActionState.Shooting;
                    flashAlpha = 1f;
                    Projectile.localAI[2] = 0;
                    Projectile.localAI[1] = 1;//ready to shoot
                    shootAnimationProgress = 0;
                }
            }
            flashAlpha -= 0.07f;
            Rotation = MathHelper.ToDegrees((float)Math.Atan2(Main.MouseWorld.Y - Projectile.Center.Y, Main.MouseWorld.X - Projectile.Center.X)) - 180;
            Vector2 playerToMouse = Main.MouseWorld - Projectile.Center;
            playerToMouse = Vector2.Normalize(playerToMouse);
            MuzzlePosition = Vector2.Normalize(playerToMouse) * (MuzzleDistance);
        }
        private void OrientSprite(Player projOwner)
        {
            Projectile.rotation = Vector2.Normalize(Main.MouseWorld - Projectile.Center).ToRotation() + MathHelper.ToRadians(0f);
            
            if (Projectile.rotation >= MathHelper.ToRadians(-90) && Projectile.rotation <= MathHelper.ToRadians(90))
            {
                Projectile.spriteDirection = 1;
                if (recoilRotation > 1)
                {
                    return;
                }
            }
            else
            {
                Projectile.spriteDirection = 0;
                Projectile.rotation += MathHelper.Pi;
                if (recoilRotation > 1)
                {
                    return;
                }
                
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // This is where we specify which way to flip the sprite. If the projectile is moving to the left, then flip it vertically.
            SpriteEffects spriteEffects = ((Projectile.spriteDirection <= 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

            // Getting texture of projectile
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            //Getting texture of the muzzle flash
            Texture2D textureFlash = (Texture2D)ModContent.Request<Texture2D>(TextureFlash);

            // Get the currently selected frame on the texture.
            Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);
            Rectangle sourceRectangleFlash = textureFlash.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.Size() / 2f;

            // Applying lighting and draw our projectile
            Color drawColor = Projectile.GetAlpha(lightColor);

            Vector2 playerToMouse = Main.MouseWorld - Projectile.Center;

            Main.EntitySpriteDraw(texture,
                   Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + Vector2.Normalize(playerToMouse) * -MathHelper.Lerp(recoilDistance, BaseDistance - 100, EaseHelper.InOutQuad(shootAnimationProgress / shootAnimationProgressMax)),
                   sourceRectangle, lightColor, Projectile.rotation, origin, Projectile.scale * ScaleModifier, spriteEffects, 0);
            Main.EntitySpriteDraw(textureFlash,
               Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
               sourceRectangle, Color.White * flashAlpha, Projectile.rotation, origin, Projectile.scale * ScaleModifier, spriteEffects, 0);

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }
        private void RotateArms(Player projOwner)
        {
            
        }

        public override void OnKill(int timeLeft)
		{
			

		}

	}
}
