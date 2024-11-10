using UnityEngine;
using System;
using System.Collections.Generic;

public class IdDistancePair : IComparable<IdDistancePair>
{
    public string Id { get; set; }
    public float Distance { get; set; }

    public IdDistancePair(string id, float distance)
    {
        Id = id;
        Distance = distance;
    }

    // Implement IComparable to compare pairs by distance
    public int CompareTo(IdDistancePair other)
    {
        return Distance.CompareTo(other.Distance);
    }
}