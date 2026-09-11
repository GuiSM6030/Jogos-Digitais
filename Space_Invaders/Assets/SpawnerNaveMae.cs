using UnityEngine;

public class SpawnerNaveMae : MonoBehaviour
{
    public GameObject naveMaePrefab;
    public Vector3 posicaoDeSpawn = new Vector3(-9f, 6f, 0f);

    void Start()
    {
        AgendarProximoSpawn();
    }

    void AgendarProximoSpawn()
    {
        float tempo = Random.Range(30f, 50f);
        Invoke(nameof(SpawnarNave), tempo);
    }

    void SpawnarNave()
    {
        Instantiate(naveMaePrefab, posicaoDeSpawn, Quaternion.identity);
        AgendarProximoSpawn();
    }
}