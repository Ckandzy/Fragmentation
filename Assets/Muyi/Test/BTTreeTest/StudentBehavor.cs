using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MuyiBTTree;

public class StudentBehavor : MonoBehaviour {
    public Transform Desk;
    public Transform Chifan;
    public Transform Chufang;
    public Vector2 movement;
    
    public BTTree tree = new BTTree();
    public Transform target;

    public float nowfood = 2;
    public float AllEatFood = 5;

    private void Start()
    {
        tree.Root().OpenBranch(
            new SequenceBTNode().OpenBranch(
                new SelectBTNode().OpenBranch(new ConditionBTNode(CanCook), new Action(this, AddFood)),
                new Action(this, EatFood),
                new SelectBTNode().OpenBranch(new ConditionBTNode(CanRun), new Action(this, Run))
                //new SelectBTNode().OpenBranch(new ConditionBTNode(CanEat), new Action(this, EatFood))
                
            ));
    }

    private void Update()
    {
        //if (target != null) Move(target);
        tree.Root().Tick();
        //nowfood -= 1f * Time.deltaTime;
    }
    public bool CanEat()
    {
        return AllEatFood <= 2;
    }

    public bool CanRun()
    {
        return nowfood <= 4;
    }

    public bool CanCook()
    {
        return AllEatFood >= 4;
    }
    public bool Move(Transform _target)
    {
        if (_target == null) return true;
        transform.position += (_target.position - transform.position).normalized * 3 * Time.deltaTime;
        target = _target;
        if((transform.position - _target.position).magnitude < 0.5f)
        {
            target = null;
            return true;
        }
        return false;
    }
    
    public bool AddFood()
    {
        bool over = Move(Chufang);
        if (over)
        {
            AllEatFood += 10;
        }
            
        return over;
    }
    public bool EatFood()
    {
        bool over = Move(Chifan);
        if (over)
        {
            AllEatFood -= 2;
            nowfood += 2;
        }
           
        return over;
    }

    public bool Run()
    {
        bool over = Move(Desk);
        if (over)
        {
            nowfood -= 4;
        } 
        return over;
    }
    private class MoveNode : ActionBTNode<StudentBehavor>
    {
        public MoveNode(StudentBehavor t) : base(t) { }

        public override BTStatus Tick()
        {
            bool enterTarget = TClass.Move(TClass.target);
            if (enterTarget)
            {
                return BTStatus.Success;
            }
            else if(TClass.target != null && !enterTarget)
            {
                return BTStatus.Running;
            }
            else 
            {
                return BTStatus.Fail;
            }
            
            throw new System.Exception("no node, but try get it");
        }
    }

    private class ZhufanNode : ActionBTNode<StudentBehavor>
    {
        public ZhufanNode(StudentBehavor t) : base(t) { }

        public override BTStatus Tick()
        {
            bool addover = TClass.AddFood();
            if (addover)
            {
                return BTStatus.Success;
            }
            else if (TClass.target != null && !addover)
            {
                return BTStatus.Running;
            }
            else
            {
                return BTStatus.Fail;
            }
            throw new System.Exception("no node, but try get it");
        }
    }

    class Action : ActionBTNode<StudentBehavor>
    {
       // System.Func<bool> func;
        public Action(StudentBehavor t, System.Func<bool> fn) : base(t) { func = fn; }

        public override BTStatus Tick()
        {
            bool addover = func();
            if (addover)
            {
                return BTStatus.Success;
            }
            else if (TClass.target != null && !addover)
            {
                return BTStatus.Running;
            }
            else
            {
                return BTStatus.Fail;
            }
            throw new System.Exception("no node, but try get it");
        }
    }
}


