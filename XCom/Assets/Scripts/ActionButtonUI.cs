using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro; //Has to be UGUI because its an UI object, TextMeshPro (no UGUI) is for world objects like the debug ones
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction) 
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper(); //Set the text of the button 

        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }
    // Start is called before the first frame update

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction); //Set to active if this one is the same than the current one
    }
    
}



/* In the code above we did an anonymous function, which does the same as this (A delegate)
button.onClick.AddListener(MoveActionButtonClick);

private void MoveActionButtonClick()
{

}

*/