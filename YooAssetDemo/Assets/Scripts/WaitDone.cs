using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitDone : IEnumerator
{
    public object Current => null;
    public bool isDone { get; set; }
    public WaitDone()
    {
        isDone = true;
    }

    public bool MoveNext()
    {
        return isDone;
    }

    public void Reset()
    {

    }


}
