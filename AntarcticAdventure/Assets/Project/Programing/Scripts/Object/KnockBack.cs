using DG.Tweening;
using UnityEngine;

public sealed class KnockBack : Interactable{
	public float knockDistance = 1f;
	public float knockDuration = 1f;
	public float jumpPower     = 1f;
	public int   jumpAmount    = 1;
	
	public override void OnPlayerApprochead(){
	}

	protected override void OnInteract(Player player){
		KnockPlayerBack(player);
	}

	private void KnockPlayerBack(Player player){
		player.IsKnocked = true;
		
		// Side Knock
		var curve = player.Curve;
		var point = curve.InterpolateByDistance(player.travelledDst);
		var random = Random.Range(-1, 2);
		var newOffset = Mathf.Clamp(point.x + player.offset + random * knockDistance, point.x - player.Setting.maxOffset, point.x + player.Setting.maxOffset);
		var dropPoint = new Vector3(newOffset, 0, point.z);
		
		player.transform.DOJump(dropPoint, jumpPower, jumpAmount, knockDuration)
			.OnPlay(() => {
				if (random == -1)
					Debug.Log("Player knocked to left");
				else 
					Debug.Log("Player knocked to right");
			})
			.OnComplete(() => {
				player.offset = newOffset;
				player.IsKnocked = false;
			});
	}
}