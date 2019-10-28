using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvObj_Lever : MonoBehaviour, EnvironmentalObject
{
    private Player plyr;
    public GameObject interactTarget;
    private EnvironmentalObject mTarget;
    public Sprite _spriteOn;
    public Sprite _spriteOff;
    private Sprite _currSprite;
    public string _name = "lever";
    public bool collidePlayer = false;
    public bool isTwoWay = false;
    public leverState flipped = leverState.off;

    SpriteRenderer mSprite;

    public enum leverState
    {
        NULL = -1,
        off,
        on,
    }
    public string GetObjectName()
    {
        return _name;
    }

    public Sprite GetSprite()
    {
        return _currSprite;
    }

    public EnvironmentObjType GetTypeEnum()
    {
        return EnvironmentObjType.lever;
    }

    public void interact(ref Player thePlayer)
    {
        switch(flipped)
        {
            case leverState.off: //The lever is currently off
                {
                    flipped = leverState.on;
                    mTarget.interact(ref thePlayer);
                    break;
                }
            case leverState.on: //The lever is currently on
                {
                    if (isTwoWay)
                    {
                        flipped = leverState.off;
                        mTarget.interact(ref thePlayer);
                    }
                    break;
                }
            case leverState.NULL:
                {
                    break;
                }
            default:
                {
                    break;
                }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mTarget = interactTarget.GetComponent<EnvironmentalObject>();
        mSprite = this.GetComponent<SpriteRenderer>();
        mSprite.sprite = _spriteOff;
    }

    // Update is called once per frame
    void Update()
    {
        //Sprite selector, do not touch unless you know what you are doing
        if(flipped == leverState.on)
        {
            mSprite.sprite = _spriteOn;
        }
        else if(flipped == leverState.off)
        {
            mSprite.sprite = _spriteOff;
        }
        else
        {
            Debug.Log("Interact Target does not exist");
        }
        _currSprite = mSprite.sprite;
        //end of sprite selector

        if (Input.GetKeyDown(KeyCode.E) && collidePlayer)
        {
            interact(ref plyr);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            collidePlayer = true;
            plyr = other.gameObject.GetComponent<Player>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collidePlayer = false;
    }
}
