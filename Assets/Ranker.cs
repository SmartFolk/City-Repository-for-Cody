using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranker : MonoBehaviour
{
    GameObject[] cities;
    //ArrayList top5 = new ArrayList(); // top 5 cities
    //ArrayList bot5 = new ArrayList(); // bottom 5 cities
    ArrayList all = new ArrayList();
    GameObject city;
    GameObject store;// stores values   
    public float popc;
    public float foodc;
    public float wealthc;
    public Color color = new Color(0.2F, 0.3F, 0.4F, 0.5F);

    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(0, 255, 0);
    }

    // Update is called once per frame
    void Update()
    {
        cities = GameObject.FindGameObjectsWithTag("City");
        for (int i = 0; i < cities.Length; i++)
        {
            city = cities[i];
            wealthc = city.GetComponent<City>().wealth;
            popc = city.GetComponent<City>().pop;

        }
    }
}
