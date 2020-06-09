using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalAttributes : MonoBehaviour, ElementalSystem
{
    public bool debugMode = false;
    public bool inclusiveElement = false;
    public bool initialized = false;
    public bool isEquipment = false;
    //For weapons/enemy attacks, these are flat damages, 150 fire means the weapon will do 150 fire damage, NOT 1.5x fire damage
    //voidType is a damage type that ignores all enemental resistances target has and always deal the full damage assgned
    //voidType does not affect damage taken
    public float voidType =     0.0f;
    [SerializeField] public float fire =         0.0f;
    [SerializeField] public float earth =        0.0f;
    [SerializeField] public float metal =        0.0f;
    [SerializeField] public float water =        0.0f;
    [SerializeField] public float wood =         0.0f;
    [SerializeField] public float wind =         0.0f;
    [SerializeField] public float lightning =    0.0f;

    //For enemies 100 means 1.00x damage is taken from that element
    //If above 100 enemy takes extra damage (example: 150 will result in enemy taking 1.50x from that element alone)
    //Like wise if below 100 enemy will take less damage (example: 50 will result in enemy taking 0.50x damage from that element)
    private float voidTypeRes    = 100.0f; //void resistances does not do anything, it's just here
    [SerializeField] public float fireRes        = 100.0f;
    [SerializeField] public float earthRes       = 100.0f;
    [SerializeField] public float metalRes       = 100.0f;
    [SerializeField] public float waterRes       = 100.0f;
    [SerializeField] public float woodRes        = 100.0f;
    [SerializeField] public float windRes        = 100.0f;
    [SerializeField] public float lightningRes   = 100.0f;

    //Chance is the chance of element effect being applied
    //Chance that an effect will occur, 100 for 100% chance and 0 being 0% chance, will not have any effect above 100
    private float voidTypeEffectChance      = 0.0f;
    [SerializeField] public float fireEffectChance           = 100.0f;
    [SerializeField] public float earthEffectChance          = 100.0f;
    [SerializeField] public float metalEffectChance          = 100.0f;
    [SerializeField] public float waterEffectChance          = 100.0f;
    [SerializeField] public float woodEffectChance           = 100.0f;
    [SerializeField] public float windEffectChance           = 100.0f;
    [SerializeField] public float lightningEffectChance      = 100.0f;

    //Intensity is the strength of the effect the element applies
    //The intensity is a flat value, not a multiplier
    private float voidTypeIntensity = 0.0f;
    [SerializeField] public float fireEffectIntensity = 0.0f;
    [SerializeField] public float earthEffectIntensity = 0.0f;
    [SerializeField] public float metalEffectIntensity = 0.0f;
    [SerializeField] public float waterEffectIntensity = 0.0f;
    [SerializeField] public float woodEffectIntensity = 0.0f;
    [SerializeField] public float windEffectIntensity = 0.0f;
    [SerializeField] public float lightningEffectIntensity = 0.0f;


    //Effect duration, how long effects lasts
    private float voidTypeDuration = 0.0f;
    [SerializeField] public float fireDuration = 0.0f;
    [SerializeField] public float earthDuration = 0.0f;
    [SerializeField] public float metalDuration = 0.0f;
    [SerializeField] public float waterDuration = 0.0f;
    [SerializeField] public float woodDuration = 0.0f;
    [SerializeField] public float windDuration = 0.0f;
    [SerializeField] public float lightningDuration = 0.0f;

    //Debug variables, please ignore
    //private float voidTypeTemp = 0.0f;
    //private float fireTemp = 0.0f;
    //private float earthTemp = 0.0f;
    //private float metalTemp = 0.0f;
    //private float waterTemp = 0.0f;
    //private float woodTemp = 0.0f;
    //private float windTemp = 0.0f;
    //private float lightningTemp = 0.0f;

    public Player mPlayer;
    public Enemy mEnemy;

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
    

    public void ApplyDamage(ref Enemy t, bool applyStatus) //Most basic form of applying elemental damage, it takes all elemental stats into calculation
    {
        if (t != null)
        {
            for (int i = 0; i < mElement.Length; i++)
            {
                switch (i)
                {
                    case (int)ElementIndex.wind: //Wind gives a buff to the use, therefore should not apply status to target
                        {
                            mElement[i].ApplyElementalDamage(t, false); //do damage to t without applying status
                            mElement[i].ApplyStatusEffectSelf();
                            break;
                        }
                    default:
                        {
                            mElement[i].ApplyElementalDamage(t, applyStatus);
                            break;
                        }

                }
            }
        }
    }
    public void ApplyDamage(ref Player t, bool applyStatus)//Most basic form of applying elemental damage, it takes all elemental stats into calculation
    {
        if (t != null)
        {
            for (int i = 0; i < mElement.Length; i++)
            {
                switch (i)
                {
                    case (int)ElementIndex.wind: //Wind gives a buff to the user, therefore should not apply status to target
                        {
                            mElement[i].ApplyElementalDamage(t, false); //do damage to t without applying status
                            mElement[i].ApplyStatusEffectSelf();
                            break;
                        }
                    default:
                        {
                            mElement[i].ApplyElementalDamage(t, applyStatus);
                            break;
                        }

                }
            }
        }
    }
    public void ApplyDamage(Character t, bool applyStatus)//Most basic form of applying elemental damage, it takes all elemental stats into calculation
    {
        if (t != null)
        {
            for (int i = 0; i < mElement.Length; i++)
            {
                switch (i)
                {
                    case (int)ElementIndex.wind: //Wind gives a buff to the user, therefore should not apply status to target
                        {
                            mElement[i].ApplyElementalDamage(t, false); //do damage to t without applying status
                            if (mPlayer != null)
                            {
                                ServiceLocator.Get<UIManager>().UpdateHPGauge(mPlayer.myHealth.GetHealth() / GetComponent<Player>().myHealth.GetMaxHealth());
                            }
                            mElement[i].ApplyStatusEffectSelf(); //Apply buff to self
                            break;
                        }
                    default:
                        {
                            mElement[i].ApplyElementalDamage(t, applyStatus);
                            if (mPlayer != null)
                            {
                                ServiceLocator.Get<UIManager>().UpdateHPGauge(mPlayer.myHealth.GetHealth() / GetComponent<Player>().myHealth.GetMaxHealth());
                            }
                            break;
                        }

                }
            }
        }
    }
    public void EvaluateElementValues()
    {
        //to do, if player have elements that can be combined into secondary element, calculate for new element values
        if(inclusiveElement)
        {
            if(water > 0 && metal > 0) //lightning
            {
                lightning += water + metal;
                lightningDuration += waterDuration + metalDuration;
                lightningEffectIntensity += waterEffectIntensity + metalEffectIntensity;
                lightningEffectChance = (lightningEffectChance + waterEffectChance + metalEffectChance) / 3;
            }
            if (fire > 0 && wood > 0) //wind
            {
                wind += fire + wood;
                windDuration += fireDuration + woodDuration;
                windEffectIntensity += fireEffectIntensity + woodEffectIntensity;
                windEffectChance = (windEffectChance + fireEffectChance + woodEffectChance) / 3;
            }
        }
        else
        {
            if (water > 0 && metal > 0) //lightning
            {
                lightning += water + metal;
                lightningDuration += waterDuration + metalDuration;
                lightningEffectIntensity += waterEffectIntensity + metalEffectIntensity;
                lightningEffectChance = (lightningEffectChance + waterEffectChance + metalEffectChance) / 3;

                water = 0;
                metal = 0;
                waterEffectChance = 0;
                metalEffectChance = 0;
            }
            if (fire > 0 && wood > 0) //wind
            {
                wind += fire + wood;
                windDuration += fireDuration + woodDuration;
                windEffectIntensity += fireEffectIntensity + woodEffectIntensity;
                windEffectChance = (windEffectChance + fireEffectChance + woodEffectChance) / 3;

                fire = 0;
                wood = 0;
                fireEffectChance = 0;
                woodEffectChance = 0;
            }
        }
    }
    public void RefreshStats()
    {
        EvaluateElementValues();

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
    public ElementalAttributes SumStats(ElementalAttributes mEle, ElementalAttributes tEle) //return the sum of tEle stats and mEle stats
    {
        ElementalAttributes ele = new ElementalAttributes();
        for (int i = 0; i < mElement.Length; i++)
        {
            ele.mElement[i] = mEle.mElement[i] + tEle.mElement[i];
        }
        return ele;
    }
    public ElementalAttributes SubStats(ElementalAttributes mEle, ElementalAttributes tEle) //returns mEle - tEle
    {
        ElementalAttributes ele = new ElementalAttributes();
        for (int i = 0; i < mElement.Length; i++)
        {
            ele.mElement[i] = mEle.mElement[i] - tEle.mElement[i];
        }
        return ele;
    }
    public static ElementalAttributes operator+(ElementalAttributes a, ElementalAttributes b)
    {
        ElementalAttributes c = new ElementalAttributes();
        c = c.SumStats(a, b);
        return c;
    }
    public static ElementalAttributes operator-(ElementalAttributes a, ElementalAttributes b)
    {
        ElementalAttributes c = new ElementalAttributes();
        c = c.SubStats(a, b);
        return c;
    }
    void Awake() //initialize element data, append this to add new elements, do not forget to add new enums in ElementalSystem.cs
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

        //if (debugMode)
        //{
        //    if (mPlayer != null)
        //    {
        //        ApplyDamage(ref mPlayer, true);
        //    }
        //    if (mEnemy != null)
        //    {
        //        ApplyDamage(ref mEnemy, true);
        //    }
        //}
    }

    //public void Randomize()
    //{

    //}

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(tag != "Hazard")
    //    {
    //        return;
    //    }

    //    var c = collision.GetComponent<Player>();
    //    if (c == null)
    //    {
    //        c = collision.GetComponentInParent<Player>();
    //        if(c == null)
    //            return;
    //    }

    //    ApplyDamage(c, true);
    //}
}
