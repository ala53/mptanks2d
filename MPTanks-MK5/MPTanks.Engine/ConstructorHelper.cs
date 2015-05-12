using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Engine
{
    public class ConstructorHelper
    {
        private static bool _ctorCalled = false;
        public static void CallGlobalCtors()
        {
            if (_ctorCalled) return;
            // Further explanation
            // See, we depend on immediate execution of the class constructors because we have a bunch of
            // ...RegisterType(type, name) functions in said constructors that, should they not be run before
            // the (e.g.) module loading code, will hopelessly break the entire game in ways you both can and
            // cannot imagine. Normally, if these aren't run beforehand, they will be auto-jitted
            // at runtime, however we use reflective initialization, so, the jit never realizes it needs to run
            // the static initializers, and thus, the initializers are not run on time.

            // Additionally, the module loader caches the types in a dictionary that it uses later to find the
            // resource types. Thus, resources would not be in the dictionary until the resource type is needed
            // and they wouldn't be needed until they are added to the resource dictionary and called by the
            // resource loader...which won't happen until the static initializer is called...which won't happen
            // until the first use of the class happens...which won't...and so on. Clearly, the static
            // initializers need to be called ahead of time, which this does. In other words, this prevents the
            // c# version of the chicken and egg problem (what comes first - use of the class or dictionary init)

            // This should be called on assembly load as we, post build, add a global constructor to the function
            // whose sole goal is to call this function right here. Unfortunately, this can only be done in the
            // IL post build because the C# code does not allow for global functions (everything has to be in a
            // class or a struct). Luckily, Nuget to the rescue, as someone wrote a tool that auto embeds a
            // function post build that calls this function here. If it breaks, just call this yourself in
            // whatever code you are using, as the method takes less than 1 second and will not crash/break/explode
            // if it is called multiple times.
            foreach (System.Reflection.Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in a.GetTypes())
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
            _ctorCalled = true;
        }
    }
}
