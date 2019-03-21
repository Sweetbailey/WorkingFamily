using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ZombieCharacterControl : MonoBehaviour
{
    private enum ControlMode
    {
        Tank,
        Direct
    }

    [SerializeField] private float m_moveSpeed = 1;
    [SerializeField] private float m_turnSpeed = 200;

    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Tank;

    [SerializeField] private Transform tf;
    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;

    private Vector3 m_currentDirection = Vector3.zero;

    private float coins = 25f;
    private float ingots = 100f;
    private float temp = 0;

    public Text moneyText;
    public Text bodyText;
    public Text sizeText;
    public Text eatText;
    public Text HText;
    public ParticleSystem blood;
    public ParticleSystem specialBlood;
    private bool canEatOpponent = false;
    void Awake()
    {
        if(!m_animator) { gameObject.GetComponent<Animator>(); }
        if(!m_rigidBody) { gameObject.GetComponent<Animator>(); }
        PlayerStats.InitializeP1Body();
        PlayerStats.InitializeP1Money();
        PlayerStats.InitializeP1Size();
        tf = GetComponent<Transform>();
        moneyText.text = "P1 Money: " + PlayerStats.p1Money.ToString("F2");
        bodyText.text = "P1 Body Count: " + PlayerStats.p1BodyCount;
        sizeText.text = "P1 Size: " + PlayerStats.p1SizeRatio.ToString("F2");
        eatText.enabled = false;
        HText.enabled = false;

        PlayerStats.InitializeBgm();
        Cursor.lockState = CursorLockMode.Locked;

    }

    void FixedUpdate ()
    {
        moneyText.text = "P1 Money: " + PlayerStats.p1Money.ToString("F2");
        bodyText.text = "P1 Body Count: " + PlayerStats.p1BodyCount;
        sizeText.text = "P1 Size: " + PlayerStats.p1SizeRatio.ToString("F2");
        m_moveSpeed = PlayerStats.p1SizeRatio;
        if (Input.GetKeyUp(KeyCode.H))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene("Menu");
        }
        if(PlayerStats.p1SizeRatio >= 1.9f)
        {
            eatText.enabled = true;
            canEatOpponent = true;

            if (PlayerStats.bgm == false){
            }
            else
            {
                PlayerStats.bgm = false;
            }
        }

        switch (m_controlMode)
        {
            case ControlMode.Direct:
                DirectUpdate();
                break;

            case ControlMode.Tank:
                TankUpdate();
                break;

            default:
                Debug.LogError("Unsupported state");
                break;
        }
    }

    private void TankUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

        m_animator.SetFloat("MoveSpeed", m_currentV);
    }

    private void DirectUpdate()
    {
        float v = Input.GetAxis("P1Vertical");
        float h = Input.GetAxis("P1Horizontal");

        Transform camera = Camera.main.transform;

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if(direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Human")
        {
            Debug.Log("Yorf");
            PlayerStats.scream = true;
            PlayerStats.p1BodyCount += 1f;
            PlayerStats.p1Money += 50f;
            PlayerStats.p1SizeRatio = PlayerStats.p1Money / 1000f + 1f;
            tf.transform.localScale = new Vector3(PlayerStats.p1SizeRatio, PlayerStats.p1SizeRatio, PlayerStats.p1SizeRatio);
            Destroy(collision.gameObject);
            blood.transform.position = tf.transform.position;
            blood.Play();
        }
        if(collision.gameObject.tag == "Player2" && canEatOpponent)
        {
            Debug.Log("Eating opponent");
            PlayerStats.p1BodyCount += 1f;
            PlayerStats.p1SizeRatio = PlayerStats.p1Money / 1000f + 1.5f;
            tf.transform.localScale = new Vector3(PlayerStats.p1SizeRatio, PlayerStats.p1SizeRatio, PlayerStats.p1SizeRatio);
            Destroy(collision.gameObject);
            specialBlood.transform.position = tf.transform.position;
            specialBlood.Play();
            HText.enabled = true;

        }

    }
   
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "GoldCoins")
        {
            PlayerStats.p1Money += (.1f * PlayerStats.p1SizeRatio);
            temp++;
            PlayerStats.collecting = true;
            PlayerStats.p1SizeRatio = PlayerStats.p1Money / 1000f + 1;
            tf.transform.localScale = tf.transform.localScale =
                new Vector3(PlayerStats.p1SizeRatio, PlayerStats.p1SizeRatio, PlayerStats.p1SizeRatio);
            if (temp >= coins)
            {
                temp = 0;
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "GoldIngots")
        {
            PlayerStats.p1Money += (1f * PlayerStats.p1SizeRatio);
            temp++;
            PlayerStats.collecting = true;

            PlayerStats.p1SizeRatio = PlayerStats.p1Money / 1000f + 1;
            tf.transform.localScale = tf.transform.localScale =
                new Vector3(PlayerStats.p1SizeRatio, PlayerStats.p1SizeRatio, PlayerStats.p1SizeRatio);
            if (temp >= ingots)
            {
                temp = 0;
                Destroy(other.gameObject);
            }

        }
    }
}
