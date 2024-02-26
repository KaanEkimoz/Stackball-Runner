using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class MovePlayerToStack : MonoBehaviour
    {
        [SerializeField] private float returnSpeed = 2.0f;
        [SerializeField] private Transform playerStopTransform;
        private bool _hasStartedToMove;
        private GameObject _player;

        private void Start()
        {
            if (playerStopTransform == null)
                playerStopTransform = GetComponentInChildren<Transform>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _hasStartedToMove = true;
                Player.isInputActive = false;
                _player = other.gameObject;
            }
        }

        private void Update()
        {
            if (_player != null)
            {
                var playerPos = _player.transform.position;
                var playerStopPos = playerStopTransform.position;

                
                if (_hasStartedToMove)
                {
                    
                    playerPos = Vector3.Lerp(playerPos, playerStopPos, returnSpeed * Time.deltaTime);
                    _player.transform.position = playerPos;
                    if (IsPlayerReached())
                    {
                        _player.GetComponent<Player>().currentPlayerState = Player.PlayerState.Playing;
                        _hasStartedToMove = false;
                        Player.isInputActive = true;
                    }
                }
            }
        }

        private bool IsPlayerReached()
        {
            return _player.transform.position.z > playerStopTransform.position.z - 0.2f;
        }
    }