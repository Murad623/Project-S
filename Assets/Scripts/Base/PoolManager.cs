using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private int slimePoolSize = 20;
    public static int[] xPos = {0,0,0};
    private Queue<GameObject> slimePool;
    [SerializeField] private GameObject slimePref;
    private void Awake() 
    {
        // Slime -->
        slimePool = new Queue<GameObject>();
        for (int i = 0; i < slimePoolSize; i++)
        {
            GameObject chainBall = Instantiate(slimePref,Vector3.zero,Quaternion.identity);
            chainBall.SetActive(false);
            slimePool.Enqueue(chainBall);
        }
    }
    private GameObject GetFromPool(Queue<GameObject> pool){
        if(pool.Count > 0){
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }
    public void ReturnToPull(GameObject obj){
        obj.SetActive(false);
        switch (obj.tag)
        {
            case "ChainBall":
                slimePool.Enqueue(obj);
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        GameObject slime = GetFromPool(slimePool);
        if (slime != null) SetPos(slime);
    }
    private void SetPos(GameObject obj){
        float x = Random.Range(-20,20);
        float z = Random.Range(-20,20);
        obj.transform.position = new Vector3(x,0,z);
    }
    private void ResetXPos(){
        for (int i = 0; i < xPos.Length; i++) xPos[i] = 0;
    }
}
