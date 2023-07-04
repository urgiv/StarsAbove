using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using StarsAbove.Items.Materials;
using StarsAbove.Items.Prisms;
using Terraria.ID;
using Terraria.GameContent.Creative;
using StarsAbove.Systems;

namespace StarsAbove.Items.Vanity.BaselessBlade
{
    [AutoloadEquip(EquipType.Body)]
	public class BaselessBladeBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Stargazing Hero's Mantle");
			// Tooltip.SetDefault("Spatial garb of ages past");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = 1;
			Item.rare = ModContent.GetInstance<StellarSpoilsRarity>().Type; // Custom Rarity
			Item.vanity = true;
		}
		
		public override void AddRecipes()
		{
			
		}
	}
	
}