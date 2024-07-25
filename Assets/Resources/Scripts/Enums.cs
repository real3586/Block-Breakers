using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum Enemies { Basic, Speedy, Slow, Boss, Tank, Turbo, Swarm, Hardened, Elite, Evolved, General, FinalBoss }
    public enum Era { Early = 30, Mid = 90, Late = 150, End }
    public enum EnemyEffects { Power, Reload, Explosion, Chill, FireRate, 
        Normal, }
    public enum Lasers { Normal, Rampager, Execute, Shatter, Overload, }
}