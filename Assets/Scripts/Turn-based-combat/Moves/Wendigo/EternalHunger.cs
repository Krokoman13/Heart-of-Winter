using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;
using HeartOfWinter.Characters.MonsterCharacters;

namespace HeartOfWinter.Moves.Wendigo
{
    public class EternalHunger : StandardAttackMove
    {
        public EternalHunger(Character caster, float power) : base(caster, power, "Eternal Hunger")
        {
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.ModifyHealth(-fullPower * caster.damageModifier);
                caster.ModifyHealth(7f);
            }
        }
    }
}
