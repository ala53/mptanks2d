using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding
{
    public struct ModAssetInfo
    {
        public string AssetName { get; set; }
        public string AssetSystemPath
        {
            get
            {
                if (ModInfo.IsLoaded && ModInfo.LoadedModule.AssetMappings.ContainsKey(AssetName))
                    return ModInfo.LoadedModule.AssetMappings[AssetName];
                return null;
            }
        }
        public ModInfo ModInfo { get; set; }

        public byte[] Encode()
        {
            var infoEncoded = ModInfo.Encode();
            var strlen = Encoding.UTF8.GetByteCount(AssetName);
            var arr = new byte[2 + strlen + infoEncoded.Length];
            Array.Copy(BitConverter.GetBytes((ushort)strlen), arr, 2);
            Array.Copy(Encoding.UTF8.GetBytes(AssetName), 0, arr, 2, strlen);
            Array.Copy(infoEncoded, 0, arr, 2 + strlen, infoEncoded.Length);
            return arr;
        }

        public static ModAssetInfo Decode(byte[] data)
        {
            var info = new ModAssetInfo();
            var strlen = BitConverter.ToUInt16(data, 0);
            info.AssetName = Encoding.UTF8.GetString(data, 2, strlen);
            byte[] modInfoArr = new byte[data.Length - strlen - 2];
            Array.ConstrainedCopy(data, strlen + 2, modInfoArr, 0, modInfoArr.Length);
            info.ModInfo = ModInfo.Decode(modInfoArr);
            return info;
        }

        public byte[] ReadAsByteArray()
        {
            if (!ModInfo.IsLoaded) return null;
            return ModInfo.LoadedModule.GetPackedFileData(AssetName);
        }

        public string ReadAsString()
        {
            if (!ModInfo.IsLoaded) return null;
            return ModInfo.LoadedModule.GetPackedFileString(AssetName);
        }
    }
}
