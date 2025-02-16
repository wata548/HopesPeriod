using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

/// <summary>
/// <para>It's subClass must sealed</para>
/// <para>and compare subParameter and essentialParameter by setter's attribute</para>
/// <para>if subParamter, It's Property's setter must be private</para>
/// <para>if essentialParameter, It's Property's setter must be protected</para>
/// </summary>
public abstract class CommandBase {

    private bool usable = false;
    private bool on = false;
    private bool end = false;
    private CommandBase Next = null;

    public bool Usable() => usable;
    public bool On() => on;
    public bool End() => end;
    
    public void SetUsable(bool value) => usable = value;
    public bool Start() {
        if (!usable) 
            return false;

        if (on) return false;
        on = true;
        return true;
    }
    public void EndProcess() {

        if (!on)
            return;
        
        on = false;
        end = true;
        usable = false;
        if(Next is not null)
            Next.SetUsable(true);
    }

    public void SetNext(CommandBase next) {
        Next = next;
    }
}
/// <summary>
/// <para>Essential: Target(int), Route(List(Direction))</para>
/// <para>Sub: Follow(bool)</para>
/// ex) Move(Target = 5001 | Route = [Left, Right, Up, Down] | Follow = True);
/// </summary>
sealed public class MoveScriptCommand : CommandBase {
    public int Target { get; protected set; }
    public List<Direction> Route { get; protected set; }
    public float Power { get; private set; }
    public int Loop { get; private set; }
    public bool Follow { get; private set; } = false;
}
/// <summary>
/// <para>Sub: Parcent(float)</para>
/// ex) Zoom(0.5f);
/// </summary>
sealed public class ZoomScriptCommand : CommandBase {
    public float Percent { get; protected set; }
    public float Power { get; private set; }
}
/// <summary>
/// <para>Essential: Code(int)</para>
/// <para>Sub: Pos(Vector2), View(Direction)</para>
/// ex) GeneratePerson( Code = 5001 | Pos = {12f, 23f} | Direction = L);
/// </summary>
sealed public class GeneratePersonScriptCommand : CommandBase {
    public int Code { get; protected set; }
    public Vector2 Pos { get; private set; }
    public Direction View { get; private set; }
}
/// <summary>
/// <para>Essential: Image(string)</para>
/// ex) SetBackGround(Image = "Background01.png");
/// </summary>
sealed public class SetBackgroundScriptCommand : CommandBase {
    public string Image { get; protected set; }
}
/// <summary>
/// <para>Sub: Pos(Vector2), Zoom(float)</para>
/// ex) ControleBackground(Pos = {1f,2f} | Zoom = 0.5f);
/// </summary>
sealed public class ControleBackgroundScriptCommand : CommandBase {
    public Vector2 Pos { get; protected set; }
    public float Zoom { get; protected set; }
    public float Power { get; protected set; }
}
/// <summary>
/// ex) ClearBackground();
/// </summary>
sealed public class ClearBackgroundScriptCommand : CommandBase { }
/// <summary>
/// <para>Essential: MapCode(int)</para>
/// <para>Sub: Pos(Vector2)</para>
/// ex) SetMap(MapCode = 8001 | Pos = {1.2f, 4.3f});
/// </summary>
sealed public class SetMapScriptCommand : CommandBase {
    public int MapCode { get; protected set; }
    public Vector2 Pos { get; private set; }
}
/// <summary>
/// <para>Essential: Pos(Vector2)</para>
/// ex) SetCameraPos(Pos = {12f, 34f});
/// </summary>
sealed public class SetCameraPosScriptCommand : CommandBase {
    public Vector2 Pos { get; protected set; }
    public float Power { get; private set; }
}
/// <summary>
/// <para>Essential: Factor(List(string))</para>
/// <para>Sub: title(string)</para>
/// ex) MakeSelect(Factor = ["One", "Two", "Three"] | Title = "Numbers");
/// </summary> 
sealed public class MakeSelectScriptCommand : CommandBase {
    public List<string> Factor { get; protected set; }
    public string Title { get; private set; }
}
/// <summary>
/// <para>Essential: Target(int)</para>
/// <para>Sub: Pos(Vector2)</para>
/// ex) Focus(Target = 5001 | Pos = {12f, 34f});
/// </summary> 
sealed public class FocusScriptCommand : CommandBase {
    public int Target { get; protected set; }
    public Vector2 Pos { get; private set; } = Vector2.Zero;
    public float Power { get; private set; }
}
/// <summary>
/// <para>Essential: Target(int)</para>
/// <para>Sub: Pos(Vector2)</para>
/// ex) (Target = 5001 | Pos = {12f, 34f});
/// </summary> 
sealed public class SetPersonPosScriptCommand : CommandBase {
    public int Target { get; protected set; }
    public Vector2 Pos { get; private set; } = Vector2.Zero;
}

sealed public class StartChangeEffectScriptCommand : CommandBase {
    public bool Roll { get; private set; } 
    public float Power { get; protected set; }
}

sealed public class ClearEffectScriptCommand : CommandBase {
    public float Power { get; protected set; }
}
sealed public class WaitScriptCommand:CommandBase
{
    public float Power { get; protected set; }
}

public enum ScriptCodeKeyword {
    Wait,
    Move,
    Zoom,
    GeneratePerson,
    SetPersonPos,
    SetBackground,
    ControleBackground,
    ClearBackground,
    SetMap,
    SetCameraPos,
    MakeSelect,
    Focus,
    StartChangeEffect,
    ClearEffect
};

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
    public static List<(ScriptCodeKeyword, CommandBase)> Interpret(string input) {

        SetUp();

        var lines = input.Split(';');
        lines = lines.Take(lines.Length - 1).ToArray();

        List<(ScriptCodeKeyword, CommandBase)> result = new();
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

            result.Add((commandType, (CommandBase)data));

        }

        return result;
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
    private static Type ToClass(this ScriptCodeKeyword keyword) => Type.GetType(keyword.ToString() + "ScriptCommand") ?? throw new Exception($"Make {keyword.ToString()}'s class");
}
