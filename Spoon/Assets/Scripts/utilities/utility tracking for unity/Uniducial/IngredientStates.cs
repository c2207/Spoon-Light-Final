using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStates : MonoBehaviour
{
    public float scaleFactor;
    public Vector3 positionAdd;
    public GameObject ingredientPrefab;

    private void OnCollisionEnter(Collision other)
    {
        GameObject child = null;

        GameObject collider = other.gameObject;
        Transform collider_t = other.transform;

        // Comprobamos si hemos colisionado con una caja de ingredientes
        if (gameObject.CompareTag("Ingredient_box"))
        {
            // Cada vez que colisionamos, comprobamos si el player ya lleva algun ingrediente o plato
            foreach (Transform child_t in collider_t)
            {
                if (child_t.tag == "Ingredient" | child_t.tag == "CutIngredient" | child_t.tag == "CookedIngredient" | child_t.tag == "OvercookedIngredient" | child_t.tag == "Plate")
                {
                    child = child_t.gameObject;
                }
            }

            // En el caso de que el player no lleve ningún otro ingrediente del tipo que sea, instanciamos uno nuevo
            if (child == null)
            {
                GetComponent<AudioSource>().Play();
                Spawn(other);
            }
        }

        // Comprobamos si hemos colisionado con la basura
        if (gameObject.CompareTag("Garbage"))
        {
            GetComponent<AudioSource>().Play();
            RemoveIngredient(other);
        }
    }

    void Spawn(Collision other)
    {
        Vector3 position = new Vector3(75, 7, 85);
        // Instanciamos un nuevo ingrediente
        GameObject ingredient = Instantiate(ingredientPrefab, position, ingredientPrefab.transform.rotation);
        // Ponemos el ingrediente como un hijo del player para que así lo siga
        ingredient.transform.parent = other.transform;

        // Añadimos una posición y una escala para adaptarlo visualmente al player
        ingredient.transform.position = other.transform.position + positionAdd;
        ingredient.transform.localScale = new Vector3(1, 1, 1) * scaleFactor;
    }

    void RemoveIngredient(Collision other)
    {
        {
            GameObject child = null;

            GameObject collider = other.gameObject;
            Transform collider_t = other.transform;
            // Buscamos si hay algun child en el player con el que hemos colisionado que corresponda a un ingrediente o plato
            foreach (Transform child_t in collider_t)
            {
                if (child_t.tag == "Ingredient" || child_t.tag == "CutIngredient" || child_t.tag == "CookedIngredient" || child_t.tag == "OvercookedIngredient" || child_t.tag == "Plate" ){
                    child = child_t.gameObject;
                }
            }

            // Si existe, lo eliminamos
            if (child != null)
            {
                Destroy(child);
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
