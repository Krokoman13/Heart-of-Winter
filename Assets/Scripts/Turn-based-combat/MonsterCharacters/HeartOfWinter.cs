using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Moves;
using HeartOfWinter.Moves.Heart;

namespace HeartOfWinter.Characters.MonsterCharacters
{
    public class HeartOfWinter : MonsterCharacter
    {
        public HeartOfWinter()
        {
            maxHealth = 70;
            initiative = 0;

            knownMoves = new List<Move>()
            {
                new StealHealth(this),
                new StealPower(this),
                new ExtremeBlizzard(this),
                new StandardAttackMove(this, 7, "Ice Lance")
            };
        }

        public override void SelectRandomMove()
        {
            _currentMove = pickMove();

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

        private Move pickMove()
        {
            if (health < 20f && !knownMoves[0].IsOnCooldown())
            {
                return knownMoves[0];   //StealHealth
            }

            if (damageModifier > 1f && !knownMoves[2].IsOnCooldown())
            {
                return knownMoves[2];   //ExtremeBlizzard
            }

            if (damageModifier < 1f && !knownMoves[1].IsOnCooldown())
            {
                return knownMoves[1];   //StealPower
            }

            if (health < maxHealth - 10 && !knownMoves[0].IsOnCooldown())
            {
                return knownMoves[0];   //StealHealth
            }

            if (!knownMoves[1].IsOnCooldown() && !knownMoves[1].IsOnCooldown())
            {
                return knownMoves[1];   //StealPower
            }

            if (!knownMoves[2].IsOnCooldown())
            {
                return knownMoves[2];   //ExtremeBlizzard
            }

            return knownMoves[3];   //Ice Lance
        }
    }
}
