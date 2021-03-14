using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdditionalButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    private const string additionalText = "\n\nWhite enemies are more powerful, than grey. Some enemies won't aim at you, but some will.\n\nThe most durable armor is located in front. Armor can either reduce damage, or completely absorb shell. The bigger angle of hit, the less damage. When angle of hit is too big, ricochet is possible.\n\nUse your front armor to reduce and reflect enemies' shots. Use toolkits to recover from damage. Movement may help you to dodge shots from those enemies, which aim you.";
    public void OnClick()
    {
        text.SetText(text.text + additionalText);
        GetComponent<Button>().interactable = false;
    }
}
