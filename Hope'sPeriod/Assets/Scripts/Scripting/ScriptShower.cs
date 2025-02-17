using SpreadInfo;
using UnityEditor.Searcher;

public class ScriptShower {

    private bool end = true;
    private bool start = false;
    private bool justEvent = false;
    
    private bool eventEnd = false;
    private bool showEnd = false;

    private Timing timing = Timing.BeforeTalking;
    
    public void Show(ScriptDBData data) {

        if(!end)
            return;
        
        end = false;
        start = true;
        eventEnd = false;
        showEnd = false;
        
        justEvent = data.JustEvent;
        timing = data.EventTiming;
        
        if (data.WindowType == WindowType.BackgroundImage) {
            
            if (data.ClearContext)
                ScriptCodePlayer.Instance.script.Erase();
            
            if (data.JustEvent) {

                justEvent = true;
                ScriptCodePlayer.Instance.Interpret(data.Event);
                return;
            }    
        }
    }

    private void Update() {

        if (!start)
            return;
        eventEnd = ScriptCodePlayer.Instance.EndEvent();
        if (eventEnd) {
            if (justEvent) {
                start = false;
                justEvent = false;
                end = true;
            }
            else if (timing == Timing.BeforeTalking) {
                
            }

            eventEnd = true;
        }   
    }
}