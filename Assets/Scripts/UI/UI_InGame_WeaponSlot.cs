using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_InGame_WeaponSlot : MonoBehaviour
{
    private Image _currWeapon;
    private Image _secWeapon;
    [SerializeField] private Sprite _emptySprite;

    private void Awake()
    {

        //Transform tf1 = transform.Find("weapon_slots");
        //Transform tf2 = tf1.Find("current_weapon");
        //Transform tf3 = tf2.Find("weapon_img");

        _currWeapon = transform.Find("current_weapon").Find("weapon_img").GetComponent<Image>();
        _secWeapon = transform.Find("second_weapon").Find("weapon_img").GetComponent<Image>();
    }
    //private Player _player;
    void Start()
    {
        //if (_player.CurrentWeapon != null)
        //{
        //    _currWeapon =
        //    _player.CurrentWeapon.transform.Find("WeaponSprite").GetComponent<SpriteRenderer>().sprite;
        //}
        
    }

    public void UI_Ingame_UpdateWeaponSprite(Sprite currWeapon, Sprite secWeapon)
    {
        _currWeapon.sprite = currWeapon ? currWeapon : _emptySprite;
        _secWeapon.sprite = secWeapon ? secWeapon : _emptySprite;
    }
 
}
