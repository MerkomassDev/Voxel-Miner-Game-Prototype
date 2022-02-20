using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float cubeSize = 0.25f;
    public int cubesInRow = 4;

    private float _cubesPivotDistance;
    private Vector3 _cubesPivot;

    public Vector3 smallcubeposition;
    //public float spacingOffset = 0.25f;
    //public Vector3 gridOrigin = Vector3.zero;

    // Start is called before the first frame update
    //sets the size of the cubes in Explode()
    private void Start()
    {
        _cubesPivotDistance = cubeSize * cubesInRow / 2;
        _cubesPivot = new Vector3(_cubesPivotDistance, _cubesPivotDistance, _cubesPivotDistance);
    }

    // calls Explode() when colliding with object which has a "StoneDestroy" tag
    //needs complete rework
    private void OnCollisionEnter(Collision stoneDestroyCollision)
    {
        if (stoneDestroyCollision.gameObject.CompareTag("StoneDestroy"))
        {
            Debug.Log(GetComponent<Collider>());
            Explode();
        }
    }

    //is called when colliding with object which has a "StoneDestroy" tag
    //should form a cube from smaller cubes
    
    public void Explode()
    {
        gameObject.SetActive(false);

        for (int x = 0; x < cubesInRow; x++)
        {
            for (int y = 0; y < cubesInRow; y++)
            {
                for (int z = 0; z < cubesInRow; z++)
                {
                    smallcubeposition = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - _cubesPivot;
                    StonePooler.Instance.SpawnFromPool("Cube", smallcubeposition, Quaternion.identity);
                    //Vector3 spawnPosition = new Vector3(x * spacingOffset, y * spacingOffset, z * spacingOffset) +
                                            //gridOrigin;
                    //CreatePiece(x, y, z);
                }
            }
        }
    }

    //code, which was used before i started implementing object pooling
    public void CreatePiece(int x, int y, int z)
    {
        GameObject smallcube;
        smallcube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        smallcube.transform.position =
            transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z) - _cubesPivot;
        smallcube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
        
        smallcube.AddComponent<Rigidbody>();
        smallcube.GetComponent<Rigidbody>().mass = cubeSize;
        
    }
}
