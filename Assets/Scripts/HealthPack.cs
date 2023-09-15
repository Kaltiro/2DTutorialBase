using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{

    #region HealthPack variables
    [SerializeField]
    [Tooltip("For the amount of health healed")]
    private int healamount;
    #endregion

    #region heal functions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().Heal(healamount);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
