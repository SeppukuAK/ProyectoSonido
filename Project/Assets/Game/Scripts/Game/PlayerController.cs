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

        //Jumping
        if (m_IsGrounded && CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump)
        {
            m_Jump = true;
            jumpSound.Play();
        }

        //Landing
        else if (!m_PreviouslyGrounded && m_IsGrounded)
            landSound.Play();

        //Walking
        else if (m_IsGrounded)
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

        CheckInteraction();
    }

    private void CheckInteraction()
    {
        if (CrossPlatformInputManager.GetButtonDown("Interact"))
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(cam.transform.position, transform.TransformDirection(Vector3.forward), out hit, 3f))
            {
                if (hit.collider.GetComponent<Interactable>())
                {
                    hit.collider.GetComponent<Interactable>().Interact();
                    Debug.Log("Did Hit");
                }
            }
        }

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (m_IsGrounded)
        {
            if (Running)
                footstepProgress += m_RigidBody.velocity.magnitude * 0.75f;
            else
                footstepProgress += m_RigidBody.velocity.magnitude;


        }


    }

}
