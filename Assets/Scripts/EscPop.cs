using UnityEngine;

public class EscPop : MonoBehaviour
{
    public GameObject menuObject;
    public void Toggle()
    {
        menuObject.SetActive(!menuObject.activeSelf);
    }
    
}
