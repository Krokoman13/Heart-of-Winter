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
            _currentMove = knownMoves[Random.Range(0, knownMoves.Count)];

            List<Character> targets = new List<Character>();
            List<Character> possibleTargets = null;

            if (_currentMove.targetsNPCs)
            {
                possibleTargets = new List<Character>(playfield.NPCs);
            }

            if (_currentMove.targetsPCs)
            {
                possibleTargets = new List<Character>(playfield.PCs);
            }

            if (possibleTargets == null) return;

            for (int i = 0; i < _currentMove.amountOfTargets; i++)
            {
                if (possibleTargets.Count < 1) break;

                Character target = possibleTargets[Random.Range(0, possibleTargets.Count)];
                targets.Add(target);

                if (possibleTargets.Count < 1) break;
                possibleTargets = new List<Character>(possibleTargets.Where(x => x != target));
            }

            _currentMove.SetTargets(targets);
        }

        protected override Transform findParent()
        {
            return GameObject.FindGameObjectWithTag("MonsterParent").transform;
        }
    }
}
