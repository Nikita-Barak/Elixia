using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    
    private bool canGrow = true;
    private bool canShrink;

    private float blackholeTimer;
    
    
    bool canCreateHotKey = true;

    private bool cloneAttackReleased;
    private bool playerCanDissapear = true;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;
    
    public List<Transform> targets = new List<Transform>();
    private List<GameObject> hotKeys = new List<GameObject>();

    public bool playerCanExitState {get; private set;}


    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks,
        float _cloneAttackCooldown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackholeDuration;
    }
    
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer <= 0)
        {
            blackholeTimer = Mathf.Infinity;
            
            if (targets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackHole();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }
        
        CloneAttackLogic();
        
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), Time.deltaTime * growSpeed);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), Time.deltaTime * shrinkSpeed);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
        {
            return;
        }
        
        DestroyAllHotKeys();
        cloneAttackReleased = true;
        canCreateHotKey = false;

        if (playerCanDissapear)
        {
            playerCanExitState = false;
            PlayerManager.instance.player.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCooldown;
            
            int randomIndex = Random.Range(0, targets.Count);
            float xOffset;

            if (Random.Range(0f, 1f) > .5f)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }
            
            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset,0));
            
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHole", 1f);
            }
        }
    }

    private void FinishBlackHole()
    {
        playerCanExitState = true;
       // PlayerManager.instance.player.ExitBlackhole();
        cloneAttackReleased = false;
        canShrink = true;
        DestroyAllHotKeys();
    }

    private void DestroyAllHotKeys()
    {
        if (hotKeys.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < hotKeys.Count; i++)
        {
            Destroy(hotKeys[i]); 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);
    private void CreateHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0 || !canCreateHotKey)
        {
            return;
        }
        
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0,2), Quaternion.identity);
        hotKeys.Add(newHotKey);
        
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        BlackHoleHotKeyController newHoleHotKeyScript = newHotKey.GetComponent<BlackHoleHotKeyController>();
            
        newHoleHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
