using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;
using HeartOfWinter.Characters.MonsterCharacters;

namespace HeartOfWinter.Moves.Shaman
{
    public class StormShield : Move
    {
        public StormShield(Character caster) : base(caster, 2, 5, "Storm Shield")
        {
            targetsPCs = true;
            _amountOfTargets = 1;
            setCooldown(1);
        }

        public override string description
        {
            get
            {
                return "Shields one ally for " + _power + "-" + _maxPower;
            }
        }

        protected override void execute()
        {
            foreach (Character character in targets)
            {
                character.Shield(fullPower);
            }
        }
    }
}
