using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 공격을 제어하는 클래스
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
    //이전 콤보를 담을 배열
    private int[] inputCombos;

    private void Start()
    {
        movement = GetComponent<Movement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        cameraController = Camera.main.GetComponent<CameraController>();

        inputCombos = new int[maxCombo];
        for (int i = 0; i < inputCombos.Length; i++)
        {
            //0 : 아무 상태 아님, 1 : 근거리 공격, 2 : 원거리 공격
            inputCombos[i] = 0;
        }
    }

    public void MeleeAttackCheck()
    {
        //공격 중일 경우 다음 콤보를 재생하기 위해 bool 변수를 true로 설정
        //공격 중이 아닐 경우 공격 실행
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
        //공격 방향을 대시
        movement.Dash(direction);

        //콤보 인풋 정보 초기화
        isMeleeInputOn = false;
        inputCombos[currentCombo] = 1;
        currentCombo++;
        //현재 콤보에 맞는 애니메이션 재생
        playerAnimation.Play("Player_MeleeAttack" + currentCombo);

        AttackEffect();

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, Vector2.one, 0, direction, meleeDistance);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag(gameObject.tag)) continue;

            ILivingEntity entity = hit.collider.GetComponent<ILivingEntity>();
            if (entity != null)
            {
                //타겟 사이 벽 체크
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

        //현재 콤보가 최대 콤보라면 공격 정보 초기화
        if (currentCombo >= maxCombo)
        {
            AttackEnd();
            yield break;
        }

        //공격 중일 때 공격 버튼을 눌렀다면 다음 콤보 실행
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
    /// 공격 정보 초기화 함수
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
    /// 콤보에 맞는 데미지 값 반환 함수
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

        //기본 데미지 반환 필요
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
