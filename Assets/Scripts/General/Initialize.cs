using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    private void Awake()
    {
        throw new NotImplementedException();
    #if UNITY_WEBGL
        Application.ExternalEval("console.warn = function(){}");
        Application.ExternalEval("console.log = function(){}");
        Application.ExternalEval("console.error = function(){}");
    #endif
    }
}
