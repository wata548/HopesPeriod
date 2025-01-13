using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        foreach (var composite in GetTypes<T>()) {
        
            composite.Power = power;
        }
        return this;
    }

     public float GetPower<T>() where T : MoveComposite {

         return GetType<T>().Power;
     }
     
    public List<float> GetPowers<T>() where T : MoveComposite {
        
        return GetTypes<T>()
            .Select(T => T.Power)
            .ToList();
    }

    public CompositeGroupBase SetApply<T>(Direction apply) where T : MoveComposite {

        foreach (var composite in GetTypes<T>()) {
        
            composite.Apply = apply;
        }

        return this;
    }

    public CompositeGroupBase AddApply<T>(Direction apply) where T : MoveComposite {

        foreach (var composite in GetTypes<T>()) {

            composite.Apply |= apply;
        }

        return this;
    }

    public CompositeGroupBase ExtractApply<T>(Direction apply) where T : MoveComposite {
        
        foreach (var composite in GetTypes<T>()) {
        
            composite.Apply |= ~apply;
        }

        return this;
    }

    public List<Direction> GetApplys<T>() where T : MoveComposite {

         return GetTypes<T>()
            .Select(T => T.Apply)
            .ToList();
    }
    public Direction GetApply<T>() where T : MoveComposite {

        return GetType<T>().Apply;
    }

    public List<T> GetTypes<T>() where T : MoveComposite {

        List<T> list = composites
            .OfType<T>()
            .ToList();

        return list;
    }
    public T GetType<T>() where T : MoveComposite {

        return GetTypes<T>()[0];
    }

    public CompositeGroupBase AddComposite(MoveComposite composite) {

        composite.SetOwner(Owner);
        composites.Add(composite);
        return this;
    }
}