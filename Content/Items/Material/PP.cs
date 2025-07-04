using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArknightsMod.Content.Items.Material
{
	public class PP : ModItem
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Polyketon"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			// Tooltip.SetDefault("A small amount of industrial organic compound."); // The (English) text shown below your item's name
			Item.ResearchUnlockCount = 100; // How many items are needed in order to research duplication of this item in Journey mode. See https://terraria.gamepedia.com/Journey_Mode/Research_list for a list of commonly used research amounts depending on item type.
		}

		public override void SetDefaults() {
			Item.width = 38; // The item texture's width
			Item.height = 42; // The item texture's height

			Item.maxStack = 9999; // The item's max stack value
			Item.value = Item.sellPrice(0, 0, 6, 00); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient<OrirockConcentration>(1)
				.AddIngredient<OrironBlock>(1)
				.AddIngredient<KetonColloid>(1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}
