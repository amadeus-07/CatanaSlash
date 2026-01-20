using System;
using Core;
using UnityEngine;

public abstract class Enemy : Character
{
    [field: SerializeField] protected Character Target {get;  private set;}

    public void SetTarget(Character target)
    {
        Target = target;
    }

    protected void Awake()
    {
        base.Awake();
    }

    protected void Start()
    {
        base.Start();
    }
}