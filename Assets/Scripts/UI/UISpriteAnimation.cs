using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISpriteAnimation : MonoBehaviour
{

    public Image m_Image;

    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;

    private int m_IndexSprite;
    Coroutine m_CorotineAnim;
    bool IsDone;
    void Awake()
    {
        m_Image = GetComponent<Image>();
    }
    void OnEnable()
    {
        PlayUIAnim();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }
    }
    public void PlayUIAnim()
    {
        IsDone = false;
        StartCoroutine(nameof(PlayAnimUI));
    }

    public void StopUIAnim()
    {
        IsDone = true;
        StopCoroutine(nameof(PlayAnimUI));
    }
    IEnumerator PlayAnimUI()
    {
        while (!IsDone)
        {
            yield return new WaitForSeconds(m_Speed);
            if (m_IndexSprite >= m_SpriteArray.Length)
            {
                StopUIAnim();
                m_IndexSprite = m_SpriteArray.Length - 1;
            }
            m_Image.sprite = m_SpriteArray[m_IndexSprite];
            m_IndexSprite += 1;
            //if (IsDone == false)
            //    m_CorotineAnim = StartCoroutine(PlayAnimUI());
        }
    }
}