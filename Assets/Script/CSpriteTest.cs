using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class CSpriteTest : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteAtlas idleAtlas;

    private void Awake()
    {

    }

    private void Start()
    {
        Sprite[] sprites = new Sprite[idleAtlas.spriteCount];
        idleAtlas.GetSprites(sprites);
    }

    private void Update()
    {
        
    }

}
