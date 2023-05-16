﻿using Terraria;
using Terraria.ModLoader;

namespace StarsAbove.Buffs.ManiacalJustice
{
    public class SlaughteringSlashCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Slaughtering Slash Cooldown");
            // Description.SetDefault("");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            
        }
    }
}
