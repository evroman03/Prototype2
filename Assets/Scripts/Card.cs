using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private bool clicked;

    public Card(bool clicked)
    {
        this.clicked = clicked;
    }
    public bool getClicked() { return clicked; }
    public void setClicked(bool clicked) { this.clicked = clicked; }
}
