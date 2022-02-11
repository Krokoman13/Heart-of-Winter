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
        [SerializeField]
        Playfield playfield;

        [SerializeField]
        Transform heroParent;

        void Start()
        {
            Spawn(PlayerInfo.character.ToString());
        }

        [PunRPC]
        void Spawn(string heroPrefab)
        {
            if (!PhotonNetwork.IsConnected)
            {
                spawn(heroPrefab);
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(spawn), RpcTarget.AllBuffered, heroPrefab);
                return;
            }

            photonView.RPC(nameof(Spawn), RpcTarget.MasterClient, heroPrefab);
        }

        [PunRPC]
        void spawn(string heroPrefabName)
        {
            GameObject copyOfHero;

            if (!PhotonNetwork.IsConnected)
            {
                copyOfHero = Instantiate<GameObject>(Resources.Load<GameObject>(heroPrefabName), heroParent);
            }
            else
            {
                copyOfHero = Instantiate<GameObject>(Resources.Load<GameObject>(heroPrefabName + " Variant"), heroParent);
            }

            HeroCharacter hero = copyOfHero.GetComponent<HeroCharacter>();
            playfield.AddPC(hero);

            if (hero.heroName == PlayerInfo.character)
            {
                hero.setMine();
            }
        }
    }
}
