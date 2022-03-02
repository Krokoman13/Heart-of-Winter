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
        float movespeed = 15f;

        public StandardAttackMove(Character caster, float power, float maxPower, string iconName, int amountOfTargets = 1, int cooldown = 0) : base(caster, power, maxPower, iconName)
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

            description = "Deals " + power + " damage to " + (amountOfTargets > 2 ? "all" : amountOfTargets.ToString()) + " enemies";
            setCooldown(cooldown);
        }

        public StandardAttackMove(Character caster, float power, string iconName, int amountOfTargets = 1, int cooldown = 0) : this(caster, power, power, iconName, amountOfTargets, cooldown)
        {
        }

        protected override void execute()
        {
            foreach (Character target in targets)
            {
                target.ModifyHealth(-fullPower*caster.damageModifier);
            }

            //caster.transform.localPosition = startPos;
            //startPos = Vector3.zero;
        }

        protected override void step()
        {
            if (ready) return;

            Transform casterBody = caster.transform.GetChild(0);
            SpriteRenderer bodySprite = casterBody.GetComponent<SpriteRenderer>();
            bodySprite.sortingOrder = 10;

            if (Mathf.Abs(targets[targets.Count - 1].transform.position.x - casterBody.position.x) < 0.1f)
            {
                ready = true;
                return;
            }

            float newX = Vector3.MoveTowards(casterBody.position, targets[targets.Count - 1].transform.position, movespeed * Time.deltaTime).x;
            casterBody.position = new Vector3(newX, casterBody.position.y, casterBody.position.z);
        }
    }
}
