using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace MenuManagement.Data
{
    public class JSONSaver : MonoBehaviour
    {
        public static readonly string jsonFileName = "GameData.sav";

        public string GetFileName() { return Application.persistentDataPath+"/"+jsonFileName; }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void saveData(SaveData saveData)
        {
            string jsonData = JsonUtility.ToJson(saveData);
            saveData.hashValue = string.Empty;
            jsonData = JsonUtility.ToJson(saveData);
            saveData.hashValue = getHashStringFromText(jsonData);
            jsonData = JsonUtility.ToJson(saveData);
            FileStream fs = new FileStream(GetFileName(), FileMode.Create);
            
            
            //Debug.Log(GetFileName());
            //Debug.Log(jsonData);
            using (StreamWriter sw = new StreamWriter(fs)) {
                sw.Write(jsonData);
            }
        }

        public SaveData loadData(SaveData data)
        {
            string dat = "";
            Debug.Log(GetFileName());

            if (File.Exists(GetFileName()))
            {
                Debug.Log(GetFileName());
                FileStream fs = new FileStream(GetFileName(), FileMode.Open);
                using (StreamReader sw = new StreamReader(fs))
                {
                    dat = sw.ReadToEnd();
                    Debug.Log(dat);
                    JsonUtility.FromJsonOverwrite(dat,data);
                    if (!checkHash(data))
                    {
                        Debug.Log("hacked..resetting");
                        data = new SaveData();
                    }
                    else
                        Debug.Log("okay!");
                }
                return data;
            }
            return data;
        }

        public bool checkHash(SaveData data)
        {
            string oldHash = data.hashValue;
            data.hashValue = "";
            string jsonData = JsonUtility.ToJson(data);
            return getHashStringFromText(jsonData).Equals(oldHash);
        }
        public void deleteFile() {
            File.Delete(GetFileName());
        }

        public string GetHashStringFromHash(byte[] hash) {
            string hexString = string.Empty;
            foreach (byte b in hash) {
                hexString += b.ToString("x2");
            }
            return hexString;
        }

        public string getHashStringFromText(string json) {
            byte[] textToBytes = Encoding.UTF8.GetBytes(json);
            SHA256Managed mySHA256 = new SHA256Managed();
            byte[] hashValue = mySHA256.ComputeHash(textToBytes);
            return GetHashStringFromHash(hashValue);
        }

    }
}