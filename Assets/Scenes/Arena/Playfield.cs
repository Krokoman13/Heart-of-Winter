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

        [SerializeField] Button readyButton;

        private enum states { wait, spawnMonsters, selectMove, getMonsterMoves, resolveMoves, endRound, checkWinOrLose}
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
            //readyButton.onClick.AddListener(() => SwitchState(states.getMonsterMoves));
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
            if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
            {
                state = states.spawnMonsters;
                return;
            }

            SwitchState(states.selectMove);
            Destroy(monsterSpawner);
        }

        private void Update()
        {
            if (needToArrange)
            {
                arrange();
                return;
            }

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

                    if (myCharacter.GetMove() != null)
                    {
                        if (!PhotonNetwork.IsConnected)
                        {
                            switchState(states.getMonsterMoves);
                            return;
                        }

                        if (!PhotonNetwork.IsMasterClient)
                        {
                            switchState(states.wait);
                            return;
                        }

                        foreach (Character character in PCs)
                        {
                            if (character.GetMove() == null) return;
                        }

                        switchState(states.getMonsterMoves);
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
                    state = states.resolveMoves;

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
                        state = states.wait;
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

                    sortedOnInitiative.RemoveAt(sortedOnInitiative.Count -1);

                    if (sortedOnInitiative.Count > 0) return;

                    sortedOnInitiative = null;
                    SwitchState(states.endRound);
                    return;

                case states.endRound:
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
                        state = states.wait;
                        return;
                    }

                    if (PCs.Count == 0)
                    {
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

        //[SerializeField] int amountPlayersDone = 0;

        [PunRPC]
        private void SwitchState(states newState)
        {
            if (PhotonNetwork.IsConnected)
            {
                state = states.wait;

                if (!PhotonNetwork.IsMasterClient) return;
                
                photonView.RPC(nameof(switchState), RpcTarget.Others, newState);
                switchState(newState);
                    

/*                amountPlayersDone++;
                
                if (amountPlayersDone >= _PCs.Count || forced)
                {
                    Debug.Log("Amount done: " + _PCs.Count);
                    photonView.RPC(nameof(switchState), RpcTarget.All, newState);
                    amountPlayersDone = 0;
                    return;
                }
*/
                return;
            }

            switchState(newState);
            return;
        }

        [PunRPC]
        private void switchState(states newState)
        {
            Debug.Log("Switching to: " + newState.ToString());
            state = newState;
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

            needToArrange = false;

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
            Destroy(heroCharacter.gameObject);
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
    }
}
