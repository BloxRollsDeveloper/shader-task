using System.Collections.Generic;
using UnityEngine;


    public class CollisionBox : MonoBehaviour
    {
        public bool isInCollision;
        private HashSet<Collider2D> collidingObjects = new HashSet<Collider2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Add the collider to the set
            collidingObjects.Add(other);
            
            // if It has IInteractable component, set interactable to true
            if (other.GetComponent<IInteractable>() != null)
            {
                other.GetComponent<IInteractable>().SetInteractable(true);
            }
        }
    

        void OnTriggerExit2D(Collider2D other)
        {
            // Remove the collider from the set
            collidingObjects.Remove(other);
            
            // if It has IInteractable component, set interactable to false
            if (other.GetComponent<IInteractable>() != null)
            {
                other.GetComponent<IInteractable>().SetInteractable(false);
            }
        }

        void Update()
        {
            // Check if any objects are still colliding
            if (collidingObjects.Count > 0)
            {
                isInCollision = true;
            }
            else
            {
                isInCollision = false;
            }
        }

        public void TryInteract()
        {
            foreach (var collidingObject in collidingObjects)
            {
                IInteractable interactable = collidingObject.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(transform);
                }
            }
        }


    }
