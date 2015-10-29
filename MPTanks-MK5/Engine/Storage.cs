using MPTanks.Engine.Settings;
using MPTanks.Modding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public class Storage
    {
        private static Dictionary<ModInfo, int> _offsetTable =
            new Dictionary<ModInfo, int>();
        private static Dictionary<ModInfo, byte[]> _infos = new Dictionary<ModInfo, byte[]>();
        private static string _fileName
        {
            get
            {
                return Path.Combine(SettingsBase.ConfigDir, "modpersistentdb.db");
            }
        }
        private static int _offsetEnd
        {
            get
            {
                var fi = new FileInfo(_fileName);
                if (!fi.Exists) return 0;
                return checked((int)fi.Length);
            }
        }

        internal static void Load()
        {
            //Structured as a set of things (each block is 4KB):
            //For each:
            //  2 bytes name length
            /// UTF8 encoded name
            /// 4 bytes major version
            /// 2 bytes contents length

            var segments = new Dictionary<int, byte[]>();
            var fs = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            for (var i = 0; i < _offsetEnd; i += 4096)
            {
                byte[] data = new byte[4096];
                if (fs.Length - i < 4096) break;
                fs.Read(data, i, 4096);
                segments.Add(i, data);
            }

            fs.Close();
            fs.Dispose();
            //And process
            foreach (var seg in segments)
            {
                var reader = Helpers.ByteArrayReader.Get(seg.Value);
                var name = reader.ReadString();
                var major = reader.ReadInt();
                var contents = reader.ReadBytes();

                var info = new ModInfo()
                {
                    ModMajor = major,
                    ModMinor = 0,
                    ModName = name
                };
                _offsetTable.Add(info, seg.Key);
                _infos.Add(info, contents);

                reader.Release();
            }


        }
        /// <summary>
        /// Stores up to 4 KB of data for a module in the persistent storage of the game.
        /// Can be arbitrary binary data.
        /// Note that this includes the length of the module name.
        /// E.g. 
        /// 2 bytes for name length
        /// UTF8 encoded name
        /// 4 bytes major version
        /// 2 bytes contents length
        /// </summary>
        /// <param name="data"></param>
        /// <param name="module"></param>
        public static void SetPersistentData(byte[] data, Module module)
        {
            var offset = _offsetEnd;
            if (_infos.ContainsKey(GetInfo(module)))
                offset = _offsetTable[GetInfo(module)];

            var serialized = Helpers.SerializationHelpers.AllocateArray(true,
                 module.ModInfo.ModName,
                 module.ModInfo.ModMajor,
                 data);

            if (serialized.Length > 4096)
                throw new Exception("Data length is greater than 4KB");

            _infos[GetInfo(module)] = data;
            _offsetTable[GetInfo(module)] = offset;

            var fs = new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Write(serialized, 0, serialized.Length);
            fs.Flush();
            fs.Close();
            fs.Dispose();

            //Guard: check if the file's too large (larger than 16MB)
            //If so, clear persistent data
            if (new FileInfo(_fileName).Length > 16 * 1024 * 1024)
                File.Delete(_fileName);
        }
        /// <summary>
        /// Gets the persistent data stored with SetPersistentData()
        /// </summary>
        /// <param name="data"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public static Helpers.ByteArrayReader GetPersistentData(Module module)
        {
            if (_infos.ContainsKey(GetInfo(module)))
                return Helpers.ByteArrayReader.Get(_infos[GetInfo(module)]);
            return Helpers.ByteArrayReader.Get(new byte[0]);
        }

        private static ModInfo GetInfo(Module module)
        {
            if (module == null && !GlobalSettings.Debug)
                return new ModInfo() { ModName = "ERR_MOD_NOT_FOUND" };
            var mi = module.ModInfo;
            mi.ModMinor = 0;
            return mi;
        }
    }
}
