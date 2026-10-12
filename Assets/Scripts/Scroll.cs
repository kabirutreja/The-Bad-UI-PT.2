using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float scrollSpeed = 0.5f;

    void Update()
    {
        transform.position += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);
    }
}
