using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockID : MonoBehaviour
{
    public Vector3 location;
    public float height;
    public enum BlockType
    {
        Hole, Ground, OneBlock, TwoBlock, StartBlock, FinishBlock
    }
    public BlockType Type;
    void Start()
    {
        location = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
        switch(Type)
        {
            case BlockType.Hole:
                height = 0.75f;
                break;
            case BlockType.Ground:
                height = 0.75f;
                break;
            case BlockType.OneBlock:
                height = 1.75f;
                break;
            case BlockType.TwoBlock:
                height = 2.75f;
                break;
            case BlockType.StartBlock:
                height = 0.75f;
                break;
            case BlockType.FinishBlock:
                height = 0.75f;
                break;
        }
    }
}
