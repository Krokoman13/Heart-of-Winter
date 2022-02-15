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


namespace HeartOfWinter.Arena
{
    public class Playfield : MonoBehaviour
    {
        [SerializeField] GameObject playerPosParent;

        [SerializeField] GameObject npcPosParent;

        List<Transform> PCPos;
        List<Transform> NPCPos;

        [SerializeField ]List<Character> _NPCs;
        public List<Character> NPCs
        {
            get { return _NPCs; }
        }

        List<Character> _PCs;
        public List<Character> PCs
        {
            get { return _PCs; }
        }

        HeroCharacter myCharacter;
        Queue<Character> targetsForMyCharacter;
        [SerializeField] MovesDisplayer myCharacterMoves;
        Move _myCurrentMove;
        
        bool needToArrange = false;

        [SerializeField] Button readyButton;

        private enum states { spawnMonsters, selectMove, getMonsterMoves, resolveMoves, checkWinLoseState}
        states state = states.selectMove;

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

            if (_NPCs == null) _NPCs = new List<Character>();
            _PCs = new List<Character>();

            readyButton.gameObject.SetActive(false);
            readyButton.onClick.AddListener(() => state = states.getMonsterMoves);
        }

        

        private void Update()
        {
            if (needToArrange) arrange();

            switch (state)
            {
                case states.spawnMonsters:
                    return;

                case states.selectMove:
                    if (myCharacter.currentMove != _myCurrentMove)
                    {
                        SelectNewMove();
                    }

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
                    if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
                    {
                        state = states.resolveMoves;
                    }

                    foreach (MonsterCharacter monster in _NPCs)
                    {
                        monster.SelectRandomMove();
                    }

                    return;

                case states.resolveMoves:
                    return;

                case states.checkWinLoseState:
                    return;
            }

        }

        private void SelectNewMove()
        {
            _myCurrentMove = myCharacter.currentMove;

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

            readyButton.gameObject.SetActive(false);
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

        public void AddNPC(Character character)
        {
            _NPCs.Add(character);
            character.SetPlayField(this);
            needToArrange = true;
        }

        public void AddPC(HeroCharacter character)
        {
            _PCs.Add(character);
            character.SetPlayField(this);
            needToArrange = true;

            if (character.heroName == PlayerInformation.PlayerInfo.character)
            {
                myCharacter = character;
                myCharacterMoves.SetCharacter(myCharacter);
            }
        }

        void arrange()
        {
            for (int i = 0; i < _NPCs.Count; i++)
            {
                Character NPC = _NPCs[i];

                NPC.transform.position = NPCPos[i].transform.position;
            }

            for (int i = 0; i < _PCs.Count; i++)
            {
                Character PC = _PCs[i];

                PC.transform.position = PCPos[i].transform.position;
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

            if (myCharacter.currentMove.amountOfTargets < targetsForMyCharacter.Count)
            {
                Character removed = targetsForMyCharacter.Dequeue();
                removed.selected = false;
            }
        }
    }
}
