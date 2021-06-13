using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPlate : MonoBehaviour
{
    public Vector3 addPosition;
    public float scaleFactor1;
    public float scaleFactor2;
    private GameObject child_collider = null;
    private GameObject child_this = null;

    void OnCollisionEnter(Collision other)
    {
        GameObject collider = other.gameObject;
        // Comprobamos que hemos colisionado con un player. Entonces intercambiamos los platos que lleven ambos (si los llevan)
        if (collider.tag == "Player1" || collider.tag == "Player2")
        {
            Transform collider_t = other.transform;
            // Comprobamos primero los childs del player que ha colisionado con este
            foreach (Transform child_t in collider_t)
            {
                if (child_t.tag == "Ingredient" | child_t.tag == "CookedIngredient" | child_t.tag == "OvercookedIngredient" | child_t.tag == "CutIngredient" | child_t.tag == "Plate")
                {
                    child_collider = child_t.gameObject;
                    // Cuando pasamos un plato de un player a otro, nos hemos dado cuenta de que se pone muy pequeño, por este motivo, le aumentamos la escala
                    if (child_t.tag == "Plate")
                    {
                        scaleFactor1 = 3.5f;
                    }
                }
            }
            // Ahora comprobamos los childs de este player
            foreach (Transform child_t in gameObject.transform)
            {
                if (child_t.tag == "Ingredient" | child_t.tag == "CookedIngredient" | child_t.tag == "OvercookedIngredient" | child_t.tag == "CutIngredient" | child_t.tag == "Plate")
                {
                    child_this = child_t.gameObject;
                    // Cuando pasamos un plato de un player a otro, nos hemos dado cuenta de que se pone muy pequeño, por este motivo, le aumentamos la escala
                    if (child_t.tag == "Plate")
                    {
                        scaleFactor2 = 3.5f;
                    }
                }
            }

            // Intercambiamos los childs
            if (child_collider != null)
            {
                // Marcamos el plato como que ya se ha pasado al menos una vez (en este player)
                Plate plateScript2 = gameObject.GetComponent<Plate>();
                plateScript2.passed = true;

                // Intercambiamos platos
                child_collider.transform.parent = gameObject.transform;
                child_collider.transform.position = gameObject.transform.position + addPosition;
                child_collider.transform.localScale = new Vector3(1, 1, 1) * scaleFactor1;
                scaleFactor1 = 1.0f;
            }
            if (child_this != null)
            {
                // Marcamos el plato como que ya se ha pasado al menos una vez (en el player que ha colisionado)
                Plate plateScript1 = collider.GetComponent<Plate>();
                plateScript1.passed = true;

                child_this.transform.parent = collider.transform;
                child_this.transform.position = collider.transform.position - addPosition;
                child_this.transform.localScale = new Vector3(1, 1, 1) * scaleFactor2;
                scaleFactor2 = 1.0f;
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
