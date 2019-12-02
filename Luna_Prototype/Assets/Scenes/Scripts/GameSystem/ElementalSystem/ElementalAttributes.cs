using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalAttributes : MonoBehaviour, ElementalSystem
{
    public bool debugMode = false;
    public bool inclusiveElement = false;
    public bool initialized = false;
    //For weapons/enemy attacks, these are flat damages, 150 fire means the weapon will do 150 fire damage, NOT 1.5x fire damage
    //voidType is a damage type that ignores all enemental resistances target has and always deal the full damage assgned
    //voidType does not affect damage taken
    public float voidType =     0.0f;
    public float fire =         0.0f;
    public float earth =        0.0f;
    public float metal =        0.0f;
    public float water =        0.0f;
    public float wood =         0.0f;
    public float wind =         0.0f;
    public float lightning =    0.0f;

    //For enemies 100 means 1.00x damage is taken from that element
    //If above 100 enemy takes extra damage (example: 150 will result in enemy taking 1.50x from that element alone)
    //Like wise if below 100 enemy will take less damage (example: 50 will result in enemy taking 0.50x damage from that element)
    public float voidTypeRes    = 100.0f; //void resistances does not do anything, it's just here
    public float fireRes        = 100.0f;
    public float earthRes       = 100.0f;
    public float metalRes       = 100.0f;
    public float waterRes       = 100.0f;
    public float woodRes        = 100.0f;
    public float windRes        = 100.0f;
    public float lightningRes   = 100.0f;

    //Chance is the chance of element effect being applied
    //Chance that an effect will occur, 100 for 100% chance and 0 being 0% chance, will not have any effect above 100
    private float voidTypeEffectChance      = 100.0f;
    public float fireEffectChance           = 100.0f;
    public float earthEffectChance          = 100.0f;
    public float metalEffectChance          = 100.0f;
    public float waterEffectChance          = 100.0f;
    public float woodEffectChance           = 100.0f;
    public float windEffectChance           = 100.0f;
    public float lightningEffectChance      = 100.0f;

    //Intensity is the strength of the effect the element applies
    //The intensity is a flat value, not a multiplier
    private float voidTypeIntensity = 0.0f;
    public float fireEffectIntensity = 0.0f;
    public float earthEffectIntensity = 0.0f;
    public float metalEffectIntensity = 0.0f;
    public float waterEffectIntensity = 0.0f;
    public float woodEffectIntensity = 0.0f;
    public float windEffectIntensity = 0.0f;
    public float lightningEffectIntensity = 0.0f;


    //Effect duration, how long effects lasts
    private float voidTypeDuration = 0.0f;
    public float fireDuration = 0.0f;
    public float earthDuration = 0.0f;
    public float metalDuration = 0.0f;
    public float waterDuration = 0.0f;
    public float woodDuration = 0.0f;
    public float windDuration = 0.0f;
    public float lightningDuration = 0.0f;

    public Player mPlayer { get; private set; }
    public Enemy mEnemy { get; private set; }
    //public ElementData dfire;

    public Elemental[] mElement = new Elemental[(int)ElementIndex.count];

    private int Indexer(ElementalType type)
    {
        //int i = (int)type;
        switch(type)
        {
            case ElementalType.none: { return -999; }
            case ElementalType.voidType: { return (int)ElementIndex.voidType; }
            case ElementalType.fire: { return (int)ElementIndex.fire; }
            case ElementalType.earth: { return (int)ElementIndex.earth; }
            case ElementalType.metal: { return (int)ElementIndex.metal; }
            case ElementalType.water: { return (int)ElementIndex.water; }
            case ElementalType.wood: { return (int)ElementIndex.wood; }
            case ElementalType.wind: { return (int)ElementIndex.wind; }
            case ElementalType.lightning: { return (int)ElementIndex.lightning; }
        }
        return -999;
    }
    private ElementalType Indexer(int i)
    {
        //int i = (int)type;
        if(i < 0){return ElementalType.none;}
        switch (i)
        {
            case 0: { return ElementalType.voidType; }
            case 1: { return ElementalType.fire; }
            case 2: { return ElementalType.earth; }
            case 3: { return ElementalType.metal; }
            case 4: { return ElementalType.water; }
            case 5: { return ElementalType.wood; }
            case 6: { return ElementalType.wind; }
            case 7: { return ElementalType.lightning; }
        }
        return ElementalType.none;
    }
    public void EvaluateElementValues()
    {
        //to do, if player have elements that can be combined into secondary element, calculate for new element values
    }

    void Start() //initialize element data, append this to add new elements, do not forget to add new enums in ElementalSystem.cs
    {
        mPlayer = GetComponent<Player>();
        mEnemy = GetComponent<Enemy>();

        mElement[Indexer(ElementalType.voidType)] = new VoidType();
        mElement[Indexer(ElementalType.voidType)].SetElement(ElementalType.voidType, ElementIndex.voidType, voidType, voidTypeRes, voidTypeDuration, voidTypeEffectChance, voidTypeIntensity, false, this);
        
        mElement[Indexer(ElementalType.fire)] = new Fire();
        mElement[Indexer(ElementalType.fire)].SetElement(ElementalType.fire, ElementIndex.fire, fire, fireRes, fireDuration, fireEffectChance, fireEffectIntensity, false, this);

        mElement[Indexer(ElementalType.earth)] = new Earth();
        mElement[Indexer(ElementalType.earth)].SetElement(ElementalType.earth, ElementIndex.earth, earth, earthRes, earthDuration, earthEffectChance, earthEffectIntensity, false, this);

        mElement[Indexer(ElementalType.metal)] = new Metal();
        mElement[Indexer(ElementalType.metal)].SetElement(ElementalType.metal, ElementIndex.metal, metal, metalRes, metalDuration, metalEffectChance, metalEffectIntensity, false, this);

        mElement[Indexer(ElementalType.water)] = new Water();
        mElement[Indexer(ElementalType.water)].SetElement(ElementalType.water, ElementIndex.water, water, waterRes, waterDuration, waterEffectChance, waterEffectIntensity, false, this);

        mElement[Indexer(ElementalType.wood)] = new Wood();
        mElement[Indexer(ElementalType.wood)].SetElement(ElementalType.wood, ElementIndex.wood, wood, woodRes, woodDuration, woodEffectChance, woodEffectIntensity, false, this);

        mElement[Indexer(ElementalType.wind)] = new Wind();
        mElement[Indexer(ElementalType.wind)].SetElement(ElementalType.wind, ElementIndex.wind, wind, windRes, windDuration, windEffectChance, windEffectIntensity, false, this);

        mElement[Indexer(ElementalType.lightning)] = new Lightning();
        mElement[Indexer(ElementalType.lightning)].SetElement(ElementalType.lightning, ElementIndex.lightning, lightning, lightningRes, lightningDuration, lightningEffectChance, lightningEffectIntensity, false, this);
    }

    void Update() //Should not be touched unless you know what you are doing
    {
        if (!initialized)
        {
            initialized = true;
        }
        else
        {
            for (int i = 0; i < mElement.Length; i++)
            {
                if (mElement[i] != null)
                {
                    mElement[i].ElementalUpdate(); //Update elemental effects, including counting down on it's duration
                }
            }
        }
    }

    private void OnMouseDown() //Debug Mode only
    {
        if (debugMode)
        {
            if (mPlayer != null)
            {
                for (int i = 0; i < mElement.Length; i++)
                {
                    switch(i)
                    {
                        case (int)ElementIndex.wind:
                        {
                                mElement[i].ApplyElementalDamage(mPlayer, mElement[i].potency, false);
                                //Apply status only to player
                                break;
                        }
                        default:
                        {
                                mElement[i].ApplyElementalDamage(mPlayer, mElement[i].potency, true);
                                break;
                        }

                    }

                    if (i == (int)ElementIndex.wind) { mElement[i].ApplyElementalDamage(mPlayer, mElement[i].potency, true); }
                    else { mElement[i].ApplyElementalDamage(mPlayer, mElement[i].potency, true); }
                }
            }
            if (mEnemy != null)
            {
                for (int i = 0; i < mElement.Length; i++)
                {
                    mElement[i].ApplyElementalDamage(mEnemy, mElement[i].potency, true);
                }
            }
        }
    }
}
