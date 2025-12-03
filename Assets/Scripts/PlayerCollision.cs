using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField] public enum CollisionX { None, Left, Middle, right }
[SerializeField] public enum CollisionY { None, Up, Middle, Down, LowDown }
[SerializeField] public enum CollisionZ { None, Forward, Middle, BackWard }

public class PlayerCollision : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private CollisionX _collisionX;
    [SerializeField] private CollisionY _collisionY;
    [SerializeField] private CollisionZ _collisionZ;

    public CollisionZ CollisionZ { get => _collisionZ; set => _collisionZ = value; }
    public CollisionY CollisionY { get => _collisionY; set => _collisionY = value; }
    public CollisionX CollisionX { get => _collisionX; set => _collisionX = value; }

    private void Awake()
    {
        playerController = gameObject.GetComponentInParent<PlayerController>();
    }

    public void OnCharacterCollision(Collider collider)
    {
        _collisionX = GetCollisionX(collider);
        _collisionY = GetCollisionY(collider);
        _collisionZ = GetCollisionZ(collider);
        SetAnimationByCollision(collider);
    }

    private CollisionX GetCollisionX(Collider collider)
    {
        Bounds characterControllerBounds = playerController.myCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minX = Mathf.Max(colliderBounds.min.x, characterControllerBounds.min.x);
        float maxX = Mathf.Min(colliderBounds.max.x, characterControllerBounds.max.x);
        float averageX = (minX + maxX) / 2 - colliderBounds.min.x;
        CollisionX colX;

        if (averageX > colliderBounds.size.x - 0.33f)
        {
            colX = CollisionX.right;
        }
        else if (averageX < 0.33f)
        {
            colX = CollisionX.Left;
        }
        else
        {
            colX = CollisionX.Middle;
        }
        return colX;
    }

    private CollisionY GetCollisionY(Collider collider)
    {
        Bounds characterControllerBounds = playerController.myCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minY = Mathf.Max(colliderBounds.min.y, characterControllerBounds.min.y);
        float maxY = Mathf.Min(colliderBounds.max.y, characterControllerBounds.max.y);
        float averageY = (minY + maxY) / 2 - colliderBounds.min.y;
        CollisionY colY;

        if (averageY > colliderBounds.size.y - 0.33f)
        {
            colY = CollisionY.Up;
        }
        else if (averageY < 0.17f)
        {
            colY = CollisionY.LowDown;
        }
        else if (averageY < 0.33f)
        {
            colY = CollisionY.Down;
        }
        else
        {
            colY = CollisionY.Middle;
        }
        return colY;
    }

    private CollisionZ GetCollisionZ(Collider collider)
    {
        Bounds characterControllerBounds = playerController.myCharacterController.bounds;
        Bounds colliderBounds = collider.bounds;
        float minY = Mathf.Max(colliderBounds.min.z, characterControllerBounds.min.z);
        float maxY = Mathf.Min(colliderBounds.max.z, characterControllerBounds.max.z);
        float averageZ = (minY + maxY) / 2 - colliderBounds.min.z;
        CollisionZ colZ;

        if (averageZ > colliderBounds.size.z - 0.33f)
        {
            colZ = CollisionZ.Forward;
        }
        else if (averageZ < 0.33f)
        {
            colZ = CollisionZ.BackWard;
        }
        else
        {
            colZ = CollisionZ.Middle;
        }
        return colZ;
    }
    private void SetAnimationByCollision(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Ramp"))
        {
            if (_collisionX == CollisionX.Middle &&
           _collisionY == CollisionY.Middle && _collisionZ == CollisionZ.BackWard)
            {
                playerController.SetPlayerAnimator(playerController.IdDeathBounce, false);
            }
        }
      
        if (collider.gameObject.CompareTag("TrainOn"))
        {
            playerController.SetPlayerAnimator(playerController.IdDeathMovingTrain, false);
        }
        if (!collider.gameObject.CompareTag("Ramp"))
        {
            if (_collisionX == CollisionX.Middle && _collisionY == CollisionY.Up && _collisionZ == CollisionZ.BackWard && !collider.gameObject.CompareTag("TrainOn"))
            {
                playerController.SetPlayerAnimator(playerController.IdDeahthUpper, false);
            }
        }

        if (_collisionX == CollisionX.right && _collisionY == CollisionY.Middle)
        {
            playerController.SetPlayerAnimator(playerController.IdStumbleCornerRight, false);
        }
        if (_collisionX == CollisionX.right && _collisionY == CollisionY.Middle)
        {
            playerController.SetPlayerAnimatorWithLayer(playerController.IdStumbleCornerRight);
        }
        if (_collisionX == CollisionX.Left && _collisionY == CollisionY.Middle)
        {
            playerController.SetPlayerAnimatorWithLayer(playerController.IdStumbleCornerLeft);
        }
        if (_collisionX == CollisionX.Middle && _collisionY == CollisionY.LowDown && _collisionZ == CollisionZ.BackWard)
        {
            playerController.SetPlayerAnimator(playerController.IdStumbleLow, false);
        }
    }
}
