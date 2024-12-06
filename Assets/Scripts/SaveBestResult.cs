using UnityEngine;
using System.IO;
using UnityEditor;

[System.Serializable]
public class Data
{
    public int width;
    public int height;
    public float timeplayed;

    public Data(int width, int height, float timeplayed)
    {
        this.width = width;
        this.height = height;
        this.timeplayed = timeplayed;
    }
}

public class SaveBestResult : MonoBehaviour
{
    // Đường dẫn lưu trữ file JSON trong thư mục persistentDataPath (sau khi build)
    private string path;

    private void Awake()
    {
        // Lưu vào thư mục persistentDataPath thay vì Assets
        path = Path.Combine(Application.persistentDataPath, "bestResult.json");
        Debug.Log("Dữ liệu sẽ được lưu vào: " + path);  // In ra để kiểm tra vị trí lưu trữ
    }

    // Hàm lưu dữ liệu mới hoặc cập nhật dữ liệu
    public void saveData(int width, int height, float timeplayed)
    {
        // Tạo đối tượng Data chứa thông tin mới
        Data newData = new Data(width, height, timeplayed);

        // Kiểm tra nếu file đã tồn tại
        if (File.Exists(path))
        {
            // Đọc dữ liệu từ file
            string json = File.ReadAllText(path);
            Data[] allData = JsonUtility.FromJson<DataArray>(json).data;

            // Kiểm tra xem đã có dữ liệu với cùng kích thước bàn chưa
            foreach (Data existingData in allData)
            {
                if (existingData.width == width && existingData.height == height)
                {
                    // Nếu có cùng kích thước bàn, chỉ lưu lại nếu thời gian chơi mới ngắn hơn
                    if (newData.timeplayed < existingData.timeplayed)
                    {
                        // Cập nhật dữ liệu mới nếu thời gian chơi ngắn hơn
                        existingData.timeplayed = newData.timeplayed;
                        saveAllData(allData);
                        Debug.Log("Cập nhật thời gian chơi ngắn hơn.");
                    }
                    return;  // Không cần thêm dữ liệu mới vào vì đã có
                }
            }

            saveNewData(newData, allData);  // Lưu dữ liệu mới
        }
        else
        {
            // Nếu file chưa tồn tại, tạo mới và lưu dữ liệu
            Data[] allData = new Data[] { newData };
            saveAllData(allData);
            Debug.Log("Dữ liệu mới đã được lưu vào file mới.");
        }
    }

    // Lưu dữ liệu mới vào file
    private void saveNewData(Data newData, Data[] existingData = null)
    {
        Data[] allData = existingData != null ? AddDataToArray(existingData, newData) : new Data[] { newData };
        saveAllData(allData);
        Debug.Log("Dữ liệu mới đã được lưu.");
    }

    // Hàm để thêm dữ liệu vào mảng
    private Data[] AddDataToArray(Data[] existingData, Data newData)
    {
        Data[] newArray = new Data[existingData.Length + 1];
        existingData.CopyTo(newArray, 0);
        newArray[existingData.Length] = newData;
        return newArray;
    }

    // Lưu tất cả dữ liệu vào file JSON
    private void saveAllData(Data[] allData)
    {
        // Chuyển mảng Data thành chuỗi JSON
        string json = JsonUtility.ToJson(new DataArray { data = allData }, true);

        // Ghi chuỗi JSON vào file
        File.WriteAllText(path, json);
        Debug.Log("Dữ liệu đã được lưu tại: " + path);  // In ra để kiểm tra vị trí lưu trữ
    }

    // Tải dữ liệu và trả về thời gian chơi theo kích thước bàn
    public float loadData(int width, int height)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data[] allData = JsonUtility.FromJson<DataArray>(json).data;

            // Duyệt qua tất cả các dữ liệu và tìm kiếm theo kích thước bàn
            foreach (Data data in allData)
            {
                if (data.width == width && data.height == height)
                {
                    // Trả về thời gian chơi nếu tìm thấy kích thước bàn khớp
                    Debug.Log($"Kích thước bàn: {data.width} x {data.height}, Thời gian chơi: {data.timeplayed} giây.");
                    return data.timeplayed;
                }
            }

            // Nếu không tìm thấy kích thước bàn trong dữ liệu
            Debug.Log("Không tìm thấy dữ liệu cho kích thước bàn này.");
            return -1f;  // Trả về -1 nếu không tìm thấy
        }
        else
        {
            Debug.Log("Không tìm thấy file lưu trữ.");
            return -1f;  // Trả về -1 nếu file không tồn tại
        }
    }
}

// Lớp trợ giúp để lưu mảng dữ liệu vào JSON
[System.Serializable]
public class DataArray
{
    public Data[] data;
}
