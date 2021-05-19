using System;
using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Entity.Controllers;
using OrchestraArmy.Keybindings;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using OrchestraArmy.Entity.Entities.Enemies.Data;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public StateData StateData { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (KeybindingManager.Instance.Keybindings["bake"].isPressed) 
        {
            NavMeshSurface[] objs = FindObjectsOfType(typeof(NavMeshSurface)) as NavMeshSurface[];
            objs[0].BuildNavMesh();
        }
    }
}
