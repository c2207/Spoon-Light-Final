using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutInPlate : MonoBehaviour
{
    public Vector3 addPosition;
    public float scaleFactor;
    public GameObject ingredientPrefab;

    private void OnCollisionEnter(Collision other)
    {
        GameObject child = null;

        GameObject collider = other.gameObject;
        Transform collider_t = other.transform;
        foreach (Transform child_t in collider_t)
        {
            // Cada vez que un player colisiona con un plato, comprobamos si lleva algun ingrediente
            if (child_t.tag == "Ingredient" || child_t.tag == "CutIngredient" || child_t.tag == "CookedIngredient")
            {
                child = child_t.gameObject;
            }

            // No nos interesa meter en el plato ingredientes quemados
            else if (child_t.tag == "OvercookedIngredient")
            {
                GetComponent<AudioSource>().Play();
            }
        }

        // Si existe, la idea es trasladar el ingrediente que lleva el player, y ponerlo como un child del plato 
        if (child != null)
        {
            string name_item = child.ToString();
            int leftParenthesis = name_item.IndexOf("(");
            string result = name_item.Substring(0, leftParenthesis);
            
            // Adaptamos la escala y la posición segun el ingrediente que sea, para mejorar la visualización
            if (result == "Meat")
            {
                if (child.tag == "CookedIngredient")
                {
                    child.transform.parent = gameObject.transform;
                    child.transform.position = gameObject.transform.position + new Vector3(-2, 5, 0);
                    child.transform.localScale = new Vector3(1, 1, 1) * 0.15f;
                }
                
            }
            else if (result == "Cutted")
            {
                child.transform.parent = gameObject.transform;
                child.transform.position = gameObject.transform.position + new Vector3(0, 0, -6);
                child.transform.localScale = new Vector3(1, 1, 1) * 0.25f;
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
