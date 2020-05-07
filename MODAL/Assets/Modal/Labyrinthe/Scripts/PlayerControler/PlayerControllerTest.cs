using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Controller
{
    public class PlayerControllerTest : MonoBehaviour
    {
        public static PlayerControllerTest instance;
        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            print("PlayerControllerTest Founded");
        }

    }
}