﻿using Terraria;
using Terraria.ModLoader;

namespace StarsAbove.Buffs.Celestial.BrilliantSpectrum
{
    public class SpectrumDashCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {

            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
            Main.persistentBuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {

        }
    }
}
