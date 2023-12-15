using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LearnDoTween : MonoBehaviour
{
    public Transform tree, topBall, middleBall, bottom;
    public Ease ease;

    private void Start()
    {
        Learn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DOTween.To(
                () => tree.position,
                _ => tree.position = _,
                Vector3.zero,2f);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DOTween.To(
                () => tree.position,
                _ => tree.position = _,
               new Vector3(9.5f, 0, 0), 2f);
        }
    }

    public void Learn()
    {
        DOTween.Sequence()
            .Append(tree.DOMoveX(10f, 2f)).SetEase(Ease.Linear).SetLoops( - 1, LoopType.Yoyo)
            .Join(topBall.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).SetLoops(10000, LoopType.Restart).SetEase(Ease.Linear))
            .Join(middleBall.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360).SetLoops(10000, LoopType.Restart).SetEase(Ease.Linear))
            .Join(bottom.DORotate(new Vector3(0f, 360f, 0f), 1f, RotateMode.FastBeyond360).SetLoops(10000, LoopType.Restart).SetEase(Ease.Linear))
        ;
    }
}
