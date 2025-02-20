using System.Collections.Generic;
using Vector2 = System.Numerics.Vector2;

public interface ICommand {}


/// <para>It's subClass must sealed</para>
/// <para>and compare subParameter and essentialParameter by setter's attribute</para>
/// <para>if subParamter, It's Property's setter must be private</para>
/// <para>if essentialParameter, It's Property's setter must be protected</para>
/// </summary>
public abstract class CommandBase: ICommand {

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
/// <para>Essential:Code(int)</para>
/// <para>Sub: Count(int)</para>
/// ex) Move(Target = 5001 | Route = [Left, Right, Up, Down] | Follow = True);
/// </summary>
sealed public class GetItemScriptCommand : CommandBase {
    
    public int Code { get; protected set; }
    public int Count { get; private set; }
}
/// <summary>
/// <para>Essential: Target(int), Route(List(Direction))</para>
/// <para>Sub: Power(float), Loop(int), Follow(bool)</para>
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
/// <para>Essential: Parcent(float)</para>
/// <para>Sub: Power(float)</para>
/// ex) Zoom(Precent = 0.5f);
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
/// <para>Essential: Pos(Vector2), Zoom(float), Power(float)</para>
/// ex) ControleBackground(Pos = {1f,2f} | Zoom = 0.5f);
/// </summary>
sealed public class ControleBackgroundScriptCommand : CommandBase {
    public Vector2 Pos { get; protected set; }
    public float Zoom { get; protected set; }
    public float Power { get; protected set; }
}
/// <summary>
/// No Parameter
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
/// <para>Sub: Power(float)</para>
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
/// <para>Sub: Pos(Vector2), Power(float)</para>
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
/// <summary>
/// <para>Essential: Power(float)</para>
/// <para>Sub: Roll(bool)</para>
/// </summary>
sealed public class StartChangeEffectScriptCommand : CommandBase {
    public float Power { get; protected set; }
    public bool Roll { get; private set; } 
}

/// <summary>
/// <para>Essential: Power(float)</para>
/// </summary>
sealed public class ClearEffectScriptCommand : CommandBase {
    public float Power { get; protected set; }
}
/// <summary>
/// <para>Essential: Power(float)</para>
/// </summary>
sealed public class WaitScriptCommand:CommandBase {
    public float Power { get; protected set; }
}

/// <summary>
/// <para>Essential: Code(int)</para>
/// </summary>
sealed public class SetChapterScriptCommand : CommandBase {
    public int Code { get; protected set; }
}

/// <summary>
/// <para> Essential: Pos(Vector2), Duraction(float)</para>
/// </summary>
sealed public class AddRealPosScriptCommand : CommandBase {
    public Vector2 Pos { get; protected set; }
    public float Duraction { get; protected set; }
}

/// <summary>
/// <para> Essential: Info(string)
/// <para> Sub: Image(string), Title(string)
/// </summary>
sealed public class TutorialScriptCommand : CommandBase {
    public string Info { get; protected set; }
    public string Title { get; private set; }
    public string Image { get; private set; }
}

/// <summary>
/// <para> Essential: Code(int)
/// </summary>
sealed public class ShowTutorialScriptCommand : CommandBase {
    public int Code { get; protected set; }
}

public enum ScriptCodeKeyword {
    Wait,
    Focus,
    SetCameraPos,
    Move,
    Zoom,
    
    //MeetMonster,
    AddRealPos,
    //SetRealMap,
    
    GeneratePerson,
    SetPersonPos,
    SetMap,
    GetItem,
    
    SetBackground,
    //ControleBackground,
    ClearBackground,
    
    MakeSelect,
    
    StartChangeEffect,
    ClearEffect,
    
    SetChapter,
    Tutorial,
    ShowTutorial,
    
    IsUseItem,
    IsHaveItem,
    IsFindItem,
    IsEventShowed,
    IsChapterEqual
};