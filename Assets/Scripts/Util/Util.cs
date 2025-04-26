using UnityEngine;


[System.Serializable]
public struct IntMinMax
{
    public int min;
    public int max;

    public static IntMinMax GetValue(int min, int max)
    {
        var value = new IntMinMax();
        value.min = min;
        value.max = max;
        return value;
    }

    public int GetRendomValue()
    {
        return UnityEngine.Random.Range(min, max);
    }
}

[System.Serializable]
public struct FloatMinMax
{
    public float min;
    public float max;

    public void SetValue(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public static FloatMinMax GetValue(float min, float max)
    {
        var value = new FloatMinMax();
        value.min = min;
        value.max = max;
        return value;
    }

    public float GetRendomValue()
    {
        return UnityEngine.Random.Range(min, max);
    }
}




public static class PlayerAnimationHashCode
{
    public static readonly int hashSpeed = Animator.StringToHash("Speed");
    public static readonly int hashMove = Animator.StringToHash("walk");
    public static readonly int hashAttack = Animator.StringToHash("attack");
    //public static readonly int hashInteract = Animator.StringToHash("interact");
    public static readonly int hashCanMove = Animator.StringToHash("canMove");
    public static readonly int hashIsFarming = Animator.StringToHash("IsFarming");
    public static readonly int hashIsPickingUp = Animator.StringToHash("IsPickingUp");
    public static readonly int hashIsAxing = Animator.StringToHash("IsAxing");

    public static readonly int hashDeath = Animator.StringToHash("Death");
    public static readonly int hashIsDeath = Animator.StringToHash("IsDeath");
}


public static class MonsterAnimationHashCode
{
    public static readonly int hashMove = Animator.StringToHash("MoveSpeed");
    public static readonly int hashAttack = Animator.StringToHash("Attack");
    public static readonly int hashDeath = Animator.StringToHash("Death");
    public static readonly int hashIsDeath = Animator.StringToHash("IsDeath");
    public static readonly int hashHit = Animator.StringToHash("Hit");
    public static readonly int hashHitFromX = Animator.StringToHash("HitFromX");

}