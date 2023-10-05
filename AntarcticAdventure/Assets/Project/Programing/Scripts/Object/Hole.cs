using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public sealed class Hole : MonoBehaviour, IComparable<Hole>{
    [field: SerializeField]
    public float distance { get; private set; }
    [field: SerializeField]
    public float offset { get; private set; }
    
    // PRIVATE MEMBERS
    private readonly UnityEvent WhenPlayerApprochead = new ();
    
    private bool hasApprochead;
    
    // MonoBehavior INTERFACE
    private void Awake(){
        var arr = GetComponentsInChildren<Interactable>();
        if (arr.Length != 0){
            foreach (var interactable in arr){
                WhenPlayerApprochead.AddListener(interactable.OnPlayerApprochead);
                Debug.Log(interactable.name + "'s interaction event has been added");
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            if (hasApprochead)
                return;
            
            WhenPlayerApprochead?.Invoke();
            hasApprochead = true;
        }
    }
    
    // DATA STRUCTURE
    public enum Type{
        small,
        big,
        fishSmall,
        sealBig
    }

    public int CompareTo(Hole other){
        return distance.CompareTo(other.distance);
    }
}
