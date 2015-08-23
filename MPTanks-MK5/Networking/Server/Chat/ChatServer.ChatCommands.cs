using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks.Networking.Server.Chat
{
    public partial class ChatServer
    {
        public int CommandListItemsPerPage { get; set; } = 20;

        #region Command Addition
        public enum ChatCommandParameter
        {
            /// <summary>
            /// "string1"
            /// "string 2"
            /// "string \"3\""
            /// </summary>
            String,
            /// <summary>
            /// playername
            /// </summary>
            Player,
            /// <summary>
            /// 1
            /// 2
            /// 1.223
            /// 5.552E72
            /// </summary>
            Number,
            /// <summary>
            /// true
            /// false
            /// </summary>
            Boolean,
            /// <summary>
            /// [ "string 1", "string2", "string \" 3" ] 
            /// </summary>
            ArrayOfString,
            /// <summary>
            /// [ player1, player2, player3 ]
            /// </summary>
            ArrayOfPlayer,
            /// <summary>
            /// [ 1, 2, 3 ]
            /// </summary>
            ArrayOfNumber,
            /// <summary>
            /// [ true, false, true ]
            /// </summary>
            ArrayOfBoolean
        }
        private Dictionary<string, List<CommandDefinition>> _commands = new Dictionary<string, List<CommandDefinition>>(StringComparer.InvariantCultureIgnoreCase);

        public class CommandDefinition
        {
            public bool IsOverload { get; internal set; }
            public string Name { get; internal set; }
            public string Description { get; internal set; }
            public Delegate Function { get; internal set; }
            public ExtendedChatCommandParameter[] Parameters { get; internal set; }
        }

        public struct ExtendedChatCommandParameter
        {
            public ChatCommandParameter Parameter { get; internal set; }
            public string DefaultValue { get; internal set; }
            public string Name { get; internal set; }
            public ExtendedChatCommandParameter(ChatCommandParameter param, ParameterInfo info)
            {
                Parameter = param;
                DefaultValue = info.IsOptional ? info.DefaultValue.ToString() : null;
                Name = info.Name;
            }
        }

        public void RegisterCommand(Delegate action, string name, string description)
        {
            name = name.Trim();
            //find the parameters
            var methodParams = action.Method.GetParameters();

            var extParameters = new List<ExtendedChatCommandParameter>();

            foreach (var methodParam in methodParams)
            {
                if (methodParam.ParameterType == typeof(string))
                    extParameters.Add(
                        new ExtendedChatCommandParameter(ChatCommandParameter.String, methodParam));
                else if (methodParam.ParameterType == typeof(string[]))
                    extParameters.Add(
                        new ExtendedChatCommandParameter(ChatCommandParameter.String, methodParam));
                else if (methodParam.ParameterType == typeof(bool))
                    extParameters.Add(
                        new ExtendedChatCommandParameter(ChatCommandParameter.Boolean, methodParam));
                else if (methodParam.ParameterType == typeof(bool[]))
                    extParameters.Add(
                        new ExtendedChatCommandParameter(ChatCommandParameter.ArrayOfBoolean, methodParam));
                else if (methodParam.ParameterType == typeof(double))
                    extParameters.Add(
                        new ExtendedChatCommandParameter(ChatCommandParameter.Number, methodParam));
                else if (methodParam.ParameterType == typeof(double[]))
                    extParameters.Add(
                        new ExtendedChatCommandParameter(ChatCommandParameter.ArrayOfNumber, methodParam));
                else if (methodParam.ParameterType == typeof(ServerPlayer))
                    extParameters.Add(
                        new ExtendedChatCommandParameter(ChatCommandParameter.Player, methodParam));
                else if (methodParam.ParameterType == typeof(ServerPlayer[]))
                    extParameters.Add(
                        new ExtendedChatCommandParameter(ChatCommandParameter.ArrayOfPlayer, methodParam));
                else
                {
                    throw new ArgumentException(
                        $"Method contains unrecognized parameter type {methodParam.ParameterType.FullName}. " +
                        "Allowed parameter types are string, string[], bool, bool[], double, double[], " +
                        "ServerPlayer, and ServerPlayer[].");
                }
            }

            var parameters = extParameters.Select(a => a.Parameter).ToArray();

            AddDefinition(new CommandDefinition()
            {
                Function = action,
                Name = name,
                Description = description,
                Parameters = extParameters.ToArray()
            });
        }

        private void AddDefinition(CommandDefinition def)
        {
            if (!_commands.ContainsKey(def.Name)) _commands.Add(def.Name, new List<CommandDefinition>());

            int optParamCount = 0;
            foreach (var param in def.Parameters)
                if (param.DefaultValue != null) optParamCount++;

            if (optParamCount > 0)
                for (var i = 0; i < optParamCount; i++)
                {
                    var definition = new CommandDefinition()
                    {
                        Name = def.Name,
                        Description = def.Description,
                        Function = def.Function,
                        Parameters = def.Parameters.Take(def.Parameters.Length - i).ToArray(),
                        IsOverload = true
                    };
                    var paramList = definition.Parameters.Select(a => a.Parameter).ToArray();

                    if (GetCommand(def.Name, paramList) != null)
                        throw new Exception($"Command {def.Name} already exists with the specified parameter set");

                    _commands[def.Name].Add(definition);
                }
            else _commands[def.Name].Add(def);

        }
        #endregion
        public CommandDefinition GetCommand(string name, ChatCommandParameter[] parameters)
        {
            if (!_commands.ContainsKey(name) || _commands[name].Count == 0) return null;

            foreach (var option in _commands[name])
            {
                if (option.Parameters.Length != parameters.Length) continue;
                bool valid = true;
                for (var i = 0; i < option.Parameters.Length; i++)
                {
                    var inputParam = parameters[i];
                    var optionParam = option.Parameters[i];
                    if (inputParam != optionParam.Parameter)
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid) return option;
            }

            return null;
        }

        public string PrintCommandList(int page)
        {
            var commandsToPrint = new List<CommandDefinition>();
            int numberOfPrintableCommandsProcessed = 0;
            int firstItemToShow = page * CommandListItemsPerPage;
            int numberOfItemsPrinted = 0;

            foreach (var commandSet in _commands)
            {
                foreach (var command in commandSet.Value)
                {
                    if (command.IsOverload) continue;
                    numberOfPrintableCommandsProcessed++;
                    if (numberOfItemsPrinted >= CommandListItemsPerPage) continue;
                    if (firstItemToShow <= numberOfPrintableCommandsProcessed)
                    {
                        commandsToPrint.Add(command);
                        numberOfItemsPrinted++;
                    }
                }
            }

            int pageCount = (int)Math.Ceiling((double)numberOfPrintableCommandsProcessed / CommandListItemsPerPage);
            var outputBuilder = new StringBuilder();
            outputBuilder.AppendLine($"Commands List (page {page} of {pageCount})")
                .Append("Strings must be in quotes, Arrays are defined as [value1, value2], ")
                .AppendLine("and booleans can be y, n, true, false, yes, or no.");
            foreach (var command in commandsToPrint)
            {
                outputBuilder.Append(command.Name).Append("(");
                foreach (var param in command.Parameters)
                {
                    if (param.Parameter == ChatCommandParameter.String)
                        outputBuilder.Append("string ");
                    if (param.Parameter == ChatCommandParameter.ArrayOfString)
                        outputBuilder.Append("string array ");
                    if (param.Parameter == ChatCommandParameter.Boolean)
                        outputBuilder.Append("boolean ");
                    if (param.Parameter == ChatCommandParameter.ArrayOfBoolean)
                        outputBuilder.Append("boolean array ");
                    if (param.Parameter == ChatCommandParameter.Number)
                        outputBuilder.Append("number ");
                    if (param.Parameter == ChatCommandParameter.ArrayOfNumber)
                        outputBuilder.Append("number array ");
                    if (param.Parameter == ChatCommandParameter.Player)
                        outputBuilder.Append("player ");
                    if (param.Parameter == ChatCommandParameter.ArrayOfPlayer)
                        outputBuilder.Append("player array ");

                    outputBuilder.Append(param.Name);
                    if (param.DefaultValue != null)
                        outputBuilder.Append(" = ").Append(param.DefaultValue);

                    outputBuilder.Append(", ");
                }
                outputBuilder.Remove(outputBuilder.Length - 3, 2);
                outputBuilder.Append(")");

                outputBuilder.Append(" | ").AppendLine(command.Description);
            }

            return outputBuilder.ToString();
        }
        #region Invokation
        public void CallCommand(CommandDefinition def, object[] arguments)
        {
            def.Function.DynamicInvoke(arguments);
        }

        public void CallCommand(string commandString, ServerPlayer sender)
        {
            try
            {
                var name = GetCommandName(commandString).Trim();
                var args = GetCommandArguments(commandString);
                var commandParamTypes = new ChatCommandParameter[args.Length];
                for (var i = 0; i < args.Length; i++)
                {
                    if (args[i].GetType() == typeof(string))
                        commandParamTypes[i] = ChatCommandParameter.String;
                    if (args[i].GetType() == typeof(string[]))
                        commandParamTypes[i] = ChatCommandParameter.ArrayOfString;
                    if (args[i].GetType() == typeof(bool))
                        commandParamTypes[i] = ChatCommandParameter.Boolean;
                    if (args[i].GetType() == typeof(bool[]))
                        commandParamTypes[i] = ChatCommandParameter.ArrayOfBoolean;
                    if (args[i].GetType() == typeof(double))
                        commandParamTypes[i] = ChatCommandParameter.Number;
                    if (args[i].GetType() == typeof(double[]))
                        commandParamTypes[i] = ChatCommandParameter.ArrayOfNumber;
                    if (args[i].GetType() == typeof(ServerPlayer))
                        commandParamTypes[i] = ChatCommandParameter.Player;
                    if (args[i].GetType() == typeof(ServerPlayer[]))
                        commandParamTypes[i] = ChatCommandParameter.ArrayOfPlayer;
                }

                //Get the command object
                var command = GetCommand(name, commandParamTypes);
                if (command == null)
                {
                    SendMessage($"No command exists by that name and accepts those parameters", sender);
                    return;
                }
            }
            catch (Exception ex)
            {
                Server.Logger.Error("[CHAT] Command Invokation Error", ex);
                SendMessage("Something went wrong when processing that command.", sender);
            }
        }

        private string GetCommandName(string commandString)
        {
            return commandString.Split(' ')[0].Substring(CommandMarker.Length);
        }
        private object[] GetCommandArguments(string commandString)
        {
            return null; //TODO
        }
        #endregion
    }
}
