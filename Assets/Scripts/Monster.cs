using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    [Header("Hit Animations")]
    [SerializeField]
    private GameObject meshObject;
    [SerializeField]
    private Material[] hitMaterials;
    private Material[] defaultMaterials;

    private bool isHit = false;
    private float hitTime;
    private float hitDuration = 0.1f;
    private int hitPoints = 3;

    private Animator animator;

    private float moveTime;
    private float moveRate = 4.0f;

    private SkinnedMeshRenderer renderer;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        moveTime = Time.time + moveRate;
        renderer = meshObject.GetComponent<SkinnedMeshRenderer>();
        defaultMaterials = renderer.materials;

    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            if (Time.time > hitTime)
            {
                renderer.materials = defaultMaterials;
                isHit = false;
                hitPoints -= 1;

                if (hitPoints <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        if (Time.time > moveTime)
        {
            if (!DungeonData.inDebugMode)
            {
                var newPos = transform.position + transform.forward * 4.0f;

                if (CanMoveToPosition(newPos))
                {
                    animator.SetBool("Walk", true);
                    LeanTween.moveLocal(gameObject, newPos, 2f)
                        .setOnComplete(() =>
                        {
                            animator.SetBool("Walk", false);
                        });
                }
                else
                {
                    var dir = Random.Range(0, 2);

                    if (dir == 0) dir = -1;

                    if (CanMoveToPosition(transform.position + transform.right * dir * 4.0f))
                    {
                        LeanTween.rotateY(gameObject, transform.rotation.eulerAngles.y + 90.0f * dir, 1f);
                    }
                    else if (CanMoveToPosition(transform.position - transform.right * dir * 4.0f))
                    {
                        LeanTween.rotateY(gameObject, transform.rotation.eulerAngles.y - 90.0f * dir, 1f);
                    }
                    else
                    {
                        LeanTween.rotateY(gameObject, transform.rotation.eulerAngles.y - 180.0f, 2f);
                    }
                }
            }

            moveTime = Time.time + moveRate;
        }
    }

    private bool CanMoveToPosition(Vector3 newPosition)
    {
        var ray = new Ray(transform.position, newPosition - transform.position);

        if (Physics.Raycast(ray, 5, 1 << 8))
        {
            return false;
        }

        return true;
    }

    public void GetHit()
    {
        isHit = true;
        renderer.materials = hitMaterials;
        hitTime = Time.time + hitDuration;
    }
}
