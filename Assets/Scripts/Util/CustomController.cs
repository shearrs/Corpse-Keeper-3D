using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof (CapsuleCollider), typeof(SphereAreaCheck))]
public class CustomController : MonoBehaviour
{
    [SerializeField] private CapsuleCollider col;

    [Header("Settings")]
    [SerializeField] private SphereAreaCheck groundCheck;
    [SerializeField] private LayerMask collisionMask;
    [SerializeField, Range(0, 1)] private float skinWidth = 0.015f;
    [SerializeField, Range(0, 90)] private float maxSlopeAngle = 55f;
    [SerializeField, Range(0, 90)] private float maxRampAngle = 10f;
    [SerializeField] private float groundSnapDistance = 1;
    [SerializeField, Range(0, 1)] private float stepHeight = 1;

    [Header("Debug")]
    [SerializeField] private bool debugGizmos;
    [SerializeField] private bool raycastGizmos;

    public LayerMask CollisionMask => collisionMask;
    public Vector3 Velocity { get; private set; }
    public bool CanBeGrounded { get; set; }
    public bool IsGrounded { get => isGrounded && CanBeGrounded; private set => isGrounded = value; }
    public bool WasGroundedLastFrame { get; private set; }
    public float Radius => col.radius;
    public float Height { get => col.height; set => col.height = value; }

    private const int MAX_STEPS = 5;
    private const int HORIZONTAL_RAY_COUNT = 12;
    private const int VERTICAL_RAY_COUNT = 2;

    private bool isGrounded;

    public Vector3 Center { get => transform.TransformPoint(col.center); }

    private Vector3 bugCenter;
    private Vector3 bugMovement;
    private Vector3 bugSnapMovement;
    private Vector3 bugSlideMovement;
    private Quaternion bugSnapRotation;
    private RaycastHit bugHit;
    private RaycastHit[] rayBugHits;

    private void Awake()
    {
        rayBugHits = new RaycastHit[HORIZONTAL_RAY_COUNT + VERTICAL_RAY_COUNT];

        col = GetComponent<CapsuleCollider>();

        CanBeGrounded = true;
    }

    private void Update()
    {
        WasGroundedLastFrame = IsGrounded;

        if (CanBeGrounded)
            IsGrounded = groundCheck.CheckArea() > 0;
        else
            IsGrounded = false;

        transform.position += ExitGeometry(Center);
    }

    public void Move(Vector3 movement)
    {
        Vector3 finalMovement = CalculateCollideAndSlide(movement);
        Velocity = finalMovement / Time.deltaTime;

        finalMovement += ExitGeometry(Center + finalMovement);

        transform.position += finalMovement;
    }

    private Vector3 CalculateCollideAndSlide(Vector3 movement)
    {
        Vector3 horizontalMovement = new(movement.x, 0, movement.z);
        Vector3 verticalMovement;

        if (IsGrounded && movement.y > 0)
        {
            horizontalMovement.y = movement.y; // add our movement up slopes to our horizontal movement

            horizontalMovement = CalculateHorizontalCollideAndSlide(horizontalMovement, Center);
            verticalMovement = CalculateVerticalCollideAndSlide(groundSnapDistance * Vector3.down, Center + horizontalMovement); // just snap us to the floor
        }
        else
        {
            verticalMovement =  new(0, movement.y, 0); // we are actually using our vertical movement

            horizontalMovement = CalculateHorizontalCollideAndSlide(horizontalMovement, Center);
            verticalMovement = CalculateVerticalCollideAndSlide(verticalMovement, Center + horizontalMovement);
        }

        return horizontalMovement + verticalMovement;
    }

    private Vector3 CalculateHorizontalCollideAndSlide(Vector3 movement, Vector3 center, int depth = 0)
    {
        if (depth > MAX_STEPS)
            return Vector3.zero;

        RaycastHit hit = CollisionCast(movement, center);
        if (hit.transform == null)
            return movement;

        Vector3 snapToSurface = Vector3.zero;
        Vector3 slideMovement;

        if (hit.distance > skinWidth) // snap us to the surface
            snapToSurface = (hit.distance - skinWidth) * movement.normalized;

        float angle = Vector3.Angle(Vector3.up, hit.normal);
        bool isGoodAngle = angle <= maxSlopeAngle;

        if (IsGrounded && !isGoodAngle)
        {
            Vector3 step = Step(hit, movement, center);
            Vector3 horizontalStep = new(step.x, 0, step.z);

            if (horizontalStep.sqrMagnitude > movement.sqrMagnitude)
                movement = Vector3.zero;
            else
                movement = (movement.magnitude - horizontalStep.magnitude) * movement.normalized;

            center += step;
            transform.position += step;
        }

        if (IsGrounded && isGoodAngle || !IsGrounded && angle <= maxRampAngle) // flat enough ground to walk on
        {
            Vector3 movementWithSnap = movement - snapToSurface;
            slideMovement = Vector3.ProjectOnPlane(movement - snapToSurface, hit.normal);

            slideMovement = slideMovement.normalized * movementWithSnap.magnitude; // we shouldn't walk slower or faster on slopes
        }
        else // else we allow for the y calculations to slide us down
        {
            Vector3 horizontalMovement = new(movement.x, 0, movement.z);
            Vector3 horizontalNormal = new(hit.normal.x, 0, hit.normal.z);

            slideMovement = Vector3.ProjectOnPlane(horizontalMovement - snapToSurface, horizontalNormal);
        }

        bugMovement = movement;
        bugSlideMovement = slideMovement;
        bugCenter = center;
        bugSnapMovement = snapToSurface;
        bugHit = hit;
        bugSnapRotation = transform.rotation;

        return snapToSurface + CalculateHorizontalCollideAndSlide(slideMovement, center + snapToSurface, ++depth); ;
    }

    private Vector3 CalculateVerticalCollideAndSlide(Vector3 movement, Vector3 center, int depth = 0)
    {
        if (depth > MAX_STEPS)
            return Vector3.zero;

        RaycastHit hit = CollisionCast(movement, center);

        if (hit.transform == null)
            return movement;

        Vector3 snapToSurface = Vector3.zero;
        Vector3 slideMovement;

        if (hit.distance > skinWidth) // we hit something actually outside of us
            snapToSurface = (hit.distance - skinWidth) * movement.normalized;

        float angle = Vector3.Angle(Vector3.up, hit.normal);

        if (IsGrounded && angle <= maxSlopeAngle)
            return snapToSurface;
        
        slideMovement = Vector3.ProjectOnPlane(movement - snapToSurface, hit.normal);

        return snapToSurface + CalculateVerticalCollideAndSlide(slideMovement, center + snapToSurface, ++depth);
    }

    private RaycastHit CollisionCast(Vector3 movement, Vector3 center)
    {
        Vector3 topPosition = GetTopPosition(center);
        Vector3 bottomPosition = GetBottomPosition(center);

        Vector3 direction = movement.normalized;
        float magnitude = movement.magnitude;

        Physics.CapsuleCast(bottomPosition, topPosition, Radius, direction, out RaycastHit hit, magnitude + skinWidth, collisionMask, QueryTriggerInteraction.Ignore);

        return hit;
    }

    private Vector3 ExitGeometry(Vector3 center)
    {
        RaycastHit[] horizontalHits = new RaycastHit[HORIZONTAL_RAY_COUNT];
        RaycastHit[] verticalHits = new RaycastHit[VERTICAL_RAY_COUNT];
        Vector3 topPosition = GetTopPosition(center);
        Vector3 bottomPosition = GetBottomPosition(center);
        Vector3 horizontalDirection = Vector3.zero;
        Vector3 verticalMovement = Vector3.zero;
        float horizontalMoveAmount = 0;
        float horizontalDistance = Radius;
        float verticalDistance = col.height * 0.5f;

        // horizontal
        Physics.Raycast(topPosition, transform.forward, out horizontalHits[0], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(topPosition, -transform.forward, out horizontalHits[1], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(topPosition, transform.right, out horizontalHits[2], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(topPosition, -transform.right, out horizontalHits[3], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(center, transform.forward, out horizontalHits[4], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(center, -transform.forward, out horizontalHits[5], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(center, transform.right, out horizontalHits[6], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(center, -transform.right, out horizontalHits[7], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(bottomPosition, transform.forward, out horizontalHits[8], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(bottomPosition, -transform.forward, out horizontalHits[9], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(bottomPosition, transform.right, out horizontalHits[10], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);
        Physics.Raycast(bottomPosition, -transform.right, out horizontalHits[1], horizontalDistance, collisionMask, QueryTriggerInteraction.Ignore);

        // vertical
        Physics.Raycast(center, transform.up, out verticalHits[0], verticalDistance, collisionMask);
        Physics.Raycast(center, -transform.up, out verticalHits[1], verticalDistance, collisionMask);

        for (int i = 0; i < horizontalHits.Length; i++)
        {
            rayBugHits[i] = horizontalHits[i];

            if (horizontalHits[i].transform != null)
            {
                horizontalDirection += horizontalHits[i].normal;
                horizontalDirection.Normalize();

                if (horizontalHits[i].distance > horizontalMoveAmount)
                    horizontalMoveAmount = horizontalHits[i].distance;
            }
        }

        if (horizontalMoveAmount > 0.01f)
            horizontalMoveAmount = horizontalDistance - horizontalMoveAmount;

        rayBugHits[HORIZONTAL_RAY_COUNT] = verticalHits[0];
        rayBugHits[HORIZONTAL_RAY_COUNT + 1] = verticalHits[1];

        if (verticalHits[0].transform != null)
        {
            verticalMovement = (verticalDistance - verticalHits[0].distance) * -transform.up;
        }

        if (verticalHits[1].transform != null)
        {
            verticalMovement += (verticalDistance - verticalHits[1].distance) * transform.up;
        }

        return (horizontalDirection * horizontalMoveAmount) + verticalMovement;
    }

    private Vector3 Step(RaycastHit originalHit, Vector3 movement, Vector3 center)
    {
        // accounting for where the player's collider collided with the step
        Vector3 hitPoint = originalHit.point;
        hitPoint.y = 0;
        float hitDistance = (hitPoint - new Vector3(transform.position.x, 0, transform.position.z)).magnitude;

        // getting the origin of the step height raycast
        movement.y = 0;
        Vector3 foot = center - (Height * 0.5f * Vector3.up);
        Vector3 rayOrigin = foot + (stepHeight * Vector3.up) + movement + (hitDistance * movement.normalized);

        Debug.DrawRay(rayOrigin, Vector3.down * stepHeight, Color.yellow);

        // we hit a valid step height
        if (!Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, stepHeight, collisionMask))
            return Vector3.zero;

        Vector3 heightMovement = (stepHeight - hit.distance + skinWidth) * Vector3.up;
        Vector3 verticalCenter = center + heightMovement;

        Vector3 change = heightMovement + (hitDistance * movement.normalized);

        // check if we can go up
        if (CollisionCast(heightMovement, center).transform != null)
        {
            return Vector3.zero;
        }
        else if (CollisionCast(movement, verticalCenter).transform != null) // check if we can go over
        {
            return Vector3.zero;
        }
        else
            return change;
    }

    public Vector3 GetTopPosition(Vector3 center)
    {
        float halfHeight = (Height * 0.5f) - Radius;
        halfHeight = Mathf.Max(halfHeight, 0);

        Vector3 topPosition = center + (halfHeight * transform.up);

        return topPosition;
    }

    public Vector3 GetBottomPosition(Vector3 center)
    {  
        float halfHeight = (Height * 0.5f) - Radius;
        halfHeight = Mathf.Max(halfHeight, 0);

        Vector3 bottomPosition = center - (halfHeight * transform.up);

        return bottomPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (col == null)
            return;

        if (debugGizmos)
        {
            Vector3 topPosition = GetTopPosition(Center);
            Vector3 bottomPosition = GetBottomPosition(Center);

            Gizmos.color = Color.blue;
            GizmoExtensions.DrawWireCylinder(topPosition, bottomPosition, Radius);

            Gizmos.color = Color.magenta;
            GizmoExtensions.DrawWireCylinder(topPosition, bottomPosition, Radius + skinWidth);

            if (bugCenter != Vector3.zero)
            {
                Vector3 bugSnapPosition = bugCenter + bugSnapMovement;
                Quaternion original = transform.rotation;
                transform.rotation = bugSnapRotation;

                // the original center
                Gizmos.color = Color.green;
                Vector3 bugCenterTop = GetTopPosition(bugCenter);
                Vector3 bugCenterBottom = GetBottomPosition(bugCenter);
                GizmoExtensions.DrawWireCylinder(bugCenterTop, bugCenterBottom, Radius);

                Gizmos.DrawRay(bugHit.point, bugSlideMovement.normalized);

                Gizmos.color = Color.white;
                Gizmos.DrawRay(bugHit.point, bugMovement.normalized);

                // the amount and direction we moved to our snap
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(bugCenter, bugSnapPosition);

                // the new snapped position
                Gizmos.color = Color.red;
                Vector3 bugTopPosition = GetTopPosition(bugSnapPosition);
                Vector3 bugBottomPosition = GetBottomPosition(bugSnapPosition);
                GizmoExtensions.DrawWireCylinder(bugTopPosition, bugBottomPosition, Radius);

                // the skin width
                Gizmos.color = Color.magenta;
                GizmoExtensions.DrawWireCylinder(bugTopPosition, bugBottomPosition, Radius + skinWidth);

                transform.rotation = original;

                // hit position
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(bugHit.point, 0.05f);
                Gizmos.color = Color.red;
                Gizmos.DrawRay(bugHit.point, bugHit.normal);
            }
        }

        if (raycastGizmos)
        {
            Vector3 topPosition = GetTopPosition(Center);
            Vector3 bottomPosition = GetBottomPosition(Center);
            float radius = Radius;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(bottomPosition, groundSnapDistance * Vector3.down);

            Gizmos.color = Color.cyan;

            Gizmos.DrawLine(topPosition, topPosition + transform.forward * radius);
            Gizmos.DrawLine(topPosition, topPosition + transform.right * radius);
            Gizmos.DrawLine(topPosition, topPosition + -transform.forward * radius);
            Gizmos.DrawLine(topPosition, topPosition + -transform.right * radius);
            Gizmos.DrawLine(Center, Center + transform.forward * radius);
            Gizmos.DrawLine(Center, Center + transform.right * radius);
            Gizmos.DrawLine(Center, Center + -transform.forward * radius);
            Gizmos.DrawLine(Center, Center + -transform.right * radius);
            Gizmos.DrawLine(Center, Center + transform.up * col.height * 0.5f);
            Gizmos.DrawLine(Center, Center + -transform.up * col.height * 0.5f);
            Gizmos.DrawLine(bottomPosition, bottomPosition + transform.forward * radius);
            Gizmos.DrawLine(bottomPosition, bottomPosition + -transform.forward * radius);
            Gizmos.DrawLine(bottomPosition, bottomPosition + transform.right * radius);
            Gizmos.DrawLine(bottomPosition, bottomPosition + -transform.right * radius);

            if (rayBugHits != null)
            {
                for (int i = 0; i < rayBugHits.Length; i++)
                {
                    if (rayBugHits[i].transform != null)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireSphere(rayBugHits[i].point, 0.1f);
                    }
                }
            }
        }
    }
}