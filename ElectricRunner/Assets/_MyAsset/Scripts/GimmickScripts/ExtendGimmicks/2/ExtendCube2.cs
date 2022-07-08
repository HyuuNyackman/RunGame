using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendCube2 : GimmickManager
{

    [SerializeField] FloorType floorType_;
    [SerializeField] DirectionType direction;
    [SerializeField] float activeTime;
    [SerializeField] GameObject parentObj;

    [SerializeField] GameObject floorObj;
    [SerializeField] float position;
    [SerializeField] int size;

    private GameObject[] floor;
    private int floorActivHold;
    private int rtRCount;
    public enum FloorType
    {
        RightToLeft,
        RightToRight
    };
    public enum DirectionType
    {
        x_axis,
        z_axis,
        stair,
    }

    protected override void Start()
    {
        base.Start();

        rtRCount = 0;

        // FloorActivHold = size;
        for (int i = 0; i < size; i++)
        {
            GameObject Cube = Instantiate(floorObj, new Vector3(parentObj.transform.position.x + position, parentObj.transform.position.y, parentObj.transform.position.z), Quaternion.identity, parentObj.transform);
        }

        int childCount = parentObj.transform.childCount;
        floor=new GameObject[childCount];

        for(int i=0;i<floor.Length;i++)
        {
            floor[i] = parentObj.transform.GetChild(i).gameObject;
            floor[i].SetActive(false);
            switch (direction)
            {
                case DirectionType.x_axis:
                    if (i > 0)
                        floor[i].transform.position = new Vector3(floor[i - 1].transform.position.x + position, floor[i].transform.position.y, floor[i].transform.position.z);
                    else
                        floor[i].transform.position = new Vector3(parentObj.transform.position.x, parentObj.transform.position.y, floor[i].transform.position.z);
                    break;
                case DirectionType.z_axis:
                    if (i > 0)
                        floor[i].transform.position = new Vector3(parentObj.transform.position.x, parentObj.transform.position.y, floor[i - 1].transform.position.z + position);
                    else
                        floor[i].transform.position = new Vector3(parentObj.transform.position.x, parentObj.transform.position.y, floor[i].transform.position.z);
                    break;
                case DirectionType.stair:
                    if (i > 0)
                        floor[i].transform.position = new Vector3(parentObj.transform.position.x, floor[i - 1].transform.position.y - position, floor[i - 1].transform.position.z - position);
                    else
                        floor[i].transform.position = new Vector3(parentObj.transform.position.x, parentObj.transform.position.y, floor[i].transform.position.z);
                    break;
            }
        }
    }

    // Update is called once per frame
   protected override void Update()
    {
       base.Update();
    }

    protected override void Current()
    {
        for (int i = 0; i < floor.Length; i++)
        {
            if (floor[i].activeSelf == true)
            {
                i++;
            }
        }
        if (floorType_ == FloorType.RightToLeft)
        {
            if (floorActivHold < floor.Length)
            {
                StartCoroutine("ActivCube");
            }
        }
        if (floorType_ == FloorType.RightToRight)
        {
            if (floorActivHold < floor.Length)
            {
                StartCoroutine("ActivCube");
            }
            
        }
    }
    protected override void CurrentOff()
    {
        if (floorType_ == FloorType.RightToLeft)
        {
            if (floorActivHold > 0)
            {
                StartCoroutine("FalseCube", activeTime);
            }
        }
        if (floorType_ == FloorType.RightToRight)
        {
            if (floorActivHold > 0)
            {
                StartCoroutine("FalseCube2", activeTime);
            }
        }
    }

    IEnumerator ActivCube()
    {
        
        for (int i = floorActivHold; i < floor.Length; i++, floorActivHold = i)
        {
            yield return new WaitForSeconds(activeTime);
            if (AllCount==false)
            {
                break;
            }
            floor[i].SetActive(true);
        }
    }
    IEnumerator FalseCube()
    {
        for (int j = floorActivHold-1 ; j >= 0; j--)
        {
            if (AllCount == true)
            {
                break;
            }
            yield return new WaitForSeconds(activeTime);
            if (AllCount == true)
            {
                break;
            }
            floor[j].SetActive(false);
            floorActivHold = j;
        }
    }

    IEnumerator FalseCube2()
    {
        rtRCount = 0;
        for (int j = 0; j < floorActivHold; j++)
        {
            if (AllCount == true)
            {
                break;
            }
            yield return new WaitForSeconds(activeTime);
            if (AllCount == true)
            {
                break;
            }
            floor[j].SetActive(false);
        }
        if (rtRCount < 1)
        {
            floorActivHold = 0;
            rtRCount++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Vector4(1,0,0,0.3f);
        for (int i = 0; i < size; i++)
        {
            switch (direction)
            {
                case DirectionType.x_axis:
                    Gizmos.DrawCube(new Vector3(parentObj.transform.position.x + (i * position), parentObj.transform.position.y, parentObj.transform.position.z), floorObj.transform.localScale);
                    break;
                case DirectionType.z_axis:
                    Gizmos.DrawCube(new Vector3(parentObj.transform.position.x, parentObj.transform.position.y, parentObj.transform.position.z + (i * position)), floorObj.transform.localScale);
                    break;
                case DirectionType.stair:
                    Gizmos.DrawCube(new Vector3(parentObj.transform.position.x, parentObj.transform.position.y - (i * position), parentObj.transform.position.z - (i * position)), floorObj.transform.localScale);
                    break;
            }
        }
    }

    
}
