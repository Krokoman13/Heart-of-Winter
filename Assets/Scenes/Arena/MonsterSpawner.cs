using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using HeartOfWinter.Characters.MonsterCharacters;

namespace HeartOfWinter.Arena
{
    public class MonsterSpawner : MonoBehaviourPun  
    {
        Playfield playfield;

        [SerializeField] Transform monsterParent;

        [SerializeField] int wave;
        private bool _done = false;
        public bool done
        {
            get { return _done; }
        }

        [SerializeField] MonsterCharacter direWolfPrefab;
        [SerializeField] MonsterCharacter draugrPrefab;
        [SerializeField] MonsterCharacter wendigofPrefab;
        [SerializeField] MonsterCharacter frostWolfPrefab;

        void Awake()
        {
            playfield = GetComponent<Playfield>();
            //Spawn(PlayerInfo.character.ToString());
        }

        public void SpawnNextWave()
        {
            spawnWave(wave);
            wave++;
        }

        void spawnWave(int i)
        {
            if (i < 0) return;

            switch (i)
            {
                case 0:
                    spawn(wendigofPrefab.gameObject);
                    spawn(frostWolfPrefab.gameObject);
                    spawn(direWolfPrefab.gameObject);
                    return;

                case 1:
                    spawn(draugrPrefab.gameObject);
                    spawn(wendigofPrefab.gameObject);
                    spawn(frostWolfPrefab.gameObject);
                    return;

                case 2:
                    spawn(direWolfPrefab.gameObject);
                    spawn(draugrPrefab.gameObject);
                    spawn(wendigofPrefab.gameObject);
                    return;

                default:
                    _done = true;
                    return;
            }
        }

        void spawn(GameObject monsterPrefab)
        {
            GameObject copyOfMonster;

            if (!PhotonNetwork.IsConnected)
            {
                copyOfMonster = Instantiate<GameObject>(Resources.Load<GameObject>(monsterPrefab.name), monsterParent);
            }
            else
            {
                //copyOfHero = Instantiate<GameObject>(Resources.Load<GameObject>(heroPrefabName + " Variant"), heroParent);
                copyOfMonster = PhotonNetwork.Instantiate(monsterPrefab.name + " Variant", Vector3.zero, Quaternion.Euler(Vector3.zero));
            }

            //MonsterCharacter monster = copyOfMonster.GetComponent<MonsterCharacter>();
            playfield.NeedsToArrange();
        }
    }
}