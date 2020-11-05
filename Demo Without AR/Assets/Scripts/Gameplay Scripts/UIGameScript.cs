using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGameScript : MonoBehaviour
{
    #region Singleton
    public static UIGameScript instance;
    void Awake()
    {
        instance = this;
    }
    #endregion
    public Camera cam;

    public GameObject PlayerTroop;
    public GameObject Player2Troop;

    public TextMeshProUGUI PlayerUI;
    public TextMeshProUGUI Player2UI;
    public TextMeshProUGUI MatchTimerUI;
    public TextMeshProUGUI ScoreText;
    private int matchGame = 0;

    public GameObject timeUpPanel;
    public Image TimeUpClock;
    public Button ResetBtn;
    private bool runOne = false;
    private bool resetOne = false;

    public int p1Score = 0;
    public int p2Score = 0;

    private int P1MatchScore = 0;
    private int P2MatchScore = 0;

    private string m_PlayerInfo;
    private string m_Player2Info;
    public string RoleP1;
    public string RoleP2;
    private float TimeCountdown = 222;


    public Slider PlayerSlider;
    public Slider Player2Slider;
    public Slider Player2SliderNormalize;
    public Slider PlayerSliderNormalize;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSlider.value = 6;
        Player2Slider.value = 6;
        matchGame = PlayerPrefs.GetInt("Match Game");
        P1MatchScore = PlayerPrefs.GetInt("P1 Point");
        P2MatchScore = PlayerPrefs.GetInt("P2 Point");
        Debug.Log(matchGame);


        if(matchGame%2 == 0)
        {
            RoleP1 = "(Attacker)";
            RoleP2 = "(Defender)";
        }else if (matchGame % 2 == 1)
        {
            RoleP1 = "(Defender)";
            RoleP2 = "(Attacker)";
        }
        m_PlayerInfo = "P1 " + RoleP1;
        m_Player2Info = "P2 " + RoleP2;


        if (P1MatchScore >= 5)
        {
            Debug.Log("Game Over: " + "Player 1 Win");
            P1MatchScore = 0;
            P2MatchScore = 0;
            matchGame = 0;
            PlayerPrefs.SetInt("P1 Point", P1MatchScore);
            PlayerPrefs.SetInt("P2 Point", P2MatchScore);
            PlayerPrefs.SetInt("Match Game", matchGame);
        }
        else if (P2MatchScore >= 5)
        {
            Debug.Log("Game Over: " + "Player 2 Win");
            P1MatchScore = 0;
            P2MatchScore = 0;
            matchGame = 0;
            PlayerPrefs.SetInt("P1 Point", P1MatchScore);
            PlayerPrefs.SetInt("P2 Point", P2MatchScore);
            PlayerPrefs.SetInt("Match Game", matchGame);
        }    
    }
    
            
    
    // Update is called once per frame
    void Update()
    {
        if (TimeCountdown > 0)
        {
            TimeCountdown -= Time.deltaTime;
        }
        else
        {
            if (!runOne)
            {
                Debug.LogError("Time up");
                StartCoroutine(TimeUpAnimation());
                runOne = true;
            }
        }
        ResetBtn.onClick.AddListener(RestartGame);
        MatchTimerUI.text = Mathf.Round(TimeCountdown).ToString();
        PlayerUI.text = m_PlayerInfo;
        Player2UI.text = m_Player2Info;
        ScoreCount();
        if(p1Score > 0)
        {
            P1MatchScore++;
            PlayerPrefs.SetInt("P1 Point", P1MatchScore);
            p1Score = 0;
            SceneManager.LoadScene("Gameloft gameplay");
            matchGame++;
            PlayerPrefs.SetInt("Match Game", matchGame);
            Debug.LogWarning(PlayerPrefs.GetInt("P1 Point"));
            Debug.LogWarning(P1MatchScore);
        }else if (p2Score > 0)
        {
            P2MatchScore++;
            PlayerPrefs.SetInt("P2 Point", P2MatchScore);
            matchGame++;
            PlayerPrefs.SetInt("Match Game", matchGame);
            p2Score = 0;
            SceneManager.LoadScene("Gameloft gameplay");
            P2MatchScore = PlayerPrefs.GetInt("P2 Point");
        }
        EnergyRegen();
        if (Input.GetMouseButtonDown(0))
        {
            SpawnTroop();
        }
    }

    private void EnergyRegen()
    {
        if (PlayerSlider.value < 6)
        {
            PlayerSlider.value += 0.5f * Time.deltaTime * 1;
            PlayerSliderNormalize.value = (int)PlayerSlider.value;
        }
        if (Player2Slider.value < 6)
        {
           Player2Slider.value += 0.5f * Time.deltaTime * 1;
           Player2SliderNormalize.value = (int)Player2Slider.value;
        }
    }

    private void SpawnTroop()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform != null)
            {
                Debug.LogWarning(hit.transform.gameObject.tag);
                Debug.LogWarning(hit.transform.gameObject.name);
                if(RoleP1 == "(Attacker)")
                {
                    if (hit.transform.gameObject.tag == "PlayerZone" && PlayerSlider.value >= 2)
                    {
                        Instantiate(PlayerTroop, hit.point, PlayerTroop.transform.rotation);
                        PlayerSlider.value -= 2;
                    }
                    else if (hit.transform.gameObject.tag == "Player2Zone" && Player2Slider.value >= 3)
                    {
                        Instantiate(Player2Troop, hit.point, Player2Troop.transform.rotation);
                        Player2Slider.value -= 3;
                    }
                }else if(RoleP1 == "(Defender)")
                {
                    if (hit.transform.gameObject.tag == "Player2Zone" && Player2Slider.value >= 2)
                    {
                        Instantiate(Player2Troop, hit.point, Player2Troop.transform.rotation);
                        Player2Slider.value -= 2;
                    }
                    else if (hit.transform.gameObject.tag == "PlayerZone" && PlayerSlider.value >= 3)
                    {
                        Instantiate(PlayerTroop, hit.point, PlayerTroop.transform.rotation);
                        PlayerSlider.value -= 3;
                    }
                }
            }
        }
    }

    public void RestartGame()
    {
        Debug.Log("Clicked");
        StopAllCoroutines();
        timeUpPanel.SetActive(false);
        if(!resetOne)
        {
            matchGame++;
            PlayerPrefs.SetInt("Match Game", matchGame);
            SceneManager.LoadScene("Gameloft gameplay");
            resetOne = true;
        }
    }

    private IEnumerator TimeUpAnimation()
    {
        timeUpPanel.SetActive(true);
        TimeUpClock.transform.localScale = new Vector3 (1,1,1);
        yield return new WaitForSeconds(0.8f);
        TimeUpClock.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        yield return new WaitForSeconds(0.8f);
        TimeUpClock.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(0.8f);
        TimeUpClock.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        yield return new WaitForSeconds(0.8f);
        TimeUpClock.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(0.8f);
        TimeUpClock.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        yield return new WaitForSeconds(0.8f);
        TimeUpClock.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(0.8f);
        TimeUpClock.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        yield return new WaitForSeconds(2f);
    }

    private void ScoreCount()
    {
        ScoreText.text = P1MatchScore.ToString() + " - " + P2MatchScore.ToString();
    }
}
