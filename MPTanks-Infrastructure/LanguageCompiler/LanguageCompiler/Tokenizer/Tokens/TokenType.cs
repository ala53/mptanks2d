using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageCompiler.Tokenizer.Tokens
{
    enum TokenType
    {
        /// <summary>
        /// namespace [namespace]
        /// </summary>
        NamespaceDeclaration,
        NamespaceName,

        //Usings
        //import namespace
        ImportDeclaration,

        /// <summary>
        /// class [class]<Type1:TypeSpecifier1:TypeSpecifier2,Type2>
        /// class abc<T:new(string):struct, U:Adef:class> <-class abc, accepts T and U as generic args.
        /// T must be a struct with a new function accepting a string value
        /// U must derive from the abstract description Adef and be a class
        /// </summary>
        ClassDeclaration,
        StructDeclaration,
        TypeName,
        /// <summary>
        /// class a, b
        /// b is the alias
        /// </summary>
        TypeAlias,

        //Generics
        GenericArgument,
        GenericTypeSpecifier,
        GenericNewSpecifier,
        GenericNewTypeSpecifier,
        GenericStructSpecifier,
        GenericClassSpecifier,

        //Conversions
        ImplicitConversionDeclaration,
        ExplicitConversionDeclaration,

        //Operators
        OperatorDeclaration,
        ArrayOperator,
        SubtractOperator,
        AddOperator,
        MultiplyOperator,
        DivideOperator,

        //Functions
        FunctionDeclaration,

        //Getters and Setters
        GetDeclaration,

        SetDeclaration,

        //Variables: List<V> v = new List<V>()
        VariableDeclaration,
        VariableName,
        VariableAssignment,

        //New
        New,
        NewCall,

        //Arrays
        NewArray, //either new Array<T> or new T[] - they're the same
        ArraySize,
        
        //literals
        StringLiteral, 
        ByteLiteral, //100B is byte
        NumLiteral, //1000000 is number literal
        PtrLiteral, //100000P is pointer literal
        TrueLiteral, //true is converted to EE.Boolean::true
        FalseLiteral, //false is converted to EE.Boolean::false
        
        //Micro methods
        RefCall, //ref value or ref(value)
        DerefCall, //deref<T> value or deref<T>(value)
        FailCall, //fail <object deriving from EE.Error or string> or fail(object deriving from EE.Error or string)



        //Implicit variable declarations: var v = new List<V>()
        ImplicitVariableDeclaration,

        //Functions - ReturnType functionName(Type argument1, Type[] paramsType)
        //or functionName(Type argument1) - has no return
        FunctionDeclaration,
        FunctionNoReturnDeclaration,
        FunctionParametersOpen,
        FunctionParametersClose,

        FunctionParameterName,
        FunctionParamsParameter, //array types are treated as params[]

        /// <summary>
        /// A special case for the compiler. A public function (static or not) named invoke()
        /// is treated such as that you can call object() rather than object.invoke()
        /// </summary>
        InvokeFunction,

        //Return
        EmptyReturn, //return (and nothing else)
        Return,

        //The completely optional semicolon
        StatementTerminator,

        //Access modifiers
        Private,
        Static,

        /// <summary>
        /// Open {
        /// </summary>
        OpenScope,
        /// <summary>
        /// Close }
        /// </summary>
        CloseScope,
    }
}
