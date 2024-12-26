using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using VInspector.Libs;
using Vector2 = UnityEngine.Vector2;

public abstract class CompositeBase : MoveComposite {
    public override int Priority { get; protected set; } = 100;
    public override Direction Apply { get; set; } = Direction.None;
    public override float Power { get; set; } = 0;

    private SortedSet<MoveComposite> composites = new();

    public override Vector2 Play(Vector2 beforeVelo, Vector2 currentVelo) {

        foreach (var composite in composites) {

            currentVelo = composite.Play(beforeVelo, currentVelo);
        }

        return currentVelo;
    }

    public void SetPower<T>(float power) where T : MoveComposite {

        foreach (var composite in GetType<T>()) {
        
            composite.Power = power;
        }
        
        GetType<T>()
            .Select(T => T.Power = power);
    }

    public List<float> GetPower<T>() where T : MoveComposite {
        
        return GetType<T>()
            .Select(T => T.Power)
            .ToList();
    }

    public void SetApply<T>(Direction apply) where T : MoveComposite {

        foreach (var composite in GetType<T>()) {
        
            composite.Apply = apply;
        }
    }

    public void AddApply<T>(Direction apply) where T : MoveComposite {

        foreach (var composite in GetType<T>()) {

            composite.Apply |= apply;
        }
    }

    public void ExtractApply<T>(Direction apply) where T : MoveComposite {
        
        foreach (var composite in GetType<T>()) {
        
            composite.Apply |= ~apply;
        }
    }

    public List<Direction> GetApply<T>() where T : MoveComposite {

         return GetType<T>()
            .Select(T => T.Apply)
            .ToList();
    }

    private List<T> GetType<T>() where T : MoveComposite {

        List<T> list = composites
            .OfType<T>()
            .ToList();

        return list;
    }


    public void AddComposite(MoveComposite composite) {

        composites.Add(composite);
    }
}