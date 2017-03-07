using UnityEngine;
using System.Collections;


public class RenderEnemy : MonoBehaviour {

    //カメラの視界に入ってるかどうかの判断

    //視界に入ってる敵の名前をとってる
	[HideInInspector]
	public System.Action<string> EnemySerchCallback;

  
    bool isActive = false;
    void Update()
    {
        if (isActive)
        {
			if (CommonUtility.GameMain != null) {
				CommonUtility.GameMain.SearchEnemyName = gameObject.name;
				EnemySerchCallback (gameObject.name);
			}
        }
        //リセット処理。バグあり
        else
        {
			if (CommonUtility.GameMain != null)
				CommonUtility.GameMain.SearchEnemyName = "";
        }
        isActive = false;
    }
    //	FildOfViewを10以下にすると受け付けない
    //	MeshRenderを入れないと反応しない
    void OnWillRenderObject()
    {
		//cardbordの下のsecondcameraのtag名
		if (Camera.current.tag == "subcamera")
        {
            isActive = true;
        }
    }

}
