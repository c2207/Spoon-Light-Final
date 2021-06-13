using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveIngredients : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        GameObject child = null;

        GameObject collider = other.gameObject;
        Transform collider_t = other.transform;
        // Buscamos si hay algun child en el player con el que hemos colisionado que corresponda a un ingrediente
        foreach (Transform child_t in collider_t)
        {
            if (child_t.tag == "Ingredient" || child_t.tag == "CutIngredient" || child_t.tag == "CookedIngredient" || child_t.tag == "OvercookedIngredient")
            {
                child = child_t.gameObject;
            }
        }

        // Si existe, lo eliminamos
        if (child != null)
        {
            SoundManager.Instance.PlayDropTrash();

            // Reseteamos la variable passed
            Plate plateScript = collider.GetComponent<Plate>();
            plateScript.passed = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
