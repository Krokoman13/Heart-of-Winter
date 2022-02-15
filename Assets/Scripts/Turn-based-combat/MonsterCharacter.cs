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
            currentMove = knownMoves[Random.Range(0, knownMoves.Count - 1)];

            List<Character> targets = new List<Character>();
            List<Character> possibleTargets = null;

            if (currentMove.targetsNPCs)
            {
                possibleTargets = new List<Character>(playfield.NPCs);
            }

            if (currentMove.targetsPCs)
            {
                possibleTargets = new List<Character>(playfield.PCs);
            }

            if (possibleTargets == null) return;

            for (int i = 0; i < currentMove.amountOfTargets; i++)
            {
                Character target = possibleTargets[Random.Range(0, possibleTargets.Count - 1)];
                targets.Add(target);

                if (possibleTargets.Count < 1) break;
                possibleTargets = new List<Character>(possibleTargets.Where(x => x != target));
            }
        }
    }
}
