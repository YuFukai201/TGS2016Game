using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

  
    public GameObject effect;
    GameObject effectclone;

    public float MoveSpeed;
    public float rotateSpeed = 0.3f;
   
    private Transform target;

    private Animator animator;

    bool isActive = false;

    Vector3 pos;

    public int EnemyHitPoint;

	scoreScript m_ScoreScript;

	EnemySEControl m_EnemySE;

	public GameObject Onibi;

	GameController gc;

	float Sink_Count;
	bool onibi_sink_flag = true;

    void Start ()
	{    
		//プレイヤー
		//3DオブジェクトならTargetに変えれば浮かない
		target = GameObject.FindWithTag ("Player").transform;
		target.position = new Vector3 (target.transform.position.x, transform.position.y, target.transform.position.z);
		gc = GameObject.Find ("fukai").GetComponent<GameController> ();
		animator = GetComponent<Animator> ();


		isActive = true;

		
		m_EnemySE = GameObject.Find("EnemySE").GetComponent<EnemySEControl> ();
	}

	// Update is called once per frame
	void Update () 
    {
		if (isActive) 
		{
			
			//実行
            float _discance = Vector3.Distance(target.transform.position, transform.position);
			MoveTowordsPlayer (_discance);
			if (transform.position.y < -6) 
			{
				if (onibi_sink_flag) {
					Onibi.transform.position = new Vector3 (Onibi.transform.position.x, Onibi.transform.position.y + 4, Onibi.transform.position.z);
					onibi_sink_flag = false;
				}
			}
			else if (transform.position.y > -0.1) {

				if (!onibi_sink_flag) {
					Onibi.transform.position = new Vector3 (Onibi.transform.position.x, Onibi.transform.position.y - 4, Onibi.transform.position.z);
					onibi_sink_flag = true;
				}
			}

		}     
    }


	public void Init(scoreScript score_script)
	{
		m_ScoreScript = score_script;
	}

	/// <summary>
	/// 当たり判定
	/// </summary>
	/// <param name="col">Col.</param>
    void OnCollisionEnter(Collision col)
    {
        //unityのバグの回避
		if (isActive)
        {
			///	相手が弾ならば処理する
			if (col.gameObject.tag == ("bullet"))
			{
                //弾削除
                Destroy(col.gameObject);
                EnemyHitPoint--;
				m_EnemySE.m_EnemyHit ();
                //ヒットポイントが0になったら以下の処理
                if (EnemyHitPoint == 0)
                {
                    //二度とcollsionが通らないように
                    isActive = false;

					if(CommonUtility.isStart)
                    gc.EnemyDestroy(gameObject.name);
                    //Debug.Log("!-------- name = " + gameObject.name);

                    //エフェクト生成。指定、位置指定、回転は標準。 位置はbulletがぶつかった場所
                    effectclone = GameObject.Instantiate(effect, col.gameObject.transform.position, Quaternion.identity) as GameObject;

                    float _discance = Vector3.Distance(target.transform.position, transform.position);
                    m_ScoreScript.EnemyDeadAddCount(_discance);

                    //DeadCountへ++
                    CommonUtility.DeadCount++;

                    //弾削除
                    Destroy(col.gameObject);

                    //エフェクト削除
                    Destroy(effectclone, 1f);

                    //エネミーを削除
                    Destroy(gameObject, 0.3f);
                }

			}
		}
	}

    void MoveTowordsPlayer(float distance)
    {
		if (!CommonUtility.m_sinkFlag && !CommonUtility.m_gameover_Flag) {
			Sink_Count = 0;
			onibi_sink_flag = true;
			//境界線(仮)
			if (distance <= 10f && !CommonUtility.m_gameover_Flag) {

				//プレイヤーの方向に向く
				transform.rotation = Quaternion.Slerp
                (transform.rotation, Quaternion.LookRotation (target.position - transform.position), rotateSpeed * Time.deltaTime);
				//向いた方向へ歩く
				transform.position += transform.forward * MoveSpeed * Time.deltaTime;
				Debug.Log ("ライン内");
			} else if (!CommonUtility.m_gameover_Flag) {
				//プレイヤーの方向に向く
				transform.rotation = Quaternion.Slerp
                (transform.rotation, Quaternion.LookRotation (target.position - transform.position), rotateSpeed * Time.deltaTime);

				//向いた方向へ歩く
				transform.position += transform.forward * MoveSpeed * Time.deltaTime;
			} 
			if (distance <= 4f) {
				// ゲームオーバー処理
				CommonUtility.m_gameover_Flag = true;
				//animation遷移はここで
				GetComponent<Animator> ().SetInteger ("OgreAttack", 1);

			}	} 
		else if (CommonUtility.m_sinkFlag && !CommonUtility.m_gameover_Flag) {
			
			Sink_Count += Time.deltaTime;
			if (this.transform.position.y <= -5) {
				if (Sink_Count <= 5f) {
					//プレイヤーの方向に向く
					transform.rotation = Quaternion.Slerp
					(transform.rotation, Quaternion.LookRotation (target.position - transform.position), rotateSpeed * Time.deltaTime);
				}
				//向いた方向へ歩く
				transform.position += transform.forward * MoveSpeed * Time.deltaTime;
			}
			if (distance <= 8.2f) {
				// ゲームオーバー処理
				CommonUtility.m_gameover_Flag = true;
				CommonUtility.m_sinkFlag = false;
				//animation遷移はここで
				GetComponent<Animator> ().SetInteger ("OgreAttack", 1);

			}
				
		}
    }
   
}
