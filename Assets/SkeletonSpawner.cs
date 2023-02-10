using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{

    public Vector3[] spawnPositions;
    public Vector3[] spawnSmokePositions;
    public GameObject skeleton;
    public GameObject bigSkeleton;

    public GameObject spawnParticle;

    private GameObject skeleInst;

    public void Spawn()
    {
        int rand = Random.Range(0, spawnPositions.Length);

        skeleInst = Instantiate(skeleton, spawnPositions[rand], transform.rotation);
        Instantiate(spawnParticle, spawnSmokePositions[rand], transform.rotation);

        StartCoroutine(RiseFromGround(skeleInst));
    }

    public void SpawnAll()
    {
        for(int i = 0; i < spawnPositions.Length; i++)
        {
            if (i == 6)
            {
                skeleInst = Instantiate(bigSkeleton, spawnPositions[i], transform.rotation);
            }
            else
            {
                skeleInst = Instantiate(skeleton, spawnPositions[i], transform.rotation);
            }

            Instantiate(spawnParticle, spawnSmokePositions[i], transform.rotation);

            StartCoroutine(RiseFromGround(skeleInst));
        }
    }

    private IEnumerator RiseFromGround(GameObject skeleton)
    {
        Rigidbody2D rb2d = skeleton.GetComponent<Rigidbody2D>();

        rb2d.isKinematic = true;
        rb2d.velocity = Vector2.up;
        skeleton.GetComponent<CircleCollider2D>().enabled = false;

        yield return new WaitForSeconds(3f);

        rb2d.isKinematic = false;
        skeleton.transform.position = new Vector3(skeleton.transform.position.x, skeleton.transform.position.y, -1f);

        skeleton.GetComponent<CircleCollider2D>().enabled = true;
    }
}
