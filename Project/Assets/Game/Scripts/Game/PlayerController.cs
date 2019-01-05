using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController
{
    [Header("Sounds")]
    [SerializeField] private Sound3D jumpSound;
    [SerializeField] private Sound3D landSound;

    [SerializeField] private float footstepRate;
    [SerializeField] private Sound3D[] footstepSounds;

    private float footstepProgress = 0;

    protected override void Update()
    {
        RotateView();

        if (m_IsGrounded && CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump)
        {
            m_Jump = true;
            jumpSound.Play();
        }


        if (m_IsGrounded)
        {
            float velocity = m_RigidBody.velocity.magnitude;

            //He andado una unidad de Unity
            if (footstepProgress >= footstepRate)
            {
                Sound3D sound = footstepSounds[Random.Range(0, footstepSounds.Length)];

                sound.Pitch = Mathf.Clamp(velocity / movementSettings.ForwardSpeed, 0.5f, 1.5f);
                sound.Pitch += Random.Range(-0.1f, 0.1f);                //Variación aleatoria
                sound.Play();
                footstepProgress = 0;
            }
        }
    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (m_IsGrounded)      
            footstepProgress += m_RigidBody.velocity.magnitude;
        
    }


    /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    protected override void GroundCheck()
    {
        m_PreviouslyGrounded = m_IsGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                               ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            m_IsGrounded = true;
            m_GroundContactNormal = hitInfo.normal;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundContactNormal = Vector3.up;
        }
        if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
        {
            m_Jumping = false;
            landSound.Play();
        }
    }

}
