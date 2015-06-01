using MPTanks.ModCompiler.Packer;
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
        public static string author;
        public static string name;
        public static string version;
        public static string description;

        static void Main(string[] args)
        {
            var options = new Mono.Options.OptionSet()
                .Add("o|out|output=", "The output *.mod file for the compiled mod.", (p) => outputFile = p)
                .Add<bool>("whitelist:", "Do whitelist verification on the DLLs (mandatory for store upload)",
                    (w) => whitelistDlls = w)
                .Add("c|code:",
                    "The executable code (*.dll) or C# source files to pack into the mod.", (p) =>
                    {
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
                .Add("i|image:", "The image assets to compile. Expects a *.json file to reside with the image",
                (p) =>
                {
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
                        imageAssets.Add(p);
                    else
                    {
                        Console.WriteLine($"{p} is not a valid image.");
                        Exit(-2);
                    }
                })
                .Add("s|sound:", "The sound assets to pack into the mod.", (p) =>
                {
                    if (!File.Exists(p))
                    {
                        Console.WriteLine($"{p} does not exist.");
                        Exit(-2);
                    }

                    var fi = new FileInfo(p);
                    if (fi.Extension == ".wav" || fi.Extension == ".mp3" || fi.Extension == ".ogg")
                        soundAssets.Add(p);
                    else
                    {
                        Console.WriteLine($"{p} is not a valid sound file");
                    }
                })
                .Add("author=", "The author for the mod", (p) => author = p)
                .Add("name=", "The display name of the mod", (p) => name = p)
                .Add("description=", "The description of the mod", (p) => description = p)
                .Add("version=", "The mod's version in the form of MAJOR.MINOR TAG. E.g. 1.2 DEV", (p) => version = p)
                .Add("dependency:",
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
                Console.WriteLine("You have disabled DLL code whitelisting." +
                    "You will not be able to put this mod on the ZSB mods page.");
            }
            if (dlls.Count == 0)
            {
                Console.WriteLine("Warning! This mod has no code." +
                    "It will only be usable in a supporting role for another mod (e.g. asset files).");
            }
            if (imageAssets.Count == 0)
            {
                Console.WriteLine("Warning! This mod has no images. Make sure references to asset mods are correct!");
            }
            if (soundAssets.Count == 0)
            {
                Console.WriteLine("Warning! This mod has no sounds. Make sure references to asset mods are correct!");
            }
            if (imageAssets.Count == 0 && soundAssets.Count == 0 && dlls.Count == 0)
            {
                Console.WriteLine("Error! This mod is empty! No sound, code, or images are included.");
                Exit(-2);
            }

            CheckCodeSafe();

            foreach (var dep in dependencies)
                if (!DependencyResolver.TryResolve(dep))
                {
                    Console.WriteLine($"Warning! Could not resolve {dep} via the ZSB Mod Workshop." +
                        " The version specified may not exist.");
                }

            if (name.Contains(":"))
            {
                Console.WriteLine($"Error! The mod name {name} contains a : (colon), which is a disallowed character.");
                Exit(-2);
            }

            if (DependencyResolver.ModExists(name))
            {
                Console.WriteLine($"Warning! The mod {name} already exists in the ZSB Mod Workshop. If you " +
                    "are not the owner, you will not be able to upload it.");
            }

            if (author.Length < 4)
            {
                Console.WriteLine("Error! The author name is too short. It must be 4 characters or more.");
                Exit(-2);
            }

            if (description.Length < 30)
            {
                Console.WriteLine("Warning! There is either a very short description or no description. Make sure the " +
                    "description or name fully describes the mod.");
            }

            if (!DependencyResolver.IsValidVersion(version))
            {
                Console.WriteLine($"Error! The version tag {version} is invalid. Make sure it's in the format of " +
                    "MAJOR.MINOR [TAG]. The tag is optional, but the major and minor aren't.");
                Exit(-2);
            }
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
                Console.WriteLine(ex.ToString());
                return false;
            }

            if (!whitelistDlls)
                return true;

            string err;
            var safe = Modding.Compiliation.Verification.WhitelistVerify.IsAssemblySafe(file, out err);
            if (!safe)
                Console.WriteLine(err);
            return safe;
        }

        static void CheckCodeSafe()
        {
            string errors;
            var asm = Modding.Compiliation.Compiler.CompileAssembly(srcFiles.ToArray(), out errors, dlls.ToArray());

            if (asm == null)
            {
                Console.WriteLine(errors);
                Console.WriteLine("Source files could not be compiled.");
                Exit(-2);
            }
           if (! CheckDLLSafe(asm))
            {
                Console.WriteLine("Source files are unsafe.");
                Exit(-2);
            }
        }

        static void Exit(int code)
        {
            Environment.Exit(code);
        }

        static void WriteCredits()
        {
            Console.WriteLine("Using oggenc2 by Michael Smith & John Edwards");
        }
    }
}
