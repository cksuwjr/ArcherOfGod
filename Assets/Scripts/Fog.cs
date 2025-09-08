using UnityEngine;

public class Fog : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Dissappear", 5);
    }

    private void Dissappear()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (collision.TryGetComponent<Status>(out var status))
                status.MoveSpeed = 1.5f;
            if (collision.TryGetComponent<PlayerController>(out var cont))
                cont.NarrowSight(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.TryGetComponent<Status>(out var status))
                status.MoveSpeed = 3.5f;
            if (collision.TryGetComponent<PlayerController>(out var cont))
                cont.NarrowSight(false);
        }
    }
}
