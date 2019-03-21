using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class P2Control : MonoBehaviour
{
    private enum ControlMode
    {
        Tank,
        Direct
    }

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;

    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Tank;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;

    private Vector3 m_currentDirection = Vector3.zero;
    private float coins = 25f;
    private float ingots = 100f;
    private float temp = 0;
    [SerializeField] private Transform tf;
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
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }
        PlayerStats.InitializeP2Body();
        PlayerStats.InitializeP2Money();
        PlayerStats.InitializeP2Size();
        tf = GetComponent<Transform>();
        moneyText.text = "P2 Money: " + PlayerStats.p2Money.ToString("F2");
        bodyText.text = "P2 Body Count: " + PlayerStats.p2BodyCount;
        sizeText.text = "P2 Size: " + PlayerStats.p2SizeRatio.ToString("F2");
        eatText.enabled = false;
        HText.enabled = false;

    }

    void FixedUpdate()
    {
        moneyText.text = "P2 Money: " + PlayerStats.p2Money.ToString("F2");
        bodyText.text = "P2 Body Count: " + PlayerStats.p2BodyCount;
        sizeText.text = "P2 Size: " + PlayerStats.p2SizeRatio.ToString("F2");
        m_moveSpeed = PlayerStats.p2SizeRatio;

        if (PlayerStats.p2SizeRatio >= 1.9f)
        {
            eatText.enabled = true;
            canEatOpponent = true;

            if (PlayerStats.bgm == false)
            {
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
        float v = Input.GetAxis("P2Vertical");
        float h = Input.GetAxis("P2Horizontal");

        Transform camera = Camera.main.transform;

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Human")
        {
            PlayerStats.scream = true;
            Debug.Log("Yorf");
            PlayerStats.p2BodyCount++;
            PlayerStats.p2Money += 50f;
            PlayerStats.p2SizeRatio = PlayerStats.p2Money / 1000f + 1f;
            tf.transform.localScale = 
                new Vector3(PlayerStats.p2SizeRatio, PlayerStats.p2SizeRatio, PlayerStats.p2SizeRatio);
            Destroy(collision.gameObject);
            blood.transform.position = tf.transform.position;
            blood.Play();
        }
        if (collision.gameObject.tag == "Player1" && canEatOpponent)
        {
            Debug.Log("Eating opponent");
            PlayerStats.p2BodyCount += 1f;
            PlayerStats.p2SizeRatio = PlayerStats.p2Money / 1000f + 1.5f;
            tf.transform.localScale = 
                new Vector3(PlayerStats.p2SizeRatio, PlayerStats.p2SizeRatio, PlayerStats.p2SizeRatio);
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
            PlayerStats.p2Money += (.1f * PlayerStats.p2SizeRatio);
            temp++;
            PlayerStats.collecting = true;

            PlayerStats.p2SizeRatio = PlayerStats.p2Money / 1000f + 1;
            tf.transform.localScale  =
                new Vector3(PlayerStats.p2SizeRatio, PlayerStats.p2SizeRatio, PlayerStats.p2SizeRatio);
            if (temp >= coins)
            {
                temp = 0;
                Destroy(other.gameObject);
            }
        }
        if (other.gameObject.tag == "GoldIngots")
        {
            PlayerStats.p2Money += (PlayerStats.p2SizeRatio);
            temp++;
            PlayerStats.collecting = true;

            PlayerStats.p2SizeRatio = PlayerStats.p2Money / 1000f + 1;
            tf.transform.localScale = tf.transform.localScale =
                new Vector3(PlayerStats.p2SizeRatio, PlayerStats.p2SizeRatio, PlayerStats.p2SizeRatio);
            if (temp >= ingots)
            {
                temp = 0;
                Destroy(other.gameObject);
            }

        }
    }
}
