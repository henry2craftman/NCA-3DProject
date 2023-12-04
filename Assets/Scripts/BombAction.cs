using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// πŸ¥⁄ø° ∆¯≈∫¿Ã ¥Í¿∏∏È ¿Ã∆Â∆Æ∏¶ ∏∏µÈ∞Ì ªÁ∂Û¡¯¥Ÿ.
public class BombAction : MonoBehaviour
{
    [SerializeField] GameObject bombEffect;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effect = Instantiate(bombEffect);

        effect.transform.position = this.transform.position;

        Destroy(this.gameObject);
    }
}
