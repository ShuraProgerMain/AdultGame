using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace EmptySoul.AdultTwitch.Core.UserData
{
    public static class WorkWithBinary
    {
        public static string GetBinaryData(string path)
        {
            if (!File.Exists(path)) return string.Empty;
            
            var dataStream = new FileStream(path, FileMode.Open);
                
            var converter = new BinaryFormatter();
            
            if (converter.Deserialize(dataStream) is byte[] saveData)
            {
                dataStream.Close();
                var rasparse = Crypt(saveData);

                Debug.Log($"encode: {Encoding.Default.GetString(rasparse)}");
                return Encoding.Default.GetString(rasparse);
            }

            return "";
        }
        
        private static byte[] Crypt(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] ^= 1;
            return bytes;
        }

        public static void SaveToBinary(string path, string data)
        {
            var dataStream = new FileStream(path, FileMode.Create);
            
            var converter = new BinaryFormatter();
            
            var khm = Encoding.Default.GetBytes(data);
            var arr = Crypt(khm);
            converter.Serialize(dataStream, arr);

            dataStream.Close();    
        }
    }
}