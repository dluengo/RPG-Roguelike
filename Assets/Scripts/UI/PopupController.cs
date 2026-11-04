using UnityEngine;
using TMPro;

public class PopupController : MonoBehaviour
{
    // ---------- Members ----------
    private TMP_Text m_text;


    // ---------- Unity Methods ----------
    private void Awake()
    {
        m_text = GetComponentInChildren<TMP_Text>();
        Debug.Assert(m_text != null, "PopupController: No TMP_Text component found in children.");
    }


    // ---------- Public Methods ----------
    public void Setup(float duration, string text)
    {
        Destroy(gameObject, duration);
        m_text.text = text;
    }
}
