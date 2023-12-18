using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;

public class MyCar
{
    public string Color;
    public string Name;
    public int Year;
}

public class LearnDoTween : MonoBehaviour
{
    public Transform tree, topBall, middleBall, bottom;
    public Ease ease;

    List<MyCar> list = new List<MyCar>()
    {
        new MyCar(){Color = "White", Name = "BMW", Year = 2001},
        new MyCar(){Color = "Purple", Name = "Ford", Year = 1998},
        new MyCar(){Color = "Black", Name = "Toyota", Year = 2000},
    };

    private void Start()
    {
        Learn();
        //查找
        //var cars = from car in list
        //           where car.Year == 2001 && car.Name == "Ford"
        //           select car;

        //var cars = list.Where(p => p.Year == 2000);
        //Debug.Log(cars.Count());

        // 排序 加查找第一个指定的东西
        //var cars = from car in list
        //           orderby car.Year descending
        //           select car;

        var cars = list.OrderByDescending(car => car.Year);

        //var mycar = list.First(car => car.Year == 2000);
        //Debug.Log(mycar.Name);

        var mycar = list.OrderByDescending(car => car.Year).First(car => car.Name == "Toyota");//排序加查找
        Debug.Log(mycar.Year);
        Debug.Log(list.TrueForAll(p => p.Year >= 1998));

        //list.ForEach(car => Debug.Log(car.Name));
        //list.ForEach(car => car.Year -= 1500);
        //list.ForEach(car => Debug.Log(car.Year)); 

        Debug.Log(list.Exists(car => car.Name == "Toyota"));//查看是否有一个符合的，返回一个bool值
        Debug.Log(list.Sum(car => car.Year));//求和
        
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
