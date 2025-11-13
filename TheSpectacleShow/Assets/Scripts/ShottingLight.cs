using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotateStruct
{
    public Vector3 dir;
    public float speeed = 5;
    public float rotTime = 5;
    public Color lightColor = Color.white;
}

public class ShottingLight : MonoBehaviour
{
    public Material originMat;
    public Material targetMat;
    public bool IsActive;
    public List<RotateStruct> todoMission; // 修改为RotateStruct列表

    private float rotSpeed;
    private Vector3 tarDir;
    private float rotTime;
    private Color currentLightColor;
    private Light shootingLight;
    private Coroutine missionCoroutine;

    // 光线检测相关变量
    private GameObject lastHitObject;

    public GameObject thisLightGo;
    private void Awake()
    {
        IsActive = false;
        shootingLight = GetComponent<Light>();
         
    }

  
    public void Active()
    {
        IsActive = true;

        // 在启用后按照todoMission来循环执行
        if (missionCoroutine != null)
            StopCoroutine(missionCoroutine);

        missionCoroutine = StartCoroutine(ExecuteMissions());
    }
     

    private IEnumerator ExecuteMissions()
    {
        int currentMissionIndex = 0;

        while (IsActive && todoMission != null && todoMission.Count > 0)
        {
            // 执行当前任务
            RotateStruct currentMission = todoMission[currentMissionIndex];
            Rotate(currentMission);

            // 等待旋转完成
            yield return new WaitForSeconds(currentMission.rotTime);

            // 移动到下一个任务
            currentMissionIndex = (currentMissionIndex + 1) % todoMission.Count;
        }
    }

    public void Rotate(RotateStruct d)
    {
        rotSpeed = d.speeed;
        rotTime = d.rotTime;
        tarDir = d.dir.normalized;
        currentLightColor = d.lightColor;

        //// 更新光线颜色
        //if (shootingLight != null)
        //    shootingLight.color = currentLightColor;
 
    }

    private float timer = 0;
    private void Update()
    {
        if (!IsActive)
        {
            return;
        }
        UpdateShotting();

        if (queue.Count > 0)
        {
            if (timer > 1f)
            {
                timer= 0;
                ExecuteRst(queue.Dequeue());    
            }
            timer += Time.deltaTime;
        }
    }

    private void UpdateShotting()
    {
        // 平滑旋转到目标方向
        if (tarDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(tarDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }

        // 发射面对方向的射线
        RaycastHit hit;
        Vector3 rayDirection = transform.forward;
        Vector3 rayStart = thisLightGo.transform.position;
 
        if (Physics.Raycast(rayStart, rayDirection, out hit, Mathf.Infinity))
        {
            

            // 检测tag为box的物品
            if (hit.collider.CompareTag("Box"))
            {
                HandleBoxHit(hit.collider.gameObject);
            }
            //else
            //{
            //    // 如果击中的不是Box，恢复上一个物体的材质
            //    ResetLastHitObject();
            //}
        }
        //else
        //{
        //    // 没有击中任何物体，显示长距离光线并恢复上一个物体材质
          
        //    ResetLastHitObject();
        //}
    }

    private void HandleBoxHit(GameObject hitObject)
    {
        // 如果击中的是新物体，恢复上一个物体的材质
        if (lastHitObject != null && lastHitObject != hitObject)
        {
            ResetMaterial(lastHitObject);
        }

        // 如果当前物体不是上一个物体，或者需要更新材质
        if (lastHitObject != hitObject)
        {
            // 设置新物体的目标材质
            SetTargetMaterial(hitObject);
            lastHitObject = hitObject;
        }
    }

    private void SetTargetMaterial(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && targetMat != null)
        {
            renderer.material = targetMat;
            renderer.material.SetColor("_EmissionColor", currentLightColor);
        }
    }

    Queue<GameObject> queue = new Queue<GameObject>();
    private void ResetMaterial(GameObject obj)
    {
        if (obj == null) return;

        queue.Enqueue(obj); 
    }

    private void ExecuteRst(GameObject obj)
    {
        if (obj == null) return;
         

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null && originMat != null)
        {
            renderer.material = originMat;
        }
    }

    private void ResetLastHitObject()
    {
        if (lastHitObject != null)
        {
            ResetMaterial(lastHitObject);
            lastHitObject = null;
        }
    }
      
    // 添加调试绘制
    private void OnDrawGizmos()
    {
        if (IsActive)
        {
            Gizmos.color = currentLightColor;
            Gizmos.DrawRay(transform.position, transform.forward * 1000f);
        }
    }
}