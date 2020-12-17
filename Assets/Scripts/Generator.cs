using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    Selections selections => FindObjectOfType<Selections>();

    [SerializeField] Vector2 dimensions = new Vector2(5, 5);
    [SerializeField] Vector2 offsetRandomRange = new Vector2(-1, 1);
    [SerializeField] GameObject petPrefab, snakePrefab;
    int index;
    System.Random rdm = new System.Random();
    GameObject newPet;

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

    IEnumerator Creation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 pos = transform.position + new Vector3(RandomFloat(offsetRandomRange.x, offsetRandomRange.y), RandomFloat(offsetRandomRange.x, offsetRandomRange.y));
        NewPet(pos);
    }

    void NewPet(Vector3 spawnPos)
    {
        int probability = rdm.Next(0, 100);
        if (probability < 2)
            newPet = Instantiate(snakePrefab, spawnPos, Quaternion.identity);
        else
            newPet = Instantiate(petPrefab, spawnPos, Quaternion.identity);

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
