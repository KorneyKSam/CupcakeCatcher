using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RoofArea : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<ISurfaceInteractable>(out var surfaceInteractable))
            {
                surfaceInteractable.Interact();
            }
        }
    }
}