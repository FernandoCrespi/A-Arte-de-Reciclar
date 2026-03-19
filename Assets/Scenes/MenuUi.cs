using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public GameObject config;

    public void AbrirConfig()
    {
        config.SetActive(true);
    }

    public void FecharConfig()
    {
        config.SetActive(false);
    }
}
