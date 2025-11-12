using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class NewBehaviourScript : MonoBehaviour
{
    public Image darkOverlay;
    public TMP_Text title;
    public float endTime = 1f;
    public RectTransform titleTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DarkenScreen()
    {
        darkOverlay.gameObject.SetActive(true);
        StartCoroutine(FadeIn(darkOverlay, 0.5f)); // 0.5s fade duration
        StartCoroutine(MoveTitle());
    }

    private IEnumerator FadeIn(Image img, float duration)
    {
        Color c = img.color;
        c.a = 0;
        img.color = c;

        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1f, t / duration);
            img.color = c;
            yield return null;
        }

        c.a = 1f;
        img.color = c;
    }

    private IEnumerator MoveTitle()
    {
        // Center Title
        Vector2 defaultPosition = titleTransform.anchoredPosition;
        Vector2 centerPosition = new Vector2(titleTransform.anchoredPosition.x, 0);
        float startTime = 0.0f;

        // Glow effect on Title
        Material material = title.fontMaterial;
        float defaultGlow = material.GetFloat(ShaderUtilities.ID_GlowPower);
        float endGlow = defaultGlow + 0.8f;

        while (startTime < endTime)
        {
            startTime += Time.deltaTime;
            titleTransform.anchoredPosition = Vector2.Lerp(defaultPosition, centerPosition, startTime / endTime);

            float increasedGlow = Mathf.Lerp(defaultGlow, endGlow, startTime / endTime);
            material.SetFloat(ShaderUtilities.ID_GlowPower, increasedGlow);

            yield return null;
        }

        titleTransform.anchoredPosition = centerPosition;
        material.SetFloat(ShaderUtilities.ID_GlowPower, endGlow);
    }
}
