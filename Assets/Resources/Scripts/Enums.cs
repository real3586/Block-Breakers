using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum Enemies { Basic, Speedy, Slow, Boss, Tank, Turbo, Hardened, Elite, Evolved, General, /*Final_Boss*/ }
    public enum Era { Early = 10, Mid = 30, Late = 55, End }
    public enum EnemyEffects { Power, Reload, Explosion, Freeze, 
        Normal, }
}