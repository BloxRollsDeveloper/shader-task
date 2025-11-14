using UnityEngine;

public interface IInteractable
{
    
    public void Interact(Transform initiator);
    
    public void SetInteractable(bool canInteract);
    
}
