using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    bool isRunning = true;
    // Update is called once per frame
    public void Running()
    {
        this.isRunning = true;   
    }
    public void NotRunning()
    {
        this.isRunning=false;
    }
    public void ResetTime()
    {   
        elapsedTime = 0f;
        isRunning = false;
    }
    public float GetTimer()
    {
        return elapsedTime;
    }
    public void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes, seconds;
        if (isRunning)
        {
            minutes = Mathf.FloorToInt(elapsedTime / 60);
            seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes,seconds);
        }
    }
}
