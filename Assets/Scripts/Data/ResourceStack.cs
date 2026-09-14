using System;
using UnityEngine;

[Serializable]
public struct ResourceStack
{
    public Resource Resource;
    public int Amount;

    public ResourceStack(Resource resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }
}
