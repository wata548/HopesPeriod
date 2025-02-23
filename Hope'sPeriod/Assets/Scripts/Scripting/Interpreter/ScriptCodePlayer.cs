using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScriptCodePlayer: MonoBehaviour {

    [SerializeField] private Image background;
    [SerializeField] private Image upperBackground;
    
    public static ScriptCodePlayer Instance { get; private set; } = null;
    
    private const int Infinity = 100;
    private const int DefaultCameraZPos = -10;
    private static readonly Vector3 DefaultPos = new(400, 400);
    private static readonly Vector3 PersonDefaultPos = new(0, 0.13f);
    private Dictionary<int, GameObject> actors = new();
    private List<(ScriptCodeKeyword, CommandBase)> process = new();
    private MapEventInfo prefabs = null;
    private GameObject currentMap = null;
    
    private void SetAnimation(Animator animator, Vector2 value, bool applySpeed = true) {
        animator.SetFloat("Horizontal", value.x);
        animator.SetFloat("Vertical", value.y);
        if(applySpeed)
            animator.SetFloat("Speed", value.magnitude);
    }
    public void Interpret(string input) {
        
        if(string.IsNullOrEmpty(input))
            return;
        
        var list = ScriptCodeInterpreter.InterpretToCommandBase(input);

        //Set Next
        (list[0].Item2).SetUsable(true);
        for (int i = 0; i < list.Count - 1; i++) {
            (list[i].Item2).SetNext(list[i + 1].Item2);
        }

        process.AddRange(list);
    }
    private void ScriptProcess() {
        foreach (var command in process) {
            ClasifyScript(command.Item1, command.Item2);
        }

        //Remove already end event
        process = process.Where(command => !command.Item2.End()).ToList();
    }

    public int Count => process.Count;
    
    public bool EndEvent() => process.Count <= 0;
    
    #region ScriptPlayer  
    private void ClasifyScript(ScriptCodeKeyword type, CommandBase command) {

        var play = typeof(ScriptCodePlayer).GetMethod($"{type.ToString()}Script", BindingFlags.NonPublic | BindingFlags.Instance)!;
        play.Invoke(this, new[] { command });
    }

    private void StartChangeEffectScript(StartChangeEffectScriptCommand command) {
        if (!command.Start())
            return;

        if (command.Roll)
            ScenceChangeEffecter.Instance
                .StartEffect(command.Power)
                .OnComplete(() => command.EndProcess());
        else
            ScenceChangeEffecter.Instance
                .Appear(command.Power)
                .OnComplete(() => command.EndProcess());

    }

    private void ClearEffectScript(ClearEffectScriptCommand command) {
        if (!command.Start())
            return;

        ScenceChangeEffecter.Instance
            .EndEffect(command.Power)
            .OnComplete(() => command.EndProcess());
    }
    private void SetBackgroundScript(SetBackgroundScriptCommand command) {
        if (!command.Start())
            return;

        var target = command.Upper ? upperBackground : background;

        target.DOFade(0, 0);
        target.enabled = true;
        target.sprite = Resources.Load<Sprite>($"Background/{command.Image}");
        target.DOFade(1, command.FadeTime)
            .OnComplete(command.EndProcess);
    }
    private void ClearBackgroundScript(ClearBackgroundScriptCommand command) {
        if (!command.Start())
            return;

        var target = command.Upper ? upperBackground : background;
        
        target.sprite = null;
        target.transform.localScale = new(1, 1);
        target.transform.localPosition = new(0,0);
        target.enabled = false;
        command.EndProcess();
    }
    private void ControleBackgroundScript(ControleBackgroundScriptCommand command) {
        if (!command.Start())
            return;
        
    }
    private void WaitScript(WaitScriptCommand command) {
        if (!command.Start())
            return;

        StartCoroutine(Wait.WaitAndDo(command.Power, () => command.EndProcess()));
    }
    private void SetMapScript(SetMapScriptCommand command) {
        
        if (!command.Start())
            return;
        
        if(prefabs is null)
            prefabs = Resources.Load<MapEventInfo>("MapPrefab/MapInfo");

        if (currentMap is not null)
            Destroy(currentMap);
        
        currentMap = Instantiate(prefabs.Prefab(command.MapCode));
        currentMap.transform.localPosition += DefaultPos + command.Pos.ToVec3();
        command.EndProcess();
        
    }
    private void SetCameraPosScript(SetCameraPosScriptCommand command) {
        
        if (!command.Start())
            return;
 
        ShakeCamera.Instance.camera.transform
            .DOMove(DefaultPos + command.Pos.ToVec3(), command.Power)
            .OnComplete(() => command.EndProcess());
    }
    private void GeneratePersonScript(GeneratePersonScriptCommand command) {

        if (!command.Start())
            return;
        
        if (actors.ContainsKey(command.Code))
            throw new Exception($"This actor{command.Code} is already exist");
        
        var target = Instantiate(Resources.Load<GameObject>($"Actor/{command.Code}/Character"));
        target.transform.localPosition += DefaultPos + command.Pos.ToVec3();
        
        Animator animator = target.GetComponent<Animator>();
        SetAnimation(animator, command.View.ConvertVector(), false);

        actors.Add(command.Code, target);
        command.EndProcess();
    }
    private void SetPersonPosScript(SetPersonPosScriptCommand command) {
        if (!command.Start())
            return;
                
        if (!actors.TryGetValue(command.Target, out GameObject target))
            throw new Exception($"This actor{command.Target} isn't exist");

        target.transform.localPosition = PersonDefaultPos + DefaultPos + command.Pos.ToVec3();
        command.EndProcess();
    }
    private void MoveScript(MoveScriptCommand command) {

        const float DefaultPower = 0.5f;

        if (!command.Start())
            return;
            
        actors.TryGetValue(command.Target, out GameObject target);
        if (target is null)
            throw new NullReferenceException($@"This Actor{command.Target} isn't exist");

        Animator animator = target.GetComponent<Animator>();
        
        Sequence animation = DOTween.Sequence();
        Vector3 pos = target.gameObject.transform.localPosition;

        if (command.Follow) {
            ShakeCamera.Instance.camera.transform.localPosition = pos;
        }

        int loop = command.Loop;    
        if (loop == -1) {
            loop = Infinity;
            command.EndProcess();
        }

        float power = command.Power;
        if (power == 0)
            power = DefaultPower;
        for (int i = 0; i < loop; i++) {

            foreach (var dir in command.Route) {
                Vector2 direction = dir.ConvertVector();
                pos += direction.ToVec3();
                animation.Append(target.transform
                    .DOLocalMove(pos, power * dir.SimpleDirection())
                    .SetEase(Ease.Linear)
                    .OnStart(() => SetAnimation(animator, direction))
                );

                if (command.Follow) {
                    Vector3 cameraPos = pos;
                    cameraPos.z = DefaultCameraZPos;
                    animation.Join(ShakeCamera.Instance.camera.transform
                        .DOLocalMove(cameraPos, power * dir.SimpleDirection())
                        .SetEase(Ease.Linear)
                    );
                }
            }
        }

        animation.OnComplete(() => {
            command.EndProcess();
            animator.SetFloat("Speed", 0);
        });
    }
    private void FocusScript(FocusScriptCommand command) {
        
        if (!command.Start())
            return;
        if (!actors.TryGetValue(command.Target, out GameObject target))
            throw new Exception($"this actor({command.Target}) isn't exist");

        var targetPos = command.Pos.ToVec3() + target.transform.localPosition + new Vector3(0,0.7f,0);
        targetPos.z = DefaultCameraZPos;

        float power = command.Power;
        ShakeCamera.Instance.camera.transform
            .DOMove(targetPos, power)
            .OnComplete(() => command.EndProcess());
    }
    private void ZoomScript(ZoomScriptCommand command) {

        if (!command.Start())
            return;

        const float DefaultZoom = 5;
        
        ShakeCamera.Instance.camera
            .DOOrthoSize(command.Percent * DefaultZoom, command.Power)
            .OnComplete(() => command.EndProcess());
    }

    private void GetItemScript(GetItemScriptCommand command) {
        if (!command.Start())
            return;

        int count = command.Count;
        if (count == 0)
            count = 1;
            
        GetItemWindow.Instance.TurnOn(new GetItemInfo(command.Code, count));
        StartCoroutine(Wait.WaitAndDo(() => GetItemWindow.Instance.On, () => command.EndProcess()));
    }

    private void SetChapterScript(SetChapterScriptCommand command) {
        if (!command.Start())
            return;
        
        ChapterInfo.Set(command.Code);
        command.EndProcess();
    }

    private void AddRealPosScript(AddRealPosScriptCommand command) {

        if (!command.Start())
            return;

        var transform = TilePlayerPhysics.Instance.@Object.transform;
        transform
            .DOLocalMove(transform.localPosition + command.Pos.ToVec3(), command.Duraction)
            .OnComplete(() => command.EndProcess());
    }

    private void TutorialScript(TutorialScriptCommand command) {

        if (!command.Start())
            return;
        
        TutorialWindow.Instance.SetTutorial(new(){command});
        StartCoroutine(Wait.WaitAndDo(() => !TutorialWindow.Instance.On, () => command.EndProcess()));
    }

    private void ShowTutorialScript(ShowTutorialScriptCommand command) {
        if (!command.Start())
            return;

        var list = TutorialInfo.Interpret(command.Code);
        TutorialWindow.Instance.SetTutorial(list);
        StartCoroutine(Wait.WaitAndDo(() => !TutorialWindow.Instance.On, () => command.EndProcess()));
    }

    private void MeetMonsterScript(MeetMonsterScriptCommand command) {
        if (!command.Start())
            return;

        command.EndProcess();
        TilePlayerPhysics.Instance.MeetMonsterEvent(command.Code);
    }
    #endregion
    
    public void EndProcess() {
        foreach (var person in actors) {
            Destroy(person.Value);
        }

        actors.Clear();
        Destroy(currentMap);
        currentMap = null;
        background.enabled = false;
        upperBackground.enabled = false;
        
        ShakeCamera.Instance.camera.transform.localPosition = new Vector3(0, 0.7f, -10);
    }
    

    private void Awake() {
        Instance = this;
    }

    private void Update() {

        ScriptProcess();
    }
}