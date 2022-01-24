using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosalAI : EnemyAI
{
    [Header("First Attack")]
    public float firstAttackDamage;
    public float firstAttackPrepareTime;
    public float firstAttackCooldown;
    public BosalFirstAttackProjectile[] firstAttackProjectiles;
    public float firstAttackYOffset;
    public int firstAttackNum;
    public float firstAttackDelay;
    public float firstAttackSpawnRange;
    [Header("Second Attack")]
    public float secondAttackDamage;
    public float secondAttackPrepareTime;
    public float secondAttackCooldown;
    public BosalSecondAttackProjectile secondAttackProjectile;
    public float secondAttackYOffset;
    [Header("Third Attack")]
    public float thirdAttackDamage;
    public float thirdAttackPrepareTime;
    public float thirdAttackCooldown;
    public BosalThirdAttackProjectile[] thirdAttackProjectiles;
    public float thirdAttackYOffset;
    public int thirdAttackNum;
    public float thirdAttackSpawnRange;
    public float thirdAttackDuration;

    protected override void Start()
    {
        base.Start();
        StartCoroutine("StartAttack");
    }

    private IEnumerator StartAttack()
    {
        IsAttacking = true;

        yield return new WaitForSeconds(1f);

        IsAttacking = false;
    }

    public void FirstAttack()
    {
        IsAttacking = true;
        entity.SetAttackTimer("FirstAttack", firstAttackCooldown);
        animator.Play("FirstAttack");
    }

    private IEnumerator FirstAttackCo()
    {
        yield return new WaitForSeconds(firstAttackPrepareTime);

        for (int i = 0; i < firstAttackNum; i++)
        {
            SpawnHandAndFoot();

            yield return new WaitForSeconds(firstAttackDelay);
        }

        yield return new WaitForSeconds(2);

        IsAttacking = false;
    }

    private void SpawnHandAndFoot()
    {
        float x = Random.Range(Target.Position.x - firstAttackSpawnRange, Target.Position.x + firstAttackSpawnRange);
        BosalFirstAttackProjectile spawnObject = firstAttackProjectiles[Random.Range(0, firstAttackProjectiles.Length)];
        BosalFirstAttackProjectile clone = Instantiate(spawnObject, new Vector2(x, firstAttackYOffset), Quaternion.identity);
        clone.Setup((int)firstAttackDamage, gameObject);
    }

    public void SecondAttack()
    {
        IsAttacking = true;
        entity.SetAttackTimer("SecondAttack", secondAttackCooldown);
        animator.Play("SecondAttack");
    }

    private IEnumerator SecondAttackCo()
    {
        BosalSecondAttackProjectile clone = Instantiate(secondAttackProjectile, new Vector2(Target.Position.x, secondAttackYOffset), Quaternion.identity);
        clone.Setup((int)secondAttackDamage, gameObject, Target.transform);

        yield return new WaitForSeconds(3);

        IsAttacking = false;
    }

    public void ThirdAttack()
    {
        IsAttacking = true;
        entity.SetAttackTimer("ThirdAttack", thirdAttackCooldown);
        animator.Play("ThirdAttack");
    }

    private IEnumerator ThirdAttackCo()
    {
        float current = 0;
        while (current < thirdAttackDuration)
        {
            float delay = thirdAttackDuration / thirdAttackNum;
            current += delay;
            SpawnFood();
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(2);

        IsAttacking = false;
    }

    private void SpawnFood()
    {
        float x = Random.Range(Target.Position.x - thirdAttackSpawnRange, Target.Position.x + thirdAttackSpawnRange);
        BosalThirdAttackProjectile spawnObject = thirdAttackProjectiles[Random.Range(0, thirdAttackProjectiles.Length)];
        BosalThirdAttackProjectile clone = Instantiate(spawnObject, new Vector2(x, thirdAttackYOffset), Quaternion.identity);
        clone.Setup((int)thirdAttackDamage, gameObject);
    }

    public override void OnDead()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        target.GetComponent<Entity>().SetCanBeDamaged(false);
    }
}
