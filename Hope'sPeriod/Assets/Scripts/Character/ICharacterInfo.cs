using System;
using UnityEngine;

public interface ICharacterInfo {
    float MaximumHp { get; } 
    float CurrentHp { get; }
    
    float MaximumMp { get; }
    float CurrentMp { get; }
}