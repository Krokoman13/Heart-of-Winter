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
        protected Character caster;
        protected List<Character> targets;

        public string iconName;
        public string description;

        protected float power;

        protected int _amountOfTargets;

        public int amountOfTargets
        {
            get { return _amountOfTargets; }
        }
        public bool targetsNPCs = false;
        public bool targetsPCs = false;

        public bool ready = false;

        private int _cooldownTimer = 0;
        private int _cooldownSpend = 1;

        protected void SetCooldown(int cooldownTime)
        {
            _cooldownTimer = cooldownTime;
            _cooldownSpend = cooldownTime + 1;
        }

        public bool IsOnCooldown()
        {
            if (_cooldownTimer < 1) return false;
            return _cooldownSpend <= _cooldownTimer;
        }

        public void CooldownTimerTick()
        {
            _cooldownSpend++;
        }

        public Move(Character pCaster, float pPower, string pIconName)
        {
            caster = pCaster;
            power = pPower;
            iconName = pIconName;
        }

        public void AddTarget(Character target)
        {
            if (targets == null) targets = new List<Character>();

            targets.Add(target);
        }

        public void SetTargets(IEnumerable<Character> pTargets)
        {
            targets = new List<Character>(pTargets);
        }

        public void Execute()
        {
            execute();
            targets = null;
            caster.SetMove(null);
            ready = false;
            _cooldownSpend = 0;
        }

        public void Step()
        {
            if (ready) return;
            step();
        }

        protected virtual void step()
        {
            ready = true;
        }

        abstract protected void execute();

        public override string ToString()
        {
            return base.ToString()+": "+iconName;
        }
    }
}
