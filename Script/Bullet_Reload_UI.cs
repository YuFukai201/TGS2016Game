using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Bullet_Reload_UI : MonoBehaviour {

	//残弾
	public Image[] BulletUI;
	//消費数
	public Image[] BulletTokenUI;
	//リロードに入ったかどうか
	//bool Reload = true;
	//だるまさんが転んだUI
	public Image[] Darumasan_UI;

	// Use this for initialization
	void Start () 
	{
		var _Canvas = GameObject.Find ("FlontCanvas") as GameObject;
		_Canvas.GetComponent<DarumaMojiControl> ().Init ();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}


	//だるまさんがころんだUI非表示処理
	IEnumerator DarumaUI_FadeOut()
	{
		var _alpha = 1.0f;
		while (_alpha > 0f) 
		{
			_alpha -= 1.0f * Time.deltaTime;
			if (_alpha <= 0.0f)
				_alpha = 0.0f;
			for(int j = 0; j < Darumasan_UI.Length; j++)
			{
				// Darumasan_UI [j].color = new Color (255, 255, 255, _alpha);
			}
			yield return new WaitForSeconds(0.01f);
		}

	}
	//残弾がない時
	public void AddBulletReload()
	{
		if (CommonUtility.m_reload_Flag) {
			//commonにあるbullet数によってuiのαをいじる　
			BulletUI [CommonUtility.Remaining_bullets].color = new Color (255, 255, 255, 255);
			BulletTokenUI [CommonUtility.Remaining_bullets].color = new Color (255, 255, 255, 0);

			///	文字表示
			var _Canvas = GameObject.Find ("FlontCanvas") as GameObject;
			_Canvas.GetComponent<DarumaMojiControl> ().AddMoji ();

			//Darumasan_UI [CommonUtility.Remaining_bullets].color = new Color (255, 255, 255, 255);
			CommonUtility.Remaining_bullets++;
			if (CommonUtility.Remaining_bullets == 10) {
					//CommonUtility.m_reload_Flag = false;
				StartCoroutine (DarumaUI_FadeOut ());
			}
		}
	}

	public void addBulletShot()
	{
		//残弾があるの時
		if (CommonUtility.m_reload_Flag == false) 
		{	//commonにあるbullet数によってuiのαをいじる　今はshot.csでbulletの数をいじっているけどあとで変える。
			BulletUI [CommonUtility.Remaining_bullets].color = new Color (255, 255, 255, 0);
			BulletTokenUI [CommonUtility.Remaining_bullets].color = new Color (255, 255, 255, 255);
				if (CommonUtility.Remaining_bullets == 0) 
				{
					//CommonUtility.m_reload_Flag = true;
				}

		}
	}





}
