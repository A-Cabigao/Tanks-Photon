using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QLE
{
    public class Move : MonoBehaviour
    {
        void Update()
        {
            if(Input.GetAxis("Horizontal") > 0)
            {
                transform.position += new Vector3(10, 0, 0) * Time.deltaTime;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                transform.position += new Vector3(-10, 0, 0) * Time.deltaTime;
            }
        }
    }
}
