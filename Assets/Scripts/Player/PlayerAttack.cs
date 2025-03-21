using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform chargedPoint;
    [SerializeField] private GameObject[] charged;
    [SerializeField] private AudioClip chargedSound;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }
 
    private void Attack()
    {
        SoundManager.instance.PlaySound(chargedSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;
        
        StartCoroutine(ShootWithDelay(0.5f));
    }
    private int FindCharged()
    {
        for (int i = 0; i < charged.Length; i++)
        {
            if (!charged[i].activeInHierarchy)
            return i;
        }
        return 0;
    }

    private IEnumerator ShootWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        int chargedIndex = FindCharged();
        charged[chargedIndex].transform.position = chargedPoint.position;
        charged[chargedIndex].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
}