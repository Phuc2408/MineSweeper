//using TMPro;
//using Unity.VisualScripting;
//using UnityEngine;

//public class GameOverScreen : MonoBehaviour
//{
//    private Game.InitDelegate initDelegate;
//    [SerializeField] TextMeshProUGUI timePlayed;
//    public void setUp(float timePlayed)
//    {
//        gameObject.SetActive(true);
//        int minutes = Mathf.FloorToInt(timePlayed / 60);
//        int seconds = Mathf.FloorToInt(timePlayed % 60);
//        this.timePlayed.text = string.Format("Time played: {0:00}:{1:00}", minutes, seconds);

//    }

//    public void playAgainButton()
//    {
//        this.gameObject.SetActive(false);

//        // Gọi delegate khởi tạo lại game
//        initDelegate?.Invoke();
//    }

//    public void setInitDelegate(Game.InitDelegate initDelegate)
//    {
//        this.initDelegate = initDelegate;
//    }
//}
using TMPro;
using UnityEngine;

public class GameOverScreen : ResultScreen
{
    public SaveBestResult saveBestResult;

    // Override phương thức SetBestTime để hiển thị thời gian tốt nhất khi game kết thúc
    protected override void SetBestTime(int width, int height)
    {
        float bestTime = saveBestResult.loadData(width, height);

        // Chuyển đổi thời gian tốt nhất sang phút và giây
        int bestMinutes = Mathf.FloorToInt(bestTime / 60);
        int bestSeconds = Mathf.FloorToInt(bestTime % 60);
        this.timeBest.text = string.Format("Best Time: {0:00}:{1:00}", bestMinutes, bestSeconds);
    }
}
