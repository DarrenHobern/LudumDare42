using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Colour")]
public class Colour : ScriptableObject {

    public Sprite playerSprite;
    public Sprite trail;
    public Sprite blight;

    public override string ToString()
    {
        return trail.name;
    }
}
