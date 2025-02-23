using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public static class ScriptCodeInterpreter {
    public class Parameter {

        public string Name;
        public readonly Type Type;

        public Parameter(string name, Type type) {
            Name = name;
            Type = type;
        }

        public override string ToString() {

            if (Type.IsGenericType) {
                string type = Type.GetGenericTypeDefinition().Name;
                string genericType = Type.GetGenericArguments()[0].Name;

                return $"{Name}({type}<{genericType}>)";
            }

            return $"{Name}({Type.Name})";
        }
    }

    private static Dictionary<Type, string> typeMatch = new() {
        { typeof(string), @"""(.*?)""" },
        { typeof(bool), @"^\s*(true|false|t|f|True|False|T|F)" },
        { typeof(int), @"^\s*(-?\d*)" },
        { typeof(float), @"^\s*(-?\d*\.?\d+)f" },
        { typeof(Direction), "(.*)"},
        { typeof(List<>), @"^\s*\[(.*)\]"},
        { typeof(Vector2), @"^\s*\{(.*)\}"},
    };
    private static Dictionary<ScriptCodeKeyword, List<Parameter>?> essentialParam = new();
    private static Dictionary<ScriptCodeKeyword, List<Parameter>?> subParam = new();
    private static List<string> keywords = Enum.GetValues(typeof(ScriptCodeKeyword))
        .Cast<ScriptCodeKeyword>()
        .Select(factor => factor.ToString())
        .ToList();
    private static string keywordPattern = $@"({string.Join("|", keywords)})\s*\((.*?)\)";
    private static string parameterPattern = @"(\S*)\s*=\s*(.*)";

    private static bool isSetting = false;

    private static void SetUp() {
        if (isSetting)
            return;

        isSetting = true;
        var commands = Enum.GetValues(typeof(ScriptCodeKeyword));

        foreach (ScriptCodeKeyword command in commands) {

            Type type = command.ToClass();
            var parameters = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            bool haveEssential = false;
            bool haveSub = false;

            foreach (var parameter in parameters) {

                if (parameter.SetMethod is null) {
                    Debug.Log(command.ToString());
                    throw new NullReferenceException($"please make set({parameter.Name})");
                }
                if (parameter.SetMethod.IsFamily) {
                    if (!haveEssential) {
                        haveEssential = true;
                        essentialParam.Add(command, new());
                    }

                    essentialParam[command].Add(new(parameter.Name, parameter.PropertyType));
                }
                else if (parameter.SetMethod.IsPrivate) {
                    if (!haveSub) {
                        haveSub = true;
                        subParam.Add(command, new());
                    }
                    subParam[command].Add(new(parameter.Name, parameter.PropertyType));
                }
                else {
                    throw new Exception($"Setter({parameter.Name})'s setter must be protected or private. read ICommand's summary");
                }

            }
            if (!haveEssential)
                essentialParam.Add(command, null);
            else if (!haveSub)
                subParam.Add(command, null);
        }
    }
    public static List<(ScriptCodeKeyword, ICommand)> Interpret(string input) {

        SetUp();

        var lines = input.Split(';');
        lines = lines.Take(lines.Length - 1).ToArray();

        List<(ScriptCodeKeyword, ICommand)> result = new();
        foreach (var line in lines) {
            var rawDatas = InterpretOneLine(line, out var commandType);
            Type command = commandType.ToClass();
            object data = Activator.CreateInstance(command);

            foreach (var rawData in rawDatas) {

                var targetParam = command.GetProperty(rawData.param.Name, BindingFlags.Public | BindingFlags.Instance);
                //List casting
                if (rawData.param.Type.IsGenericType) {
                    var cast = typeof(Enumerable)
                        .GetMethod("Cast", BindingFlags.Public | BindingFlags.Static)!
                        .MakeGenericMethod(rawData.param.Type.GetGenericArguments()![0]);
                    var fixEnumerable = (IEnumerable)cast.Invoke(null, new[] { rawData.value });
                    var toList = typeof(Enumerable)
                        .GetMethod("ToList", BindingFlags.Public | BindingFlags.Static)!
                        .MakeGenericMethod(rawData.param.Type.GetGenericArguments()![0]);
                    IList fixList = (IList)toList.Invoke(null, new[] { fixEnumerable });

                    targetParam.SetValue(data, fixList);
                }
                else
                    targetParam.SetValue(data, rawData.value);
            }

            result.Add((commandType, (ICommand)data));

        }

        return result;
    }

    public static List<(ScriptCodeKeyword, CommandBase)> InterpretToCommandBase(string input) {
        var list = Interpret(input);
        
        List<(ScriptCodeKeyword, CommandBase)> result = new();
        list.ForEach(factor => result.Add((factor.Item1, (CommandBase)factor.Item2)));

        return result;
    }
    
    public static bool ToCondition(this List<(ScriptCodeKeyword, ICommand)> infos) {
        
        foreach (var info in infos) {

            if (info.Item2 is not BooleanCommandBase fixInfo)
                throw new Exception($"This {info.Item1.ToString()}type isn't returning boolean type");

            if (!fixInfo.Result())
                return false;
        }

        return true;
    }
    private static List<(Parameter param, object value)> InterpretOneLine(string input, out ScriptCodeKeyword keyword) {

        Match match = Regex.Match(input, keywordPattern);

        Debug.Log($"{match.Groups[1].Value} + Char({match.Groups[0].Value.Length})");
        keyword = Enum.Parse<ScriptCodeKeyword>(match.Groups[1].Value);
        List<(Parameter, object)> result = new();

        //non parameter command
        if (string.IsNullOrEmpty(match.Groups[2].Value)) {
            if (essentialParam[keyword] is null) {

                Console.WriteLine("Success");
                return result;
            }

            throw new NotEnoughParameter(essentialParam[keyword], subParam[keyword]);
        }

        //check obsolete parameter
        if (essentialParam[keyword] is null && subParam[keyword] is null)
            throw new Exception("This Command haven't parameter");

        //separate parameters
        var parameters = match.Groups[2].Value.Split('|');
        List<Parameter>? essential = essentialParam[keyword]?.ToList();
        bool correct = false;

        foreach (var parameter in parameters) {

            Match parameterMatch = Regex.Match(parameter, parameterPattern);
            string parameterName = parameterMatch.Groups[1].Value;
            Parameter? temp = essential?.FirstOrDefault(param => param.Name.Equals(parameterName));
            if (temp is not null) {
                essential!.Remove(temp);
            }
            else
                temp = subParam[keyword]?.FirstOrDefault(param => param.Name.Equals(parameterName));
            if (temp is null)
                throw new NotEnoughParameter(essentialParam[keyword], subParam[keyword]);

            bool isGeneric = false;
            Type parameterType = temp!.Type;
            Type genericType = typeof(int);

            //GenericTypeCheck
            if (parameterType.IsGenericType) {
                isGeneric = true;
                genericType = parameterType.GetGenericArguments()[0];
                parameterType = parameterType.GetGenericTypeDefinition();
            }
            if (parameterType == typeof(Vector2))
                genericType = typeof(float);

            string typePattern = typeMatch[parameterType];
            string data = Regex.Match(parameterMatch.Groups[2].Value, typePattern).Groups[1].Value;
            if (isGeneric) {

                List<object> list = new();
                string[] rawDatas = data.Split(',');
                typePattern = typeMatch[genericType];

                foreach (var rawData in rawDatas) {

                    Match factor = Regex.Match(rawData, typePattern);
                    list.Add(Parse(genericType, factor.Groups[1].Value));
                }

                result.Add((temp!, list));
            }
            else if (parameterType == typeof(Vector2)) {
                string[] rawDatas = data.Split(',');
                typePattern = typeMatch[genericType];

                if (rawDatas.Length != 2)
                    throw new Exception($"this vector type is not correct ({keyword})");
                float x = float.Parse((Regex.Match(rawDatas[0], typePattern).Groups[1].Value));
                float y = float.Parse((Regex.Match(rawDatas[1], typePattern).Groups[1].Value));
                result.Add((temp!, new Vector2(x, y)));
            }
            else
                result.Add((temp!, Parse(parameterType, data)));


            if (essential is null || essential!.Count <= 0)
                correct = true;
        }

        if (!correct) {
            throw new NotEnoughParameter(essentialParam[keyword], subParam[keyword]);
        }

        Console.WriteLine("Success");
        return result;
    }
    private class NotEnoughParameter : Exception {
        public NotEnoughParameter()
            : base("please set parameters") { }
        public NotEnoughParameter(List<Parameter>? essential)
            : base("please set parameters"
                   + $"\nessential: {string.Join(", ", essential?.Select(param => param.ToString()) ?? new string[1])}") { }
        public NotEnoughParameter(List<Parameter>? essential, List<Parameter>? sub)
            : base("please set parameters"
                   + $"\nessential: {string.Join(", ", essential?.Select(param => param.ToString()) ?? new string[1])}"
                   + $"\nsub: {string.Join(", ", sub?.Select(param => param.ToString()) ?? new string[1])}") { }
        public NotEnoughParameter(Parameter[]? essential, Parameter[]? sub)
            : base("please set parameters"
                   + $"\nessential: {string.Join(", ", essential?.Select(param => param.ToString()) ?? new string[1])}"
                   + $"\nsub: {string.Join(", ", sub?.Select(param => param.ToString()) ?? new string[1])}") { }
        public NotEnoughParameter(Parameter[]? essential)
            : base("please set parameters"
                   + $"\nessential: {string.Join(", ", essential?.Select(param => param.ToString()) ?? new string[1])}") { }
    }
    private static object Parse(Type type, string context) {
        if (string.IsNullOrEmpty(context))
            throw new Exception("parse target should not null or empty");

        if (type == typeof(string))
            return context;

        if (type == typeof(Direction)) {

            string[] directions = context.Split('+');
            Direction result = Direction.None;

            foreach (var direction in directions) {
                string dir = Regex.Match(direction, @"^\s*(left|right|up|down|l|r|u|d|Left|Right|Up|Down|L|R|D|U)").Groups[1].Value;
                char initial = (char)(dir[0] | 32);

                result |= initial switch {
                    'l' => Direction.Left,
                    'r' => Direction.Right,
                    'u' => Direction.Up,
                    'd' => Direction.Down,
                };
            }

            return result;
        }

        if (type == typeof(bool)) {
            char initial = (char)(context[0] | 32);
            return initial switch {
                't' => true,
                'f' => false
            };
        }
        var parse = type.GetMethod("Parse", new[] { typeof(string) });
        return parse.Invoke(null, new[] { context });
    }
    private static Type ToClass(this ScriptCodeKeyword keyword) 
        => Type.GetType(keyword.ToString() + "ScriptCommand") ?? throw new Exception($"Make {keyword.ToString()}'s class");
}
