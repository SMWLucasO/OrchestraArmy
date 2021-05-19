using System;
using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Keybindings;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (KeybindingManager.Instance.Keybindings["bake"].isPressed) 
        {
            NavMeshSurface[] objs = FindObjectsOfType(typeof(NavMeshSurface)) as NavMeshSurface[];
            //var objs = Object.FindObjectOfType<NavMeshSurface>();
            objs[0].BuildNavMesh();
        }
    }
}
