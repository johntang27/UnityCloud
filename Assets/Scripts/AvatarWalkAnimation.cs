using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarWalkAnimation : MonoBehaviour {

    public Image hairSprite;
    public Image eyeSprite;
    public Animator poseAnimator;
    public Animator outfitAnimator;

	// Use this for initialization
	void Start () {
        hairSprite.sprite = Resources.Load("AnimationController/female_hair_" + PlayerData.Instance.playerAvatar.hairSprite.ToString(), typeof(Sprite)) as Sprite;
        poseAnimator.runtimeAnimatorController = Resources.Load("AnimationController/Pose", typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        outfitAnimator.runtimeAnimatorController = Resources.Load("AnimationController/Outfit_Female_" + PlayerData.Instance.playerAvatar.outfitSprite.ToString(), typeof(RuntimeAnimatorController)) as RuntimeAnimatorController;
        outfitAnimator.GetComponent<Image>().sprite = Resources.Load("AnimationController/female_athletic_" + PlayerData.Instance.playerAvatar.outfitSprite.ToString(), typeof(Sprite)) as Sprite;

        hairSprite.color = PlayerData.Instance.playerAvatar.hairColor;
        poseAnimator.GetComponent<Image>().color = PlayerData.Instance.playerAvatar.bodyColor;
        outfitAnimator.GetComponent<Image>().color = PlayerData.Instance.playerAvatar.outfitColor;

        //poseAnimator.Play("Idle");
        //outfitAnimator.Play("Idle");
    }
	
	
    public void PlayAnimation()
    {
        poseAnimator.Play("Pose_Walk_Female");
        outfitAnimator.Play("Outfit_Walk_Female_" + PlayerData.Instance.playerAvatar.outfitSprite.ToString());
    }

    public void StopAnimation()
    {
        poseAnimator.Play("Idle");
        outfitAnimator.Play("Idle");
    }
}
