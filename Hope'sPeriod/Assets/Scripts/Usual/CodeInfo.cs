using System;
using UnityEngine;
public static class CodeInfo {

    private static Sprite error = null;
    
    private const int CodeMask = 1000;
    public static CodeType ToCodeType(this int code) {
        return (CodeType)(code / CodeMask); 
    }
    
    public static Sprite LoadImage(int code) {

        var image = Resources.Load<Sprite>($"CodeImage/{code.ToCodeType().ToString()}/{code}");
        if (image is null) {
            if (error is null) {
                error = Resources.Load<Sprite>("CodeImage/Error");
                if (error is null)
                    throw new Exception("error image isn't exist, check CodeImage/Error");
            }

            image = error;
        }

        return image;
    }
}