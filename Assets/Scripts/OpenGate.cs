using UnityEngine;

public class OpenGate : MonoBehaviour
{
    [SerializeField]
    private Animator animController;
    public AudioSource opening;

    void Start()
    {
        opening = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            Debug.Log("collided gate");
            animController.SetBool("Trigger1", true);
            opening.Play();
        }
    }

}
