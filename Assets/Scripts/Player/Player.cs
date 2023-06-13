using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public static UnityEvent OnPlayerSmash;
    
    private Rigidbody _playerRigidbody;
    private float invincibleTimer;
    private bool isSmashing;
    private bool isInvincible;
    

    public GameObject invincibleObject;
    public Image invincibleFill;
    public GameObject fireEffect;
    public GameObject winEffect;
    public GameObject splashEffect;

    public enum PlayerState
    {
        Running,
        Prepare,
        Playing,
        Died,
        Finish
    }

    [HideInInspector] public PlayerState currentPlayerState = PlayerState.Running;

    public AudioClip bounceOffClip;
    public AudioClip deadClip;
    public AudioClip winClip;
    public AudioClip destroyClip;
    

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        currentPlayerState = PlayerState.Running;
    }
    private void Update()
    {
        if (currentPlayerState == PlayerState.Playing)
        {
            HandleInputs();

            if (isInvincible)
                UpdateInvincibleState();
            else
                UpdateNormalState();

            UpdateInvincibleObject();
        }

        if (currentPlayerState == PlayerState.Finish)
        {
            HandleFinishState();
        }
    }

    private void FixedUpdate()
    {
        if (currentPlayerState == PlayerState.Playing)
        {
            HandleSmashing();
            LimitVerticalVelocity();
        }
    }

    private void HandleInputs()
    {
        if (Input.GetMouseButtonDown(0))
            isSmashing = true;

        if (Input.GetMouseButtonUp(0))
            isSmashing = false;
    }

    private void UpdateInvincibleState()
    {
        DecreaseInvincibleTimer();
        ActiveFireEffect();
        

        if (invincibleTimer < 0)
        {
            invincibleTimer = 0;
            isInvincible = false;
            invincibleFill.color = Color.white;
        }
    }

    private void ActiveFireEffect()
    {
        if (!fireEffect.activeInHierarchy)
            fireEffect.SetActive(true);
    }
    private void DecreaseInvincibleTimer()
    {
        invincibleTimer -= Time.deltaTime * .35f;
    }

    private void UpdateNormalState()
    {
        if (fireEffect.activeInHierarchy)
            fireEffect.SetActive(false);

        if (isSmashing)
            invincibleTimer += Time.deltaTime * .8f;
        else
            invincibleTimer -= Time.deltaTime * .5f;

        
        if (invincibleTimer >= 1)
        {
            invincibleTimer = 1;
            isInvincible = true;
            invincibleFill.color = Color.red;
        }
    }

    private void UpdateInvincibleObject()
    {
        invincibleObject.SetActive(invincibleTimer >= 0.3f || invincibleFill.color == Color.red);
        invincibleFill.fillAmount = invincibleTimer / 1;
    }

    private void HandleFinishState()
    {
        if (Input.GetMouseButtonDown(0))
            FindObjectOfType<LevelSpawner>().NextLevel();
    }

    private void HandleSmashing()
    {
        if (Input.GetMouseButton(0))
        {
            isSmashing = true;
            _playerRigidbody.velocity = new Vector3(0, -100 * Time.fixedDeltaTime * 7, 0);
        }
    }

    private void LimitVerticalVelocity()
    {
        Vector3 playerVelocity = _playerRigidbody.velocity;
        
        if (playerVelocity.y > 5)
            _playerRigidbody.velocity = new Vector3(playerVelocity.x, 5, playerVelocity.z);
    }
    

    private void OnCollisionEnter(Collision target)
    {
        if (!isSmashing)
            HandleBounce(target);
        else
            HandleSmash(target);

        OnPlayerSmash?.Invoke();

        if (target.gameObject.CompareTag("Finish") && currentPlayerState == PlayerState.Playing)
        {
            currentPlayerState = PlayerState.Finish;
            
            PlayWinEffectAndSound();
        }

        if (target.gameObject.CompareTag("Platform") && currentPlayerState == PlayerState.Playing)
        {
            currentPlayerState = PlayerState.Running;
            isSmashing = false;
            invincibleTimer = 0;
        }
    }

    private void PlayWinEffectAndSound()
    {
        SoundManager.instance.PlaySoundFX(winClip, 0.7f);
        
        GameObject win = Instantiate(winEffect, Camera.main.transform, true);
        win.transform.localPosition = Vector3.up * 1.5f;
        win.transform.eulerAngles = Vector3.zero;
    }

    private void HandleBounce(Collision target)
    {
        _playerRigidbody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);

        if (target.gameObject.CompareTag("Finish"))
        {
            return;
        }

        GameObject splash = Instantiate(splashEffect);
        splash.transform.SetParent(target.transform);
        splash.transform.localEulerAngles = new Vector3(90, Random.Range(0, 359), 0);
        float randomScale = Random.Range(0.18f, 0.25f);
        splash.transform.localScale = new Vector3(randomScale, randomScale, 1);
        splash.transform.position = new Vector3(transform.position.x, transform.position.y - 0.22f, transform.position.z);
        splash.GetComponent<SpriteRenderer>().color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;

        SoundManager.instance.PlaySoundFX(bounceOffClip, 0.5f);
    }

    private void HandleSmash(Collision target)
    {
        if (isInvincible)
        {
            if (target.gameObject.CompareTag("enemy") || target.gameObject.CompareTag("plane"))
                target.transform.parent.GetComponent<StackController>().ShatterAllParts();
        }
        else
        {
            if (target.gameObject.CompareTag("enemy"))
                target.transform.parent.GetComponent<StackController>().ShatterAllParts();

            if (target.gameObject.CompareTag("plane"))
            {
                _playerRigidbody.isKinematic = true;
                transform.GetChild(0).gameObject.SetActive(false);
                
                currentPlayerState = PlayerState.Died;
                SoundManager.instance.PlaySoundFX(deadClip, 0.5f);
            }
        }
    }
    /*private void OnCollisionStay(Collision target)
    {
        if (!isSmashing || target.gameObject.CompareTag("Finish"))
            _playerRigidbody.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
    }*/
}