using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Lớp pool game object cho phép tạo sẵn, gom nhóm, thu hồi cũng như trả ra các game object
/// </summary>
public class GameObjectPool : MonoBehaviour
{

    public Stack<GameObject> pooledGOStack = new();

    public GameObject pooledGO;

    public GameObject tempPooledGO;
    public List<GameObject> pooledGOList = new();
    /// <summary>
    /// Phương thức thu hồi đối tượng game object vào pool
    /// </summary>
    /// <param name="pooledGameObject"></param>
    public void GoInPool(GameObject pooledGameObject)

    {
        pooledGOStack.Push(pooledGameObject);
        pooledGameObject.transform.parent = transform;
        pooledGameObject.transform.localPosition = Vector3.zero;
        pooledGameObject.SetActive(false);

        pooledGOList = pooledGOStack.ToList();
    }

    /// <summary>
    /// Phương thức lấy đối tượng game object ra ngoài pool
    /// </summary>
    /// <param name="defaultPooledGO">Nếu không có đối tượng nào còn trong pool thì tạo mới mặc định 1 pooled game object được truyền vào</param>
    /// <returns></returns>
    public GameObject GoOutPool(Transform parentTransform)
    {
        if (pooledGOStack.Count > 0)
        {
            tempPooledGO = pooledGOStack.Pop();
            tempPooledGO.transform.SetParent(parentTransform);
            tempPooledGO.SetActive(true);

            pooledGOList = pooledGOStack.ToList();

            return tempPooledGO;
        }
        else
        {

            tempPooledGO = Instantiate(pooledGO, parentTransform);
            tempPooledGO.transform.localPosition = Vector3.zero;
            tempPooledGO.SetActive(true);

            pooledGOList = pooledGOStack.ToList();

            return tempPooledGO;
        }

    }

    /// <summary>
    /// Phương thức tạo sẵn các game object theo số lượng yêu cầu
    /// </summary>
    /// <param name="quantity"></param>
    /// <param name="pooledGO"></param>
    /// <param name="parentTransform"></param>
    public void PreWarm(int quantity, Transform parentTransform)
    {
        for (int i = 0; i < quantity; i++)
        {
            tempPooledGO = Instantiate(pooledGO, parentTransform);
            tempPooledGO.transform.localPosition = Vector3.zero;
            tempPooledGO.SetActive(false);
            pooledGOStack.Push(tempPooledGO);

            pooledGOList = pooledGOStack.ToList();
        }
    }


}
