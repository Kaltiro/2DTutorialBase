using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    #region GameObject_variables
    [SerializeField]
    [Tooltip("Healthpack")]
    private GameObject healthpack;
    #endregion

    #region Chest functions
    IEnumerator DestroyChest()
    {
        yield return new WaitForSeconds(.3f);
        Instantiate(healthpack, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void Interact()
    {
        StartCoroutine("DestroyChest");
    }
    #endregion
}
