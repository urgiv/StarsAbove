using Terraria;
using Terraria.ModLoader;

namespace StarsAbove.Items.Armor.Manifestation

{
    [AutoloadEquip(EquipType.Legs)]
	public class RedMistLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Red Mist Leggings");
			// Tooltip.SetDefault("Unobtainable; vanity by using 'Manifestation'");
        }

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = 1;
			Item.rare = 10;
			Item.vanity = true;
		}
		
	}
}