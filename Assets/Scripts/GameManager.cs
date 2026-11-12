using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ball;
    private int score = 0;
    private GameObject[] pins; //trying this method for pin knockdown + score! https://www.youtube.com/watch?v=bykFPogxvv4
    public TMP_Text scoreUI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pins = GameObject.FindGameObjectsWithTag("Pin");
    }

    // Update is called once per frame
    void Update()
    {
        if (ball.transform.position.y < 20)
        {
            CountPinsDown();
        }
        
        if (score == 10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        
    }

    void CountPinsDown()
    {
        //about to use a for loop... scary
        for (int i = 0; i < pins.Length; i++) //wait i think ive done this before... many a year ago in undergrad... wait i feel it in my bones...
        {
            //checking angles time. I will use z to determine if a pin is knocked down.  
            if (pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355 && //researching euler angles... yeah i know euler... i know quaternion 
            pins[i].activeSelf)
            {
                score++;
                pins[i].SetActive(false);
            }
        }

        scoreUI.text = score.ToString();
    }
           
}
