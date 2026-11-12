using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
 public GameObject ball;
 private int score = 0;
 private GameObject[] pins;
 public TextMeshProUGUI scoreText;

  void Start()
  {
      pins = GameObject.FindGameObjectsWithTag("Pin"); //these r the pins
  }

  void Update()
  {
      if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame || ball.transform.position.z > 0) // either count on reset or when ball leaves defined range
      {
          CountPinsDown();
      }
  }

  void CountPinsDown()
  {
      for (int i = 0; i < pins.Length; i++) // checking for rotation smaller than -5 or larger than 5.
      {
          if (pins[i].transform.eulerAngles.z > 5 && pins[i].transform.eulerAngles.z < 355 && pins[i].activeSelf)  // easier than rotations, we want angle to be larger than 5 and smaller than 355. also checks if the pin is active
          {
              score++;
              pins[i].SetActive(false); // prevents pins from being counted more than once, i cannot think of a good way to do this so... Begone Foul Beasts
          }
      }
      scoreText.text = score.ToString();
      Debug.Log("the score is " + score);
  }
}
