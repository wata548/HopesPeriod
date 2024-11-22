using UnityEngine;

public interface ISkill {
    string KoreanName { get; }
    string Info       { get; }
    float  Power      { get; }
    float  RequireHP  { get; }
    float  RequireMP  { get; }
    int    Duration   { get; }
    AccelerateType AccelerateType { get; }
    float  Accelerate { get; }
}

