using UnityEngine;

//[System.Serializable]
public class Card : MonoBehaviour
{
    [SerializeField]
    private bool clicked;

    public Card(bool clicked)
    {
        this.clicked = clicked;
    }
    public bool GetClicked() { return clicked; }
    public void SetClicked(bool clicked) { this.clicked = clicked; }
}