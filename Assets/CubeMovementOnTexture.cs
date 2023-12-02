using UnityEngine;

public class CubeMovementOnTexture : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float borderSize = 4f;

    void Update()
    {
        // Generate random movement values
        float randomHorizontal = Random.Range(-1f, 1f);
        float randomVertical = Random.Range(-1f, 1f);

        // Calculate movement direction
        Vector3 movement = new Vector3(randomHorizontal, randomVertical, 0f).normalized;

        // Move the cube
        transform.Translate(movement * moveSpeed * Time.deltaTime);

        // Clamp the cube position to stay within the border
        float clampedX = Mathf.Clamp(transform.position.x, -borderSize, borderSize);
        float clampedY = Mathf.Clamp(transform.position.y, -borderSize, borderSize);
        transform.position = new Vector3(clampedX, clampedY, 0f);
    }
}
