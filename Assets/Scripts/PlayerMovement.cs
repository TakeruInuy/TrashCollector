using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _deceleration = 15f;
    [SerializeField] private float _turnSpeed = 720f;

    private float _currentSpeed = 0f;

    public bool isMoving;
    public bool canMove = true;
    public UnityEvent onMovement;
    public UnityEvent onStop;




    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

    }


    //void FixedUpdate()
    //{

    //    if(InputManager.inputDirection.magnitude != 0)
    //    {

    //        RotateToInput();

    //        Vector3 movement = _rb.rotation * Vector3.forward * _moveSpeed * Time.fixedDeltaTime;
    //        _rb.MovePosition(_rb.position + movement); //moves forward
    //        isMoving = true;
    //        onMovement.Invoke();
    //    }
    //    else
    //    {
    //        isMoving = false;
    //        onStop.Invoke();
    //    }

    //}
    private void FixedUpdate()
    {
        Vector3 input = InputManager.inputDirection;


        float targetSpeed = input.magnitude > 0 ? _moveSpeed : 0f;

        float speedChangeRate = input.magnitude > 0 ? _acceleration : _deceleration;
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, speedChangeRate * Time.fixedDeltaTime);


        if(canMove)
        {
            if (input.magnitude > 0)
            {
                RotateToInput();
            }


            Vector3 movement = _rb.rotation * Vector3.forward * _currentSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + movement);
        }




        bool wasMoving = isMoving;
        isMoving = _currentSpeed > 0.01f;

        if (isMoving && !wasMoving)
            onMovement.Invoke();
        else if (!isMoving && wasMoving)
            onStop.Invoke();
    }

    private void RotateToInput()
    {
        Quaternion targetRotation = Quaternion.LookRotation(InputManager.inputDirection, Vector3.up); //Rotation to rotate to
        Quaternion newRotation = Quaternion.RotateTowards(_rb.rotation, targetRotation, _turnSpeed * Time.fixedDeltaTime); //smooths rotation based on turn speed

        _rb.MoveRotation(newRotation);
    }

}
