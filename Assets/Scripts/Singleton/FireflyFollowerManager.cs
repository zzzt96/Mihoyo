using UnityEngine;
using System.Collections.Generic;

public class FireflyFollowerManager : SingletonMono<FireflyFollowerManager>
{
    public Transform leader;          // 主萤火虫
    public Transform followerParent;
    public GameObject followerPrefab; // 随从预制体
    public int initialCount = 0;      // 初始随从数量
    public float spacing = 1.2f;      // 每个随从间距

    private List<Transform> followers = new List<Transform>();

     void Start()
    {
        for (int i = 0; i < initialCount; i++)

        {
            AddFollower();
        }
    }

    public void AddFollower()
    {
        Transform followTarget = (followers.Count == 0) ? leader : followers[followers.Count - 1];
        Vector3 spawnPos = followTarget.position - followTarget.forward * spacing;

        GameObject newFollower = Instantiate(followerPrefab, spawnPos, Quaternion.identity, followerParent);
        FireflyFollower followScript = newFollower.GetComponent<FireflyFollower>();
        followScript.target = followTarget;
        followScript.followDistance = spacing;
        followers.Add(newFollower.transform);
    }
    public void AddFollower(GameObject newFollower)
    {
        Transform followTarget = (followers.Count == 0) ? leader : followers[followers.Count - 1];
        Vector3 spawnPos = followTarget.position - followTarget.forward * spacing;

        
        //newFollower.transform.position = spawnPos;
        //newFollower.transform.rotation = Quaternion.identity;
        newFollower.transform.SetParent(followerParent);
        FireflyFollower followScript = newFollower.GetComponent<FireflyFollower>();
        followScript.target = followTarget;
        followScript.followDistance = spacing;
        followers.Add(newFollower.transform);
    }
}
