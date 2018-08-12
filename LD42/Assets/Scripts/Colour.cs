using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Colour")]
public class Colour : ScriptableObject {

    public Sprite playerSprite;
    public Sprite trail;
    public Sprite blight;

    public override bool Equals(object other)
    {
        if (other.GetType() != typeof(Colour))
            return false;

        return ((Colour)other).trail == trail;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return trail.name;
    }
}
