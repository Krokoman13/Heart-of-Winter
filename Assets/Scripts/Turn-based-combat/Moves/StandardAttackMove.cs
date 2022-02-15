using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;
using HeartOfWinter.Characters.HeroCharacters;
using HeartOfWinter.Characters.MonsterCharacters;

namespace HeartOfWinter.Moves
{
    public class StandardAttackMove : Move
    {
        public StandardAttackMove(Character caster, float power, string iconName, int amountOfTargets = 1) : base(caster, power, iconName)
        {
            _amountOfTargets = amountOfTargets;

            if (caster is HeroCharacter)
            {
                targetsNPCs = true;
                return;
            }

            if (caster is MonsterCharacter)
            {
                targetsPCs = true;
            }
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.RemoveHealth(power);
            }
        }
    }
}
