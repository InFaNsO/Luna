using UnityEngine;

public class OpenGate : MonoBehaviour
{
    [SerializeField]
    private Animator animController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            Debug.Log("collided gate");
            animController.SetBool("Trigger1", true);
        }
    }

}
