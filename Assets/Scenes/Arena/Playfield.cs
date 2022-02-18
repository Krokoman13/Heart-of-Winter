using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
        List<Character> _PCs;
        public List<Character> PCs
        {
            get { return _PCs; }
        }

        HeroCharacter myCharacter;
        Queue<Character> targetsForMyCharacter;
        [SerializeField] MovesDisplayer myCharacterMoves;
        Move _myCurrentMove;

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
                photonView.RPC(nameof(needsToArrange), RpcTarget.All);
                return;
            }

            photonView.RPC(nameof(NeedsToArrange), RpcTarget.MasterClient);
        }

        [PunRPC]
        private void needsToArrange()
        {
            needToArrange = true;
        }

        [SerializeField] Button readyButton;

        private enum states { wait, spawnMonsters, selectMove, getMonsterMoves, resolveMoves, endRound}
        [SerializeField] states state = states.wait;

        List<Character> sortedOnInitiative = null;

        MonsterSpawner monsterSpawner;

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

            readyButton.gameObject.SetActive(false);
            readyButton.onClick.AddListener(() => SwitchState(states.getMonsterMoves));
            readyButton.onClick.AddListener(() => stopOutlining(_NPCs));
            readyButton.onClick.AddListener(() => stopOutlining(_PCs));
            readyButton.onClick.AddListener(() => readyButton.gameObject.SetActive(false));
            readyButton.onClick.AddListener(() => myCharacter.SetMove(_myCurrentMove)); 
            readyButton.onClick.AddListener(() => myCharacter.AddTargetsToCurrentMove(targetsForMyCharacter));
            readyButton.onClick.AddListener(() => targetsForMyCharacter.Clear());

            monsterSpawner = GetComponent<MonsterSpawner>();
        }

        private void Start()
        {
            SwitchState(states.selectMove);

            if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
            {
                state = states.spawnMonsters;
                return;
            }

            Destroy(monsterSpawner);
        }

        private void Update()
        {
            if (needToArrange) arrange();

            switch (state)
            {
                case states.wait:
                    return;

                case states.spawnMonsters:
                    monsterSpawner.SpawnNextWave();
                    SwitchState(states.selectMove);
                    return;

                case states.selectMove:
                    if (myCharacter == null) return;
                    if (myCharacter.IsStunned())
                    {
                        SwitchState(states.getMonsterMoves);
                        return;
                    }

                    readyButton.gameObject.SetActive(false);

                    if (_myCurrentMove == null) return;

                    if (targetsForMyCharacter.Count == _myCurrentMove.amountOfTargets)
                    {
                        readyButton.gameObject.SetActive(true);
                        return;
                    }

                    if (_myCurrentMove.targetsNPCs && targetsForMyCharacter.Count == _NPCs.Count)
                    {
                        readyButton.gameObject.SetActive(true);
                        return;
                    }

                    if (_myCurrentMove.targetsPCs && targetsForMyCharacter.Count == _PCs.Count)
                    {
                        readyButton.gameObject.SetActive(true);
                        return;
                    }
                    return;

                case states.getMonsterMoves: 
                    state = state = states.resolveMoves;

                    if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
                    {
                        return;
                    }

                    foreach (MonsterCharacter monster in _NPCs)
                    {
                        monster.SelectRandomMove();
                    }
                    return;

                case states.resolveMoves:
                    if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
                    {
                        switchState(states.selectMove);
                        return;
                    }

                    if (sortedOnInitiative == null)
                    {
                        List<Character> allCharacters = new List<Character>(PCs);
                        allCharacters.AddRange(NPCs);
                        sortedOnInitiative = sortOnInitiative(allCharacters);
                        return;
                    }

                    Character currentChar = sortedOnInitiative.Last();
                    Move currentMove = currentChar.GetMove();

                    if (currentMove != null)
                    {
                        currentChar.MoveStep();

                        if (!currentMove.ready) return;
                        Debug.Log(currentMove.iconName);

                        currentMove.Execute();
                    }

                    sortedOnInitiative.RemoveAt(sortedOnInitiative.Count -1);

                    if (sortedOnInitiative.Count > 0) return;

                    sortedOnInitiative = null;
                    switchState(states.endRound);
                    return;

                case states.endRound:
                    foreach (Character character in PCs)
                    {
                        character.HandleCooldown();
                    }

                    switchState(states.selectMove);
                    return;
            }
        }

        int amountPlayersDone = 0;

        [PunRPC]
        private void SwitchState(states newState)
        {
            if (PhotonNetwork.IsConnected)
            {
                state = states.wait;

                if (!PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC(nameof(SwitchState), RpcTarget.MasterClient, newState);
                    return;
                }

                amountPlayersDone++;

                if (amountPlayersDone >= PCs.Count)
                {
                    photonView.RPC(nameof(switchState), RpcTarget.All, newState);
                    amountPlayersDone = 0;
                    return;
                }

                return;
            }

            switchState(newState);
            return;
        }

        [PunRPC]
        private void switchState(states newState)
        {
            state = newState;
        }

        private List<Character> sortOnInitiative(List<Character> allCharacters)
        {
            allCharacters.Sort((x, y) => x.initiative <= y.initiative ? -1 : 1);
            return allCharacters;
        }

        public void SelectNewMove(Move newMove)
        {
            if (myCharacter.IsStunned()) return;
            if (state != states.selectMove) return;
            if (newMove.IsOnCooldown()) return;

            _myCurrentMove = newMove;

            if (_myCurrentMove == null)
            {
                stopOutlining(_NPCs);
                stopOutlining(_PCs);
                targetsForMyCharacter.Clear();
                return;
            }

            targetsForMyCharacter = new Queue<Character>();

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

            needToArrange = false;
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
    }
}
