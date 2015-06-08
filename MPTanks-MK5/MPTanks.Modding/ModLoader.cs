using MPTanks.Modding.Unpacker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding
{
    public static class ModLoader
    {
        private static Dictionary<string, Module> loadedModFiles = new Dictionary<string, Module>();
        public static IReadOnlyDictionary<string, Module> LoadedMods { get { return loadedModFiles; } }

        public static Module LoadMod(string modFile, string dllUnpackDir, string assetUnpackDir, out string errors, bool verifySafe = true)
        {
            errors = "";
            if (loadedModFiles.ContainsKey(modFile))
                return loadedModFiles[modFile];

            Module output = null;
            string err = "";
#if !DISABLE_ERROR_HANDLING_FOR_MODLOADER
            try
            {
#endif
                //Get the header to resolve dependencies
                var header = ModUnpacker.GetHeader(modFile);
                var deps = new List<string>();
                //Resolve the dependencies and get all of their dlls
                foreach (var dep in header.Dependencies)
                    deps.AddRange(DependencyResolver.LoadDependency(dep.ModName, dep.Major, dep.Minor,
                        dllUnpackDir, assetUnpackDir, header.Name));
                //Remove duplicates
                deps = deps.Distinct().ToList();
                //Then, unpack the assemblies to the correct directory
                var dllPaths = ModUnpacker.UnpackDlls(modFile, dllUnpackDir);

                //If it has source code, compile that
                if (header.CodeFiles.Length > 0)
                {
                    string mErr = "";
                    Load(
                        ModUnpacker.GetSourceCode(modFile),
                        verifySafe,
                        out mErr,
                        dllPaths,
                        deps.ToArray()
                        );

                    err += mErr;
                }
                else
                {
                    //Otherwise, just do a simple load
                    string mErr = "";
                    Load(
                        dllPaths, verifySafe, out mErr);
                    err += mErr;
                }
                //And finally, unpack assets
                ModUnpacker.UnpackImages(modFile, assetUnpackDir);
                ModUnpacker.UnpackSounds(modFile, assetUnpackDir);
#if !DISABLE_ERROR_HANDLING_FOR_MODLOADER
        }
            catch (Exception ex)
            {
                err += ex.ToString() + "\n\n";
            }
#endif
            errors = err;
            loadedModFiles.Add(modFile, output);
            return output;
        }

        public static Module Load(string source, bool verifySafe, out string errors, string[] precompiledAssemblies = null, string[] otherAssemblyReferences = null)
        {
            return Load(new[] { source }, verifySafe, out errors, precompiledAssemblies, otherAssemblyReferences);
        }
        public static Module Load(string[] sources, bool verifySafe, out string errors, string[] precompiledAssemblies, string[] otherAssemblyReferences = null)
        {
            if (otherAssemblyReferences == null) otherAssemblyReferences = new string[0];
            if (precompiledAssemblies == null) precompiledAssemblies = new string[0];

            var compileErrors = "";
            var asm = Compiliation.Compiler.CompileAssembly(sources, out compileErrors, otherAssemblyReferences);
            var mbuilderrors = "";

            if (asm == null)
            {

                errors = "Compiliation failed \n\n\n" + compileErrors;
                return null;
            }

            var assemblies = precompiledAssemblies.ToList();
            assemblies.Add(asm);

            var module = Load(assemblies.ToArray(), verifySafe, out mbuilderrors);

            errors = compileErrors + "\n\n\n" + mbuilderrors;
            return module;

        }

        public static Module Load(string asm, bool verifySafe, out string errors)
        {
            return Load(new[] { asm }, verifySafe, out errors);
        }

        public static Module Load(string[] assemblies, bool verifySafe, out string errors)
        {
            assemblies = assemblies.Select((a) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, a)).ToArray();
            var safetyCheckErrors = "";
            if (verifySafe)
            {//Scan the IL of the assembly
                foreach (var asm in assemblies)
                {
                    if (!Compiliation.Verification.WhitelistVerify.IsAssemblySafe(asm, out safetyCheckErrors))
                    {
                        errors = safetyCheckErrors;
                        return null;
                    }
                }
            }

            var builderErrors = "";
            var module = new Module();


            //Get the declaration
            ModuleDeclarationAttribute moduleDeclaration = null;
            foreach (var asm in assemblies)
            {
                var decl = FindModuleDeclaration(Assembly.LoadFile(asm));
                if (decl != null)
                {
                    moduleDeclaration = decl;
                    break;
                }
            }
            if (moduleDeclaration == null)
            {
                builderErrors = "Missing module declaration. ([ModuleDeclarationAttribute]). Cannot proceed.";
                errors = safetyCheckErrors + "\n\n\n" + builderErrors;
                return null;
            }

            module.Assemblies = assemblies.Select(a => Assembly.LoadFile(a)).ToArray();
            //Resolve dependencies
            module.Dependencies =
                module.Assemblies.SelectMany(
                    a => a.GetReferencedAssemblies().Select(
                        an => Assembly.Load(an))).ToArray();
            //And get the name
            module.Name = moduleDeclaration.Name;
            module.Description = moduleDeclaration.Description;
            module.Author = moduleDeclaration.Author;
            module.Version = moduleDeclaration.Version;

            //Tanks
            var tanks = new List<TankType>();
            foreach (var asm in module.Assemblies)
                foreach (var tank in ScanTankTypes(asm))
                {
#if !DISABLE_ERROR_HANDLING_FOR_MODLOADER
                    try
                    {
#endif
                    var typ = new TankType(tank);
                    var attrib = (GameObjectAttribute)typ.Type.GetCustomAttribute(typeof(GameObjectAttribute), true);
                    attrib.Owner = module;
                    Inject(typ);
                    tanks.Add(typ);
#if !DISABLE_ERROR_HANDLING_FOR_MODLOADER
                }
                    catch (Exception e)
                    {
                        builderErrors += "\n\n\nTank: " + tank.FullName + " has error: " + e.Message;
                    }
#endif
                }
            module.Tanks = tanks.ToArray();

            //Projectiles
            var projectiles = new List<ProjectileType>();
            foreach (var asm in module.Assemblies)
                foreach (var prj in ScanProjectileTypes(asm))
                {
#if !DISABLE_ERROR_HANDLING_FOR_MODLOADER
                    try
                    {
#endif
                    var typ = new ProjectileType(prj, module.Tanks);
                    var attrib = (GameObjectAttribute)typ.Type.GetCustomAttribute(typeof(GameObjectAttribute), true);
                    attrib.Owner = module;
                    Inject(typ);
                    projectiles.Add(typ);
#if !DISABLE_ERROR_HANDLING_FOR_MODLOADER
                    }
                    catch (Exception e)
                    {
                        builderErrors += "\n\n\nProjectile: " + prj.FullName + " has error: " + e.Message;
                    }
#endif
                }
            module.Projectiles = projectiles.ToArray();

            //Map objects
            var mapObjects = new List<MapObjectType>();
            foreach (var asm in module.Assemblies)
                foreach (var mapObject in ScanMapObjectTypes(asm))
                {
#if !DISABLE_ERROR_HANDLING_FOR_MODLOADER
                    try
                    {
#endif
                    var typ = new MapObjectType(mapObject);
                    var attrib = (GameObjectAttribute)typ.Type.GetCustomAttribute(typeof(GameObjectAttribute), true);
                    attrib.Owner = module;
                    Inject(typ);
                    mapObjects.Add(typ);
#if !DISABLE_ERROR_HANDLING_FOR_MODLOADER
                    }
                    catch (Exception e)
                    {
                        builderErrors += "\n\n\nMap object: " + mapObject.FullName + " has error: " + e.Message;
                    }
#endif
                }
            module.MapObjects = mapObjects.ToArray();

            //Gamemodes
            var gamemodes = new List<GamemodeType>();
            foreach (var asm in module.Assemblies)
                foreach (var gamemode in ScanGamemodeTypes(asm))
                {
#if !DISABLE_ERROR_HANDLING_FOR_MODLOADER
                    try
                    {
#endif
                    var typ = new GamemodeType(gamemode);
                    var attrib = (GameObjectAttribute)typ.Type.GetCustomAttribute(typeof(GameObjectAttribute), true);
                    attrib.Owner = module;
                    Inject(typ);
                    gamemodes.Add(typ);
#if !DISABLE_ERROR_HANDLING_FOR_MODLOADER
                    }
                    catch (Exception e)
                    {
                        builderErrors += "\n\n\nGamemode: " + gamemode.FullName + " has error: " + e.Message;
                    }
#endif
                }
            module.Gamemodes = gamemodes.ToArray();

            //And call the constructors
            foreach (var asm in module.Assemblies)
                CallStaticCtors(asm);

            //Mark the mod as loaded
            ModDatabase.AddLoaded(module);

            //And finally, inject the code

            errors = safetyCheckErrors + "\n\n" + builderErrors;

            return module;
        }

        public static void CallStaticCtors(Assembly asm)
        {
            foreach (Type t in asm.GetTypes())
            {
                try
                {
                    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(t.TypeHandle);
                }
                catch (TypeInitializationException)
                {
                }
            }

            GC.Collect(2, GCCollectionMode.Forced, true);
        }


        private static ModuleDeclarationAttribute FindModuleDeclaration(Assembly asm)
        {
            foreach (var type in asm.GetTypes())
                if (type.GetCustomAttribute<ModuleDeclarationAttribute>() != null)
                    return type.GetCustomAttribute<ModuleDeclarationAttribute>();

            return null;
        }

        private static Type[] ScanProjectileTypes(Assembly asm)
        {
            var projectileTypes = new List<Type>();
            foreach (var type in asm.GetTypes())
                if (ProjectileType.IsProjectileType(type))
                    projectileTypes.Add(type);

            return projectileTypes.ToArray();
        }
        private static Type[] ScanGamemodeTypes(Assembly asm)
        {
            var gamemodeTypes = new List<Type>();
            foreach (var type in asm.GetTypes())
                if (GamemodeType.IsGamemodeType(type))
                    gamemodeTypes.Add(type);

            return gamemodeTypes.ToArray();
        }
        private static Type[] ScanMapObjectTypes(Assembly asm)
        {
            var mapObjectTypes = new List<Type>();
            foreach (var type in asm.GetTypes())
                if (MapObjectType.IsMapObjectType(type))
                    mapObjectTypes.Add(type);

            return mapObjectTypes.ToArray();
        }
        private static Type[] ScanTankTypes(Assembly asm)
        {
            var tankTypes = new List<Type>();
            foreach (var type in asm.GetTypes())
                if (TankType.IsTankType(type))
                    tankTypes.Add(type);

            return tankTypes.ToArray();
        }
        private static void Inject(TankType type)
        {
            var typ = GetTypeHelper.GetType(Settings.TankTypeName);
            var method = typ.GetMethod("RegisterType", BindingFlags.Static | BindingFlags.NonPublic);
            method.MakeGenericMethod(type.Type).Invoke(null, null);
        }
        private static void Inject(ProjectileType type)
        {
            var typ = GetTypeHelper.GetType(Settings.ProjectileTypeName);
            var method = typ.GetMethod("RegisterType", BindingFlags.Static | BindingFlags.NonPublic);
            method.MakeGenericMethod(type.Type).Invoke(null, null);
        }
        private static void Inject(MapObjectType type)
        {
            var typ = GetTypeHelper.GetType(Settings.MapObjectTypeName);
            var method = typ.GetMethod("RegisterType", BindingFlags.Static | BindingFlags.NonPublic);
            method.MakeGenericMethod(type.Type).Invoke(null, null);
        }
        private static void Inject(GamemodeType type)
        {
            var typ = GetTypeHelper.GetType(Settings.GamemodeTypeName);
            var method = typ.GetMethod("RegisterType", BindingFlags.Static | BindingFlags.NonPublic);
            method.MakeGenericMethod(type.Type).Invoke(null, null);
        }
    }
}
