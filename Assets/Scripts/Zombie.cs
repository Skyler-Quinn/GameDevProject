using UnityEngine;
using TMPro;
using UnityEngine.AI;
using System.Collections;

public class Zombie : MonoBehaviour
{
    public TextMeshProUGUI wordText;
    public bool isActive = false;
    public WordData myWordData;

    public Transform player;
    private NavMeshAgent agent;
    [SerializeField] private float attackRange = 2f; 
    private Animator animator; 
    private bool isAttacking = false;
    [SerializeField] private float destroyDelay = 0.5f;
    private bool isDestroying = false;

    public void Setup(WordData data, Transform playerRef)
    {
        myWordData = data;
        player = playerRef;
        wordText.text = data.word;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        isActive = true;
        wordText.color = Color.green;
    }

    public void Deactivate()
    {
        isActive = false;
        wordText.color = Color.white;
    }

    public void DestroyZombie()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (player != null && agent != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (distanceToPlayer <= attackRange)
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    if (animator != null)
                    {
                        animator.SetBool("IsAttacking", true);
                        if (!isDestroying)
                        {
                            StartCoroutine(DestroyAfterDelay());
                        }
                    }
                }
            }
            else
            {
                if (isAttacking)
                {
                    isAttacking = false;
                    if (animator != null)
                    {
                        animator.SetBool("IsAttacking", false);
                    }
                }
                agent.SetDestination(player.position);
            }
        }
    }
    private IEnumerator DestroyAfterDelay()
    {
        isDestroying = true;
        yield return new WaitForSeconds(destroyDelay);
        DestroyZombie();
    }
}
