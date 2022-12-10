using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> spawnPointList;
    private List<Character> spawnCharacters;
    private bool hasSpawned;
    public Collider _collider;
    public UnityEvent onAllSpawnedCharEliminated;

    private void Awake()
    {
        var spawnPointsArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        spawnPointList = new List<SpawnPoint>(spawnPointsArray);
        spawnCharacters = new List<Character>();
    }

    private void Update()
    {
        if (!hasSpawned || spawnCharacters.Count == 0)
            return;

        bool allSpawnedAreDead = true;

        foreach (Character c in spawnCharacters)
        {
            if (c.currentState != Character.CharacterState.Dead)
            {
                allSpawnedAreDead = false;
                break;
            }
        }

        if (allSpawnedAreDead)
        {
            if(onAllSpawnedCharEliminated != null)
                onAllSpawnedCharEliminated.Invoke();

            spawnCharacters.Clear();
        }
    }

    public void SpawnCharacters()
    {
        if (hasSpawned)
            return;

        hasSpawned = true;

        foreach (SpawnPoint point in spawnPointList)
        {
            if (point.EnemyToSpan != null)
            {
                GameObject spawnedGameObject = Instantiate(point.EnemyToSpan, point.transform.position, Quaternion.identity);
                spawnCharacters.Add(spawnedGameObject.GetComponent<Character>());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SpawnCharacters();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _collider.bounds.size);
    }
}