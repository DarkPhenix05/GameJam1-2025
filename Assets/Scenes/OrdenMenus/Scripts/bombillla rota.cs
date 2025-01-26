using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombilllarota : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 relativeOffset = new Vector2(0, -4.5f);
    // Start is called before the first frame update
    void Start()
    {
        CalculateInitialPosition();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositionRelativeToPlayer();
    }

    private void CalculateInitialPosition()
    {
        Vector3 playerPosition = player.position;
        Vector3 newPosition = new Vector3(playerPosition.x + relativeOffset.x, playerPosition.y + relativeOffset.y, transform.position.z);
        transform.position = newPosition;

    }
    private void UpdatePositionRelativeToPlayer()
    {
        Vector3 playerPosition = player.position;
        Vector3 newPosition = new Vector3(playerPosition.x + relativeOffset.x, playerPosition.y + relativeOffset.y, transform.position.z);
        transform.position = newPosition;
    }
}