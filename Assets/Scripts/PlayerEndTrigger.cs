using UnityEngine;

public class PlayerEndTrigger : MonoBehaviour
{
    GameManager gameManager;

    public void InitTrigger()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            gameManager.Reset();
        }
    }
}
