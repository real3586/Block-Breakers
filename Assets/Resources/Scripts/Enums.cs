using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum Enemies { Basic, Speedy, Slow, Boss, Tank, Turbo, Swarm, Hardened, Elite, Evolved, General, /*Final_Boss*/ }
    public enum Era { Early = 20, Mid = 40, Late = 100, End }
    public enum EnemyEffects { Power, Reload, Explosion, Chill, FireRate, 
        Normal, }
}