using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosalAI : EnemyAI
{
    [Header("First Attack")]
    public float firstAttackDamage;
    public float firstAttackPrepareTime;
    public float firstAttackCooldown;
    public BosalHandAndFoot firstAttackProjectile;
    public float firstAttackYOffset;
    public int firstAttackNum;
    public float firstAttackDelay;
    public float firstAttackSpawnRange;
    [Header("Second Attack")]
    public float secondAttackDamage;
    public float secondAttackPrepareTime;
    public float secondAttackCooldown;
    public BosalHorizontalHand secondAttackProjectile;
    public float secondAttackYOffset;
    [Header("Third Attack")]
    public float thirdAttackDamage;
    public float thirdAttackPrepareTime;
    public float thirdAttackCooldown;
    public BosalFood thirdAttackProjectile;
    public float thirdAttackYOffset;
    public int thirdAttackNum;
    public float thirdAttackDelay;
    public float thirdAttackSpawnRange;
    public float thirdAttackDuration;

    public void FirstAttack()
    {
        StartCoroutine("FirstAttackCo");
    }

    private IEnumerator FirstAttackCo()
    {
        yield return new WaitForSeconds(firstAttackPrepareTime);

        for (int i = 0; i < firstAttackNum; i++)
        {
            SpawnHandAndFoot();

            yield return new WaitForSeconds(firstAttackDelay);
        }

        IsAttacking = false;
    }

    private void SpawnHandAndFoot()
    {
        float x = Random.Range(Target.Position.x - firstAttackSpawnRange, Target.Position.x + firstAttackSpawnRange);
        BosalHandAndFoot clone = Instantiate(firstAttackProjectile, new Vector2(x, firstAttackYOffset), Quaternion.identity);
        clone.Setup((int)firstAttackDamage, gameObject);
    }

    public void SecondAttack()
    {
        BosalHorizontalHand clone = Instantiate(secondAttackProjectile, new Vector2(Target.Position.x, secondAttackYOffset), Quaternion.identity);
        clone.Setup((int)secondAttackDamage, gameObject, Target.transform);

        IsAttacking = false;
    }

    public void ThirdAttack()
    {
        StartCoroutine("ThirdAttackCo");
    }

    private IEnumerator ThirdAttackCo()
    {
        float current = 0;
        while (current < thirdAttackDuration)
        {
            current += thirdAttackDelay;
            SpawnFood();
            yield return new WaitForSeconds(thirdAttackDelay);
        }

        IsAttacking = false;
    }

    private void SpawnFood()
    {
        float x = Random.Range(Target.Position.x - thirdAttackSpawnRange, Target.Position.x + thirdAttackSpawnRange);
        BosalFood clone = Instantiate(thirdAttackProjectile, new Vector2(x, thirdAttackYOffset), Quaternion.identity);
        clone.Setup((int)thirdAttackDamage, gameObject);
    }
}
