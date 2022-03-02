using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace HeartOfWinter.Arena
{
    public class ShakeCalculator : MonoBehaviourPun
    {
        [SerializeField] List<float> timestamps = new List<float>();
        [SerializeField] float hostTimestamp = 0;

        public int timestampCount
        {
            get
            {
                int outp;

                outp = timestamps.Count;
                if (hostTimestamp != 0) outp++;

                return outp;
            }
        }

        public void AddTimestamp()
        {
            float timestamp = Time.time;

            if (PhotonNetwork.IsMasterClient)
            {
                hostTimestamp = timestamp;
                return;
            }

            photonView.RPC(nameof(addTimestamp), RpcTarget.MasterClient, timestamp);
            
        }

        [PunRPC]
        private void addTimestamp(float timestamp)
        {
            timestamps.Add(timestamp);
        }

        public float AverageDiffrence() 
        {
            float diffrence = 0;

            foreach (float timestamp in timestamps)
            {
                diffrence += Mathf.Abs(timestamp - hostTimestamp);
            }

            diffrence /= timestamps.Count;

            timestamps = new List<float>();
            hostTimestamp = 0;

            return diffrence;
        }
    }
}
