using System;
using UnityEngine;

public class Player:MonoBehaviour {

    public static Player Instance { get; private set; } = null;
    public GameObject Object { get; private set; }
    public SpriteRenderer Renderer{ get; private set; }
    public CompositeGroupBase Movement { get; private set; }

    private void Start() {

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        Object = gameObject;
        Renderer = GetComponent<SpriteRenderer>();
        Movement = gameObject.GetComponent<PlayerPhysics>().Movement;
    }
}