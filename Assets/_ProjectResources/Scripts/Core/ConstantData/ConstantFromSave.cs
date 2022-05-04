using UnityEngine;

namespace EmptySoul.AdultTwitch.Core.ConstantData
{
    public static class ConstantFromSave
    {
        public static readonly string DirectoryPath;

        private const string Postfix = ".save";
        
        static ConstantFromSave()
        {
            DirectoryPath = $"{Application.persistentDataPath}/UserSaves";
        }

        public static string UserSaveDirectoryPath(string userName)
        {
            return $"{Application.persistentDataPath}/UserSaves/{userName}_user";
        }
        
        public static string UserParamsSavePath(string userName)
        {
            return $"{Application.persistentDataPath}/UserSaves/{userName}_user/{userName}_params{Postfix}";
        }
        
        public static string GalleryProductsSavePath(string userName)
        {
            return $"{Application.persistentDataPath}/UserSaves/{userName}_user/{userName}_products{Postfix}";
        }
        public static string MiningProgressSavePath(string userName)
        {
            return $"{Application.persistentDataPath}/UserSaves/{userName}_user/{userName}_mining{Postfix}";
        }
    }
}