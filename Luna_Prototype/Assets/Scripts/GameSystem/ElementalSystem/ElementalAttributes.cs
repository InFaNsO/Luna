using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalAttributes : MonoBehaviour, ElementalSystem
{
    //For enemies 100 means 1.00x damage is taken from that element
    //If above 100 enemy takes extra damage (example: 150 will result in enemy taking 1.50x from that element alone)
    //Like wise if below 100 enemy will take less damage (example: 50 will result in enemy taking 0.50x damage from that element)
    //For weapons, these are flat damages, 150 fire means the weapon will do 150 fire damage, NOT 1.5x fire damage
    //voidType is a damage type that ignores all enemental resistances target has and always deal the full damage assgned
    //voidType does not affect damage taken
    public float voidType =     0;
    public float fire =         100;
    public float earth =        100;
    public float metal =        100;
    public float water =        100;
    public float wood =         100;
    public float wind =         100;
    public float lightning =    100;

    //Chance that an effect will occur, 100 for 100% chance and 0 being 0% chance, will not have any effect above 100
    public float fireEffectChance = 0.0f;
    public float earthEffectChance = 0.0f;
    public float metalEffectChance = 0.0f;
    public float waterEffectChance = 0.0f;
    public float woodEffectChance = 0.0f;
    public float windEffectChance = 0.0f;
    public float lightningEffectChance = 0.0f;

    //The intensity is a flat value, not a multiplier
    public float fireEffectIntensity = 0.0f;
    public float earthEffectIntensity = 0.0f;
    public float metalEffectIntensity = 0.0f;
    public float waterEffectIntensity = 0.0f;
    public float woodEffectIntensity = 0.0f;
    public float windEffectIntensity = 0.0f;
    public float lightningEffectIntensity = 0.0f;

    struct elementalPair
    {
        ElementalType 
    }
    public void ApplyElementalDamage(ref Enemy target, ElementalType[] element, float damage)
    {
    }

    public void ApplyElementalEffect(ref Enemy target, ElementalType[] element, float intensity, float chance)
    {
    }

    public void ApplyElementalEffect(ref Player target, ElementalType[] element, float intensity, float chance)
    {
        throw new System.NotImplementedException();
    }

    public elementalPair[] EvaluateElementIntensity(ElementalType[] element)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
