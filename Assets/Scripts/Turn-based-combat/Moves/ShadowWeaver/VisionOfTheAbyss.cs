using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;
using HeartOfWinter.Characters.MonsterCharacters;

namespace HeartOfWinter.Moves.ShadowWeaver
{
    public class VisionOfTheAbyss : StandardAttackMove
    {
        public VisionOfTheAbyss(Character caster) : base(caster, 2f, 5f, "Vision of the Abyss", 3, 4)
        {
            setCooldown(4);
        }

        public override string description
        {
            get
            {
                return "Deals " + _power + "-" + _maxPower + " damage to all enemies, also stuns them";
            }
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.SetStunned();
                target.ModifyHealth(-fullPower * caster.damageModifier);
            }
        }
    }
}
