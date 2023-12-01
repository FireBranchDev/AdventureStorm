using AdventureStorm.Tools;
using System.Collections;
using UnityEngine;

namespace AdventureStorm.Gameplay.Player
{
    public class PlayerController : Controller, IDamageable, IHealable
    {
        public override float MovementSpeed => 3.75f;

        public override float AttackDistance => 2f;

        public override float AttackDamage => 1.75f;

        public float DodgeDistance => 2f;
        public float DodgeHeight => 0.9f;

        public float TimeToRechargeDodgeStamina => 1.5f;

        public float MaxDodgeStamina => 6f;
        public float DodgeStamina { get; private set; }

        public float JumpHeight => 1.8f;

        public float TimeToReachJumpHeight => 0.25f;

        public float JumpInAirTime => 0.4f;

        public float DelayAfterDodge => 0.15f;

        private Coroutine _dodge;

        private Coroutine _attack;

        public bool WasKeyPickedUp { get; private set; }

        private Coroutine _jump;

        public override float MaxHealth => 10f;

        protected override void Start()
        {
            base.Start();
            Debug.Log("PlayerController starting...");
            DodgeStamina = MaxDodgeStamina;
            StartCoroutine(RechargeDodgeStamina());
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(SceneHelper.LoadSceneCoroutine(SceneHelper.MainMenu));
            }

            if (IsAlive)
            {
                float hAxis = Input.GetAxis("Horizontal");
                FlipSprite(hAxis);

                // Attacking.
                if (Input.GetMouseButtonDown(0) && _attack == null)
                {
                    _attack = StartCoroutine(AttackCoroutine());
                }
                // Dodging left.
                else if (Input.GetKeyDown(KeyCode.Q)
                    && DodgeStamina > 0f
                    && _dodge == null
                    && _jump == null)
                {
                    _dodge = StartCoroutine(DodgeCoroutine(false));
                }
                // Dodging to the right.
                else if (Input.GetKeyDown(KeyCode.E)
                    && DodgeStamina > 0f
                    && _dodge == null
                    && _jump == null)
                {
                    _dodge = StartCoroutine(DodgeCoroutine(true));
                }
                // Jumping.
                else if (Input.GetKeyDown(KeyCode.Space)
                    && _jump == null
                    && _dodge == null)
                {
                    _jump = StartCoroutine(JumpCoroutine());
                }
                // Walking.
                else if (hAxis != 0f
                       && AnimatorManager.DidAnimationFinish(PlayerAnimation.Hurt)
                       && _attack == null
                       && _dodge == null
                       && _jump == null)
                {
                    AnimatorManager.ChangeAnimationState(PlayerAnimation.Walking);
                    AnimatorManager.ReplayAnimation(PlayerAnimation.Walking);

                    Vector3 position = new()
                    {
                        x = MovementSpeed * hAxis * Time.deltaTime
                    };
                    transform.Translate(position);
                }
                // Idle.
                else if (_attack == null
                    && _dodge == null
                    && _jump == null
                    && hAxis == 0f)
                {
                    AnimatorManager.ChangeAnimationState(PlayerAnimation.Idle);

                    if (AnimatorManager.DidAnimationFinish(PlayerAnimation.Idle))
                    {
                        AnimatorManager.ReplayAnimation(PlayerAnimation.Idle);
                    }
                }
            }
            else
            {
                StartCoroutine(DeathCoroutine());
            }
        }

        private void FlipSprite(float direction)
        {
            Vector3 localScale = transform.localScale;

            if (localScale.x < 0f && direction > 0f)
            {
                localScale.x = Mathf.Abs(localScale.x);
            }
            else if (localScale.x > 0f && direction < 0f)
            {
                localScale.x = -localScale.x;
            }

            transform.localScale = localScale;
        }

        public void Damage(float damage)
        {
            if (!IsAlive) return;

            AnimatorManager.ChangeAnimationState("Hurt");
            _health -= damage;

            if (!IsAlive)
            {
                AnimatorManager.ChangeAnimationState("Death");
            }
        }

        public void Damage(int damage)
        {
            if (!IsAlive) return;

            AnimatorManager.ChangeAnimationState("Hurt");
            _health -= damage;

            if (!IsAlive)
            {
                AnimatorManager.ChangeAnimationState("Death");
            }
        }

        private IEnumerator DodgeCoroutine(bool dodgingRight)
        {
            AnimatorManager.ChangeAnimationState(PlayerAnimation.Dodge);
            float maxHeight = transform.position.y + DodgeHeight;
            float original = transform.position.y;
            Vector3 position = Vector3.zero;

            if (dodgingRight)
            {
                position.x = DodgeDistance;
            }
            else
            {
                position.x = -DodgeDistance;
            }

            if (transform.position.y < maxHeight)
            {
                position.y += DodgeHeight;
                transform.Translate(position);
            }

            yield return new WaitForSeconds(0.2f);

            position = transform.position;
            position.y = original;
            transform.position = position;

            DodgeStamina--;

            yield return new WaitForSeconds(DelayAfterDodge);

            _dodge = null;
        }

        private IEnumerator RechargeDodgeStamina()
        {
            while (true)
            {
                if (_dodge == null)
                {
                    yield return new WaitForSeconds(TimeToRechargeDodgeStamina);
                    if (_dodge == null)
                    {
                        if (DodgeStamina < MaxDodgeStamina)
                        {
                            DodgeStamina++;
                        }
                    }
                }

                yield return null;
            }
        }

        private IEnumerator AttackCoroutine()
        {
            AnimatorManager.ChangeAnimationState(PlayerAnimation.Attacking);

            if (AnimatorManager.DidAnimationFinish(PlayerAnimation.Attacking))
            {
                AnimatorManager.ReplayAnimation(PlayerAnimation.Attacking);
            }

            while (!AnimatorManager.DidAnimationFinish(PlayerAnimation.Attacking))
            {
                yield return null;
            }

            LayerMask layerMask = LayerMask.GetMask("Enemy");
            Vector2 direction = transform.localScale.x > 0f ? Vector2.right : Vector2.left;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, AttackDistance, layerMask);

            if (hit.collider != null)
            {
                IDamageable enemy = hit.collider.GetComponent<IDamageable>();
                if (enemy != null)
                {
                    enemy.Damage(AttackDamage);
                }
            }

            _attack = null;
        }

        public void Heal(float health)
        {
            if (Health + health > MaxHealth)
            {
                _health = MaxHealth;
            }
            else
            {
                _health += health;
            }
        }

        public void Heal(int health)
        {
            if (Health + health > MaxHealth)
            {
                _health = MaxHealth;
            }
            else
            {
                _health += health;
            }
        }

        public void PickupKey()
        {
            WasKeyPickedUp = true;
        }

        private IEnumerator DeathCoroutine()
        {
            while (!AnimatorManager.DidAnimationFinish(PlayerAnimation.Death))
            {
                yield return null;
            }

            Destroy(gameObject);

            StartCoroutine(SceneHelper.LoadSceneCoroutine(SceneHelper.RestartLevel));
        }

        private IEnumerator JumpCoroutine()
        {
            AnimatorManager.ChangeAnimationState(PlayerAnimation.Jump);

            if (AnimatorManager.DidAnimationFinish(PlayerAnimation.Jump))
            {
                AnimatorManager.ReplayAnimation(PlayerAnimation.Jump);
            }

            Vector3 originalPosition = transform.position;

            Vector3 maxPosition = transform.position + new Vector3 { y = JumpHeight };

            Vector3 direction = maxPosition - transform.position;
            direction.x = 0;
            direction.z = 0;

            float distance = direction.magnitude;

            while (transform.position.y <= maxPosition.y)
            {
                transform.Translate(direction * Time.deltaTime * (distance / TimeToReachJumpHeight));
                yield return null;
            }

            yield return new WaitForSeconds(JumpInAirTime);

            transform.position = originalPosition;

            _jump = null;
        }
    }
}