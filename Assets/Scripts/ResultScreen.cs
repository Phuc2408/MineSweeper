using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{
    protected Game.InitDelegate initDelegate;
    [SerializeField] protected TextMeshProUGUI timePlayed;
    [SerializeField] protected TextMeshProUGUI timeBest;

    public void setUp(float timePlayed, int width, int height)
    {
        gameObject.SetActive(true);

        // Hiển thị thời gian chơi
        int minutes = Mathf.FloorToInt(timePlayed / 60);
        int seconds = Mathf.FloorToInt(timePlayed % 60);
        this.timePlayed.text = string.Format("Played Time: {0:00}:{1:00}", minutes, seconds);

        // Gọi phương thức SetBestTime để hiển thị thời gian tốt nhất của mỗi màn hình
        SetBestTime(width, height);
    }

    // Phương thức sẽ được kế thừa và triển khai trong các lớp con
    protected virtual void SetBestTime(int width, int height)
    {
        // Cần được triển khai lại trong các lớp con
    }

    // Phương thức callback cho nút Play Again
    public void playAgainButton()
    {
        this.gameObject.SetActive(false);
        initDelegate?.Invoke(); // Gọi lại phương thức khởi động lại trò chơi
    }

    // Thiết lập InitDelegate (dùng để khởi động lại trò chơi)
    public void setInitDelegate(Game.InitDelegate initDelegate)
    {
        this.initDelegate = initDelegate;
    }
}
