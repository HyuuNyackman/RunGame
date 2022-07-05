using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public static bool FadeInstance = false;

    public bool IsFadeIn = false;
    public bool IsFageOut = false;

    public float Alpha = 0.0f;
    public float FadeSpeed = 0.2f;
    void Awake()
    {
        if (!FadeInstance)
        {
            DontDestroyOnLoad(gameObject);
            FadeInstance = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if(IsFadeIn)
        {
            Alpha -= Time.unscaledDeltaTime / FadeSpeed;
            if(Alpha<=0.0f)
            {
                IsFadeIn= false;
                Alpha=0.0f;
            }
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, Alpha);
        }
        else if(IsFageOut)
        {
            Alpha += Time.unscaledDeltaTime / FadeSpeed;
            if(Alpha>=1.0f)
            {
                IsFageOut=false;
                Alpha = 1.0f;
            }
            this.GetComponentInChildren<Image>().color=new Color(0.0f, 0.0f, 0.0f, Alpha);
        }
    }

    public void FadeIn()
    {
        IsFadeIn=true;
        IsFageOut=false;
    }
    public void FadeOut()
    {
        IsFageOut =true;
        IsFadeIn=false;
    }
}
