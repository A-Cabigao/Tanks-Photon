using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace QLE
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "Photon Event Handler", menuName = "Photon Event Handler")]
    public class PhotonSO : ScriptableObject {
        public void Disconnect(){
            PhotonNetwork.Disconnect();
        }
    }
}
