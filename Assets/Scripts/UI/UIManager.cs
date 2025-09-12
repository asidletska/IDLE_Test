using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject missionsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject workerPanel;
    [SerializeField] private GameObject[] chef;
    [SerializeField] private GameObject[] waiter;
    [SerializeField] private GameObject[] fridge;
    [SerializeField] private GameObject[] stove;
    [SerializeField] private GameObject[] table;
    [SerializeField] private GameObject[] toilet;
    public void OnBackButtonHandler()
    {
        missionsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        shopPanel.SetActive(false);
        workerPanel.SetActive(false);
    }
    public void OnMissionsPanelActive()
    {
        missionsPanel.SetActive(true);
    }
    public void OnSettingsPanelActive()
    {
        settingsPanel.SetActive(true);
    }
    public void OnShopPanelActive()
    {
        shopPanel.SetActive(true);
    }
    public void OnWorkerPanelActive()
    {
        workerPanel.SetActive(true);
    }

}
