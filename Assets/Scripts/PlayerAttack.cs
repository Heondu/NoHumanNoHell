using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾��� ������ �����ϴ� Ŭ����
/// </summary>
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float meleeDistance;
    [SerializeField] private float meleeCooldown;

    [SerializeField] private float rangedDistance;
    [SerializeField] private float rangedCooldown;
    [SerializeField] private GameObject projectile;

    [SerializeField] private bool isDrawDebug = true;

    [SerializeField] private int maxCombo;
    private int currentCombo;

    [SerializeField] private LayerMask groundLayer;

    private Movement movement;
    private Vector3 attackDirection;
    private PlayerAnimation playerAnimation;
    private CameraController cameraController;

    private bool isAttacking = false;
    private bool isMeleeInputOn;
    private bool isRangedInputOn;
    //���� �޺��� ���� �迭
    private int[] inputCombos;

    private void Start()
    {
        movement = GetComponent<Movement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        cameraController = Camera.main.GetComponent<CameraController>();

        inputCombos = new int[maxCombo];
        for (int i = 0; i < inputCombos.Length; i++)
        {
            //0 : �ƹ� ���� �ƴ�, 1 : �ٰŸ� ����, 2 : ���Ÿ� ����
            inputCombos[i] = 0;
        }
    }

    public void MeleeAttackCheck()
    {
        //���� ���� ��� ���� �޺��� ����ϱ� ���� bool ������ true�� ����
        //���� ���� �ƴ� ��� ���� ����
        if (isAttacking)
        {
            isMeleeInputOn = true;
        }
        else
        {
            isAttacking = true;
            MeleeAttack();
        }
    }

    public void RangedAttackCheck()
    {
        if (isAttacking)
        {
            isRangedInputOn = true;
        }
        else
        {
            isAttacking = true;
            RangedAttack();
        }
    }

    private void MeleeAttack()
    {
        Vector3 direction = (Utilities.GetMouseWorldPos() - transform.position).normalized;
        attackDirection = direction;
        //���� ������ ���
        movement.Dash(direction);

        //�޺� ��ǲ ���� �ʱ�ȭ
        isMeleeInputOn = false;
        inputCombos[currentCombo] = 1;
        currentCombo++;
        //���� �޺��� �´� �ִϸ��̼� ���
        playerAnimation.Play("Player_MeleeAttack" + currentCombo);

        AttackEffect();

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Vector2.one, 0, direction, meleeDistance);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag(gameObject.tag)) continue;

            ILivingEntity entity = hit.collider.GetComponent<ILivingEntity>();
            if (entity != null)
            {
                //Ÿ�� ���� �� üũ
                RaycastHit2D hitWallCheck = Physics2D.Raycast(transform.position, hit.point - (Vector2)transform.position, meleeDistance, groundLayer);
                if (hitWallCheck) continue;

                entity.TakeDamage(CalcComboDamage(), gameObject);
            }
        }

        StartCoroutine("Cooldown", meleeCooldown);
    }

    private void RangedAttack()
    {
        Vector3 direction = (Utilities.GetMouseWorldPos() - transform.position).normalized;
        attackDirection = direction;

        isRangedInputOn = false;
        inputCombos[currentCombo] = 2;
        currentCombo++;
        playerAnimation.Play("Player_RangedAttack");

        AttackEffect();

        PlayerProjectile clone = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<PlayerProjectile>();
        clone.Setup(direction, rangedDistance, CalcComboDamage(), gameObject);

        StartCoroutine("Cooldown", rangedCooldown);
    }

    private IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);

        //���� �޺��� �ִ� �޺���� ���� ���� �ʱ�ȭ
        if (currentCombo >= maxCombo)
        {
            AttackEnd();
            yield break;
        }

        //���� ���� �� ���� ��ư�� �����ٸ� ���� �޺� ����
        if (isMeleeInputOn)
        {
            MeleeAttack();
        }
        else if (isRangedInputOn)
        {
            RangedAttack();
        }
        else
        {
            AttackEnd();
        }
    }

    /// <summary>
    /// ���� ���� �ʱ�ȭ �Լ�
    /// </summary>
    private void AttackEnd()
    {
        isAttacking = false;
        isMeleeInputOn = false;
        isRangedInputOn = false;
        currentCombo = 0;
        attackDirection = Vector3.zero;
        playerAnimation.Play("Player_Idle");
        for (int i = 0; i < inputCombos.Length; i++)
        {
            inputCombos[i] = 0;
        }
    }

    /// <summary>
    /// �޺��� �´� ������ �� ��ȯ �Լ�
    /// </summary>
    /// <returns></returns>
    private int CalcComboDamage()
    {
        if (currentCombo == 1)
        {
            if (inputCombos[0] == 1) return 10;
            else if (inputCombos[0] == 2) return 5;
        }
        else if (currentCombo == 2)
        {
            if (inputCombos[0] == 1 && inputCombos[1] == 1) return 10;
            else if (inputCombos[0] == 1 && inputCombos[1] == 2) return 15;
            else if (inputCombos[0] == 2 && inputCombos[1] == 2) return 10;
        }
        else if (currentCombo == 3)
        {
            if (inputCombos[0] == 1 && inputCombos[1] == 1 && inputCombos[2] == 1) return 20;
            else if (inputCombos[0] == 1 && inputCombos[1] == 1 && inputCombos[2] == 2) return 20;
            else if (inputCombos[0] == 1 && inputCombos[1] == 2 && inputCombos[2] == 2) return 15;
            else if (inputCombos[0] == 2 && inputCombos[1] == 2 && inputCombos[2] == 2) return 25;
        }

        //�⺻ ������ ��ȯ �ʿ�
        return 0;
    }

    private void OnDrawGizmos()
    {
        if (!isDrawDebug) return;

        if (isAttacking)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, attackDirection * meleeDistance);
            Gizmos.DrawWireCube(transform.position + attackDirection * meleeDistance, Vector3.one);
        }
    }

    public Vector3 GetAttackDirection()
    {
        return attackDirection;
    }

    private void AttackEffect()
    {
        if (currentCombo == maxCombo)
        {
            cameraController.Shake(0.1f, 0.1f);
            cameraController.Slow(0.4f, 0.2f);
        }
    }
}
