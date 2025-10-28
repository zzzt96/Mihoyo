using UnityEngine;
using System.Collections.Generic;

public class FireflyFollowerManager : SingletonMono<FireflyFollowerManager>
{
    public Transform leader;          // ��ө���
    public Transform followerParent;
    public GameObject followerPrefab; // ���Ԥ����
    public int initialCount = 0;      // ��ʼ�������
    public float spacing = 1.2f;      // ÿ����Ӽ��

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
