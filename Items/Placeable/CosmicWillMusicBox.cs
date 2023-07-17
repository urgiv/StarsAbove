using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using StarsAbove.Items.Materials;
using StarsAbove.Systems;

namespace StarsAbove.Items.Placeable
{
	public class CosmicWillMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Music Box (The Vagrant of Space and Time)");
			/* Tooltip.SetDefault("" +
				"'Cosmic Will' - The Stars Above OST" +
				"\nOriginal"
				+ $"\n"); */
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

			MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Sounds/Music/CosmicWill"), ModContent.ItemType<CosmicWillMusicBox>(), ModContent.TileType<Tiles.CosmicWillMusicBox>());

		}

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("CosmicWillMusicBox").Type;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ModContent.GetInstance<StellarSpoilsRarity>().Type; // Custom Rarity
			Item.value = 1;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ModContent.ItemType<Materials.StellarRemnant>(), 20)
				.AddCustomShimmerResult(ModContent.ItemType<Materials.StellarRemnant>(), 3)
				.AddTile(Terraria.ID.TileID.Anvils)
				.Register();
		}
	}

}
