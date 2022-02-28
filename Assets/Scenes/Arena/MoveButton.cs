using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HeartOfWinter.Moves;
using HeartOfWinter.Characters;

namespace HeartOfWinter.Arena
{
    public class MoveButton : MonoBehaviour
    {
        Move _myMove;
        Button _myButton;
        Image _myImage;

        [SerializeField] Playfield playfield;
        [SerializeField] Text text;

        // Start is called before the first frame update
        void Awake()
        {
            _myButton = GetComponent<Button>();
            _myImage = GetComponent<Image>();
        }

        public void SetMove(Move pMove)
        {
            _myMove = pMove;

            Texture2D tex = new Texture2D(2, 2);
            var rawData = System.IO.File.ReadAllBytes("Assets/Art/UI/MoveButtons/" + pMove.iconName + ".png");
            tex.LoadImage(rawData);
            _myImage.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

            _myButton.onClick.AddListener(() => playfield.SelectNewMove(pMove));
        }

        public void Update()
        {
            if (_myMove == null) return;

            if (!_myMove.IsOnCooldown())
            {
                if (!text.gameObject.activeSelf) return;

                _myImage.color = new Color(1f, 1f, 1f, 1f);
                text.gameObject.SetActive(false);
                return;
            }

            text.text = _myMove.CooldownLeft().ToString();

            if (!text.gameObject.activeSelf)
            {
                _myImage.color = new Color(0.25f, 0.25f, 0.25f);
                text.gameObject.SetActive(true);
            }
        }
    }
}
