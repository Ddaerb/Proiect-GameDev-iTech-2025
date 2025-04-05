using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // The radius within which the player can interact with objects
    private float _interactionRadius = 12f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _interactionRadius);
            foreach(Collider collider in colliders)
            {
                if (collider.CompareTag("NPC"))
                {
                    NPCInteraction.Instance.Speak();
                }
            }
            
        }
        
    }
}
