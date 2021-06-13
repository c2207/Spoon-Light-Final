using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutIngredient : MonoBehaviour
{
    public GameObject tomato_cutted;
    public float scaleFactor;
    public Vector3 positionAdd;
    public GameObject bar;

    private float cuttime = 5.0f;

    private void OnCollisionEnter(Collision other)
    {
        GameObject collider = other.gameObject;
        Transform collider_t = other.transform;
        // Buscamos si hay algun child en el player con el que hemos colisionado que corresponda a un tomate
        foreach (Transform child_t in collider_t)
        {
            string name_item = child_t.ToString();
            int leftParenthesis = name_item.IndexOf("(");
            string result = name_item.Substring(0, leftParenthesis);
            if (result == "Tomato")
            {
                // Si se trata de un tomate empezamos a animar la barra y ponemos un sonido
                GetComponent<AudioSource>().Play();
                AnimateBar();
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        GameObject child = null;

        GameObject collider = other.gameObject;
        Transform collider_t = other.transform;
        // Buscamos si hay algun child en el player con el que hemos colisionado que corresponda a un tomate
        foreach (Transform child_t in collider_t)
        {
            string name_item = child_t.ToString();
            int leftParenthesis = name_item.IndexOf("(");
            string result = name_item.Substring(0, leftParenthesis);
            if (result == "Tomato")
            {
                child = child_t.gameObject;
            }
        }

        // Si corresponde a un tomate empezamos a cortar
        if (child != null)
        {
            // Bajamos el contador del tiempo hasta que pasen 5 segundos
            cuttime -= Time.deltaTime;

            // Una vez recorridos, instanciamos un prefab de tomate cortado y lo ponemos como child del player
            if (cuttime < 0.0f)
            {
                GameObject ingredient = Instantiate(tomato_cutted, child.transform.position, Quaternion.identity);
                // Instanciamos el ingrediente como un hijo del player para que así lo siga
                ingredient.transform.parent = other.transform;
                // Añadimos una posición y una escala para adaptarlo visualmente al player
                ingredient.transform.position = other.transform.position + positionAdd;
                ingredient.transform.localScale = new Vector3(1, 1, 1) * scaleFactor;
                // Eliminamos el tomate antiguo
                Destroy(child);
                ResetAnimateBar();
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        // Reseteamos la barra y tiempo cada vez que salimos de la colisión
        StopAnimateBar();
        ResetAnimateBar();
        GetComponent<AudioSource>().Stop();
        cuttime = 5.0f;
    }

    public void AnimateBar()
    {
        LeanTween.scaleX(bar, 1, cuttime);
    }

    public void StopAnimateBar()
    {
        LeanTween.cancel(bar);
    }

    public void ResetAnimateBar()
    {
        LeanTween.scaleX(bar, 0, 0);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
