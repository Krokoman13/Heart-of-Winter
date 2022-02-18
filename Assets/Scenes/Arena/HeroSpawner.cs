using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using HeartOfWinter.PlayerInformation;
using HeartOfWinter.Arena;
using HeartOfWinter.Characters.HeroCharacters;

namespace HeartOfWinter
{
    public class HeroSpawner : MonoBehaviourPun
    {
        Playfield playfield;

        [SerializeField]
        Transform heroParent;

        void Start()
        {
            playfield = GetComponent<Playfield>();
            spawn(PlayerInfo.character.ToString());
        }

        void spawn(string heroPrefabName)
        {
            GameObject copyOfHero;

            if (!PhotonNetwork.IsConnected)
            {
                copyOfHero = Instantiate<GameObject>(Resources.Load<GameObject>(heroPrefabName), heroParent);
            }
            else
            {
                //copyOfHero = Instantiate<GameObject>(Resources.Load<GameObject>(heroPrefabName + " Variant"), heroParent);
                copyOfHero = PhotonNetwork.Instantiate(heroPrefabName + " Variant", Vector3.zero, Quaternion.Euler(Vector3.zero));
                //copyOfHero.transform.SetParent(heroParent);
            }

            //HeroCharacter hero = copyOfHero.GetComponent<HeroCharacter>();
            playfield.NeedsToArrange();
        }
    }
}
