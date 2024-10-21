using Fusion;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public float MaxHealth = 100f;
    public float ImmortalDurationAfterSpawn = 2f;
    public GameObject ImmortalityIndicator;
    public GameObject HitEffectPrefab;

    public bool IsAlive => CurrentHealth > 0f;
    public bool IsImmortal => _immortalTimer.ExpiredOrNotRunning(Runner) == false;

    [Networked]
    public float CurrentHealth { get; private set; }

    [Networked]
    private int _hitCount { get; set; }
    [Networked]
    private Vector3 _lastHitPosition { get; set; }
    [Networked]
    private Vector3 _lastHitDirection { get; set; }
    [Networked]
    private TickTimer _immortalTimer { get; set; }

    private int _visibleHitCount;


    public bool ApplyDamage(PlayerRef instigator, float damage, Vector3 position, Vector3 direction, bool isCritical)
    {
        if (CurrentHealth <= 0f)
            return false;


        CurrentHealth -= damage;

        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;

        }

     
        _lastHitPosition = position - transform.position;
        _lastHitDirection = -direction;

        _hitCount++;

        return true;
    }

    public bool AddHealth(float health)
    {
        if (CurrentHealth <= 0f)
            return false;
        if (CurrentHealth >= MaxHealth)
            return false;

        CurrentHealth = Mathf.Min(CurrentHealth + health, MaxHealth);

        if (HasInputAuthority && Runner.IsForward)
        {

        }

        return true;
    }

    public void StopImmortality()
    {
        _immortalTimer = default;
    }

    public override void Spawned()
    {


        if (HasStateAuthority)
        {
            CurrentHealth = MaxHealth;

            _immortalTimer = TickTimer.CreateFromSeconds(Runner, ImmortalDurationAfterSpawn);
        }

        _visibleHitCount = _hitCount;
    }

    public override void Render()
    {
        if (_visibleHitCount < _hitCount)
        {
            // Network hit counter changed in FUN, play damage effect.
            PlayDamageEffect();
        }

        ImmortalityIndicator.SetActive(IsImmortal);

        // Sync network hit counter with local.
        _visibleHitCount = _hitCount;
    }

    private void PlayDamageEffect()
    {
        if (HitEffectPrefab != null)
        {
            var hitPosition = transform.position + _lastHitPosition;
            var hitRotation = Quaternion.LookRotation(_lastHitDirection);

            Instantiate(HitEffectPrefab, hitPosition, hitRotation);
        }
    }
}

