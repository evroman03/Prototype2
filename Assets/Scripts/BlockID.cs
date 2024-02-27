using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockID : MonoBehaviour
{
    public Vector3 location;
    public enum BlockType
    {
        Hole, Ground, OneBlock, TwoBlock, StartBlock, FinishBlock
    }
    public BlockType Type;
    void Start()
    {
        location = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
    }
    void Update()
    {
        
    }
}
