using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerComboEmitter : MonoBehaviour
{
    [SerializeField] private PlayerPackage playerPackage;

    private Animator animator;
    const string ONGOING_COMBO = "OngoingCombo";

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void SetOngoingBoolFalse() // Animation event!!!!!
    {
        animator.SetBool(ONGOING_COMBO, false);
    }
}
