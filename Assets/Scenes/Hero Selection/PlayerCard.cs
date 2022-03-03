using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HeartOfWinter.PlayerInformation;
using HeartOfWinter.Characters.HeroCharacters;

namespace HeartOfWinter.Heroselection
{
    public class PlayerCard : MonoBehaviour
    {
        [SerializeField] RectTransform outline;

        Text playerText;
        Text characterText;

        [SerializeField] List<GameObject> heroImages;

        string _player;

        public string player
        {
            get { return _player; }
            private set { _player = value; }
        }

        public void SetPlayer(string player, int character = 5)
        {
            if (player == null)
            {
                playerText.text = "...";
                characterText.text = "...";
                gameObject.SetActive(false);

                

                return;
            }

            if (player == PlayerInfo.name) outline.gameObject.SetActive(true);
            playerText.text = player;
            gameObject.SetActive(true);
            SetCharacter(5);
        }

        public void SetCharacter(int character)
        {
            if (character > 4)
            {
                characterText.text = "..";
                return;
            }

            foreach (GameObject heroImage in heroImages)
            {
                heroImage.SetActive(false);
            }

            heroImages[character].SetActive(true);
            characterText.text = ((Hero)character).ToString();
        }

        public bool HasCharacter()
        {
            return characterText.text != "..";
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
