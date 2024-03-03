using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        print("PO");
        if (other.gameObject.tag == "Player")
        {
            print("YO");
            SceneManager.LoadScene("EndScene");
        }
    }
}
