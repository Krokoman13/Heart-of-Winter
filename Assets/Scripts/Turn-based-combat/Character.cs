using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

using Photon.Pun;

using HeartOfWinter.Arena;
using HeartOfWinter.Moves;
using System;

namespace HeartOfWinter.Characters
{
    public abstract class Character : MonoBehaviourPun
    {
        [SerializeField] AudioClip hit;
        [SerializeField] protected AudioClip attack;
        AudioSource source;
        [SerializeField] AudioMixerGroup MyMixerGroup;

        [SerializeField] GameObject stunnedParticles;
        [SerializeField] GameObject buffedparticles;
        [SerializeField] GameObject deBuffedParticles;

        private HealthBar _healthBar;
        [SerializeField] protected float _health;
        public float maxHealth = 1.0f;

        bool _moving = false;
        bool _shaking = false;
        bool _floating = false;

        PopupScript popupScript;

        public float health
        {
            get { return _health; }
            protected set
            {
                if (value < 0)
                {
                    health = 0;
                    return;
                }

                else if (value > maxHealth)
                {
                    health = maxHealth;
                    return;
                }

                _healthBar.SetValue(value , maxHealth);
                _health = value;
            }
        }

        public bool isDead
        {
            get { return health < 0.5f; }
        }

        [SerializeField] private float _damageModifier = 1.0f;

        public float damageModifier
        {
            get { return _damageModifier; }
            set 
            {
                _damageModifier = value;

                if (value < 1) popupScript.SpawnPopup('-' + Math.Round(value * 100f).ToString() + '%', Color.magenta);
                else if (value > 1) popupScript.SpawnPopup('+' + Math.Round((value - 1) * 100f).ToString() + '%', Color.green);
            }
        } 
            
        public int damageModifierDuration;

        private Vector3 startPos;

        SpriteRenderer outlineRenderer;
        GameObject outlineChild;

        public int initiative;

        protected Playfield playfield;

        public List<Move> knownMoves;
        protected Move _currentMove = null;

        public Move GetMove()
        {
            if (_stunned) return null;
            return _currentMove;
        }

        public void SetMove(Move move)
        {
            SetMove(knownMoves.FindIndex(a => a == move));
        }

        [PunRPC]
        public void SetMove(int moveIt)
        {
            if (!PhotonNetwork.IsConnected)
            {
                setMove(moveIt);
                return;
            }

            setMove(moveIt);
            photonView.RPC(nameof(setMove), RpcTarget.Others, moveIt);
        }

        [PunRPC]
        protected void setMove(int moveIt)
        {
            if (IsStunned()) return;

            if (moveIt < 0 || moveIt >= knownMoves.Count)
            {
                _currentMove = null;
                return;
            }

            _currentMove = knownMoves[moveIt];
        }

        public bool outLine
        {
            set
            {
                outlineChild.SetActive(value);
                if (value) outlineRenderer.color = new Color(119, 251, 255, 255) / 255.0f;
            }

            get { return outlineChild.activeSelf; }
        }

        bool _selected;
        private bool _stunned = false;

        public bool selected
        {
            get
            {
                return _selected;
            }

            set
            {
                _selected = value;
                if (value) outlineRenderer.color = new Color(255, 246, 97, 255) / 255.0f;
                else outlineRenderer.color = new Color(119, 251, 255, 255) / 255.0f;
            }
        }

        virtual protected void Awake()
        {
            if (transform.parent == null) transform.parent = findParent();

            GameObject healthbarGameobject = Instantiate<GameObject>(Resources.Load<GameObject>("Healthbar"), transform);
            _healthBar = healthbarGameobject.GetComponent<HealthBar>();
            health = maxHealth;
            _healthBar.SetValue(health, maxHealth);

            BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
            box.size = new Vector2(2, 4);

            outlineChild = new GameObject();
            outlineChild.transform.SetParent(transform);
            outlineChild.transform.localPosition = new Vector3();
            outlineChild.transform.localScale = new Vector3(1,1,1);
            outlineRenderer = outlineChild.AddComponent<SpriteRenderer>();
            outlineRenderer.sprite = Resources.Load<Sprite>("Outline");
            outlineChild.SetActive(false);

            startPos = transform.GetChild(0).localPosition;

            source = gameObject.AddComponent<AudioSource>();

            popupScript = gameObject.AddComponent<PopupScript>();
        }

        protected void Update()
        {
            source.outputAudioMixerGroup = MyMixerGroup;

            stunnedParticles.SetActive(_stunned);
            deBuffedParticles.SetActive(damageModifier < 1f);
            buffedparticles.SetActive(damageModifier > 1f);

            if (_shaking)
            {
                Vector3 pos = transform.GetChild(0).position;
                pos.x += Mathf.Sin(Time.time * 30f) * 0.005f;
                transform.GetChild(0).position = pos;

                return;
            }

            if (_floating)
            {
                Vector3 pos = transform.GetChild(0).position;
                pos.y += Mathf.Abs(Mathf.Sin(Time.time * 30f)) * 0.005f;
                transform.GetChild(0).position = pos;

                return;
            }

            if (_moving)
            {
                if (_currentMove == null)
                {
                    _moving = false;
                    return;
                }

                _currentMove.Step();
                if (_currentMove.ready) _moving = false;

                return;
            }
        }

        protected abstract Transform findParent();

        [PunRPC]
        public float ModifyHealth(float amount)
        {
            if (!PhotonNetwork.IsConnected)
            {
                return modifyHealth(amount);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(modifyHealth), RpcTarget.Others, amount);
                return modifyHealth(amount);
            }

            //photonView.RPC(nameof(ModifyHealth), RpcTarget.MasterClient, amount);
            return 0;
        }

        [PunRPC]
        protected float modifyHealth(float amount)
        {
            if (isDead) return 0f;

            if (amount < 0)
            {
                popupScript.SpawnPopup(Math.Round(amount).ToString());
                StartCoroutine(shaking(0.25f));
                source.clip = hit;
                source.Play();
            }
            else if (amount > 0)
            {
                popupScript.SpawnPopup('+' + (Math.Round(amount)).ToString(), Color.green);
                StartCoroutine(floating(0.25f));
            }

            health = health + amount;
            return amount;
        }

        [PunRPC]
        public void ResetBody()
        {
            if (!PhotonNetwork.IsConnected)
            {
                resetBody();
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(resetBody), RpcTarget.Others);
                resetBody();
                return;
            }

            photonView.RPC(nameof(ResetBody), RpcTarget.MasterClient);
        }

        [PunRPC]
        protected void resetBody()
        {
            Transform childBody = transform.GetChild(0);
            childBody.localPosition = startPos;

            SpriteRenderer bodySprite = childBody.GetComponent<SpriteRenderer>();
            bodySprite.sortingOrder = 0;
        }

        public void SetPlayField(Playfield pPlayfield)
        {
            playfield = pPlayfield;
        }

        [PunRPC]
        public void MoveStep()
        {
            if (_moving) return;

            if (!PhotonNetwork.IsConnected)
            {
                moveStep();
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(moveStep), RpcTarget.AllBuffered);
                //moveStep();
                return;
            }

            photonView.RPC(nameof(MoveStep), RpcTarget.MasterClient);
        }

        protected virtual AudioClip attackSound
        {
            get { return attack; }
        }

        [PunRPC]
        protected void moveStep()
        {
            if (_currentMove == null || _stunned) return;
            _moving = true;
            source.clip = attackSound;
            source.Play();
        }

        public void AddTargetsToCurrentMove(IEnumerable<Character> targets)
        {
            if (_currentMove == null) return;

            foreach (Character target in targets)
            {
                AddTargetToCurrentMove(target);
            }
        }

        private void AddTargetToCurrentMove(Character target)
        {
            if (target is MonsterCharacters.MonsterCharacter)
            {
                AddTargetToCurrentMove(playfield.NPCs.FindIndex(a => a == target), true);
                return;
            }

            if (target is HeroCharacters.HeroCharacter)
            {
                AddTargetToCurrentMove(playfield.PCs.FindIndex(a => a == target), false);
            }
        }

        [PunRPC]
        public void MoveExecute()
        {
            if (!PhotonNetwork.IsConnected)
            {
                moveExecute();
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(moveExecute), RpcTarget.Others);
                moveExecute();
                return;
            }

            photonView.RPC(nameof(MoveExecute), RpcTarget.MasterClient);
        }

        [PunRPC]
        protected void moveExecute()
        {
            _currentMove.Execute();
        }

        [PunRPC]
        public void AddTargetToCurrentMove(int targetIt, bool TargetIsNPC)
        {
            if (!PhotonNetwork.IsConnected)
            {
                addTargetToCurrentMove(targetIt, TargetIsNPC);
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(addTargetToCurrentMove), RpcTarget.All, targetIt, TargetIsNPC);
                return;
            }

            photonView.RPC(nameof(AddTargetToCurrentMove), RpcTarget.MasterClient, targetIt, TargetIsNPC);
        }

        [PunRPC]
        protected void addTargetToCurrentMove(int targetIt, bool TargetIsNPC)
        {
            if (TargetIsNPC) _currentMove.AddTarget(playfield.NPCs[targetIt]);
            else _currentMove.AddTarget(playfield.PCs[targetIt]);
        }

        public bool IsStunned()
        {
            return _stunned;
        }

        //[PunRPC]
        public void SetStunned(bool stunned = true)
        {
            if (!PhotonNetwork.IsConnected)
            {
                setStunned(stunned);
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(setStunned), RpcTarget.All, stunned);
                return;
            }

            //photonView.RPC(nameof(SetStunned), RpcTarget.MasterClient, stunned);
        }

        [PunRPC]
        protected void setStunned(bool stunned)
        {
            if (stunned) popupScript.SpawnPopup("Stunned", Color.yellow);
            _stunned = stunned;
        }

        public void HandleCooldown()
        {
            if (!PhotonNetwork.IsConnected)
            {
                handleCooldown();
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(handleCooldown), RpcTarget.All);
                return;
            }
        }

        [PunRPC]
        protected void handleCooldown()
        {
            foreach (Move move in knownMoves)
            {
                if (move.IsOnCooldown()) move.CooldownTimerTick();
            }

            if (damageModifier == 1.0f) return;

            if (damageModifierDuration > 0)
            {
                damageModifierDuration--;
                return;
            }

            damageModifier = 1.0f;
        }

        private IEnumerator shaking(float duration)
        {
            _shaking = true;
            yield return new WaitForSeconds(duration);
            _shaking = false;
            resetBody();
        }
        private IEnumerator floating (float duration)
        {
            _floating = true;
            yield return new WaitForSeconds(duration);
            _floating = false;
            resetBody();
        }

        public abstract void SelectRandomMove();
    }
}
