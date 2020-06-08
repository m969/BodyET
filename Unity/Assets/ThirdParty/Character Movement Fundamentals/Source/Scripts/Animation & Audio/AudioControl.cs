using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This script handles and plays audio cues like footsteps, jump and land audio clips based on character movement speed and events; 
	public class AudioControl : MonoBehaviour {

		//References to components;
		Controller controller;
		Animator animator;
		Mover mover;
		Transform tr;
		public AudioSource audioSource;

		//Whether footsteps will be based on the currently playing animation or calculated based on walked distance (see further below);
		public bool useAnimationBasedFootsteps = true;

		//Footsteps will be played every time the traveled distance reaches this value (if 'useAnimationBasedFootsteps' is set to 'true');
		public float footstepDistance = 0.2f;
		float currentFootstepDistance = 0f;

		private float currentFootStepValue = 0f;

		//Volume of all audio clips;
		[Range(0f, 1f)]
		public float audioClipVolume = 0.1f;

		//Range of random volume deviation used for footsteps;
		//Footstep audio clips will be played at different volumes for a more "natural sounding" result;
		public float RelativeRandomizedVolumeRange = 0.2f;

		//Audio clips;
		public AudioClip[] footStepClips;
		public AudioClip jumpClip;
		public AudioClip landClip;

		//Setup;
		void Start () {
			//Get component references;
			controller = GetComponent<Controller>();
			animator = GetComponentInChildren<Animator>();
			mover = GetComponent<Mover>();
			tr = transform;

			//Connecting events to controller events;
			controller.OnLand += OnLand;
			controller.OnJump += OnJump;

			if(!animator)
				useAnimationBasedFootsteps = false;
		}
		
		//Update;
		void Update () {

			//Get controller velocity;
			Vector3 _velocity = controller.GetVelocity();

			//Calculate horizontal velocity;
			Vector3 _horizontalVelocity = VectorMath.RemoveDotVector(_velocity, tr.up);

			FootStepUpdate(_horizontalVelocity.magnitude);
		}

		void FootStepUpdate(float _movementSpeed)
		{
			float _speedThreshold = 0.05f;

			if(useAnimationBasedFootsteps)
			{
				//Get current foot step value from animator;
				float _newFootStepValue = animator.GetFloat("FootStep");

				//Play a foot step audio clip whenever the foot step value changes its sign;
				if((currentFootStepValue <= 0f && _newFootStepValue > 0f) || (currentFootStepValue >= 0f && _newFootStepValue < 0f))
				{
					//Only play footstep sound if mover is grounded and movement speed is above the threshold;
					if(mover.IsGrounded() && _movementSpeed > _speedThreshold)
						PlayFootstepSound(_movementSpeed);
				}
				currentFootStepValue = _newFootStepValue;
			}
			else
			{
				currentFootstepDistance += Time.deltaTime * _movementSpeed;

				//Play foot step audio clip if a certain distance has been traveled;
				if(currentFootstepDistance > footstepDistance)
				{
					//Only play footstep sound if mover is grounded and movement speed is above the threshold;
					if(mover.IsGrounded() && _movementSpeed > _speedThreshold)
						PlayFootstepSound(_movementSpeed);
					currentFootstepDistance = 0f;
				}
			}
		}

		void PlayFootstepSound(float _movementSpeed)
		{
			int _footStepClipIndex = Random.Range(0, footStepClips.Length);
			audioSource.PlayOneShot(footStepClips[_footStepClipIndex], audioClipVolume + audioClipVolume * Random.Range(-RelativeRandomizedVolumeRange, RelativeRandomizedVolumeRange));
		}

		void OnLand(Vector3 _v)
		{
			//Play land audio clip;
			audioSource.PlayOneShot(landClip, audioClipVolume);
		}

		void OnJump(Vector3 _v)
		{
			//Play jump audio clip;
			audioSource.PlayOneShot(jumpClip, audioClipVolume);
		}
	}
}

