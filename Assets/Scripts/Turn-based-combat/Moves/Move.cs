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

        private float _power;
        
        protected float maxPower;
        protected float shakeModifier
        {
            get
            {
                float powerDiffrence = maxPower - _power;

                float multiplier = 0f;

                if (caster is Characters.HeroCharacters.HeroCharacter)
                {
                    multiplier = ((Characters.HeroCharacters.HeroCharacter)caster).playfieldShakeModifier;
                }

                return powerDiffrence * multiplier;
            }
        }
        protected float fullPower
        {
            get { return _power + shakeModifier; }
        }

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

        protected void setCooldown(int cooldownTime)
        {
            _cooldownTimer = cooldownTime;
            _cooldownSpend = cooldownTime + 1;
        }

        public bool IsOnCooldown()
        {
            if (_cooldownTimer < 1) return false;
            return _cooldownSpend <= _cooldownTimer;
        }

        public int CooldownLeft()
        {
            return _cooldownTimer - _cooldownSpend + 1;
        }

        public void CooldownTimerTick()
        {
            _cooldownSpend++;
        }

        public Move(Character pCaster, float pPower, float pMaxpower, string pIconName)
        {
            caster = pCaster;
            _power = pPower;
            iconName = pIconName;
            maxPower = pMaxpower;
        }

        public Move(Character pCaster, float pPower, string pIconName) : this (pCaster, pPower, pPower, pIconName)
        {
            
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
            caster.ResetBody();
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
