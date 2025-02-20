using SpreadInfo;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.WSA;

public class ScriptShower: MonoBehaviour {

    [SerializeField] private BackgroundScriptShower backgroundScript;
    [SerializeField] private DefaultScriptShower defaultScript;
    public static ScriptShower Instance { get; private set; }
    
    private bool end = true;
    private bool start = false;
    
    private int eventCode = 0;
    private int index = 0;
    
    private bool startTalking = false;
    private ScriptDBData currentData;
    private ScriptDBDataTable table = null;
    public int EventCode => eventCode;
    public void StartScript(int code) {
        Debug.Log($"Start Script: {code}");
        
        TilePlayerPhysics.SetMovable(false);
        SetTable();
        eventCode = code;
        index = 0;
        Show(table.DataTable[eventCode][index++]);
    }
    
    public void Show(ScriptDBData data) {

        if(!end)
            return;
        
        end = false;
        start = true;
        currentData = data;
        
        if (data.ClearContext)
            backgroundScript.Erase();
            
        //StartEvent
        ScriptCodePlayer.Instance.Interpret(data.Event);

        if (data.ClearContext) {
            backgroundScript.TurnOff();
            defaultScript.TurnOff();
        }
        
        if (data.JustEvent)
            return;
            
        //Process of Event and Showing script run at same time
        if (data.EventTiming == Timing.Talking) {

            ShowScript(data.WindowType);
        }
    }

    private void ShowScript(WindowType type) {
        
        
        if (type == WindowType.BackgroundImage) {
                            
            backgroundScript.TurnOn();
            defaultScript.TurnOff();
            backgroundScript.ShowScript(currentData.Actor, currentData.Script);
        }
        else if ( type == WindowType.Default) {
                            
            defaultScript.TurnOn();
            backgroundScript.TurnOff();
            defaultScript.ShowScript(currentData.Actor, currentData.Script, currentData.Profile);
        }
    }
    
    private void EndProcess() {
        Debug.Log("end");
        
        eventCode = 0;
        defaultScript.TurnOff();
        backgroundScript.TurnOff();
        ScriptCodePlayer.Instance.EndProcess();
        TilePlayerPhysics.SetMovable(true);
    }
    
    private void SetTable() => 
        table ??= Resources.Load<ScriptDBDataTable>("SpreadInfo/Generated/ScriptDBDataTable");

    private void Awake() {
        Instance = this;
    }
    
    private void Update() {

        if (eventCode == 0)
            return;

        SetTable();    

        //check: is showing script start 
        if (!start)
            return;
        
        bool eventEnd = ScriptCodePlayer.Instance.EndEvent();
        if (!eventEnd) return;

        //Next script
        if (currentData.JustEvent || backgroundScript.End || defaultScript.End) {
            //SetUp
            start = false;
            end = true;
            startTalking = false;
            
            if (table.DataTable[eventCode].Count <= index)
                EndProcess();

            else {
                backgroundScript.StartSetUp();
                defaultScript.StartSetUp();
                Show(table.DataTable[eventCode][index++]);
            }

            return;
        }
        
        //Start Show Talking
        if (!startTalking && currentData.EventTiming == Timing.BeforeTalking) {

            ShowScript(currentData.WindowType);
            startTalking = true;
        }   
    }

    
}