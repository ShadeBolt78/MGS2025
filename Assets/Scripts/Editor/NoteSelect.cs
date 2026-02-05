using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Dropdown))]
public class NoteSelect : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(index =>
        {
            Debug.Log(index);
            switch (index)
            {
                case 0:
                    EditorManager.Instance.SelectNoteType("Tap");
                    break;
                case 1:
                    EditorManager.Instance.SelectNoteType("Hold");
                    break;
                case 2:
                    EditorManager.Instance.SelectNoteType("Dead");
                    break;
                default:
                    Debug.LogError("congratulations! this shouldn't be possible.");
                    break;
            }
        });
    }
}
