using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHit : MonoBehaviour
{
    public enum Current_Type
    {
        Hold,       // オンオフ
        Charged,    // 帯電
    };
    [SerializeField] Current_Type type;
    [SerializeField] bool currentBool;
    [SerializeField] Color colr;
    [SerializeField] float intensity;
    private Material mtl;
    private float maxintensity;
    private bool plusSwitchBool;
    private bool minusSwitchBool;
    [Header("仮")]
    [SerializeField] private bool comeOffBool;
    [SerializeField] float offSpeed;
    [Header("TypeがChargedの場合")]
    [SerializeField] GimmickManager gimmickManager;
    void Start()
    {
        plusSwitchBool = false;
        minusSwitchBool = true;
        maxintensity = intensity;
        currentBool = false;
        mtl = GetComponent<MeshRenderer>().material;
        mtl.SetColor("_Color", new Vector4(0,0,0,mtl.color.a) * Mathf.Pow(0, intensity));
        if(type == Current_Type.Charged && currentBool==false)
        {
            comeOffBool = false;
            intensity = -5;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBool)
        {

            Current_true();
            mtl.SetColor("_Color", colr * Mathf.Pow(2f, intensity));
            
        }
        if (!currentBool)
        {
            if (type == Current_Type.Hold)
            {
                mtl.SetColor("_Color", new Vector4(0, 0, 0, 0) * Mathf.Pow(0, intensity));
                plusSwitchBool = false;

                if (minusSwitchBool==false)
                {
                    gimmickManager.CurrentCount--;
                    minusSwitchBool = true;
                }
            }
            
        }
        if(comeOffBool==true)
        {
            if (type == Current_Type.Charged)
            {
                if (intensity > -9)
                {
                    intensity -= Time.deltaTime * offSpeed;
                }
                mtl.SetColor("_Color", colr * Mathf.Pow(2f, intensity));
            }
        }

        if (comeOffBool==true&&intensity < -5)
        {
            plusSwitchBool = false;
            gimmickManager.CurrentCount--;
            currentBool = false;
            comeOffBool=false;
        }
    }

    void Current_true()
    {
        if (plusSwitchBool != true)
        {
            intensity = maxintensity;
            plusSwitchBool = true;
            minusSwitchBool = false;
            gimmickManager.CurrentCount++;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Knife")
        {
            currentBool = true;
            comeOffBool = false;
            Current_true();
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Knife")
        {
            currentBool = true;
            comeOffBool = false;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (type == Current_Type.Hold)
        {
            if (col.gameObject.tag == "Knife")
            {
                currentBool = false;
            }
        }
        comeOffBool = true;
    }

    public bool Current
    {
        get { return currentBool; }
    }

}
