using UnityEngine;
using UnityEngine.UI;

public class TextureToggle : MonoBehaviour
{
    public Button toggleButton; // ���ڴ������Ǻ�ȡ�����ǲ����İ�ť
    public RawImage targetImage; // Ҫ�����������Ŀ��RawImage
    public RawImage sourceImage; // ԴRawImage�����������ǵ�Ŀ��RawImage��

    private bool isTextureOverlaid = false; // ��־��ǰ�Ƿ��Ѿ�����������
    private Texture originalTexture; // ���ڱ���Ŀ��RawImageԭʼ���������

    void Start()
    {
        originalTexture = targetImage.texture; // ����ԭʼ����
        toggleButton.onClick.AddListener(ToggleTexture); // Ϊ��ť����¼�ע�������
    }

    void ToggleTexture()
    {
        if (!isTextureOverlaid)
        {
            // �����ǰδ������������ԴRawImage�������ǵ�Ŀ��RawImage��
            targetImage.texture = sourceImage.texture;
            isTextureOverlaid = true;
        }
        else
        {
            // �����ǰ�Ѹ���������ָ���ԭʼ����
            targetImage.texture = originalTexture;
            isTextureOverlaid = false;
        }
    }
}
