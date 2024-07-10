using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    
    public static SoundManager instance { get; set; }

    public AudioSource dragItemSound;
    public AudioSource inventoryOpenSound;
    public AudioSource PickUpSound;
    public AudioSource pickupSwordSound;
    public AudioSource pickupAxePickAxeSound;
    public AudioSource pickupBottleSound;
    public AudioSource coinSound;
    public AudioSource AttackSound;
    public AudioSource AttackSound2;

    public AudioClip[] choppingTree;
    private AudioSource audioSource;

    public AudioClip[] miningOre;
/*    private AudioSource miningaudioSource;
*/
    public AudioClip[] oreBreaks;
    /*    private AudioSource orebreaksAuidioSource;
    */
    public AudioClip[] miningMetallicOre;

    public AudioSource fallingTree;

    public AudioClip[] projectileWarmup_rock;
    public AudioClip[] projectileExplotion_rock;


    private int currentIndex = 0;
    private int curIndx = 0;
    private int cIndx = 0;
    private int MetallicIndx = 0;
    private int projectileRock = 0;
    private int projectileRockExplotionIndx = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public void PlayDragSound()
    {
        if (!dragItemSound.isPlaying)
        {
            dragItemSound.Play();
        }
    }
    public void PlayInventoryOpenSound()
    {
        if (!inventoryOpenSound.isPlaying)
        {
            inventoryOpenSound.Play();
        }
    } 
    public void PlayDropSound()
    {
        if (!PickUpSound.isPlaying)
        {
            PickUpSound.Play();
        }
    }
    public void PlayPickupSound()
    {
        if (PickUpSound.isPlaying ||  !PickUpSound.isPlaying)
        {
            PickUpSound.Play();
        }
    }
    public void PlayChoppingTree() 
    {
        if (choppingTree.Length >0 )
        {
            audioSource.volume = 1;
            audioSource.clip = choppingTree[currentIndex];
            audioSource.Play();
            currentIndex = UnityEngine.Random.Range(0, 4);
        }
    }
    public void PlayFallingTree()
    {
        fallingTree.Play();
        /*if (fallingTree.Length >= 0)
        {
            fallingTreeAudioSource.volume = 1f;
            fallingTreeAudioSource.clip = fallingTree[curIndx];
            fallingTreeAudioSource.Play();
            curIndx = UnityEngine.Random.Range(0, 1);
        }*/
        
    }
    public void PlayAttackSound()
    {
        
        AttackSound.Play();
    }
    public void PlayStoneMining()
    {
        if (miningOre.Length > 0)
        { 
            audioSource.volume = 1;
            audioSource.clip = miningOre[curIndx];
            audioSource.Play();
        curIndx = UnityEngine.Random.Range(0, 3);
        }
    }
    public void PlayOreBreak()
    {
        if(oreBreaks.Length > 0)
        {
            audioSource.volume = 1;
            audioSource.clip = oreBreaks[cIndx];
            audioSource.Play();
            cIndx = UnityEngine.Random.Range(0, 3);
        }
    }

    public void PlayMetallicOreBreak()
    {
        if (miningMetallicOre.Length>0)
        {
            audioSource.clip = miningMetallicOre[MetallicIndx];
            audioSource.Play();
            MetallicIndx=UnityEngine.Random.Range(0, 3);
        }

    }

    //Projectile Explotaions

    public void PlayProjectileRock()
    {
        if (projectileWarmup_rock.Length > 0)
        {
            audioSource.volume= 2;
            audioSource.clip = projectileWarmup_rock[projectileRock];
            audioSource.Play();
            projectileRock = UnityEngine.Random.Range(0, 3);
        }

    }public void PlayProjectileRockExplotion()
    {
        if (projectileExplotion_rock.Length > 0)
        {
            audioSource.volume= 2;
            audioSource.clip = projectileExplotion_rock[projectileRockExplotionIndx];
            audioSource.Play();
            projectileRockExplotionIndx = UnityEngine.Random.Range(0, 3);
        }

    }
}
