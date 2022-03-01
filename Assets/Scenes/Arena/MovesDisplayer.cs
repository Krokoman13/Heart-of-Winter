using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HeartOfWinter.Moves;
using HeartOfWinter.Characters.HeroCharacters;
using System;

namespace HeartOfWinter.Arena
{
    public class MovesDisplayer : MonoBehaviour
    {        
        HeroCharacter myCharacter;
        //[SerializeField] RectTransform ultimatePanel;
        //[SerializeField] Playfield playfield;

        [SerializeField] List<MoveButton> moveButtons;

        public void SetCharacter(HeroCharacter character)
        {
            myCharacter = character;

            for (int i = 0; i < myCharacter.knownMoves.Count; i++)
            {
                moveButtons[i].SetMove(myCharacter.knownMoves[i]);
            }


            /*
            float xValue = 0.5f;
            
            Vector2 pos = new Vector2(0, 0.7f);
            Vector2 size = new Vector2(0.35f, 0.6f);
            Vector2 anchor = new Vector2(0, 1);

            for (int i = 0; i < myCharacter.knownMoves.Count - 1; i++)
            {
                pos.x = xValue;
                spawnButton(myCharacter.knownMoves[i], pos, size, transform);
                xValue += 1.2f;
            }

            spawnButton(myCharacter.knownMoves[myCharacter.knownMoves.Count - 1], new Vector2(0.45f, 0.45f), new Vector2(0.75f, 0.75f), ultimatePanel);
            */
        }

        private void spawnButton(Move move, Vector2 pos, Vector2 size, Transform parent)
        {
/*            DefaultControls.Resources uiResources = new DefaultControls.Resources();
            //Set the Button Background Image someBgSprite;
            Texture2D tex = new Texture2D(2, 2);
            var rawData = System.IO.File.ReadAllBytes("Assets/Art/UI/MoveButtons/" + move.iconName + ".png");
            tex.LoadImage(rawData);
            uiResources.standard = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            GameObject uiButton = DefaultControls.CreateButton(uiResources);

            RectTransform rectTransform = uiButton.GetComponent<RectTransform>();
            
            rectTransform.anchorMin = (new Vector2(-0.25f, -0.25f) + pos) * size;
            rectTransform.anchorMax = (new Vector2(0.75f, 0.75f) + pos) * size;

            rectTransform.offsetMin = new Vector2();
            rectTransform.offsetMax = new Vector2();

            //uiButton.GetComponent<Image>().sprite.texture = move.icon;
            uiButton.transform.SetParent(parent, false);

            uiButton.transform.GetChild(0).gameObject.SetActive(false);

            uiButton.GetComponent<Button>().onClick.AddListener(() => playfield.SelectNewMove(move));*/
        }

        internal void DisableMoves()
        {
            foreach (MoveButton moveButton in moveButtons)
            {
                moveButton.gameObject.SetActive(false);
            }
        }
    }
}
