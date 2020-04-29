using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SquareBattle
{
    public enum ActionLayer
    {
        Layer1 = 0, Layer2 = 1, Layer3 = 2, Layer4 = 3
    }

    [Serializable]
    public struct PlayerActionChain
    {
        public InputActionReference input;
        public int inputPriority;
        public ActionLayer actionLayer;

        public GameObject[] actions;
    }

   [Serializable]
    public struct PlayerActionLoop
    {
        public InputActionReference input;
        public int inputPriority;
        public ActionLayer actionLayer;
        public GameObject action;
    }
}