
using UnityEngine;

public class Stick : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public float endDistance = 0.5f;

    Vector3 prevPosition;
    float _speed;
    public float _forceFactor = 500f;
    public float _fadeTime = 1f;
    float _timer = 0f;

    public bool hasShot;

    private AudioSource _audioSource;

    private LineRenderer _line;

    public Aim aim;
    public Transform aimPoint;
    public float radius;

    private bool _isLocked = false;
    private Vector3 _lockedPosition;
    private Vector3 _initScale;
    void Start()
    {
        prevPosition = transform.position;
        _speed = 0f;
        _initScale = transform.lossyScale;
        _audioSource = GetComponent<AudioSource>();
        _line = GetComponent<LineRenderer>();
        hasShot = false;
    }

    public void deactivate()
    {
        aim.gameObject.SetActive(false);
        aimPoint.gameObject.SetActive(false);
        _line.enabled = false;
        _speed = 0f;
        prevPosition = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void activate()
    {
        aim.gameObject.SetActive(true);
        aimPoint.gameObject.SetActive(true);
        _line.enabled = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
    }

    public void Lock()
    {
        _isLocked = true;
        _lockedPosition = transform.position;
    }

    public void Unlock()
    {
        _isLocked = false;
        _lockedPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if ((canInstantiate() || (_isLocked && _line.enabled == true)) && _timer <= 0f && !hasShot)
        {
            activate();
            drawAimLine();
            float remap = 0f;
            float curRadius = radius * (transform.lossyScale.x / _initScale.x);
            if (!_isLocked)
            {
                Ray ray = new Ray(end.position, start.position - end.position);
                transform.position = ray.GetPoint(endDistance);
                transform.up = start.position - end.position;
            }
            else
            {
                Vector3 reproject = Vector3.Project(start.position - end.position, transform.up);
                Vector3 vertical = reproject - (start.position - end.position);
                remap = Mathf.Clamp(vertical.magnitude, 0f, curRadius * 10f) / 10f;

                Vector3 offset = ((start.position - _lockedPosition) - reproject);
                offset *= 3f;
                transform.position = start.position - reproject +
                (vertical.normalized * remap + reproject.normalized * endDistance) + offset;

            }
            Ray rayToBall = new Ray(transform.position, transform.up);
            float dis = Mathf.Sqrt(Mathf.Pow(curRadius, 2) - Mathf.Pow(remap, 2));
            float dis2 = Mathf.Sqrt(Mathf.Pow(Vector3.Distance(transform.position, start.position), 2) - Mathf.Pow(remap, 2));
            aimPoint.position = rayToBall.GetPoint((dis2 - dis - 0.02f) * (transform.lossyScale.x / _initScale.x));
            aimPoint.up = transform.up;
        }
        else
        {
            deactivate();
        }

    }

    void FixedUpdate()
    {
        if (prevPosition != Vector3.zero)
        {
            _speed = (transform.position - prevPosition).magnitude / Time.fixedDeltaTime;
        }
        else
        {
            _speed = 0f;
        }
        prevPosition = transform.position;

        if (_timer > 0f)
        {
            _timer -= Time.fixedDeltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CueBall") && _timer <= 0f && _speed > 0.02f)
        {

            // ToDO Currently there is no torque Don't know why AddForceAtPosition is not working
            other.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(transform.up * _speed * _forceFactor, other.ClosestPoint(transform.position), ForceMode.Impulse);
            _audioSource.volume = _speed * 0.5f;
            _audioSource.Play();
            deactivate();
            _timer = _fadeTime;
            hasShot = true;
        }
    }

    public bool HasShot()
    {
        return hasShot && _timer <= 0f;
    }

    bool canInstantiate()
    {
        return endDistance * 2 < Vector3.Distance(start.position, end.position) && end.position.y > start.position.y - 0.05f;
    }

    void drawAimLine()
    {
        Ray ray;
        if (_lockedPosition != Vector3.zero)
            ray = new Ray(start.position, Vector3.ProjectOnPlane(start.position - _lockedPosition, Vector3.up));
        else ray = new Ray(start.position, Vector3.ProjectOnPlane(start.position - transform.position, Vector3.up));
        Physics.Raycast(ray, out RaycastHit hit, 100f);
        _line.SetPosition(0, start.position);
        _line.SetPosition(1, hit.point);
        _line.startWidth = 0.001f;
        _line.endWidth = 0.005f;
    }
}
