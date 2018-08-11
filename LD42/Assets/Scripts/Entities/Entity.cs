using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour {

    public enum EntityType
    {
        NEUTRAL,
        BLIGHT,
        TRAIL,
        PLAYER
    }

    public Colour colour;
    public EntityType type = EntityType.NEUTRAL;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }


    public override bool Equals(object other)
    {
        if (!other.GetType().Equals(typeof(Entity)))
            return false;

        return (colour.Equals(((Entity)other).colour) && type == ((Entity)other).type);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

