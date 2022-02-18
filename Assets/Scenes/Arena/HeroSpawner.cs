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

        [PunRPC]
        void spawn(string heroPrefabName)
        {
            GameObject copyOfHero;

            if (!PhotonNetwork.IsConnected)
            {
                copyOfHero = Instantiate<GameObject>(Resources.Load<GameObject>(heroPrefabName), heroParent);
                playfield.NeedsToArrange();
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                //copyOfHero = Instantiate<GameObject>(Resources.Load<GameObject>(heroPrefabName + " Variant"), heroParent);
                copyOfHero = PhotonNetwork.Instantiate(heroPrefabName + " Variant", Vector3.zero, Quaternion.Euler(Vector3.zero));
                playfield.NeedsToArrange();
                //copyOfHero.transform.SetParent(heroParent);
            }
            else
            {
                photonView.RPC(nameof(spawn), RpcTarget.MasterClient, heroPrefabName);
            }

            //HeroCharacter hero = copyOfHero.GetComponent<HeroCharacter>();
            
        }
    }
}
