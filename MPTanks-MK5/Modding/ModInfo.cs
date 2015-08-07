using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding
{
    public struct ModInfo
    {
        public string ModName { get; set; }
        public int ModMajor { get; set; }
        public int ModMinor { get; set; }
        public byte[] Encode()
        {
            var strlen = Encoding.UTF8.GetByteCount(ModName);
            var arr = new byte[2 + strlen + 4 + 4];
            Array.Copy(BitConverter.GetBytes((ushort)strlen), arr, 2);
            Array.Copy(Encoding.UTF8.GetBytes(ModName), 0, arr, 2, strlen);
            Array.Copy(BitConverter.GetBytes(ModMajor), 0, arr, 2 + strlen, 4);
            Array.Copy(BitConverter.GetBytes(ModMinor), 0, arr, 2 + 4 + strlen, 4);
            return arr;
        }
        public static ModInfo Decode(byte[] encoded)
        {
            var info = new ModInfo();
            var strlen = BitConverter.ToUInt16(encoded, 0);
            info.ModName = Encoding.UTF8.GetString(encoded, 2, strlen);
            info.ModMajor = BitConverter.ToInt32(encoded, 2 + strlen);
            info.ModMinor = BitConverter.ToInt32(encoded, 2 + strlen + 4);
            return info;
        }

        public bool IsLoaded
        {
            get
            {
                if (!ModDatabase.Contains(ModName, ModMajor)) return false;
                var mod = ModDatabase.Get(ModName, ModMajor);
                if (mod.Major < ModMajor) return false;
                foreach (var module in ModDatabase.LoadedModules)
                    if (mod.Name.Equals(
                        module.Name, StringComparison.InvariantCultureIgnoreCase) && module.Version.Major == ModMajor)
                        return true;

                return false;
            }
        }

        public Module LoadedModule
        {
            get
            {
                if (!IsLoaded) return null;
                return ModDatabase.GetLoaded(ModName, ModMajor);
            }
        }

        public bool LocalVersionIsLowerThanRequired
        {
            get
            {

                if (!ModDatabase.Contains(ModName, ModMajor)) return true;
                var mod = ModDatabase.Get(ModName, ModMajor);
                if (mod.Minor < ModMinor) return true;

                return false;
            }
        }

        public Module Load(string dllUnpackDir, string mapUnpackDir, string assetUnpackDir, bool overwriteExisting = false)
        {
            if (IsLoaded) return null;

            var mod = ModDatabase.Get(ModName, ModMajor);
            if (mod == null) return null;
            string errors;
            return ModLoader.LoadMod(mod.File, dllUnpackDir, mapUnpackDir, assetUnpackDir,
                 out errors, mod.UsesWhitelist, overwriteExisting);
        }
    }
}
