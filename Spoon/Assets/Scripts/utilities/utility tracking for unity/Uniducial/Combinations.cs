using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Combinations : MonoBehaviour
{
    // Imagenes que muestran la receta que hay que hacer en este momento
    public Image t1;
    public Image t2;
    public Image m1;
    public Image t1_m1;

    public Image t1_2;
    public Image t2_2;
    public Image m1_2;
    public Image t1_m1_2;

    // Contadores para el número de tomates y de carnes
    public int n_t = 0;
    public int n_m = 0;
    public int n_t_2 = 0;
    public int n_m_2 = 0;
    private int seed = 0;

    // Número de puntos
    private int point = 0;
    private bool recipe_done = false;
    // Contador de tiempo para ralizar las recetas
    private float time = 60.0f;

    // Textos para mostrar en pantalla
    public Text scoreText;
    public Text timeLeft;

    public Plate plate;
    public Plate plate2;

    void Start()
    {
        // Empezamos una coroutine para ir mostrando nuevas recetas cada 60 segundos sin parar
        StartCoroutine(WaitBeforeRemove());
    }

    void Requesets()
    {       
        // Definimos rangos para generar las combinaciones de los platos de forma random
        n_t = Random.Range(0, 3);
        n_m = Random.Range(0, 2);
        n_t_2 = Random.Range(0, 3);
        n_m_2 = Random.Range(0, 2);
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject collider = other.gameObject;
        Transform collider_t = other.transform;
        GameObject object_to_destroy = null;

        // Comprobamos que haya colisionado uno de los dos players
        if (collider.tag == "Player1" || collider.tag == "Player2")
        {
            foreach (Transform child_t in collider_t)
            {
                // El player tiene que tener un plato y haberselo pasado almenos una vez con el otro player para mirar si coincide con la receta
                Plate plateScript = collider.GetComponent<Plate>();
                if (child_t.tag == "Plate" & plateScript.passed == true)
                {
                    object_to_destroy = child_t.gameObject;
                    Score(object_to_destroy, plateScript);
                }
            }
        }
    }
        IEnumerator WaitBeforeRemove()
    {
        // Empezamos a generar recetas
        bool start = true;
        while (start == true)
        {
            // Generamos los rangos aleatorios
            Requesets();
            // Definimos una seed para hacer el random que dependa del tiempo actual, así cada vez que se inicie el juego será diferente
            Random.seed = System.DateTime.Now.Millisecond;

            // Comprobamos los contadores y generamos la receta acorde a eso
            if ((n_t == 1 && n_m == 0))
            {
                t1.enabled = true;
                SecondRecipe();
                // Hasta que la receta no esté completa o haya pasado el tiempo del contador, no seguimos con la coroutine para generar nuevas recetas
                yield return new WaitUntil(() => (recipe_done == true | time < 0.0f));
                // Una vez la receta se ha completado o el tiempo se ha agotado, reiniciamos y generamos una nueva receta
                t1.enabled = false;
                recipe_done = false;
                time = 60.0f;

            }
            else if ((n_t == 1 && n_m == 1))
            {
                t1_m1.enabled = true;
                SecondRecipe();
                yield return new WaitUntil(() => (recipe_done == true | time < 0.0f));
                t1_m1.enabled = false;
                recipe_done = false;
                time = 60.0f;
            }
            else if ((n_t == 0 && n_m == 1))
            {
                m1.enabled = true;
                SecondRecipe();
                yield return new WaitUntil(() => (recipe_done == true | time < 0.0f));
                m1.enabled = false;
                recipe_done = false;
                time = 60.0f;
            }
            else if ((n_t == 2 && n_m == 0))
            {
                t2.enabled = true;
                SecondRecipe();
                yield return new WaitUntil(() => (recipe_done == true | time < 0.0f));
                t2.enabled = false;
                recipe_done = false;
                time = 60.0f;
            }
            t2_2.enabled = false;
            t1_2.enabled = false;
            t1_m1_2.enabled = false;
            m1_2.enabled = false;
        }
    }

    public void SecondRecipe()
    {
        bool check = true;
        while (check == true){
            Requesets();
            Random.seed = System.DateTime.Now.Millisecond;
            if ((n_t_2 == 1 && n_m_2 == 0))
            {
                t1_2.enabled = true;
                check = false;              

            }
            else if ((n_t_2 == 1 && n_m_2 == 1))
            {
                t1_m1_2.enabled = true;
                check = false;                
            }
            else if ((n_t_2 == 0 && n_m_2 == 1))
            {
                m1_2.enabled = true;
                check = false;                
            }
            else if ((n_t_2 == 2 && n_m_2 == 0))
            {
                t2_2.enabled = true;
                check = false;
            }
        }
    }

    void Score(GameObject child_t, Plate plateScript)
    {
        // Comprobamos que el plato que lleva el player tiene una de las dos combinaciones que muestran la receta
        if (n_t == plate.tomata_count_1 && n_m == plate.meat_count_1 || n_t_2 == plate.tomata_count_2 && n_m_2 == plate.meat_count_2)
        {
            CheckScore(child_t, plateScript);
        }

        else if (n_t == plate2.tomata_count_1 && n_m == plate2.meat_count_1 || n_t_2 == plate2.tomata_count_2 && n_m_2 == plate2.meat_count_2)
        {
            CheckScore(child_t, plateScript);
        }
    }

    void CheckScore(GameObject child_t, Plate plateScript)
    {
        // Si es correcto sumamos un punto al contador
        point += 1;
        GetComponent<AudioSource>().Play();

        // Eliminamos el plato del player
        Destroy(child_t);
        // Marcamos la receta como completada para generar una nueva
        recipe_done = true;
        scoreText.text = "Score: " + point.ToString();
        // Reiniciamos los contadores
        plate.tomata_count_1 = 0;
        plate.meat_count_1 = 0;
        plate.tomata_count_2 = 0;
        plate.meat_count_2 = 0;

        // Reseteamos la variable que marca si el plato ha sido pasado entre players almenos una vez
        plateScript.passed = false;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        string seconds = (time % 60).ToString("0");

        timeLeft.text = "Next recipe in: " + seconds; 
    }
}
