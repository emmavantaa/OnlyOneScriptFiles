using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameScript : MonoBehaviour
{
    // Lasi joka rikkoontuu kun ostu "gate:iin"
    public GameObject prefab;

    // Pelaajan rb 
    private Rigidbody rb;

    public float mspeed;
    public float speeder;

    public GameObject player;


    // Portit ja teksti

    private int Gates;

    public GameObject[] gate;

    public Text gateText;
    public Text goalScore;
    public Text timerText;
    public Text GoaltimerText;
    public float timerStart;

    private bool hitsGoal;

    public GameObject start;
    public GameObject goal;

    //Kaikki ihanat kamerat jotka aktivoidaan 

    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;
    public GameObject cam5;
    public GameObject cam6;
    public GameObject cam7;
    public GameObject cam8;

    public Animator anim;
    public GameObject allcam;

    public GameObject RestartMenu;



    public AudioSource gateSound;

    public AudioSource Sounds;


    bool playerMoves;


    public Animator animTree;

    float timer=0;
    float secondsT=0;

    public Transform effect;

    public GameObject CollectedAll;

    public GameObject soundON;
    public GameObject soundOff;

    public GameObject soundMusic;

    // Start is called before the first frame update
    void Start()
    {
      

        rb = GetComponent<Rigidbody>();
        Time.timeScale = 0;
        start.SetActive(true);
        goal.SetActive(false);
        Sounds = GetComponent<AudioSource>();
        playerMoves = false;
        timerStart = Time.time;

       
    }

    // Update is called once per frame
    void Update()
    {

        if (hitsGoal == true)
        {
            return;
        }

        else
        {
            float t = Time.time - timerStart;

            string minutes = ((int)t / 60).ToString();
            string seconds = ((t % 60).ToString("00"));

            timerText.text ="Time: "+ minutes + ":" + seconds;
            GoaltimerText.text = "Your time was: " + minutes + ":" + seconds;
        }

       
        // Efekti joka on pelaajan ympärillä
        effect.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

        GoalHit();
        PlayerController();
        GameUI();

        // Nopea restart tehty itseäni varten myös oli aikaisemmin camera missä pystyit katsomaan koko tason läpi, myös tehty itseäni varten. (E pysäyttää pelaajan kuvia varten ja Q vapauttaa)
   if (Input.GetKeyDown(KeyCode.Space))
        {
            //anim.SetBool("SpacePressed", true);
            //allcam.SetActive(false);
            RestartMenu.SetActive(true);
            Time.timeScale = 0;
        }

   if (Input.GetKeyDown(KeyCode.E))
        {
            rb.isKinematic = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            rb.isKinematic = false;
        }


        // Liittyy vikan tason lopetukseen ja if (gates == 14) antaa erikois efektin kun keräät kaikki viimeisestä tasosta
        if (timer == 1)
        {
            secondsT++;

            if (secondsT == 250)
            {
                goal.SetActive(true);
            }
        }

        if (Gates == 14)
        {
            CollectedAll.SetActive(true);
        }



    }


    // Hahmon liikkuminen
    void PlayerController()
    {
       float mVertical = Input.GetAxis("Vertical");
       float mHorizontal = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(mHorizontal, 0.0f, mVertical)* speeder;

        rb.AddForce(movement * mspeed);

    }



    private void OnTriggerEnter(Collider other)
    {

        // CAMERA
        if (other.gameObject.CompareTag("Camera1"))
        {
            cam1.SetActive(false);
            cam2.SetActive(true);
        }

        if (other.gameObject.CompareTag("Camera2"))
        {
            cam2.SetActive(false);
            cam3.SetActive(true);
        }

        if (other.gameObject.CompareTag("Camera3"))
        {
            cam3.SetActive(false);
            cam4.SetActive(true);
        }

        if (other.gameObject.CompareTag("Camera4"))
        {
            cam4.SetActive(false);
            cam5.SetActive(true);
        }

        if (other.gameObject.CompareTag("Camera5"))
        {
            cam5.SetActive(false);
            cam6.SetActive(true);
        }

        if (other.gameObject.CompareTag("Camera6"))
        {
            cam6.SetActive(false);
            cam7.SetActive(true);
        }


        // Pelin lopetuksesa tulee pieni pätkä kun pelaat viimeisen tason
        if (other.gameObject.CompareTag("End"))
        {
           
            cam7.SetActive(false);
            cam8.SetActive(true);
            rb.isKinematic = true;
            animTree.SetBool("HitsEnd", true);
           

            timer++;
            Debug.Log("Timer is: " + timer);
            if (timer == 150)
            {
                goal.SetActive(true);
            }
        }

        // COLLECT
        // katsoa että pelaaja osuu objektiin joka on tag:issa ja anna piste + pelaa ääni sekä pelaa efekti ja tuhoa objekti
        if (other.gameObject.CompareTag("Gate"))
        {
            gateSound.Play();
            Gates += 1;
            Destroy(other.gameObject);
            Instantiate(prefab, this.transform.position, Quaternion.identity);
           

        }
        // katsoa että pelaaja osuu objektiin joka on tag:issa ja pysöytä ns aika sitten avaa restart menu UI
        if (other.gameObject.CompareTag("Fall"))
        {
            RestartMenu.SetActive(true);
            Time.timeScale = 0;
        }

        // katsoa että pelaaja osuu objektiin joka on tag:issa sitten avaa voitto UI näkymä
        if (other.gameObject.CompareTag("Goal"))
        {

            hitsGoal = true;
            goal.SetActive(true);
            Time.timeScale = 0;
        }

        

    }

    // Gate UI texti kun pelaat
    void GameUI()
    {
        gateText.text = "Gates: "+ Gates + " / " + gate.Length;
    }


    // Aloita peli nappula
    public void StartGame()
    {

            Time.timeScale = 1;
            start.SetActive(false);

    }

    // Pelkkät gate:it mistä olet mennyt (Loppu score)
    public void GoalHit()
    {
        goalScore.text = Gates + "/" + gate.Length;
    }

    // Nappuloille jotta pääset eri tasoille

    public void Level1()
    {
        SceneManager.LoadScene("Level");
       
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level1");

    }

    public void Level3()
    {
        SceneManager.LoadScene("Level2");

    }

    public void SoundON()
    {
        soundOff.SetActive(true);
        soundON.SetActive(false);
        soundMusic.SetActive(false);
    }

    public void SoundOFF()
    {
        soundON.SetActive(true);
        soundOff.SetActive(false);
        soundMusic.SetActive(true);
    }

}
