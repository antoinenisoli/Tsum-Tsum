using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [Serializable]
    class SpawnRate
    {
        public GameObject prefab;
        [Range(0,1)]
        public float rate = 0.5f;
    }

    Selections selections => FindObjectOfType<Selections>();
    [SerializeField] Vector2 dimensions = new Vector2(5, 5);
    [SerializeField] Vector2 offsetRandomRange = new Vector2(-1, 1);
    [SerializeField] SpawnRate[] spawns;
    int index;
    System.Random rdm = new System.Random();   

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(dimensions.x/2, dimensions.y/2), dimensions);
    }

    private void Start()
    {
        EventManager.Instance.onNewPet.AddListener(CreatePet);
        EventManager.Instance.onNewGame.AddListener(NewGame);
    }

    public float RandomFloat(float min, float max)
    {
        return (float)rdm.NextDouble() * (max - min) + min;
    }

    public void CreatePet(float delay)
    {
        StartCoroutine(Creation(delay));
    }

    GameObject GetRandomObject()
    {
        GameObject randomPrefab = null;
        while (randomPrefab == null)
        {
            double randomValue = rdm.NextDouble();
            foreach (var spawn in spawns)
            {
                if (randomValue < spawn.rate)
                    randomPrefab = spawn.prefab;
            }
        }

        return randomPrefab;
    }

    IEnumerator Creation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 randomPos = transform.position + new Vector3(RandomFloat(offsetRandomRange.x, offsetRandomRange.y), RandomFloat(offsetRandomRange.x, offsetRandomRange.y));
        NewPet(randomPos);
    }

    void NewPet(Vector3 spawnPos)
    {
        GameObject newPet = Instantiate(GetRandomObject(), transform);
        newPet.transform.position = spawnPos;
        newPet.name = "PET" + index++;
        selections.allPets.Add(newPet.GetComponent<Pet>());
    }

    void NewGame()
    {
        for (int j = 0; j < dimensions.x; j++)
        {
            for (int k = 0; k < dimensions.y; k++)
            {
                Vector3 randomPos = transform.position + new Vector3(j + RandomFloat(offsetRandomRange.x, offsetRandomRange.y), k + RandomFloat(offsetRandomRange.x, offsetRandomRange.y));
                NewPet(randomPos);
            }
        }
    }
}
