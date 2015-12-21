using MPTanks.Engine.Maps;
using MPTanks.ModCompiler.Packer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.ModCompiler
{
    class Program
    {
        public static string outputFile;
        public static bool whitelistDlls = true;
        public static List<string> dlls = new List<string>();
        public static List<string> srcFiles = new List<string>();
        public static List<string> imageAssets = new List<string>();
        public static List<string> soundAssets = new List<string>();
        public static List<string> dependencies = new List<string>();
        public static List<string> maps = new List<string>();
        public static List<string> components = new List<string>();
        public static string author;
        public static string name;
        public static string version;
        public static string description;

        const int MAX_SIZE_MB = 10;

        static void Main(string[] args)
        {
            Mono.Options.OptionSet options = null;
            options = new Mono.Options.OptionSet()
                .Add("o=|out=|output=", "The output *.mod file for the compiled mod.", (p) => outputFile = p)
                .Add("nowhitelist|nowl", "Disable whitelist verification on the DLLs (mandatory for store upload)",
                    (w) => { whitelistDlls = false; })
                .Add("?|help|h", (a) =>
                {
                    options.WriteOptionDescriptions(Console.Out);
                    Exit(0);
                })
                .Add("c:|code:",
                    "The executable code (*.dll) or C# source files to pack into the mod.", (p) =>
                    {
                        p = Path.GetFullPath(p);
                        if (!File.Exists(p))
                        {
                            Console.WriteLine($"{p} does not exist.");
                            Exit(-2);
                        }

                        var fi = new FileInfo(p);
                        if (fi.Extension == ".cs")
                        {
                            srcFiles.Add(p);
                        }
                        else if (fi.Extension == ".dll")
                        {
                            if (whitelistDlls)
                                if (!CheckDLLSafe(p))
                                {
                                    Console.WriteLine($"{p} is either an unrecognized mod or does not follow the whitelist.");
                                    Exit(-2);
                                }
                            dlls.Add(p);
                        }
                        else
                        {
                            Console.WriteLine($"{p} is not a valid dll");
                            Exit(-2);
                        }
                    })
                .Add("indir:|inputdirectory:", "Looks for all known files in a directory, validates them, and then includes them.", p =>
                {
                    p = Path.GetFullPath(p);
                    ProcessInputFileList(Directory.GetFiles(p));
                })
                .Add("rindir:|recursiveindir:", "Looks for all known files in a directory and it's subdirectories, validates them, then includes them.", p =>
                {
                    p = Path.GetFullPath(p);
                    ProcessInputFileList(Directory.GetFiles(p, "*.*", SearchOption.AllDirectories));
                })
                .Add("cmp:|componentfile:", "Component JSON files to describe in what way a GameObject should be constructed.",
                (p) =>
                {
                    p = Path.GetFullPath(p);
                    if (!File.Exists(p))
                    {
                        Console.WriteLine($"{p} does not exist.");
                        Exit(-2);
                    }
                    //And continue
                    components.Add(p);
                })
                .Add("i:|image:", "The image assets to compile. Expects a *.json file to reside with the image",
                (p) =>
                {
                    p = Path.GetFullPath(p);
                    if (!File.Exists(p))
                    {
                        Console.WriteLine($"{p} does not exist.");
                        Exit(-2);
                    }
                    if (!File.Exists(p + ".json"))
                    {
                        Console.WriteLine($"{p} is missing *.json sprite sheet descriptor.");
                        Exit(-2);
                    }
                    var fi = new FileInfo(p);
                    if (fi.Extension == ".png" || fi.Extension == ".bmp" || fi.Extension == ".jpg" ||
                        fi.Extension == ".gif")
                    {
                        imageAssets.Add(p);
                    }
                    else
                    {
                        Console.WriteLine($"{p} is not a valid image.");
                        Exit(-2);
                    }
                })
                .Add("s:|sound:", "The sound assets to pack into the mod.", (p) =>
                {
                    p = Path.GetFullPath(p);
                    if (!File.Exists(p))
                    {
                        Console.WriteLine($"{p} does not exist.");
                        Exit(-2);
                    }

                    var fi = new FileInfo(p);
                    if (fi.Extension == ".wav" || fi.Extension == ".mp3" || fi.Extension == ".ogg" || fi.Extension == ".flac")
                        soundAssets.Add(p);
                    else
                    {
                        Console.WriteLine($"{p} is not a valid sound file");
                        Exit(-2);
                    }
                })
                .Add("map:|m:", "A map file to embed in the mod.", (m) =>
                {
                    m = Path.GetFullPath(m);
                    if (!File.Exists(m))
                    {
                        Console.WriteLine($"{m} does not exist.");
                        Exit(-2);
                    }

                    try
                    {
                        Map.Load(File.ReadAllText(m), null);
                        maps.Add(m);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine($"{m} is not a valid map file");
                        Exit(-2);
                    }

                })
                .Add("author=", "The author for the mod", (p) => author = p)
                .Add("name=", "The display name of the mod", (p) => name = p)
                .Add("description=", "The description of the mod", (p) => description = p)
                .Add("version=", "The mod's version in the form of MAJOR.MINOR TAG. E.g. 1.2 DEV", (p) => version = p)
                .Add("dep:|dependency:",
                    "A mod that this mod depends on. In the form of Name:MinVersion. E.g. ZSB Core Assets:1.0.",
                    (p) =>
                    {
                        dependencies.Add(p);
                    });

            options.Parse(args);

            if (outputFile == null)
            {
                Console.WriteLine("Missing output file");
                options.WriteOptionDescriptions(Console.Out);
                Exit(-1);
            }
            if (whitelistDlls == false)
            {
                Console.WriteLine();
                Console.WriteLine("You have disabled DLL code whitelisting. " +
                    "You will not be able to put this mod on the ZSB mods page.");
            }
            if (dlls.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Warning! This mod has no code." +
                    "It will only be usable in a supporting role for another mod (e.g. asset files).");
            }
            if (imageAssets.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Warning! This mod has no images. Make sure dependencies on asset mods are correct!");
            }
            if (soundAssets.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Warning! This mod has no sounds. Make sure dependencies on asset mods are correct!");
            }
            if (components.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Warning! This mod has no component files. It is a bad idea to generate components from code alone!");
            }
            if (imageAssets.Count == 0 && soundAssets.Count == 0 && dlls.Count == 0 && maps.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Error! This mod is empty! No sound, code, images, or maps are included.");
                Exit(-2);
            }

            if (!DependencyResolver.IsNameValid(name))
            {
                Console.WriteLine();
                Console.WriteLine($"Error! {name} is not a valid name. " +
                    "It must be between 4 and 64 characters, containing only A-Z 0-9 and - (dash).");
                Exit(-2);
            }

            if (whitelistDlls) CheckCodeSafe();

            foreach (var dep in dependencies)
            {
                Console.WriteLine();
                Console.WriteLine($"Verifying dependency: {dep}");
                Console.WriteLine("======================================================");
                if (!DependencyResolver.ModExists(dep))
                {
                    Console.WriteLine($"Warning! Could not resolve {dep} via the ZSB Mod Workshop." +
                        " The mod may not exist.");
                }
                Console.WriteLine("Dependency verified.");
            }

            if (name.Contains(":"))
            {
                Console.WriteLine();
                Console.WriteLine($"Error! The mod name {name} contains a : (colon), which is a disallowed character.");
                Exit(-2);
            }

            Console.WriteLine();
            Console.WriteLine("Checking if mod with same name exists in workshop.");
            if (DependencyResolver.ModExists(name))
            {
                Console.WriteLine();
                Console.WriteLine($"Warning! The mod {name} already exists in the ZSB Mod Workshop. If you " +
                    "are not the owner, you will not be able to upload it.");
            }

            if (author.Length < 4)
            {
                Console.WriteLine();
                Console.WriteLine("Error! The author name is too short. It must be 4 characters or more.");
                Exit(-2);
            }

            if (description.Length < 30)
            {
                Console.WriteLine();
                Console.WriteLine("Warning! There is either a very short description or no description. Make sure the " +
                    "description or name fully describes the mod.");
            }

            if (!DependencyResolver.IsValidVersion(version))
            {
                Console.WriteLine();
                Console.WriteLine($"Error! The version tag {version} is invalid. Make sure it's in the format of " +
                    "MAJOR.MINOR [TAG]. The tag is optional, but the major and minor version numbers are required.");
                Exit(-2);
            }

            //And then actually compile the mod files
            var packed = Packer.Packer.Pack();

            if (packed.Length > (1024 * 1024 * MAX_SIZE_MB)) // 10mb usually
            {
                Console.WriteLine($"Warning! Mod is {(packed.Length / (1024d * 1024)).ToString("N1")}mb (" +
                    $"max size is {MAX_SIZE_MB}mb). You cannot upload it to the ZSB database.");
            }

            File.WriteAllBytes(outputFile, packed);
        }

        static bool CheckDLLSafe(string file)
        {
            Assembly asm;
            try
            {
                asm = Assembly.LoadFile(file);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not load assembly: " + file);
                Console.WriteLine();
                Console.WriteLine(ex.ToString());
                return false;
            }

            if (!whitelistDlls)
                return true;

            string err;
            var safe = Modding.Compiliation.Verification.WhitelistVerify.IsAssemblySafe(file, out err);
            if (!safe)
            {
                Console.WriteLine($"Assembly verification for {file} failed. Errors printed below.");
                Console.WriteLine("===============================================================");
                Console.WriteLine(err);
            }
            return safe;
        }

        static void CheckCodeSafe()
        {
            string errors;
            var asm = Modding.Compiliation.Compiler.CompileAssembly(srcFiles.ToArray(), out errors, dlls.ToArray());

            if (asm == null)
            {
                Console.WriteLine(errors);
                Console.WriteLine();
                Console.WriteLine("=======================================");
                Console.WriteLine();
                Console.WriteLine("Source files could not be compiled.");
                Console.WriteLine();
                Exit(-2);
            }
            if (!CheckDLLSafe(asm))
            {
                Console.WriteLine("Source files (post compiliation) are unsafe.");
                Console.WriteLine();
                Exit(-2);
            }
        }

        static void ProcessInputFileList(IEnumerable<string> files)
        {
            var mappings = new Dictionary<string, string>();
            foreach (var file in files)
            {
                var info = new FileInfo(file);
                var ext = info.Extension.ToLower();

                if (file.ToLower().EndsWith(".png.json") ||
                    file.ToLower().EndsWith(".gif.json") ||
                    file.ToLower().EndsWith(".jpg.json") ||
                    file.ToLower().EndsWith(".bmp.json"))
                    continue; //Unnecessary: it's included when archiving the png file

                if (ext == ".png" || ext == ".gif" || ext == ".jpg" || ext == ".bmp")
                {
                    if (File.Exists(file + ".json"))
                    {
                        mappings.Add(file, "image");
                        imageAssets.Add(file);
                    }
                    else
                    {
                        Console.WriteLine($"Warning! {file} is missing matching .json descriptor. Ignoring for now.");
                    }
                }
                else if (ext == ".ssjson")
                {
                    mappings.Add(file, "image");
                    imageAssets.Add(ext);
                }
                else if (ext == ".wav" || ext == ".mp3" || ext == ".ogg" || ext == ".flac")
                {
                    mappings.Add(file, "sound");
                    soundAssets.Add(file);
                }
                else if (ext == ".cs")
                {
                    mappings.Add(file, "c# source");
                    srcFiles.Add(file);
                }
                else if (ext == ".dll")
                {
                    mappings.Add(file, "mod dll");
                    dlls.Add(file);
                    if (whitelistDlls)
                        if (!CheckDLLSafe(file))
                        {
                            Console.WriteLine($"Error! {file} is either an unrecognized mod or does not follow the whitelist.");
                            Exit(-2);
                        }
                }
                else if (ext == ".json")
                {
                    //map OR component file
                    dynamic deserializedGeneric = JsonConvert.DeserializeObject(
                        File.ReadAllText(file));

                    if (deserializedGeneric.type == "map" || deserializedGeneric.Type == "map")
                    {
                        //map file
                        try
                        {
                            Console.WriteLine($"Assuming {info.Name} is a map file (reflectionName property not found), not a components file.");
                            Map.Load(File.ReadAllText(file), null);
                            maps.Add(file);
                            mappings.Add(file, "map");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading map file: {file}");
                            Console.WriteLine(ex.ToString());
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Assuming {info.Name} is a components file (no type: \"map\" property), not a map file.");
                        mappings.Add(file, "components");
                        //component file
                        components.Add(file);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Files identified");
            foreach (var file in mappings)
            {
                var fi = new FileInfo(file.Key);
                Console.WriteLine($"\t{fi.Name} -> {file.Value}");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        static void Exit(int code)
        {
            Environment.Exit(code);
        }
    }
}
