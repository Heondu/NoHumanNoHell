using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsuraAI : EnemyAI
{
    [Header("First Attack")]
    public int firstAttackDamage;
    public float firstAttackCooldown;
    public int firstAttackColumnNum;
    public int firstAttackRowNum;
    public float firstAttackDelay;
    public AsuraFirstAttackProjectile firstAttackProjectile;
    public AudioClip[] firstAttackClips;
    [Header("Column")]
    public float firstAttackYOffset;
    public float firstAttackSpawnRangeX;
    [Header("Row")]
    public float firstAttackXOffset;
    public float firstAttackSpawnRangeY;
    [Header("Second Attack")]
    public EnemyAI[] bosses;
    public float secondAttackPrepareTime;
    public float secondAttackCooldown;
    [Header("Third Attack")]
    public int thirdAttackDamage;
    public float thirdAttackPrepareTime;
    public float thirdAttackCooldown;
    public Vector2 thirdAttackOffset;
    public AsuraThirdAttackProjectile thirdAttackProjectile;
    public AudioClip thirdAttackClip;

    private EnemyAI spawnedEnemy;

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
        StartCoroutine("FirstAttackCo");
    }

    private IEnumerator FirstAttackCo()
    {
        int columnCount = 0;
        int rowCount = 0;
        while (columnCount + rowCount < firstAttackColumnNum + firstAttackRowNum)
        {
            if (columnCount < firstAttackColumnNum && Random.Range(0, 2) == 0)
            {
                columnCount++;
                float x = Random.Range(target.Position.x - firstAttackSpawnRangeX, target.Position.x + firstAttackSpawnRangeX);
                Projectile clone = Instantiate(firstAttackProjectile, new Vector2(x, firstAttackYOffset), Quaternion.Euler(Vector3.zero));
                clone.Setup(Vector2.down, firstAttackDamage, gameObject);
            }
            else
            {
                rowCount++;
                float y = transform.position.y + 0.5f + Random.Range(0, firstAttackSpawnRangeY);
                if (Random.Range(0, 2) == 0)
                {
                    Projectile clone = Instantiate(firstAttackProjectile, new Vector2(-firstAttackXOffset, y), Quaternion.Euler(new Vector3(0, 0, 90)));
                    clone.Setup(Vector2.right, firstAttackDamage, gameObject);
                }
                else
                {
                    Projectile clone = Instantiate(firstAttackProjectile, new Vector2(firstAttackXOffset, y), Quaternion.Euler(new Vector3(0, 0, -90)));
                    clone.Setup(Vector2.left, firstAttackDamage, gameObject);
                }
            }
            SoundManager.PlaySFX(firstAttackClips[Random.Range(0, firstAttackClips.Length)]);

            yield return new WaitForSeconds(firstAttackDelay);
        }

        yield return new WaitForSeconds(firstAttackCooldown);

        IsAttacking = false;
    }

    public void SecondAttack()
    {
        IsAttacking = true;
        entity.SetAttackTimer("SecondAttack", secondAttackCooldown);

        int index = Random.Range(0, 2);
        spawnedEnemy = Instantiate(bosses[index], transform.position, Quaternion.identity);
        spawnedEnemy.Entity.CanBeDamaged = false;
        spawnedEnemy.GetComponent<SpriteRenderer>().color = Color.black;
        spawnedEnemy.onAttackEnd.AddListener(DestroyObject);
    }

    private void DestroyObject()
    {
        Destroy(spawnedEnemy.gameObject);
        StartCoroutine("SecondAttackCo");
    }

    private IEnumerator SecondAttackCo()
    {
        yield return new WaitForSeconds(secondAttackCooldown);

        IsAttacking = false;
    }

    public void ThirdAttack()
    {
        IsAttacking = true;
        entity.SetAttackTimer("ThirdAttack", thirdAttackCooldown);
        StartCoroutine("ThirdAttackCo");
    }

    private IEnumerator ThirdAttackCo()
    {
        float x = Camera.main.transform.position.x + thirdAttackOffset.x;
        Projectile clone = Instantiate(thirdAttackProjectile, new Vector2(x, thirdAttackOffset.y), Quaternion.identity);
        clone.Setup(Vector2.zero, thirdAttackDamage, gameObject);

        yield return new WaitForSeconds(thirdAttackPrepareTime);

        SoundManager.PlaySFX(thirdAttackClip);
        clone.Setup(Vector2.left, thirdAttackDamage, gameObject);
        clone.GetComponent<CircleCollider2D>().enabled = true;
        clone.GetComponent<Animator>().enabled = true;

        yield return new WaitForSeconds(thirdAttackCooldown);

        IsAttacking = false;
    }

    public override void OnDead()
    {
        entity.CanBeDamaged = false;
        target.GetComponent<Entity>().CanBeDamaged = false;
        FindObjectOfType<SceneFadeInOut>().FadeIn(2f);
        StartCoroutine("LoadScene");
    }

    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2f);
        FindObjectOfType<SceneLoader>().LoadScene();
    }
}
