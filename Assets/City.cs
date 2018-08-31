using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public float pop = 0; // total population in city
    public float ruralpop;
    public float urbanpop;
    public float milpop;
    public float milspending;
    public float idealpop;
    public float urbanidealpop;
    public float farmeffper; // farm efficentcy percent
    public float urbaneffper; // urban effiecentcy percent
    public float food;
    public float happyness;
    public Color color = new Color(0.2F, 0.3F, 0.4F, 0.5F);
    public float wealth; // wealth of city
    public float pwealth; // personal wealth
    public float taxper; // yearly tax on wealth
    int month = 0;
    float oldmilspending = 0; // military spending last month
    
    public float miltime = 0; // time military has been trianed for
    //string[] monthn = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"];
    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(0, 255, 0);
        pop = ruralpop + urbanpop + milpop;
        //oldmilspending = milspending;
    }

    // Update is called once per frame
    void Update()
    {

        if ((int)Time.time % 5 == 0 && month != (int)Time.time / 5 % 12 + 1)
        {
            miltime++;
            month = (int)Time.time / 5 % 12 + 1;
            //Debug.Log(month);
            if (food > 0)
            {
                ruralpop += idealpop / 100 * (idealpop / (idealpop + ruralpop));
            }
            if (food > 0)
            {
                urbanpop += urbanidealpop / 100 * (urbanidealpop / (urbanidealpop + urbanpop));
            }
            if (milspending < 0)
            {
                milspending = 0;
            }
            pop = ruralpop + urbanpop + milpop;
            food -= pop;
            wealth += (urbanpop * (100 + urbaneffper) / 100)/12;
            pwealth += wealth * taxper/100;
            wealth -= wealth * taxper/100;
            if (pwealth > 0)
            {
                pwealth -= milspending;
            }
            milpop = milspending * 100;
            
            if (oldmilspending < milspending && ruralpop - 12 * (milspending - oldmilspending) > 0)
            {
                miltime = miltime * oldmilspending / milspending; // adjusts years training because ofe new recriuts
                ruralpop -= 12 * (milspending - oldmilspending); // recruits military from pesantry
                oldmilspending = milspending;
            }
            else
            {
                pwealth += milspending - oldmilspending;
            }
    
            if (oldmilspending > milspending)
            {
                ruralpop -= 12 * (milspending - oldmilspending); // recruits military from pesantry
                oldmilspending = milspending;
            }
            
            if (wealth <= 0)
            {
                ruralpop += milpop;
                milpop = 0;
                miltime = 0;
            }
            if (month == 5)
            {
                food += (int)((farmeffper + 100) * ruralpop * 12 / 100 * (Random.value / 2 + 0.25) * 2);
            }
            if (food < 0)
            {
                food = 0;
            }
            if (food == 0)
            {
                ruralpop = ruralpop / 10 * 9;
                urbanpop = urbanpop / 10 * 9;
                ruralpop -= milpop / 10;
            }
            if (pop < 1000)
            {
                Destroy(this.gameObject);
            }
            if(miltime >= 30 * 12) // limits training bonus to 30 years
            {
                miltime = 30 * 12;
            }
            if (wealth < 0 || food < 0)
            {
                //GetComponent<Renderer>().material.color = new Color(255, 0, 0);
            }
            else
            {
                //GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            }
        }
        pop = ruralpop + urbanpop + milpop;
    }
}
