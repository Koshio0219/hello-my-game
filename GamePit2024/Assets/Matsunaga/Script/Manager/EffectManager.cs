using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    public enum EffectID
    {
        None = -1,
        // Lighter�̍U����������������������G�t�F�N�g
        EnemyDead,
    }
    [SerializeField]
    List<GameObject> _EffectList = new List<GameObject>();

    /// �G�t�F�N�g�Đ�
    /// ���� �G�t�F�N�gID, �Đ��|�W�V����
    /// �g����
    /// EffectManager.Instance.Play(EffectManager.EffectID.HitBullet, Vector3(0,0,0));
    public GameObject Play(EffectID id, Vector3 position)
    {
        if (id == EffectID.None)
        {
            return null;
        }

        var index = (int)id;
        var prefab = _EffectList[index];
        if (index < 0 || _EffectList.Count <= index)
        {
            Debug.Log("Over Range!");
            return null;
        }
        if (prefab == null) return null;
        var obj = Instantiate(prefab, position, prefab.transform.rotation);
        obj.transform.SetParent(transform);
        Destroy(obj, 1.2f);
        return obj;
    }
}
