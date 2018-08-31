using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CityManager : MonoBehaviour {
    // Use this for initialization
    public int attitude; // 1 is warleader, 2 is mayor, 3 is feaudal, 4 is dullard (does nothing), 5 is ballenced, 6 deffender  
    public float ruralpop;
    public float urbanpop;
    public float milpop;
    public float milspending;
    public float idealpop;
    public float urbanidealpop;
    public float pop;
    public float farmeffper; // farm efficentcy percent
    public float urbaneffper; // urban effiecentcy percent
    public float food;
    public float wealth;
    public float pwealth; // personal wealth
    public float taxper; // yearly tax on wealth
    public float happyness;
    public float miltime;
    public bool atWar; // at war for def
    public bool agrWar; //at war for agressor
    float distance;
    public GameObject enemy;
    int month = 0;
    int i;

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("City");
        GameObject closest = null;
        distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && curDistance != 0)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    void Start () {
        atWar = false;
        attitude = Random.Range(1, 7);
	}
	
	// Update is called once per frame
	void Update () {
        if (((int)Time.time-1) % 5 == 0 && month != ((int)Time.time-1) / 5 % 12 + 1)
        {
            month = (int)Time.time / 5 % 12 + 1;
            farmeffper = GetComponent<City>().farmeffper;
            wealth = GetComponent<City>().wealth;
            urbanidealpop = GetComponent<City>().urbanidealpop;
            idealpop = GetComponent<City>().idealpop;
            milpop = GetComponent<City>().milpop;
            pop = GetComponent<City>().pop;
            miltime = GetComponent<City>().miltime;
            milspending = GetComponent<City>().milspending;
            pwealth = GetComponent<City>().pwealth;
            food = GetComponent<City>().food;

            if (attitude == 1) // warleader
            {
                if (agrWar == false)
                {
                    milspending = pwealth;
                    if(month == 6)
                    {                     
                        enemy = FindClosestEnemy();
                        enemy.GetComponent<CityManager>().atWar = true;
                        i = 0;
                        agrWar = true;
                    }
                }
                if (agrWar == true)
                {
                    milspending = pwealth;
                    i++;
                    if (i > distance)// battle starts
                    {
                        //Debug.Log("meme");
                        if (!enemy)
                        {
                            i = 0;
                            agrWar = false;
                        }
                        float emilpop = enemy.GetComponent<City>().milpop; // enemy mil population
                        float emiltime = enemy.GetComponent<City>().miltime; // enemy mil training time
                        float epop = enemy.GetComponent<City>().pop; // enemy total pop
                        float skill;
                        if ((milpop * (1 + miltime * 2 / 12 * 30)) > (emilpop * (1 + emiltime * 2 / 12 * 30)))  //with full traing soldiers are 3 times more effective
                        {
                            Debug.Log(name + " attacker wins");
                            skill = (1 + emiltime * 2 / 12 * 30) * emilpop;
                            enemy.GetComponent<City>().miltime = 0;
                            enemy.GetComponent<City>().milpop = 0;
                          
                            milpop -= skill / (1 + miltime * 2 / 12 * 30);
                            if (pop < milpop * 3)
                            {                               
                                GetComponent<City>().ruralpop += enemy.GetComponent<City>().ruralpop;
                                enemy.GetComponent<City>().ruralpop = 0;
                                GetComponent<City>().urbanpop += enemy.GetComponent<City>().urbanpop;
                                enemy.GetComponent<City>().urbanpop = 0;
                                wealth += enemy.GetComponent<City>().wealth;
                                pwealth += enemy.GetComponent<City>().pwealth;
                                enemy.GetComponent<City>().wealth = 0;
                                enemy.GetComponent<City>().pwealth = 0;
                                enemy.GetComponent<City>().food = 0;
                            }
                            else
                            {
                                GetComponent<City>().ruralpop += enemy.GetComponent<City>().ruralpop * milpop * 3 / epop;
                                enemy.GetComponent<City>().ruralpop -= enemy.GetComponent<City>().ruralpop * milpop * 3 / epop;
                                GetComponent<City>().urbanpop += enemy.GetComponent<City>().urbanpop * milpop * 3 / epop;
                                enemy.GetComponent<City>().urbanpop -= enemy.GetComponent<City>().urbanpop * milpop * 3 / epop;
                                wealth += enemy.GetComponent<City>().wealth * milpop * 10 / epop;
                                pwealth += enemy.GetComponent<City>().pwealth;
                                enemy.GetComponent<City>().wealth -= enemy.GetComponent<City>().wealth * milpop * 10 / epop;
                                enemy.GetComponent<City>().pwealth = 0;
                                food += enemy.GetComponent<City>().food * milpop * 10 / epop;
                                enemy.GetComponent<City>().food = enemy.GetComponent<City>().food * milpop * 10 / epop;
                            }


                        }
                        else
                        {
                            Debug.Log("defender wins");
                            skill = (1 + miltime * 2 / 12 * 30) * milpop;
                            miltime = 0;
                            milpop = 0;
                            enemy.GetComponent<City>().milpop -= skill / (1 + emiltime * 2 / 12 * 30);
                        }
                        i = 0;
                        agrWar = false;
                        enemy.GetComponent<CityManager>().atWar = false;
                    }
                    

                }

            }

            if (attitude == 2) // mayor
            {
                milspending = 0;
                if (atWar == false)
                {
                    pwealth = pwealth / 2;
                    urbanidealpop += pwealth / 5;
                }
                if (atWar == true)
                {
                    milspending = pwealth;
                }
            }
            if (attitude == 3) // feadual
            {
                milspending = 0;
                if (atWar == false)
                {
                    pwealth = pwealth / 2;
                    idealpop += pwealth;
                }
                if (atWar == true)
                {
                    milspending = pwealth;
                }
            }
            if (attitude == 4) // dullard
            {
                milspending = 0;
                if (atWar == true)
                {
                    milspending = pwealth / 2;
                }
            }
            if (attitude == 5) // ballenced
            {
                if (atWar == false)
                {
                    milspending = 0;
                    if (idealpop * farmeffper / 100 >= urbanidealpop)
                    {
                        pwealth = pwealth / 2;
                        urbanidealpop += pwealth / 5;
                    }
                    else
                    {
                        pwealth = pwealth / 2;
                        idealpop += pwealth;
                    }
                }
                if (atWar == true)
                {
                    milspending = pwealth;
                }
            }
            if (attitude == 6) // defender
            {
                if (atWar == false && pwealth >= 100)
                {
                    milspending = 0;
                    if (idealpop * farmeffper / 100 >= urbanidealpop)
                    {
                        pwealth = pwealth / 2;
                        urbanidealpop += pwealth / 5;
                    }
                    else
                    {
                        pwealth = pwealth / 2;
                        idealpop += pwealth;
                    }
                }
                if (atWar == true)
                {
                    milspending = pwealth/2;
                }
            }
            GetComponent<City>().wealth = wealth;
            GetComponent<City>().pwealth = pwealth;
            GetComponent<City>().urbanidealpop = urbanidealpop;
            GetComponent<City>().idealpop = idealpop;
            GetComponent<City>().milpop = milpop;
            GetComponent<City>().pop = pop;
            GetComponent<City>().miltime = miltime;
            GetComponent<City>().milspending = milspending;
        }
	}
}
