using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;

namespace HeartOfWinter.Characters.HeroCharacters
{
    public enum Hero { Sorceress = 0, Priest = 1, ShadowWeaver = 2, Shaman = 3, NULL = default };

    public class HeroCharacter : Character
    {
        public bool isMine = false;

        public Hero heroName;

        public void setMine()
        {
            isMine = true;
        }

        protected override Transform findParent()
        {
            return GameObject.FindGameObjectWithTag("HeroParent").transform;
        }

        public override void SelectRandomMove()
        {
            SetMove(0);

            List<Character> targets;
            List<Character> possibleTargets = null;

            if (_currentMove.targetsNPCs)
            {
                possibleTargets = playfield.NPCs;
            }

            if (_currentMove.targetsPCs)
            {
                possibleTargets = playfield.PCs;
            }

            if (possibleTargets == null) return;

            if (_currentMove.amountOfTargets < 3)
            {
                targets = new List<Character>();

                Character target = possibleTargets[Random.Range(0, possibleTargets.Count)];
                targets.Add(target);
            }
            else
            {
                targets = possibleTargets;
            }

            AddTargetsToCurrentMove(targets);
        }

        public float playfieldShakeModifier
        {
            get { return playfield.shakeModifier; }
        }
    }
}
