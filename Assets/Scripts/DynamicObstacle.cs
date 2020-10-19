using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObstacle : MonoBehaviour
{
    public Vector2[] obstaclePoints;
    public float moveSpeed;
    private int nextIndex;
    private bool isIncreasingIndex;
    public GameObject edgeObject;
    public GameObject lineObject;

    // Start is called before the first frame update
    void Start()
    {
        nextIndex = 1;
        isIncreasingIndex = true;

        foreach (var point in obstaclePoints) {
            Instantiate(edgeObject, point, Quaternion.identity);
        }

        for (int i = 1; i < obstaclePoints.Length; ++i) {
            instantiateLines(obstaclePoints[i - 1], obstaclePoints[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var step = moveSpeed * Time.deltaTime;
        gameObject.transform.position = Vector2.MoveTowards(transform.position, obstaclePoints[nextIndex], step);

        if (isAboutEqual(transform.position.x, obstaclePoints[nextIndex].x) && isAboutEqual(transform.position.y, obstaclePoints[nextIndex].y)) {
            if (nextIndex == 0) {
                nextIndex++;
                isIncreasingIndex = true;
            } else if (nextIndex == obstaclePoints.Length - 1) {
                nextIndex--;
                isIncreasingIndex = false;
            } else if (isIncreasingIndex) {
                nextIndex++;
            } else {
                nextIndex--;
            }
        }
    }

    private bool isAboutEqual(float a, float b)
    {
        return Mathf.Abs(a - b) <= 0.0001f;
    }

    private void instantiateLines(Vector2 a, Vector2 b)
    {
        int segmentsToCreate = Mathf.RoundToInt(Vector2.Distance(a, b) / 0.5f);
        float distance = 1f / segmentsToCreate;
        float lerpValue = 0f;
        Vector2 diffV2 = a - b;
        float angle = Mathf.Atan2(diffV2.y, diffV2.x) * Mathf.Rad2Deg - 90;

        for (int i = 0; i < segmentsToCreate; ++i) {
            {
                lerpValue += distance;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);

                Instantiate(lineObject, Vector2.Lerp(a, b, lerpValue), rotation);
            }
        }
    }
}
