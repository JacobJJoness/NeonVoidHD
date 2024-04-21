using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[DisallowMultipleComponent]
public class EnemyReferences : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;

    [Header("Stats")]
    public float pathUpdateDelay = 0.2f;
    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float projectileForce = 10f; // Variable for the force applied to the projectile

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
}
