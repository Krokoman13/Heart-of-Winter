using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeartOfWinter.Characters;

namespace HeartOfWinter.Arena
{
    public class Playfield : MonoBehaviour
    {
        [SerializeField]
        GameObject playerPosParent;

        [SerializeField]
        GameObject npcPosParent;

        List<Transform> PCPos;
        List<Transform> NPCPos;

        List<Character> NPCs;
        List<Character> PCs;

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

            NPCs = new List<Character>();
            PCs = new List<Character>();
        }

        bool needToArrange = false;

        private void Update()
        {
            if (needToArrange) arrange();
        }

        public void AddNPC(Character character)
        {
            NPCs.Add(character);
            character.SetPlayField(this);
            needToArrange = true;
        }

        public void AddPC(Character character)
        {
            PCs.Add(character);
            character.SetPlayField(this);
            needToArrange = true;
        }

        void arrange()
        {
            for (int i = 0; i < NPCs.Count; i++)
            {
                Character NPC = NPCs[i];

                NPC.transform.position = NPCPos[i].transform.position;
            }

            for (int i = 0; i < PCs.Count; i++)
            {
                Character PC = PCs[i];

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

            foreach (Character pc in PCs)
            {
                if (!pc.isDead) i++;
            }

            return i;
        }

    }
}
