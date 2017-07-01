using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using QBA;

public class MonoHPCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float HP = 100.0f;
    public float MAXHP = 100.0f;
    public int shields;
    public int fire, water, earth, wind;
    public int dot;
    public int pdot;

    public void Strike(QBAEffect eff)
    {
        if (eff.shield == 0)
        {
            if (eff.revDMG)
            {
                HP += eff.dmg;
            }
            else
            {
                if(eff.pure) {
                    HP -= eff.dmg;
                }
                else if (eff.fire == 0 && eff.water == 0 && eff.earth == 0 && eff.wind == 0)
                {
                    float dmg = eff.dmg;
                    for (int i = 0; i < shields; i++)
                    {
                        dmg *= 0.95f;
                    }
                    HP -= dmg;
                }
                else if ((fire < eff.fire || fire == 0) && (water < eff.water || water == 0) && (earth < eff.earth || earth == 0) && (wind < eff.wind || wind == 0))
                {
                    float dmg = eff.dmg;
                    for (int i = 0; i < shields; i++)
                    {
                        dmg *= 0.95f;
                    }
                    fire = 0;
                    water = 0;
                    earth = 0;
                    wind = 0;
                    shields = 0;
                    HP -= dmg;
                }
            }
        }
        else
        {
            if (eff.revshield)
            {
                shields -= eff.shield;
            }
            else
            {
                fire = Mathf.Max(fire, eff.fire);
                water = Mathf.Max(water, eff.water);
                earth = Mathf.Max(earth, eff.earth);
                wind = Mathf.Max(wind, eff.wind);
                shields += eff.shield;
            }
        }
        if (eff.revdot)
        {
            dot -= eff.dot;
        }
        else
        {
            dot += eff.dot;
        }
        if (eff.speed > 0)
        {
            if (eff.revspeed)
            {
                float mult = 0.8f;
                for (int i = 0; i < eff.speed - 1; i++)
                {
                    mult *= 0.95f;
                }
                this.GetComponent<MonoPlayer_Cntrl>().Slow(mult);
            }
            else
            {
                float mult = 1.2f;
                for (int i = 0; i < eff.speed - 1; i++)
                {
                    mult *= 1.05f;
                }
                this.GetComponent<MonoPlayer_Cntrl>().Slow(mult);
            }
        }
        if(HP < 0)
        {
            Die();
        }
    }

    public void Antidote()
    {
        dot = 0;
        pdot = 0;
    }

    public void Fall(float dist)
    {

    }

    void Die (){

    }

    void Respawn()
    {
        dot = 0;
        pdot = 0;
        HP = MAXHP;
        shields = 0;
        fire = 0;
        water = 0;
        earth = 0;
        wind = 0;
        this.GetComponent<MonoPlayer_Cntrl>().Respawn();
        this.GetComponent<MonoPlayer>().Respawn();
    }

    float lastTick = 0;

    void Update2()
    {
        if (Time.time > lastTick + 1)
        {
            HP -= dot;
            HP -= pdot * MAXHP;
            lastTick = Time.time;
            if (HP <= 0)
            {
                Die();
            }
        }
    }

	// Update is called once per frame
	void Update ()
    {
        MonogameController.Update2();
        if (!gameController.paused)
            Update2();
    }
}
