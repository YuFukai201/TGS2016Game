using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{

	#region インスペクターフィールド
	[System.SerializableAttribute]
	public class ValueList
	{
		public class ValueParam
		{
			public Transform generatePos;
			public float enemyPopTime;
			public ValueParam(Transform trans, float time)
			{
				generatePos = trans;
				enemyPopTime = time;
			}
		}

		 public List<Transform> m_genaratePos = new List<Transform>();
		public List<float> m_generateTimer = new List<float>();

		public ValueList(List<Transform> vpos, List<float> vtime)
		{
			m_genaratePos = vpos;
			m_generateTimer = vtime;
		}
	}
   
    
	//Inspectorに表示される
	[SerializeField]
	private List<ValueList> m_generateObj = new List<ValueList>();

	#endregion
	

	#region データフィールド

	/// <summary>
	/// 敵を配置する親オブジェクト
	/// </summary>
	public GameObject m_Parents;

	/// <summary>
	/// 経過時間タイマー
	/// </summary>
	float Count_Timer = 0;

	/// <summary>
	/// 敵出現チェックに使うカウンター
	/// </summary>
	static public int EnemyAppearCount = 0;

	//	構造体 publicにしないと怒られる（不安定）
	public struct EnemyType
	{
		public string EnemyName;		//	名前
		public int EnemyNum;			//	ナンバー
		public Vector3 pos;				//	ポジション
		public float time;				//	時間
		public bool dead;				//	フラグ

		//リストが参照にする
		public EnemyType(string EneName, int EneNum, Vector3 pos, float time ,bool dead )
		{
			this.EnemyName = EneName;
			this.EnemyNum = EneNum;
			this.pos = pos;
			this.time = time;
			this.dead = false;
		}
	};

	/// <summary>
	/// 敵のデータを保持する場所
	/// </summary>
	static public List<EnemyType> m_enemy  = new List<EnemyType>();

	/// <summary>
	/// メインが初期化されると true になる
	/// </summary>
	public bool isAction = false;

	bool isClearFlag = true;
    //第一幕表示用
	private bool isFadeOneFlag = true;

	//test用
	//    CommonUtility.m_Score m_Score;
	#endregion


	#region プロパティ

	string	m_EnemyName = "";
   
	public  string SearchEnemyName{ get { return m_EnemyName; } set { m_EnemyName = value; } }
    ValueList val;

	#endregion

	#region 初期化

	// ゲームクリアのフラグ(第三幕が終わった後) 
	bool isGameClear_Flag = false;
	// BGM_Controlオブジェクトの取得
	private BGMControll BGMObj;
	// SoundControlオブジェクトの取得
	private SoundControll SoundObj;

	void Start()
	{
		Count_Timer = 0;						///	タイマー初期化
		isAction = true;						///	メインの実態生成ON
		///	ゲームメインをグローバルに登録する
		CommonUtility.GameMain = this;
        if (isFadeOneFlag)
        {
            Fade m_Fade = GetComponent<Fade>();
			m_Fade.FadeStart(2, Stage1_Fade_out);
        }
		m_enemy.Clear ();
	
		BGMObj = GameObject.Find ("BGM_Control").GetComponent<BGMControll> ();
		SoundObj = GameObject.Find ("SoundsControll").GetComponent<SoundControll> ();
	}
		
	#endregion



	#region 毎フレームの更新

	// Update is called once per frame
	void Update()
	{
        
		///	動作開始
		if (isAction)
		{
           
			//時間計測
			Count_Timer += Time.deltaTime;
            //ここで倒されたかどうか毎フレームチェック
            StageMove();

			///	設定時間がある限り発生チェックを行う mganerateObjじゃなくposにしたい
			if (EnemyAppearCount < m_generateObj[CommonUtility.StageNum].m_genaratePos.Count) {
                //何ステージ目の何番湧き地か
				var _paraPos = m_generateObj [CommonUtility.StageNum].m_genaratePos [EnemyAppearCount];
				var _paraTime = m_generateObj [CommonUtility.StageNum].m_generateTimer [EnemyAppearCount];
				//カウントタイマーがポップタイム以上だったら
				if (Count_Timer >= _paraTime) {
					//	敵の情報生成
					var _enemy = new EnemyType ("Ene" + EnemyAppearCount.ToString ("D2"), EnemyAppearCount, _paraPos.position, _paraTime, false);
					//リストに追加、名前付け、型、ポジション、ポップタイム、出現したかどうか
					m_enemy.Add(_enemy);
					CreateEnemy(_enemy);
					EnemyAppearCount++;
					//Debug.Log (ValueList.m_genaratePos.Count);
                   
				}
			}
          
		}

		if (CommonUtility.m_gameover_Flag) {
			//一回湧きを止める
			isAction = false;
			//敵タイマー初期化
			Count_Timer = 0;
			//敵の数リセット
			EnemyAppearCount = 0;
			//デッドカウントリセット
			CommonUtility.DeadCount = 0;
			m_enemy.Clear ();
		}


	}

	#endregion



	#region メソッド

	/// <summary>
	/// 敵の生成
	/// </summary>
	/// <param name="enemy">Enemy.</param>
	public void CreateEnemy(EnemyType enemy)
	{
		///	生成する敵を指定
		var Enemy = Instantiate(Resources.Load("Prefabs/Enemy/ogre")) as GameObject;

		Enemy.GetComponent<EnemyScript> ().Init (gameObject.GetComponent<scoreScript> ());
		//	生成したゲームオブジェクトの名前を吐き出す
		Enemy.GetComponent<RenderEnemy>().EnemySerchCallback = EnemySerchCheck;
		//	構造体からデータの取得
		Enemy.name = enemy.EnemyName;
		//	親オブジェクト指定
		Enemy.transform.parent = m_Parents.transform;
		//	生成位置 配列から取得
		Enemy.transform.localPosition = enemy.pos;

		//	Dictionaryに追加
		//CommonUtility.m_EnemyDictionary.Add(Enemy.name, Enemy);
	}

	/// <summary>
	/// 敵の消去
	/// </summary>
	/// <param name="name">Name.</param>
	public void EnemyDestroy(string name)
	{
		if (CommonUtility.m_EnemyDictionary.ContainsKey(name))
		{
			//ディクショナリから名前削除
			CommonUtility.m_EnemyDictionary.Remove(name);
		}
	}

	/// <summary>
	/// 敵の名前を表示する
	/// </summary>
	/// <param name="enemyName">Enemy name.</param>
	void EnemySerchCheck(string enemyName)
	{
		
	}
    
    //ステージ内の敵が全員倒されたらステージ移動の処理
    //フェードアウト
    void StageMove()
    {
       if (m_generateObj[CommonUtility.StageNum].m_genaratePos.Count <= CommonUtility.DeadCount)
		{
			
            if (isClearFlag)
            {
                isClearFlag = false;
                //暗転
                //fade処理
                Fade m_Fade = GetComponent<Fade>();
                m_Fade.FadeStart(1, NextGameSceneInit);

				SoundObj.stageClear_Play ();
				Debug.Log ("StageClear");
            }
        }
    }
    //フェードイン　内部数値の初期化
    void NextGameSceneInit()
    {
        //ステージ移動
        CommonUtility.StageNum++;
        //一回湧きを止める
        isAction = false;
        
        //敵タイマー初期化
        Count_Timer = 0;
        //敵の数リセット
        EnemyAppearCount = 0;
        //デッドカウントリセット
        CommonUtility.DeadCount = 0;
        

        //フェードイン
        Fade m_Fade = GetComponent<Fade>();
        m_Fade.FadeStart(0, StageNameImage);

		//リザルト画面へ gameclear
        if (CommonUtility.StageNum >= 3)
        {
			m_Fade.FadeStart(4, GameClear);
            Debug.Log("Clear");  
		}
    }
    //～幕を表示
    void StageNameImage()
    {
        Fade m_Fade = GetComponent<Fade>();
        //～幕フェードイン
        m_Fade.FadeStart(2, reStartGameScene);
       
		SoundObj.StageStart_Play ();
    }
    
     //止めた処理を再開と ～幕のフェードアウト
    void reStartGameScene()
    {
        //～幕フェードアウト　
        Fade m_Fade = GetComponent<Fade>();
        m_Fade.FadeStart(3, test);
        //～幕が消えたら処理再開
    }

    void test()
    {
       // Application.LoadLevel("ResultScene");
        Debug.Log("finish");
        isAction = true;
		isClearFlag = true;
    }

	void GameClear()
	{
		// isGameClear_Flag = false;
		// gameClear_Flag = true;
		//titleに飛ばす
		//SceneManager.LoadScene ("tutorial");

		// GameClear_BGMの呼び出し
		BGMObj.GameClear_BGM ();
	}
  
	void Stage1_Fade_out()
	{
		Fade m_Fade = GetComponent<Fade>();
		m_Fade.FadeStart(3, empty);
		isFadeOneFlag = false;
	}
	void empty()
	{
		
	}
	#endregion

}

