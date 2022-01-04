using UnityEngine;

/// <summary>
/// 플레이어와 적을 공격할 때 다형성을 이용하기 위한 인터페이스
/// </summary>
public interface ILivingEntity
{
    /// <summary>
    /// 공격 처리 함수
    /// </summary>
    /// <param name="damage">데미지</param>
    /// <param name="instigator">공격한 오브젝트</param>
    void TakeDamage(int damage, GameObject instigator);
}
