using UnityEngine;

public class GeradorAsteroid : MonoBehaviour
{
    [Header("Referencias")]
    public SpaceShipGame game;
    public GameObject asteroid1Prefab;
    public GameObject asteroid2Prefab;
    public GameObject asteroid3Prefab;

    [Header("Probabilidades")]
    [Range(0f, 1f)] public float probabilidadeAsteroid1 = 0.65f;
    [Range(0f, 1f)] public float probabilidadeAsteroid2 = 0.25f;
    [Range(0f, 1f)] public float probabilidadeAsteroid3 = 0.10f;

    [Header("Geracao")]
    public float intervalo = 1.2f;
    public float velocidade = 2.4f;
    public float posicaoX = 6.2f;
    public Vector2 limitesY = new Vector2(-2.8f, 2.8f);

    private float proximoAsteroide;

    void Awake()
    {
        if (game == null)
            game = FindFirstObjectByType<SpaceShipGame>();
    }

    void Start()
    {
        proximoAsteroide = Time.time + 0.5f;
    }

    void Update()
    {
        if (game == null || game.JogoEncerrado() || Time.time < proximoAsteroide)
            return;

        CriarAsteroide();
        proximoAsteroide = Time.time + intervalo;
    }

    void CriarAsteroide()
    {
        float sorteio = Random.value;
        float limiteAsteroid1 = probabilidadeAsteroid1;
        float limiteAsteroid2 = limiteAsteroid1 + probabilidadeAsteroid2;
        int tipo = sorteio < limiteAsteroid1 ? 1 : sorteio < limiteAsteroid2 ? 2 : 3;

        GameObject prefab = tipo == 1 ? asteroid1Prefab : tipo == 2 ? asteroid2Prefab : asteroid3Prefab;
        if (prefab == null)
            return;

        GameObject asteroid = Instantiate(prefab, new Vector3(posicaoX, Random.Range(limitesY.x, limitesY.y), 0f), Quaternion.identity);
        AsteroidSpaceShip controller = asteroid.GetComponent<AsteroidSpaceShip>();
        controller.game = game;
        controller.velocidade = velocidade;
    }
}
