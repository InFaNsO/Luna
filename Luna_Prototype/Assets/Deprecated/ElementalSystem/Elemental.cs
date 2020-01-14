using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Elemental
{
    public ElementalAttributes mElementalAttribute;
    //Elemental Data for the gameObject that's running this component
    public ElementalType type;
    public int i;
    public float potency;
    public float resistance;
    public float duration;
    public float procChance;
    public float intensityStatus;
    public bool immune = false;
    public bool flag = false;
    private GameObject other; 

    //Data for currently active status ailments (buffs/debuffs), these values are set by other classes as they apply status ailments to this object
    public bool mStatusActive = false;
    public float mActiveDuration = 0;
    private float mActiveIntensity = 0;
    public float mCount = 0;

    public void OnHit() //This is called whenever the player hits a valid target, intended for adding particle codes
    {
        switch (type)
        {
            case ElementalType.fire:
                break;
            case ElementalType.earth:
                break;
            case ElementalType.metal:
                break;
            case ElementalType.water:
                break;
            case ElementalType.wind:
                break;
            case ElementalType.wood:
                break;
            case ElementalType.lightning:
                break;
            default:
                break;
        }
    }

    public void SetElement(ElementalType _type, ElementIndex _index, float _potency, float _resistance, float _duration, float _procChance, float _intensityStatus, bool _flag, ElementalAttributes mElementalAttributes)
    {
        type = _type;
        i = (int)_index;
        potency = _potency;
        resistance = _resistance;
        duration = _duration;
        procChance = _procChance;
        intensityStatus = _intensityStatus;
        flag = _flag;
        mElementalAttribute = mElementalAttributes;
    }
    public float GetActiveIntensity() { return mActiveIntensity; }
    public void ApplyStatusEffect(ref Enemy target) //Apply status to target - Enemy overload
    {
        ElementalAttributes t = target.GetComponent<ElementalAttributes>();
        if (t != null)
        {
            float c = Random.Range(0.0f, 100.0f);
            if (c <= procChance)
            {
                t.mElement[i].mSetActiveEffect(intensityStatus, duration);
            }
        }
    }
    public void ApplyStatusEffect(ref Player target) //Apply status to target - Player overload
    {
        ElementalAttributes t = target.GetComponent<ElementalAttributes>();
        if (t != null)
        {
            float c = Random.Range(0.0f, 100.0f);
            if (c <= procChance)
            {
                t.mElement[i].mSetActiveEffect(intensityStatus, duration);
            }
        }
    }
    public void ApplyStatusEffect(ref Character target) //Apply status to target - Character overload
    {
        ElementalAttributes t = target.GetComponent<ElementalAttributes>();
        if (t != null)
        {
            float c = Random.Range(0.0f, 100.0f);
            if (c <= procChance)
            {
                t.mElement[i].mSetActiveEffect(intensityStatus, duration);
            }
        }
    }
    public void ApplyStatusEffectSelf() //Apply status to self
    {
        float c = Random.Range(0.0f, 100.0f);
        if (c <= procChance)
        {
            mSetActiveEffect(intensityStatus, duration);
        }
    }
    public void ApplyElementalDamage(Enemy target, bool applyStatus) //Do elemental damage to target based on potency - Enemy overload
    {
        ElementalAttributes t = target.GetComponent<ElementalAttributes>();
        if (t != null)
        {
            target.GetHit(potency * (t.mElement[i].resistance / 100.0f));
            OnHit();
            if(applyStatus)
            {
                ApplyStatusEffect(ref target);
            }
        }
    }
    public void ApplyElementalDamage(Player target, bool applyStatus) //Do elemental damage to target based on potency - Player overload
    {
        ElementalAttributes t = target.GetComponent<ElementalAttributes>();
        if (t != null)
        {
            target.GetHit(potency * (t.mElement[i].resistance / 100.0f));
            OnHit();
            if(applyStatus)
            {
                ApplyStatusEffect(ref target);
            }
        }
    }
    public void ApplyElementalDamage(Character target, bool applyStatus) //Do elemental damage to target based on potency - Character overload
    {
        ElementalAttributes t = target.GetComponent<ElementalAttributes>();
        if (t != null)
        {
            target.GetHit(potency * (t.mElement[i].resistance / 100.0f));
            OnHit();
            if (applyStatus)
            {
                ApplyStatusEffect(ref target);
            }
        }
    }
    public abstract void EffectTick();
    public void mSetActiveEffect(float _intensity, float _duration) //Set data for currently active status effect
    {
        mActiveIntensity = _intensity;
        //mActiveDuration = duration;
        mCount = _duration;
        mStatusActive = true;
    }
    public void TakeElementalDamage(float dmg)
    {
        if(mElementalAttribute.mPlayer != null)
        {
            Debug.Log("Player Elemental damage taken");
            mElementalAttribute.mPlayer.mCurrentHealth -= dmg * (resistance / 100.0f);
        }
        if(mElementalAttribute.mEnemy != null)
        {
            Debug.Log("Enemy Elemental damage taken");
            mElementalAttribute.mEnemy.mCurrentHealth -= dmg * (resistance / 100.0f);
        }
    } //Mainly for debug only, do not use unless you know what you are doing
    public ElementalAttributes GetAttribute()
    {
        if (mElementalAttribute != null)
        {
            return mElementalAttribute;
        }
        else
        {
            return null;
        }
    }
    public Player GetPlayer()
    {
        if(mElementalAttribute.mPlayer != null)
        {
            return mElementalAttribute.mPlayer;
        }
        else
        {
            return null;
        }
    }
    public Enemy GetEnemy()
    {
        if(mElementalAttribute.mEnemy != null)
        {
            return mElementalAttribute.mEnemy;
        }
        else
        {
            return null;
        }
    }
    public void SetOther(GameObject t)
    {
        other = t;
    }
    public GameObject GetOther() { return other; }
    public void mSetImmune(bool isImmune) { immune = isImmune; }
    public void ElementalUpdate()
    {
        if(mCount <= 0)
        {
            mCount = 0;
            mStatusActive = false;
        }
        if(mStatusActive && !immune && mCount > 0)
        {
            EffectTick();
        }
        //EffectTick();
    }
    public Elemental CopyStats(Elemental destination, Elemental src) //Copy src stats into destination
    {
        destination.potency = src.potency;
        destination.resistance = src.resistance;
        destination.duration = src.duration;
        destination.procChance = src.procChance;
        destination.intensityStatus = src.intensityStatus;
        return destination;

    }
    public static Elemental operator+(Elemental a, Elemental b)
    {
        VoidType c = new VoidType();
        c.potency = a.potency + b.potency;
        c.resistance = a.resistance + b.resistance;
        c.duration = a.duration + b.duration;
        c.procChance = a.procChance + b.procChance;
        c.intensityStatus = a.intensityStatus + b.intensityStatus;
        return c;
    }
    public static Elemental operator -(Elemental a, Elemental b)
    {
        VoidType c = new VoidType();
        c.potency = a.potency - b.potency;
        c.resistance = a.resistance - b.resistance;
        c.duration = a.duration - b.duration;
        c.procChance = a.procChance - b.procChance;
        c.intensityStatus = a.intensityStatus - b.intensityStatus;
        return c;
    }
    public static Elemental operator *(Elemental a, Elemental b)
    {
        VoidType c = new VoidType();
        c.potency = a.potency * b.potency;
        c.resistance = a.resistance * b.resistance;
        c.duration = a.duration * b.duration;
        c.procChance = a.procChance * b.procChance;
        c.intensityStatus = a.intensityStatus * b.intensityStatus;
        return c;
    }
    public static Elemental operator /(Elemental a, Elemental b)
    {
        VoidType c = new VoidType();
        c.potency = a.potency / b.potency;
        c.resistance = a.resistance / b.resistance;
        c.duration = a.duration / b.duration;
        c.procChance = a.procChance / b.procChance;
        c.intensityStatus = a.intensityStatus / b.intensityStatus;
        return c;
    }
}
