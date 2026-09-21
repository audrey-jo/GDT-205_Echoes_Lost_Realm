using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseEntity))]
public class SteeringBehaviours : MonoBehaviour
{
    public ObstacleManager  obstacleManager = null;
    private BaseEntity      baseEntity      = null;

    //Toggle flags to enable / disable behaviours.
    public bool     bEnabled_Seek       = false;
    public float    fSeekWeight         = 0.5f;
    public bool     bEnabled_Arrive     = false;
    public float    fArriveWeight       = 0.5f;
    public bool     bEnabled_Flee       = false;
    public float    fFleeWeight         = 0.5f;
    public bool     bEnabled_Avoidance  = false;
    public float    fAvoidanceWeight    = 0.75f;
    public bool     bEnabled_Wander     = false;
    public float    fWanderWeight       = 0.25f;

    public bool     bEnabled_Flocking   = false;
    public float    fFlockingWeight     = 0.25f;

    //Target used for behaviours.
    public Transform targetTransform = null;
    public List<Transform> feelers = new List<Transform>();
    //public Vector2 targetPosition;

    public float fWander_Radius         = 5.0f;
    public float fWander_DistanceAhead  = 10.0f;

    public float fFlocking_Distance = 25.0f;
    public float fFlocking_FOV = -0.5f;

    public float fMaximumForce          = 10.0f;

    List<Vector2> whiskers = new List<Vector2>();
    public float fWhiskerMaxDistanceAhead   = 11.0f;
    public float fWhiskerAngle              = 45.0f;
    private void Start()
    {
        baseEntity = GetComponent<BaseEntity>();
    }
    public Vector2 GetCombinedForce()
    {
        float fRemainingForceAllowance = fMaximumForce;
        Vector2 vAccumulatedForce = Vector2.zero;

        //Avoidance is the primary behaviour, so this is always added (scaled to match maximum force, or as is)
        if (bEnabled_Avoidance)
        {
            Vector2 vAvoidanceForce = Avoidance() * fAvoidanceWeight;
            if (VectorMath.Magnitude(vAvoidanceForce) > fRemainingForceAllowance)
            {
                return VectorMath.Normalize(vAvoidanceForce) * fRemainingForceAllowance;
            }

            //Add the avoidance force.
            vAccumulatedForce += vAvoidanceForce;

            fRemainingForceAllowance = fMaximumForce - VectorMath.Magnitude(vAccumulatedForce);
        }

        if (bEnabled_Flocking)
        {
            Vector2 vFlockingForce = Flocking() * fFlockingWeight;

            if (VectorMath.Magnitude(vFlockingForce) > fRemainingForceAllowance)
            {
                vAccumulatedForce += VectorMath.Normalize(vFlockingForce) * fRemainingForceAllowance;
                return vAccumulatedForce;
            }

            //Otherwise add the flocking force.
            vAccumulatedForce += vFlockingForce;

            fRemainingForceAllowance = fMaximumForce - VectorMath.Magnitude(vAccumulatedForce);
        }

        if (bEnabled_Seek)
        {
            Vector2 vSeekForce = Seek(targetTransform.position) * fSeekWeight;

            if (VectorMath.Magnitude(vSeekForce) > fRemainingForceAllowance)
            {
                vAccumulatedForce += VectorMath.Normalize(vSeekForce) * fRemainingForceAllowance;
                return vAccumulatedForce;
            }

            //Otherwise add the seek force.
            vAccumulatedForce += vSeekForce;

            fRemainingForceAllowance = fMaximumForce - VectorMath.Magnitude(vAccumulatedForce);
        }

        if (bEnabled_Arrive)
        {
            Vector2 vArriveForce = Arrive(targetTransform.position) * fArriveWeight;

            if (VectorMath.Magnitude(vArriveForce) > fRemainingForceAllowance)
            {
                vAccumulatedForce += VectorMath.Normalize(vArriveForce) * fRemainingForceAllowance;
                return vAccumulatedForce;
            }

            //Otherwise add the arrive force.
            vAccumulatedForce += vArriveForce;

            fRemainingForceAllowance = fMaximumForce - VectorMath.Magnitude(vAccumulatedForce);
        }

        if (bEnabled_Flee)
        {
            Vector2 vFleeForce = Flee(targetTransform.position) * fFleeWeight;

            if (VectorMath.Magnitude(vFleeForce) > fRemainingForceAllowance)
            {
                vAccumulatedForce += VectorMath.Normalize(vFleeForce) * fRemainingForceAllowance;
                return vAccumulatedForce;
            }

            //Otherwise add the flee force.
            vAccumulatedForce += vFleeForce;

            fRemainingForceAllowance = fMaximumForce - VectorMath.Magnitude(vAccumulatedForce);
        }

        if (bEnabled_Wander)
        {
            Vector2 vWanderForce = Wander() * fWanderWeight;

            if (VectorMath.Magnitude(vWanderForce) > fRemainingForceAllowance)
            {
                vAccumulatedForce += VectorMath.Normalize(vWanderForce) * fRemainingForceAllowance;
                return vAccumulatedForce;
            }

            //Otherwise add the wander force.
            vAccumulatedForce += vWanderForce;

            fRemainingForceAllowance = fMaximumForce - VectorMath.Magnitude(vAccumulatedForce);
        }

        return vAccumulatedForce;
    }

    Vector2 Seek(Vector2 targetPosition)
    {
        //Calculate vector in direction of target poition.
        Vector2 vecToTarget = targetPosition - (Vector2)transform.position;

        //Normalize it to get the unit vector.
        vecToTarget = VectorMath.Normalize(vecToTarget);

        //Get the velocity in this direction by multiplying by the max speed.
        Vector2 desiredVelocity = vecToTarget * baseEntity.GetMaxMoveSpeed();

        //Deduct the current velocity to get the velocity we require.
        desiredVelocity -= baseEntity.GetCurrentVelocity();

        return desiredVelocity;
    }

    Vector2 Arrive(Vector2 targetPosition)
    {
        const float fDECELERATION = 0.1f;

        //Calculate vector in direction of target position.
        Vector2 vecToTarget = targetPosition - (Vector2)transform.position;

        //Get the magnitude of the vector to the target.
        float fDistanceToTarget = VectorMath.Magnitude(vecToTarget);

        //Are we at the target?
        if (fDistanceToTarget > 0)
        {
            //Calculate the desired speed using the deceleration variable.
            float fSpeed = fDistanceToTarget * fDECELERATION;

            //If we are far away fSpeed will be greater than our maximum move speed, so
            //cap the speed value to the maximum this entity can move.
            fSpeed = Mathf.Min(baseEntity.GetMaxMoveSpeed(), fSpeed);

            //Normalize vecToTarget to get the unit vector equivalent.
            vecToTarget = VectorMath.Normalize(vecToTarget);

            //Get the velocity in this direction by multiplying by the modified speed.
            Vector2 desiredVelocity = vecToTarget * fSpeed;

            //Deduct the current velocity to get the velocity we require.
            desiredVelocity -= baseEntity.GetCurrentVelocity();

            return desiredVelocity;
        }

        //At target so no force required.
        return Vector2.zero;
    }
    Vector2 Flee(Vector2 targetPosition)
    {
        //Calculate vector in direction from target poition.
        Vector2 vecFromTarget = (Vector2)transform.position - targetPosition;

        //Normalize it to get the unit vector.
        vecFromTarget = VectorMath.Normalize(vecFromTarget);

        //Get the velocity in this direction by multiplying by the max speed.
        Vector2 desiredVelocity = vecFromTarget * baseEntity.GetMaxMoveSpeed();

        //Deduct the current velocity to get the velocity we require.
        desiredVelocity -= baseEntity.GetCurrentVelocity();

        return desiredVelocity;
    }
    Vector2 Avoidance()
    {
        //Early out if obstacle manager is not set up - We need this to be able to avoid obstacles.
        if (obstacleManager == null) { return Vector2.zero; }

        SetUpWhiskers();

        for (int whiskerIndex = 0; whiskerIndex < whiskers.Count; whiskerIndex++)
        {
            Vector2 currentWhiskerPos = whiskers[whiskerIndex];

            for (int obstacleIndex = 0; obstacleIndex < obstacleManager.zCollidables.Count; obstacleIndex++)
            {
                CollisionRect obstacle = obstacleManager.zCollidables[obstacleIndex];
                if (obstacle != null)
                {
                    //Does a whisker intersect with the edge (line) of a rectangle?
                    float fPenetration = 0.0f;

                    //Find the line on the rectangle that was intersected.
                    Vector2 vIntersection1 = Vector2.zero;
                    bool bLine1 = VectorMath.LineToLineIntersection((Vector2)baseEntity.transform.position, 
                        currentWhiskerPos, new Vector2(obstacle.vPosition.x, obstacle.vPosition.y), 
                        new Vector2(obstacle.vPosition.x + obstacle.width, obstacle.vPosition.y), ref vIntersection1);

                    Vector2 vIntersection2 = Vector2.zero;
                    bool bLine2 = VectorMath.LineToLineIntersection((Vector2)baseEntity.transform.position, 
                        currentWhiskerPos, new Vector2(obstacle.vPosition.x + obstacle.width, obstacle.vPosition.y), 
                        new Vector2(obstacle.vPosition.x + obstacle.width, obstacle.vPosition.y + obstacle.height), ref vIntersection2);

                    Vector2 vIntersection3 = Vector2.zero;
                    bool bLine3 = VectorMath.LineToLineIntersection((Vector2)baseEntity.transform.position, 
                        currentWhiskerPos, new Vector2(obstacle.vPosition.x + obstacle.width, obstacle.vPosition.y + obstacle.height), 
                        new Vector2(obstacle.vPosition.x, obstacle.vPosition.y + obstacle.height), ref vIntersection3);

                    Vector2 vIntersection4 = Vector2.zero;
                    bool bLine4 = VectorMath.LineToLineIntersection((Vector2)baseEntity.transform.position, 
                        currentWhiskerPos, new Vector2(obstacle.vPosition.x, obstacle.vPosition.y + obstacle.height), 
                        new Vector2(obstacle.vPosition.x, obstacle.vPosition.y), ref vIntersection4);

                    if (bLine1 || bLine2 || bLine3 || bLine4)
                    {
                        //Caculate the depth we penetrated the obstacle.
                        if (bLine1) fPenetration = VectorMath.Magnitude(currentWhiskerPos - vIntersection1);
                        else if (bLine2) fPenetration = VectorMath.Magnitude(currentWhiskerPos - vIntersection2);
                        else if (bLine3) fPenetration = VectorMath.Magnitude(currentWhiskerPos - vIntersection3);
                        else if (bLine4) fPenetration = VectorMath.Magnitude(currentWhiskerPos - vIntersection4);

                        //Get the vector from the centre of the obstacle to us.
                        Vector2 vCentreOfObstacle = new Vector2(obstacle.vPosition.x + obstacle.width * 0.5f, obstacle.vPosition.y + obstacle.height * 0.5f);
                        Vector2 vObstacleToMe = (Vector2)baseEntity.transform.position - vCentreOfObstacle;

                        //Get unit vector.
                        Vector2 vUnitObstacleToMe = VectorMath.Normalize(vObstacleToMe);

                        //Get force.
                        Vector2 vRepellingForce = vUnitObstacleToMe * fPenetration;
                        return vRepellingForce;
                    }
                }
            }
        }
        return Vector2.zero;
    }


    void SetUpWhiskers()
    {
        whiskers.Clear();

        //Speed modifier is a 0-1 value: If moving at maximum speed it wil be a 1.
        float fSpeedModifier = VectorMath.Magnitude(baseEntity.GetCurrentVelocity()) / baseEntity.GetMaxMoveSpeed();
        float fWhiskerDistanceAhead = fWhiskerMaxDistanceAhead * fSpeedModifier;

        //Whisker ahead.
        Vector2 vWhiskerPos = (Vector2)baseEntity.transform.position + (baseEntity.GetFacing() * fWhiskerDistanceAhead);
        whiskers.Add(vWhiskerPos);
        Debug.DrawRay(baseEntity.transform.position, (baseEntity.GetFacing() * fWhiskerDistanceAhead), Color.black, 0.0f, true);

        float rad = Mathf.Deg2Rad * fWhiskerAngle;
        //Whisker at angle forward and to the left.
        vWhiskerPos = (Vector2)baseEntity.transform.position + baseEntity.GetFacing();
        Vector2 vWhiskerDirection = VectorMath.RotateAroundAPoint((Vector2)baseEntity.transform.position, vWhiskerPos, rad);
        vWhiskerDirection -= (Vector2)baseEntity.transform.position;
        vWhiskerDirection = VectorMath.Normalize(vWhiskerDirection);
        vWhiskerPos = (Vector2)baseEntity.transform.position + (vWhiskerDirection * fWhiskerDistanceAhead);
        whiskers.Add(vWhiskerPos);
        Debug.DrawRay(baseEntity.transform.position, (vWhiskerDirection * fWhiskerDistanceAhead), Color.black, 0.0f, true);

        //Whisker at angle forward and to the right.
        vWhiskerPos = (Vector2)baseEntity.transform.position + baseEntity.GetFacing();
        vWhiskerDirection = VectorMath.RotateAroundAPoint((Vector2)baseEntity.transform.position, vWhiskerPos, -rad);
        vWhiskerDirection -= (Vector2)baseEntity.transform.position;
        vWhiskerDirection = VectorMath.Normalize(vWhiskerDirection);
        vWhiskerPos = (Vector2)baseEntity.transform.position + (vWhiskerDirection * fWhiskerDistanceAhead);
        whiskers.Add(vWhiskerPos);
        Debug.DrawRay(baseEntity.transform.position, (vWhiskerDirection * fWhiskerDistanceAhead), Color.black, 0.0f, true);

        //Whisker to the left.
        vWhiskerPos = (Vector2)baseEntity.transform.position + (-baseEntity.GetRight() * fWhiskerDistanceAhead);
        whiskers.Add(vWhiskerPos);
        Debug.DrawRay(baseEntity.transform.position, (-baseEntity.GetRight() * fWhiskerDistanceAhead), Color.black, 0.0f, true);

        //Whisker to the right.
        vWhiskerPos = (Vector2)baseEntity.transform.position + (baseEntity.GetRight() * fWhiskerDistanceAhead);
        whiskers.Add(vWhiskerPos);
        Debug.DrawRay(baseEntity.transform.position, (baseEntity.GetRight() * fWhiskerDistanceAhead), Color.black, 0.0f, true);
    }

    Vector2 Wander()
    {
        //Get a random value in the range of 0.0 -> 1.0.
        float fRandomDot = Random.Range(-1.0f, 1.0f);

        float radian = Mathf.Acos(fRandomDot);

        //Degree will be between 0-180, so to ensure a full rotation, lets randomly add 180 degrees.
        if (Random.Range(0, 100) > 50)
        {
            radian += Mathf.PI;
        }

        //Calculate the wander position ahead of the agent.
        Vector2 vWanderPosition = (Vector2)baseEntity.transform.position;
        vWanderPosition += baseEntity.GetFacing() * fWander_DistanceAhead;

        //Rotate a point around unit circle.
        Vector2 vWanderDirection = VectorMath.RotateAroundOrigin(radian);

        //Expand the point of the wander direction by the radius.
        vWanderDirection *= fWander_Radius;

        //Add the direction to the projected position.
        vWanderPosition += vWanderDirection;

        //Draw a ray from ahead position to wander position.
        Vector2 vAheadPos = (Vector2)baseEntity.transform.position + (baseEntity.GetFacing() * fWander_DistanceAhead);
        Debug.DrawRay(vAheadPos, vWanderDirection, Color.green, 0.0f, true);

        //Instead of repeating the exact code written in Seek(), just call Seek().
        return Seek(vWanderPosition);
    }

    Vector2 Flocking()
    {
        List<BaseEntity> neighbourhood = new List<BaseEntity>();

        //------------------------------------------------------------------------
        //Find all base entities within within distance and field of view.
        for (int entityIndex = 0; entityIndex < transform.parent.childCount; entityIndex++)
        {
            Transform otherTransform = transform.parent.GetChild(entityIndex);

            //Ensure the transform is available and is not our own transform.
            if (otherTransform != null && otherTransform != transform)
            {
                BaseEntity otherBaseEntity = otherTransform.GetComponent<BaseEntity>();

                if (otherBaseEntity != null)
                {
                    Vector2 vecToOther = otherTransform.position - transform.position;

                    //Is this transform close enough to be considered in range?
                    if (VectorMath.Magnitude(vecToOther) < fFlocking_Distance)
                    {
                        Vector2 unitVecToOther = VectorMath.Normalize(vecToOther);

                        //Is this transform within the designated fov range?
                        if (VectorMath.Dot(baseEntity.GetFacing(), unitVecToOther) > fFlocking_FOV)
                        {
                            neighbourhood.Add(otherBaseEntity);
                        }
                    }
                }
            }
        }

        //If we have no entities in our neighbourhood, return a zero velocity.
        if(neighbourhood.Count == 0)
        {
            return Vector2.zero;
        }

        Vector2 desiredVelocity = Vector2.zero;
        desiredVelocity += Separation(neighbourhood);
        desiredVelocity += Alignment(neighbourhood);
        desiredVelocity += Cohesion(neighbourhood);

        return desiredVelocity;
    }
    Vector2 Separation(List<BaseEntity> neighbourhood)
    {
        Vector2 accumulatedSeparationForce = Vector2.zero;

        //Loop through all entities in our neighbourhood.
        for(int entityIndex = 0; entityIndex < neighbourhood.Count; entityIndex++)
        {
            BaseEntity otherBaseEntity = neighbourhood[entityIndex];

            //Calculate the vector to the other entity.
            Vector2 vecToOther = otherBaseEntity.transform.position - transform.position;
            
            //Get the distance to the other entity.
            float fMagnitude = VectorMath.Magnitude(vecToOther);

            //Get the unit vector to the other entity.
            Vector2 unitVecToOther = VectorMath.Normalize(vecToOther);

            //Add unit vector divided by original magnitude to the desired force.
            accumulatedSeparationForce += (unitVecToOther / fMagnitude);
        }

        return accumulatedSeparationForce;
    }
    Vector2 Alignment(List<BaseEntity> neighbourhood)
    {
         Vector2 accumulatedHeading = Vector2.zero;

        //Loop through all entities in our neighbourhood.
        for (int entityIndex = 0; entityIndex < neighbourhood.Count; entityIndex++)
        {
            BaseEntity otherBaseEntity = neighbourhood[entityIndex];

            //Accumulate all the facing directions of entities in our neighbourhood.
            accumulatedHeading += otherBaseEntity.GetFacing();
        }

        //We get our alignment force by dividing our accumulatedHeading by the number of entities in the neighbourhood.
        Vector2 alignmentForce = (accumulatedHeading / neighbourhood.Count);

        //Deduct our own direction.
        alignmentForce -= baseEntity.GetFacing();
        
        return alignmentForce;
    }

    Vector2 Cohesion(List<BaseEntity> neighbourhood)
    {
        Vector2 accumulatedPosition = Vector2.zero;

        //Loop through all entities in our neighbourhood.
        for (int entityIndex = 0; entityIndex < neighbourhood.Count; entityIndex++)
        {
            BaseEntity otherBaseEntity = neighbourhood[entityIndex];

            //Accumulate all the positions of entities in our neighbourhood.
            accumulatedPosition += (Vector2)otherBaseEntity.transform.position;
        }

        //We get an averaged position by dividing our accumulatedPosition by the number of entities in the neighbourhood.
        Vector2 averagedPosition = (accumulatedPosition / neighbourhood.Count);

        //We get our cohesion force by calling Seek with the averaged position.
        Vector2 cohesionForce = Seek(accumulatedPosition);

        return cohesionForce;
    }
}