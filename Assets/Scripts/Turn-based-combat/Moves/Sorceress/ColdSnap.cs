using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Sorcceress
{
    public class ColdSnap : StandardAttackMove
    {
        float bonusDamage;

        public ColdSnap(Character caster, float power, float pBonusDamage) : base(caster, power, "Cold Snap", 1)
        {
            setCooldown(1);
            bonusDamage = pBonusDamage;
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                if (target.IsStunned())
                {
                    target.ModifyHealth(-(power + bonusDamage) * caster.damageModifier);
                }
                
                target.ModifyHealth(-power);
            }
        }
    }
}
