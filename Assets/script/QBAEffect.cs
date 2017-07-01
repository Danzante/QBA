using System;

namespace QBA
{
    public class QBAEffect
    {
        public float dmg;
        public float pdmg;
        public bool revDMG;
        public bool revdot;
        public bool revspeed;
        public bool revshield;
        public int fire, water, earth, wind;
        public bool pure;
        public int speed;
        public int shield;
        public int dot;
        public float pdot;

        void init()
        {
            dmg = 0;
            pdmg = 0;
            revDMG = false;
            revdot = false;
            revspeed = false;
            revshield = false;
            fire = 0;
            water = 0;
            earth = 0;
            wind = 0;
            pure = false;
            speed = 0;
            shield = 0;
            dot = 0;
            pdot = 0;
        }

        public QBAEffect()
        {
            init();
            dmg = 50;
        }

        public QBAEffect(float DMG)
        {
            init();
            dmg = DMG;
        }

        public QBAEffect(float pdmg, bool PURE)
        {
            init();
            pure = PURE;
            pdot = pdmg;
        }

        public QBAEffect(bool revArr, bool revDot, bool revSpeed, bool revShield, int Fire, int Water, int Earth, int Wind, int Speed, int Shield, int Dot, int Arr)
        {
            init();
            revDMG = revArr;
            revdot = revDot;
            revspeed = revSpeed;
            revshield = revShield;
            fire = Fire;
            water = Water;
            earth = Earth;
            wind = Wind;
            speed = Speed;
            shield = Shield;
            dot = 5 * Dot;
            if(Shield == 0 && Arr == 1)
            {
                dmg = 100;
            }
            if (Arr >= 2)
            {
                shield = 0;
                dmg = 100;
                Arr--;
                for (int i = 0; i < Arr; i++)
                {
                    dmg *= 1.1f;
                }
            }
        }
    }
}