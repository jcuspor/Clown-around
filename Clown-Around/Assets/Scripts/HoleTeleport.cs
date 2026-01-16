using UnityEngine;

public class HoleTeleport : MonoBehaviour
{
    public GameObject teleport;
    public AudioSource sonido;

    void Start()
    {
        sonido = gameObject.GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("player2"))
        {
            //Debug.Log("Funca");
            CharacterController cc = other.GetComponent<CharacterController>();
        if (cc)
        {
            cc.enabled = false;
            sonido.Play();
            other.transform.position = teleport.transform.position;     // teleport
            cc.enabled = true;
        }
        }
        else
        {
            other.transform.position = teleport.transform.position;
        }
    }
}
