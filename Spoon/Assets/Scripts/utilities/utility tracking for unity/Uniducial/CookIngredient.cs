using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookIngredient : MonoBehaviour
{
    public float scaleFactor;
    public Vector3 positionAdd;
    public GameObject bar;

    // Tiempos de cocinado y quemado
    private float cookTime = 5.0f;
    private float overcookTime = 10.0f;

    // Usado para saber si estamos cocinando en este momento
    private bool isCooking = false;

    // GameObject que guarda el ingrediente que se está cocinando en este momento
    private GameObject cooking_ingredient;
    private bool isOvercooked = false;

    private void OnCollisionEnter(Collision other)
    {
        GameObject child = null;

        GameObject collider = other.gameObject;
        Transform collider_t = other.transform;

        // Buscamos si hay algun child en el player con el que hemos colisionado que corresponda a un ingrediente
        foreach (Transform child_t in collider_t)
        {
            // Cocinamos solo si es un ingrediente. No nos interesa cocinar elementos cortados, cocinados o quemados 
            if (child_t.tag == "Ingredient" && !isCooking)
            {
                // Instanciamos el child en la variable cooking_ingredient, para así poder recoger el ingrediente al volver
                GetComponent<AudioSource>().Play();
                AnimateBar();
                cooking_ingredient = GameObject.Instantiate(child_t.gameObject);

                // Ponemos los fogones a cocinar
                isCooking = true;
                // Eliminamos el ingrediente del player
                child = child_t.gameObject;
                Destroy(child);
            }
        }

        // Si ya estamos cocinando y el player no lleva ningun ingrediente, recogemos la carne cocinada o quemada con la tag ya cambiada.
        if (isCooking && child == null)
        {
            // Si el ingrediente se ha quemado
            if (overcookTime < 0.0f)
            {
                // Cambiamos el tag
                cooking_ingredient.tag = "OvercookedIngredient";
                // Ponemos el ingrediene como child del player para llevarselo
                cooking_ingredient.transform.parent = other.transform;
                cooking_ingredient.transform.position = other.transform.position + positionAdd;
                cooking_ingredient.transform.localScale = new Vector3(1, 1, 1) * scaleFactor;

                // Reiniciamos los contadores y la barra de tiempo
                cookTime = 5.0f;
                overcookTime = 10.0f;
                isCooking = false;
                StopAnimateBar();
                ResetAnimateBar();
            }

            // Si estamos dentro del tiempo de ingrediente cocinado
            else if (cookTime < 0.0f)
            {
                // Cambiamos el color de la carne a marrón (cocinado)
                // El material en el que tenemos que cambiar el color es un child del ingrediente, por eso tenemos que hacer este GetChild(0)
                var ingredient_renderer = cooking_ingredient.transform.GetChild(0).GetComponent<Renderer>();
                ingredient_renderer.material.color = new Color((100f / 255f), (53 / 255f), (26 / 255f));

                // Cambiamos el tag
                cooking_ingredient.tag = "CookedIngredient";
                // Ponemos el ingrediente como child del player para que se lo lleve
                cooking_ingredient.transform.parent = other.transform;
                cooking_ingredient.transform.position = other.transform.position + positionAdd;
                cooking_ingredient.transform.localScale = new Vector3(1, 1, 1) * scaleFactor;

                // Reiniciamos los contadores y la barra de tiempo
                cookTime = 5.0f;
                overcookTime = 10.0f;
                isCooking = false;
                StopAnimateBar();
                ResetAnimateBar();
            }
        }
    }

    public void AnimateBar()
    {
        LeanTween.scaleY(bar, 1, cookTime);
    }

    public void AnimateBar_over()
    {
        LeanTween.scaleY(bar, 1, overcookTime);
    }

    public void ResetAnimateBar()
    {
        LeanTween.scaleY(bar, 0, 0);
    }

    public void StopAnimateBar()
    {
        LeanTween.cancel(bar);
    }

    void Start()
    {

    }

    void Update()
    {
        // Si isCooking esta activado, restamos tiempo
        if (isCooking)
        {
            cookTime -= Time.deltaTime;
            overcookTime -= Time.deltaTime;
            // Solo queremos entrar aquí cuando hayamos estemos a punto de pasar la frontera entre cocinado y quemado para resetear la barra
            if (cookTime < 0.0 & !isOvercooked)
            {
                isOvercooked = true;
                // Cambiamos el color de la barra a azul para alertar de que el ingrediente se está quemando
                bar.transform.GetComponent<Image>().color = Color.blue;
                StopAnimateBar();
                ResetAnimateBar();
                AnimateBar_over();
            }
        }
    }
}
