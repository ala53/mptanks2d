using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Modding
{
    public class ModLoader
    {
        public static Module Load(string source, bool verifySafe, out string errors, Assembly[] precompiledAssemblies = null, string[] otherAssemblyReferences = null)
        {
            return Load(new[] { source }, verifySafe, out errors, precompiledAssemblies, otherAssemblyReferences);
        }
        public static Module Load(string[] sources, bool verifySafe, out string errors, Assembly[] precompiledAssemblies, string[] otherAssemblyReferences = null)
        {
            if (otherAssemblyReferences == null) otherAssemblyReferences = new string[0];
            if (precompiledAssemblies == null ) precompiledAssemblies = new Assembly[0];

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

        public static Module Load(Assembly asm, bool verifySafe, out string errors)
        {
            return Load(new[] { asm }, verifySafe, out errors);
        }

        public static Module Load(Assembly[] assemblies, bool verifySafe, out string errors)
        {
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
                var decl = FindModuleDeclaration(asm);
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

            module.Assemblies = assemblies;
            module.Name = moduleDeclaration.Name;
            module.Description = moduleDeclaration.Description;
            module.Author = moduleDeclaration.Author;
            module.Version = moduleDeclaration.Version;

            //Tanks
            var tanks = new List<TankType>();
            foreach (var asm in assemblies)
                foreach (var tank in ScanTankTypes(asm))
                {
                    try
                    {
                        tanks.Add(new TankType(tank));
                    }
                    catch (Exception e)
                    {
                        builderErrors += "\n\n\nTank: " + tank.FullName + " has error: " + e.Message;
                    }
                }
            module.Tanks = tanks.ToArray();

            //Projectiles
            var projectiles = new List<ProjectileType>();
            foreach (var asm in assemblies)
                foreach (var prj in ScanProjectileTypes(asm))
                {
                    try
                    {
                        projectiles.Add(new ProjectileType(prj, module.Tanks));
                    }
                    catch (Exception e)
                    {
                        builderErrors += "\n\n\nProjectile: " + prj.FullName + " has error: " + e.Message;
                    }
                }
            module.Projectiles = projectiles.ToArray();

            //Map objects
            var mapObjects = new List<MapObjectType>();
            foreach (var asm in assemblies)
                foreach (var mapObject in ScanMapObjectTypes(asm))
                {
                    try
                    {
                        mapObjects.Add(new MapObjectType(mapObject));
                    }
                    catch (Exception e)
                    {
                        builderErrors += "\n\n\nMap object: " + mapObject.FullName + " has error: " + e.Message;
                    }
                }
            module.MapObjects = mapObjects.ToArray();

            //Gamemodes
            var gamemodes = new List<GamemodeType>();
            foreach (var asm in assemblies)
                foreach (var gamemode in ScanGamemodeTypes(asm))
                {
                    try
                    {
                        gamemodes.Add(new GamemodeType(gamemode));
                    }
                    catch (Exception e)
                    {
                        builderErrors += "\n\n\nGamemode: " + gamemode.FullName + " has error: " + e.Message;
                    }
                }
            module.Gamemodes = gamemodes.ToArray();

            //And call the constructors
            foreach (var asm in assemblies)
                CallStaticCtors(asm);

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
    }
}
