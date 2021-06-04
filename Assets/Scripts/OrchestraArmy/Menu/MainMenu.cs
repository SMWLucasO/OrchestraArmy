using System.Collections;
using System.Collections.Generic;
using OrchestraArmy.Enum;
using UnityEngine;

public class MainMenu : IMenu
{
    public MenuButtons Selected { get; set; }
    public MenuButtons[] PosibleButtons { get; set; }
}
