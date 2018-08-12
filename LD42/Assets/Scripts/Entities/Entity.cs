using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour {

    [SerializeField] Transform effects;
    public Colour colour;
    public Entities type = Entities.NEUTRAL;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetEffectsActive(bool active)
    {
        effects.gameObject.SetActive(active);
    }

    public override bool Equals(object other)
    {
        if (!other.GetType().Equals(typeof(Entity)))
            return false;

        return (colour == ((Entity)other).colour && type == ((Entity)other).type);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

