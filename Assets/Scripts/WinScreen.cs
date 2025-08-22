//using TMPro;
//using UnityEngine;

//public class WinScreen : MonoBehaviour
//{
//    private Game.InitDelegate initDelegate;
//    public SaveBestResult saveBestResult;
//    [SerializeField] TextMeshProUGUI timePlayed;
//    [SerializeField] TextMeshProUGUI timeBest;
//    // Hiển thị màn hình chiến thắng
//    public void setUp(float timePlayed, int width, int height)
//    {
//        Debug.Log("winscreen");
//        gameObject.SetActive(true);
//        int minutes = Mathf.FloorToInt(timePlayed / 60);
//        int seconds = Mathf.FloorToInt(timePlayed % 60);
//        this.timePlayed.text = string.Format("Time played: {0:00}:{1:00}", minutes, seconds);
//        float best= saveBestResult.loadData(width, height);
//        int bestMinutes = Mathf.FloorToInt(best / 60);
//        int bestSeconds = Mathf.FloorToInt(best % 60);
//        this.timeBest.text = string.Format("Time best: {0:00}:{1:00}", bestMinutes, bestSeconds);
//    }

//    // Hàm Play Again khi nhấn nút
//    public void playAgainButton()
//    {
//        // Tắt màn hình Win
//        this.gameObject.SetActive(false);

//        // Gọi delegate để khởi động lại trò chơi
//        initDelegate?.Invoke();  // Nếu delegate không phải null, gọi lại phương thức _start()
//    }

//    // Thiết lập InitDelegate
//    public void setInitDelegate(Game.InitDelegate initDelegate)
//    {
//        this.initDelegate = initDelegate;
//    }
//}
using TMPro;
using UnityEngine;

public class WinScreen : ResultScreen
{
    public SaveBestResult saveBestResult;

    // Override phương thức SetBestTime để hiển thị thời gian tốt nhất khi chiến thắng
    protected override void SetBestTime(int width, int height)
    {
        float bestTime = saveBestResult.loadData(width, height);

        // Chuyển đổi thời gian tốt nhất sang phút và giây
        int bestMinutes = Mathf.FloorToInt(bestTime / 60);
        int bestSeconds = Mathf.FloorToInt(bestTime % 60);
        this.timeBest.text = string.Format("Best time: {0:00}:{1:00}", bestMinutes, bestSeconds);
    }
}
