using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    protected readonly SamuelSaysModule _module;

    protected State(SamuelSaysModule module)
    {
        _module = module;
        _module.StartCoroutine(OnStateEnter());
    }

    public virtual IEnumerator OnStateEnter()
    {
        yield return null;
    }

    public virtual IEnumerator HandlePress(ColouredButton button)
    {
        yield return null;
    }

    public virtual IEnumerator HandleRelease(ColouredButton button)
    {
        yield return null;
    }

    public virtual IEnumerator HandleSubmitPress()
    {
        yield return null;
    }
}
