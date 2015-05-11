using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.StringData
{
    public class StringTable : DynamicObject, IReadOnlyDictionary<string, string>
    {
        const string unrecognizedStringDisplayValue = "E_STRING_NOT_FOUND: {0}";
        private string _filename;
        private Dictionary<string, string> _loadedStrings;
        internal StringTable(string filename)
        {
            _filename = filename;
        }
        public string GetByName(string name)
        {
            Load();
            if (_loadedStrings.ContainsKey(name)) return _loadedStrings[name];
            //not found
            return String.Format(unrecognizedStringDisplayValue, name);
        }

        private void Load()
        {
            if (_loadedStrings != null) return;

            var lines = System.IO.File.ReadAllLines(GetLocalizedFile(_filename));
            _loadedStrings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); //optimize searches to avoid .ToLower allocation

            //It's a flat file of type: key<space>value pairs
            for (var i = 0; i < lines.Length; i++)
            {
                try
                {
                    if (lines[i].Split(' ').Length < 2 || lines[i].Trim().StartsWith("###")) continue;
                    var name = lines[i].TrimStart().Split(' ')[0];
                    var value = String.Join(" ", lines[i].Split(' ').Skip(1)).Replace(@"\n", "\n").TrimEnd();
                    _loadedStrings.Add(name, value);
                }
                catch (Exception)
                {
                }
            }
        }

        private string GetLocalizedFile(string filename)
        {
            if (System.IO.File.Exists(
                String.Format(filename, System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName)))
                return String.Format(filename, System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            else
                return String.Format(filename, "en");
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetByName(binder.Name);

            return true;
        }


        public bool ContainsKey(string key)
        {
            Load();
            return _loadedStrings.ContainsKey(key);
        }

        public IEnumerable<string> Keys
        {
            get
            {
                Load();
                return _loadedStrings.Keys;
            }
        }

        public bool TryGetValue(string key, out string value)
        {
            Load();
            return _loadedStrings.TryGetValue(key, out value);
        }

        public IEnumerable<string> Values
        {
            get
            {
                Load();
                return _loadedStrings.Values;
            }
        }

        public string this[string key]
        {
            get
            {
                Load();
                return _loadedStrings[key];
            }
        }

        public int Count
        {
            get
            {
                Load();
                return _loadedStrings.Count;
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            Load();
            return _loadedStrings.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            Load();
            return _loadedStrings.GetEnumerator();
        }
    }
}
