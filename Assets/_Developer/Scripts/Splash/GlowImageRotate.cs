using UnityEngine;

public class GlowImageRotate : MonoBehaviour
{
    [SerializeField] float speed = 1;

    void Update()
    {
        if (this.gameObject.activeInHierarchy)
            transform.Rotate(transform.forward * speed * Time.deltaTime);
    }
}
