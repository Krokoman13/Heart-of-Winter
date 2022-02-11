using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeartOfWinter.Heroselection
{
    public class PlayerCard : MonoBehaviour
    {
        Text playerText;
        Text characterText;

        string _player;

        public string player
        {
            get { return _player; }
            private set { _player = value; }
        }

        public void SetPlayer(string player, string character = null)
        {
            if (player == null)
            {
                playerText.text = "...";
                characterText.text = "...";
                gameObject.SetActive(false);
                return;
            }

            playerText.text = player;
            gameObject.SetActive(true);
            SetCharacter(character);
        }

        public void SetCharacter(string character)
        {
            if (character == null)
            {
                characterText.text = "..";
                return;
            }

            characterText.text = character;
        }

        public bool IsPlayer(string player)
        {
            return playerText.text == player;
        }

        // Start is called before the first frame update
        void Awake()
        {
            playerText = transform.GetChild(0).GetComponent<Text>();
            characterText = transform.GetChild(1).GetComponent<Text>();

            SetPlayer(null);
        }
    }
}
