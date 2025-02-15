using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ActorRepository: MonoBehaviour {
     
    private Dictionary<int, GameObject> actors = new();

    public void GeneratePersonScript(GeneratePersonScriptCommand command) {

        if (actors.ContainsKey(command.ActorCode))
            throw new Exception($"This actor{command.ActorCode} is already exist");
        
        var target = Instantiate(Resources.Load<GameObject>($"Actor/{command.ActorCode}"));
        target.transform.localPosition = target.transform.localPosition + command.Pos.ToVec3();
        
        Animator animator = target.GetComponent<Animator>();
        SetAnimation(animator, command.View.ConvertVector(), false);

        actors.Add(command.ActorCode, target);
    }
    
    private void Start() {

        Debug.Log("start");
        GeneratePersonScript(ScriptCode.Interpret(@"GeneratePerson(ActorCode = 5001 | Pos = {1f , 1f} | View = l + u);")[0].Item2 as GeneratePersonScriptCommand);
        MoveScript(ScriptCode.Interpret(@"Move(Target = 5001| Route = [r] | Loop = 100 | Power = 0.5f | Follow = true);")[0].Item2 as MoveScriptCommand);
    }
     
    public void MoveScript(MoveScriptCommand command) {

        actors.TryGetValue(command.Target, out GameObject target);
        if (target == null)
            throw new NullReferenceException($@"This Actor{command.Target} isn't exist");
        
        Animator animator = target.GetComponent<Animator>();
        
        Sequence animation = DOTween.Sequence();
        Vector3 pos = target.gameObject.transform.localPosition;

        if (command.Follow) {
            ShakeCamera.Instance.camera.transform.localPosition = pos;
        }

        int loop = command.Loop;    
        if (loop == -1) {
            loop = 100;
            //end process
        }

        for (int i = 0; i < loop; i++) {

            foreach (var dir in command.Route) {
                Vector2 direction = dir.ConvertVector();
                pos += direction.ToVec3();
                animation.Append(target.transform
                    .DOLocalMove(pos, command.Power * dir.SimpleDirection())
                    .SetEase(Ease.Linear)
                    .OnStart(() => SetAnimation(animator, direction))
                );

                if (command.Follow) {
                    Vector3 cameraPos = pos;
                    cameraPos.z = -10;
                    animation.Join(ShakeCamera.Instance.camera.transform
                        .DOLocalMove(cameraPos, command.Power * dir.SimpleDirection())
                        .SetEase(Ease.Linear)
                    );
                }
            }
        }

        //TODO: End Process
        animation.OnComplete(() => {
            Debug.Log("end");
            animator.SetFloat("Speed", 0);
        });
    }

    private void SetAnimation(Animator animator, Vector2 value, bool applySpeed = true) {
        animator.SetFloat("Horizontal", value.x);
        animator.SetFloat("Vertical", value.y);
        if(applySpeed)
            animator.SetFloat("Speed", value.magnitude);
    }
}