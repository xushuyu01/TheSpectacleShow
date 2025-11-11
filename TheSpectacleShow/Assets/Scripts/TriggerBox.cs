using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class TriggerBox : MonoBehaviour
{
    [SerializeField]
    private bool IsActived;
    [SerializeField]
    private bool IsStay;
    private void Awake()
    {
        IsActived = false;
    }

    private Coroutine cor;

    public List<GameObject> otherLight;
    public GameObject globalLight;
    public List<GameObject> spotLight;

    public Animator animator;
    private void OnTriggerEnter(Collider other)
    {

        if (IsActived)
        {
            return;
        }
        Debug.LogError("进来了");
        if (other.tag == "Player")
        {
            Debug.LogError("是哦他");
            IsStay = true;   

            if (cor != null)
            {
                StopCoroutine(cor);
            }
           cor= StartCoroutine(DelayLoad());

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            IsStay = false ; 

        }
    }
    [Header("首次进入等待时间")]
    public float stayTime = 5f;
     [Header("等够时间后延迟时间")]
    public float waitTime = 5f;
    private IEnumerator DelayLoad()
    {
        Debug.LogError("开等");
        yield return new WaitForSeconds(waitTime);

        if (IsStay)
        {
            Debug.LogError("关等");
            IsActived = true;
            
            foreach(var i in otherLight) { i.gameObject.SetActive(false); }
            foreach(var i in spotLight) {  i.gameObject.SetActive(false); }
            globalLight.SetActive(false);
            
            yield return new WaitForSeconds(waitTime);
            Debug.LogError("开灯1");

            animator.SetTrigger("do");
            foreach (var i in otherLight) { i.gameObject.SetActive(true); } 
            globalLight.SetActive(true);

            yield return new WaitForSeconds(5f);
            Debug.LogError("开蛇灯1");

            foreach (var i in spotLight) { i.gameObject.SetActive(true );
                i.GetComponent<ShottingLight>().Active();
            }


        }
        else
        {

        }



    }


}
 
