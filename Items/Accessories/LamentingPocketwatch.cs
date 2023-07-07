﻿using Microsoft.Xna.Framework;
using StarsAbove.Projectiles.LamentingPocketwatch;
using StarsAbove.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarsAbove.Items.Accessories
{
    public class LamentingPocketwatch : ModItem
	{
		public override void SetStaticDefaults() {
			
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override void SetDefaults() {
			Item.width = 40;
			Item.height = 40;
			Item.accessory = true;
			Item.value = Item.sellPrice(silver: 30);
			Item.rare = ModContent.GetInstance<StellarSpoilsRarity>().Type; // Custom Rarity
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			
		}

		

		public override void AddRecipes() {
			
		}
	}
	public class PocketwatchModPlayer : ModPlayer
    {
        public bool pocketwatchEquipped;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			//if crit
			if (pocketwatchEquipped)
			{
				modifiers.DamageVariationScale *= 0;

				//Determine if clash win

				//Spawn coin on enemy to signify
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(null, new Vector2(target.Center.X, target.Center.Y - target.height - 30), Vector2.Zero, ProjectileType<LamentClashWin>(), 0, 0, Player.whoAmI);
				}

				//Do effect
			}
		}
        
        public override void ResetEffects()
        {
			pocketwatchEquipped = false;
        }
    }
}
