using UnityEngine;

public class SpawnerNaveMae : MonoBehaviour
{
    public GameObject naveMaePrefab;
    public Vector3 posicaoDeSpawn = new Vector3(-9f, 5f, 0f);

    [Header("Tempo de aparição")]
    public float tempoMinimo = 15f;
    public float tempoMaximo = 25f;
    public bool aparecerUmaVez = true;

    void Start()
    {
        AgendarProximoSpawn();
    }

    void AgendarProximoSpawn()
    {
        float tempo = Random.Range(tempoMinimo, tempoMaximo);
        Invoke(nameof(SpawnarNave), tempo);
    }

    void SpawnarNave()
    {
        if (naveMaePrefab == null)
            return;

        Instantiate(naveMaePrefab, posicaoDeSpawn, Quaternion.identity);

        if (!aparecerUmaVez)
        {
            AgendarProximoSpawn();
        }
    }
}