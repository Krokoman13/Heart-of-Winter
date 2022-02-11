using System.Collections;
using System.Collections.Generic;

using HeartOfWinter.Arena;
using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves
{
    abstract public class Move
    {
        Character caster;

        protected List<Character> targets;

        public Move(Character pCaster)
        {
            caster = pCaster;
        }

        public void SetTargets(Playfield playfield)
        {
            targets = getTargets(playfield);
        }

        abstract protected List<Character> getTargets(Playfield playfield);

        abstract public void Execute();
    }
}
