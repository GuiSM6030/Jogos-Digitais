using UnityEngine;

public class SpawnerNaveMae : MonoBehaviour
{
    public GameObject naveMaePrefab;
    public Vector3 posicaoDeSpawn = new Vector3(-9f, 5f, 0f);

    private bool naveMaeAtivada;

    public void AtivarNaveMae()
    {
        if (naveMaeAtivada || naveMaePrefab == null)
            return;

        naveMaeAtivada = true;
        Instantiate(naveMaePrefab, posicaoDeSpawn, Quaternion.identity);
    }
}