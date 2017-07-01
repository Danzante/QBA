using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using QBA;

public class MonoPlayer: MonoBehaviour {
    Ray myray;
    RaycastHit help;
    const float dist = 5;

    public GameObject parrow;
    public GameObject pdisk;
    public GameObject psmoke;
    public GameObject pbomb;
    public GameObject pgas;

    GameObject Head;
    GameObject Body;

    float v1 = 0;
    float v2 = 0;

    // Use this for initialization
    void Start ()
    {
        CheckPlayerName();
        Body = GameObject.Find(this.gameObject.name + "/Body");
        Head = GameObject.Find(this.gameObject.name + "/Head");
        InitWords();
        InitDiWeapon();
        InitCharCords();
    }

    string[] words;

    int wordnum = 0;
    const int maxwordnum = 10;

    void InitWords()
    {
        words = new string[maxwordnum];
        if (File.Exists("Words.fdk"))
        {
            StreamReader sr = new StreamReader("Words.fdk");
            string s;
            while((s = sr.ReadLine()) != null) {
                s = s.ToLower();
                words[wordnum] = s;
                wordnum++;
                string s1 = sr.ReadLine();
                s1 = s1.ToLower();
                if (!PlayerPrefs.HasKey(s))
                {
                    PlayerPrefs.SetString(s, s1);
                    PlayerPrefs.SetInt(s1, -wordnum);
                }
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Respawn()
    {
        wearth = 0;
        wwater = 0;
        wwind = 0;
        wearth = 0;
    }

    void CheckPlayerName()
    {
        if (this.gameObject.name == "Player(Clone)")
            this.gameObject.name = "Player" + MonogameController.GetNewID();
    }

    void Update()
    {
        if (!MonogameController.paused)
        {
            Update2();
        }
    }
    
    void StartArrow(float dmg)
    {
        GameObject arrow = GameObject.Instantiate(parrow);
        arrow.transform.position = Head.transform.position + Head.transform.forward;
        arrow.transform.rotation = Head.transform.rotation;
        arrow.transform.Rotate(new Vector3(90, 0, 0));
        QBAEffect eff = new QBAEffect(dmg);
        arrow.GetComponent<MonoArrowCtrl>().Init(eff);
    }
    
    void StartSmoke()
    {
        GameObject smoke = GameObject.Instantiate(psmoke);
        smoke.transform.position = Head.transform.position - Head.transform.up * 2;
    }

    void StartDisk(float vel, bool t)
    {
        GameObject disk = GameObject.Instantiate(pdisk);
        disk.transform.position = Head.transform.position + Head.transform.forward * 1f;
        disk.transform.rotation = Head.transform.rotation;
        disk.GetComponent<MonoDiskCtrl>().Init(vel, t);
    }

    bool writing = false;

    void StartWriting()
    {
        spell = "";
        writing = true;
    }

    public string spell;

    void RewriteSpell()
    {
        spell = spell.ToLower();
        int l = spell.Length;
        string s = "";
        for(int i = 0; i < l; i++)
        {
            if((spell[i] > 'a')&&(spell[i] <= 'z'))
            {
                if ((spell[i] != 's')&&(spell[i] != 'w')&&(spell[i] != 'd'))
                {
                    s += spell[i];
                }
            }
        }
        spell = s;
    }

    bool sreversed;
    bool smultrev;
    bool sreversedArr;
    bool srevdot;
    bool srevspeed;
    bool srevshield;
    int sfire, swater, searth, swind;
    int sarrows;
    int sspeed;
    int sshield;
    int sdot;

    int[] charCords;

    void InitCharCords()
    {
        charCords = new int[26];
        charCords[1] = 205;
        charCords[2] = 203;
        charCords[4] = 3;
        charCords[5] = 104;
        charCords[6] = 105;
        charCords[7] = 106;
        charCords[8] = 8;
        charCords[9] = 107;
        charCords[10] = 108;
        charCords[11] = 109;
        charCords[12] = 207;
        charCords[13] = 206;
        charCords[14] = 9;
        charCords[15] = 10;
        charCords[16] = 1;
        charCords[17] = 4;
        charCords[19] = 5;
        charCords[20] = 7;
        charCords[21] = 204;
        charCords[23] = 202;
        charCords[24] = 6;
        charCords[25] = 201;
    }

    int CharInd(char c1, char c2)
    {
        int i1 = c1 - 'a';
        int i2 = c2 - 'a';
        int res = 0;
        i1 = charCords[i1];
        i2 = charCords[i2];
        res += i1 / 100 - i2 / 100;
        i2 += 100 * res;
        res = Mathf.Abs(res);
        res += Mathf.Abs(i1 - i2);
        return res;
    }

    int CountIndex(string w1, string w2)
    {
        int cur = 0;
        for(int i = 0; i < w1.Length; i++)
        {
            cur += CharInd(w1[i], w2[i]);
        }
        return cur;
    }

    void ExecuteWord(string word)
    {
        int index = 1000000;
        string meaning = "";
        string w = "";
        for (int i = 0; i < wordnum; i++) {
            int ind = CountIndex(word, words[i]);
            if(ind < index)
            {
                index = ind;
                w = words[i];
                meaning = PlayerPrefs.GetString(w);
            }
            else
            {
                if(ind == index)
                {
                    if(PlayerPrefs.GetInt(meaning) < PlayerPrefs.GetInt(PlayerPrefs.GetString(words[i])))
                    {
                        index = ind;
                        w = words[i];
                        meaning = PlayerPrefs.GetString(w);
                    }
                }
            }
        }
        int val = PlayerPrefs.GetInt(meaning);
        PlayerPrefs.SetInt(meaning, val + 1);
        if(meaning == "arr")
        {
            if (sreversed)
            {
                sreversedArr = true;
            }
            else
            {
                if (smultrev)
                {
                    sreversedArr = false;
                }
            }
            sarrows += 1;
            sreversed = false;
            smultrev = false;
        }
        if (meaning == "shl")
        {
            if (sreversed)
            {
                srevshield = true;
            }
            else
            {
                if (smultrev)
                {
                    srevshield = false;
                }
            }
            sshield += 1;
            sreversed = false;
            smultrev = false;
        }
        if (meaning == "dot")
        {
            if (sreversed)
            {
                srevdot = true;
            }
            else
            {
                if (smultrev)
                {
                    srevdot = false;
                }
            }
            sdot += 1;
            sreversed = false;
            smultrev = false;
        }
        if (meaning == "spd")
        {
            if (sreversed)
            {
                srevspeed = true;
            }
            else
            {
                if (smultrev)
                {
                    srevspeed = false;
                }
            }
            sspeed += 1;
            sreversed = false;
            smultrev = false;
        }
        if (meaning == "fre")
        {
            if (!sreversed)
            {
                sfire += 1;
            }
            else
            {
                swater += 1;
            }
            sreversed = false;
            smultrev = false;
        }
        if (meaning == "wnd")
        {
            if (!sreversed)
            {
                swind += 1;
            }
            else
            {
                searth += 1;
            }
            sreversed = false;
            smultrev = false;
        }
        if (meaning == "ert")
        {
            if (!sreversed)
            {
                searth += 1;
            }
            else
            {
                swind += 1;
            }
            sreversed = false;
            smultrev = false;
        }
        if (meaning == "wat")
        {
            if (sreversed)
            {
                sfire += 1;
            }
            else
            {
                swater += 1;
            }
            sreversed = false;
            smultrev = false;
        }
        if (meaning == "rev")
        {
            if (sreversed)
            {
                smultrev = true;
            }
            sreversed = !sreversed;
        }
        else
        {
            sreversed = false;
        }
    }

    void InitMagicParams()
    {
        sreversed = false;
        smultrev = false;
        srevdot = false;
        sreversedArr = false;
        srevshield = false;
        srevspeed = false;
        sfire = 0;
        swater = 0;
        swind = 0;
        searth = 0;
        sarrows = 0;
        sspeed = 0;
        sshield = 0;
        sdot = 0;
    }

    void ExecuteSpell()
    {
        InitMagicParams();
        int l = spell.Length;
        string s = "";
        for (int i = 0; i < l; i++)
        {
            s += spell[i];
            if(s.Length == 4)
            {
                ExecuteWord(s);
                s = "";
            }
        }
        if(s.Length != 0)
        {
            ExecuteWord(s);
            s = "";
        }
        ExecuteBuildedSpell();
    }

    int wfire, wwater, wearth, wwind;

    void ExecuteSelf()
    {
        if (sshield == 0) {
            wfire = sfire;
            wwind = swind;
            wwater = swater;
            wearth = searth;
        }
        QBAEffect eff = new QBAEffect(false, srevdot, srevspeed, srevshield, sfire, swater, searth, swind,sspeed, sshield, sdot, 0);
        this.GetComponent<MonoHPCtrl>().Strike(eff);
    }

    void ExecuteMArrow()
    {
        QBAEffect eff = new QBAEffect(sreversedArr, srevdot, srevspeed, srevshield, sfire, swater, searth, swind, sspeed, sshield, sdot, sarrows);
        GameObject arrow = GameObject.Instantiate(parrow);
        arrow.transform.position = Head.transform.position + Head.transform.forward * 0.6f;
        arrow.transform.rotation = Head.transform.rotation;
        arrow.transform.Rotate(new Vector3(90, 0, 0));
        arrow.GetComponent<MonoArrowCtrl>().Init(eff);
    }

    void ExecuteBuildedSpell()
    {
        if(sarrows == 0)
        {
            ExecuteSelf();
        }
        else
        {
            ExecuteMArrow();
        }
    }

    void StopWriting()
    {
        ReinSpell.SetActive(false);
        writing = false;
        RewriteSpell();
        ExecuteSpell();
    }

    void WritingUPD()
    {
        ReinSpell.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StopWriting();
        }
        spell += Input.inputString;
        RewriteSpell();
        ReinSpell.GetComponent<Text>().text = spell;
    }

    public int type;

    void Update2()
    {
        if(type == 0)
        {
            PlayRein();
        }
        else if(type == 1)
        {
            PlayAzi();
        }
        else if(type == 2)
        {
            PlayDi();
        }
        else if(type == 3)
        {
            PlayAeschyl();
        }
    }

    void StartReinUlti()
    {
        QBAEffect eff = new QBAEffect(20, true);
        GameObject arrow = GameObject.Instantiate(parrow);
        arrow.transform.position = Head.transform.position + Head.transform.forward;
        arrow.transform.rotation = Head.transform.rotation;
        arrow.transform.Rotate(new Vector3(90, 0, 0));
        arrow.GetComponent<MonoArrowCtrl>().Init(eff, 0.5f);
    }

    Vector3 ReinPoint1;
    Vector3 ReinPoint2;

    void StartReinSword()
    {
        ReinSword.GetComponent<ReinSwordCtrl>().init(ReinPoint1, ReinPoint2);
    }

    // Update is called once per frame
    void PlayRein()
    {
        ReinSword.SetActive(true);
        ReinHint.SetActive(true);
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    myray = new Ray(transform.position, transform.forward);
        //    Physics.Raycast(myray, out help, dist);
        //    if (help.collider.gameObject.tag == "Activable")
        //    {
        //        help.collider.gameObject.GetComponent<MonoActivateCtrl>().Activate();
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ReinPoint1 = Head.transform.position + Head.transform.forward * 2.4f - this.transform.position;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            ReinPoint2 = Head.transform.position + Head.transform.forward * 2.4f - this.transform.position;
            StartReinSword();
        }
        if (writing)
        {
            WritingUPD();
            return;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartWriting();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartReinUlti();
        }
    }

    void PlayAzi()
    {

    }

    int DiType = 0;

    GameObject DiShield = null;
    GameObject DiSword1 = null;
    GameObject DiSword2 = null;
    GameObject DiWings = null;
    GameObject ReinSword = null;
    GameObject ReinHint = null;
    GameObject ReinSpell = null;
    GameObject HP = null;

    void InitDiWeapon()
    {
        if (ReinSword == null)
        {
            ReinSword = GameObject.Find(this.gameObject.name + "/" + Body.name + "/ReinSword");
        }
        ReinSword.SetActive(false);
        if (ReinHint == null)
        {
            ReinHint = GameObject.Find(this.gameObject.name + "/Canvas/ReinHint");
        }
        ReinHint.SetActive(false);
        if (ReinSpell == null)
        {
            ReinSpell = GameObject.Find(this.gameObject.name + "/Canvas/ReinSpell");
        }
        ReinSpell.SetActive(false);
        if (DiShield == null)
        {
            DiShield = GameObject.Find(this.gameObject.name + "/" + Head.name + "/Shield");
        }
        DiShield.SetActive(false);
        if (DiSword1 == null)
        {
            DiSword1 = GameObject.Find(this.gameObject.name + "/" + Body.name + "/LeftSword");
        }
        if (DiSword2 == null)
        {
            DiSword2 = GameObject.Find(this.gameObject.name + "/" + Body.name + "/RightSword");
        }
        DiSword1.SetActive(false);
        DiSword2.SetActive(false);
        if (DiWings == null)
        {
            DiWings = GameObject.Find(this.gameObject.name + "/" + Body.name + "/Wings");
        }
        DiWings.SetActive(false);
    }

    float lastDiBow = -100;


    void PlayDi()
    {
        if(DiType == 0 || Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)
            || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)
            || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            DiType = 1;
            InitDiWeapon();
            this.GetComponent<MonoPlayer_Cntrl>().StopFlying();
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            DiType = 1;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            DiType = 2;
            DiSword1.SetActive(true);
            DiSword2.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            DiType = 3;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            DiType = 4;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            DiType = 5;
            DiWings.SetActive(true);
            this.GetComponent<MonoPlayer_Cntrl>().Fly();
        }
        if (DiType == 1)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)&&(Time.time - lastDiBow > 2.5f))
            {
                lastDiBow = Time.time;
                StartArrow(50);
            }
        }
        if(DiType == 2)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector3 p = Head.transform.position + Head.transform.forward * 1.1f;
                DiSword1.GetComponent<SwordCtrl>().init(p);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Vector3 p = Head.transform.position + Head.transform.forward;
                DiSword2.GetComponent<SwordCtrl>().init(p);
            }
        }
        if (DiType == 3)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                v1 += 2 * Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                StartDisk(v1, true);
                v1 = 0;
            }
            if (Input.GetKey(KeyCode.Mouse1))
            {
                v2 += 2 * Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                StartDisk(v2, false);
                v2 = 0;
            }
        }
        if(DiType == 4)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                DiShield.SetActive(true);
                DiShield.GetComponent<ShieldCtrl>().init();
            }
            if (!DiShield.GetComponent<ShieldCtrl>().active)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    DiShield.SetActive(true);
                }
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    DiShield.SetActive(false);
                }
            }
        }   
    }

    float lastSmoke = -100;
    float lastPGas = -100;
    float lastADote = -100;
    float lastMine = -100;
    float lastSpeed = -100;

    void PlayAeschyl()
    {
        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) && (Time.time - lastSmoke > 10f))
        {
            lastSmoke = Time.time;
            StartSmoke();
        }
        if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) && (Time.time - lastPGas > 10f))
        {
            lastPGas = Time.time;
            //StartPoisonGas();
        }
        if ((Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) && (Time.time - lastADote > 8f))
        {
            lastADote = Time.time;
            this.gameObject.GetComponent<MonoHPCtrl>().Antidote();
        }
        if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) && (Time.time - lastMine > 10f))
        {
            lastMine = Time.time;
            //StartMine();
        }
        if ((Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) && (Time.time - lastSpeed > 20f))
        {
            lastSpeed = Time.time;
            float hp = this.gameObject.GetComponent<MonoHPCtrl>().HP;
            float mhp = this.gameObject.GetComponent<MonoHPCtrl>().MAXHP;
            this.gameObject.GetComponent<MonoPlayer_Cntrl>().Slow(1 + (mhp - hp) / hp);
        }
    }
}
