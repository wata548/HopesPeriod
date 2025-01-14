using UnityEngine;
using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public enum Effect {
        
    Flow,
    Shake,
    None
};

public static class TextExtension {


    private static readonly Dictionary<Effect, Func<float, Vector3>> match = new() {
            { Effect.Flow, index => new Vector3(0, Mathf.Sin(index * 3f + Time.time) / 4f)},
            { Effect.Shake, index => new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.5f, 0.5f), 0) / 5f },
    };
    
    /*public void StartTyping(TMP_Text text, string s, float interval = 0.1f) {
        
        text.text = "";
        StartCoroutine(Typing(text,s, interval));
    }*/
        
    public static IEnumerator Typing(this TMP_Text text, string context, float interval) {
        
        bool tag = false;
        
        foreach (var character in context) {
        
            if (character == '<' || character == '[') {
                tag = true;
            }
        
            if (character == '>' || character == ':')
                tag = false;
            text.text += character;
        
            if (!tag) {
                EffectProcedure(text);
                yield return new WaitForSeconds(interval);
            }
        
        }
    }
        
    public static void EffectProcedure(TMP_Text text) {
        
        
        bool isEffectTypeTyping = false;
        string effectType = "";
        Effect currentEffect = Effect.None;
                
        text.ForceMeshUpdate();
        var textInfo = text.textInfo;
        
        int index = 0;
        bool effect = false;
                
        foreach (var character in textInfo.characterInfo) {
        
            if(!character.isVisible) 
                continue;
        
            //check effectType
            if (isEffectTypeTyping) {
                if (character.character == ':') {
                    isEffectTypeTyping = false;
        
                    currentEffect = (Effect)Enum.Parse(typeof(Effect),  effectType);
                    continue;
                }
        
                effectType += character.character;
                continue;
            }    
        
            //Start effect
            if (character.character == '[') {
                effectType = "";
                effect = true;
                isEffectTypeTyping= true;
                continue;
            }
        
            //End effect
            if (character.character == ']' && effect) {
                currentEffect = Effect.None;
                effect = false;
            }
                    
            if (!effect) {
                continue;
            }
            var vertices = textInfo.meshInfo[character.materialReferenceIndex].vertices;
        
            //Effect impact calculate
            var move = match[currentEffect](index / 2f + Time.time);
            for (int vertex = 0; vertex < 4; vertex++) {
        
                var tempIndex = character.vertexIndex + vertex;
                        
                var origin = vertices[tempIndex];
                vertices[tempIndex] = origin + move;
            }
            index++;
        }
        
        foreach (var mesh in textInfo.meshInfo) {
        
            mesh.mesh.vertices = mesh.vertices;
        }
        
    }
    
    /// <summary>
    /// Color format example: "#ff00ff" or "#ffffffdd"
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string AddColor(this string context, string color) {
            
        return $"<color={color}>{context}</color>";
    }
    
    public static string AddColor(this string context, Color color) {
        
        Debug.Log(color.ToString());
        return AddColor($"#{ToHex(color.r)}{ToHex(color.g)}{ToHex(color.b)}{ToHex(color.a)}", context);
        
        string ToHex(float factor) {
        
            int value = (int)(factor * 255);
            return Convert.ToString(value, 16).PadLeft(2, '0');
        }
    }
        
    public static string AddEffect(this string context, Effect effect) {
        
        return $"<size=0%>[{effect}:</size>{context}<size=0%>]</size>";
    }
    public static string AddEffect(this string context, string effect) {
            
        return AddEffect(effect.ToString(), context);
    }
}
