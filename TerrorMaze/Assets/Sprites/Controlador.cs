using UnityEngine;
using System.Collections.Generic;

public class Controlador : MonoBehaviour {
	public Vector2 MoveDire = Vector2.zero;
	public float movementSpeed = 50F;
	public Rigidbody2D rb = null;
	public Vector2 move = Vector2.zero;
	public Animator Animator = null;
	public SpriteRenderer Sprite_Renderer = null;
	public float VELOCIDADE_TOTAL = 5F;

	void Movimento() {
		rb.velocity = ((MoveDire * movementSpeed) * VELOCIDADE_TOTAL);
	}

	public void FixedUpdate() {
		Inputs_leitura();
		Movimento();
		Animação();
	}

	public void Animação() {
		if(MoveDire != Vector2.zero) {
			Animator.SetFloat("Horizontal", MoveDire.x);
			Animator.SetFloat("Vertical", MoveDire.y);
		//	Sprite_Renderer.flipX = (MoveDire.x > 0.01F);
		}
		Animator.SetFloat("Velocidade", movementSpeed);
		//Sprite_Renderer.flipX = (MoveDire.x > 0.01F);
	}

	public void Inputs_leitura() {
		MoveDire = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		movementSpeed = Mathf.Clamp(MoveDire.magnitude, 0F, 1F);
		MoveDire.Normalize();
	}
}
