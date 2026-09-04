using UnityEngine;

public class audio : MonoBehaviour
{
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();

        if (source == null)
            Debug.LogError("NÃO ENCONTROU O AUDIOSOURCE!");
        else
            Debug.Log("AudioSource encontrado!");
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("COLISÃO!");

        if (source != null)
        {
            Debug.Log("Tentando tocar: " + source.clip);

            source.Play();
        }
    }
}