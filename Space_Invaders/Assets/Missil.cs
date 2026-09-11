using UnityEngine;

public class Missil : MonoBehaviour
{
    public float velocidade = 8f;
    private int direcao = -1;

    private float limiteSuperior;
    private float limiteInferior;

    public void SetDirecao(int dir)
    {
        direcao = dir;
    }

    void Start()
    {
        float alturaCam = Camera.main.orthographicSize;
        Vector3 centroCam = Camera.main.transform.position;

        limiteSuperior = centroCam.y + alturaCam;
        limiteInferior = centroCam.y - alturaCam;
    }

    void Update()
    {
        transform.position += Vector3.up * direcao * velocidade * Time.deltaTime;

        if (transform.position.y > limiteSuperior || transform.position.y < limiteInferior)
        {
            Destroy(gameObject);
        }
    }
}