using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockLayer;

    private BoxCollider2D collider;
    private Rigidbody2D body;
    private float inverseMoveTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    protected IEnumerator smoothMethod(Vector3 finalPoint)
    {
        float remainingDistance = (transform.position - finalPoint).sqrMagnitude;
        while (remainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(body.position, finalPoint, inverseMoveTime * Time.deltaTime);
            body.MovePosition(newPosition);
            remainingDistance = (transform.position - finalPoint).sqrMagnitude;
            yield return null;
        }
    }

    protected abstract void OnCantMove <T> (T component) where T : Component;

    protected virtual void MoveAttempt <T> (int x, int y) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(x, y, out hit);

        if (hit.transform == null)
            return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
            OnCantMove(hitComponent);
    }

    protected bool Move(int x, int y, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(x, y);

        collider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockLayer);
        collider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(smoothMethod(end));
            return true;
        }

        return false;
    }
}