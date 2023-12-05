using System.Collections;
using System.Collections.Generic;
using UnityEngine;

private MeshCollider meshCollider;

public class Collider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        meshCollider.sharedMesh = mesh;
    }
}
