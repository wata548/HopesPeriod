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
    private bool justTutorial = false;
    private ScriptDBData currentData;
    private ScriptDBDataTable table = null;
    public int EventCode => eventCode;
    public void StartScript(int code) {
        Debug.Log($"Start Script: {code}");

        if (this.eventCode != 0) {

            Debug.Log($"Script({code}) is canceled");
            return;
        }
        
        TilePlayerPhysics.SetMovable(false);
        SettingWindow.SetInteractable(false);
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
        
        //if (data.ClearContext)
            //backgroundScript.Erase();
            
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

    public void ShowTutorial(int code) {
         Debug.Log($"Start Tutorial: {code}");
                
         TilePlayerPhysics.SetMovable(false);
         SetTable();
         eventCode = code;
         justTutorial = true;
        
        if(!end)
            return;
                
        end = false;
        start = true;
        currentData = new();

        string eventInfo = $"ShowTutorial( Code = {code});";
        
        ScriptCodePlayer.Instance.Interpret(eventInfo);
        return;
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
        SettingWindow.SetInteractable(true);
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
        if (justTutorial || currentData.JustEvent || backgroundScript.End || defaultScript.End) {
            //SetUp
            start = false;
            end = true;
            startTalking = false;

            //End Script
            if (justTutorial || table.DataTable[eventCode].Count <= index) {
                justTutorial = false;
                
                
                FindEventInfo.FindEvent(EventCode);
                
                backgroundScript.StartSetUp();
                defaultScript.StartSetUp();
                EndProcess();
                Debug.Log("Script End");
                EverytimeEvent.StartEvent();
            }

            //Next Script
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