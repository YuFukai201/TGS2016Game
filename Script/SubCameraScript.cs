using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SubCameraScript : MonoBehaviour {
    //TargetIcon
	public Image CrossHaier;

    public Camera Subcam;
    

    void Start() 
    { 
		
    }
   
	// Update is called once per frame
	void Update () 
    {   
        
        //unityのcameraスクリプトリファレンスを参考に

        cameraView();

	}

    void cameraView()
    {
		var scale = CrossHaier.transform.lossyScale.x;
       
            //視界の変更。targetIconと同じに。
            Subcam.fieldOfView = 20 * scale;
		//レティクル内に敵が入ったら
		if (CommonUtility.GameMain.SearchEnemyName == "") 
		{
			CrossHaier.color= new Color (255, 255, 255,255);

		} else 
		{
			
			CrossHaier.color= new Color (255, 0, 0,255);
		}
    }

}
