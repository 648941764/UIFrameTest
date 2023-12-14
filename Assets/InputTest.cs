using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTest : MonoBehaviour
{
    public float duration = 1.0f;
    public bool snapping = false;
    public Ease ease = Ease.Linear;

    public Text txt;
    public Transform t0, t1, t2, t3;

    private Tweener tweener;

    // Tweener, Sequence, DOVirtual

    private void Awake()
    {
        transform.GetComponent<MeshRenderer>().material.DOColor(Color.red, duration);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            tweener.Kill();
            transform.position = Vector3.zero;
        }

        DOTweenForTweener();
        DOTweenForSequence();
        DOTweenForDOVirtual();
    }

    private void DOTweenForDOVirtual()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DOVirtual.Float(0.0f, 10000.0f, 5.0f, _ => txt.text = _.ToString());
            Vector_3(Vector3.zero, new Vector3(20.0f, 9.0f, 0.0f), duration, _ => transform.position = _);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            DOVirtual.Float(0.0f, 2.0f, duration, _ =>
            {
                Vector3 p0 = Vector3.Lerp(t0.position, t1.position, _ / 2.0f);
                Vector3 p1 = Vector3.Lerp(t1.position, t2.position, _ / 2.0f);
                Vector3 p2 = Vector3.Lerp(t2.position, t3.position, _ / 2.0f);
                Vector3 p01 = Vector3.Lerp(p0, p1, _ / 2.0f);
                Vector3 p12 = Vector3.Lerp(p1, p2, _ / 2.0f);
                transform.position = Vector3.Lerp(p01, p12, _ / 2.0f);
            });
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            DOVirtual.Float(0.0f, 2.0f, duration, _ =>
            {
                Vector3 p0 = Vector3.Lerp(t0.position, t1.position, _ / 2.0f);
                Vector3 p1 = Vector3.Lerp(t1.position, t2.position, _ / 2.0f);
                transform.position = Vector3.Lerp(p0, p1, _ / 2.0f);
            });
        }
    }

    private void DOTweenForSequence()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            DOTween.Sequence()
                .Append(transform.DOMove(new Vector3(20.0f, 9.0f, 0.0f), duration).SetEase(ease))
                .Join(transform.GetComponent<MeshRenderer>().material.DOColor(Color.green, duration))
                .AppendInterval(0.5f)
                .Append(transform.DOMove(Vector3.zero, duration).SetEase(ease))
                .Join(transform.GetComponent<MeshRenderer>().material.DOColor(Color.white, duration))
                .AppendCallback(() => Debug.Log("Sequence Complete."));
        }
    }

    private void DOTweenForTweener()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DOTween.To(
                () => transform.position,
                _ => transform.position = _,
                new Vector3(25.0f, 0.0f, 0.0f), duration);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            tweener = transform
                .DOMoveX(25.0f, duration, snapping)
                .SetEase(ease)
                .OnStart(OnStart)
                .OnComplete(OnComplete)
                .OnUpdate(OnUpdate)
                .OnKill(OnKill);

            transform.GetComponent<MeshRenderer>().material.DOColor(Color.red, duration);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            tweener = transform.DOMoveX(-25.0f, duration, snapping).SetEase(ease)
                .SetEase(ease)
                .OnStart(OnStart)
                .OnComplete(OnComplete);
        }
    }

    private void OnStart()
    {
        Debug.Log("<color=#CF2C2C>OnStart</color>");
    }

    private void OnComplete()
    {
        Debug.Log("<color=#CF2C2C>OnComplete</color>");
    }

    private void OnKill()
    {
        Debug.Log("<color=#CF2C2C>OnKill</color>");
    }

    private void OnUpdate()
    {
        Debug.Log("OnUpdate");
    }

    public static Tweener Vector_3(Vector3 from, Vector3 to, float duration, TweenCallback<Vector3> onVirtualUpdate)
    {
        return DOTween.To(() => from, delegate (Vector3 x)
        {
            from = x;
        }, to, duration).OnUpdate(delegate
        {
            onVirtualUpdate(from);
        });
    }
}
