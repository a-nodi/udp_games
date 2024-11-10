using UnityEngine;

public class IdDistancePair : IComparable<IdDistancePair>
{
    public string Id { get; set; }
    public int Distance { get; set; }

    public IdDistancePair(string id, int distance)
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