using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

using HeartOfWinter.Arena;
using HeartOfWinter.Moves;
using System;

namespace HeartOfWinter.Characters
{
    public abstract class Character : MonoBehaviourPun
    {
        private HealthBar _healthBar;
        private float _health;
        public float maxHealth = 1.0f;
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

                _healthBar.setValue(value / maxHealth);
                _health = value;
            }
        }

        public bool isDead
        {
            get { return health > 0.5f; }
        }

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

            gameObject.AddComponent<BoxCollider2D>();

            outlineChild = new GameObject();
            outlineChild.transform.SetParent(transform);
            outlineChild.transform.localPosition = new Vector3();
            outlineRenderer = outlineChild.AddComponent<SpriteRenderer>();
            outlineRenderer.sprite = Resources.Load<Sprite>("Outline");
            outlineChild.SetActive(false);
        }

        protected abstract Transform findParent();

        [PunRPC]
        public float ModifyHealth(float amount)
        {
            if (!PhotonNetwork.IsConnected)
            {
                return removeHealth(amount);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(removeHealth), RpcTarget.Others, amount);
                return removeHealth(amount);
            }

            photonView.RPC(nameof(ModifyHealth), RpcTarget.MasterClient, amount);
            return 0;
        }

        [PunRPC]
        protected float removeHealth(float amount)
        {
            health = health + amount;
            return amount;
        }

        public void SetPlayField(Playfield pPlayfield)
        {
            playfield = pPlayfield;
        }

        [PunRPC]
        public void MoveStep()
        {
            if (!PhotonNetwork.IsConnected)
            {
                moveStep();
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(moveStep), RpcTarget.All);
                //moveStep();
                return;
            }

            photonView.RPC(nameof(MoveStep), RpcTarget.MasterClient);
        }

        [PunRPC]
        protected void moveStep()
        {
            if (_currentMove == null || _stunned) return;
            _currentMove.Step();
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

        [PunRPC]
        public void GetStunned()
        {
            if (!PhotonNetwork.IsConnected)
            {
                getStunned();
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(getStunned), RpcTarget.All);
                return;
            }

            photonView.RPC(nameof(GetStunned), RpcTarget.MasterClient);
        }

        [PunRPC]
        protected void getStunned()
        {
            _stunned = true;
        }

        public void HandleCooldown()
        {
            foreach (Move move in knownMoves)
            {
                if (move.IsOnCooldown()) move.CooldownTimerTick();
            }
        }
    }
}
