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

        [SerializeField] List<Sprite> moveSprites;

        [SerializeField] RectTransform descriptionpannel;
        Text descriptionText;

        // Start is called before the first frame update
        void Awake()
        {
            _myButton = GetComponent<Button>();
            _myImage = GetComponent<Image>();
        }

        private void Start()
        {
            descriptionText = descriptionpannel.GetChild(0).GetComponent<Text>();
        }

        public void SetMove(Move pMove)
        {
            _myMove = pMove;
            _myButton.onClick.AddListener(() => playfield.SelectNewMove(_myMove));
            
            foreach (Sprite sprite in moveSprites)
            {
                if (sprite.name == _myMove.iconName)
                {
                    _myImage.sprite = sprite;
                    return;
                }
            }
            
            Debug.LogWarning("Could not find moveIcon: " + _myMove.iconName);
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

        public void OnClick()
        {
            if (descriptionpannel.gameObject.activeSelf && descriptionText.text == moveDescription())
            {
                descriptionpannel.gameObject.SetActive(false);
                return;
            }

            descriptionpannel.gameObject.SetActive(true);
            descriptionText.text = moveDescription();
        }

        string moveDescription()
        {
            string outp = "UNKNOWN";

            if (_myMove == null) return outp;

            outp = _myMove.iconName + '\n';
            outp += '\n';
            outp += _myMove.description;

            return outp;
        }
    }
}
