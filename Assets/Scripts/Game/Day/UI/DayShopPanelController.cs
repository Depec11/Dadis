using Unity.VisualScripting;
using UnityEngine;

public class DayShopPanelController : MonoBehaviour {
    public DayShopPanelView view;

    private void Awake()
    {
        if (!view) view = transform.GetComponent<DayShopPanelView>();
    }

    private void Start()
    {
        view.btnExit.onClick.AddListener(() =>
        {
            transform.gameObject.SetActive(false);
        });

        view.btnLeft.onClick.AddListener(() =>
        {
            Debug.Log("btnExit.onClick");
        });

        view.btnRight.onClick.AddListener(() =>
        {
            Debug.Log("btnExit.onClick");
        });

        view.btnConfirm.onClick.AddListener(() =>
        {
            Debug.Log("btnExit.onClick");
        });
    }
}
