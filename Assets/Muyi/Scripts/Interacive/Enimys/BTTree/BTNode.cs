using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MuyiBTTree
{
    public enum BTStatus
    {
        Fail,
        Running,
        Success,
        Exit
    }

    public class BTTree
    {
        protected Root root;
        public Root Root() { if (root == null){ return root = new Root(); } else{ return root; } } 
    }

    public abstract class BTNode
    {
        public abstract BTStatus Tick();
    }

    public class Root : Branch
    {
        public bool isTerminated = false;

        public override BTStatus Tick()
        {
            if (isTerminated) return BTStatus.Exit;
            // 根节点的tick要一直循环
            while (true)
            {
                switch (ChildrenNodes[NowChild].Tick())
                {
                    case BTStatus.Running:
                        return BTStatus.Running;
                    case BTStatus.Exit:
                        isTerminated = true;
                        return BTStatus.Exit;
                    default:
                        NowChild++;
                        if (NowChild == ChildrenNodes.Count)
                        {
                            NowChild = 0;
                            return BTStatus.Success;
                        }
                        continue;
                }
            }
        }
    }

    // 行为节点(叶节点)
    public abstract class ActionBTNode<T> : BTNode
    {
        protected T TClass;
        public System.Func<bool> func;
        public ActionBTNode(T t)
        {
            TClass = t;
        }
    }

    public class Action : BTNode
    {
        System.Action act;
        public Action(System.Action fn)
        {
            act = fn;
        }
        public override BTStatus Tick()
        {
            if(act != null)
            {
                act();
                return BTStatus.Success;
            }

            return BTStatus.Fail;
        }
    }

    // 条件节点（叶节点）
    public class ConditionBTNode : BTNode
    {
        public System.Func<bool> func;
        bool tested = false;
        public ConditionBTNode(System.Func<bool> fn)
        {
            this.func = fn;
        }
        public override BTStatus Tick()
        {
            tested = func();
            if (tested) return BTStatus.Success;
            else return BTStatus.Fail;
        }
    }


    // 分支节点(组合节点)
    public abstract class Branch : BTNode
    {
        protected int NowChild = 0;
        protected List<BTNode> ChildrenNodes = new List<BTNode>();
        public virtual Branch OpenBranch(params BTNode[] nodes)
        {
            foreach (BTNode node in nodes)
                ChildrenNodes.Add(node);
            return this;
        }
        public int GetNowChild()
        {
            return NowChild;
        }
        public List<BTNode> GetChildrenNodes()
        {
            return ChildrenNodes;
        }
        // 重置节点
    }
    // 顺序节点(Sequence),它从左向右依次执行所有节点，只要节点返回Success，
    // 就继续执行后续节点，当一个节点返回Fail或 Running 时，停止执行后续节点。
    // 向父节点返回 Fail 或 Running，只有当所有节点都返回 Success 时，才向父节点返回 Success。 
    // 与选择节点相似，当节点返回Running 时，顺序节点除了终止后续节点的执行，
    // 还要记录返回 Running的这个节点，下次迭代会直接从该节点开始执行。 
    // 全部按顺序遍历完  优先级从左到右
    // 如吃饭时 ： 走到餐桌  坐下  吃饭   三个行为顺序执行，且都要必须执行
    public class SequenceBTNode : Branch
    {
        public override BTStatus Tick()
        {
            // 迭代所有字节点
            BTStatus status = ChildrenNodes[NowChild].Tick();
            switch (status)
            {
                case BTStatus.Fail:
                    NowChild = 0; // 重置
                    return BTStatus.Fail;
                case BTStatus.Running:
                    return BTStatus.Running;
                case BTStatus.Success:
                    NowChild++;
                    if (NowChild == ChildrenNodes.Count)
                    {
                        NowChild = 0;
                        return BTStatus.Success;
                    }
                    return BTStatus.Running;
                case BTStatus.Exit:
                    NowChild = 0;
                    return BTStatus.Exit;
            }
            throw new System.Exception("no node, but try to get it");
        }
    }

    // 选择节点(Select)，遍历方式为从左到右依次执行所有子节点，只要节点返回 Fail，
    // 就继续执行后续节点，直到一个节点返回Success或Running为止，停止执行后续节点。
    // 如果有一个节点返回Success或Running则向父节点返回Success或Running。否则向父节点返回 Fail。
    // 选择一个节点 优先级从左到右
    // 例如： 饿了吃东西， 节点有面包(无库存)，水果（有库存），米饭（有库存）等， 选到水果后，直接返回
    public class SelectBTNode : Branch
    {
        public override BTStatus Tick()
        {
            BTStatus status = ChildrenNodes[NowChild].Tick();
            switch (status)
            {
                case BTStatus.Fail:
                    NowChild++;
                    if (NowChild == ChildrenNodes.Count)
                    {
                        NowChild = 0;
                    }
                    return BTStatus.Fail;
                case BTStatus.Running:
                    return BTStatus.Running;
                case BTStatus.Success:
                    NowChild = 0;
                    return BTStatus.Success;
                case BTStatus.Exit:
                    NowChild = 0;
                    return BTStatus.Exit;
            }
            throw new System.Exception("no node, but try get it");
        }
    }

    // 随机节点(Random)遍历优先级与选择节点、顺序节点不同。 
    // 选择节点、顺序节点都是默认优先级的，最左边的节点具有最高优先级，最右边的优先级最低。
    // 随机节点则是随机执行每个子节点。如从左到右顺序为：A、B、C、D。而执行时可能是D、A、C、B 或 C、B、D、A 等。 
    // 当一个节点返回 Success 或者 Running 时，则停止执行后续节点，向父节点返回 Success或Running。
    // 当返回 Running 时记录返回 Running 的节点，下次迭代时首先执行 Running 节点。
    // 随机一个节点
    // 例如，根据AI心情不同每天随机选择吃 宫保鸡丁、香菇肉片、还是鱼香肉丝。这样提高游戏结果的多样性。
    public class RandomBTNode : Branch
    {
        // 洗牌算法
        public void RandomNodes()
        {
            int n = 0;
            while (n++ < ChildrenNodes.Count)
            {
                BTNode value = ChildrenNodes[n];
                int k = Mathf.FloorToInt(Random.value * ChildrenNodes.Count);
                ChildrenNodes[n] = ChildrenNodes[k];
                ChildrenNodes[k] = value;
            }
        }

        public override BTStatus Tick()
        {
            RandomNodes();
            BTStatus status = ChildrenNodes[NowChild].Tick();
            switch (status)
            {
                case BTStatus.Fail:
                    NowChild++;
                    if (NowChild >= ChildrenNodes.Count)
                    {
                        NowChild = 0;
                    }
                    return BTStatus.Fail;
                case BTStatus.Running:
                    return BTStatus.Running;
                case BTStatus.Success:
                    NowChild = 0;
                    return BTStatus.Success;
                case BTStatus.Exit:
                    NowChild = 0;
                    return BTStatus.Exit;
            }

            throw new System.NotImplementedException();
        }
    }


    // 修饰节点(Decorator)只包含一个节点，用户以某种方式改变这个节点的行为。 
    // 修饰节点有很多种，其中有一些是用于决定是否允许子节点运行的，也叫过滤器，例如 Until Success, Until Fail 等，首先确定需要的结果，循环执行子节点，直到节点返回的结果和需要的结果相同时向父节点返回需要的结果，否则返回 Running。 
    // 如需要结果为 Until Fail，则当子节点返回 Success或者 Running 时都向父节点返回 Running，当节点返回结果为 Fail 时，才向父节点返回 Fail。 
    // 反之需要的结果为 Until Success，则当子节点返回 Fail 或者 Running 时向父节点返回 Running，当节点返回 Success 时，才向父节点返回 Success。 
    // 只有一个节点
    public class DecoratorBTNode : BTNode
    {
        public BTStatus DecoratorStatus, FalseStatus;
        public BTNode ChildNode;
        public BTNode OpenBranch(BTNode _node, BTStatus _decorator, BTStatus falseStatus)
        {
            ChildNode = _node;
            DecoratorStatus = _decorator;
            FalseStatus = falseStatus;
            return this;
        } 

        public override BTStatus Tick()
        {
            BTStatus status = ChildNode.Tick();
            // 子节点匹配装饰节点
            if (status == DecoratorStatus) return DecoratorStatus;
            // 不匹配装饰节点
            else return FalseStatus;
        }
    }

    /// 并行节点(Parallel)有 N 个节点，每次执行所有节点，直到一个节点返回 Fail 或者全部返回 Success为止，此时并行节点向父节点返回 Fail 或者 Success，并终止执行其他所有节点。否则至少有一个节点处于 Running 状态，则执行完所有节点向父节点返回 Running。与选择节点不同的是并行节点不需要记录返回 Running 结果的节点，每次执行都会从左向右依次执行所有子节点。 
    // 当外界环境发生变化时，影响到子节点的执行，如何处理？如，AI 正在炒菜，突然煤气不足，无法点燃，则AI 就不能继续炒菜这个动作了，将退出炒菜的所有节点。 
    // 并行节点对于外部环境发生变化，需要随时应对变化的AI 决策十分有效。如，AI 追逐玩家，使用并行节点就特别合适，需要并行的执行 “是否看到玩家？”，“朝玩家移动”。当某一帧无法看到玩家，则不能执行朝向玩家移动这个节点的行为。 
    public class ParllelBTNode : Branch
    {
        public override BTStatus Tick()
        {
            BTStatus status = ChildrenNodes[NowChild].Tick();
            switch (status)
            {
                case BTStatus.Fail:
                    NowChild = 0;
                    return BTStatus.Fail;
                case BTStatus.Running:
                    return BTStatus.Running;
                case BTStatus.Success:
                    NowChild++;
                    if(NowChild == ChildrenNodes.Count)
                    {
                        NowChild = 0;
                        return BTStatus.Success;
                    }
                    return BTStatus.Running;
                case BTStatus.Exit:
                    NowChild = 0;
                    return BTStatus.Exit;
            }
            throw new System.Exception("no node, but try get it");
        }
    }
}
