using UnityEngine;

public class MoveElevator : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            Debug.Log("collided elevator");
            anim.SetBool("Trigger1", true);
        }
    }
}
