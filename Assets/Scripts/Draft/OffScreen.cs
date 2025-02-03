using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreen : MonoBehaviour
{
    void OnBecameInvisible()
{
    Destroy(gameObject);
}


}
