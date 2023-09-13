using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FishController : MonoBehaviour{
    [Header("Components")]
    [SerializeField] 
    private Fish fish;

    private void OnCollisionEnter(Collision other){
        
    }
}
