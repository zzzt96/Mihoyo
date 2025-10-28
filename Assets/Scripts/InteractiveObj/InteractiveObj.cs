using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObj : MonoBehaviour
{

    public List<GameObject> followers = new List<GameObject>();

    private void AddFollowers()
    {
        for(int i=0;i< followers.Count; i++)
        {
            FireflyFollowerManager.Instance.AddFollower(followers[i]);
        }

        Destroy(gameObject);

    }

    public virtual void CallFunction()
    {
        AddFollowers();
    }

    public virtual void ResetFunction()
    {

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CallFunction();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetFunction();
        }
    }
}
