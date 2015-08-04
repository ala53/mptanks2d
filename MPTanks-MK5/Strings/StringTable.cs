using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.StringData
{
    public class StringTable : DynamicObject, IReadOnlyDictionary<string, string>
    {
        public const string unrecognizedStringDisplayValue = "E_STRING_NOT_FOUND: {0}";
        private string _filename;
        private Dictionary<string, string> _loadedStrings;
        private List<KeyValuePair<string, string>> _orderedLoadedStrings;
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
            _loadedStrings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); //optimize searches to avoid .ToLower() allocations
            _orderedLoadedStrings = new List<KeyValuePair<string, string>>();
            //It's a flat file of type: key<space>value pairs
            for (var i = 0; i < lines.Length; i++)
            {
                try
                {
                    if (lines[i].Split(' ').Length < 2 || lines[i].Trim().StartsWith("###")) continue;
                    if (lines[i].Trim().StartsWith("\""))
                    {//The name is in quotes 
                        var lastIndex = lines[i].Trim().IndexOf("\"", 1);
                        var name = lines[i].Trim().Substring(1, lastIndex - 2);
                        var value = lines[i].Trim().Substring(lastIndex + 1);
                        _loadedStrings.Add(name, value);
                        _orderedLoadedStrings.Add(new KeyValuePair<string, string>(name, value));
                    }
                    else
                    {
                        var name = lines[i].TrimStart().Split(' ')[0];
                        var value = String.Join(" ", lines[i].Split(' ').Skip(1)).Replace(@"\n", "\n").TrimEnd();
                        _loadedStrings.Add(name, value);
                        _orderedLoadedStrings.Add(new KeyValuePair<string, string>(name, value));
                    }
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

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = String.Format(binder.Name, args);

            return true;
        }

        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            result = String.Format(args[0].ToString(), args.Skip(1).ToArray());

            return true;
        }


        public bool ContainsKey(string key)
        {
            Load();
            return _loadedStrings.ContainsKey(key);
        }

        public Dictionary<string, string>.KeyCollection Keys
        {
            get
            {
                Load();
                return _loadedStrings.Keys;
            }
        }
        IEnumerable<string> IReadOnlyDictionary<string, string>.Keys
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

        public Dictionary<string, string>.ValueCollection Values
        {
            get
            {
                Load();
                return _loadedStrings.Values;
            }
        }

        IEnumerable<string> IReadOnlyDictionary<string, string>.Values
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

        public StringTableEnumerator GetEnumerator()
        {
            Load();
            return new StringTableEnumerator(_orderedLoadedStrings);
        }
        IEnumerator<KeyValuePair<string, string>> IEnumerable<KeyValuePair<string, string>>.GetEnumerator()
        {
            Load();
            return new StringTableEnumerator(_orderedLoadedStrings);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Load();
            return new StringTableEnumerator(_orderedLoadedStrings);
        }

        public struct StringTableEnumerator : IEnumerator<KeyValuePair<string, string>>
        {
            private List<KeyValuePair<string, string>> _list;
            private int _index;
            internal StringTableEnumerator(List<KeyValuePair<string, string>> list)
            {
                _list = list;
                _index = 0;
            }
            public KeyValuePair<string, string> Current => _list[_index];

            object IEnumerator.Current => _list[_index];

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                _index++;
                return _index < _list.Count;
            }

            public void Reset()
            {
                _index = 0;
            }
        }
    }
}
