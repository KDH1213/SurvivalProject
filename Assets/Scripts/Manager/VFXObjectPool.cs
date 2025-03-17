//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Pool;

//public class VFXObjectPool : MonoBehaviour
//{
//    public int CreateID {  get; set; }
//    [SerializeField]
//    private VfxContainerData vfxContainerData;
//    [SerializeField]
//    private VFXObject[] vfxObjects;
//    public Dictionary<int, IObjectPool<VFXObject>> vfxObjectPoolTable { get; private set; } = new Dictionary<int, IObjectPool<VFXObject>>();

//    private void Awake()
//    {

//        foreach (var vfxObject in vfxObjects)
//        {
//            if(!vfxObjectPoolTable.ContainsKey(vfxObject.ID))
//            {
//                vfxObjectPoolTable.Add(vfxObject.ID, new ObjectPool<VFXObject>(OnCreateVFX, OnGetVFX, OnReleaseVFX, OnDestroyVFX, true, 1000));
//            }
//        }
//    }

//    private VFXObject OnCreateVFX()
//    {
//        Instantiate(vfxContainerData.VfxContainerTable[CreateID][0]).TryGetComponent(out VFXObject vFXObject);
//        vFXObject.SetPool(vfxObjectPoolTable[CreateID]);
//        return vFXObject;
//    }

//    private void OnGetVFX(VFXObject vfxObject)
//    {
//        vfxObject.gameObject.SetActive(true);
//    }

//    private void OnReleaseVFX(VFXObject vfxObject)
//    {
//        vfxObject.gameObject.SetActive(false);
//    }

//    private void OnDestroyVFX(VFXObject vfxObject)
//    {
//        Destroy(vfxObject.gameObject);
//    }

//    public VFXObject GetVFX(int vfxID)
//    {
//        CreateID = vfxID;
//        return vfxObjectPoolTable[CreateID].Get();
//    }
//}
