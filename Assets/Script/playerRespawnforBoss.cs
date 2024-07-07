using UnityEngine;

public class playerRespawnforBoss : MonoBehaviour
{
    private Transform currentCheckpoint;
    private HealthforBoss playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<HealthforBoss>();
    }

    public void Respawn()
    {

        playerHealth.Respawn();
        transform.position = currentCheckpoint.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
        }
    }
}
