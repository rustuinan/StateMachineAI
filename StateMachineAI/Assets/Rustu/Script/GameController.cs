using UnityEngine;

public class GameController : MonoBehaviour
{
    public int targetBoxScore = 10;
    public int currentBoxScore = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Box"))
        {
            Destroy(collision.gameObject);
            currentBoxScore++;
        }
    }

    private void Update()
    {
        FinishGame();
    }

    void FinishGame()
    {
        if (currentBoxScore >= targetBoxScore)
        {
            Debug.Log("Game Finished");
            Application.Quit();
        }
    }
}
