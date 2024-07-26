using UnityEngine;

namespace SolidLib.Components;

using UnityEngine;

public class ExtendedEnemyAnimEvents : MonoBehaviour
{
    public ExtendedEnemyAI mainScript;

    public void PlayEventA()
    {
        mainScript.AnimationEventA();
    }

    public void PlayEventB()
    {
        mainScript.AnimationEventB();
    }
    
    public void PlayEventC()
    {
        mainScript.AnimationEventC();
    }
    
    public void PlayEventD()
    {
        mainScript.AnimationEventD();
    }
    
    public void PlayEventE()
    {
        mainScript.AnimationEventE();
    }
    
    public virtual void AnimationEventStringA(string value)
    {
        mainScript.AnimationEventStringA(value);
    }
        
    public virtual void AnimationEventStringB(string value)
    {
        mainScript.AnimationEventStringB(value);
    }
        
    public virtual void AnimationEventStringC(string value)
    {
        mainScript.AnimationEventStringC(value);
    }
        
    public virtual void AnimationEventFloatA(float value)
    {
        mainScript.AnimationEventFloatA(value);
    }
        
    public virtual void AnimationEventFloatB(float value)
    {
        mainScript.AnimationEventFloatB(value);
    }
        
    public virtual void AnimationEventFloatC(float value)
    {
        mainScript.AnimationEventFloatC(value);
    }
        
    public virtual void AnimationEventBoolA(bool value)
    {
        mainScript.AnimationEventBoolA(value);
    }
        
    public virtual void AnimationEventBoolB(bool value)
    {
        mainScript.AnimationEventBoolB(value);
    }
        
    public virtual void AnimationEventBoolC(bool value)
    {
        mainScript.AnimationEventBoolC(value);
    }

}
