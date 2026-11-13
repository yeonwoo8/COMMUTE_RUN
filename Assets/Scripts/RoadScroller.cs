using UnityEngine;

public class RoadScroller : MonoBehaviour
{
    [SerializeField] GameConfig config;
    [SerializeField] CommuteRun run;
    [SerializeField] float tileWidth = 4f;
    [SerializeField] float speedScale = 1f;

    float offset;

    void Update()
    {
        if (!run.Running) return;

        offset = Mathf.Repeat(offset + config.scrollSpeed * speedScale * Time.deltaTime, tileWidth);
        transform.localPosition = new Vector3(-offset, 0f, 0f);
    }
}
