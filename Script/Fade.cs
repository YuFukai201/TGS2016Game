using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{



    [SerializeField]
	Image FadePanel;

    //ステージごとのステージ名UI。CommonUtility.StageNumでなんステージ化を拾う
	public Image[] StageName;

	public Image otu;

    System.Action m_CallBack;

    // Use this for initialization
    void Start()
    {

    }

    void FadeInEnd()
    {
    }

    void FadeOutEnd()
    {
        //第～幕を入れる
        //フラグ管理
        Debug.Log("Fade out end");
    }


    public void FadeStart(int mode, System.Action CallBack)
    {

        m_CallBack = CallBack;
		if (mode == 0) {
			//引数
			StartCoroutine (FadeOutAction(FadePanel));

		} else if (mode == 1) {
			StartCoroutine (FadeInAction (FadePanel));
		}
        //~幕のフェードイン
        else if (mode == 2) {
			StartCoroutine (FadeInStageName (StageName[CommonUtility.StageNum]));	
		}
        //~幕のフェードアウト
        else if (mode == 3) {
			StartCoroutine (FadeOutStageName (StageName[CommonUtility.StageNum]));
		}
		//お疲れ様表示
		else if (mode == 4) {
			StartCoroutine (Clear_UI_Fadein ());
		}

    }

   
	IEnumerator FadeOutAction(Image fpanel)
    {
        //2秒経つまで毎フレーム中断
        //yield return new WaitForSeconds(1.5f);
        while (fpanel.color.a > 0)
        {
            float _subData = 1.0f * Time.deltaTime;
            float _alpha = (float)fpanel.color.a - _subData;
            if (_alpha < 0)
                _alpha = 0;
            fpanel.color = new Color(fpanel.color.r, fpanel.color.g, fpanel.color.b, _alpha);
            
            yield return new WaitForSeconds(0.01f);
        }
        m_CallBack();

    }

	IEnumerator FadeOutStageName(Image fpanel)
	{
		while (fpanel.color.a > 0)
		{
			float _subData = 1.0f * Time.deltaTime;
			float _alpha = (float)fpanel.color.a - _subData;
			if (_alpha < 0)
				_alpha = 0;
			fpanel.color = new Color(fpanel.color.r, fpanel.color.g, fpanel.color.b, _alpha);

			yield return new WaitForSeconds(0.01f);
		}
		m_CallBack();

	}


    
	IEnumerator FadeInAction(Image fpanel)
    {
        // 1.5秒経つまで毎フレーム中断
        //yield return new WaitForSeconds(1.0f);
        while (fpanel.color.a < 1f)
        {
            float _subData = 1.0f * Time.deltaTime;
            float _alpha = (float)fpanel.color.a + _subData;
            if (_alpha > 1.0f)
                _alpha = 1.0f;
            fpanel.color = new Color(fpanel.color.r, fpanel.color.g, fpanel.color.b, _alpha);
            yield return new WaitForSeconds(0.01f);
        }
        m_CallBack();
    }

    
	IEnumerator FadeInStageName(Image fpanel)
    {
       
		//yield return new WaitForSeconds(3.0f);	
        while (fpanel.color.a < 1f)
        {
            float _subData = 1.0f * Time.deltaTime;
            float _alpha = (float)fpanel.color.a + _subData;
			if (_alpha > 1.0f) 
				_alpha = 1.0f;
			fpanel.color = new Color(fpanel.color.r, fpanel.color.g, fpanel.color.b, _alpha);
            yield return new WaitForSeconds(0.01f);
        }

        m_CallBack();
    }

	IEnumerator Clear_UI_Fadein()
	{

		otu.color = new Color (255, 255, 255, 255);

		yield return new WaitForSeconds (10f);
		CommonUtility.init ();
		SceneManager.LoadScene ("Title");
	}
}