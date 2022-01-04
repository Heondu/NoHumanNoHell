using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ComboNode
{
    [SerializeField]
    private AttackType attackType;
    [SerializeField]
    private float damage;
    [SerializeField]
    private ComboNode[] nextNodes;

    public AttackType AttackType => attackType;
    public float Damage => damage;
    public IReadOnlyList<ComboNode> NextNodes => nextNodes;

    public ComboNode FindNextNode(AttackType attackType) => nextNodes.FirstOrDefault(x => x.AttackType == attackType);
    public bool HasNextNode => nextNodes.Length != 0;
}

[CreateAssetMenu]
public class ComboTree : ScriptableObject
{
    [SerializeField]
    private ComboNode[] tree;

    public ComboNode FindFirstNode(AttackType attackType) => tree.FirstOrDefault(x => x.AttackType == attackType);
}
