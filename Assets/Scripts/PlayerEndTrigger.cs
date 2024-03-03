using UnityEngine;

public class PlayerEndTrigger : MonoBehaviour
{
    GameManager gameManager;

    public void InitTrigger()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other) {
        print("PO");
        if (other.gameObject.tag == "Player")
        {
            print("YO");
            gameManager.Reset();
        }
    }
}
