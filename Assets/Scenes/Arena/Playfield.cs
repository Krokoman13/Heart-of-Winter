using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;

using HeartOfWinter.Characters;
using HeartOfWinter.Characters.HeroCharacters;
using HeartOfWinter.Characters.MonsterCharacters;
using HeartOfWinter.Moves;

using System;

namespace HeartOfWinter.Arena
{
    public class Playfield : MonoBehaviourPun
    {
        [SerializeField] GameObject playerPosParent;

        [SerializeField] GameObject npcPosParent;

        List<Transform> PCPos;
        List<Transform> NPCPos;

        [SerializeField] Transform NPCParent;
        List<Character> _NPCs;
        public List<Character> NPCs
        {
            get { return _NPCs; }
        }

        [SerializeField] Transform PCParent;
        [SerializeField] List<Character> _PCs;
        public List<Character> PCs
        {
            get { return _PCs; }
        }

        HeroCharacter myCharacter;
        Queue<Character> targetsForMyCharacter;
        [SerializeField] MovesDisplayer myCharacterMoves;
        Move _myCurrentMove;

        [SerializeField] Timer arenaTimer;

        private bool needToArrange = false;

        [PunRPC]
        public void NeedsToArrange()
        {
            if (!PhotonNetwork.IsConnected)
            {
                needsToArrange();
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(needsToArrange), RpcTarget.Others);
                needsToArrange();
                return;
            }

            photonView.RPC(nameof(NeedsToArrange), RpcTarget.MasterClient);
        }

        [PunRPC]
        private void needsToArrange()
        {
            needToArrange = true;
        }

        [SerializeField] Button _readyButton;

        private enum states { wait, spawnMonsters, selectMove, getMonsterMoves, shaking, resolveMoves, endRound, checkWinOrLose }
        [SerializeField] states _state = states.wait;

        List<Character> _sortedOnInitiative = null;

        MonsterSpawner _monsterSpawner;

        ShakeCalculator _shakeCalculator;
        bool _shaken = false;
        public float shakeModifier = 0f;

        [SerializeField] Text shakemodText;
        [SerializeField] RectTransform shakePannel;
        [SerializeField] RectTransform shakeTextBox;


        void Awake()
        {
            NPCPos = new List<Transform>();
            foreach (Transform transform in npcPosParent.transform)
            {
                NPCPos.Add(transform);
            }

            PCPos = new List<Transform>();
            foreach (Transform transform in playerPosParent.transform)
            {
                PCPos.Add(transform);
            }

            _NPCs = new List<Character>();
            _PCs = new List<Character>();

            _readyButton.gameObject.SetActive(false);
            //readyButton.onClick.AddListener(() => SwitchState(states.getMonsterMoves));
            _readyButton.onClick.AddListener(() => stopOutlining(_NPCs));
            _readyButton.onClick.AddListener(() => stopOutlining(_PCs));
            _readyButton.onClick.AddListener(() => _readyButton.gameObject.SetActive(false));
            _readyButton.onClick.AddListener(() => myCharacter.SetMove(_myCurrentMove));
            _readyButton.onClick.AddListener(() => myCharacter.AddTargetsToCurrentMove(targetsForMyCharacter));
            _readyButton.onClick.AddListener(() => targetsForMyCharacter.Clear());

            _monsterSpawner = GetComponent<MonsterSpawner>();
            _shakeCalculator = gameObject.AddComponent<ShakeCalculator>();
        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
            {
                _state = states.spawnMonsters;
                return;
            }

            _state = states.selectMove;
            Destroy(_monsterSpawner);
        }

        private void Update()
        {
            if (needToArrange)
            {
                arrange();
                return;
            }

            switch (_state)
            {
                case states.wait:
                    arenaTimer.enabled = false;
                    //shakeTimer.enabled = false;
                    return;

                case states.spawnMonsters:
                    _state = states.selectMove;
                    if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;

                    _monsterSpawner.SpawnNextWave();
                    return;

                case states.selectMove:
                    if (myCharacter == null) return;
                    _shaken = false;

                    if (myCharacter.GetMove() != null)
                    {
                        arenaTimer.enabled = false;

                        if (!PhotonNetwork.IsConnected)
                        {
                            _state = states.getMonsterMoves;
                            return;
                        }

                        if (!PhotonNetwork.IsMasterClient)
                        {
                            _state = states.wait;
                            return;
                        }

                        foreach (Character character in PCs)
                        {
                            if (character.GetMove() == null) return;
                        }

                        _state = states.getMonsterMoves;
                        return;
                    }

                    if (!arenaTimer.enabled && !arenaTimer.done) arenaTimer.StartTimer();

                    _readyButton.gameObject.SetActive(false);

                    if (arenaTimer.done)
                    {
                        myCharacter.SelectRandomMove();
                        arenaTimer.StartTimer();

                        if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
                        {
                            _state = states.wait;
                            return;
                        }
                    }

                    if (_myCurrentMove == null) return;

                    if (targetsForMyCharacter.Count == _myCurrentMove.amountOfTargets)
                    {
                        _readyButton.gameObject.SetActive(true);
                        return;
                    }

                    if (_myCurrentMove.targetsNPCs && targetsForMyCharacter.Count == _NPCs.Count)
                    {
                        _readyButton.gameObject.SetActive(true);
                        return;
                    }

                    if (_myCurrentMove.targetsPCs && targetsForMyCharacter.Count == _PCs.Count)
                    {
                        _readyButton.gameObject.SetActive(true);
                        return;
                    }
                    return;

                case states.getMonsterMoves:
                    if (PCs.Count > 1)
                    {
                        SwitchState(states.shaking);
                    }
                    else SwitchState(states.resolveMoves);

                    if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
                    {
                        return;
                    }

                    foreach (Character monster in _NPCs)
                    {
                        monster.SelectRandomMove();
                    }
                    return;

                case states.shaking:
                    if (!shakePannel.gameObject.activeSelf)
                    {
                        shakePannel.gameObject.SetActive(true);
                        StartCoroutine(ShakeTimer());
                    }

                    if (_shaken)
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            if (_shakeCalculator.timestampCount == PCs.Count)
                            {
                                float mod = _shakeCalculator.AverageDiffrence();
                                mod -= 0.2f;
                                mod = mod / 2f;
                                mod = 1f - mod;

                                if (mod < 0f) mod = 0f;
                                SetShakeModifier(mod);

                                Debug.Log("Modifier: " + mod);
                                SwitchState(states.resolveMoves, 3f);
                                return;
                            }
                        }

                        return;
                    }

                    Vector3 acc = Input.acceleration;

                    if (acc.sqrMagnitude >= 3f)
                    {
                        MarkShaking();
                    }
                    return;

                case states.resolveMoves:
                    StopAllCoroutines();

                    shakePannel.gameObject.SetActive(false);
                    shakeTextBox.gameObject.SetActive(false);

                    if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
                    {
                        _state = states.wait;
                        return;
                    }

                    if (_sortedOnInitiative == null)
                    {
                        List<Character> allCharacters = new List<Character>(PCs);
                        allCharacters.AddRange(NPCs);
                        _sortedOnInitiative = sortOnInitiative(allCharacters);
                        return;
                    }

                    Character currentChar = _sortedOnInitiative.Last();
                    if (!currentChar.isDead)
                    {
                        Move currentMove = currentChar.GetMove();

                        if (currentMove != null)
                        {
                            currentChar.MoveStep();

                            if (!currentMove.ready) return;
                            Debug.Log("Move:" + currentMove.iconName);

                            currentChar.MoveExecute();
                            return;
                        }

                        currentChar.SetStunned(false);
                    }

                    _sortedOnInitiative.RemoveAt(_sortedOnInitiative.Count - 1);

                    if (_sortedOnInitiative.Count > 0)
                    {
                        StartCoroutine(waitFor(1f));
                        return;
                    }

                    _sortedOnInitiative = null;
                    SwitchState(states.endRound);
                    return;

                case states.endRound:
                    StopAllCoroutines();
                    if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;

                    foreach (Character character in PCs)
                    {
                        character.HandleCooldown();
                    }
                    NeedsToArrange();
                    SwitchState(states.checkWinOrLose);
                    return;

                case states.checkWinOrLose:
                    if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
                    {
                        _state = states.wait;
                        return;
                    }

                    if (PCs.Count == 0)
                    {
                        _state = states.wait;

                        if (PhotonNetwork.IsConnected)
                        {
                            PhotonNetwork.LoadLevel(12);
                            //PhotonNetwork.CurrentRoom.IsOpen = false;
                            //PhotonNetwork.CurrentRoom.IsVisible = false;
                            //PhotonNetwork.CurrentRoom.PlayerTtl = 0;
                            //PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;
                            //PhotonNetwork.LeaveRoom();
                            //PhotonNetwork.LeaveLobby();
                            return;
                        }

                        SceneManager.LoadScene(12);
                        return;
                    }

                    if (NPCs.Count == 0)
                    {
                        SwitchState(states.spawnMonsters);
                        return;
                    }

                    SwitchState(states.selectMove);
                    return;
            }
        }

        public void MarkShaking()
        {
            if (_shaken) return;
            _shakeCalculator.AddTimestamp();
            _shaken = true;
        }

        //[SerializeField] int amountPlayersDone = 0;

        [PunRPC]
        private void SwitchState(states newState, float delay = 0f)
        {
            if (PhotonNetwork.IsConnected)
            {
                _state = states.wait;

                if (!PhotonNetwork.IsMasterClient) return;

                photonView.RPC(nameof(switchState), RpcTarget.Others, newState, delay);
                switchState(newState, delay);
                return;
            }

            switchState(newState, delay);
            return;
        }

        [PunRPC]
        private void switchState(states newState, float delay)
        {
            Debug.Log("Switching to: " + newState.ToString());
            _state = newState;

            if (delay != 0f) StartCoroutine(waitFor(delay));

        }

        private List<Character> sortOnInitiative(List<Character> allCharacters)
        {
            allCharacters.Sort((x, y) => x.initiative <= y.initiative ? -1 : 1);
            return allCharacters;
        }

        public void SelectNewMove(Move newMove)
        {
            if (myCharacter.GetMove() != null) return;
            //if (state != states.selectMove) return;
            if (newMove.IsOnCooldown()) return;

            _myCurrentMove = newMove;

            stopOutlining(_NPCs);
            stopOutlining(_PCs);
            targetsForMyCharacter = new Queue<Character>();

            if (_myCurrentMove == null)
            {
                return;
            }

            if (_myCurrentMove.targetsNPCs)
            {
                outlineAll(_NPCs);
                if (_myCurrentMove.amountOfTargets >= _NPCs.Count) selectAll(_NPCs);
                return;
            }

            if (_myCurrentMove.targetsPCs)
            {
                outlineAll(_PCs);
                if (_myCurrentMove.amountOfTargets >= _PCs.Count) selectAll(_PCs);
                return;
            }
        }

        private void selectAll(List<Character> characters)
        {
            foreach (Character character in characters)
            {
                select(character);
            }
        }

        private void stopOutlining(IEnumerable<Character> characters)
        {
            foreach (Character character in characters)
            {
                character.outLine = false;
            }
        }

        private void outlineAll(List<Character> characters)
        {
            foreach (Character character in characters)
            {
                character.outLine = true;
            }
        }

        private void addNPC(MonsterCharacter character)
        {
            if (_NPCs.Contains(character)) return;
            _NPCs.Add(character);
            character.SetPlayField(this);
        }

        private void addPC(HeroCharacter character)
        {
            if (_PCs.Contains(character)) return;

            _PCs.Add(character);
            character.SetPlayField(this);

            if (myCharacter != null) return;

            if (character.heroName == PlayerInformation.PlayerInfo.character)
            {
                myCharacter = character;
                myCharacterMoves.SetCharacter(myCharacter);
            }
        }

        void arrange()
        {
            needToArrange = false;

            if (NPCs != null && PCs != null)
            {
                NPCs.RemoveAll(item => item == null);
                PCs.RemoveAll(item => item == null);

                foreach (Character NPC in NPCs)
                {
                    if (NPC.isDead) killNPC(NPC);
                }

                foreach (Character PC in PCs)
                {
                    if (PC.isDead) killPC(PC);
                }
            }

            int i = 0;

            foreach (Transform NPC in NPCParent)
            {
                addNPC(NPC.GetComponent<MonsterCharacter>());
                NPC.position = NPCPos[i].position;
                i++;
            }

            i = 0;

            foreach (Transform PC in PCParent)
            {
                addPC(PC.GetComponent<HeroCharacter>());
                PC.position = PCPos[i].position;
                i++;
            }
        }

        private void killPC(Character PC)
        {
            HeroCharacter heroCharacter = (HeroCharacter)PC;

            //PCs.Remove(heroCharacter);
            if (heroCharacter == myCharacter) myCharacterMoves.DisableMoves();
            DestroyImmediate(heroCharacter.gameObject);
            NeedsToArrange();
        }

        private void killNPC(Character NPC)
        {
            //NPCs.Remove(NPC);
            Destroy(NPC.gameObject);
            NeedsToArrange();
        }

        bool PCsDone()
        {
            return true;
        }

        int livingPCs()
        {
            int i = 0;

            foreach (Character pc in _PCs)
            {
                if (!pc.isDead) i++;
            }

            return i;
        }

        public void ClickedOn(Character character)
        {
            if (!character.selected) select(character);
            else deselect(character);
        }

        private void deselect(Character character)
        {
            character.selected = false;
            targetsForMyCharacter = new Queue<Character>(targetsForMyCharacter.Where(x => x != character));
        }

        private void select(Character character)
        {
            character.selected = true;
            targetsForMyCharacter.Enqueue(character);

            if (_myCurrentMove.amountOfTargets < targetsForMyCharacter.Count)
            {
                Character removed = targetsForMyCharacter.Dequeue();
                removed.selected = false;
            }
        }

        private IEnumerator waitFor(float seconds)
        {
            states backupState = _state;
            _state = states.wait;
            yield return new WaitForSeconds(seconds);
            _state = backupState;
        }

        public void SetShakeModifier(float pShakeMod)
        {
            if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient) return;

            photonView.RPC(nameof(setShakeModifier), RpcTarget.Others, pShakeMod);
            setShakeModifier(pShakeMod);
        }

        [PunRPC]
        private void setShakeModifier(float pShakeMod)
        {
            shakeModifier = pShakeMod;
            shakemodText.text = (Math.Round(shakeModifier, 2) * 10).ToString();
            shakeTextBox.gameObject.SetActive(true);
        }

        IEnumerator ShakeTimer()
        {
            yield return new WaitForSeconds(10f);

            Debug.Log("Timer ran out!");

            SetShakeModifier(0f);
            SwitchState(states.resolveMoves, 3f);
        }
    }
}
