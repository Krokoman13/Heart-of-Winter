using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace HeroSelection
{
    public class PlayerMngr : MonoBehaviourPun
    {
        public List<PlayerCard> playerCards;

        void Awake()
        {
            foreach (Transform child in transform)
            {
                PlayerCard playerCard = child.GetComponent<PlayerCard>();

                if (playerCard) playerCards.Add(playerCard);
            }
        }

        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                AddPlayer(PlayerInfo.name);
                return;
            }

            photonView.RPC(nameof(AddPlayer), RpcTarget.MasterClient, PlayerInfo.name);
        }

/*        private void OnApplicationQuit()
        {
            if (!PhotonNetwork.IsConnected)
            {
                RemovePlayer(PlayerInfo.name);
                return;
            }

            photonView.RPC(nameof(RemovePlayer), RpcTarget.MasterClient, PlayerInfo.name);
        }*/

        [PunRPC]
        public void AddPlayer(string player)
        {
            addPlayer(player);

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(AddPlayer), RpcTarget.OthersBuffered, player);
            }
        }

        public void addPlayer(string player)
        {
            foreach (PlayerCard playerCard in playerCards)
            {
                if (playerCard.name == player)
                {
                    playerCard.SetPlayer(player);
                    return;
                }

                if (playerCard.gameObject.activeSelf) continue;

                playerCard.SetPlayer(player);
                return;
            }

            Debug.LogError("Not possible to add more players!");
        }

        [PunRPC]
        public void RemovePlayer(string player)
        {
            removePlayer(player);

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RemovePlayer), RpcTarget.OthersBuffered, player);
            }
        }

        private void removePlayer(string player)
        {
            foreach (PlayerCard playerCard in playerCards)
            {
                if (playerCard.IsPlayer(player))
                {
                    playerCard.SetPlayer(null);
                }

                return;
            }

            Debug.LogError("Could not remove player: " + player);
        }

        public void SetPlayerCharacter(int playerID, string character)
        {
            if (playerID > 0) playerID--;

            playerCards[playerID].SetCharacter(character);
        }

        public bool AllPlayersHaveSelected()
        {
            foreach (PlayerCard playerCard in playerCards)
            {
                if (!playerCard.HasCharacter())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
