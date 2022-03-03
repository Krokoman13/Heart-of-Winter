using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using System;

namespace HeartOfWinter.Arena
{
    public class ShakeCalculator : MonoBehaviourPun
    {
        [SerializeField] List<float> timestamps = new List<float>();
        //[SerializeField] float hostTimestamp = 0;

        public int timestampCount
        {
            get
            {
                int outp;

                outp = timestamps.Count;
                //if (hostTimestamp != 0) outp++;

                return outp;
            }
        }

        public void AddTimestamp()
        {
/*            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(setHostTimestamp), RpcTarget.MasterClient);
                return;
            }*/

            photonView.RPC(nameof(addTimestamp), RpcTarget.MasterClient);  
        }

        [PunRPC]
        private void addTimestamp()
        {
            DateTime currentTime = DateTime.Now;
            float timestamp = (currentTime.Minute * 60f) + currentTime.Second + (currentTime.Millisecond / 1000f);
            timestamps.Add(timestamp);
        }

        [PunRPC]
        private void setHostTimestamp()
        {
            DateTime currentTime = DateTime.Now;
            float timestamp = (currentTime.Minute * 60f) + currentTime.Second + (currentTime.Millisecond / 1000f);
            //hostTimestamp = timestamp;
        }

        public float AverageDiffrence() 
        {
            float diffrence = 0;

            for (int i = 1; i < timestamps.Count; i++)
            {
                diffrence += Mathf.Abs(timestamps[i] - timestamps[0]);
            }

            diffrence /= timestamps.Count;

            Clear();

            return diffrence;
        }

        public void Clear()
        {
            timestamps = new List<float>();
            //hostTimestamp = 0;
        }
    }
}
