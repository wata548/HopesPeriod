using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.UI;

public class ScriptCodePlayer: MonoBehaviour {

    [SerializeField] private Image background;
    
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
        var list = ScriptCodeInterpreter.Interpret(input);
            
        //Set Next
        list[0].Item2.SetUsable(true);
        for (int i = 0; i < list.Count - 1; i++) {
            list[i].Item2.SetNext(list[i + 1].Item2);
        }
        process.AddRange(list);
    }
    private void ScriptProcess() {
        foreach (var command in process) {
            ClasifyScript(command.Item1, command.Item2);
        }
        process = process.Where(command => !command.Item2.End()).ToList();
    }

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

        background.enabled = true;
        background.sprite = Resources.Load<Sprite>($"Background/{command.Image}");
        command.EndProcess();
    }
    private void ClearBackgroundScript(ClearBackgroundScriptCommand command) {
        if (!command.Start())
            return;

        background.sprite = null;
        background.transform.localScale = new(1, 1);
        background.transform.localPosition = new(0,0);
        background.enabled = false;
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

        Debug.Log("map");
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
            .DOLocalMove(DefaultPos + command.Pos.ToVec3(), command.Power)
            .OnComplete(() => command.EndProcess());
    }
    private void GeneratePersonScript(GeneratePersonScriptCommand command) {

        if (!command.Start())
            return;
        
        if (actors.ContainsKey(command.Code))
            throw new Exception($"This actor{command.Code} is already exist");
        
        var target = Instantiate(Resources.Load<GameObject>($"Actor/{command.Code}"));
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

        var targetPos = command.Pos.ToVec3() + target.transform.localPosition;
        targetPos.z = DefaultCameraZPos;

        float power = command.Power;
        ShakeCamera.Instance.camera.transform
            .DOLocalMove(targetPos, power)
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
    #endregion
    
    private void SamplePlayer() {
        if (Input.GetKeyDown(KeyCode.R)) {
                    
            string input = @"StartChangeEffect(Power = .3f);";
            input += @"SetMap(MapCode = 8402);";
            input += @"GeneratePerson(Code = 5002 | Pos = {1f , 1f} | View = l + u);"; 
            input += @"Focus(Target = 5002);";
            /*input += @"ClearEffect(Power = 0.3f);";
            input += @"Zoom(Percent = 0.7f| Power = 0.2f);";
            input += @"Move(Target = 5002|Route=[u]|Loop = 8 | Follow = true);";
            input += @"Move(Target = 5002| Route = [u,l,d,r] | Loop = 10 | Power = 0.1f | Follow = true);";
            input += @"Zoom(Percent = 1f | Power = 1f);";
            input += @"StartChangeEffect(Power = .3f);";
            input += @"SetMap(MapCode = 8401);";
            input += @"SetPersonPos(Target = 5002 | Pos = {0f, 0f});";
            input += @"Focus(Target = 5002);";*/
            input += @"ClearEffect(Power = 0.3f);";
            input += @"Move(Target = 5002|Route=[r]|Loop = 10 | Follow = true);";
            input += @"SetBackground(Image = ""human"");";
            input += @"Wait(Power = .5f);";
            input += @"ClearBackground();";
            input += @"Wait(Power = .5f);";
            input += @"SetBackground(Image = ""human"");";
            Interpret(input);
        }
    }
    
    private void Update() {

        SamplePlayer();
        ScriptProcess();
    }

      
}