using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class shot : MonoBehaviour {
    
    //弾
    public GameObject Bullet;
    GameObject BulletClone; 
    //弾数
    public float BulletCount = 10;
    //プレイヤー
    public Transform tip;
    //弾速
    public float speed = 500;
    //発射間隔
    private float time = 0f;
    public float interval = 100f;
    //オブジェクトが出現から消えるまで
    public float destroytime = 10f;

	//デバッグ用テキスト
	public  Text debug_Text;
    
    
    //着弾エフェクト
    public GameObject AttackEffect;

    bool isActive = false;
	// Use this for initialization
	void Start ()
	{
		isActive = true;
	}

	
	// Update is called once per frame
	public void Update ()
    {
		if (!CommonUtility.bulletLock) {
			if (isActive && CommonUtility.gameControl != null && CommonUtility.m_gameover_Flag == false) {
				time += Time.deltaTime;
				Debug.Log ("flagIs:" + CommonUtility.gameControl.IsBPress);

				if (CommonUtility.gameControl.IsBPress && CommonUtility.m_reload_Flag == false && CommonUtility.Remaining_bullets > 0) {
					shoot ();
					CommonUtility.Remaining_bullets--;
					var _fukai = GameObject.Find ("fukai") as GameObject;
					Bullet_Reload_UI m_bullet_reload = _fukai.GetComponent<Bullet_Reload_UI> ();
					m_bullet_reload.addBulletShot ();
				}
			//Reload
			else if (CommonUtility.gameControl.IsBPress && CommonUtility.m_reload_Flag) 
				{
					var _fukai = GameObject.Find ("fukai") as GameObject;
					Bullet_Reload_UI m_bullet_reload = _fukai.GetComponent<Bullet_Reload_UI> ();
					m_bullet_reload.AddBulletReload ();
				}
			}     
		}
    }
	
    void shoot()
    {
		if (!CommonUtility.isStart) {
			//Debug.Log (main._serchEnemyName);
			if (CommonUtility.GameMain.SearchEnemyName == "") {
				BulletClone = GameObject.Instantiate (Bullet, tip.position, tip.rotation) as GameObject;
				BulletClone.GetComponent<Rigidbody> ().AddForce (tip.forward * speed);
			} else {
           
				BulletClone = GameObject.Instantiate (Bullet, tip.position, tip.rotation) as GameObject;
				BulletClone.GetComponent<Rigidbody> ().AddForce (tip.forward * speed);
				BulletClone.GetComponent<Bullet> ().SetStates (GameObject.Find (CommonUtility.GameMain.SearchEnemyName), 3.0f, 5.0f);
			}
		} else if(CommonUtility.isStart){
			BulletClone = GameObject.Instantiate (Bullet, tip.position, tip.rotation) as GameObject;
			BulletClone.GetComponent<Rigidbody> ().AddForce (tip.forward * speed);
		}
    }
}

