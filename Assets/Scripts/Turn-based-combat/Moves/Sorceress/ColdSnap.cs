using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Sorcceress
{
    public class ColdSnap : StandardAttackMove
    {
        float bonusDamage;

        public ColdSnap(Character caster) : base(caster, 3, 6, "Cold Snap", 1)
        {
            setCooldown(1);
            bonusDamage = 8;
        }

        public override string description
        {
            get
            {
                return "Deals " + _power + "-" + _maxPower + " damage to one enemy, +" +bonusDamage+ " if the character is stunned.";
            }
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                if (target.IsStunned())
                {
                    target.ModifyHealth(-(fullPower + bonusDamage) * caster.damageModifier);
                }
                else target.ModifyHealth(-fullPower * caster.damageModifier);
            }
        }
    }
}
