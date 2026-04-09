using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour {
    [SerializeField] private Tool tool;
    public void UpdateUI(Tool tool) {
        this.tool = tool;
        Finder.FindObjectWithNameInChildren("ItemImage", gameObject).GetComponent<Image>().sprite = tool.UiIcon;
        gameObject.transform.GetComponentInChildren<TMPro.TMP_Text>().text = tool.UiOrder.ToString();

        var button = gameObject.GetComponent<Button>();

        
        button.colors = new ColorBlock()
        {
            normalColor = tool.UiColor,
            highlightedColor = tool.UiColor,
            pressedColor = tool.UiColor,
            selectedColor = tool.UiColor,
            disabledColor = Color.gray,
            colorMultiplier = 1
        };
        button.onClick.AddListener(() => {
            InventoryManager.Instance.CurrentTool = tool;
        });
    }
}