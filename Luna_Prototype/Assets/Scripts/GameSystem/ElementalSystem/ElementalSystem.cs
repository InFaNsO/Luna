using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum ElementalType
 {
     none = -999,
     fire = -11,
     earth = -7,
     metal = -5,
     water = -3,
     wood = -2,
     voidType = 0, //Ignores elemental resistances
     wind = ElementalType.fire * ElementalType.wood,
     lightning = ElementalType.water * ElementalType.metal,

 }

public enum ElementIndex //for parsing into an array for later calculations, like evaluate final element values
{
    voidType = 0,
    fire = 1,
    earth,
    metal,
    water,
    wood,
    wind,
    lightning,
    count
}
public interface ElementalSystem
{
    void ApplyElementalDamage(ref Enemy target, ElementalType[] element, float damage);
    void ApplyElementalEffect(ref Enemy target, ElementalType[] element, float intensity, float chance);
    void ApplyElementalEffect(ref Player target, ElementalType[] element, float intensity, float chance);
//    ElementalType[] EvaluateElementIntensity(ElementalType[] element);
}
