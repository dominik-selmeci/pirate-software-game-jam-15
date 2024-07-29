using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float speed = 1.5f;
	[SerializeField] private float viewRadius;
	[SerializeField] Transform[] moveSpots;
	[SerializeField] LayerMask layerMask;
	[SerializeField] Player player;
	private int randomSpot;

	private Vector3 localScale;

	private float waitTime;
	public float startWaitTime;
	private bool isChasing = false;

	private Vector2 lastPos;
	

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        localScale = transform.localScale;
		randomSpot = Random.Range(0, moveSpots.Length);
		waitTime = startWaitTime;
		lastPos = transform.position;
    }

	private void FixedUpdate()
	{
		SearchPlayer();

		if (!isChasing)
		{
			Patrol();
		}else
		{
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
		}
	}

	private void LateUpdate()
	{
		Vector2 currentPos = transform.position;
		Vector2 deltaPos = currentPos - lastPos;

		if (deltaPos.x > 0)
		{
			transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
		}
		else if (deltaPos.x < 0)
		{
			transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
		}

		lastPos = transform.position;
	}

	private void SearchPlayer()
	{
		Collider2D collider = Physics2D.OverlapCircle(transform.position, viewRadius, layerMask);
		if (collider != null)
		{
			if (collider.CompareTag("Player"))
			{
				isChasing = true;
			}
		}
	}


	private void Patrol()
	{
		transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);

		if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
		{
			if (waitTime <= 0)
			{
				randomSpot = Random.Range(0, moveSpots.Length);
				waitTime = startWaitTime;
			}
			else
			{
				waitTime -= Time.deltaTime;
			}
		}
	}

	public void Die()
	{
		Destroy(gameObject);
		Destroy(this);
	}
}
