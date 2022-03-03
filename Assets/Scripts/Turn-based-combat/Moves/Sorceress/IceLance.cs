using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Sorcceress
{
    public class IceLance : StandardAttackMove
    {
        public IceLance(Character caster) : base(caster, 7, 12, "Ice Lance", 1)
        {
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.ModifyHealth(-fullPower);
                target.initiative -= 2;

                if (target.initiative < 2) target.initiative = 2;
            }
        }

        public override string description
        {
            get
            {
                return "Deals " + _power + "-" + _maxPower + " damage to one enemy, also lowers the targets initiative";
            }
        }
    }
}
