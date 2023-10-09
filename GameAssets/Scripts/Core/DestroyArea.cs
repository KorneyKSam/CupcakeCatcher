using UnityEngine;

namespace Core
{
    public class DestroyArea : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.gameObject.GetComponent<IDestroyable>()?.Destroy();
        }
    }
}