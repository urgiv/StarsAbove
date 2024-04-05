﻿using Terraria;
using Terraria.ModLoader;

namespace StarsAbove.Buffs.Magic.ParadiseLost
{
    public class BaptisedBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
        }

        public override void Update(Player player, ref int buffIndex)
        {
            
        }
    }
}
