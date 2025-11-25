using UnityEngine;
using System.Collections.Generic;

public class PlataformaMovimiento : MonoBehaviour
{
    private Vector3 posicionAnterior;
    public List<GameObject> personajes = new List<GameObject>();

    void Start()
    {
        posicionAnterior = transform.position;
    }

    void Update()
    {
        Vector3 deltaMovimiento = transform.position - posicionAnterior;

        if (deltaMovimiento != Vector3.zero)
        {
            foreach (GameObject personaje in personajes)
            {
                CharacterController cc = personaje.GetComponent<CharacterController>();
                if (cc != null && personaje.GetComponent<Movimiento>().EstaSobrePlataforma())
                {
                    cc.Move(deltaMovimiento);
                }
                else
                {
                    personaje.transform.position += deltaMovimiento;
                }
            }
        }

        posicionAnterior = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !personajes.Contains(other.gameObject))
        {
            personajes.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && personajes.Contains(other.gameObject))
        {
            personajes.Remove(other.gameObject);
        }
    }
}
