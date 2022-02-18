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
        float movespeed = 0.03f;
        Vector3 startPos = Vector3.zero;

        public StandardHealMove(Character caster, float power, string iconName, int amountOfTargets = 1) : base(caster, power, iconName)
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

            description = "Heals " + power + " health to " + (amountOfTargets > 2 ? "all" : amountOfTargets.ToString()) + " allies";
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.ModifyHealth(+power);
            }

            //caster.transform.localPosition = startPos;
            //startPos = Vector3.zero;
        }

        protected override void step()
        {
            if (ready) return;

            if (startPos == Vector3.zero)
            {
                startPos = caster.transform.localPosition;
            }

            if (Vector3.Distance(targets[targets.Count - 1].transform.position, caster.transform.position) < 0.1f)
            {
                caster.transform.localPosition = startPos;
                startPos = Vector3.zero;
                ready = true;
                return;
            }

            caster.transform.position = Vector3.MoveTowards(caster.transform.position, targets[targets.Count - 1].transform.position, movespeed);
        }
    }
}
