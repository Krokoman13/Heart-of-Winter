using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;
using HeartOfWinter.Characters.HeroCharacters;
using HeartOfWinter.Characters.MonsterCharacters;

namespace HeartOfWinter.Moves
{
    public class StandardHealMove : Move
    {
        public StandardHealMove(Character caster, float power, float maxPower, string iconName, int amountOfTargets = 1, int cooldown = 0) : base(caster, power, maxPower, iconName)
        {
            _amountOfTargets = amountOfTargets;

            if (caster is HeroCharacter)
            {
                targetsPCs = true;
                return;
            }

            if (caster is MonsterCharacter)
            {
                targetsNPCs = true;
            }

            setCooldown(cooldown);
        }

        public StandardHealMove(Character caster, float power, string iconName, int amountOfTargets = 1, int cooldown = 0) : this(caster, power, power, iconName, amountOfTargets, cooldown)
        {

        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.ModifyHealth(+fullPower);
            }

            //caster.transform.localPosition = startPos;
            //startPos = Vector3.zero;
        }
    }
}
