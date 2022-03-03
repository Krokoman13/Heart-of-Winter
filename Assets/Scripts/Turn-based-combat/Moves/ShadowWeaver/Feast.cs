using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;
using HeartOfWinter.Characters.MonsterCharacters;

namespace HeartOfWinter.Moves.ShadowWeaver
{
    public class Feast : StandardAttackMove
    {
        public Feast(Character caster) : base(caster, 5, 10, "Feast")
        {
        }

        public override string description
        {
            get
            {
                return "Deals " + _power + "-" + _maxPower + " damage to one enemy, also heals self if the character is debuffed (by Prepare the Meat).";
            }
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.ModifyHealth(-fullPower * caster.damageModifier);
                if (target.damageModifier < 1.0f) caster.ModifyHealth(4f);
            }
        }
    }
}
