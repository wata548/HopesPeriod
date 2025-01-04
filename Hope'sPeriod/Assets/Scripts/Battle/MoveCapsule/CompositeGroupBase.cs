using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using VInspector.Libs;
using Vector2 = UnityEngine.Vector2;

public class CompositeGroupBase : MoveComposite {
    public override int Priority { get; protected set; } = 100;
    public override Direction Apply { get; set; } = Direction.None;
    public override float Power { get; set; } = 0;

    public override GameObject Owner { get; protected set; }
    
    private SortedSet<MoveComposite> composites = new();
    private SettingCollider Collider = null;

    public Direction GetCollisionDir() {

        if (Collider == null) {
            return Direction.None;
        }

        return Collider.ContactInfo.Contact;
    }
    
    public CompositeGroupBase(GameObject owner) : base(owner) {}

    public override Vector2 Play(Vector2 currentVelo, Vector2 nextVelo, Direction contactInfo = Direction.None) {

        if (Collider != null)
            contactInfo = this.Collider.ContactInfo.Contact;
        
        foreach (var composite in composites) {

            nextVelo = composite.Play(currentVelo, nextVelo, contactInfo);
        }

        return nextVelo;
    }

    public CompositeGroupBase SetCollider(SettingCollider colliderSetting) {

        Collider = colliderSetting;
        return this;
    }
    
    public CompositeGroupBase SetPower<T>(float power) where T : MoveComposite {

        foreach (var composite in GetType<T>()) {
        
            composite.Power = power;
        }
        
        GetType<T>()
            .Select(T => T.Power = power);

        return this;
    }

    public List<float> GetPower<T>() where T : MoveComposite {
        
        return GetType<T>()
            .Select(T => T.Power)
            .ToList();
    }

    public CompositeGroupBase SetApply<T>(Direction apply) where T : MoveComposite {

        foreach (var composite in GetType<T>()) {
        
            composite.Apply = apply;
        }

        return this;
    }

    public CompositeGroupBase AddApply<T>(Direction apply) where T : MoveComposite {

        foreach (var composite in GetType<T>()) {

            composite.Apply |= apply;
        }

        return this;
    }

    public CompositeGroupBase ExtractApply<T>(Direction apply) where T : MoveComposite {
        
        foreach (var composite in GetType<T>()) {
        
            composite.Apply |= ~apply;
        }

        return this;
    }

    public List<Direction> GetApply<T>() where T : MoveComposite {

         return GetType<T>()
            .Select(T => T.Apply)
            .ToList();
    }

    public List<T> GetType<T>() where T : MoveComposite {

        List<T> list = composites
            .OfType<T>()
            .ToList();

        return list;
    }


    public CompositeGroupBase AddComposite(MoveComposite composite) {

        composite.SetOwner(Owner);
        composites.Add(composite);
        return this;
    }
}