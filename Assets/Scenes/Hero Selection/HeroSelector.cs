using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

using HeartOfWinter.PlayerInformation;
using HeartOfWinter.Characters.HeroCharacters;

namespace HeartOfWinter.Heroselection
{ 
    public class HeroSelector : MonoBehaviourPun
    {
        [SerializeField]
        PlayerMngr playerMngr;

        [SerializeField]
        GameObject characterButtonsParent;
        List<Button> buttons = new List<Button>();

        int selectedIt = -1;

        [SerializeField] Button continueButton;

        private void Start()
        {
            continueButton.gameObject.SetActive(false);

            for (int i = 0; i < characterButtonsParent.transform.childCount; i++)
            {
                Transform child = characterButtonsParent.transform.GetChild(i);

                Button button = child.GetComponent<Button>();

                if (button == null) continue;

                int value = i;

                //EnableButton(button);

                buttons.Add(button);
                button.onClick.AddListener(() => ButtonClick(value));
            }
        }

        [PunRPC]
        public void HeroSelected(int playerID, int buttonIt)
        {
            if (!PhotonNetwork.IsConnected)
            {
                heroSelected(playerID, buttonIt);
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(heroSelected), RpcTarget.AllBuffered, playerID, buttonIt);
                if (playerMngr.AllPlayersHaveSelected() /*&& playerMngr.playerCards.Count == 3*/) activateContinueButton();
                return;
            }

            photonView.RPC(nameof(HeroSelected), RpcTarget.MasterClient, playerID, buttonIt);
        }

        [PunRPC]
        private void heroSelected(int playerID, int buttonIt)
        {
            Button button = buttons[buttonIt];

            if (playerID == PlayerInfo.ID)
            {
                if (selectedIt >= 0)
                {
                    EnableButton(selectedIt);
                }

                PlayerInfo.character = (Hero)buttonIt;

                selectedIt = buttonIt;
            }

            playerMngr.SetPlayerCharacter(playerID, buttonIt);
            disableButton(button);
        }

        public void ButtonClick(int buttonIt)
        {
            HeroSelected(PlayerInfo.ID, buttonIt);
        }

        private void disableButton(Button button)
        {
            button.interactable = false;

            Image image = button.gameObject.GetComponent<Image>();
            image.color = new Color(123, 123, 123);
        }

        [PunRPC]
        public void EnableButton(int buttonIt)
        {
            enableButton(buttons[buttonIt]);

            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC(nameof(enableButton), RpcTarget.OthersBuffered, buttonIt);
            }
        }

        [PunRPC]
        private void enableButton(int buttonIt)
        {
            Button button = buttons[buttonIt];
            enableButton(button);
        }

        private static void enableButton(Button button)
        {
            button.interactable = true;

            Image image = button.gameObject.GetComponent<Image>();
            image.color = new Color(123, 123, 123);
        }

        private void activateContinueButton()
        {
            continueButton.gameObject.SetActive(true);
            continueButton.onClick.AddListener(() => SceneManager.LoadScene(2));
        }
    }
}
