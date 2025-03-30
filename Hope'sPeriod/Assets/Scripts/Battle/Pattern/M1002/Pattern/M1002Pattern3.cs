using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class M1002Pattern3: EnemyPatternBase {
    public override bool Active { get; protected set; } = false;

    [SerializeField] private List<GameObject> fingers = new();
    [SerializeField] private List<GameObject> warnning = new();
    private const float fingerInterval = 2.6f;
    private const float startPosX = -3.75f;
    private const float startPosY = -7.5f;
    private const float endPosY = -3f;
    private const float warnningPosY = -2.45f;
    private const float objectWidth = 1.3f;
    
    private IEnumerator Pattern(int repeat, float warnningTime, float appearTime, float waitTime , float disappearTime) {
        Active = true;
        yield return new WaitForSeconds(0.5f);
        /*Player.Instance.Movement
            .SetApply<CompoGravity>(Direction.Down)
            .SetApply<CompoInput>(DirectionInfo.Horizontal);
        ShakeCamera.Instance.Shake(0.9f,0.35f).DOBeforeWait(0.43f);*/

        yield return new WaitForSeconds(0.5f);
        float warnningInterval = warnningTime / 6;
        for (int i = 0; i < repeat; i++) {

            float sum = 0;

            //set Position and warnning
            for (int j = 0; j < 4; j++) {
                sum = Random.Range(sum + objectWidth + 0.1f, j * fingerInterval);
                sum += (j * fingerInterval - sum) / 4;
                if(j > 2)
                    sum += (j * fingerInterval - sum) / 2;
                
                fingers[j].transform.localPosition = new(startPosX + sum, startPosY, -0.1f);

                warnning[j].transform.localPosition = new(startPosX + sum, warnningPosY, -0.1f);
                warnning[j].GetComponent<SpriteRenderer>()
                    .DOBlink(warnningInterval, warnningInterval, warnningInterval, 0.8f, 0)
                    .DORepeat(2);
            }

            yield return new WaitForSeconds(warnningTime + 0.25f);
            
            //move
            var animation = DOTween.Sequence();
            for (int j = 0; j < 4; j++) {
                int j1 = j;
                
                animation
                    .Join(
                        fingers[j].transform.DOMoveY(endPosY, appearTime)
                            .SetEase(Ease.OutBack)
                    )
                    .JoinCallback(() => {
                            fingers[j1].transform.DOMoveY(startPosY, disappearTime)
                                .SetEase(Ease.InSine)
                                .DOBeforeWait(waitTime);
                        }
                    );
            }

            yield return new WaitForSeconds(appearTime + waitTime + disappearTime + 0.1f);
        }

        Active = false;
    }
    
    public override void StartPattern() {
        MapSizeManager.Instance.Resize(new Vector2(9f, 6f));
        MapSizeManager.Instance.Move();
        StartCoroutine(Pattern(5, 1.1f, 0.4f, 0.7f, 0.5f));
    }

    public override void GrouptPattern() {
    }
}