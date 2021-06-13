using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public float scaleFactor;
    public Vector3 positionAdd;
    public Combinations combinations;

    // Contadores para saber el número de tomates y de carne que hay en el plato
    public int tomata_count_1 = 0;
    public int meat_count_1 = 0;

    public int tomata_count_2 = 0;
    public int meat_count_2 = 0;

    private float timeTakePlate1 = 2.0f;
    private float timeTakePlate2 = 2.0f;

    // Usado para saber si el plato / ingrediente se ha pasado entre players
    public bool passed = false;

    // Barras animadas que marcan una acción que se está completando
    public GameObject barPlate1;
    public GameObject barPlate2;

    private void OnCollisionStay(Collision other)
    {
        GameObject collider = other.gameObject;
        Transform collider_t = other.transform;

        // Comprobamos que hemos colisionado con un plato
        if (collider.name == "Plate1" | collider.name == "Plate1(Clone)")
        {
            // Cada vez que colisionamos, restamos el tiempo para contar 2 segundos, y animamos la barra para mostrarlo en pantalla
            timeTakePlate1 -= Time.deltaTime;
            AnimateBar(timeTakePlate1, barPlate1);

            // Para coger un plato el player tiene que estar colisionando durante 2 segundos
            if (timeTakePlate1 < 0.0f)
            {
                // Cogemos el plato (si se puede)
                TakePlate(collider, collider_t);
            }
        }

        // Seguimos el mismo procedimiento, pero ahora comprobando el plato 2
        else if (collider.name == "Plate2" | collider.name == "Plate2(Clone)")
        {
            timeTakePlate2 -= Time.deltaTime;
            AnimateBar(timeTakePlate2, barPlate2);

            if (timeTakePlate2 < 0.0f)
            {
                TakePlate(collider, collider_t);
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        GameObject collider = other.gameObject;
        Transform collider_t = other.transform;

        if (collider.name == "Plate1" | collider.name == "Plate1(Clone)")
        {
            if (timeTakePlate1 < 0.0f)
            {
                GetComponent<AudioSource>().Play();
            }
            timeTakePlate1 = 2.0f;
            StopAnimateBar(barPlate1);
            ResetAnimateBar(barPlate1);
        }

        if (collider.name == "Plate2" | collider.name == "Plate2(Clone)")
        {
            if (timeTakePlate2 < 0.0f)
            {
                GetComponent<AudioSource>().Play();
            }
            timeTakePlate2 = 2.0f;
            StopAnimateBar(barPlate2);
            ResetAnimateBar(barPlate2);
        }
    }

    public void TakePlate(GameObject collider, Transform collider_t)
    {
        // Recorremos cada uno de los childs que haya en el plato y contamos el número de tomates y carnes que contiene
        foreach (Transform child_t in collider_t)
        {
            string name_child = child_t.ToString();
            int parentesis = name_child.IndexOf("(");
            string output = name_child.Substring(0, parentesis);


            if (output == "Cutted")
            {
                tomata_count_2 += 1;
            }

            else if (output == "Meat")
            {
                meat_count_2 += 1;
            }
        }

        // Comprobamos también que el player no tenga ya algun plato o ingrediente
        GameObject child_player = null;
        foreach (Transform child_player_t in gameObject.transform)
        {
            if (child_player_t.tag == "Ingredient" | child_player_t.tag == "CutIngredient" | child_player_t.tag == "CookedIngredient" | child_player_t.tag == "OvercookedIngredient" | child_player_t.tag == "Plate")
            {
                child_player = child_player_t.gameObject;
            }
        }

        // Si tiene algun otro child, no cogemos el plato
        if (child_player == null)
        {
            // Primero instanciamos un nuevo plato para hacer una fuente ilimitada y que siempre puedas volver a coger otro
            GameObject plate = Instantiate(collider, collider.transform.position, collider.transform.rotation);
            plate.transform.parent = collider.transform.parent;
            plate.transform.position = collider.transform.position;
            plate.transform.localScale = collider.transform.localScale;
            // Eliminamos los childs del nuevo plato
            foreach (Transform ing_ch in plate.transform)
            {
                Destroy(ing_ch.gameObject);
            }

            // Eliminamos el box collider del plato para no colisionar con un plato al chocar con otro player
            collider_t.GetComponent<BoxCollider>().enabled = false;
            // Cogemos el otro plato y lo ponemos como child del player
            collider_t.transform.parent = gameObject.transform;
            collider_t.transform.position = gameObject.transform.position + positionAdd;
            collider_t.transform.localScale = new Vector3(1, 1, 1) * scaleFactor;
            // Una vez la barra ha llegado al límite la reseteamos para poder pararla
            ResetAnimateBar(barPlate1);
        }
    }

    public void AnimateBar(float time, GameObject bar)
    {
        LeanTween.scaleY(bar, 1, time);
    }

    public void StopAnimateBar(GameObject bar)
    {
        LeanTween.cancel(bar);
    }

    public void ResetAnimateBar(GameObject bar)
    {
        LeanTween.scaleY(bar, 0, 0);
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
