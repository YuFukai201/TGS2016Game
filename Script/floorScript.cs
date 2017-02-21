using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class floorScript : MonoBehaviour
{
    float n = 0.8f / 15f;

    
    public GameObject effect;
    GameObject effectclone;
    public GameObject light;
    List<GameObject> m_lightClone = new List<GameObject>();
    
    public GameObject player;

    public GameObject CrossHaier;
	private float CrossHaierScale = 0.5f;
    

    //プレイヤーの向きとlightの位置を入れてる
    Vector3 vec1, vec2;


    private float lightPosX;
    private float lightPosZ;
    float theta;
    //rightとeffectの消える時間
    public float DestroyTime = 5f;

	// SoundsControllの取得
	private SoundControll SoundObj;

    // Use this for initialization
    void Start()
    {
		SoundObj = GameObject.Find ("SoundsControll").GetComponent<SoundControll> ();
    }
    
    // Update is called once per frame
    
    void Update()
    {

       
        //Debug.Log(m_lightClone.Count);

              
        float minDistance =-1;

        var LCount = m_lightClone.Count;
        
         
        foreach (var lightC in m_lightClone)
        {
            
            if (lightC == true)
            {

                
                lightPosX = lightC.transform.position.x;
                lightPosZ = lightC.transform.position.z;

                
                //Debug.Log( System.Math.Abs( (PlayerPosY - lightPosX) -180 ));

                vec1 = new Vector3(lightPosX, 0, lightPosZ);
                
                //プレイヤーの向いてる方向の取得
                vec2 = new Vector3(player.transform.forward.x, player.transform.forward.y, player.transform.forward.z);
                
                //複数
                var checkFlag = false;
                var d = Vector3.Distance(vec1,vec2);
                if (minDistance < 1) minDistance = d;
                if (minDistance >= d)
                {
                    minDistance = d;
                    checkFlag = true;

                }
                if (!checkFlag) continue;
               // Debug.Log(minDistance);
                
                //角度の取得    内積取得
                theta = Mathf.Acos(Vector3.Dot(vec1, vec2) / (vec1.magnitude * vec2.magnitude)) * Mathf.Rad2Deg;                
                
                //拡大率
                // a = 基本倍率+n*(最大角度-theta) n=最大倍率/最大角度
                
               
                if (theta <= 15)
                {
                    CrossHaier.transform.localScale = Vector3.one * CrossHaierScale * (0.5f + n * (15 - theta + 1));
                    if (LCount >= 1)
                    {
                        //光を数えてその数かける0.1倍率。lightCがdestroyしたら要素数-1
						CrossHaier.transform.localScale = Vector3.one * CrossHaierScale * (0.5f + n * (15 - theta + 1) + LCount * 0.1f);
                        
                    }
                }

                if (theta >= 15)
                {
                    CrossHaier.transform.localScale =Vector3.one * CrossHaierScale;
                }

               
            }
            /*
            if (lightC == false)
            {
                CrossHaier.transform.localScale = Vector3.one * 0.5f;
            }
              */
        }
	
    }
    

    void OnCollisionEnter(Collision col)
    {
        
        if (col.gameObject.tag == ("bullet"))
        {

          
            //エフェクト生成。指定、位置指定、回転は標準。 位置はbulletがぶつかった場所
            effectclone = GameObject.Instantiate(effect, col.gameObject.transform.position, Quaternion.Euler(-90,0,0)) as GameObject;


            //光源生成。エフェクトと同じ
            var lightC = GameObject.Instantiate(light, col.gameObject.transform.position, Quaternion.identity) as GameObject;
            //要素追加
            m_lightClone.Add(lightC);
           //レティクルの初期スケール
            CrossHaier.transform.localScale = Vector3.one * CrossHaierScale;
           
           
            //光源削除。要素も削除
            StartCoroutine(LightDestroy(lightC, DestroyTime));
//            Destroy(lightC, 5f);
            
			// 着弾した時のSE
			SoundObj.Fire_Play();

            //弾削除
            Destroy(col.gameObject);
            //エフェクト削除
            Destroy(effectclone, DestroyTime);


        }

    }

    //要素を消す
    IEnumerator LightDestroy(GameObject gobj, float timer)
    {
        yield return new WaitForSeconds(timer);
        m_lightClone.Remove(gobj);
        Destroy(gobj);
        CrossHaier.transform.localScale = Vector3.one * CrossHaierScale;
    }
}
