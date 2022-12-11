using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro; //Has to be UGUI because its an UI object, TextMeshPro (no UGUI) is for world objects like the debug ones
    [SerializeField] private Button button;

    public void SetBaseAction(BaseAction baseAction) 
    {
        textMeshPro.text = baseAction.GetActionName().ToUpper(); //Set the text of the button 

        button.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }
    // Start is called before the first frame update
    
}



/* In the code above we did an anonymous function, which does the same as this (A delegate)
button.onClick.AddListener(MoveActionButtonClick);

private void MoveActionButtonClick()
{

}

*/