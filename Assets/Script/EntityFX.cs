using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Material hitMat;
    private Material originalMat;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }
    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(.2f);
        sr.material = originalMat;
    }
}
