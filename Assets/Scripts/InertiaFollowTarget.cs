using UnityEngine;

public class InertiaFollowTarget : MonoBehaviour
{
    private Rigidbody _bottomTarget;
    private Transform _targetToLook;
    private float _height = 2f;
    private float _depth = 4f;
    public bool isFollowing;

    [Header("Physics Parameters")]
    private float _stiffness = 1000f;
    private float _damper = 50f;

    private Rigidbody _rb;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        if(_bottomTarget && isFollowing)
        {
            Vector3 targetPos = _bottomTarget.position - _bottomTarget.transform.forward * _depth + Vector3.up * _height;
            Vector3 springForce = (targetPos - _rb.position) * _stiffness;


            Vector3 relativeVelocity = _rb.linearVelocity - _bottomTarget.linearVelocity;
            Vector3 dampingForce = -relativeVelocity * _damper;

            _rb.AddForce(springForce + dampingForce);

            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.down, (_targetToLook.position - transform.position).normalized);

            //targetRotation.y = _targetToLook.rotation.y;
            _rb.MoveRotation(targetRotation);            
            
        }
    }


    public void EnableFollow(Rigidbody followTarget, Transform lookTarget, float positionHeight, float positionDepth, float stiffness, float damper)
    {
        _bottomTarget = followTarget;
        _targetToLook = lookTarget;
        _height = positionHeight;
        _depth = positionDepth;
        _stiffness= stiffness;
        _damper = damper;

        isFollowing = true;
    }

    public void DisableFollow()
    {
        _bottomTarget = null;
        _targetToLook = null;

        isFollowing = false;
    }


}
