using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalAttributes : MonoBehaviour, ElementalSystem
{
    public bool inclusiveElement = false;

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
    private float voidTypeEffectChance = 0.0f;
    public float fireEffectChance = 0.0f;
    public float earthEffectChance = 0.0f;
    public float metalEffectChance = 0.0f;
    public float waterEffectChance = 0.0f;
    public float woodEffectChance = 0.0f;
    public float windEffectChance = 0.0f;
    public float lightningEffectChance = 0.0f;

    //The intensity is a flat value, not a multiplier
    private float voidTypeIntensity = 0.0f;
    public float fireEffectIntensity = 0.0f;
    public float earthEffectIntensity = 0.0f;
    public float metalEffectIntensity = 0.0f;
    public float waterEffectIntensity = 0.0f;
    public float woodEffectIntensity = 0.0f;
    public float windEffectIntensity = 0.0f;
    public float lightningEffectIntensity = 0.0f;

    private ElementData[] mElementData;

    public struct ElementData
    {
        public ElementalType type;
        public float potency;
        public float intensity;
        public float chance;
        public bool flag; //flag for re-evaluation
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
    public void SetElementValues(ElementalType _type, float _potency, float _intensity, float _chance, bool _flag)
    {
        switch(_type)
        {
            case ElementalType.fire: //primaries
                {
                    mElementData[(int)ElementIndex.fire].potency = _potency;
                    mElementData[(int)ElementIndex.fire].intensity = _intensity;
                    mElementData[(int)ElementIndex.fire].chance = _chance;
                    break;
                }
            case ElementalType.earth:
                {
                    mElementData[(int)ElementIndex.earth].potency = _potency;
                    mElementData[(int)ElementIndex.earth].intensity = _intensity;
                    mElementData[(int)ElementIndex.earth].chance = _chance;
                    break;
                }
            case ElementalType.metal:
                {
                    mElementData[(int)ElementIndex.metal].potency = _potency;
                    mElementData[(int)ElementIndex.metal].intensity = _intensity;
                    mElementData[(int)ElementIndex.metal].chance = _chance;
                    break;
                }
            case ElementalType.water:
                {
                    mElementData[(int)ElementIndex.water].potency = _potency;
                    mElementData[(int)ElementIndex.water].intensity = _intensity;
                    mElementData[(int)ElementIndex.water].chance = _chance;
                    break;
                }
            case ElementalType.wood:
                {
                    mElementData[(int)ElementIndex.wood].potency = _potency;
                    mElementData[(int)ElementIndex.wood].intensity = _intensity;
                    mElementData[(int)ElementIndex.wood].chance = _chance;
                    break;
                }
            //Secondaries
            case ElementalType.wind: //Fire, Wood
                {
                    mElementData[(int)ElementIndex.wind].potency = _potency;
                    mElementData[(int)ElementIndex.wind].intensity = _intensity;
                    mElementData[(int)ElementIndex.wind].chance = _chance;
                    break;
                }
            case ElementalType.lightning:
                {
                    mElementData[(int)ElementIndex.lightning].potency = _potency;
                    mElementData[(int)ElementIndex.lightning].intensity = _intensity;
                    mElementData[(int)ElementIndex.lightning].chance = _chance;
                    break;
                }
            case ElementalType.voidType:
                {
                    mElementData[(int)ElementIndex.voidType].potency = _potency;
                    mElementData[(int)ElementIndex.voidType].intensity = 0;
                    mElementData[(int)ElementIndex.voidType].chance = 0; //voidType shouldnot have any effect, thus chance should be 0
                    break;
                }
        }
    }
    public void EvaluateElementValues()
    {
        //Check for wind
        if(mElementData[(int)ElementIndex.fire].potency > 0 &&
           mElementData[(int)ElementIndex.wood].potency > 0)
        {
            //wind += (fire + wood) / 2
            mElementData[(int)ElementIndex.wind].potency += (mElementData[(int)ElementIndex.fire].potency + mElementData[(int)ElementIndex.wind].potency) * 0.5f;
            mElementData[(int)ElementIndex.wind].chance += (mElementData[(int)ElementIndex.fire].chance + mElementData[(int)ElementIndex.wind].chance) * 0.5f;
            mElementData[(int)ElementIndex.wind].intensity += (mElementData[(int)ElementIndex.fire].intensity + mElementData[(int)ElementIndex.wind].intensity) * 0.5f;
            if (!inclusiveElement)
            {
                mElementData[(int)ElementIndex.fire].flag = true;
                mElementData[(int)ElementIndex.wood].flag = true;
            }
        }
        //Check for lightning
        if (mElementData[(int)ElementIndex.metal].potency > 0 &&
            mElementData[(int)ElementIndex.water].potency > 0)
        {
            //lightning += (metal + water) / 2
            mElementData[(int)ElementIndex.lightning].potency += (mElementData[(int)ElementIndex.metal].potency + mElementData[(int)ElementIndex.water].potency) * 0.5f;
            mElementData[(int)ElementIndex.lightning].chance += (mElementData[(int)ElementIndex.metal].chance + mElementData[(int)ElementIndex.water].chance) * 0.5f;
            mElementData[(int)ElementIndex.lightning].intensity += (mElementData[(int)ElementIndex.metal].intensity + mElementData[(int)ElementIndex.water].intensity) * 0.5f;
            if (!inclusiveElement)
            {
                mElementData[(int)ElementIndex.metal].flag = true;
                mElementData[(int)ElementIndex.water].flag = true;
            }
        }
    }
    // Start is called before the first frame update
    void Start() //initialize element data
    {
        mElementData = new ElementData[(int)ElementIndex.count];
        SetElementValues(ElementalType.fire, fire, fireEffectIntensity, fireEffectChance, false);
        SetElementValues(ElementalType.earth, earth, earthEffectIntensity, earthEffectChance, false);
        SetElementValues(ElementalType.metal, metal, metalEffectIntensity, metalEffectChance, false);
        SetElementValues(ElementalType.water, water, waterEffectIntensity, waterEffectChance, false);
        SetElementValues(ElementalType.wood, wood, woodEffectIntensity, woodEffectChance, false);
        SetElementValues(ElementalType.wind, wind, windEffectIntensity, windEffectChance, false);
        SetElementValues(ElementalType.lightning, lightning, lightningEffectIntensity, lightningEffectChance, false);
        SetElementValues(ElementalType.voidType, voidType, voidTypeIntensity, voidTypeEffectChance, false);

        if(!inclusiveElement)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
