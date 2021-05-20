using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace DefaultNamespace
{
    public class ttt : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.mKey.isPressed)
            {
                NavMeshSurface[] objs = FindObjectsOfType(typeof(NavMeshSurface)) as NavMeshSurface[];
                //NavMeshSurface[] objs = (NavMeshSurface[]) GameObject.FindObjectsOfType (typeof(NavMeshSurface));
                objs[0].BuildNavMesh();
            }
        }
    }
}