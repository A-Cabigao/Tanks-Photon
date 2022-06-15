using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QLE
{
    public abstract class PowerUp : MonoBehaviour
    {
        [SerializeField] float _minRecoverValue;
        [SerializeField] float _maxRecoverValue;

        bool isActivated = false;

        public float minRecover => _minRecoverValue;
        public float maxRecover => _maxRecoverValue;

        AudioSource audioSource;

        public virtual void ActivatePowerUp(Transform playerTransform)
        {
            PowerUpManager.OnActivatePowerUp?.Invoke();
        }

        public virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!isActivated)
                if(collision.CompareTag("Player"))
                {
                    isActivated = true;
                    ActivatePowerUp(collision.transform.root);
                    DestroyOnPickUp();
                }
        }

        void DestroyOnPickUp()
        {
            audioSource.Play();
            LeanTween.alpha(gameObject, 0f, 0.65f);
            Destroy(gameObject, 1f);
        }
    }
}
