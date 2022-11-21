using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody rig;

    private float startTime;
    private float timeTaken;

    private int collectablesPicked;
    public int maxCollectables = 10;

    private bool isPlaying;

    public GameObject playButton;
    public TextMeshProUGUI curTimeText;

    //Mouse counter
    public TextMeshProUGUI mouseCounter;

    //Floating text TEST//
    public GameObject floatingText; 


    void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isPlaying)
            return;

        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        rig.velocity = new Vector3(x, rig.velocity.y, z);

        curTimeText.text = (Time.time - startTime).ToString("F2");

        //Floating text code//
        if (collectablesPicked <= 0)
        {
            floatingText.GetComponent<TextMeshPro>().text = "Hello furry adventurer! \n I need your help catching the pesky mice! \n Here is one now!";
        }

        if (collectablesPicked > 0 && collectablesPicked < 5)
        {
            floatingText.GetComponent<TextMeshPro>().text = "I saw mice near the \n -church, \n -behind the tree to the right \n -mossy grave";
        }

        if(collectablesPicked >= 5 && collectablesPicked < 7)
        {
            floatingText.GetComponent<TextMeshPro>().text = "Almost there! I saw mice near the \n -coffin, \n -behind a shovel \n -infront of a cross";
        }

        if(collectablesPicked >= 7)
        {
            floatingText.GetComponent<TextMeshPro>().text = "One more to go! He has to be around here somewhere!";
        }

    }

    public void Begin()
    {
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);

        //Hide the leaderboard?
        Leaderboard.instance.leaderboardCanvas.SetActive(false);
    }

    void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
       
        Leaderboard.instance.SetLeaderBoardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
        playButton.SetActive(true);

        //Show the leaderboard
        Leaderboard.instance.leaderboardCanvas.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Collectable"))
        {
            collectablesPicked++;
            mouseCounter.GetComponent<TextMeshProUGUI>().text = collectablesPicked + "/" + maxCollectables; 
            Destroy(other.gameObject);

            if (collectablesPicked == maxCollectables)
                End();
        }
    }
}
