using UnityEngine;

public class PanelAnim : MonoBehaviour
{
    public Animator animator;
    public GameObject panel;

    public void PlayButton()
    {
        animator.SetTrigger("Hide");
    }

    public void ShowPanel()
    {
        panel.SetActive(true);

        animator.Rebind();  
        animator.Update(0f);

        animator.Play("Panel_Show", 0, 0f);
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }
}