using UnityEngine;

namespace QLE
{
    /// <summary>
    /// Shows/Hides the sprite outline material on the sprite renderer
    /// </summary>
    public class SpriteOutline : MonoBehaviour
	{
		[SerializeField] bool hideOutlineOnDisable = true;
		/// <summary>
		/// No outline material for this object
		/// </summary>
		[SerializeField] Material defaultSpriteMaterial;
		[SerializeField] Material spriteOutlineMaterial;
		// must contain the Sprite Outline material / shader
		SpriteRenderer spriteRenderer;

		public bool IsOutlineShowing {
			get =>spriteRenderer.material == spriteOutlineMaterial;
			set {
				spriteRenderer.material = value ? spriteOutlineMaterial : defaultSpriteMaterial;
			}
		}

        void Awake()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			IsOutlineShowing = false;
		}
        void OnDisable()
        {
			if (hideOutlineOnDisable) IsOutlineShowing = false;
		}
	}
}