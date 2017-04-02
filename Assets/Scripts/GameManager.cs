using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum gameStatus {
	next, play, gameover, win
}
public class GameManager : Singleton<GameManager> {

	[SerializeField]
	private int totalWaves = 10;
	[SerializeField]
	private Text totalMoneyLbl;
	[SerializeField]
	private Text currentWaveLbl;
	[SerializeField]
	private Text totalEscapedLbl;
	[SerializeField]
	private GameObject spawnPoint;
	[SerializeField]
	private Enemy[] enemies;
	[SerializeField]
	private int totalEnemies = 3;
	[SerializeField]
	private Text playBtnLbl;
	[SerializeField]
	private Button playBtn;
	[SerializeField]
	private Button gameOverBanner;
	[SerializeField]
	private Text gameOverBannerLbl;

	private int waveNumber = 0;
	private int totalMoney = 10;
	private int totalEscaped = 0;
	private int roundEscaped = 0;
	private int totalKilled = 0;
	private int enemiesToSpawn = 0;
	private gameStatus currentState = gameStatus.play;
	private AudioSource audioSource;


	public int enemiesPerSpawn;
	public List<Enemy> EnemyList = new List<Enemy>();
	public List<Projectile> ProjectileList = new List<Projectile>();
	const float spawnDelay = 0.8f;

	public int TotalEscaped {
		get {
			return totalEscaped;
		}
		set {
			totalEscaped = value; 
		}
	}

	public int RoundEscaped {
		get {
			return roundEscaped;
		}

		set {
			roundEscaped = value;
		}
	}

	public int TotalKilled {
		get {
			return totalKilled;
		}

		set {
			totalKilled = value;
		}
	}

	public int TotalMoney {
		get {
			return totalMoney;
		}
		set {
			totalMoney = value;
			totalMoneyLbl.text = totalMoney.ToString();
		}
	}

	public AudioSource AudioSource {
		get {
			return audioSource;
		}
	}

	// Use this for initialization
	void Start () {
		// StartCoroutine(spawn());
		playBtn.gameObject.SetActive(false);
		gameOverBanner.gameObject.SetActive(false);
		audioSource = GetComponent<AudioSource>();
		showMenu();
	}

	void Update() {
		handleEscape();
	}
	
	IEnumerator spawn () {
		if(enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies) {
			for(int i = 0; i < enemiesPerSpawn; i++) {
				if(EnemyList.Count < totalEnemies) {
					//Instantiate only creates an object not a GameObject
					Enemy newEnemy = Instantiate(enemies[Random.Range(0, enemiesToSpawn)]) as Enemy;
					newEnemy.transform.position = spawnPoint.transform.position;
				}
			}
			yield return new WaitForSeconds(spawnDelay);
			StartCoroutine(spawn());
		}
	}

	public void RegisterEnemy(Enemy enemy) {
		EnemyList.Add(enemy);
	}	

	public void UnRegisterEnemy(Enemy enemy) {
		EnemyList.Remove(enemy);
		Destroy(enemy.gameObject);
	}

	public void DestroyAllEnemies() {
		foreach(Enemy enemy in EnemyList) {
			Destroy(enemy.gameObject);
		}
		EnemyList.Clear();
	}

	public void RegiserProjectile(Projectile projectile) {
		ProjectileList.Add(projectile);
	}

	public void UnRegisterProjectile(Projectile projectile) {
		ProjectileList.Remove(projectile);
		if(projectile != null) {
			Destroy(projectile.gameObject);
		}
	}

	public void DestroyAllProjectiles() {
		foreach(Projectile projectile in ProjectileList) {
			Destroy(projectile.gameObject);
		}
	}

	

	public void addMoney(int amount) {
		TotalMoney += amount;
	}

	public void subtractMoney(int amount) {
		TotalMoney -= amount;
	}

	public void isWaveOver() {
		totalEscapedLbl.text = "Escaped " + totalEscaped + "/10";
		if((roundEscaped + totalKilled) == totalEnemies) {
			if(waveNumber <= enemies.Length) {
				enemiesToSpawn = waveNumber;
			}
			setCurrentGameState();
			showMenu();
		}	
	}

	public void setCurrentGameState() {
		if(TotalEscaped >= 10) {
			currentState = gameStatus.gameover;
		} else if(waveNumber == 0 && (totalKilled + roundEscaped) == 0) {
			currentState = gameStatus.play;
		} else if(waveNumber >= totalWaves) {
			currentState = gameStatus.win;
		} else {
			currentState = gameStatus.next;
		}
	}

	public void showMenu() {
		switch(currentState) {
			case gameStatus.gameover:
				playBtnLbl.text = "Play Again!";
				gameOverBannerLbl.text = "Game Over!";
				gameOverBanner.gameObject.SetActive(true);
				AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
				break;
			case gameStatus.next:
				playBtnLbl.text = "Next Wave";
				break;
			case gameStatus.play:
				playBtnLbl.text = "Play";
				break;
			case gameStatus.win:
				gameOverBannerLbl.text = "You Win!";
				gameOverBanner.gameObject.SetActive(true);
				playBtnLbl.text = "Play";
				break;			
		}
		playBtn.gameObject.SetActive(true);
	}

	public void playBtnPressed() {
		switch(currentState) {
			case gameStatus.next:
				waveNumber +=1;
				totalEnemies += waveNumber;
				break;
			default:
				totalEnemies = 3;
				TotalEscaped = 0;
				TotalMoney = 10;
				enemiesToSpawn = 0;
				TowerManager.Instance.DestroyAllTowers();
				TowerManager.Instance.RenameTagBuildSites();
				totalMoneyLbl.text = TotalMoney.ToString();
				totalEscapedLbl.text = "Escaped " + totalEscaped + "/10";
				audioSource.PlayOneShot(SoundManager.Instance.NewGame);
				break;	
		}
		DestroyAllEnemies();
		DestroyAllProjectiles();
		TotalKilled = 0;
		RoundEscaped = 0;
		currentWaveLbl.text = "Wave " + (waveNumber +1);
		StartCoroutine(spawn());
		playBtn.gameObject.SetActive(false);
		gameOverBanner.gameObject.SetActive(false);
	}

	private void handleEscape() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			TowerManager.Instance.disableDragSprite();
			TowerManager.Instance.towerBtnPressed = null;
		}
	}
}
