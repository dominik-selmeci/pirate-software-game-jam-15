using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float speed = 1.5f;
	[SerializeField] LayerMask layer;
    private GameObject player;
    private Vector3 localScale;

	private bool hasLineOfSight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
		if (hasLineOfSight)
		{
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
		}
	}

	private void FixedUpdate()
	{

		RaycastHit2D ray = Physics2D.Raycast(transform.position, (player.transform.position - transform.position), 10f);
		if(ray.collider != null)
		{
			Debug.Log(ray.collider.tag);
			if(ray.collider.CompareTag("Walls") || ray.collider.CompareTag("Player"))
			{
				hasLineOfSight = (!ray.collider.CompareTag("Walls"));
				if (hasLineOfSight)
				{
					Debug.DrawRay(transform.position, (player.transform.position - transform.position), Color.green);
				}
				else
				{
					Debug.DrawRay(transform.position, (player.transform.position - transform.position), Color.red);
				}
			}
		}
	}

	//private void LateUpdate()
	//{
	//	if(transform.position.x > 0)
	//	{
	//           transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
	//	}else if(transform.position.x < 0)
	//	{
	//           transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
	//       }
	//}
}
