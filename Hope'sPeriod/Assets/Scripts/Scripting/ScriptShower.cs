using SpreadInfo;
using UnityEditor.Searcher;
using UnityEngine;

public class ScriptShower: MonoBehaviour {

    [SerializeField] private BackgroundScriptShower backgroundScript;
    [SerializeField] private DefaultScriptShower defaultScript;
    private bool end = true;
    private bool start = false;
    
    private bool startTalking = false;
    private ScriptDBData currentData;
    private ScriptDBDataTable table = null;
    
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
            
        if (data.JustEvent)
            return;
            
        //Process of Event and Showing script run at same time
        if (data.EventTiming == Timing.Talking) {

            ScriptCodePlayer.Instance.Interpret(data.Event);
            if (data.WindowType == WindowType.BackgroundImage) {
                
                backgroundScript.TurnOn();
                defaultScript.TurnOff();
                backgroundScript.ShowScript(data.Actor, data.Script);
            }
            else if (data.WindowType == WindowType.Default) {
                
                defaultScript.TurnOn();
                backgroundScript.TurnOff();
                defaultScript.ShowScript(data.Actor, data.Script, data.Profile);
            }
                
        }
    }

    private int index = 0;
    private void Update() {

        table ??= Resources.Load<ScriptDBDataTable>("SpreadInfo/Generated/ScriptDBDataTable");

        if (Input.GetKeyDown(KeyCode.T)) {
            Show(table.DataTable[6000][index]);
        }
        
        //If didn't read info, code didn't run
        if (!start)
            return;
        
        bool eventEnd = ScriptCodePlayer.Instance.EndEvent();
        //Next script
        if (eventEnd && (currentData.JustEvent || backgroundScript.End || defaultScript.End)) {
            start = false;
            end = true;
            startTalking = false;
            backgroundScript.Use();
            defaultScript.Use();
            Show(table.DataTable[6000][index++]);

            return;
        }
        
        //Start Show Talking
        if (!startTalking && eventEnd && currentData.EventTiming == Timing.BeforeTalking) {

            if (currentData.WindowType == WindowType.BackgroundImage) {
                
                backgroundScript.TurnOn();
                defaultScript.TurnOff();
                backgroundScript.ShowScript(currentData.Actor, currentData.Script);
            }
            else if (currentData.WindowType == WindowType.Default) {
                
                defaultScript.TurnOn();
                backgroundScript.TurnOff();
                defaultScript.ShowScript(currentData.Actor, currentData.Script, currentData.Profile);
            }
            startTalking = true;
        }   
    }
}