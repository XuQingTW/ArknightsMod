﻿using ArknightsMod.Common.Items;
using ArknightsMod.Common.Players;
using ArknightsMod.Content.Buffs;
using ArknightsMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArknightsMod.Content.Items.Weapons
{
	public class PozemkaCrossbow : UpgradeWeaponBase
	{
		private static SoundStyle SkillActive1;
		private static SoundStyle PozemkaCrossbowS0;
		private static SoundStyle PozemkaCrossbowS2;
		private static SoundStyle PozemkaCrossbowS3;
		private static SoundStyle PozemkaCrossbowSentry;
		public override void Load() {
			SkillActive1 = new SoundStyle("ArknightsMod/Sounds/SkillActive1") {
				Volume = 0.4f,
				MaxInstances = 4,
			};
			PozemkaCrossbowS0 = new SoundStyle("ArknightsMod/Sounds/PozemkaCrossbowS0") {
				Volume = 0.4f,
				MaxInstances = 4,
			};
			PozemkaCrossbowS2 = new SoundStyle("ArknightsMod/Sounds/PozemkaCrossbowS2") {
				Volume = 0.4f,
				MaxInstances = 4,
			};
			PozemkaCrossbowS3 = new SoundStyle("ArknightsMod/Sounds/PozemkaCrossbowS3") {
				Volume = 0.4f,
				MaxInstances = 4,
			};
			PozemkaCrossbowSentry = new SoundStyle("ArknightsMod/Sounds/PozemkaCrossbowSentry") {
				Volume = 0.4f,
				MaxInstances = 4,
			};
		}
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
		}

		public override void SetDefaults() {
			Item.damage = 100;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 88;
			Item.height = 44;
			Item.scale = 0.7f;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.reuseDelay = 10;
			Item.consumeAmmoOnLastShotOnly = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.channel = true; //Channel so that you can held the weapon [Important]
			Item.knockBack = 5;
			Item.shoot = ModContent.ProjectileType<PozemkaCrossbowProjectile>();
			Item.shootSpeed = 20f;
			Item.useAmmo = AmmoID.Arrow;
			Item.crit = 0; // The percent chance at hitting an enemy with a crit, plus the default amount of 4.


			Item.rare = ItemRarityID.Yellow;
			Item.value = Item.sellPrice(0, 0, 10, 0);

		}
		public override bool AltFunctionUse(Player player) => true;
		public override bool CanUseItem(Player player) {
			var modPlayer = Main.LocalPlayer.GetModPlayer<WeaponPlayer>();
			if (Main.myPlayer == player.whoAmI) {
				if (player.altFunctionUse == 2) {
					if (!modPlayer.SummonMode) {
						// S2
						if (modPlayer.Skill == 1 && modPlayer.StockCount > 0 && !modPlayer.SkillActive) {
							modPlayer.SkillActive = true;
							modPlayer.SkillTimer = 0;
							Item.useTime = 10;

							modPlayer.DelStockCount();

							Item.UseSound = PozemkaCrossbowS2;
							SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
							return true;
						}
						// S3
						if (modPlayer.Skill == 2 && modPlayer.StockCount > 0 && !modPlayer.SkillActive) {
							modPlayer.SkillActive = true;
							modPlayer.SkillTimer = 0;

							modPlayer.DelStockCount();

							Item.UseSound = SkillActive1;
							SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
						}
						else
							return false;
					}
					else {
						modPlayer.SummonMode = false;
						return false;
					}
				}
				else {
					if (!modPlayer.SummonMode) {
						Item.useAnimation = 30;
						Item.useTime = 30;
						Item.crit = 0;
						Item.sentry = false;
						Item.UseSound = PozemkaCrossbowS0;
						SoundEngine.PlaySound(Item.UseSound.Value, player.Center);
						// S1
						if (modPlayer.Skill == 0) {
							if (!modPlayer.SkillActive) {
								modPlayer.OffensiveRecovery();
								if (modPlayer.StockCount == 1) {
									modPlayer.SkillActive = true;
								}
							}
							else {
								Item.crit = 36;
								Item.UseSound = PozemkaCrossbowS0;
							}
						}

						// S3
						if (modPlayer.Skill == 2 && modPlayer.SkillActive) {
							Item.useAnimation = 20;
							Item.useTime = 20;

							Item.UseSound = PozemkaCrossbowS3;
						}
					}
					else {
						Item.sentry = true;
						Item.useTime = 30;

						Item.UseSound = PozemkaCrossbowSentry;
					}
				}
			}
			return base.CanUseItem(player);
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage) {
			var modPlayer = Main.LocalPlayer.GetModPlayer<WeaponPlayer>();
			if (Main.myPlayer == player.whoAmI) {
				//S1
				if (modPlayer.Skill == 0 && modPlayer.SkillActive == true) {
					damage *= 1.6f;
				}
				//S3
				if (modPlayer.Skill == 2 && modPlayer.SkillActive == true) {
					damage *= 2.0f;
				}
			}
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			var modPlayer = Main.LocalPlayer.GetModPlayer<WeaponPlayer>();
			if (!modPlayer.SummonMode) {
				type = ModContent.ProjectileType<PozemkaCrossbowProjectile>();
			}
			else {
				type = ModContent.ProjectileType<PozemkaCrossbowSentry>();
				position = Main.MouseWorld;
				velocity *= 0;
				modPlayer.SummonMode = false;
			}
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			var modPlayer = Main.LocalPlayer.GetModPlayer<WeaponPlayer>();
			if (player.altFunctionUse == 2) {
				if (modPlayer.Skill == 1) {
					damage = (int)Math.Round(damage * 2.3f);
				}
			}
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset() {
			return new Vector2(-21f, -3f);
		}

		// This method lets you adjust position of the gun in the player's hands. Play with these values until it looks good with your graphics.
		//public override Vector2? HoldoutOffset() {
		//	return new Vector2(-2f, -2f);
		//}

		public override void AddRecipes() {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<Material.CEU>(3);
			recipe.AddIngredient<Material.OrirockConcentration>(9);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
