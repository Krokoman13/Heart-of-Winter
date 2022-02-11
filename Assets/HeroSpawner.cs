using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.PlayerInformation;
using HeartOfWinter.Characters.HeroCharacters;

namespace HeartOfWinter
{
    public class HeroSpawner : MonoBehaviour
    {
        void Start()
        {
            switch (PlayerInfo.character)
            {
                case Hero.Sorceress:
                    break;

                case Hero.Priest:
                    break;

                case Hero.ShadowWeaver:
                    break;

                case Hero.Other:
                    break;

                default:
                    Debug.LogError("INVALID Hero Entered");
                    break;
            }

    }
}
