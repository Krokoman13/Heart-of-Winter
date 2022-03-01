using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HeartOfWinter.Characters.MonsterCharacters
{
    public class MonsterCharacter : Character
    {
        public void SelectRandomMove()
        {
            if (IsStunned()) return;

            SetMove(knownMoves[Random.Range(0, knownMoves.Count)]);

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

        protected override Transform findParent()
        {
            return GameObject.FindGameObjectWithTag("MonsterParent").transform;
        }
    }
}
