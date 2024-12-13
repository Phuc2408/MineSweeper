//using UnityEngine;
//using System.IO;
//using UnityEditor;

//[System.Serializable]
//public class Data
//{
//    public int width;
//    public int height;
//    public float timeplayed;

//    public Data(int width, int height, float timeplayed)
//    {
//        this.width = width;
//        this.height = height;
//        this.timeplayed = timeplayed;
//    }
//}

//public class SaveBestResult : MonoBehaviour
//{
//    // Đường dẫn lưu trữ file JSON trong thư mục persistentDataPath (sau khi build)
//    private string path;

//    private void Awake()
//    {
//        // Lưu vào thư mục persistentDataPath thay vì Assets
//        path = Path.Combine(Application.persistentDataPath, "bestResult.json");
//        Debug.Log("Dữ liệu sẽ được lưu vào: " + path);  // In ra để kiểm tra vị trí lưu trữ
//    }

//    //Hàm lưu dữ liệu mới hoặc cập nhật dữ liệu
//    public void saveData(int width, int height, float timeplayed)
//    {
//        // Tạo đối tượng Data chứa thông tin mới
//        Data newData = new Data(width, height, timeplayed);

//        // Kiểm tra nếu file đã tồn tại
//        if (File.Exists(path))
//        {
//            // Đọc dữ liệu từ file
//            string json = File.ReadAllText(path);
//            Data[] allData = JsonUtility.FromJson<DataArray>(json).data;

//            // Kiểm tra xem đã có dữ liệu với cùng kích thước bàn chưa
//            foreach (Data existingData in allData)
//            {
//                if (existingData.width == width && existingData.height == height)
//                {
//                    // Nếu có cùng kích thước bàn, chỉ lưu lại nếu thời gian chơi mới ngắn hơn
//                    if (newData.timeplayed < existingData.timeplayed)
//                    {
//                        // Cập nhật dữ liệu mới nếu thời gian chơi ngắn hơn
//                        existingData.timeplayed = newData.timeplayed;
//                        saveAllData(allData);
//                        Debug.Log("Cập nhật thời gian chơi ngắn hơn.");
//                    }
//                    return;  // Không cần thêm dữ liệu mới vào vì đã có
//                }
//            }

//            saveNewData(newData, allData);  // Lưu dữ liệu mới
//        }
//        else
//        {
//            // Nếu file chưa tồn tại, tạo mới và lưu dữ liệu
//            Data[] allData = new Data[] { newData };
//            saveAllData(allData);
//            Debug.Log("Dữ liệu mới đã được lưu vào file mới.");
//        }
//    }

//    // Lưu dữ liệu mới vào file
//    private void saveNewData(Data newData, Data[] existingData = null)
//    {
//        Data[] allData = existingData != null ? AddDataToArray(existingData, newData) : new Data[] { newData };
//        saveAllData(allData);
//        Debug.Log("Dữ liệu mới đã được lưu.");
//    }

//    // Hàm để thêm dữ liệu vào mảng
//    private Data[] AddDataToArray(Data[] existingData, Data newData)
//    {
//        Data[] newArray = new Data[existingData.Length + 1];
//        existingData.CopyTo(newArray, 0);
//        newArray[existingData.Length] = newData;
//        return newArray;
//    }

//    // Lưu tất cả dữ liệu vào file JSON
//    private void saveAllData(Data[] allData)
//    {
//        // Chuyển mảng Data thành chuỗi JSON
//        string json = JsonUtility.ToJson(new DataArray { data = allData }, true);

//        // Ghi chuỗi JSON vào file
//        File.WriteAllText(path, json);
//        Debug.Log("Dữ liệu đã được lưu tại: " + path);  // In ra để kiểm tra vị trí lưu trữ
//    }

//    // Tải dữ liệu và trả về thời gian chơi theo kích thước bàn
//    public float loadData(int width, int height)
//    {
//        if (File.Exists(path))
//        {
//            string json = File.ReadAllText(path);
//            Data[] allData = JsonUtility.FromJson<DataArray>(json).data;

//            // Duyệt qua tất cả các dữ liệu và tìm kiếm theo kích thước bàn
//            foreach (Data data in allData)
//            {
//                if (data.width == width && data.height == height)
//                {
//                    // Trả về thời gian chơi nếu tìm thấy kích thước bàn khớp
//                    Debug.Log($"Kích thước bàn: {data.width} x {data.height}, Thời gian chơi: {data.timeplayed} giây.");
//                    return data.timeplayed;
//                }
//            }

//            // Nếu không tìm thấy kích thước bàn trong dữ liệu
//            Debug.Log("Không tìm thấy dữ liệu cho kích thước bàn này.");
//            return -1f;  // Trả về -1 nếu không tìm thấy
//        }
//        else
//        {
//            Debug.Log("Không tìm thấy file lưu trữ.");
//            return -1f;  // Trả về -1 nếu file không tồn tại
//        }
//    }
//}

//// Lớp trợ giúp để lưu mảng dữ liệu vào JSON
//[System.Serializable]
//public class DataArray
//{
//    public Data[] data;
//}

using UnityEngine;
using System.IO;

[System.Serializable]
public class Data
{
    // Private fields
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float timeplayed;

    // Constructor
    public Data(int width, int height, float timeplayed)
    {
        this.width = width;
        this.height = height;
        this.timeplayed = timeplayed;
    }

    // Properties to get private fields
    public int Width
    {
        get { return width; }
    }

    public int Height
    {
        get { return height; }
    }

    public float TimePlayed
    {
        get { return timeplayed; }
    }

    // Method to update time played
    public void UpdateTimePlayed(float newTimePlayed)
    {
        timeplayed = newTimePlayed;
    }
}

public class SaveBestResult : MonoBehaviour
{
    // Path to save the JSON file
    private string path;

    private void Awake()
    {
        // Store in persistentDataPath (after build)
        path = Path.Combine(Application.persistentDataPath, "bestResult.json");
        Debug.Log("Data will be saved to: " + path);  // Debug log for checking save location
    }

    // Save or update new data
    public void saveData(int width, int height, float timeplayed)
    {
        // Create a new Data object with the provided information
        Data newData = new Data(width, height, timeplayed);

        // Check if the file already exists
        if (File.Exists(path))
        {
            // Read data from the file
            string json = File.ReadAllText(path);
            Data[] allData = JsonUtility.FromJson<DataArray>(json).data;

            // Check if there's data with the same width and height
            foreach (Data existingData in allData)
            {
                if (existingData.Width == width && existingData.Height == height)
                {
                    // If the board size is the same, only update if the new time played is shorter
                    if (newData.TimePlayed < existingData.TimePlayed)
                    {
                        // Update data with shorter time played
                        existingData.UpdateTimePlayed(newData.TimePlayed);
                        saveAllData(allData);
                        Debug.Log("Updated time played with shorter duration.");
                    }
                    return;  // No need to add new data since it's already there
                }
            }

            saveNewData(newData, allData);  // Save the new data
        }
        else
        {
            // If the file doesn't exist, create a new file and save the data
            Data[] allData = new Data[] { newData };
            saveAllData(allData);
            Debug.Log("New data saved in a new file.");
        }
    }

    // Save new data into the file
    private void saveNewData(Data newData, Data[] existingData = null)
    {
        Data[] allData = existingData != null ? AddDataToArray(existingData, newData) : new Data[] { newData };
        saveAllData(allData);
        Debug.Log("New data saved.");
    }

    // Method to add new data to the existing array
    private Data[] AddDataToArray(Data[] existingData, Data newData)
    {
        Data[] newArray = new Data[existingData.Length + 1];
        existingData.CopyTo(newArray, 0);
        newArray[existingData.Length] = newData;
        return newArray;
    }

    // Save all data into the file
    private void saveAllData(Data[] allData)
    {
        // Convert Data array into a JSON string
        string json = JsonUtility.ToJson(new DataArray { data = allData }, true);

        // Write the JSON string into the file
        File.WriteAllText(path, json);
        Debug.Log("Data saved to: " + path);  // Debug log for checking save location
    }

    // Load data and return the time played for the given board size
    public float loadData(int width, int height)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data[] allData = JsonUtility.FromJson<DataArray>(json).data;

            // Loop through all data to find the matching board size
            foreach (Data data in allData)
            {
                if (data.Width == width && data.Height == height)
                {
                    // Return time played if a match is found
                    Debug.Log($"Board size: {data.Width} x {data.Height}, Time played: {data.TimePlayed} seconds.");
                    return data.TimePlayed;
                }
            }

            // If no matching board size is found
            Debug.Log("No data found for this board size.");
            return -1f;  // Return -1 if not found
        }
        else
        {
            Debug.Log("No storage file found.");
            return -1f;  // Return -1 if the file doesn't exist
        }
    }
}

// Helper class to store the array of Data objects for JSON serialization
[System.Serializable]
public class DataArray
{
    public Data[] data;
}
