using System;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    private Animator _animator;

    public bool isInteractable = true;
    public bool isActivated = false;
    

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    
    
    public void Activate()
    {
        print("Lever pulled");
        _animator.Play("Closed");
    }

    public void Interact(Transform initiator)
    {
        Activate();
    }

    public void SetInteractable(bool canInteract)
    {
        isInteractable = canInteract;
    }
}
