using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxTrigger : MonoBehaviour
{
    public Sprite sprite;
    public string text;
    public float duration_stay;
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
         Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            ServiceLocator.Get<UIManager>().PopUpMessageBox(duration_stay,text, sprite);
            StartCoroutine("DestoryMyself");
        }
    }
    private IEnumerator DestoryMyself()
    {
        yield return new WaitForSeconds(duration_stay);
        Destroy(this.gameObject);
    }
}
