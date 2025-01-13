using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static  class DOExtention {

    public static Tween DOBlink(this SpriteRenderer target, float appear = 0, float stay = 0, float disappear = 0, float power = 1, float underPower = 0) {

        if (appear < 0 || stay < 0 || disappear < 0 || power is < 0 or > 1 || underPower is < 0 or > 1)
            throw new ArgumentOutOfRangeException(" function's parameter should be over 0" + 
                                                  $"AppearTime: {appear}, StayTime: {stay}, DisappearTime: {disappear}, AppearPower: {power}");
        
        var procedure = DOTween.Sequence()
            .Append(target.DOFade(endValue: power, duration: appear))
            .AppendInterval(interval: stay)
            .Append(target.DOFade(endValue: underPower, duration: disappear));

        return procedure;
    }
    public static Tween DOBlink(this Image target, float appear = 0, float stay = 0, float disappear = 0, float power = 1, float underPower = 0) {
       
        if (appear < 0 || stay < 0 || disappear < 0 || power is < 0 or > 1 || underPower is < 0 or > 1)
            throw new ArgumentOutOfRangeException(" function's parameter should be over 0" + 
                                                  $"AppearTime: {appear}, StayTime: {stay}, DisappearTime: {disappear}, AppearPower: {power}");
        
        var procedure = DOTween.Sequence()
            .Append(target.DOFade(endValue: power, duration: appear))
            .AppendInterval(interval: stay)
            .Append(target.DOFade(endValue: underPower, duration: disappear));
        
        return procedure;
    }
    
    public static Tween DOBlink(this TMP_Text target, float appear = 0, float stay = 0, float disappear = 0, float power = 1, float underPower = 0) {
           
        if (appear < 0 || stay < 0 || disappear < 0 || power is < 0 or > 1 || underPower is < 0 or > 1)
            throw new ArgumentOutOfRangeException(" function's parameter should be over 0" + 
                                                  $"AppearTime: {appear}, StayTime: {stay}, DisappearTime: {disappear}, AppearPower: {power}");
            
        var procedure = DOTween.Sequence()
            .Append(target.DOFade(endValue: power, duration: appear))
            .AppendInterval(interval: stay)
            .Append(target.DOFade(endValue: underPower, duration: disappear));
            
        return procedure;
    }

    public static Tween DOBeforeWait(this Tween target, float time) {
        
        var result = DOTween.Sequence() 
            .AppendInterval(time)
            .Append(target);

        return result;
    }
}