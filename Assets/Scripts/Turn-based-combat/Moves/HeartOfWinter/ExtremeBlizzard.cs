using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Moves.Heart
{
    public class ExtremeBlizzard : StandardAttackMove
    {
        public ExtremeBlizzard(Character caster) : base(caster, 10f, "ExtremeBlizzard", 3)
        {
            setCooldown(1);
        }

        protected override void execute()
        {
            base.execute();
            caster.ModifyHealth((-fullPower * caster.damageModifier)/2f);
        }

        protected override void step()
        {
            ready = true;
        }
    }
}
