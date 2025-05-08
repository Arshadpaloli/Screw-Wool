using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Tray : MonoBehaviour
{
    public List<EachCap> TrayHold;
    public bool IsKeyHolder,IsLock;
    public GameObject key,Lock,RopObject;
    private bool CheckHid,checkRop,OnKin;
    void Start()
    {
        if (IsKeyHolder)
        {
            transform.parent.parent.transform.GetComponent<Levelfeatures>().LockedTrays.Add(this);

            key =   Instantiate(Levelfeatures.instance.KeyPrefab,transform.parent.position,Quaternion.identity);
          key.transform.parent = transform.parent;
          key.transform.localPosition =new Vector3(0f,0f,1.5f);
          key.transform.localRotation = Quaternion.Euler(90, 0, 0);
          key.transform.DOScale(.8F, .5f)
              .SetLoops(-1, LoopType.Yoyo)  // Loop infinitely with Yoyo effect
              .SetEase(Ease.InOutQuad);  // Add easing for smooth scaling

        }
        else if (IsLock)
        {
            transform.parent.parent.transform.GetComponent<Levelfeatures>().LockedTrays.Add(this);

             Lock =   Instantiate(Levelfeatures.instance.LockPrefab,transform.parent.position,Quaternion.identity);
            Lock.transform.parent = transform.parent;
            Lock.transform.localPosition =new Vector3(0f,-0.5f,1.5f);
            Lock.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            
                print(transform.parent.transform.lossyScale);
                print(Lock.transform.lossyScale);
                Transform topParent = transform;
                while (topParent.parent != null)
                {
                    topParent = topParent.parent;
                    // Get the top parent's X scale and extract the decimal part
                    float topParentXScale = topParent.localScale.x;
                    float decimalPart = topParentXScale - Mathf.Floor(topParentXScale);
                    int decimalAsInt = Mathf.RoundToInt(decimalPart * 10);

// Get the current scale of the Lock object
                    Vector3 lockScale = Lock.transform.localScale;

// If the top parent's X scale is less than 1, minimize Lock's scale by 0.02 per axis
                    if (topParentXScale < 1f)
                    {
                        lockScale.x -= 0.02f;  // Minimize X scale by 0.02
                        lockScale.y -= 0.02f;  // Minimize Y scale by 0.02
                        lockScale.z -= 0.02f;  // Minimize Z scale by 0.02
                    }
                    else
                    {
                        // Otherwise, add the decimal part to the Lock's scale (multiplied by 0.01)
                        lockScale.x += decimalAsInt * 0.01f;
                        lockScale.y += decimalAsInt * 0.01f;
                        lockScale.z += decimalAsInt * 0.01f;
                    }

// Apply the new scale to Lock
                    Lock.transform.localScale = lockScale;

                    Debug.Log("Lock Scale after update: " + Lock.transform.localScale);


                }
                

            
            for (int i = 0; i < TrayHold.Count; i++)
            {
                TrayHold[i].transform.GetComponent<CapsuleCollider>().enabled = false;
            }
        }

      

        // SetUpRop();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Cap") && !IsInTrayHold(other.gameObject))
        {
            other.tag = "Untagged";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Untagged") && !IsInTrayHold(other.gameObject))
        {
            other.tag = "Cap";
        }
    }

    private bool IsInTrayHold(GameObject capObject)
    {
        foreach (EachCap cap in TrayHold)
        {
            if (cap.gameObject == capObject)
                return true;
        }
        return false;
    }


    public void OnOneGo()
    {
        if (!OnKin)
        {
            OnKin = true;
            transform.GetComponent<Rigidbody>().isKinematic = false;
        }
       
        if (TrayHold.Count < 1)
        {
            transform.parent = null;
        }
    }
    

//     public void SetUpRop()
//     {
//         if (checkRop)
//         {
//             List<EachCap> ropeCaps = new List<EachCap>();
//
//             foreach (var tray in TrayHold)
//             {
//                 if (tray.Rope)
//                 {
//                     ropeCaps.Add(tray.GetComponent<EachCap>());
//                 }
//             }
//             Transform topParent = transform;
//             while (topParent.parent != null)
//             {
//                 topParent = topParent.parent;
//
//             }
// //
//             Vector3 TopParentPos = topParent.lossyScale;
//             topParent.transform.localScale = Vector3.one;
//
//             if (ropeCaps.Count == 2)
//             {
//                 EachCap ropeA = ropeCaps[0];
//                 EachCap ropeB = ropeCaps[1];
//
//                 ropeA.ConnectedObject = ropeB;
//                 ropeB.ConnectedObject = ropeA;
//
//                 Vector3 midPoint = (ropeA.transform.position + ropeB.transform.position) / 2f;
//
//                 // Default rotation
//                 Quaternion rotation = Quaternion.identity;
//
//                 // Check if both are "down" — adjust this condition as needed
//                 // Check if ropeA and ropeB have the same parent Y position
//                 if (ropeA.transform.parent. transform.position.y == ropeB.transform.parent.transform.position.y)
//                 {
//                     // print("x");
//                     if (transform.parent.localEulerAngles.x == 270)
//                     {
//                         if (ropeA.transform.parent.transform.position.x == ropeB.transform.parent.transform.position.x)
//                         {
//                             rotation = Quaternion.Euler(0f, 90f, 0f);
//
//                         }
//
//                     }
//                     else
//                     {
//                         if (ropeA.transform.parent.transform.position.x == ropeB.transform.parent.transform.position.x)
//                         {
//                             if (transform.parent.localEulerAngles.x == 90)
//                             {
//                                 rotation = Quaternion.Euler(0f, 90f, 0f);
//
//                             }
//                             
//                         }
//                         else
//                         {
//                             rotation = Quaternion.Euler(0f, 0f, 0f);
//
//                         }
//                     }
//
//                 }
//                 else
//                 {
//                     if (ropeA.transform.parent.transform.localPosition.z == ropeB.transform.parent.transform.localPosition.z)
//                     {
//                         rotation = Quaternion.Euler(0f, 0f, 0f);
//
//                     }
//                     else
//                     {
//                         rotation = Quaternion.Euler(0f, 90f, 0f);
//
//                     }
//                     
//                 }
//
//                 // if (transform.parent.localEulerAngles.x==270)
//                 // {
//                 //    rotation = Quaternion.Euler(0, 90, 0); 
//                 //     print(rotation);
//                 // }
//               
//                 RopObject = Instantiate(Levelfeatures.instance.RopePrefab, midPoint, Quaternion.identity);
//                 RopObject.transform.SetParent(transform);
//                 RopObject.transform.localRotation = rotation;
//                 RopObject.transform.localPosition = new Vector3(
//                     RopObject.transform.localPosition.x,
//                     1.4f,
//                     RopObject.transform.localPosition.z
//                 );
//                 topParent.transform.localScale = TopParentPos;
//                 RopObject.gameObject.SetActive(false);
//             }
//             else
//             {
//                 transform.name = "cahane";
//                 print(transform.name);
//                 Debug.LogWarning("SetUpRop: Expected exactly 2 Rope objects, but found " + ropeCaps.Count);
//             }
//         }
//        
//     }

    public void TurnOnRop()
    {
        if (RopObject !=null&&!RopObject.activeInHierarchy)
        {
            Vector3 RopOgScale = RopObject.transform.localScale;
            RopObject.transform.localScale = Vector3.zero;
            RopObject.gameObject.SetActive(true);
            RopObject.transform.DOScale(RopOgScale, 0.4f).SetEase(Ease.OutBack);

        }
    }
    // public void CheckForNeighbors(GameObject Mover)
    // {
    //     if (CheckHid)
    //     {
    //         List<GameObject> nearestObjects = new List<GameObject>();
    //         float shortestDistance = Mathf.Infinity;
    //         float threshold = 0.01f; // use a small threshold to handle floating point inaccuracies
    //
    //         foreach (EachCap cap in TrayHold)
    //         {
    //             GameObject obj = cap.gameObject;
    //             if (obj == Mover) continue;
    //
    //             float distance = Vector3.Distance(Mover.transform.position, obj.transform.position);
    //
    //             if (Mathf.Abs(distance - shortestDistance) <= threshold)
    //             {
    //                 nearestObjects.Add(obj);
    //             }
    //             else if (distance < shortestDistance - threshold)
    //             {
    //                 nearestObjects.Clear();
    //                 nearestObjects.Add(obj);
    //                 shortestDistance = distance;
    //             }
    //         }
    //
    //         if (nearestObjects.Count > 0)
    //         {
    //             Debug.Log("Nearest objects:");
    //             foreach (GameObject obj in nearestObjects)
    //             {
    //                 EachCap cap = obj.GetComponent<EachCap>();
    //                 if (cap.Hidden)
    //                 {
    //                     cap.transform.GetComponent<CapsuleCollider>().enabled = true;
    //
    //                     cap.HiddenGameObject.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.Linear);
    //                 }
    //                 // obj.name = "near";
    //                 // Debug.Log(obj.name + " at distance: " + shortestDistance);
    //             }
    //         }
    //         else
    //         {
    //             Debug.Log("No neighbors found.");
    //         }
    //     }
    //  
    // }
    public void SecondTraySpwan()
    {
        if (transform.parent.parent.transform.GetComponent<Levelfeatures>() != null)
        {
            transform.parent.parent.transform.GetComponent<Levelfeatures>().NewTraySpawner(gameObject);

        }
    }
    public void IfAny()
    {
        if (IsLock)
        {
           

        }
        else if (IsKeyHolder)
        {
            
            key.transform.parent=null;
            Levelfeatures.instance.key_MoveToLock(key.GetComponent<KeyAndLock>());
            
        }
    }
    
}
