using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileEntity : HPEntity
{
    // Refs:

    // Vars:
    public float moveSpeed = 1.3f;
    public float followRange = 2f;
    public GameObject trackedPlayer;
    WaitForSeconds delay = new WaitForSeconds(1);

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        StartCoroutine(DetectPlayer());
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        transform.position += 0.05f * OffsetTowardsEnemy();
    }

    // Utils:
    private IEnumerator DetectPlayer()
    {
        while (true)
        {
            UpdateClosestEnemy();
            yield return delay;
        }
    }
    private Vector3 OffsetTowardsEnemy()
    {
        if (trackedPlayer == null)
        {
            return Vector3.zero;
        }
        Vector3 offset = trackedPlayer.transform.position - transform.position;
        return Mathf.Clamp(offset.magnitude, moveSpeed / 2f, moveSpeed) * offset.normalized;
    }
    private void UpdateClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, followRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player")) {
                trackedPlayer = collider.gameObject;
                return;
            }
        }
        trackedPlayer = null;
        return;
    }
}
