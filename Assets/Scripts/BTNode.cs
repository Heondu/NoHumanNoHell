using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public class BTNode
    {
        public bool reverse;

        public virtual bool Invoke()
        {
            return false;
        }
    }

    public class CompositNode : BTNode
    {
        [SerializeField] private List<BTNode> children = new List<BTNode>();

        public void AddChild(BTNode node)
        {
            children.Add(node);
        }

        public List<BTNode> GetChildren()
        {
            return children;
        }
    }

    public class Selector : CompositNode
    {
        public override bool Invoke()
        {
            foreach (BTNode node in GetChildren())
            {
                if (node.Invoke())
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Sequence : CompositNode
    {
        public override bool Invoke()
        {
            foreach (BTNode node in GetChildren())
            {
                if (!node.Invoke())
                {
                    return false;
                }
            }
            return true;
        }
    }
}