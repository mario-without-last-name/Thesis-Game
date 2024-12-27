using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExampleMovePiece : MonoBehaviour
{
    public GameObject examplePiece;
    
    // Start is called before the first frame update
    void Start()
    {
        examplePiece.transform.DOMove(new Vector3(-2, 4, examplePiece.transform.position.z), 0.5f);
        // Somehow the Z increases 10 on it's own? Somehow is affected the camera Z position
    }
}
