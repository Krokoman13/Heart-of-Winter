using System.Collections;
using System.IO;
using System.Collections.Generic;

using HeartOfWinter.Arena;
using HeartOfWinter.Characters;

using UnityEngine;

namespace HeartOfWinter.Moves
{
    abstract public class Move
    {
        Character caster;
        protected List<Character> targets;

        public string iconName;
        public string description;

        protected float power;

        protected int _amountOfTargets;

        public int amountOfTargets
        {
            get { return _amountOfTargets; }
        }

/*        public Move(Character pCaster, float pPower, Texture2D pIcon)
        {
            caster = pCaster;
            power = pPower;
            icon = pIcon;
        }*/

        public Move(Character pCaster, float pPower, string pIconName)
        {
            caster = pCaster;
            power = pPower;
            iconName = pIconName;

        }

        public void SetTargets(IEnumerable<Character> pTargets)
        {
            targets = new List<Character>(pTargets);
        }

        public void Execute()
        {
            execute();
            targets = null;
            caster.currentMove = null;
        }

        abstract protected void execute();

        public override string ToString()
        {
            return base.ToString()+": "+iconName;
        }
    }
}
