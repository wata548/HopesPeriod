using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor.Searcher;
using UnityEngine;

public class ActorRepository: MonoBehaviour {

    private const int Infinity = 100;
    private const int DefaultCameraZPos = -10;
    private Dictionary<int, GameObject> actors = new();
    private List<(ScriptCodeKeyword, CommandBase)> process = new();
    
    private void SetAnimation(Animator animator, Vector2 value, bool applySpeed = true) {
        animator.SetFloat("Horizontal", value.x);
        animator.SetFloat("Vertical", value.y);
        if(applySpeed)
            animator.SetFloat("Speed", value.magnitude);
    }
    public void Interpret(string input) {
            var list = ScriptCode.Interpret(input);
            
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
    private void ClasifyScript(ScriptCodeKeyword type, CommandBase command) {
        switch (type) {
                
            case ScriptCodeKeyword.Move:
                MoveScript(command as MoveScriptCommand);
                break;
            case ScriptCodeKeyword.GeneratePerson:
                GeneratePersonScript(command as GeneratePersonScriptCommand);
                break;
            case ScriptCodeKeyword.Focus:
                FocusScript(command as FocusScriptCommand);
                break;
        }
    }
    private void GeneratePersonScript(GeneratePersonScriptCommand command) {

        if (!command.Start())
            return;
        
        if (actors.ContainsKey(command.ActorCode))
            throw new Exception($"This actor{command.ActorCode} is already exist");
        
        var target = Instantiate(Resources.Load<GameObject>($"Actor/{command.ActorCode}"));
        target.transform.localPosition = target.transform.localPosition + command.Pos.ToVec3();
        
        Animator animator = target.GetComponent<Animator>();
        SetAnimation(animator, command.View.ConvertVector(), false);

        actors.Add(command.ActorCode, target);
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
        const float DefaultPower = 0.7f;
        
        if (!command.Start())
            return;
        if (!actors.TryGetValue(command.Target, out GameObject target))
            throw new Exception($"this actor({command.Target}) isn't exist");

        var targetPos = command.Pos.ToVec3() + target.transform.localPosition;
        targetPos.z = DefaultCameraZPos;

        float power = command.Power;
        if(power == 0) 
            power = DefaultPower;
        ShakeCamera.Instance.camera.transform
            .DOLocalMove(targetPos, power)
            .OnComplete(() => command.EndProcess());
    }
    
    private void Update() {

        if (Input.GetKeyDown(KeyCode.R)) {
            
            string input = @"GeneratePerson(ActorCode = 5001 | Pos = {1f , 1f} | View = l + u);\n\rFocus(Target = 5001);\n\rMove(Target = 5001|Route=[u,d]|Loop = 3);\n\rMove(Target = 5001| Route = [r] | Loop = 5 | Power = 0.5f | Follow = true);";
            Interpret(input);
        }
        
        ScriptProcess();
    }

      
}