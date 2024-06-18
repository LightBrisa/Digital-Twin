using UnityEngine;
using UnityEngine.UI;

public class TextureDisplay : MonoBehaviour
{
    public RawImage rawImage;   // ��Inspector����й���RawImage���
    public Texture2D displayTexture;  // ��Inspector����й���Ҫ��ʾ��Texture
    private bool isTextureVisible = false;

    void Start()
    {
        // ȷ��RawImage�����Button�������Ϊnull
        if (rawImage == null)
        {
            Debug.LogError("RawImage component not assigned!");
            return;
        }

        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found!");
            return;
        }

        // ��Ӱ�ť����¼�������
        button.onClick.AddListener(ToggleTextureDisplay);
    }

    void ToggleTextureDisplay()
    {
        // �л�����Ŀɼ���
        isTextureVisible = !isTextureVisible;

        // ���ݿɼ�������RawImage������
        if (isTextureVisible)
        {
            rawImage.texture = displayTexture;
        }
        else
        {
            rawImage.texture = null; // ��������
        }
    }
}
