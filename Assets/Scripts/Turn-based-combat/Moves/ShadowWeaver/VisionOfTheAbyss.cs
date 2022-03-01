using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;
using HeartOfWinter.Characters.MonsterCharacters;

namespace HeartOfWinter.Moves.Priest
{
    public class VisionOfTheAbyss : StandardAttackMove
    {
        public VisionOfTheAbyss(Character caster, float power) : base(caster, power, "Vision of the Abyss", 3, 4)
        {
            setCooldown(4);
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.ModifyHealth(-power * caster.damageModifier);
                target.SetStunned();
            }
        }
    }
}
