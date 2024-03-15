using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingBehavior : MonoBehaviour
{
    public Animator anim;
    public Image loadingBar;

    private void Awake()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        anim = GetComponent<Animator>();
        showLoadingBar();
    }

    public void showLoadingBar() 
    {
        anim.SetBool("show",true);
    }

    public void hideLoadingBar()
    {
        anim.SetBool("show", false);
    }

    void Start()
    {
        StartCoroutine(FillLoadingBar());
    }

    IEnumerator FillLoadingBar()
    {
        yield return new WaitForSeconds(4f);

        hideLoadingBar();

        yield return new WaitForSeconds(2f);
        Destroy(transform.parent.gameObject);
    }
}
