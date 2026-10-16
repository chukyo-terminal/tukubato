using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerStatus))]
[RequireComponent(typeof(MobAttack))]
public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float movespeed = 3;
    [SerializeField] private float jumpPower = 3;
    [SerializeField] private Animator animator;
    [SerializeField] private Button attackButton;
    private CharacterController _characterController;
    private Transform _transform;
    private Vector3 _moveVelocity;
    private InputAction _move;
    private InputAction _jump;
    private InputAction _attack;
    private PlayerStatus _status;
    private MobAttack _mobAttack;

    private bool _isMoveCoroutineRunning = false;
    private Vector2 _prevMoveValue = Vector2.zero;


    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _transform = transform;
        _status = GetComponent<PlayerStatus>();
        _mobAttack = GetComponent<MobAttack>();
        var input = GetComponent<PlayerInput>();
        input.currentActionMap.Enable();
        _move = input.currentActionMap.FindAction("Move");
        _jump = input.currentActionMap.FindAction("Jump");
        attackButton.onClick.AddListener(Attack);
        _attack = input.currentActionMap.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_characterController.isGrounded ? "地上にいます" : "空中です");

        if(_status.IsMovable)
        {
            var moveValue = _move.ReadValue<Vector2>();
            _moveVelocity.x = moveValue.x * movespeed;
            _moveVelocity.z = moveValue.y * movespeed;
            _transform.LookAt(_transform.position + new Vector3(_moveVelocity.x, 0, _moveVelocity.z));
            if (_prevMoveValue == Vector2.zero && moveValue != Vector2.zero)
            {
             

                // 5秒後イベントをまだ走らせていないなら開始
                if (!_isMoveCoroutineRunning)
                {
                    StartCoroutine(NotifyMoveDelayed());
                }
            }

            _prevMoveValue = moveValue;
        }

        else
        {
            _moveVelocity.x = 0;
            _moveVelocity.z = 0;
        }

        _characterController.Move(_moveVelocity * Time.deltaTime);
        
        if (_characterController.isGrounded)
        {
            if(_jump.WasPressedThisFrame())
            {
                Debug.Log("ジャンプ！");
                _moveVelocity.y = jumpPower;
            }
        }
        else
        {
            _moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

        if (_attack.WasPressedThisFrame())
        {
            Attack();
        }

        animator.SetFloat("MoveSpeed", new Vector3(_moveVelocity.x, 0, _moveVelocity.z).magnitude);
    }

    public void Attack()
    {
        _mobAttack.AttackIfPossible();
    }

    private IEnumerator NotifyMoveDelayed()
    {
        _isMoveCoroutineRunning = true;

        yield return new WaitForSeconds(300f*Time.deltaTime);

        PlayerEvents.OnMove?.Invoke();

        _isMoveCoroutineRunning = false;
    }

}
